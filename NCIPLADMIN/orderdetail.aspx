<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="orderdetail.aspx.cs" Inherits="PubEntAdmin.orderdetail" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Order Details</title>
    <link href="Css/Common.css" type="text/css" rel="stylesheet">
</head>
<body class="Admin">
    <div class="skip"><a href="#mainBody">Skip to content</a></div>
    <div id="container">
        <h1>Publications Enterprise Admin Tool</h1>
        <h2> Order Details</h2>
        <form id="cssetupform" runat="server">
            <div id="header">
                <asp:PlaceHolder ID="plcHldMenu" runat="server"></asp:PlaceHolder>
            </div>
            <div id="mainBody">
                <fieldset>
                    <legend class="Legend0">
                        <asp:Label ID="Label2" runat="server" AssociatedControlID="txtOrderNum">Search for Order</asp:Label></legend>
                    <asp:TextBox ID="txtOrderNum" runat="server"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtOrderNum"
                        Display="Dynamic" ErrorMessage="*" ValidationExpression="\d{6,9}">*</asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtOrderNum"
                        ErrorMessage="*" Display="Dynamic">*</asp:RequiredFieldValidator>
                    <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search" />
                    <asp:Label ID="lblSearchMsg" runat="server" ForeColor="#990000"></asp:Label>
                </fieldset>
                <div id="dvSearch">
                    <fieldset>
                        <legend id="legend1" class="Legend1" runat="server">Order details </legend>
                        <div>
                            <asp:Button ID="btnDelete" runat="server" Text="Mark Bad" OnClick="btnDelete_Click" />
                            <asp:Label ID="Label1" runat="server" AssociatedControlID="txtReason">because</asp:Label>
                            <asp:TextBox ID="txtReason" runat="server" MaxLength="100" Width="172px"></asp:TextBox>
                            <asp:Label ID="lblMessage" runat="server" ForeColor="#990000"></asp:Label>
                            <strong>Order ID</strong>
                            <asp:Label ID="bOrderID" runat="server" Font-Bold="True"></asp:Label>
                            <strong>Created</strong>
                            <asp:Label ID="bCreated" runat="server"></asp:Label>
                            <strong>Name</strong>
                            <asp:Label ID="bName" runat="server"></asp:Label>
                            <strong>Organization</strong>
                            <asp:Label ID="bOrg" runat="server"></asp:Label>
                            <strong>Address 1</strong>
                            <asp:Label ID="bAddr1" runat="server"></asp:Label>
                            <strong>Address 2</strong>
                            <asp:Label ID="bAddr2" runat="server"></asp:Label>
                            <strong>ZIP Code</strong>
                            <asp:Label ID="bZip" runat="server"></asp:Label>
                            <strong>City</strong>
                            <asp:Label ID="bCity" runat="server"></asp:Label>
                            <strong>State</strong>
                            <asp:Label ID="bState" runat="server"></asp:Label>
                            <strong>Phone</strong>
                            <asp:Label ID="bPhone" runat="server"></asp:Label>
                            <strong>E-mail</strong>
                            <asp:Label ID="bEmail" runat="server"></asp:Label>
                            <strong>3rd Party?</strong>
                            <asp:Label ID="bShipmethod" runat="server"></asp:Label>
                            <strong>Comment</strong>
                            <asp:Label ID="bComment" runat="server"></asp:Label>
                            <strong>Order Details</strong>
                            <asp:GridView ID="bGrid" runat="server" AutoGenerateColumns="False" BorderStyle="Solid"
                                PageSize="50" Width="400px">
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
