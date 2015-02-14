<%@ Page Language="C#" EnableEventValidation="false" %>
<%@ Import Namespace="edu.neu.ccis.rasala" %>
<%@ Import  Namespace="System.Data.SqlClient" %>

<!--
    File: ~/database/database.aspx
    
    Copyright 2013,
    Richard Rasala,
    College of Computer and Information Science
    Northeastern University, Boston, MA 02115
    rasala@ccs.neu.edu
-->

<script runat="server">

    string cs = null;

    string csID = "";
    string dbID = "";

    string no_csID = "No connection string ID supplied as query param cs";
    string no_dbID = "No database ID supplied as query param db";

    string invalid_csID = "The connection string ID is invalid";
    string invalid_dbID = "The database ID is invalid";

    
    protected void Page_Load(object sender, EventArgs e)
    {
        NameValueCollection collection = Request.QueryString;

        csID = collection["cs"] ?? "";
        dbID = collection["db"] ?? "";

        if (StringTools.IsTrivial(csID))
        {
            csError(no_csID);
        }
        else
        {
            csData.Text = Server.HtmlEncode(csID);
        }

        if (StringTools.IsTrivial(dbID))
        {
            dbError(no_dbID);
        }
        else
        {
            dbData.Text = Server.HtmlEncode(dbID);
        }

        if (StringTools.IsTrivial(csID))
            return;

        try
        {
            cs = ConfigurationManager.ConnectionStrings[csID].ConnectionString;
        }
        catch { }

        if (StringTools.IsTrivial(cs))
        {
            csError(invalid_csID);
            return;
        }

        if (StringTools.IsTrivial(dbID))
        {
            return;
        }
            
        // Replace %20 with blank in dbID

        dbID = dbID.Replace("%20", " ");

        try
        {
            griddata.ConnectionString = cs;
            griddata.SelectCommand = "SELECT * FROM " + dbID;

            grid.DataBind();

            csData.Text = csID;
            dbData.Text = dbID;
        }
        catch
        {
            dbError(invalid_dbID);
        }
    }


    void csError(string message)
    {
        csDataErrorWrapper.Visible = true;
        csDataError.Text = message;
    }


    void dbError(string message)
    {
        dbDataErrorWrapper.Visible = true;
        dbDataError.Text = message;
    }


</script>

<html lang="en">
<head>
    <meta charset="utf-8" />

    <title></title>

    <meta name="viewport"
        content="initial-scale=1, minimum-scale=1">
    
    <link rel="Stylesheet" type="text/css" href="../css/style.css" />
    
    <link rel="Stylesheet" type="text/css" href="database.css" />
    
</head>
<body>
   
<div class="pad">

<form id="form1" runat="server" enableviewstate="false" >

<div class="fs120">

<p>
<span class="bold">Connection String ID:</span>
<asp:Label ID="csData" CssClass="mono bold" runat="server" />
</p>

<asp:Panel ID="csDataErrorWrapper" runat="server" Visible="false">
<p>
<asp:Label ID="csDataError" CssClass="red bold" runat="server" />
</p>
</asp:Panel>

<p>
<span class="bold">Database Table ID:</span>
<asp:Label ID="dbData" CssClass="mono bold" runat="server" />
</p>

<asp:Panel ID="dbDataErrorWrapper" runat="server" Visible="false">
<p>
<asp:Label ID="dbDataError" CssClass="red bold" runat="server" />
</p>
</asp:Panel>

<p>
<asp:Label ID="debug" Text="Debug:" CssClass="bold"
    Visible="false" runat="server" />
</p>

</div>


<asp:GridView
    ID="grid"
    DataSourceID="griddata"
    AutoGenerateColumns="true"
    AllowSorting="true"
    HeaderStyle-BackColor="#C0C0FF"
    RowStyle-BackColor="#DEB887"
    AlternatingRowStyle-BackColor="#FA8072"
    runat="server" >
</asp:GridView>


<asp:SqlDataSource
    ID="griddata"
    runat="server" >
</asp:SqlDataSource>

</form>
            
</div>

</body>
</html>
