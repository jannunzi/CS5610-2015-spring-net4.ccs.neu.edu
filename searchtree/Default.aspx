<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" %>

<%@ Import Namespace="edu.neu.ccis.rasala" %>
<%@ Import Namespace="System.Collections.Generic" %>

<script runat="server">

    public const bool onlyPublic = true;

    public const string NoPattern = "No search pattern provided";
    

    void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SourceTools.LoadCSSandJavascript(this);

            InitRootData();
            InitDirectoryList();
        }
    }
    

    void InitRootData()
    {
        RootPath.Text = FileTools.GetRoot(this);
    }


    void InitDirectoryList()
    {
        string rootPath = RootPath.Text;

        List<string> directoryList =
            SourceTools.MakeDirectoryList(rootPath, onlyPublic);

        List<string> tildeDirectoryList =
            FileTools.GetTildePaths(rootPath, directoryList);

        tildeDirectoryList.Insert(0, "");
        
        DirectoryList.DataSource = tildeDirectoryList;
        DirectoryList.DataBind();
    }


    string GetSelectedDirectory()
    {
        if (DirectoryList.Items.Count == 0)
            return "";
        else
            return DirectoryList.SelectedValue;
    }


    protected void SearchButton_Click(object sender, EventArgs e)
    {
        string pattern = PatternBox.Text;
        
        if (StringTools.IsTrivial(pattern))
        {
            SearchResults.Text = HTML_Tools.pre(NoPattern);
            return;
        }

        bool isRegex = SearchType.SelectedValue == "Regex";
        bool ignoreCase = IgnoreCaseBox.Checked;
        bool statistics = StatisticsBox.Checked;
        bool download = DownloadBox.Checked;

        string tildeDirectoryPath = GetSelectedDirectory();

        if (StringTools.IsTrivial(tildeDirectoryPath))
        {
            tildeDirectoryPath = "/";
        }

        SearchResults.Text =
            SearchTools.SearchTreeMarkup
                (this, tildeDirectoryPath, pattern,
                    isRegex, ignoreCase, statistics, download, onlyPublic);
    }

</script>

<!DOCTYPE html>

<html lang='en'>

<head id="head1" runat="server">
    <meta charset="utf-8" />
    <title>Search Tree Server</title>
</head>

<body>
    <form id="form1" runat="server">

    <h1>Search Tree Server</h1>
    
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
            runat="server">Search Tree Server</asp:HyperLink>
    </h4>
    
    
    <asp:Label
        ID="RootPath"
        runat="server"
        Visible="False" />
    

    <asp:ScriptManager ID="ScriptManagerForSource" runat="server">
    </asp:ScriptManager>
    
    
    <asp:UpdatePanel ID="UpdatePanelForDirectoryList" runat="server">
    <ContentTemplate>
    
    <h4>Search Pattern String</h4>
    
    <asp:TextBox ID="PatternBox" runat="server" 
    Width="700px" CssClass="fs120 mono"></asp:TextBox>
    
    
    <h4>Search Options</h4>
    
    <asp:RadioButtonList ID="SearchType" CssClass="fs120" runat="server">
        <asp:ListItem Selected="True" Value="Plain">Plain Text Search</asp:ListItem>
        <asp:ListItem Value="Regex">Regular Expression Search</asp:ListItem>
    </asp:RadioButtonList>
    
    <br />
    
    &nbsp;&nbsp;<asp:CheckBox Text="Ignore Case" ID="IgnoreCaseBox"
        Checked="true" CssClass="fs120" runat="server" />
    
    <br />
    
    &nbsp;&nbsp;<asp:CheckBox Text="Show File Statistics" ID="StatisticsBox"
        CssClass="fs120" runat="server" />
    
    <br />
    
    &nbsp;&nbsp;<asp:CheckBox Text="Show File Download Button" ID="DownloadBox"
        CssClass="fs120" runat="server" />
    
    <p class="fs120">To narrow search to a directory tree,
        select the directory that will be the root: </p>
    
    <asp:DropDownList ID="DirectoryList"
        Runat="server"
        Width="700px"
        CssClass="fs115" >
    </asp:DropDownList>
    
    <br />
    
    <hr />
    
    <asp:Button Text="Search" ID="SearchButton" CssClass="fs120"
        BackColor="SkyBlue" OnClick="SearchButton_Click" runat="server" />
    
    <hr />
    
    <asp:Label Text="" ID="SearchResults"
        runat="server" />
    
    </ContentTemplate>
    </asp:UpdatePanel>
    <!-- End UpdatePanelForDirectoryList -->
    
    </form>
</body>
</html>
