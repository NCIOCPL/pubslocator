<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RelatedTabEditInfo.ascx.cs"
    Inherits="PubEntAdmin.UserControl.RelatedTabEditInfo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<asp:DataGrid ID="gvResult" runat="server" AutoGenerateColumns="False" CssClass="gray-border valuestable"
    Width="100%" AllowSorting="True" AllowPaging="false"
    PageSize="4" ShowFooter="false" ShowHeader="true" OnItemDataBound="gvResult_ItemDataBound"
    OnItemCommand="gvResult_ItemCommand">
    <HeaderStyle CssClass="rowHead" />
    <AlternatingItemStyle CssClass="rowOdd" />
    <ItemStyle CssClass="rowEven" />
    <Columns>
        <asp:BoundColumn Visible="False" DataField="PubID"></asp:BoundColumn>
        <asp:BoundColumn HeaderText="Publication ID" ItemStyle-Width="20%" DataField="ProdID"></asp:BoundColumn>
        <asp:BoundColumn HeaderText="Publication Title" ItemStyle-Width="60%" DataField="PubName"></asp:BoundColumn>
        <asp:TemplateColumn>
            <HeaderTemplate>
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Panel ID="pnlConfirm" runat="server" CssClass="modalPopup" Style="display: none"
                    Width="233px">
                    <asp:Label ID="lblConfirm" runat="server" Text=""></asp:Label>
                    <div>
                        <asp:Button ID="OkButton" runat="server" Text="OK" />
                        <asp:Button ID="CancelButton" runat="server" Text="Cancel" />
                    </div>
                </asp:Panel>
                <cc2:ConfirmButtonExtender ID="confirmBtnExtDel" runat="server" TargetControlID="lnkbtnDel"
                    ConfirmText="" DisplayModalPopupID="modalPopupExtDel">
                </cc2:ConfirmButtonExtender>
                <cc2:ModalPopupExtender ID="modalPopupExtDel" runat="server" TargetControlID="lnkbtnDel"
                    PopupControlID="pnlConfirm" BackgroundCssClass="modalBackground" DropShadow="true"
                    OkControlID="OkButton" CancelControlID="CancelButton">
                </cc2:ModalPopupExtender>
                <asp:LinkButton ID="lnkbtnDel" runat="server" Text="Delete"></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateColumn>
    </Columns>
</asp:DataGrid>
<table>
    <tr id="trRelated" runat="server">
        <td class="">
            <div class="editboxrow">
                <div class="editbox">
                    <asp:Label ID="lblRelPubID" runat="server" Text="Related Publication ID " CssClass=""></asp:Label>
                    <asp:TextBox ID="txtRelated" runat="server" Columns="10" MaxLength="10"></asp:TextBox>
                    <asp:Button ID="btnRelatedTabInfo" runat="server" Text="Add" OnClick="btnRelatedTabInfo_Click" Visible="false" />
                    <asp:Label ID="lblErrRelProd" runat="server" Text=""></asp:Label>
                </div>
            </div>
        </td>
    </tr>
</table>
