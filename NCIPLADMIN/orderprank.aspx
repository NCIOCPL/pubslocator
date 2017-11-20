<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="orderprank.aspx.cs" Inherits="PubEntAdmin.orderprank" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Prank Orders</title>
    <link href="Css/Common.css" type="text/css" rel="stylesheet">
</head>
<body class="Admin">
    <div class="skip"><a href="#mainBody">Skip to content</a></div>
    <div id="container">
        <h1>Publications Enterprise Admin Tool</h1>
        <h2> Prank Orders on Hold</h2>
        <form id="cssetupform" runat="server">
            <div id="header">
                <asp:PlaceHolder ID="plcHldMenu" runat="server"></asp:PlaceHolder>
            </div>
            <div id="mainBody">
                <div id="dvSearch">
                    <fieldset>
                        <legend class="Legend1">Prank Orders</legend>
                        <asp:Label ID="Label1" runat="server" Text="Filter"
                            CssClass="" AssociatedControlID="txtFind"
                            Visible="False"></asp:Label>
                        <asp:TextBox ID="txtFind" runat="server"
                            Width="400px" MaxLength="69" ValidationGroup="ValidationSummary1"
                            EnableViewState="False" Visible="False"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                            ErrorMessage="Invalid input. Please use space to separate each Record ID, if you are searching for multiple Record IDs."
                            ControlToValidate="txtFind" Display="Static"
                            ValidationExpression="^[a-zA-Z0-9 ]{1,69}$" EnableClientScript="False"> * </asp:RegularExpressionValidator>
                        <asp:Button ID="btnFind" runat="server" Text="Search" OnClick="btnFind_Click"
                            Visible="False" />
                        <asp:GridView ID="grdUsers" runat="server" AllowPaging="True"
                            AllowSorting="True" AutoGenerateColumns="False" PageSize="50"
                            Width="800px" OnPageIndexChanging="PageIndexChanging"
                            EnableModelValidation="True" OnRowDataBound="RowDataBound">
                            <Columns>
                                <asp:BoundField HeaderText="Order ID" DataField="OrderID" />
                                <asp:BoundField HeaderText="Name" DataField="ShipToName" />
                                <asp:TemplateField HeaderText="Address1">
                                    <ItemTemplate>
                                        <%# Eval("ShipTo.Addr1")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="City">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <%# Eval("ShipTo.City")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="State">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <%# Eval("ShipTo.State")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Zip5">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <%# Eval("ShipTo.Zip5")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Created">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <%# Eval("DateCreated")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDetails" runat="server">Details</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </fieldset>
                </div>
            </div>
        </form>
    </div>
</body>
</html>
