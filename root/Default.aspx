<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="NCIPLRoot._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <br />
    <br />
    <br />
    <br />
    <div>
        <asp:Panel ID="pn" runat="Server" Visible="false">
            <p style="font-family: Arial; font-size: medium">
                You will be redirected to the
                <asp:HyperLink ID="lnkPubSite" runat="server" Text="NCI Publications Locator"></asp:HyperLink>
                in a moment.<br />
                <br />
                <span style="font-weight: bold">If you see an error message on the next screen,
                please update your browser to the newest version.</span>
            </p>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
