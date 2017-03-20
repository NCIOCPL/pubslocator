<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FeaturedPubs.ascx.cs" Inherits="PubEntAdmin.UserControl.FeaturedPubs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<span class="headFeature">Find Existing Stack</span>
<asp:Label ID="Label1" runat="server" Text="Search:" AssociatedControlID="txtSearch" CssClass=""></asp:Label>
<asp:Label ID="Label2" runat="server" Text="Active (Live):" AssociatedControlID="chkActive" CssClass=""></asp:Label>
<asp:TextBox ID="txtSearch" runat="server" Width="58%" MaxLength="50"></asp:TextBox>
<cc1:TextBoxWatermarkExtender ID="SearchTextBoxWatermarkExtender"
    runat="server" TargetControlID="txtSearch"
    WatermarkText="[All]"
    WatermarkCssClass="textGray">
</cc1:TextBoxWatermarkExtender>
The search will target the following fields: Stack Title, Selected Publications                                     (Pub IDs)
                                    <asp:CheckBox ID="chkActive" runat="server" />
<asp:Button ID="btnSearch" runat="server"
    Text="Search" CausesValidation="false" OnClick="btnSearch_Click" />
<!--End Search Area-->
<span class="headFeature">Add New Stack</span>
<asp:Panel ID="pnlAdd" runat="server" CssClass="gray-border">
    <asp:Label ID="Label3" runat="server" Text="Stack Title *:" AssociatedControlID="txtTitle" CssClass=""></asp:Label>

    <asp:TextBox ID="txtTitle" runat="server" MaxLength="50" Width="43%"></asp:TextBox><asp:RequiredFieldValidator ID="ReqFieldValTitle" runat="server"
        ErrorMessage="Please provide a title for the stack." ControlToValidate="txtTitle"></asp:RequiredFieldValidator>
    <asp:Label ID="Label4" runat="server" Text="Sequence:" AssociatedControlID="txtSequence" CssClass=""></asp:Label>
    <asp:TextBox ID="txtSequence" runat="server" MaxLength="3" Width="5%"></asp:TextBox>
    <asp:Button ID="btnAdd" runat="server" Text="Add"
        OnClick="btnAdd_Click" />
    <asp:Button ID="btnCancel" runat="server" Text="Cancel"
        CausesValidation="false" OnClick="btnCancel_Click" />
</asp:Panel>
<asp:Label ID="lblMessage" runat="server" Text="" Visible="false" EnableViewState="false" CssClass=""></asp:Label>
<!--Begin Stack Table Area-->
<asp:Panel ID="gridPanel" runat="server" Visible="false">
    <span class="headFeature">Edit Existing Stack</span>
    <asp:GridView ID="gvStacks" runat="server"
        AutoGenerateColumns="False" CssClass="gray-border valuestable" Width="100%" 
        OnRowCommand="gvStacks_RowCommand" OnRowDataBound="gvStacks_RowDataBound">
        <HeaderStyle CssClass="rowHead" />
        <AlternatingRowStyle CssClass="rowOdd" />
        <RowStyle CssClass="rowEven" />
        <Columns>
            <asp:TemplateField HeaderText="Stack Title" ItemStyle-Width="35%" HeaderStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:HiddenField ID="hidStackId" runat="server" />
                    <asp:TextBox ID="txtStackTitle" runat="server" MaxLength="50" Width="95%"></asp:TextBox>
                    <asp:Label ID="lblStackTitle" runat="server" Width="95%" Visible="false"></asp:Label>
                </ItemTemplate>
                <ItemStyle Width="35%" />
            </asp:TemplateField>
            <asp:BoundField DataField="StackProdIds" HeaderText="Selected Publications"
                ItemStyle-Width="15%" HeaderStyle-HorizontalAlign="Left">
                <ItemStyle Width="15%" />
            </asp:BoundField>
            <asp:BoundField DataField="StackStatusText" HeaderText="Status"
                ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                <ItemStyle Width="5%" />
            </asp:BoundField>
            <asp:TemplateField ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:Button ID="btnChangeStatus" runat="server" Text="Change Status" CausesValidation="false" />
                    <cc1:ConfirmButtonExtender ID="btnChangeStatus_ConfirmButtonExtender"
                        runat="server" ConfirmText="" Enabled="True" TargetControlID="btnChangeStatus" DisplayModalPopupID="btnChangeStatus_ModalPopupExtender">
                    </cc1:ConfirmButtonExtender>
                    <cc1:ModalPopupExtender ID="btnChangeStatus_ModalPopupExtender" runat="server"
                        DynamicServicePath="" Enabled="True" TargetControlID="btnChangeStatus"
                        PopupControlID="pnlConfirmA" BackgroundCssClass="modalBackground" DropShadow="true"
                        OkControlID="OkButtonA" CancelControlID="CancelButtonA">
                    </cc1:ModalPopupExtender>
                    <!--Begin Modal Popup Area-->
                    <asp:Panel ID="pnlConfirmA" runat="server" CssClass="modalPopup" Style="display: none">
                        <asp:Label ID="lblConfirmA" runat="server" Text=""></asp:Label>
                        <div>
                            <asp:Button ID="OkButtonA" runat="server" Text="OK" CausesValidation="false" />
                            <asp:Button ID="CancelButtonA" runat="server" Text="Cancel" CausesValidation="false" />
                        </div>
                    </asp:Panel>
                    <!--End Modal Popup Area-->
                    <asp:Button ID="btnDelete" runat="server" Text="Delete" CausesValidation="false" />
                    <cc1:ConfirmButtonExtender ID="btnDelete_ConfirmButtonExtender" runat="server"
                        ConfirmText="" Enabled="True" TargetControlID="btnDelete" DisplayModalPopupID="btnDelete_ModalPopupExtender">
                    </cc1:ConfirmButtonExtender>
                    <cc1:ModalPopupExtender ID="btnDelete_ModalPopupExtender" runat="server"
                        DynamicServicePath="" Enabled="True" TargetControlID="btnDelete"
                        PopupControlID="pnlConfirmDel" BackgroundCssClass="modalBackground" DropShadow="true"
                        OkControlID="OkButton" CancelControlID="CancelButton">
                    </cc1:ModalPopupExtender>
                    <!--Begin Modal Popup Area-->
                    <asp:Panel ID="pnlConfirmDel" runat="server" CssClass="modalPopup" Style="display: none">
                        <asp:Label ID="lblConfirmDel" runat="server" Text=""></asp:Label>
                        <div>
                            <asp:Button ID="OkButton" runat="server" Text="OK" CausesValidation="false" />
                            <asp:Button ID="CancelButton" runat="server" Text="Cancel" CausesValidation="false" />
                        </div>
                    </asp:Panel>
                    <!--End Modal Popup Area-->
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" Width="20%" />
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-Width="15%">
                <ItemTemplate>
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Text=""></asp:Label>
                </ItemTemplate>
                <ItemStyle Width="15%" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Sequence" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:TextBox ID="txtStackSequence" runat="server" MaxLength="3" Width="30%" Style="text-align: right"></asp:TextBox>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" Width="10%" />
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:Button ID="btnCancelChanges" runat="server" Text="Cancel Changes"
        CausesValidation="false" OnClick="btnCancelChanges_Click" />
    <asp:Button ID="btnSaveChanges" runat="server" Text="Save Changes" CausesValidation="false"
        OnClick="btnSaveChanges_Click" />
</asp:Panel>
