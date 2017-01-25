<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="donotmail.aspx.cs" MaintainScrollPositionOnPostback="True" Inherits="PubEntAdmin.donotmail" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Do-Not-Mail</title>
    <link href="Css/Common.css" type="text/css" rel="stylesheet">
</head>
<body class="Admin">
    <div class="skip"><a href="#mainBody">Skip to content</a></div>
    <div id="container">
        <h1>Publications Enterprise Admin Tool</h1>
        <h2> Do-Not-Mail List Management</h2>
        <form id="cssetupform" runat="server">
            <div id="header">
                <asp:PlaceHolder ID="plcHldMenu" runat="server"></asp:PlaceHolder>
            </div>
            <div id="mainBody">
                <div id="dvSearch">
                    <fieldset>
                        <legend class="Legend0">Adding a New Entry</legend>
                        <asp:Label ID="Label1" runat="server" AssociatedControlID="txtAddress">Address with ZIP</asp:Label>
                        <asp:Label ID="Label3" runat="server" AssociatedControlID="txtZip">- or- </asp:Label>
                        <asp:Label ID="Label2" runat="server" AssociatedControlID="txtIPAddress">IP Address</asp:Label>
                        <asp:TextBox ID="txtAddress" runat="server" MaxLength="100" Width="212px"></asp:TextBox>
                        <asp:TextBox ID="txtZip" runat="server" MaxLength="20" Width="78px"></asp:TextBox>
                        <asp:RangeValidator ID="RangeValidator1" runat="server"
                            ControlToValidate="txtZip" ErrorMessage="RangeValidator" MaximumValue="99999"
                            MinimumValue="00000" Type="Integer">Invalid ZIP Code</asp:RangeValidator>
                        <asp:TextBox ID="txtIPAddress" runat="server" MaxLength="20"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1"
                            runat="server" ControlToValidate="txtIPAddress"
                            ErrorMessage="RegularExpressionValidator"
                            ValidationExpression="\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}">Invalid IP Address</asp:RegularExpressionValidator>
                        <asp:Button ID="btnAdd" runat="server"
                            Text="Add to Do-Not-Mail List" OnClick="btnAdd_Click" /><asp:Label
                                ID="lblMessage" runat="server" ForeColor="#990000"></asp:Label>
                    </fieldset>
                    <fieldset>
                        <legend class="Legend1">Do-Not-Mail List</legend>
                        <div>
                            <asp:Label
                                ID="lblMessage2" runat="server" ForeColor="#990000"></asp:Label>
                            <asp:GridView ID="grdUsers" runat="server" AllowPaging="True"
                                AllowSorting="True" AutoGenerateColumns="False" PageSize="50"
                                Width="800px" OnPageIndexChanging="PageIndexChanging"
                                EnableModelValidation="True" OnRowDataBound="RowDataBound"
                                OnRowCancelingEdit="grdUsers_Cancel" OnRowDeleting="grdUsers_Deleting"
                                OnRowEditing="grdUsers_RowEditing" OnRowUpdating="grdUsers_RowUpdating">
                                <Columns>
                                    <asp:TemplateField HeaderText="ID">
                                        <EditItemTemplate>
                                            <asp:Label ID="lblID" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblID" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Address">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtAddress" runat="server" MaxLength="100"
                                                Text='<%# Bind("Addr1") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblAddress" runat="server" Text='<%# Bind("Addr1") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ZIP">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtZip" runat="server" MaxLength="5"
                                                Text='<%# Bind("Zip5") %>'></asp:TextBox>
                                            <asp:RangeValidator ID="RangeValidator1" runat="server"
                                                ControlToValidate="txtZip" ErrorMessage="RangeValidator" MaximumValue="99999"
                                                MinimumValue="00000" Type="Integer" Display="Dynamic">Invalid ZIP Code</asp:RangeValidator>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblZip" runat="server" Text='<%# Bind("Zip5") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="IP Address">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtIPAddress" runat="server" Text='<%# Bind("Addr2") %>'></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1"
                                                runat="server" ControlToValidate="txtIPAddress"
                                                ErrorMessage="RegularExpressionValidator"
                                                ValidationExpression="\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}"
                                                Display="Dynamic">Invalid IP Address</asp:RegularExpressionValidator>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblIPAddress" runat="server" Text='<%# Bind("Addr2") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="False">
                                        <EditItemTemplate>
                                            <asp:Button ID="Button1" runat="server" CausesValidation="True"
                                                CommandName="Update" Text="Update" />
                                            <asp:Button ID="Button2" runat="server" CausesValidation="False"
                                                CommandName="Cancel" Text="Cancel" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Button ID="Button1" runat="server" CausesValidation="False"
                                                CommandName="Edit" Text="Edit" />
                                            <asp:Button ID="Button2" runat="server" CausesValidation="False"
                                                CommandName="Delete" Text="Delete" OnClientClick="return confirm('Are you sure you want to delete this?');" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
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
