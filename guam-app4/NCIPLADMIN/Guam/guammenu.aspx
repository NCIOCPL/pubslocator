<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="guammenu.aspx.cs" Inherits="PubEntAdmin.Guam.guammenu" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>GUAM Menu</title>
    <link href="../CSS/Common.css" rel="stylesheet" type="text/css" />
</head>
<body class="Admin">
    <div class="skip"><a href="#mainBody">Skip to content</a></div>
    <div id="container">
        <h1>Publications Enterprise Admin Tool</h1>
        <h2> GUAM User Management</h2>
        <form id="cssetupform" runat="server">
            <div id="header">
                <asp:PlaceHolder ID="plcHldMenu" runat="server"></asp:PlaceHolder>
            </div>
            <div id="mainBody">
                <div id="dvSearch">
                    Registered Users
                    <asp:Label ID="Label1" runat="server" Text="Filter"
                        CssClass="" AssociatedControlID="txtFind"></asp:Label>
                    <asp:TextBox ID="txtFind" runat="server"
                        Width="400px" MaxLength="69" ValidationGroup="ValidationSummary1"
                        EnableViewState="False"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                        ErrorMessage="Invalid input. Please use space to separate each Record ID, 
                        if you are searching for multiple Record IDs."
                        ControlToValidate="txtFind" Display="Static"
                        ValidationExpression="^[a-zA-Z0-9 ]{1,69}$" EnableClientScript="False"> * 
                    </asp:RegularExpressionValidator>
                    <asp:Button ID="btnFind" runat="server" Text="Search" OnClick="btnFind_Click" />
                    <asp:GridView ID="grdUsers" runat="server" AllowPaging="True"
                        AllowSorting="True" AutoGenerateColumns="False" PageSize="15"
                        Width="800px" OnPageIndexChanging="PageIndexChanging"
                        EnableModelValidation="True" OnRowDataBound="RowDataBound">
                        <Columns>
                            <asp:BoundField HeaderText="Username" DataField="Email" />
                            <asp:BoundField HeaderText="Role(s)" DataField="Role" />
                            <asp:BoundField DataField="IsActive" HeaderText="Active" />
                            <asp:BoundField DataField="IsEnabled" HeaderText="Enabled" />
                            <asp:BoundField DataField="IsLocked" HeaderText="Locked" />
                            <asp:BoundField DataField="IsPwdExpired" HeaderText="Password Expired" />
                            <asp:TemplateField HeaderText="Action">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEdit" runat="server">Edit</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </form>
    </div>
</body>
</html>
