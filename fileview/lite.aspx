<%@ Page Language="C#" %>
<%@ Import Namespace="System.IO" %>

<script runat="server">

static string filepatherror = @"FILE PATH ERROR:
The query string must supply a valid file path
of a file to view in tilde path format: ~/...";


static string specialfilepatherror = filepatherror +
@"

The query string must not contain any one of:
  /hidden/
  /secret/
  /secure/";


static string textfilepatherror = filepatherror +
@"

The file extension is not recognized as a text file.";


static string[] text_extensions =
    {
        ".txt",
        ".cs",
        ".vb",
        ".c",
        ".h",
        ".cpp",
        ".jsl",
        ".java",
        ".js",
        ".vbs",
        ".wsf",
        ".htm",
        ".html",
        ".hta",
        ".htc",
        ".css",
        ".xml",
        ".xsd",
        ".xsp",
        ".xsl",
        ".xslt",
        ".asp",
        ".aspx",
        ".asmx",
        ".ascx",
        ".asax",
        ".ashx",
        ".config",
        ".wsdl",
        ".disco",
        ".discomap",
        ".sitemap",
        ".master",
        ".sql",
        ".xaml",
        ".csproj",
        ".user",
        ".svg",
        ".php",
        ".jsp"
    };


protected void Page_Load(object sender, EventArgs e)
{
    Response.ContentType = "text/plain";
    Response.Write("");

    string query = Request.Url.Query;

    string filepath = "";
    string data = "";
        
    // First check for errors

    // Query is empty
    if (query.Length == 0)
    {
        Response.Write(filepatherror);
        return;
    }

    query = query.Substring(1);

    // Query is ?
    if (query.Length == 0)
    {
        Response.Write(filepatherror);
        return;
    }

    // Query does not start with ~/
    if (!query.StartsWith("~/"))
    {
        ErrorWithQuery(query);
        return;
    }
        
    // Query contains one of /hidden/, /secret/, /secure/
    bool specialerror = query.Contains("/hidden/")
        || query.Contains("/secret/")
        || query.Contains("/secure/");

    if (specialerror)
    {
        SpecialErrorWithQuery(query);
        return;
    }

    // Does query have a recognized text file extension?
    if (!IsTextFile(query))
    {
        TextErrorWithQuery(query);
        return;
    }
        
    // Attempt to retrieve file data        
    try
    {
        filepath = Server.MapPath(query);
            
        using (StreamReader reader = new StreamReader(filepath))
        {
            data = reader.ReadToEnd();
        }
    }
    catch
    {
        ErrorWithQuery(query);
        return;
    }
        
    // If successful, show file data
    Response.Write(data);
}


protected void ErrorWithQuery(string query)
{
    Response.Write(filepatherror);
    Response.Write("\n\nQuery String:\n");
    Response.Write(query);
}


protected void SpecialErrorWithQuery(string query)
{
    Response.Write(specialfilepatherror);
    Response.Write("\n\nQuery String:\n");
    Response.Write(query);
}


protected void TextErrorWithQuery(string query)
{
    Response.Write(textfilepatherror);
    Response.Write("\n\nQuery String:\n");
    Response.Write(query);
}


protected bool IsTextFile(string query)
{
    bool isText = false;

    int length = text_extensions.Length;
    int i = 0;

    while (!isText && (i < length))
    {
        isText = query.EndsWith(text_extensions[i]);
        i++;
    }

    return isText;
}


</script>