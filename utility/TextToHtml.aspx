<%@ Page Language="C#" EnableEventValidation="false" ValidateRequest="false" %>
<%@ Import Namespace="edu.neu.ccis.rasala" %>

<script runat="server">

    protected void EncodeButton_Click(object sender, EventArgs e)
    {
        string text = InputBox.Text;
        string encodeText = StringTools.HtmlEncode(text);
        string doubleEncodeText = StringTools.HtmlEncode(encodeText);
        
        // After rendering the double encoded text in the browser
        // the user will see the single encoded text
        
        Output.Text = pre(doubleEncodeText);
    }

    
    public static string pre(string item)
    {
        return "<pre>" + item + "</pre>";
    }

    
    protected void ClearButton_Click(object sender, EventArgs e)
    {
        InputBox.Text = "";
        Output.Text = "";
    }

</script>

<!DOCTYPE html>

<html lang='en'>

<head runat="server">

<meta charset='utf-8' />
<meta name="viewport" content="initial-scale=1, minimum-scale=1">
  
<title>Text to Html</title>
    
<link rel="stylesheet" type="text/css" href="~/css/style.css" />
<link rel="shortcut icon" type="image/ico" href="~/wavygon.ico" />
    
<style type="text/css" media="screen">
    .pad { padding: 10px; }
</style>

</head>

<body>
<div class="pad">

<form id="form1" runat="server">
    
<p class="fs125 bold">
Utility to Encode Text to Html
</p>

<p class="bold">
Enter the text in the text box and click the button
</p>


<rasala:FileView runat="server" HR_Before="true" HR_After="true"/>


<asp:ScriptManager ID="MasterScriptManager" runat="server" />
    
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
    
<p>
<asp:Button ID="EncodeButton" runat="server" Text="HTML Encode Text" 
    onclick="EncodeButton_Click" CssClass="bold" />

<asp:Button ID="ClearButton" runat="server" Text="Clear Text" 
    onclick="ClearButton_Click" CssClass="bold" />
</p>

<div>
<asp:TextBox ID="InputBox" runat="server" TextMode="MultiLine"
Width="850px" Height="200px" CssClass="fs125 mono"></asp:TextBox>
</div>

<asp:Label ID="Output" runat="server" Text="" CssClass="fs125" />
    
</ContentTemplate>
</asp:UpdatePanel>
    
</form>

</div>
</body>
</html>
