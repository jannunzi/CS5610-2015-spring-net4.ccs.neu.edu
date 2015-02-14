<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" %>

<%@ Import Namespace="edu.neu.ccis.rasala" %>
<%@ Import Namespace="System.Collections.Generic" %>

<script runat="server">

    public const string NoPattern = "No search pattern provided";

    long StartLine = 1;
    long MaxLines = 100;
    bool ShowLineNumbers = false;

    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SourceTools.LoadCSSandJavascript(this);
        }
    }
    

    protected void Statistics_Click(object sender, EventArgs e)
    {
        string tildeFilePath = TildeFilePathBox.Text;
        
        StatisticsResults.InnerHtml =
            LargeTextTools.TextFileStatistics(this.Server, tildeFilePath);
    }


    protected void GetLineConstraints()
    {
        try
        {
            StartLine = Convert.ToInt64(StartLineBox.Text);

            if (StartLine < 1)
                StartLine = 1;
        }
        catch
        {
            StartLine = 1;
        }

        try
        {
            MaxLines = Convert.ToInt64(MaxLinesBox.Text);

            if (MaxLines < 1)
                MaxLines = 1;
        }
        catch
        {
            MaxLines = 100;
        }

        StartLineBox.Text = StartLine + "";
        MaxLinesBox.Text = MaxLines + "";

        ShowLineNumbers = ShowLineNumbersBox.Checked;
    }


    protected void ListButton_Click(object sender, EventArgs e)
    {
        string tildeFilePath = TildeFilePathBox.Text;
        
        GetLineConstraints();

        ListResults.InnerHtml =
            LargeTextTools.ListLines
                (this.Server, tildeFilePath, StartLine, MaxLines, ShowLineNumbers);
    }
    

    protected void SearchButton_Click(object sender, EventArgs e)
    {
        string tildeFilePath = TildeFilePathBox.Text;

        GetLineConstraints();

        string pattern = PatternBox.Text;
        
        if (StringTools.IsTrivial(pattern))
        {
            ListResults.InnerHtml = HTML_Tools.pre(NoPattern);
            return;
        }

        bool isRegex = SearchType.SelectedValue == "Regex";
        bool ignoreCase = IgnoreCaseBox.Checked;
        bool ShowAllLines = ShowAllLinesBox.Checked;
        
        ListResults.InnerHtml =
            LargeTextTools.ListLinesFromSearch
            (this.Server, tildeFilePath, pattern, isRegex,
             ignoreCase, StartLine, MaxLines, ShowLineNumbers, ShowAllLines);        
    }

</script>

<!DOCTYPE html>

<html lang='en'>

<head id="head1" runat="server">

<meta charset="utf-8" />
<meta name="viewport" content="initial-scale=1, minimum-scale=1">

<title>TextView</title>

<style type="text/css" media="screen">

body {
    margin: 0;
    padding: 0;
}

.pad {
    padding: 10px;
}

</style>

</head>

<body>
<div class="pad">

<form id="form1" runat="server">

<h1>TextView</h1>
    
<h4><asp:HyperLink
        ID="HomeLink"
        NavigateUrl="~/Default.aspx"
        Target="_blank"
        runat="server">Home</asp:HyperLink>
            
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        
    Open New
    <asp:HyperLink
        ID="OpenNewLink"
        NavigateUrl="Default.aspx"
        Target="_blank"
        runat="server">TextView</asp:HyperLink>
</h4>
    
    
<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
    
    
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>

<i>

<p>
The TextView tool is designed for listing or searching
portions of a large text file that cannot be read into
memory all at once.  Of course, it may also be used to
study a small text file as well.  
</p>

<p>
The TextView tool has the following features:
</p>

<ul>
    <li>
    The user may list a consecutive set of lines in the
    file.
    </li>
    <li>
    The user may exhibit the results of a search within
    a consecutive set of lines in the file.
    </li>
</ul>

<p>
This tool therefore complements the regular search tools
because the results of a search may be viewed within a
multiline context if desired.
</p>
    
</i>

<hr />

<h4>Tilde Path to Text File</h4>

<p>
<asp:TextBox ID="TildeFilePathBox" runat="server" 
Width="700px" CssClass="fs120 mono"></asp:TextBox>
</p>

<div ID="StatisticsResults" runat="server"></div>

<asp:Button Text="Statistics" ID="Statistics" CssClass="fs120"
    BackColor="SkyBlue" OnClick="Statistics_Click" runat="server" />
    
<hr />

<h4>Simple Lists and Search Lists</h4>

<i>

<p>
Since this tool is intended for large files, it may be impossible
to list all lines at once.  Therefore, regardless of the size of
the file, we ask that the user specify:
</p>

<ul>
    <li>
    the start line number for processing
    </li>
    <li>
    the maximum number of lines to display
    </li>
</ul>

<p>
These line constraints are used for both simple lists of consecutive
lines and lists resulting from searches.
</p>

<p>
Result lists are always shown at the bottom of the page.
</p>

</i>

<hr />

<h4>Line Constraints</h4>

<p><b>
Start Line Number [default: 1]
</b></p>

<p>
<asp:TextBox ID="StartLineBox" runat="server" 
Width="300px" CssClass="fs120 mono"></asp:TextBox>
</p>

<p><b>
Maximum Lines to Display [default: 100]
</b></p>

<p>
<asp:TextBox ID="MaxLinesBox" runat="server" 
Width="300px" CssClass="fs120 mono"></asp:TextBox>
</p>

<br />
    
<div>
<asp:CheckBox Text="Show Line Numbers" ID="ShowLineNumbersBox"
    Checked="false" CssClass="fs120" runat="server" />
</div>

<hr />
    
<asp:Button Text="List Consecutive Lines" ID="ListButton" CssClass="fs120"
    BackColor="SkyBlue" OnClick="ListButton_Click" runat="server" />
    
<hr />
    
<h4>Search Pattern</h4>
    
<p>
<asp:TextBox ID="PatternBox" runat="server" 
Width="700px" CssClass="fs120 mono"></asp:TextBox>
</p>
    
<h4>Search Options</h4>
    
<asp:RadioButtonList ID="SearchType" CssClass="fs120" runat="server">
    <asp:ListItem Selected="True" Value="Plain">Plain Text Search</asp:ListItem>
    <asp:ListItem Value="Regex">Regular Expression Search</asp:ListItem>
</asp:RadioButtonList>
    
<br />
    
<div>
<asp:CheckBox Text="Ignore Case" ID="IgnoreCaseBox"
    Checked="true" CssClass="fs120" runat="server" />
</div>

<div>
<asp:CheckBox Text="Show All Lines" ID="ShowAllLinesBox"
    Checked="true" CssClass="fs120" runat="server" />
</div>

<hr />
    
<asp:Button Text="List Search Matches Within Consecutive Lines"
    ID="SearchButton" CssClass="fs120"
    BackColor="SkyBlue" OnClick="SearchButton_Click" runat="server" />
    
<hr />
    
<div id="ListResults" runat="server"></div>
    
</ContentTemplate>
</asp:UpdatePanel>
<!-- End UpdatePanelForDirectoryList -->
    
</form>

</div>
</body>
</html>
