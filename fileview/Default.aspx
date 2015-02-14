<%@ Page Language="C#" %>
<!DOCTYPE html>

<%@ Import Namespace="edu.neu.ccis.rasala" %>

<script runat="server">

    public const bool onlyPublic = true;

    
    public const string fileview_expects =
@"<p class='h'>FileView expects:</p>
<p>A query string whose only item is the tilde path of the file to view.</p>";

    
    public const string error_preamble = "<p>FileView Error:</p>";


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        SourceTools.LoadCSSandJavascript(this);
    }
    
    
    protected void Page_Load(object sender, EventArgs e)
    {
        string[] parts = RequestTools.QueryParts(Request);

        int n = parts.Length;

        if (n == 0)
        {
            DataBlock.InnerHtml = fileview_expects;
            return;
        }

        string tildeFilePath = parts[0];

        bool success =
            SourceTools.OKtoServeFile(Server, tildeFilePath, onlyPublic);

        if (!success)
        {
            string markupText = StringTools.HtmlEncode(tildeFilePath);

            DataBlock.InnerHtml = error_preamble;
            DataBlock.InnerHtml += SourceTools.NotToServeInQuery;
            DataBlock.InnerHtml += HTML_Tools.open_pre;
            DataBlock.InnerHtml += markupText;
            DataBlock.InnerHtml += HTML_Tools.shut_pre;
            return;
        }

        if (!IsPostBack)
        {
            long bytes;
            DateTime? created;
            DateTime? modified;

            DataBlock.InnerHtml =
                SourceTools.StatisticsMarkup
                    (this, tildeFilePath, null, onlyPublic,
                     true, false, true, true, null,
                     out bytes, out created, out modified);

            DataBlock.InnerHtml += HTML_Tools.hr;

            bool istext =
                FileTools.GetFileCategory(tildeFilePath) == FileTools.TEXT;

            if (istext)
            {
                ShowLineNumbers.Visible = true;
                RuleBlock.InnerHtml = HTML_Tools.hr;
            }
        }

        bool numbers = ShowLineNumbers.Checked;

        ViewBlock.InnerHtml +=
            SourceTools.FileViewMarkup
                (this, tildeFilePath, numbers);
    }

</script>

<html>
<head>
    <meta charset='utf-8' />
    <title>FileView</title>
</head>

<body>
    <form id="form1" runat="server">

    <asp:ScriptManager ID="manager" runat="server">
    </asp:ScriptManager>
    
    <asp:UpdatePanel ID="updatepanel" runat="server">
    <ContentTemplate>

    <div id="DataBlock" runat="server"></div>

    <div class="hide_if_printing">
        <asp:CheckBox ID="ShowLineNumbers" runat="server"
            Text="Show Line Numbers"
            Visible="false"
            AutoPostBack="true" />
    </div>

    <div id="RuleBlock" runat="server"
         class="hide_if_printing"></div>

    <div id="ViewBlock" runat="server" enableviewstate="false"></div>

    </ContentTemplate>
    </asp:UpdatePanel>

    </form>
</body>
</html>
