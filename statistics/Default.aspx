<%@ Page Language="C#"
    EnableEventValidation="false"
    EnableViewState="false" %>

<%@ Import Namespace="edu.neu.ccis.rasala" %>
<%@ Import Namespace="System.Collections.Generic" %>

<script runat="server">
    
    List<string> tildeDirectoryList;

    
    void Page_Load(object sender, EventArgs e)
    {
        SourceTools.LoadCSSandJavascript(this);

        InitDirectoryDropdown();

        CheckQueryStringForDirectory();
    }


    void InitDirectoryDropdown()
    {
        string rootPath = FileTools.GetRoot(this);
        bool onlyPublic = true;
        
        List<string> directoryList =
            SourceTools.MakeDirectoryList(rootPath, onlyPublic);

        tildeDirectoryList =
            FileTools.GetTildePaths(rootPath, directoryList);

        tildeDirectoryList.Insert(0, "");
        
        DirectoryDropdown.DataSource = tildeDirectoryList;
        DirectoryDropdown.DataBind();
    }


    void CheckQueryStringForDirectory()
    {
        string[] parts = RequestTools.QueryParts(Request);

        int n = parts.Length;

        if (n == 0)
            return;

        string directory = parts[0];

        if (directory.Length == 0)
            return;

        int index = 0;

        int length = tildeDirectoryList.Count;

        for (int i = 1; i < length; i++)
        {
            if (String.Equals
                    (tildeDirectoryList[i],
                     directory,
                     StringComparison.OrdinalIgnoreCase))
            {
                index = i;
                break;
            }
        }

        if (index > 0)
        {
            DirectoryDropdown.SelectedIndex = index;
        }
    }
</script>


<!DOCTYPE html>

<html lang='en'>

<head id="head1" runat="server">
    <meta charset="utf-8" />
    <title>Statistics Server</title>
</head>

<body>

<a id="top"></a>

<h1>Statistics Server</h1>
    
<h4>
    <a href="../Default.aspx" target="_blank">Home</a>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    Open New
    <a href="Default.aspx" target="_blank">Statistics Server</a>
</h4>
    
<p class="fs120">Select the directory on the web site to show statistics:</p>
    
<form id="form1" runat="server" enableviewstate="false">

<asp:DropDownList ID="DirectoryDropdown"
    Runat="server"
    EnableViewState="false"
    Width="700px"
    CssClass="dropdown fs115" >
</asp:DropDownList>
    
</form>

<br />
    
<hr />

<br />
   
<div id="content"></div>

<p id="gototop" class="fs120"><a href="#top">Go To Top</a></p>
    
</body>

</html>


<script type="text/javascript">

    var content$;
    var dropdown$;
    var directory = "";
    var gototop$;


    function fetchstatistics() {
        content$.html("");
        gototop$.hide();

        directory = $(".dropdown>option:selected").val();

        if (!directory) {
            return;
        }

        var url = "statisticsdata.aspx?" + directory + " .results";
        content$.load(url);
        gototop$.show();
    }


    function errorhandler() {
        var s = "<p>Error with <code>" + directory + "</code></p>";
        content$.html(s);
    }


    function initialize() {
        $.ajaxSetup({
            timeout: 240000,
            error: errorhandler
        });

        content$ = $("#content");
        gototop$ = $("#gototop");

        dropdown$ = $(".dropdown");
        dropdown$.change(fetchstatistics);

        fetchstatistics();
    }


    $(initialize);

</script>