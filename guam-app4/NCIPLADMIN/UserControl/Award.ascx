<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Award.ascx.cs" Inherits="PubEntAdmin.UserControl.Award" %>
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
<h3>Add New Award</h3>
<div class="editboxrow">
    <div class="editbox">
        <asp:Label runat="server" AssociatedControlID="txtAwdName"> Award Name</asp:Label>
        <asp:TextBox ID="txtAwdName" runat="server" Width="80%"></asp:TextBox>
    </div>
    <div class="editbox">
        <asp:Label ID="Label1" runat="server" AssociatedControlID="txtAwdYear">Award Year</asp:Label>
        <asp:TextBox ID="txtAwdYear" runat="server" Width="80%"></asp:TextBox>
    </div>
    <div class="editbox">
        <asp:Label ID="Label2" runat="server" AssociatedControlID="txtAwdCategory">Award Category</asp:Label>
        <asp:TextBox ID="txtAwdCategory" runat="server" Width="80%" MaxLength="100"></asp:TextBox>
    </div>
    <div class="editbox lookupbtn">
        <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn" OnClick="btnAdd_Click" />
        <asp:Label ID="Message" runat="server" CssClass="errorText" Visible="False"></asp:Label>
    </div>
</div>
<h3>Edit Existing Award</h3>
<asp:DataGrid ID="gvResult" runat="server" AutoGenerateColumns="False" CellPadding="4"
    GridLines="Horizontal" CssClass="gray-border valuestable"
    Width="100%" AllowSorting="True" HorizontalAlign="Center"
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
        <asp:BoundColumn Visible="False" DataField="AwardID"></asp:BoundColumn>
        <asp:TemplateColumn HeaderText="Award Description">
            <EditItemTemplate>
                <asp:TextBox ID="txtAwdName" runat="server" CssClass="awardvalue1"
                    Text='<%# DataBinder.Eval(Container, "DataItem.AwdName")%>' placeholder="Name"></asp:TextBox>
                <%-- <cc1:TextBoxWatermarkExtender ID="awdnametxtwatermark" runat="server"
                    TargetControlID="txtAwdName" WatermarkText="Name">
                </cc1:TextBoxWatermarkExtender>--%>
                <asp:TextBox ID="txtAwdCategory" runat="server" placeholder="Category" CssClass="awardvalue2"
                    Text='<%# DataBinder.Eval(Container, "DataItem.AwdCategory")%>'></asp:TextBox>
                <%--  <cc1:TextBoxWatermarkExtender ID="awdcategtxtwatermark" runat="server"
                    TargetControlID="txtAwdCategory" WatermarkText="Category">
                </cc1:TextBoxWatermarkExtender>--%>
                <asp:TextBox ID="txtAwdYear" runat="server" placeholder="Year" CssClass="awardvalue2"
                    Text='<%# DataBinder.Eval(Container, "DataItem.AwdYear")%>'></asp:TextBox>
                <%-- <cc1:TextBoxWatermarkExtender ID="awdyeartxtwatermark" runat="server"
                    TargetControlID="txtAwdYear" WatermarkText="Year">
                </cc1:TextBoxWatermarkExtender>--%>
            </EditItemTemplate>
            <ItemTemplate>
                <asp:Label ID="lblAwdName" runat="server" Text=""></asp:Label>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="State Enabled">
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
                <asp:Label ID="lblMessage" runat="server" Text="" CssClass="LabelDefault" Width="70px" Font-Size="XX-Small" ForeColor="Maroon"></asp:Label>
            </ItemTemplate>
        </asp:TemplateColumn>
    </Columns>
</asp:DataGrid>
