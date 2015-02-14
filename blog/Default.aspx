<%@ Page Language="C#"
    AutoEventWireup="true" %>

<%@ Import Namespace="edu.neu.ccis.rasala" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>

<script runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {
        Blog blog = new Blog(this, BlogEntryURL.Text);

        int[][] list = blog.GetYearMonthCountList();

        int n = list.Length;

        if (!IsPostBack)
        {
            for (int i = (n - 1); i >= 0; i--)
            {
                ListItem listitem = new ListItem(FormatTriple(list[i]));
                BlogSelection.Items.Add(listitem);
            }
        }

        bool reverse = BlogSortCheckBox.Checked;
        int selection = BlogSelection.SelectedIndex;

        if (selection == 0)
        {
            BlogContent.Text = blog.GetBlog(reverse);
        }
        else if (selection == 1)
        {
            BlogContent.Text = blog.GetRecentBlog(reverse);
        }
        else
        {
            int index = n + 1 - selection;
            int[] triple = list[index];

            int year = triple[0];
            int month = triple[1];

            BlogContent.Text = blog.GetBlog(reverse, year, month);
        }
    }


    public static string FormatTriple(int[] triple)
    {
        return String.Format("&nbsp;&nbsp;{0:0000}-{1:00}: {2}", triple[0], triple[1], triple[2]);
    }
    
</script>

<!DOCTYPE html PUBLIC
    "-//W3C//DTD XHTML 1.0 Transitional//EN"
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<!-- Adapted from blog code by Peter Douglass. -->

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%=ConfigurationManager.AppSettings["blogtitle"]%></title>
    <link rel="stylesheet" type="text/css" href="../css/style.css" />
    <link rel="stylesheet" type="text/css" href="blog.css" />
    <link rel="stylesheet" type="text/css" media="screen" href="blog_screen.css" />
    <link rel="stylesheet" type="text/css" media="print" href="blog_print.css" />
</head>
<body>
<form id="form1" runat="server">

<div id="content">

<div class="page_content">

<p class="fs200 bold">
<%=ConfigurationManager.AppSettings["blogtitle"]%>
</p>

<asp:Label ID="BlogContent" runat="server" Text=""></asp:Label>

</div>

<div class="page_menu">

<p class=" bold">
<asp:CheckBox ID="BlogSortCheckBox" runat="server" 
    Text="&nbsp;&nbsp;Reverse"
    Checked="True"
    AutoPostBack="True"/>
</p>

<p class="bold">
Blogs:
</p>

<p class="bold">
<asp:RadioButtonList ID="BlogSelection" runat="server"
    RepeatColumns="1"
    AutoPostBack="True">
    <asp:ListItem runat="server" Text="&nbsp;&nbsp;All">
    </asp:ListItem>
    <asp:ListItem runat="server" Text="&nbsp;&nbsp;Recent" Selected="True">
    </asp:ListItem>
</asp:RadioButtonList>
</p>

</div>


</div>

<asp:Label ID="BlogEntryURL" runat="server"
    Text="../blog_entries" Visible="false">
</asp:Label>

</form>
</body>
</html>
