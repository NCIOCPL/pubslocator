<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dispimage.aspx.cs" Inherits="NCIPLex.dispimage" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>NCI Publications Locator</title>
    <META HTTP-EQUIV="Pragma" CONTENT="no-cache">
	<META HTTP-EQUIV="CACHE-CONTROL" CONTENT="NO-CACHE">
    <META HTTP-EQUIV="CACHE-CONTROL" CONTENT="NO-STORE">  
    <META HTTP-EQUIV="Expires" CONTENT="-1"/>
    <SCRIPT LANGUAGE=JAVASCRIPT>
    <!--
        if (top.frames.length != 0)
            top.location = self.document.location;
    // -->
    </SCRIPT>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Image ID="PubLargeImage" runat="server" />
    </div>
    </form>
</body>
</html>
