<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="holddetails.aspx.cs" Inherits="PubEntAdmin.holddetails" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Order on-hold</title>
    <link href="../CSS/Common.css" rel="stylesheet" type="text/css" />
</head>
<body class="Admin">
    <div class="skip"><a href="#mainBody">Skip to content</a></div>
    <div id="container">
        <h1>Publications Enterprise Admin Tool</h1>
        <h2>Order Compare</h2>
        <form id="cssetupform" runat="server">
            <asp:PlaceHolder ID="plcHldMenu" runat="server"></asp:PlaceHolder>
            <div id="mainBody">
                <asp:HyperLink ID="HyperLink1" runat="server"
                    NavigateUrl="~/AdminHome.htm">Back to list</asp:HyperLink>
                <div id="dvSearch">
                    <fieldset>
                        <legend id="legend1" class="Legend1" runat="server">Order on-hold</legend>
                        <div>
                            <asp:Label ID="lblMessage" runat="server" ForeColor="#990000"></asp:Label>
                            OLD ORDER
                        <asp:Button ID="btnRelease" runat="server" Text="Release Order"
                            OnClick="btnRelease_Click" />
                            <asp:Button ID="btnDelete" runat="server" Text="Delete Order" OnClick="btnDelete_Click" />
                            <strong>Order ID</strong>
                            <asp:Label ID="aOrderID" runat="server" Font-Bold="True"></asp:Label>
                            <asp:Label ID="bOrderID" runat="server" Font-Bold="True"></asp:Label>
                            <strong>Created</strong>
                            <asp:Label ID="aCreated" runat="server"></asp:Label>
                            <asp:Label ID="bCreated" runat="server"></asp:Label><strong>Name</strong>
                            <asp:Label ID="aName" runat="server"></asp:Label>
                            <asp:Label ID="bName" runat="server"></asp:Label>
                            <strong>Organization </strong>
                            <asp:Label ID="aOrg" runat="server"></asp:Label>
                            <asp:Label ID="bOrg" runat="server"></asp:Label>
                            <strong>Address 1</strong>
                            <asp:Label ID="aAddr1" runat="server"></asp:Label>
                            <asp:Label ID="bAddr1" runat="server"></asp:Label>
                            <strong>Address 2</strong>
                            <asp:Label ID="aAddr2" runat="server"></asp:Label>
                            <asp:Label ID="bAddr2" runat="server"></asp:Label>
                            <strong>ZIP Code</strong>
                            <asp:Label ID="aZip" runat="server"></asp:Label>
                            <asp:Label ID="bZip" runat="server"></asp:Label>
                            <strong>City</strong>
                            <asp:Label ID="aCity" runat="server"></asp:Label>
                            <asp:Label ID="bCity" runat="server"></asp:Label>
                            <strong>State</strong>
                            <asp:Label ID="aState" runat="server"></asp:Label>
                            <asp:Label ID="bState" runat="server"></asp:Label>
                            <strong>Phone</strong>
                            <asp:Label ID="aPhone" runat="server"></asp:Label>
                            <asp:Label ID="bPhone" runat="server"></asp:Label>
                            <strong>E-mail</strong>
                            <asp:Label ID="aEmail" runat="server"></asp:Label>
                            <asp:Label ID="bEmail" runat="server"></asp:Label>
                            <strong>3rd Party?</strong>
                            <asp:Label ID="aShipmethod" runat="server"></asp:Label>
                            <asp:Label ID="bShipmethod" runat="server"></asp:Label>
                            <strong>Comment</strong>
                            <asp:Label ID="aComment" runat="server"></asp:Label>
                            <asp:Label ID="bComment" runat="server"></asp:Label>
                            <strong>Order Details</strong>
                            <asp:GridView ID="aGrid" runat="server" AutoGenerateColumns="False"
                                BorderStyle="Solid" PageSize="50" Width="300px">
                                <Columns>
                                    <asp:BoundField DataField="ProductId" HeaderText="PUBID" />
                                    <asp:BoundField DataField="LongTitle" HeaderText="Title" />
                                    <asp:BoundField DataField="NumQtyOrdered" HeaderText="Quantity" />
                                </Columns>
                            </asp:GridView>
                            <asp:GridView ID="bGrid" runat="server" AutoGenerateColumns="False"
                                BorderStyle="Solid" PageSize="50" Width="300px">
                                <Columns>
                                    <asp:BoundField DataField="ProductID" HeaderText="PUBID" />
                                    <asp:BoundField DataField="LongTitle" HeaderText="Title" />
                                    <asp:BoundField DataField="NumQtyOrdered" HeaderText="Qty" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </fieldset>
                </div>
            </div>
        </form>
    </div>
</body>
</html>
