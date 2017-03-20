<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Subject.ascx.cs" Inherits="PubEntAdmin.UserControl.Subject" %>
<%@ Register Src="LiveIntSel.ascx" TagName="LiveIntSel" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Label ID="lblMsg" runat="server" Text="" CssClass="errorText"></asp:Label>
<h3>Add New Subject</h3>
<div class="editboxtable subj">
    <div class="editboxseries">
        <asp:Label ID="lblSubjDescp" runat="server" CssClass="lbl" Text="Subject Description" AssociatedControlID="txtSubjName"></asp:Label>
        <asp:TextBox ID="txtSubjName" runat="server" CssClass="txtfld" MaxLength="50"></asp:TextBox>
    </div>
    <div class="editboxseries">
        <asp:Label ID="lblHsSubcat" runat="server" Text="Has SubCategory" CssClass="lbl" AssociatedControlID="chboxSubj"></asp:Label>
        <asp:CheckBox ID="chboxSubj" runat="server" />
    </div>
</div>
<div class="editboxtable subj">
    <div class="editboxseries">
        <asp:Label ID="lblLivInt" runat="server" Text="Live Interfaces" CssClass="lbl"></asp:Label>
        <uc1:LiveIntSel ID="LiveIntSelSubj" runat="server" RepeatDirection="Horizontal" />
    </div>
    <div class="editboxseries lookupbtn">
        <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn" OnClick="btnAdd_Click" />
    </div>
</div>
<h3>Edit Existing Subject</h3>
<asp:DataGrid ID="gvResult" runat="server" AutoGenerateColumns="False"
    GridLines="Horizontal" CssClass="valuestable gray-border"
    Width="100%" AllowSorting="True" AllowPaging="false"
    PageSize="4" OnItemDataBound="gvResult_ItemDataBound" ShowFooter="false" ShowHeader="true"
    OnItemCommand="gvResult_ItemCommand" OnSortCommand="gvResult_SortCommand"
    OnEditCommand="gvResult_EditCommand"
    OnCancelCommand="gvResult_CancelCommand" OnUpdateCommand="gvResult_UpdateCommand"
    OnItemCreated="gvResult_ItemCreated" UseAccessibleHeader="true">
    <FooterStyle BackColor="White" Font-Bold="True" ForeColor="Black" Font-Size="Small"
        VerticalAlign="Top" />
    <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
    <SelectedItemStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
    <HeaderStyle CssClass="rowHead" />
    <AlternatingItemStyle CssClass="rowOdd" />
    <ItemStyle CssClass="rowEven" />
    <Columns>
        <asp:BoundColumn Visible="false" DataField="CannotRem"></asp:BoundColumn>
        <asp:BoundColumn Visible="False" DataField="SubjID"></asp:BoundColumn>
        <asp:TemplateColumn HeaderText="Subject Description">
            <EditItemTemplate>
                <asp:TextBox ID="txtSubjName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.SubjName")%>' MaxLength="50"></asp:TextBox>
            </EditItemTemplate>
            <ItemTemplate>
                <asp:Label ID="lblSubjName" runat="server" Text=""></asp:Label>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="Live Interfaces">
            <EditItemTemplate>
                <uc1:LiveIntSel ID="LiveIntSel2" runat="server" />
            </EditItemTemplate>
            <ItemTemplate>
                <asp:Label ID="lblLiveIntName2" runat="server" Text=""></asp:Label>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="Has SubCategory">
            <EditItemTemplate>
                <asp:CheckBox ID="ckboxHasSubCat" runat="server" Text="" />
            </EditItemTemplate>
            <ItemTemplate>
                <asp:Label ID="lblHasSubCat" runat="server" Text=""></asp:Label>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="Status">
            <EditItemTemplate>
                <asp:Label ID="lblStatus2" runat="server" Text=""></asp:Label>
            </EditItemTemplate>
            <ItemTemplate>
                <asp:Label ID="lblStatus1" runat="server" Text=""></asp:Label>
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
                        <asp:Button ID="OkButton" runat="server" CssClass="btn" Text="OK" />
                        <asp:Button ID="CancelButton" runat="server" CssClass="btn" Text="Cancel" />
                    </div>
                </asp:Panel>
                <cc1:ConfirmButtonExtender ID="confirmBtnExtDel" runat="server" TargetControlID="lnkbtnDel"
                    ConfirmText="" DisplayModalPopupID="modalPopupExtDel">
                </cc1:ConfirmButtonExtender>
                <cc1:ModalPopupExtender ID="modalPopupExtDel" runat="server" TargetControlID="lnkbtnDel"
                    PopupControlID="pnlConfirmDel" BackgroundCssClass="modalBackground" DropShadow="true"
                    OkControlID="OkButton" CancelControlID="CancelButton">
                </cc1:ModalPopupExtender>
                <asp:Button ID="lnkbtnDel" runat="server" Text="" CssClass="btn"></asp:Button>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
            <ItemTemplate>
                <asp:CustomValidator ID="CustValDisable" Display="Dynamic" runat="server"
                    ErrorMessage="Unable to inactivate, value associated with publication."></asp:CustomValidator>
            </ItemTemplate>
        </asp:TemplateColumn>
    </Columns>
</asp:DataGrid>

