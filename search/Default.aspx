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
            InitFileList();

            StatisticsBoxSpan.Visible = onlyPublic;
        }
    }
    

    void InitRootData()
    {
        RootPath.Text = FileTools.GetRoot(this);
        RootOffset.Text = FileTools.GetRootOffset(this);
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


    void InitFileList()
    {
        FileList.Items.Clear();

        string tildeDirectoryPath = GetSelectedDirectory();

        if (tildeDirectoryPath.Length == 0)
            return;

        string directoryPath = MapPath(tildeDirectoryPath);

        List<string> fileList =
            SourceTools.MakeFileList(directoryPath, FileTools.TEXT);
        
        fileList.Insert(0, "");

        FileList.DataSource = fileList;
        FileList.DataBind();
    }


    string GetSelectedDirectory()
    {
        if (DirectoryList.Items.Count == 0)
            return "";
        else
            return DirectoryList.SelectedValue;
    }


    string GetSelectedFile()
    {
        if (FileList.Items.Count == 0)
            return "";
        else
            return FileList.SelectedValue;
    }


    public void DirectoryList_SelectedIndexChanged(object sender, EventArgs e)
    {
        InitFileList();
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
            SearchResults.Text =
                SearchTools.SearchSiteMarkup
                    (this, pattern,
                     isRegex, ignoreCase, statistics, download, onlyPublic);

            return;
        }

        string filePath = GetSelectedFile();

        if (StringTools.IsTrivial(filePath))
        {
            SearchResults.Text =
                SearchTools.SearchDirectoryMarkup
                    (this, tildeDirectoryPath, pattern,
                     isRegex, ignoreCase, statistics, download, onlyPublic);

            return;
        }

        string tildeFilePath = tildeDirectoryPath + filePath;

        bool showAllLines = ShowAllLinesBox.Checked;

        SearchResults.Text =
            SearchTools.SearchFileMarkup
                (this, tildeFilePath, pattern,
                 isRegex, ignoreCase, statistics, download,
                 showAllLines, onlyPublic);
    }

</script>

<!DOCTYPE html>

<html lang='en'>

<head id="head1" runat="server">
    <meta charset="utf-8" />
    <title>Search Server</title>
</head>

<body>
    <form id="form1" runat="server">

    <h1>Search Server</h1>
    
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
            runat="server">Search Server</asp:HyperLink>
    </h4>
    
    
    <asp:Label
        ID="RootPath"
        runat="server"
        Visible="False" />
    
    <asp:Label
        ID="RootOffset"
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
    
    <span id="StatisticsBoxSpan" runat="server">
    &nbsp;&nbsp;<asp:CheckBox Text="Show File Statistics" ID="StatisticsBox"
        CssClass="fs120" runat="server" />
    
    <br />
    </span>
    
    &nbsp;&nbsp;<asp:CheckBox Text="Show File Download Button" ID="DownloadBox"
        CssClass="fs120" runat="server" />
    
    <p class="fs120">To narrow search to one directory, select the directory: </p>
    
    <asp:DropDownList ID="DirectoryList"
        Runat="server"
        AutoPostBack="True"
        Width="700px"
        CssClass="fs115"
        OnSelectedIndexChanged="DirectoryList_SelectedIndexChanged">
    </asp:DropDownList>
    
    <p class="fs120">To narrow search to one text file, select the directory
        above and then select the file: </p>
    
    <asp:DropDownList ID="FileList"
        Runat="server"
        AutoPostBack="True"
        Width="700px"
        CssClass="fs115">
    </asp:DropDownList>
    
    <p class="fs120">If one text file is selected, you may choose to show
        all lines in that file.</p>
    
    &nbsp;&nbsp;<asp:CheckBox Text="Show All Lines" ID="ShowAllLinesBox"
        CssClass="fs120" runat="server" />
    
    <br />
    
    <hr />
    
    <br />
    
    <asp:Button Text="Search" ID="SearchButton" CssClass="fs120"
        BackColor="SkyBlue" OnClick="SearchButton_Click" runat="server" />
    
    <br />
    
    <hr />
    
    <asp:Label Text="" ID="SearchResults"
        runat="server" />
    
    </ContentTemplate>
    </asp:UpdatePanel>
    <!-- End UpdatePanelForDirectoryList -->
    
    </form>
</body>
</html>
