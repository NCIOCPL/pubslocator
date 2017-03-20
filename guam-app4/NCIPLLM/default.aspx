<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="PubEnt._default" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>NCI Publications Locator for Lockheed Martin</title>
    <meta name="content-language" content="en" />
    <script type="text/javascript">
        window.onload = function() {
        window.self.location.replace('./login.aspx?js=1'); 
        }
    </script>
    <noscript>
        <meta http-equiv="refresh" content="0;url=./login.aspx?js=2"/>
    </noscript>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <noscript>
            <!--Re-directing...If the page does not re-direct immediately please click <a href="./home.aspx?js=2">here</a>.-->
            Re-directing...If the page does not re-direct immediately please click <a href="./login.aspx?js=2">here</a>.
        </noscript>
    </div>
    </form>
</body>
</html>
