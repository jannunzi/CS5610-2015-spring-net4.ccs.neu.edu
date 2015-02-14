<%@ Page Language="C#" EnableEventValidation="false" %>
<%@ Import Namespace="edu.neu.ccis.rasala" %>
<%@ Import Namespace="System.Collections.Generic" %>

<!--
    File: paths.aspx
    
    Copyright 2013,
    Richard Rasala,
    College of Computer and Information Science
    Northeastern University, Boston, MA 02115
    rasala@ccs.neu.edu
-->


<!--

Returns a tree of public tilde paths on a site.


Parameter: root=basepath

basepath is a tilde directory path that is the
root of the directory tree for the paths

If missing then defaults to ~/

-->


<script runat="server">

    void Page_Load(object sender, EventArgs e)
    {
        string root = Context.Request.QueryString.Get("root") ?? "~/";
        List<string> list = FileListTools.TildeDirectoryPathList(this.Server, root);
        results.InnerHtml = StringTools.Build(list, "\n");
    }

</script>


<!DOCTYPE html>

<html lang='en'>

<head>
<meta charset='utf-8' />
<meta name="viewport" content="initial-scale=1, minimum-scale=1">
  
<title>Paths</title>
  
<link rel="stylesheet" type="text/css" href="source/source.css" />
<link rel="stylesheet" type="text/css" href="source/print.css" media="print" />

</head>

<body>

<form id="form1" runat="server" enableviewstate="false">
<pre id="results" runat="server"></pre>
</form>

</body>
</html>
