<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Sponsor.ascx.cs" Inherits="PubEntAdmin.UserControl.Sponsor" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Label ID="lblMessage" runat="server" CssClass="errorText"></asp:Label>
<h3>Add New Sponsor</h3>
<div class="editboxtable sponsor">
    <div class="editbox">
        <asp:Label runat="server" AssociatedControlID="txtSpCode">Sponsor Code *</asp:Label>
        <asp:TextBox ID="txtSpCode" runat="server" MaxLength="7"></asp:TextBox>
        <asp:Label ID="lblErSpCode" CssClass="errorText" Font-Size="Small" runat="server"></asp:Label>
    </div>
    <div class="editbox">
        <asp:Label runat="server" AssociatedControlID="txtSpLongDesc">Long Sponsor Description *</asp:Label>
        <asp:TextBox ID="txtSpLongDesc" runat="server" MaxLength="101"></asp:TextBox>
        <asp:Label ID="lblErLongDesc" CssClass="errorText" Font-Size="Small" runat="server"></asp:Label>
    </div>
    <div class="editbox lookupbtn">
        <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn" OnClick="btnAdd_Click" />
        <asp:Label ID="Message" runat="server" CssClass="errorText" Visible="False"></asp:Label>
    </div>
</div>
<h3>Find Existing Sponsor</h3>
<div class="lookupnew">
    <div class="lookupfld namefld">
        <asp:Label ID="Label2" runat="server" AssociatedControlID="txtFind">Search</asp:Label>
        <asp:TextBox ID="txtFind" runat="server" placeholder="Enter a word (or part of a word). Leave blank to find all."></asp:TextBox>
    </div>
    <div class="lookupbtn">
        <asp:Button ID="btn_Find" runat="server" CssClass="btn" Text="Search" OnClick="btn_Find_Click" />
    </div>
</div>
<asp:Panel ID="pnlResult" runat="server" Visible="false">
    <h3>Edit Existing Sponsor </h3>
    <asp:ImageButton ID="ImgBtnExportSchRsltToExcel" runat="server" AlternateText="Export Search Result to Excel"
        ImageUrl="../Image/excelicon.gif" ImageAlign="Bottom" OnClick="ImgBtnExportSchRsltToExcel_OnClick" />
    <asp:DataGrid ID="gvResult" runat="server" AutoGenerateColumns="false" GridLines="Horizontal"
        CssClass="gray-border valuestable"
        Width="100%" HorizontalAlign="Center" AllowPaging="true"
        PageSize="25"
        OnItemCommand="gvResult_ItemCommand"
        OnEditCommand="gvResult_EditCommand"
        OnCancelCommand="gvResult_CancelCommand"
        OnUpdateCommand="gvResult_UpdateCommand"
        OnDeleteCommand="gvResult_DeleteCommand"
        OnItemDataBound="gvResult_ItemDataBound"
        OnItemCreated="gvResult_ItemCreated"
        OnPageIndexChanged="gvResult_PageindexChange">
        <FooterStyle BackColor="White" Font-Bold="True" ForeColor="Black" Font-Size="Small" VerticalAlign="Top" />
        <PagerStyle ForeColor="#333333" Mode="NumericPages" Wrap="false" Position="TopAndBottom" HorizontalAlign="right" />
        <SelectedItemStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
        <HeaderStyle CssClass="rowHead" Wrap="false" />
        <AlternatingItemStyle CssClass="rowOdd" />
        <ItemStyle CssClass="rowEven" HorizontalAlign="left" />
        <Columns>
            <asp:BoundColumn Visible="False" DataField="SPONSORID"></asp:BoundColumn>
            <asp:TemplateColumn HeaderText="Sponsor Code">
                <EditItemTemplate>
                    <asp:TextBox ID="txtSpCode" runat="server" Width="30" Text='<%# DataBinder.Eval(Container, "DataItem.SPONSORCODE")%>' MaxLength="7"></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblSpCode" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.SPONSORCODE")%>'></asp:Label><br />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Long Sponsor Description">
                <EditItemTemplate>
                    <asp:TextBox ID="txtSpLongDesc" runat="server" MaxLength="101" Text='<%# DataBinder.Eval(Container, "DataItem.LongDescription")%>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblLongDesc" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LongDescription")%>'></asp:Label><br />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="CPJ Short Description">
                <EditItemTemplate>
                    <asp:Label ID="lblSpDescEd" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.DESCRIPTION")%>'></asp:Label>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblSpDesc" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.DESCRIPTION")%>'></asp:Label><br />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="State Enabled" ItemStyle-HorizontalAlign="Center">
                <EditItemTemplate>
                    <asp:Label ID="lblChecked1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Checked")%>'></asp:Label>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblChecked" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Checked")%>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:EditCommandColumn ButtonType="PushButton" UpdateText="Update" CancelText="Cancel"
                EditText="Edit">
                <ItemStyle CssClass="btnparent"></ItemStyle>
            </asp:EditCommandColumn>
            <asp:TemplateColumn>
                <ItemTemplate>
                    <asp:Panel ID="pnlConfirmDel" runat="server" CssClass="modalPopup" Style="display: none">
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
                    <asp:Label ID="lblMessage" runat="server" Text="" CssClass="LabelDefault" Font-Size="XX-Small" ForeColor="Maroon"></asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>
    <asp:Label ID="lblSearchSpids" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="lblSearchASpids" runat="server" Visible="false"></asp:Label>
    <asp:GridView ID="gvExcelRpt" runat="server" AutoGenerateColumns="false" Visible="false">
        <Columns>
            <asp:BoundField DataField="SPONSORCODE" HeaderText="Sponsor Code" SortExpression="SPONSORCODE" />
            <asp:BoundField DataField="LongDescription" HeaderText="Long Sponsor Description" SortExpression="LongDescription" ItemStyle-Width="90%" />
        </Columns>
    </asp:GridView>
</asp:Panel>
