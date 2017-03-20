<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KitPub.aspx.cs" Inherits="PubEntAdmin.KitPub"
    ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Reference Page="DisplayKitPub.aspx" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>Update/Create Virtual Publication or Linked Publication</title>
    <link href="CSS/Common.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        window.onload = KitPubOnload;
        var checkRemLKReminder = 0;
        function KitPubOnload() {
            checkRemLKReminder++;
        }
        function getPubTitle() {
            __doPostBack('<%= this.btnGetKitPubTitle.UniqueID %>', '');
        }
    </script>
    <noscript>
        This site requires Javascript. Please enable Javascript in your browser or switch to a browser 
			that supports Javascript in order to view this site.
    </noscript>
</head>
<body class="Admin">
    <div class="skip"><a href="#mainBody">Skip to content</a></div>
    <div id="container">
        <h1>Publications Enterprise Admin Tool</h1>
        <h2>
            <asp:Label ID="lblPageTitle" runat="server" Text=""></asp:Label></h2>
        <form id="formKits" runat="server">
            <asp:ScriptManager ID="ScriptManager_KitPub" EnableScriptGlobalization="false" runat="Server">
            </asp:ScriptManager>
            <asp:PlaceHolder ID="plcHldMenu" runat="server"></asp:PlaceHolder>
            <div id="mainBody">
                <input id="btnGetKitPubTitle" runat="server" style="display: none;" type="button"
                    onserverclick="btnGetKitPubTitle_Click" />
                <asp:UpdatePanel ID="udpnlNewPub" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblInstruction" runat="server" Text=""></asp:Label>
                        <strong>Publication Title:</strong>
                        <asp:Label ID="lblTitle" runat="server"></asp:Label>
                        <asp:Label ID="lblMsg" runat="server" CssClass="error"></asp:Label>
                        <asp:Label ID="lblPubID" runat="server" Text="Publication ID" CssClass=""></asp:Label>
                        <asp:Label ID="lblPubTitle" runat="server" Text="Title" CssClass=""></asp:Label>
                        <asp:Label ID="lblQty" runat="server" Text="Qty" CssClass=""></asp:Label>
                        <strong>
                            <asp:Label ID="lblRemv" runat="server" Text="Remove"></asp:Label></strong>
                        <asp:TextBox ID="txtNewPub" runat="server" MaxLength="10"></asp:TextBox><br />
                        Press the TAB key after entering the publication ID to verify the publication title.                                
                                    <asp:Label ID="lblNewTitle" runat="server" Text=""></asp:Label>
                        <asp:TextBox ID="txtNewQty" runat="server" Columns="5" MaxLength="8" Text=""></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="ftbeNewQty" runat="server" TargetControlID="txtNewQty"
                            ValidChars="0123456789">
                        </cc1:FilteredTextBoxExtender>
                        <asp:DataGrid ID="gvResult" runat="server" AutoGenerateColumns="False" 
                            Width="100%" AllowSorting="True" HorizontalAlign="Center" AllowPaging="false"
                            PageSize="4" ShowFooter="false" ShowHeader="false" OnItemDataBound="gvResult_ItemDataBound"
                            OnItemCreated="gvResult_ItemCreated" CssClass="gray-border valuestable" UseAccessibleHeader="true">
                            <HeaderStyle CssClass="rowHead" />
                            <AlternatingItemStyle CssClass="rowOdd" />
                            <ItemStyle CssClass="rowEven" />
                            <Columns>
                                <asp:BoundColumn Visible="false" DataField="pubid"></asp:BoundColumn>
                                <asp:BoundColumn Visible="True" DataField="prodid" HeaderText="Publication ID" ItemStyle-Width="20%"></asp:BoundColumn>
                                <asp:BoundColumn Visible="True" DataField="PubName" HeaderText="Pub Title" ItemStyle-Width="51%"
                                    ItemStyle-HorizontalAlign="Left"></asp:BoundColumn>
                                <asp:TemplateColumn ItemStyle-Width="4%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtQty" runat="server" Columns="5" MaxLength="8"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="ftbeQty" runat="server" TargetControlID="txtQty"
                                            ValidChars="0123456789">
                                        </cc1:FilteredTextBoxExtender>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkboxDel" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                            </Columns>
                        </asp:DataGrid>
                        <asp:Button ID="btnAddNew" runat="server" Text="Add New" OnClick="btnAddNew_Click" />
                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGetKitPubTitle" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </form>
    </div>
</body>
</html>
