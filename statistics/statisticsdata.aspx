<%@ Page Language="C#"
    EnableEventValidation="false"
    EnableViewState="false" %>

<%@ Import Namespace="edu.neu.ccis.rasala" %>
<%@ Import Namespace="System.Collections.Generic" %>

<script runat="server">
    
    public string NoTildeDirectoryPath =
        "<pre style='color:red'>No directory path provided</pre>";
    
    
    void Page_Load(object sender, EventArgs e)
    {
        SourceTools.LoadCSS(this);
        CheckQueryStringForDirectory();
    }


    void CheckQueryStringForDirectory()
    {
        string[] parts = RequestTools.QueryParts(Request);

        int n = parts.Length;

        if (n == 0)
        {
            Statistics.Text = NoTildeDirectoryPath;
            return;
        }
        
        string directory = parts[0];

        if (directory.Length == 0)
        {
            Statistics.Text = NoTildeDirectoryPath;
            return;
        }

        ShowStatistics(directory);
    }

    
    void ShowStatistics(string tildeDirectoryPath)
    {
        if (tildeDirectoryPath == "")
        {
            Statistics.Text = NoTildeDirectoryPath;
            return;
        }

        bool onlyPublic = true;
        int filetype = FileTools.ALL;
        bool showfiles = true;
        bool summarize = true;
        
        // These out parameters are not used here
        int fileCount;
        long totalBytes;
        DateTime? createdFirst;
        DateTime? createdLast;
        DateTime? modifiedFirst;
        DateTime? modifiedLast;

        Statistics.Text =
            SourceTools.StatisticsMarkupForDirectory
                (this,
                    tildeDirectoryPath,
                    onlyPublic,
                    filetype,
                    showfiles,
                    summarize,
                    out fileCount,
                    out totalBytes,
                    out createdFirst,
                    out createdLast,
                    out modifiedFirst,
                    out modifiedLast);
    }

</script>


<!DOCTYPE html>

<html lang='en'>

<head>
    <meta charset="utf-8" />
    <title>Statistics Data</title>
</head>

<body>

<form id="form1" runat="server">
<asp:Label ID="Statistics" CssClass="results" runat="server" />
</form>
    
</body>

</html>
