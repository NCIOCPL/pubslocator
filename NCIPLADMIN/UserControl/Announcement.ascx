<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Announcement.ascx.cs"
    Inherits="PubEntAdmin.UserControl.Announcement" %>
<%@ Register Src="TabStrip.ascx" TagName="TabStrip" TagPrefix="uc1" %>
<uc1:TabStrip ID="TabStrip1" runat="server" OnSelectionChanged="TabStrip1_SelectionChanged" />
<asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
    <asp:View ID="Tab1" runat="server">
        <asp:UpdatePanel ID="updpnlAddTab" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <strong>Add New Announcement</strong>
                <fieldset>
                    <asp:Label ID="lblAnnoDescp" runat="server" Text="Announcement Description" AssociatedControlID="txtAnnouncementName" CssClass=""></asp:Label>
                    *                                                   
                    <asp:TextBox ID="txtAnnouncementName" runat="server" Width="80%" MaxLength="500"></asp:TextBox><asp:RequiredFieldValidator ID="reqValAnnouncementName" runat="server" ControlToValidate="txtAnnouncementName" ErrorMessage="Required" ValidationGroup="AddNewVal"></asp:RequiredFieldValidator>
                    <asp:Label ID="lblAnnoURL" runat="server" Text="Announcement URL" AssociatedControlID="txtAnouncementURL" CssClass=""></asp:Label>
                    *                                                   
                    <asp:TextBox ID="txtAnouncementURL" runat="server" MaxLength="500" Width="80%" TextMode="MultiLine"></asp:TextBox><asp:RequiredFieldValidator ID="reqValAnouncementURL" runat="server" ControlToValidate="txtAnouncementURL" ErrorMessage="Required" ValidationGroup="AddNewVal" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:Label ID="lblStartDate" runat="server" Text="Start Date" AssociatedControlID="txtStartDate" CssClass=""></asp:Label>
                    *                                                                           
                    <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="reqValStartDate" runat="server" ControlToValidate="txtStartDate" ErrorMessage="Required" ValidationGroup="AddNewVal" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="custValStartDate" runat="server" ErrorMessage="Invalid Date format" ControlToValidate="txtStartDate" OnServerValidate="DateOnServerValidate" Display="Dynamic" ValidationGroup="AddNewVal"></asp:CustomValidator>
                    <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="reqValEndDate" runat="server" ControlToValidate="txtEndDate" ErrorMessage="Required" ValidationGroup="AddNewVal" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="custValEndDate" runat="server" ErrorMessage="Invalid Date format" ControlToValidate="txtEndDate" OnServerValidate="DateOnServerValidate" Display="Dynamic" ValidationGroup="AddNewVal"></asp:CustomValidator>
                    <asp:CompareValidator ID="comValDates" runat="server" ErrorMessage="Start Date cannot be greater than End Date." ControlToCompare="txtStartDate" ControlToValidate="txtEndDate" ValidationGroup="AddNewVal" Display="Dynamic" Type="Date" Operator="GreaterThan"></asp:CompareValidator>
                    <asp:Label ID="lblEndDate" runat="server" Text="End Date" AssociatedControlID="txtEndDate" CssClass=""></asp:Label>
                    *                                                   
                    <asp:Label ID="lblMsg" runat="server"></asp:Label>
                    <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="Add" ValidationGroup="AddNewVal" />
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:View>
    <asp:View ID="Tab2" runat="server">
        <strong>Edit Existing Announcement</strong>
        <asp:DataGrid ID="gvResult" runat="server" AutoGenerateColumns="False" CssClass="gray-border valuestable"
             AllowSorting="false"  AllowPaging="false"
            PageSize="4" OnItemDataBound="gvResult_ItemDataBound" ShowFooter="false" ShowHeader="true"
            OnItemCommand="gvResult_ItemCommand" OnSortCommand="gvResult_SortCommand" OnEditCommand="gvResult_EditCommand"
            OnCancelCommand="gvResult_CancelCommand" OnUpdateCommand="gvResult_UpdateCommand"
            OnItemCreated="gvResult_ItemCreated" UseAccessibleHeader="true">           
            <HeaderStyle CssClass="rowHead" />
            <AlternatingItemStyle CssClass="rowOdd" />
            <ItemStyle CssClass="rowEven" />
            <Columns>
                <asp:BoundColumn Visible="False" ReadOnly="True" DataField="AnnouncementID"></asp:BoundColumn>
                <asp:TemplateColumn HeaderText="Announcement Description" SortExpression="ANNOUNCEMENT_DESC"
                    ItemStyle-Width="30%">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtAnnouncementName_gd" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.AnnouncementName")%>'
                            Width="90%" MaxLength="500"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="reqValAnnouncementName_gd" runat="server" ControlToValidate="txtAnnouncementName_gd"
                            ErrorMessage="Required" ValidationGroup="EditVal"></asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblAnnouncementName_gd" runat="server" Text="" Width="90%"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Announcement URL" SortExpression="ANNOUNCEMENT_URL"
                    ItemStyle-Width="35%">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtAnnouncementURL_gd" runat="server" TextMode="MultiLine" Rows="3"
                            Text='<%# DataBinder.Eval(Container, "DataItem.AnnouncementURL")%>' Width="90%"
                            MaxLength="500"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="reqValAnouncementURL_gd" runat="server" ControlToValidate="txtAnnouncementURL_gd"
                            ErrorMessage="Required" ValidationGroup="EditVal" Display="Dynamic"></asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:HyperLink ID="hylnkAnnouncementURL_gd" runat="server" Width="90%" Target="_blank"></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn ItemStyle-Width="10%" HeaderText="Start Date" SortExpression="S_DATE">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtStartDate_gd" runat="server" Columns="6" Text='<%# DataBinder.Eval(Container, "DataItem.StartDate", "{0:d}")%>'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="reqValStartDate_gd" runat="server" ControlToValidate="txtStartDate_gd"
                            ErrorMessage="Required" ValidationGroup="EditVal" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="custValStartDate_gd" runat="server" ErrorMessage="Invalid Date format"
                            ControlToValidate="txtStartDate_gd" OnServerValidate="DateOnServerValidate"
                            Display="Dynamic" ValidationGroup="EditVal"></asp:CustomValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblStartDate_gd" runat="server" Text=""></asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn ItemStyle-Width="10%" HeaderText="End Date" SortExpression="E_DATE">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEndDate_gd" runat="server" Columns="6" Text='<%# DataBinder.Eval(Container, "DataItem.EndDate", "{0:d}")%>'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="reqValEndDate_gd" runat="server" ControlToValidate="txtEndDate_gd"
                            ErrorMessage="Required" ValidationGroup="EditVal" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="custValEndDate_gd" runat="server" ErrorMessage="Invalid Date format"
                            ControlToValidate="txtEndDate_gd" OnServerValidate="DateOnServerValidate"
                            Display="Dynamic" ValidationGroup="EditVal"></asp:CustomValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblEndDate_gd" runat="server" Text=""></asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:EditCommandColumn ButtonType="PushButton" UpdateText="Update" CancelText="Cancel"
                    EditText="Edit" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center"></asp:EditCommandColumn>
                <asp:TemplateColumn>
                    <HeaderTemplate>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Button ID="lnkbtnDel" runat="server" Text="Delete"></asp:Button>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
    </asp:View>
</asp:MultiView>