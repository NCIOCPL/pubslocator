<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowFile.aspx.cs" Inherits="PubEntAdmin.ShowFile" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>ShowFile</title>
</head>
<body>
    <form id="ShowFile" method="post" runat="server">
        <span id="spnMessage" runat="server">Loading File Attachment...</span>
        <button runat="server" id="btnClose" title="Close" onclick="window.close();" type="button">
            Close</button>
    </form>
</body>
</html>
