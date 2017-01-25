<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="KitPubList.ascx.cs" Inherits="PubEntAdmin.UserControl.KitPubList" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>

<asp:DataGrid ID="gvResult" runat="server" AutoGenerateColumns="False" CssClass="gray-border valuestable"
    Width="100%" AllowSorting="false"  AllowPaging="false"
    PageSize="4" OnItemDataBound="gvResult_ItemDataBound" ShowFooter="false" OnItemCommand="gvResult_ItemCommand"
    UseAccessibleHeader="true">    
    <HeaderStyle CssClass="rowHead" />
    <AlternatingItemStyle CssClass="rowOdd" />
    <ItemStyle CssClass="rowEven" />
    <Columns>
        <asp:BoundColumn Visible="False" DataField="pubid"></asp:BoundColumn>
        <asp:BoundColumn Visible="True" DataField="ProdID" HeaderText="Publication ID" ItemStyle-Width="20%"></asp:BoundColumn>
        <asp:BoundColumn Visible="True" HeaderText="Interface(s)" ItemStyle-Width="50%"></asp:BoundColumn>
        <asp:ButtonColumn Visible="True" Text="Edit" ButtonType="PushButton" CommandName="edit" ItemStyle-Width="7%"></asp:ButtonColumn>
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
                <asp:Button ID="lnkbtnDel" runat="server" ItemStyle-Width="8%" Text="Delete" CommandName="Delete"></asp:Button>
            </ItemTemplate>
        </asp:TemplateColumn>
    </Columns>
</asp:DataGrid>
