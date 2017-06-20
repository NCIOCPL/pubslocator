<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Announcementx.ascx.cs"
    Inherits="PubEntAdmin.UserControl.Announcementx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<script type="text/javascript">
    var currentTabIndex = 0;
    function AnnouncementxOnload() {
        //alert('in AnnouncementxOnload ' + document.getElementById('<%= this.hidCurrTabIndex.UniqueID %>').value);
        var newTabIndex = 0;
        if (document.getElementById('<%= this.hidCurrTabIndex.UniqueID %>').value.length > 0) {
            newTabIndex = document.getElementById('<%= this.hidCurrTabIndex.UniqueID %>').value;
            document.getElementById('<%= this.hidCurrTabIndex.UniqueID %>').value = "";
            if (currentTabIndex != newTabIndex)
                AfterAdd2ChangeTab(newTabIndex);
        }
    }
    function AfterAdd2ChangeTab(tabNo) {
        currentTabIndex = tabNo;
        var tab = $find('<%= this.tabCont.ClientID %>');
        tab.ActiveTabIndex = tabNo;
        //alert('tab.ActiveTabIndex ' + tab.ActiveTabIndex);
        //hideSpellChk();
        __doPostBack('<%= this.btnEditTab.UniqueID %>', '');
        tab.set_activeTab(tab.get_tabs()[tabNo]);
    }
    function clientActiveTabChanged(sender, args) {
        if (currentTabIndex != sender.get_activeTabIndex())//not clicking the current tab
        {
            switch (sender.get_activeTabIndex()) {
                case 0:
                    currentTabIndex = 0;
                    __doPostBack('<%= this.btnAddTab.UniqueID %>', '');
                    break;
                case 1:
                    currentTabIndex = 1;
                    __doPostBack('<%= this.btnEditTab.UniqueID %>', '');
                    break;
            }
        }
    }
