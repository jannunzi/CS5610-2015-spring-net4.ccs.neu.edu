<%@ Page Language="C#"
    AutoEventWireup="true" %>

<%@ Import Namespace="edu.neu.ccis.rasala" %>

<script runat="server">
    
    public const bool onlyPublic = true;

    
    void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SourceTools.LoadCSSandJavascript(this);

            bool showlinks = true;
            bool summarize = true;
            
            // These out parameters are not used here
            int fileCount;
            long totalBytes;
            DateTime? createdFirst;
            DateTime? createdLast;
            DateTime? modifiedFirst;
            DateTime? modifiedLast;

            SiteStatistics.Text =
                SourceTools.StatisticsMarkupForSite(
                    this,
                    onlyPublic,
                    showlinks,
                    summarize,
                    out fileCount,
                    out totalBytes,
                    out createdFirst,
                    out createdLast,
                    out modifiedFirst,
                    out modifiedLast);
        }
    }

</script>


<!DOCTYPE html>

<html lang='en'>

<head id="head1" runat="server">
    <meta charset="utf-8" />
    <title>Site Statistics Server</title>
</head>

<body>

    <form id="form1" runat="server">

    <h1>Site Statistics Server</h1>
    
    <h4><asp:HyperLink
            ID="HomeLink"
            NavigateUrl="~/Default.aspx"
            Target="_blank"
            runat="server">Home</asp:HyperLink>
    </h4>
    
    <hr />

    <asp:Label ID="SiteStatistics" runat="server"
        enableviewstate="false" />
    
    </form>
    
</body>

</html>
