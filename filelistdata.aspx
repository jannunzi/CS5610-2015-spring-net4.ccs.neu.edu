<%@ Page Language="C#"
    EnableEventValidation="false"
    EnableViewState="false" %>

<%@ Import Namespace="edu.neu.ccis.rasala" %>
<%@ Import Namespace="System.Collections.Generic" %>

<!--
    File: filelistdata.aspx
    
    Copyright 2013,
    Richard Rasala,
    College of Computer and Information Science
    Northeastern University, Boston, MA 02115
    rasala@ccs.neu.edu
-->


<!--

First constructs a list of TildePathInfo objects
subject to the following constraints based on
query parameters.


Parameter: root=basepath

basepath is a tilde directory path that is the
root of the directory tree for the files

If missing then defaults to ~/


Parameter: start=somedate

somedate represents a date that can be read via
DateTimeExtensions.FromYMDHMS

If missing then defaults to first moment of the
current year


Parameter: end=somedate

somedate represents a date that can be read via
DateTimeExtensions.FromYMDHMS

If missing then defaults to now


Parameter: sort=compare_mode

compare_mode is one of "alpha", "size", "date"

If missing or incorrect the defaults to "alpha"


Once the list of TildePathInfo objects is built
and sorted, the data is output using the method
ToHtml of TildePathInfo for each list object.

-->


<script runat="server">

    void Page_Load(object sender, EventArgs e)
    {
        string root = Context.Request.QueryString.Get("root") ?? "~/";
        string start = Context.Request.QueryString.Get("start") ?? "";
        string end = Context.Request.QueryString.Get("end") ?? "now";
        string sort = Context.Request.QueryString.Get("sort") ?? "alpha";

        List<TildePathInfo> tildefileinfolist =
            FileListTools.TildeFilePathList
                (this.Server, root, start, end, sort);

        StringBuilder builder = new StringBuilder();

        int count = tildefileinfolist.Count;

        builder.Append("<p><b>File count: ");
        builder.Append(count);
        builder.Append("</b></p>\n");
        
        foreach (TildePathInfo tildefileinfo in tildefileinfolist)
        {
            builder.Append(tildefileinfo.ToHtml(this));
        }
        
        results.InnerHtml = builder.ToString();
    }

</script>


<!DOCTYPE html>

<html lang='en'>

<head>
<meta charset='utf-8' />
<meta name="viewport" content="initial-scale=1, minimum-scale=1">
  
<title>Files</title>
  
<link rel="stylesheet" type="text/css" href="source/source.css" />
<link rel="stylesheet" type="text/css" href="source/print.css" media="print" />

</head>

<body>

<form id="form1" runat="server" enableviewstate="false">
<div id="results" class="results" runat="server"></div>
</form>

</body>
</html>
