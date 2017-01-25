<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="NCIPLex.Default" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>NCIPLex</title>
    <meta name="content-language" content="en" />
    <script type="text/javascript">
        window.onload = function() {
	window.self.location.replace('./conf.aspx?js=1'); 
        }
    </script>
    <noscript>
        <meta http-equiv="refresh" content="0;url=./conf.aspx?js=2"/>
    </noscript>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <noscript>
            Re-directing...If the page does not re-direct immediately please click <a href="./conf.aspx?js=2">here</a>.
        </noscript>
    </div>
    </form>
</body>
</html>
