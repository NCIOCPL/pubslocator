<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditSubsubcat.ascx.cs"
    Inherits="PubEntAdmin.UserControl.EditSubsubcat" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Panel ID="pnlConfirm" runat="server" CssClass="modalPopup" Style="display: none"
    Width="233px">
    <asp:Label ID="lblConfirm" runat="server" Text="Are you sure you want to delete this SubSubCategory?"></asp:Label>
    <div>
        <asp:Button ID="OkButton" runat="server" Text="OK" />
        <asp:Button ID="CancelButton" runat="server" Text="Cancel" />
    </div>
</asp:Panel>
<cc1:ConfirmButtonExtender ID="confirmBtnExtDel" runat="server" TargetControlID="btnDeleteSubsubcat"
    ConfirmText="" DisplayModalPopupID="modalPopupExtDel">
</cc1:ConfirmButtonExtender>
<cc1:ModalPopupExtender ID="modalPopupExtDel" runat="server" TargetControlID="btnDeleteSubsubcat"
    PopupControlID="pnlConfirm" BackgroundCssClass="modalBackground" DropShadow="true"
    OkControlID="OkButton" CancelControlID="CancelButton">
</cc1:ModalPopupExtender>
<asp:Panel ID="pnlConfirmCancel" runat="server" CssClass="modalPopup" Style="display: none"
    Width="233px">
    <asp:Label ID="lblConfirmCancel" runat="server" Text="Are you sure you want to continue?"></asp:Label>
    <div>
        <asp:Button ID="OkButton2" runat="server" Text="OK" />
        <asp:Button ID="CancelButton2" runat="server" Text="Cancel" />
    </div>
</asp:Panel>
<cc1:ConfirmButtonExtender ID="confirmBtnExtCancel" runat="server" TargetControlID="btnCancelSubsubcat"
    ConfirmText="" DisplayModalPopupID="modalPopupExtCancel">
</cc1:ConfirmButtonExtender>
<cc1:ModalPopupExtender ID="modalPopupExtCancel" runat="server" TargetControlID="btnCancelSubsubcat"
    PopupControlID="pnlConfirmCancel" BackgroundCssClass="modalBackground" DropShadow="true"
    OkControlID="OkButton2" CancelControlID="CancelButton2">
</cc1:ModalPopupExtender>
<asp:Label ID="lblSubsubcatDecrp" runat="server" Text="Subsubcategory Description:" AssociatedControlID="txtSubsubcatDes" CssClass=""></asp:Label>
<asp:TextBox ID="txtSubsubcatDes" runat="server" Columns="80" MaxLength="100"></asp:TextBox>
<asp:Label ID="lblAssign_to_Subcategory" runat="server" Text="Assign to Subcategory:" AssociatedControlID="ddlSubCategory" CssClass=""></asp:Label>
<asp:DropDownList ID="ddlSubCategory" runat="server" AutoPostBack="false">
</asp:DropDownList>
<asp:Button ID="btnAddSubsubcat" runat="server" Text="Save" OnClick="btnAdd_EditSubsubcat_Click" />
<asp:Button ID="btnCancelSubsubcat" runat="server" Text="Cancel" OnClick="btnCancel_EditSubsubcat_Click" />
<asp:Button ID="btnDeleteSubsubcat" runat="server" Text="Delete" OnClick="btnDel_EditSubsubcat_Click" />

