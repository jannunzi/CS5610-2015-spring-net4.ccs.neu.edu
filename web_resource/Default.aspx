<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" %>

<%@ Import Namespace="edu.neu.ccis.rasala" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Net" %>
<%@ Import Namespace="System.Text" %>

<script runat="server">

    private string[] schemes = Http.GetHttpSchemes();

    private string[] headers = Http.GetHttpHeaders();


    void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            ShowInfoOrData_Click(sender, e);
        }
    }


    public void ShowInfoOrData_Click(object sender, EventArgs e)
    {
        ShowInfoOrData();
    }


    string GetURIFromUser()
    {
        return TheURI.Text;
    }


    void ShowInfoOrData()
    {
        HideInfoAndData();

        string uriString = GetURIFromUser();

        UriPlus uri;

        try
        {
            uri = new UriPlus(uriString);
        }
        catch (Exception ex)
        {
            string error = "URI Parse Error";

            string comment =
                "Comment: Use full URIs not relative URIs.";

            URI_Information.Text = pb(error) + p(ex.Message) + p(comment);

            return;
        }

        if (UriInfoCheckBox.Checked)
            showURIinfo(uri);

        if (UriDataCheckBox.Checked)
            showURIdata(uri);
    }


    void showURIinfo(UriPlus uri)
    {
        string[] data =
            {
                property("AbsoluteUri",   uri.AbsoluteUri),
                property("Scheme",        uri.Scheme),
                property("UserInfo",      uri.UserInfo),
                property("Host",          uri.Host),
                property("AbsolutePath",  uri.AbsolutePath),
                property("Query",         uri.Query),
                property("Fragment",      uri.Fragment),
            };

        URI_Information.Text = StringTools.Build(data);

        ShowSegments(uri);
        ShowParsedSegments(uri);
        ShowFileNameAndExt(uri);
        ShowParsedQuery(uri);
    }


    void ShowSegments(UriPlus uri)
    {
        URI_Information.Text += pb("Segments:");

        string[] segments = uri.Segments;

        URI_Information.Text +=
            HTML_Tools.Ver_HTML_Table(segments, 2, 4, null);
    }


    void ShowParsedSegments(UriPlus uri)
    {
        URI_Information.Text += pb("Parsed Segments:");

        string[][] parsed = uri.ParsedSegments;

        URI_Information.Text +=
            HTML_Tools.HTML_Table(parsed, 2, 4, null);
    }


    void ShowFileNameAndExt(UriPlus uri)
    {
        string filename = uri.FileName;

        URI_Information.Text += property("Filename", filename);


        string extension = uri.FileExtension;

        URI_Information.Text += property("Extension", extension);
    }


    void ShowParsedQuery(UriPlus uri)
    {
        string[] parsedquery = uri.ParsedQuery;

        if (parsedquery.Length == 0)
            return;

        URI_Information.Text += pb("Parsed Query:");

        URI_Information.Text +=
            HTML_Tools.Ver_HTML_Table(parsedquery, 2, 4, null);
    }


    void showURIdata(UriPlus uri)
    {
        if (SupportsURI(uri))
            RequestAndShowURIdata(uri);
        else
            NotSupportedURI(uri);
    }


    bool SupportsURI(UriPlus uri)
    {
        string scheme = uri.Scheme;

        foreach (string s in schemes)
        {
            if (scheme.Equals(s))
                return true;
        }

        return false;
    }


    void NotSupportedURI(UriPlus uri)
    {
        string[] data =
            {
                "<p>The URI scheme <b>",
                uri.Scheme,
                "</b> is not currently supported on this page.</p>\n",
                "<p>The supported schemes are:</p>\n<b>\n",
                HTML_Tools.Ver_HTML_Table(schemes, 2, 4, null),
                "\n</b>\n"
            };

        Response_Feedback.Text += StringTools.Build(data);
    }


    void RequestAndShowURIdata(UriPlus uri)
    {
        HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;

        request.Date = DateTime.Now;
        
        RequestDetails(request);

        HttpWebResponse response = null;

        HttpWebResponse error_response = null;

        try
        {
            response = request.GetResponse() as HttpWebResponse;
        }
        catch (WebException ex1)
        {
            string[] data1 =
                {
                    "<p><b>WebException Error Status: ",
                    StringTools.numeric(ex1.Status),
                    " = ",
                    StringTools.textual(ex1.Status),
                    "</b></p>\n",
                    "<p>WebException Error Message: ",
                    ex1.Message,
                    "</p>\n"
                };

            Response_Feedback.Text += StringTools.Build(data1);

            if (ex1.Status == WebExceptionStatus.ProtocolError)
                error_response = ex1.Response as HttpWebResponse;
        }
        catch (Exception ex2)
        {
            string[] data2 =
                {
                    "<p>Exception Error Message: ",
                    ex2.Message,
                    "</p>\n"
                };

            Response_Feedback.Text += StringTools.Build(data2);
        }

        if (response != null)
        {
            ResponseStatus(response);
            ResponseHeader(response);
            ResponseContent(uri, response);

            response.Close();
        }

        if (error_response != null)
        {
            Response_Feedback.Text += pb("Error Response Data");

            ResponseStatus(error_response);
            ResponseHeader(error_response);
            ResponseContent(uri, error_response);

            error_response.Close();
        }
    }


    void RequestDetails(HttpWebRequest request)
    {
        Request_Information.Text +=
            property("RequestUri", request.RequestUri);
        
        Request_Information.Text +=
            property("Address", request.Address);
        
        Request_Information.Text +=
            property("Method", request.Method);

        Request_Information.Text +=
            property("ProtocolVersion", request.ProtocolVersion);

        Request_Information.Text +=
            property("Date", request.Date);

        Request_Information.Text +=
            property("Accept", request.Accept);
        
        Request_Information.Text +=
            property("AllowAutoRedirect", request.AllowAutoRedirect);
        
        Request_Information.Text +=
            property("AllowWriteStreamBuffering", request.AllowWriteStreamBuffering);
        
        Request_Information.Text +=
            property("AuthenticationLevel", request.AuthenticationLevel);
        
        Request_Information.Text +=
            property("AutomaticDecompression", request.AutomaticDecompression);

        Request_Information.Text +=
            property("CachePolicy", request.CachePolicy);

        Request_Information.Text +=
            property("ClientCertificates", request.ClientCertificates);

        Request_Information.Text +=
            property("Connection", request.Connection);
        
        Request_Information.Text +=
            property("ConnectionGroupName", request.ConnectionGroupName);
        
        Request_Information.Text +=
            property("ContentLength", request.ContentLength.ToString());
        
        Request_Information.Text +=
            property("ContentType", request.ContentType);

        Request_Information.Text +=
            property("ContinueDelegate", request.ContinueDelegate);

        Request_Information.Text +=
            property("Credentials", request.Credentials);

        Request_Information.Text +=
            property("Expect", request.Expect);

        Request_Information.Text +=
            property("Host", request.Host);

        Request_Information.Text +=
            property("IfModifiedSince", request.IfModifiedSince);

        Request_Information.Text +=
            property("ImpersonationLevel", request.ImpersonationLevel);

        Request_Information.Text +=
            property("KeepAlive", request.KeepAlive);

        Request_Information.Text +=
            property("MaximumAutomaticRedirections", request.MaximumAutomaticRedirections);

        Request_Information.Text +=
            property("MaximumResponseHeadersLength", request.MaximumResponseHeadersLength);

        Request_Information.Text +=
            property("MediaType", request.MediaType);

        Request_Information.Text +=
            property("Pipelined", request.Pipelined);

        Request_Information.Text +=
            property("PreAuthenticate", request.PreAuthenticate);

        if (request.Proxy != null)
        {
            object proxy = request.Proxy.GetProxy(request.RequestUri);
            Request_Information.Text += property("Proxy", proxy);
        }
        else
        {
            Request_Information.Text += property("Proxy", null);
        }

        Request_Information.Text +=
            property("ReadWriteTimeout", request.ReadWriteTimeout);

        Request_Information.Text +=
            property("Referer", request.Referer);

        Request_Information.Text +=
            property("SendChunked", request.SendChunked);

        Request_Information.Text +=
            property("Timeout", request.Timeout);

        Request_Information.Text +=
            property("TransferEncoding", request.TransferEncoding);

        Request_Information.Text +=
            property("UseDefaultCredentials", request.UseDefaultCredentials);

        Request_Information.Text +=
            property("UserAgent", request.UserAgent);

        foreach (string header in request.Headers.Keys)
        {
            string result = request.Headers.Get(header);
            Request_Information.Text += property(header, result);
        }
    }
    

    void ResponseStatus(HttpWebResponse response)
    {
        var protocol = response.ProtocolVersion.ToString();
        var status = response.StatusCode;
        var n = StringTools.numeric(status);
        var t = StringTools.textual(status);

        Response_Feedback.Text += "\n<p><b>";
        Response_Feedback.Text += "HTTP/";
        Response_Feedback.Text += protocol;
        Response_Feedback.Text += " ";
        Response_Feedback.Text += n;
        Response_Feedback.Text += " ";
        Response_Feedback.Text += t;
        Response_Feedback.Text += "</b></p>\n";
    }


    void ResponseHeader(HttpWebResponse response)
    {
        foreach (string header in headers)
        {
            string result = response.GetResponseHeader(header);

            if (StringTools.IsTrivial(result))
                continue;

            Response_Feedback.Text += property(header, result);
        }
    }


    void ResponseContent(UriPlus uri, HttpWebResponse response)
    {
        int category = Http.ResponseCategory(response);

        if (category == FileTools.TEXT)
            ResponseContentText(response);
        else if (category == FileTools.IMAGE)
            ResponseContentImage(uri, response);
        else
            URI_Source.Text = pb("Response content is neither text nor image");

        if (category != FileTools.OTHER)
            ResponseContentLink(uri);
    }


    void ResponseContentText(HttpWebResponse response)
    {
        string content = Http.GetUncompressedTextContent(response);

        if (StringTools.IsTrivial(content))
            content = "No content returned!";

        URI_Source.Text += show(content);
    }


    void ResponseContentImage(UriPlus uri, HttpWebResponse response)
    {
        int status = (int)response.StatusCode;

        if ((200 <= status) && (status < 300))
        {
            URI_Image.Visible = true;
            URI_Image.ImageUrl = uri.OriginalString;
        }
    }


    void ResponseContentLink(UriPlus uri)
    {
        string[] data =
            {
                "<p><a href='",
                uri.OriginalString,
                "' target='_blank' >",
                uri.OriginalString,
                "</a></p>"
           };

        URI_HTML_Link.Text = StringTools.Build(data);
    }
    
    
    void HideInfoAndData()
    {
        URI_Information.Text = "";
        Response_Feedback.Text = "";
        URI_Source.Text = "";
        URI_HTML_Link.Text = "";
        URI_Image.Visible = false;
        URI_Image.ImageUrl = "";
    }


    string b(string item)
    {
        return "<b>" + item + "</b>";
    }


    string p(string item)
    {
        return "<p>" + item + "</p>\n";
    }


    string pb(string item)
    {
        return p(b(item));
    }


    string property(string name, object value)
    {
        string s = "";
        
        if (value == null)
        {
            s = "null";
        }
        else
        {
            s = value.ToString();
        }
        
        return p(b(name) + ": " + s);
    }


    string pre(string item)
    {
        return "<pre>" + item + "</pre>\n";
    }


    string show(string item)
    {
        return pre(StringTools.HtmlEncode(item));
    }

