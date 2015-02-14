<%@ Page Language="C#" %>

<%@ Import Namespace="edu.neu.ccis.rasala" %>

<script runat="server">

    static string validchars = "^[A-Za-z0-9_]+$";
    
    
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.ContentType = "text/plain";
        // Response.Write("\n1. Enter\n");
        
        string query = RequestTools.Query(Request);

        if (StringTools.IsTrivial(query))
        {
            // Response.Write("2. Trivial Key\n");
            return;
        }

        // Response.Write("3. Key: " + key + "\n");

        string[] parts = RequestTools.QueryParts(Request);

        string key = parts[0];

        // Response.Write("3. Key: " + key + "\n");

        bool valid = Regex.IsMatch(key, validchars,
            RegexOptions.IgnoreCase);

        if (!valid)
        {
            // Response.Write("4. Regex Failure\n");
            return;
        }
        
        string secret = KeyTools.GetKey(this, key);

        // Response.Write("5. Requested Key\n");

        Response.Write(secret);
    }

</script>