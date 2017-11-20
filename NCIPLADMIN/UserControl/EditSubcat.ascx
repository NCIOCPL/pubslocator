<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditSubcat.ascx.cs"
    Inherits="PubEntAdmin.UserControl.EditSubcat" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Panel ID="pnlConfirm" runat="server" CssClass="modalPopup" Style="display: none" Width="233px">
    <asp:Label ID="lblConfirm" runat="server" Text="Are you sure you want to delete this SubCategory?<br />NOTE: ALL the SubSubCategories assigned to it will also be deleted."></asp:Label>
    <div>
        <asp:Button ID="OkButton" runat="server" Text="OK" />
        <asp:Button ID="CancelButton" runat="server" Text="Cancel" />
    </div>
</asp:Panel>
<cc1:ConfirmButtonExtender ID="confirmBtnExtDel" runat="server" TargetControlID="btnDeleteSubcat"
    ConfirmText="" DisplayModalPopupID="modalPopupExtDel">
</cc1:ConfirmButtonExtender>
<cc1:ModalPopupExtender ID="modalPopupExtDel" runat="server" TargetControlID="btnDeleteSubcat"
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
<cc1:ConfirmButtonExtender ID="confirmBtnExtCancel" runat="server" TargetControlID="btnCancelSubcat"
    ConfirmText="" DisplayModalPopupID="modalPopupExtCancel">
</cc1:ConfirmButtonExtender>
<cc1:ModalPopupExtender ID="modalPopupExtCancel" runat="server" TargetControlID="btnCancelSubcat"
    PopupControlID="pnlConfirmCancel" BackgroundCssClass="modalBackground" DropShadow="true"
    OkControlID="OkButton2" CancelControlID="CancelButton2">
</cc1:ModalPopupExtender>
<asp:Label ID="lblSubcatDecrp" runat="server" Text="Subcategory Description:" AssociatedControlID="txtSubcatDes" CssClass=""></asp:Label>
<asp:TextBox ID="txtSubcatDes" runat="server" Columns="80" MaxLength="100"></asp:TextBox>
<asp:Label ID="lblAssign_to_Cat" runat="server" AssociatedControlID="ddlCategory" Text="Assign to category:" CssClass=""></asp:Label>
<asp:DropDownList ID="ddlCategory" runat="server">
</asp:DropDownList>
<asp:CheckBox ID="ckboxAllowSubsubcat" runat="server" Text="Allow Subsubcategories" CssClass="" />
<asp:Button ID="btnAddSubcat" runat="server" Text="Save" OnClick="btnAdd_EditSubcat_Click" />
<asp:Button ID="btnCancelSubcat" runat="server" Text="Cancel" OnClick="btnCancel_EditSubcat_Click" />
<asp:Button ID="btnDeleteSubcat" runat="server" Text="Delete" OnClick="btnDel_EditSubcat_Click" />
<asp:Label ID="lblAlert" runat="server" Text=""></asp:Label>
<table>
    <tr id="trEditSubsubcat" runat="server">
        <td>
            <asp:Button ID="btnAddSubsubcat" runat="server" Text="Add SubSubCat"
                OnClick="btnAddSubsubcat_Click" />
            <asp:Button ID="btnEditSubsubcat" runat="server" Text="Edit SubSubCat" OnClick="btnEditSubsubcat_Click" />
        </td>
    </tr>
</table>