</script>
<input id="btnAddTab" runat="server" style="display: none;" type="button" onserverclick="btnAddTab_Click" />
<input id="btnEditTab" runat="server" style="display: none;" type="button" onserverclick="btnEditTab_Click" />
<cc1:TabContainer ID="tabCont" runat="server" OnClientActiveTabChanged="clientActiveTabChanged">
    <cc1:TabPanel runat="server" ID="tabpnlAdd" Enabled="true" HeaderText="Add New Announcement"
        Width="100%">
        <ContentTemplate>
            <div id="AddTabAlertDiv" class="AlertStyle">
            </div>
            <asp:UpdatePanel ID="updpnlAddTab" runat="server" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnAddTab" />
                </Triggers>
                <ContentTemplate>
                    <input id="hidCurrTabIndex" type="hidden" runat="server" value="" />
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
                        <cc1:CalendarExtender ID="CalExtStartDate" runat="server" CssClass="MyCalendar"
                            Format="MM/dd/yyyy" TargetControlID="txtStartDate">
                        </cc1:CalendarExtender>
                        <cc1:MaskedEditExtender ID="MaskedEditExtStartDate" runat="server"
                            Mask="99/99/9999" MaskType="Date" TargetControlID="txtStartDate">
                        </cc1:MaskedEditExtender>
                        <cc1:MaskedEditValidator ID="MaskedEditValStartDate" runat="server"
                            ControlExtender="MaskedEditExtStartDate" ControlToValidate="txtStartDate"
                            Display="Dynamic" EmptyValueMessage="A Date is Required"
                            InvalidValueMessage="Invalid Date format" IsValidEmpty="false" TooltipMessage="" ValidationGroup="AddNewVal"></cc1:MaskedEditValidator>
                        <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                        <cc1:CalendarExtender ID="CalExtEndDate" runat="server" CssClass="MyCalendar"
                            Format="MM/dd/yyyy" TargetControlID="txtEndDate">
                        </cc1:CalendarExtender>
                        <cc1:MaskedEditExtender ID="MaskedEditExtEndDate" runat="server"
                            Mask="99/99/9999" MaskType="Date" TargetControlID="txtEndDate">
                        </cc1:MaskedEditExtender>
                        <cc1:MaskedEditValidator ID="MaskedEditValEndDate" runat="server"
                            ControlExtender="MaskedEditExtEndDate" ControlToValidate="txtEndDate"
                            Display="Dynamic" EmptyValueMessage="A Date is Required"
                            InvalidValueMessage="Invalid Date format" IsValidEmpty="false" TooltipMessage="" ValidationGroup="AddNewVal"></cc1:MaskedEditValidator>
                        <asp:CompareValidator ID="comValDates" runat="server" ErrorMessage="Start Date cannot be greater than End Date." ControlToCompare="txtStartDate" ControlToValidate="txtEndDate" ValidationGroup="AddNewVal" Display="Dynamic" Type="Date" Operator="GreaterThan"></asp:CompareValidator>
                        <asp:Label ID="lblEndDate" runat="server" Text="End Date" AssociatedControlID="txtEndDate" CssClass=""></asp:Label>
                        *                                                   
                        <asp:Label ID="lblMsg" runat="server"></asp:Label>
                        <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="Add" ValidationGroup="AddNewVal" />
                    </fieldset>
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </cc1:TabPanel>
    <cc1:TabPanel runat="server" ID="tabpnlEdit" Enabled="true" HeaderText="Edit Existing Announcement"
        Width="100%">
        <ContentTemplate>
            <div id="EditTabAlertDiv" class="AlertStyle">
            </div>
            <asp:UpdatePanel ID="updpnlEditTab" runat="server" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnEditTab" />
                </Triggers>
                <ContentTemplate>
                    <strong>Edit Existing Announcement</strong>
                    <asp:DataGrid ID="gvResult" runat="server" AutoGenerateColumns="False" CssClass="gray-border valuestable"
                         AllowSorting="false" HorizontalAlign="Center" AllowPaging="false"
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
                                    <asp:TextBox ID="txtStartDate_gd" runat="server" Columns="6" Text='<%# DataBinder.Eval(Container, "DataItem.StartDate")%>'></asp:TextBox>
                                    <cc1:CalendarExtender ID="CalExtStartDate_gd" runat="server" TargetControlID="txtStartDate_gd"
                                        CssClass="MyCalendar" Format="MM/dd/yyyy">
                                    </cc1:CalendarExtender>
                                    <cc1:MaskedEditExtender ID="MaskedEditExtStartDate_gd" runat="server" TargetControlID="txtStartDate_gd"
                                        Mask="99/99/9999" MaskType="Date">
                                    </cc1:MaskedEditExtender>
                                    <cc1:MaskedEditValidator ID="MaskedEditValStartDate_gd" runat="server" ControlToValidate="txtStartDate_gd"
                                        ControlExtender="MaskedEditExtStartDate_gd" Display="Dynamic" TooltipMessage=""
                                        IsValidEmpty="false" EmptyValueMessage="A Date is Required" InvalidValueMessage="Invalid Date format"
                                        ValidationGroup="EditVal"></cc1:MaskedEditValidator>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblStartDate_gd" runat="server" Text=""></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn ItemStyle-Width="10%" HeaderText="End Date" SortExpression="E_DATE">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtEndDate_gd" runat="server" Columns="6" Text='<%# DataBinder.Eval(Container, "DataItem.EndDate")%>'></asp:TextBox>
                                    <cc1:CalendarExtender ID="CalExtEndDate_gd" runat="server" TargetControlID="txtEndDate_gd"
                                        CssClass="MyCalendar" Format="MM/dd/yyyy">
                                    </cc1:CalendarExtender>
                                    <cc1:MaskedEditExtender ID="MaskedEditExtEndDate_gd" runat="server" TargetControlID="txtEndDate_gd"
                                        Mask="99/99/9999" MaskType="Date">
                                    </cc1:MaskedEditExtender>
                                    <cc1:MaskedEditValidator ID="MaskedEditValEndDate_gd" runat="server" ControlToValidate="txtEndDate_gd"
                                        ControlExtender="MaskedEditExtEndDate_gd" Display="Dynamic" TooltipMessage=""
                                        IsValidEmpty="false" EmptyValueMessage="A Date is Required" InvalidValueMessage="Invalid Date format"
                                        ValidationGroup="EditVal"></cc1:MaskedEditValidator>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblEndDate_gd" runat="server" Text=""></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:EditCommandColumn ButtonType="LinkButton" UpdateText="Update" CancelText="Cancel"
                                EditText="Edit" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center"></asp:EditCommandColumn>
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
                                    <cc1:ConfirmButtonExtender ID="confirmBtnExtDel" runat="server" TargetControlID="lnkbtnDel"
                                        ConfirmText="" DisplayModalPopupID="modalPopupExtDel">
                                    </cc1:ConfirmButtonExtender>
                                    <cc1:ModalPopupExtender ID="modalPopupExtDel" runat="server" TargetControlID="lnkbtnDel"
                                        PopupControlID="pnlConfirm" BackgroundCssClass="modalBackground" DropShadow="true"
                                        OkControlID="OkButton" CancelControlID="CancelButton">
                                    </cc1:ModalPopupExtender>
                                    <asp:LinkButton ID="lnkbtnDel" runat="server" Text="Delete"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </cc1:TabPanel>
</cc1:TabContainer>