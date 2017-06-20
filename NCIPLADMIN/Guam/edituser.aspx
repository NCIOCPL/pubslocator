<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="edituser.aspx.cs" Inherits="PubEntAdmin.Guam.edituser" %>

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
        <h2>GUAM User Management</h2>
        <form id="cssetupform" runat="server">
            <div id="header">
                <asp:PlaceHolder ID="plcHldMenu" runat="server"></asp:PlaceHolder>
            </div>
            <div id="mainBody">
                <div id="dvSearch">
                    Registered User                                       
                    <asp:Label ID="lblMessage" runat="server" ForeColor="#990000"></asp:Label>
                    <asp:Button ID="btnSave1" runat="server" OnClick="btnSave1_Click" Text="Save" />
                    <table>
                        <tr>
                            <td>Username</td>
                            <td>
                                <asp:Label ID="lblUsername" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label3" runat="server" AssociatedControlID="CheckBox1">Active</asp:Label></td>
                            <td>
                                <asp:CheckBox ID="CheckBox1" runat="server" Enabled="False" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" AssociatedControlID="CheckBox2">Enabled</asp:Label></td>
                            <td>
                                <asp:CheckBox ID="CheckBox2" runat="server" Enabled="False" />
                            </td>

                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label2" runat="server" AssociatedControlID="CheckBox3">Locked</asp:Label></td>
                            <td>
                                <asp:CheckBox ID="CheckBox3" runat="server" Enabled="False" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label4" runat="server" AssociatedControlID="CheckBox4">Password Expired</asp:Label></td>
                            <td>
                                <asp:CheckBox ID="CheckBox4" runat="server" Enabled="False" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label5" runat="server" AssociatedControlID="drpRoles">NCIPL Role</asp:Label></td>
                            <td>
                                <asp:DropDownList ID="drpRoles" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <asp:Button ID="btnSave2" runat="server" OnClick="btnSave2_Click" Text="Save" />
                </div>
            </div>
        </form>
    </div>
</body>
</html>
