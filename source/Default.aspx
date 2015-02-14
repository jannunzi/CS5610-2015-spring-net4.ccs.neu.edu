<%@ Page Language="C#"
    AutoEventWireup="true" %>

<%@ Import Namespace="edu.neu.ccis.rasala" %>
<%@ Import Namespace="System.Collections.Generic" %>

<script runat="server">
    
    public const bool onlyPublic = true;
    

    void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SourceTools.LoadCSSandJavascript(this);

            InitRootData();
            InitDirectoryList();
            InitFileList();
        }
    }


    void InitRootData()
    {
        RootPath.Text = FileTools.GetRoot(this);
        RootOffset.Text = FileTools.GetRootOffset(this);
    }


    void InitDirectoryList()
    {
        string rootpath = RootPath.Text;

        List<string> directoryList =
            SourceTools.MakeDirectoryList(rootpath, onlyPublic);

        List<string> tildeDirectoryList =
            FileTools.GetTildePaths(rootpath, directoryList);
        
        DirectoryList.DataSource = tildeDirectoryList;
        DirectoryList.DataBind();
    }


    void InitFileList()
    {
        FileList.Items.Clear();

        string tildeDirectoryPath = GetSelectedDirectory();

        if (tildeDirectoryPath.Length == 0)
        {
            HideSourceOrData();
            return;
        }

        string directoryPath = MapPath(tildeDirectoryPath);

        bool restricted = FileListChoice.SelectedValue == "Viewable";

        List<string> fileList =
            SourceTools.MakeFileList(directoryPath, restricted);

        fileList.Insert(0, "");

        FileList.DataSource = fileList;
        FileList.DataBind();

        ShowSourceOrData();
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


    public void FileListChoice_Changed(object sender, EventArgs e)
    {
        InitFileList();
    }


    public void FileList_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShowSourceOrData();
    }


    protected void
        LineNumbers_CheckBox_CheckedChanged(object sender, EventArgs e)
    {
        ShowSourceOrData();
    }


    void ShowSourceOrData()
    {
        HideSourceOrData();

        string tildePath = GetSelectedDirectory();

        if (StringTools.IsTrivial(tildePath))
            return;
        
        string fileName = GetSelectedFile();

        if (StringTools.IsTrivial(fileName))
            return;

        string tildeFilePath = tildePath + fileName;
        
        long bytes;
        DateTime? created = null;
        DateTime? modified = null;

        FileDataBlock.InnerHtml =
            SourceTools.StatisticsMarkup
                (this, tildeFilePath, null,
                 onlyPublic,  false, false, true, true, null,
                 out bytes, out created, out modified);

        FileDataBlock.InnerHtml += HTML_Tools.hr;

        bool numberlines = LineNumbers_CheckBox.Checked;
        
        FileViewBlock.InnerHtml =
            SourceTools.FileViewMarkup
                (this, tildeFilePath, numberlines);
    }


    void HideSourceOrData()
    {
        FileDataBlock.InnerHtml = "";
        FileViewBlock.InnerHtml = "";
    }
    
</script>


<!DOCTYPE html>

<html lang='en'>

<head id="head1" runat="server">
    <meta charset="utf-8" />
    <title>Source Server</title>
</head>

<body>

    <form id="form1" runat="server">

    <h1>Source Server</h1>
    
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
            runat="server">Source Server</asp:HyperLink>
    </h4>
    
    <asp:ScriptManager ID="ScriptManagerForSource" runat="server">
    </asp:ScriptManager>
    
    <asp:UpdatePanel ID="UpdatePanelForDirectoryList" runat="server">
    <ContentTemplate>
    
    <p class="fs120">Directory List for the Web Site:</p>
    
    <asp:DropDownList ID="DirectoryList"
        Runat="server"
        AutoPostBack="True"
        Width="700px"
        CssClass="fs115"
        OnSelectedIndexChanged="DirectoryList_SelectedIndexChanged">
    </asp:DropDownList>
    
    <br />
    <br />
    
    <hr />
    
    <p class="fs120">File List for the Selected Directory:</p>
    
    <asp:RadioButtonList ID="FileListChoice" runat="server"
        OnSelectedIndexChanged="FileListChoice_Changed"
        AutoPostBack="True" >
    
        <asp:ListItem Selected="True" Value="Viewable">
            <span class="fs120">List Viewable Files</span>
        </asp:ListItem>
            
        <asp:ListItem Value="All" >
            <span class="fs120">List All Files</span>
        </asp:ListItem>
    
    </asp:RadioButtonList>
    
    <br />
    
    <asp:DropDownList ID="FileList"
        Runat="server"
        AutoPostBack="True"
        Width="700px"
        CssClass="fs115"
        OnSelectedIndexChanged="FileList_SelectedIndexChanged">
    </asp:DropDownList>
    <br />
    <br />
    
    <hr />
    
    <p>
    <asp:CheckBox
        ID="LineNumbers_CheckBox"
        Text=" Show Line Numbers"
        AutoPostBack="True"
        OnCheckedChanged="LineNumbers_CheckBox_CheckedChanged"
        CssClass="fs120"
        runat="server" />
    </p>
    
    <hr />
    
    <div id="FileDataBlock" runat="server"></div>
    
    <div id="FileViewBlock" runat="server"></div>
        
    <hr />
    
    <asp:Label ID="Debug" runat="server" />
    
    <asp:Label
        ID="RootPath"
        runat="server"
        Visible="False" />
    
    <br />
    
    <asp:Label
        ID="RootOffset"
        runat="server"
        Visible="False" />
    
    <br />
    
    </ContentTemplate>
    </asp:UpdatePanel>
    <!-- End UpdatePanelForDirectoryList -->
    
    </form>
    
</body>

</html>
