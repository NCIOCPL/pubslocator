<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Owner.ascx.cs" Inherits="PubEntAdmin.UserControl.Owner" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Label ID="lblMessage" runat="server" CssClass="errorText"></asp:Label>
<h3>Add New Owner</h3>
<div class="editboxrow">
    <div class="editbox">
        <asp:Label runat="server" AssociatedControlID="txtFName" Text="Owner First Name *"></asp:Label>
        <asp:TextBox ID="txtFName" runat="server" Rows="400" MaxLength="20"></asp:TextBox>
        <asp:Label ID="lblErFname" runat="server" CssClass="error" Font-Size="Small"></asp:Label>
    </div>
    <div class="editbox">
        <asp:Label runat="server" AssociatedControlID="txtLName" Text="Owner Last Name *"></asp:Label>
        <asp:TextBox ID="txtLName" runat="server" Rows="400" MaxLength="20"></asp:TextBox>
        <asp:Label ID="lblErLname" runat="server" CssClass="error" Font-Size="Small"></asp:Label>
    </div>
    <div class="editbox">
        <asp:Label runat="server" AssociatedControlID="txtMInit" Text="Owner Middle Initial"></asp:Label>
        <asp:TextBox ID="txtMInit" runat="server" MaxLength="1"></asp:TextBox>
    </div>
    <div class="editbox lookupbtn">
        <asp:Button ID="btnAdd" runat="server" CssClass="btn" Text="Add" OnClick="btnAdd_Click" />
    </div>
</div>
<h3>Find Existing Owner</h3>
<div class="lookupnew">
    <div class="lookupfld namefld">
        <asp:Label ID="Label1" runat="server" AssociatedControlID="txtFind" Text="Search by owner name"></asp:Label>
        <asp:TextBox ID="txtFind" runat="server" 
            placeholder="Enter a name (or part of a name). Leave blank to find all."></asp:TextBox>
    </div>
    <div class="lookupbtn">
        <asp:Button ID="btn_Find" runat="server" Text="Search" CssClass="btn" OnClick="btn_Find_Click" />
    </div>
</div>
<asp:Panel ID="pnlResult" runat="server" Visible="false">
    <h3>Edit Existing Owner </h3>
    <asp:DataGrid ID="gvResult" runat="server" AutoGenerateColumns="False" CssClass="gray-border valuestable ownertable"
        Width="100%" AllowSorting="True" HorizontalAlign="Center" AllowPaging="true"
        PageSize="25" ShowFooter="false" ShowHeader="true"
        OnItemCommand="gvResult_ItemCommand"
        OnEditCommand="gvResult_EditCommand"
        OnCancelCommand="gvResult_CancelCommand"
        OnUpdateCommand="gvResult_UpdateCommand"
        OnDeleteCommand="gvResult_DeleteCommand"
        OnItemDataBound="gvResult_ItemDataBound"
        OnItemCreated="gvResult_ItemCreated"
        OnPageIndexChanged="gvResult_PageindexChange">
        <FooterStyle BackColor="White" Font-Bold="True" ForeColor="Black" Font-Size="Small" VerticalAlign="Top" />
        <PagerStyle ForeColor="#333333" Mode="NumericPages" Wrap="true" Position="TopAndBottom" HorizontalAlign="right" />
        <SelectedItemStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
        <HeaderStyle CssClass="rowHead" />
        <AlternatingItemStyle CssClass="rowOdd" />
        <ItemStyle CssClass="rowEven" HorizontalAlign="left" />
        <Columns>
            <asp:BoundColumn Visible="False" DataField="OWNERID"></asp:BoundColumn>
            <asp:TemplateColumn HeaderText="Last Name">
                <EditItemTemplate>
                    <asp:TextBox ID="txtLname" runat="server" Width="90%" Text='<%# DataBinder.Eval(Container, "DataItem.LastName")%>' MaxLength="20"></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblLname" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LastName")%>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="First Name">
                <EditItemTemplate>
                    <asp:TextBox ID="txtFname" runat="server" Width="90%" MaxLength="20" Text='<%# DataBinder.Eval(Container, "DataItem.FirstName")%>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblFname" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FirstName")%>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Middle Initial">
                <EditItemTemplate>
                    <asp:TextBox ID="txtMname" runat="server" Width="90%" MaxLength="1" Text='<%# DataBinder.Eval(Container, "DataItem.MiddleInitial")%>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblMname" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.MiddleInitial")%>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Long Name">
                <EditItemTemplate>
                    <asp:Label ID="lblLongName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LongName")%>'></asp:Label>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblLongName1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LongName")%>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="State Enabled" >
                <EditItemTemplate>
                    <asp:Label ID="lblChecked1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Checked")%>'></asp:Label>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblChecked" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Checked")%>'></asp:Label>
                </ItemTemplate>                
            </asp:TemplateColumn>
            <asp:EditCommandColumn ButtonType="PushButton" UpdateText="Update" CancelText="Cancel"
                EditText="Edit">
                <ItemStyle CssClass="btnparent" />
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
    <asp:Label ID="lblSearchKeywordOwerids" runat="server" Visible="false"></asp:Label>
</asp:Panel>
