<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductFormat.ascx.cs" Inherits="PubEntAdmin.UserControl.ProductFormat" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<script type="text/javascript">
    function showSpellChk() {
        document.getElementById('<%= this.ParentSpellCkrID1 %>').style.display = '';
        document.getElementById('<%= this.ParentSpellCkrID2 %>').style.display = '';
    }
    function hideSpellChk() {
        document.getElementById('<%= this.ParentSpellCkrID1 %>').style.display = 'none';
        document.getElementById('<%= this.ParentSpellCkrID2 %>').style.display = 'none';
    }
</script>
<h3>Add New Publication Format</h3>
<div class="lookupnew">
    <div class="lookupfld">
        <asp:Label runat="server" AssociatedControlID="txtPftName">Format Description</asp:Label>
        <asp:TextBox ID="txtPftName" runat="server"></asp:TextBox>
    </div>
    <div class="lookupbtn">
        <asp:Button ID="btnAdd" runat="server" CssClass="btn" Text="Add" OnClick="btnAdd_Click" />
        <asp:Label ID="Message" runat="server" CssClass="errorText" Visible="False"></asp:Label>
    </div>
</div>
<h3>Edit Existing Publication Format</h3>
<asp:DataGrid ID="gvResult" runat="server" AutoGenerateColumns="False"
    GridLines="Horizontal" BackColor="White" CssClass="gray-border valuestable formattbl"
    AllowSorting="True" HorizontalAlign="Center" Width="100%"
    PageSize="4" OnItemDataBound="gvResult_ItemDataBound"
    OnItemCommand="gvResult_ItemCommand" OnSortCommand="gvResult_SortCommand" OnEditCommand="gvResult_EditCommand"
    OnCancelCommand="gvResult_CancelCommand"
    OnUpdateCommand="gvResult_UpdateCommand"
    OnDeleteCommand="gvResult_DeleteCommand">
    <FooterStyle BackColor="White" Font-Bold="True" ForeColor="Black" Font-Size="Small"
        VerticalAlign="Top" />
    <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
    <SelectedItemStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
    <HeaderStyle CssClass="rowHead" />
    <AlternatingItemStyle CssClass="rowOdd" />
    <ItemStyle CssClass="rowEven" />
    <Columns>
        <asp:BoundColumn Visible="False" DataField="PftID"></asp:BoundColumn>
        <asp:TemplateColumn HeaderText="Publication Format Description" HeaderStyle-CssClass="formatcol" ItemStyle-CssClass="formatcol">
            <EditItemTemplate>
                <asp:TextBox ID="txtPftName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.PftName")%>'></asp:TextBox>
            </EditItemTemplate>
            <ItemTemplate>
                <asp:Label ID="lblPftName" runat="server" Text=""></asp:Label>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="State Enabled" >
            <EditItemTemplate>
                <asp:Label ID="lblChecked1" runat="server" Text=""></asp:Label>
            </EditItemTemplate>
            <ItemTemplate>
                <asp:Label ID="lblChecked" runat="server" Text=""></asp:Label>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:EditCommandColumn ButtonType="PushButton" UpdateText="Update" CancelText="Cancel"
            EditText="Edit">
            <ItemStyle CssClass="btnparent"></ItemStyle>
        </asp:EditCommandColumn>
        <asp:TemplateColumn>
            <HeaderTemplate>
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Panel ID="pnlConfirmDel" runat="server" CssClass="modalPopup" Style="display: none"
                    Width="233px">
                    <asp:Label ID="lblConfirmDel" runat="server" Text=""></asp:Label>
                    <div>
                        <asp:Button ID="OkButton" runat="server" Text="OK" />
                        <asp:Button ID="CancelButton" runat="server" Text="Cancel" />
                    </div>
                </asp:Panel>
                <cc1:ConfirmButtonExtender ID="confirmBtnExtDel" runat="server" TargetControlID="lnkbtnDel"
                    ConfirmText="" DisplayModalPopupID="modalPopupExtDel">
                </cc1:ConfirmButtonExtender>
                <cc1:ModalPopupExtender ID="modalPopupExtDel" runat="server" TargetControlID="lnkbtnDel"
                    PopupControlID="pnlConfirmDel" BackgroundCssClass="modalBackground" DropShadow="true"
                    OkControlID="OkButton" CancelControlID="CancelButton">
                </cc1:ModalPopupExtender>
                <asp:Button ID="lnkbtnDel" runat="server" CssClass="btn" Text=""></asp:Button>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <ItemTemplate>
                <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Maroon"></asp:Label>
            </ItemTemplate>
        </asp:TemplateColumn>
    </Columns>
</asp:DataGrid>