</script>

<!DOCTYPE html
    PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >

<head id="Head1" runat="server">
<title>Web URI Tester</title>

<link rel="Stylesheet" type="text/css" href="../css/style.css" />
<link rel="Stylesheet" type="text/css" href="../css/style_screen.css" media="screen" />
<link rel="Stylesheet" type="text/css" href="../css/enlarge.css" />

<script type='text/javascript'
        src='http://ajax.googleapis.com/ajax/libs/jquery/1/jquery.min.js'>
</script>

<link rel="stylesheet" href="../expandable/expandable.css" type="text/css" />
<script type="text/javascript" src="../expandable/expandable.js"></script>

</head>

<body>

<div class="pad">

<form id="form1" runat="server">

<asp:ScriptManager ID="ScriptManager1" runat="server" />

<h1 class="center">Web URI and Resource Tester</h1>
    
<hr />

<p class="expandable bold">Links to Source</p>

<div>

<rasala:FileView runat="server" />

<rasala:FileView runat="server"
    TildeFilePath="~/app_code/UriPlus.cs" />

<rasala:FileView runat="server"
    TildeFilePath="~/app_code/Http.cs" />

</div>

<hr />

<p class="expandable bold">Instructions</p>

<div>

    
<p>This site permits a user both to explore the parsing of a web URI
and to see what happens when the corresponding resource is requested.</p>
    
