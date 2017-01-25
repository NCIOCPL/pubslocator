<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HoldOrderReport.aspx.cs" Inherits="PubEntAdmin.holdorderreport" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Held Order Report</title>
    <link href="Css/Common.css" type="text/css" rel="stylesheet">
</head>
<body class="Admin">
    <div class="skip"><a href="#mainBody">Skip to content</a></div>
    <div id="container">
        <h1>Publications Enterprise Admin Tool</h1>
        <h2>
            <asp:Label ID="lblPageTitle" runat="server" Text=""></asp:Label></h2>
        <form id="cssetupform" runat="server">
            <div id="header">
                <asp:PlaceHolder ID="plcHldMenu" runat="server"></asp:PlaceHolder>
            </div>
            <div id="mainBody">
                <div id="dvSearch">
                    <fieldset>
                        <legend class="Legend1">Held Order Report</legend>
                        <div>
                            <asp:Label ID="Label1" runat="server" Text="Start Date" AssociatedControlID="txtStartDt"
                                CssClass=""></asp:Label>
                            <asp:TextBox ID="txtStartDt" runat="server"></asp:TextBox>
                            <asp:Label ID="Label2" runat="server" Text="End Date"
                                CssClass="" AssociatedControlID="txtEndDt"></asp:Label>
                            <asp:TextBox ID="txtEndDt" runat="server"></asp:TextBox>
                            <asp:Button ID="btnFind" runat="server" Text="Run" OnClick="btnFind_Click" />
                            <asp:Label ID="lblTitle" runat="server"></asp:Label>
                            <asp:Label ID="Label4" runat="server" Text="Flagged Orders"></asp:Label>
                            <asp:Label ID="lblHeld" runat="server"></asp:Label>
                            <asp:Label ID="Label5" runat="server" Text="Deleted"></asp:Label>
                            <asp:Label ID="lblDeleted" runat="server"></asp:Label>
                            <asp:Label ID="Label6" runat="server" Text="Released"></asp:Label>
                            <asp:Label ID="lblReleased" runat="server"></asp:Label>
                            <asp:Label ID="Label7" runat="server" Text="% Deleted"></asp:Label>
                            <asp:Label ID="lblDeletedPercent" runat="server"></asp:Label>
                            <asp:Label ID="Label8" runat="server" Text="Pubs Not Distributed"></asp:Label>
                            <asp:Label ID="lblPubsNotDist" runat="server"></asp:Label>
                        </div>
                    </fieldset>
                </div>
            </div>
        </form>
    </div>
</body>
</html>
