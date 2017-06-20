<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Template/Default2.Master"
    CodeBehind="Conference.aspx.cs" Inherits="PubEntAdmin.Conference" ValidateRequest="false"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="contentMain" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="exhibit">
        <div class="editboxtable">
            <asp:Panel ID="pnlTimeoutDisplay" runat="server" CssClass="editboxrow">
                <div class="editbox">
                    Attract Page Rotation Time (in seconds)
                <asp:Label ID="lblRotationTime" runat="server"></asp:Label>
                </div>
                <div class="editbox">
                    Page Expiration Time (in seconds)
              <asp:Label ID="lblPageTime" runat="server"></asp:Label>
                </div>
                <div class="editbox">
                    Session Expiration Time (in seconds)
                <asp:Label ID="lblSessionTime" runat="server"></asp:Label>
                </div>
                <div class="editbox">
                    <div class="btncontainer">
                        <asp:Button ID="btnEditTimeout" runat="server" CssClass="btn" Text="Edit" OnClick="btnTimoutEdit_click" />
                    </div>
                </div>
            </asp:Panel>
            <asp:Label ID="lblTimeoutErrorMeg" runat="server" CssClass="errorText"></asp:Label>
            <asp:Panel ID="pnlTimoutEdit" runat="server" Visible="false">
                <div class="editbox">
                    <asp:Label AssociatedControlID="txtRotateTime" Text="Attract Page Rotation Time * (in seconds)" runat="server"></asp:Label>
                    <asp:TextBox ID="txtRotateTime" runat="server"></asp:TextBox>
                    <cc1:MaskedEditExtender ID="MaskedEditExRotateTime" runat="server" TargetControlID="txtRotateTime"
                        MaskType="Number" Mask="99999" AutoComplete="false" PromptCharacter=" ">
                    </cc1:MaskedEditExtender>
                    <cc1:MaskedEditValidator ID="MaskedEditValRotateTime" runat="server" ControlToValidate="txtRotateTime"
                        ControlExtender="MaskedEditExRotateTime" Display="Dynamic" TooltipMessage=""
                        IsValidEmpty="true" InvalidValueMessage="Please enter numeric values in the three timeout fields"></cc1:MaskedEditValidator>
                </div>
                <div class="editbox">
                    <asp:Label ID="Label4" AssociatedControlID="txtPageTime" Text="Page Expiration Time * (in seconds)" runat="server"></asp:Label>
                    <asp:TextBox ID="txtPageTime" runat="server"></asp:TextBox>
                    <cc1:MaskedEditExtender ID="MaskedEditExPageTime" runat="server" TargetControlID="txtPageTime"
                        MaskType="Number" Mask="99999" AutoComplete="false" PromptCharacter=" ">
                    </cc1:MaskedEditExtender>
                    <cc1:MaskedEditValidator ID="MaskedEditValPageTime" runat="server" ControlToValidate="txtPageTime"
                        ControlExtender="MaskedEditExPageTime" Display="Dynamic" TooltipMessage="" IsValidEmpty="true"
                        InvalidValueMessage="Please enter numeric values in the three timeout fields"></cc1:MaskedEditValidator>
                </div>
                <div class="editbox">
                    <asp:Label ID="Label5" AssociatedControlID="txtSessionTime" Text="Session Expiration Time * (in seconds)" runat="server"></asp:Label>
                    <asp:TextBox ID="txtSessionTime" runat="server"></asp:TextBox>
                    <cc1:MaskedEditExtender ID="MaskedEditExSessionTime" runat="server" TargetControlID="txtSessionTime"
                        MaskType="Number" Mask="99999" AutoComplete="false" PromptCharacter=" ">
                    </cc1:MaskedEditExtender>
                    <cc1:MaskedEditValidator ID="MaskedEditValiSessionTime" runat="server" ControlToValidate="txtSessionTime"
                        ControlExtender="MaskedEditExSessionTime" Display="Dynamic" TooltipMessage=""
                        IsValidEmpty="true" InvalidValueMessage="Please enter numeric values in the three timeout fields"></cc1:MaskedEditValidator>
                </div>
                <div class="editbox">
                    <div class="btncontainer">
                        <asp:Button ID="btnAddTime" runat="server" Text="Add" Visible="false" OnClick="btnTimeoutAdd_click" />
                        <asp:Button ID="btnUpdateTime" runat="server" CssClass="btnGo" Text="Update" Visible="false" OnClick="btnTimeoutAdd_click" />
                        <asp:Button ID="btnCancelTime" runat="server" CssClass="btn" Text="Cancel" Visible="false" OnClick="btnTimeoutCancel_click" />
                    </div>
                </div>
            </asp:Panel>
        </div>
        <div class="editboxtable">
            <div class="editboxrow">
                <div class="editbox">
                    <asp:Label ID="Label0" runat="server" AssociatedControlID="txtConfName">Exhibit Name*</asp:Label>
                    <asp:TextBox ID="txtConfName" runat="server" MaxLength="10"></asp:TextBox>
                </div>
                <div class="editbox">
                    <asp:Label ID="Label1" runat="server" AssociatedControlID="txtStartDate">Start Date*</asp:Label>
                    <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
                    <cc1:CalendarExtender ID="CalExtStartDate" runat="server" TargetControlID="txtStartDate"
                        CssClass="MyCalendar" Format="MM/dd/yyyy">
                    </cc1:CalendarExtender>
                    <cc1:MaskedEditExtender ID="MaskedEditExtStartDate" runat="server" TargetControlID="txtStartDate"
                        Mask="99/99/9999" MaskType="Date">
                    </cc1:MaskedEditExtender>
                    <cc1:MaskedEditValidator ID="MaskedEditValStartDate" runat="server" ControlToValidate="txtStartDate"
                        ControlExtender="MaskedEditExtStartDate" Display="Dynamic" TooltipMessage=""
                        IsValidEmpty="true" EmptyValueMessage="A Date is Required" InvalidValueMessage="Ths date is invalid"></cc1:MaskedEditValidator>
                </div>
                <div class="editbox">
                    <asp:Label ID="Label2" runat="server" AssociatedControlID="txtEndDate">End Date*</asp:Label>
                    <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                    <cc1:CalendarExtender ID="CalExtEndDate" runat="server" TargetControlID="txtEndDate"
                        CssClass="MyCalendar" Format="MM/dd/yyyy">
                    </cc1:CalendarExtender>
                    <cc1:MaskedEditExtender ID="MaskedEditExtEndDate" runat="server" TargetControlID="txtEndDate"
                        Mask="99/99/9999" MaskType="Date">
                    </cc1:MaskedEditExtender>
                    <cc1:MaskedEditValidator ID="MaskedEditValEndDate" runat="server" ControlToValidate="txtEndDate"
                        ControlExtender="MaskedEditExtEndDate" Display="Dynamic" TooltipMessage="" IsValidEmpty="true"
                        EmptyValueMessage="A Date is Required" InvalidValueMessage="This date is invalid"></cc1:MaskedEditValidator>
                </div>
                <div class="editbox">
                    <asp:Label ID="Label3" runat="server" AssociatedControlID="txtMaxOrder">Max Order / Int'l</asp:Label>
                    <asp:TextBox ID="txtMaxOrder" Text="5" Width="60" runat="server"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="filttxtMaxOrder" runat="server" TargetControlID="txtMaxOrder"
                        FilterType="Numbers">
                    </cc1:FilteredTextBoxExtender>
                    <asp:Button ID="btnAdd" runat="server" CssClass="btnGo" Text="Add" OnClick="btnAdd_Click" />
                    <asp:Label ID="Message" runat="server" CssClass="errorText"></asp:Label>

                </div>
            </div>
        </div>
        <asp:DataGrid ID="gvResult" runat="server" AutoGenerateColumns="False" 
           AllowSorting="False" HorizontalAlign="Center" CssClass="gray-border"
            GridLines="Horizontal" OnItemDataBound="gvResult_ItemDataBound" OnEditCommand="gvResult_EditCommand"
            OnCancelCommand="gvResult_CancelCommand" OnUpdateCommand="gvResult_UpdateCommand"
            OnItemCommand="gvResult_ItemCommand" OnItemCreated="gvResult_ItemCreated" OnDeleteCommand="gvResult_DeleteCommand">
            <ItemStyle CssClass="rowOdd" />
            <HeaderStyle CssClass="rowHead" Font-Bold="true" />
            <AlternatingItemStyle CssClass="rowEven" />
            <ItemStyle  />
            <Columns>
                <asp:TemplateColumn>
                    <ItemTemplate>
                        <asp:Label ID="sConfid" Visible="False" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ConfID")%>'
                            ForeColor="#FFFFCC"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Conference Name" >
                    <EditItemTemplate>
                        <asp:TextBox ID="txtConfName" MaxLength="10" runat="server" 
                            CssClass="confvalue" Text='<%# DataBinder.Eval(Container, "DataItem.ConfName")%>'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rqfvConfName" ControlToValidate="txtConfName" Text="A Date is Required"
                            runat="server" Display="Dynamic"></asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblConfName2" runat="server" Text=""></asp:Label>
                    </ItemTemplate>
                    <ItemStyle></ItemStyle>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Start Date" >
                    <EditItemTemplate>
                        <asp:TextBox ID="txtStartDate_gd"  CssClass="confvalue" runat="server" Columns="15" Text='<%# DataBinder.Eval(Container, "DataItem.StartDate")%>'></asp:TextBox>
                        <cc1:CalendarExtender ID="CalExtStartDate_gd" runat="server" TargetControlID="txtStartDate_gd"
                            CssClass="MyCalendar" Format="MM/dd/yyyy">
                        </cc1:CalendarExtender>
                        <cc1:MaskedEditExtender ID="MaskedEditExtStartDate_gd" runat="server" TargetControlID="txtStartDate_gd"
                            Mask="99/99/9999" MaskType="Date">
                        </cc1:MaskedEditExtender>
                        <cc1:MaskedEditValidator ID="MaskedEditValStartDate_gd" runat="server" ControlToValidate="txtStartDate_gd"
                            ControlExtender="MaskedEditExtStartDate_gd" Display="Dynamic" TooltipMessage=""
                            IsValidEmpty="false" EmptyValueMessage="A Date is Required" InvalidValueMessage="This date is invalid"></cc1:MaskedEditValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblStartDate" runat="server" Text=""></asp:Label>
                    </ItemTemplate>
                    <ItemStyle ></ItemStyle>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="End Date" >
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEndDate_gd"  CssClass="confvalue" runat="server" Columns="15" Text='<%# DataBinder.Eval(Container, "DataItem.EndDate")%>'></asp:TextBox>
                        <cc1:CalendarExtender ID="CalExtEndDate_gd" runat="server" TargetControlID="txtEndDate_gd"
                            CssClass="MyCalendar" Format="MM/dd/yyyy">
                        </cc1:CalendarExtender>
                        <cc1:MaskedEditExtender ID="MaskedEditExtEndDate_gd" runat="server" TargetControlID="txtEndDate_gd"
                            Mask="99/99/9999" MaskType="Date">
                        </cc1:MaskedEditExtender>
                        <cc1:MaskedEditValidator ID="MaskedEditValEndDate_gd" runat="server" 
                            ControlToValidate="txtEndDate_gd"
                            ControlExtender="MaskedEditExtEndDate_gd" Display="Dynamic" TooltipMessage=""
                            IsValidEmpty="false" EmptyValueMessage="A Date is Required" 
                            InvalidValueMessage="Ths date is invalid"></cc1:MaskedEditValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblEndDate" runat="server" Text=""></asp:Label>
                    </ItemTemplate>
                    <ItemStyle  />
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Max Order / Int'l">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtMaxIntlOrder_gd"  CssClass="confvalue" runat="server" Columns="15" Text='<%# DataBinder.Eval(Container, "DataItem.MaxOrder_INTL")%>'></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="filttxtMaxOrder1" runat="server" TargetControlID="txtMaxIntlOrder_gd"
                            FilterType="Numbers">
                        </cc1:FilteredTextBoxExtender>
                        <asp:RequiredFieldValidator ID="rqfvMaxOrder"
                             ControlToValidate="txtMaxIntlOrder_gd" Display="Dynamic"
                            Text="A Date is Required" runat="server" ></asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblMaxOrder" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:EditCommandColumn ButtonType="PushButton" UpdateText="Update" CancelText="Cancel"
                    EditText="Edit" >
                    <ItemStyle CssClass="btnparent" ></ItemStyle>                    
                </asp:EditCommandColumn>
                <asp:TemplateColumn HeaderText=""  ItemStyle-CssClass="btnparent">
                    <HeaderTemplate />
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
                        <asp:Button ID="btnRotatPubs" runat="server" CssClass="btn" Text="Rotation Publications" />
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
    </div>

   
    <asp:GridView ID="gvRotatPubs" runat="server" AutoGenerateColumns="false" Width="70%">
        <Columns>
            <asp:BoundField DataField="ProdID" HeaderText="Publication ID" SortExpression="ProdID"
                ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField DataField="LongTitle" HeaderText="Long Title" SortExpression="LongTitle"
                ItemStyle-Width="98%" />
        </Columns>
    </asp:GridView>
</asp:Content>