<p>To explore URI parsing, check <i>Show URI Information</i> but do
not check <i>Show URI Web Data</i>.  With these settings, the URI
may be valid or be complete nonsense entered simply to test the
parsing rules.</p>
    
<p>To explore the text content or image data downloaded as part of a
web response, check <i>Show URI Web Data</i>.  Optionally, check
<i>Show URI Information</i> to examine how the URI is being parsed.</p>
    
<p>When information is requested, status codes and possible error
codes are displayed as well as other information about the content.</p>
    
<p>The application is activated by clicking the button.</p>

</div>

<hr />

<p class="bold">The Web URI</p>

<asp:TextBox ID="TheURI" runat="server"
    Width="800px"
    Wrap="False" ></asp:TextBox>
        
<br />
<br />
    
<asp:CheckBox ID="UriInfoCheckBox"
    runat="server"
    Text="Show URI Information" />
    
<br />
<br />
    
<asp:CheckBox ID="UriDataCheckBox"
    runat="server"
    Text="Show URI Web Data" />
    
<br />
<br />
    
<asp:Button ID="ShowUriInfoOrDataButton"
    Runat="server"
    Text="Show URI Information and/or Web Data"
    OnClick="ShowInfoOrData_Click" />
    
<br />
<br />
    
<hr />

