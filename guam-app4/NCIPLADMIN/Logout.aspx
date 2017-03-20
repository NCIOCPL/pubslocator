<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Logout.aspx.cs" Inherits="PubEntAdmin.Logout" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Log out</title>
    <base target="_self" />
    <meta http-equiv="Pragma" content="no-cache">
    <meta http-equiv="Expires" content="0">
    <meta http-equiv="Cache-Control" content="no-cache">

    <script type="text/javascript">
        window.onload = function() {            
            window.close();
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <!--<noscript>Click [X] to completely log out the system. -->
            If you want to continue using the system, please click <a href="./AdminHome.htm">here</a>.
        <!--</noscript>-->
    </div>
    </form>
</body>
</html>
