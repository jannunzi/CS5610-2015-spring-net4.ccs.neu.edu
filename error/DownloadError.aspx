<%@ Page Language="C#" %>

<!DOCTYPE html PUBLIC
    "-//W3C//DTD XHTML 1.0 Transitional//EN"
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    
<%@ Import Namespace="edu.neu.ccis.rasala" %>

<script runat="server">
    
    public const string error_preamble = "\n<p>File Download Error Page</p>\n";

    public const string correction =
@"
<p>This page reached in error since the file below may be downloaded.
Click on the button to initiate download.</p>
";

    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SourceTools.LoadCSSandJavascript(this);
        }

        string url = "";

        DataBlock.InnerHtml = error_preamble;
        
        try
        {
            url = RequestTools.Query(Request);

            if (url.StartsWith("url="))
            {
                url = url.Substring(4);
            }
        }
        catch (Exception) {}

        if (StringTools.IsTrivial(url))
        {
            DataBlock.InnerHtml += SourceTools.NoPathInQuery;
            return;
        }

        bool success =
            SourceTools.OKtoServeFile(Server, url, true);

        if (!success)
        {
            DataBlock.InnerHtml += SourceTools.NotToServeInQuery;
            DataBlock.InnerHtml += HTML_Tools.open_pre;
            DataBlock.InnerHtml += StringTools.HtmlEncode(url);
            DataBlock.InnerHtml += HTML_Tools.shut_pre;
            return;
        }

        DataBlock.InnerHtml += correction;
        
        DataBlock.InnerHtml += SourceTools.HeaderLineMarkup
            (this, url, null, true, true);
        
        DataBlock.InnerHtml += SourceTools.DownloadButtonMarkup
            (this, url);
    }
    
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>File Download Error</title>
    
    <style type="text/css">
        body { font-weight: bold }
        pre,code { color: Blue; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div id="DataBlock" runat="server"></div>
    </form>
</body>
</html>