<p class="expandable bold">URI Information</p>

<div>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">

<ContentTemplate>

<asp:Label
    ID="URI_Information" 
    Runat="server" />

</ContentTemplate>

</asp:UpdatePanel>

</div>
   
<hr />
    
<p class="expandable bold">Request Data</p>

<div>

<asp:UpdatePanel ID="UpdatePanel2" runat="server">

<ContentTemplate>

<asp:Label
    ID="Request_Information"
    Runat="server" />

</ContentTemplate>

</asp:UpdatePanel>

</div>

<hr />

<p class="expandable bold">Response Data</p>

<div>

<asp:UpdatePanel ID="UpdatePanel3" runat="server">

<ContentTemplate>

<asp:Label
    ID="Response_Feedback"
    Runat="server" />

</ContentTemplate>

</asp:UpdatePanel>

</div>

<hr />

<p class="bold">Web URI Link</p>

<asp:UpdatePanel ID="UpdatePanel4" runat="server">

<ContentTemplate>

<asp:Label
    ID="URI_HTML_Link"
    Runat="server" />
    
</ContentTemplate>

</asp:UpdatePanel>

<p class="bold">Web URI Response Data</p>

<asp:UpdatePanel ID="UpdatePanel5" runat="server">

<ContentTemplate>

<asp:Label
    ID="URI_Source"
    Runat="server" />

<asp:Image
    ID="URI_Image"
    Runat="server"
    Visible="False" />
    
</ContentTemplate>

</asp:UpdatePanel>

</form>

</div>

</body>

</html>
