<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PubHistTabEditInfo.ascx.cs"
    Inherits="PubEntAdmin.UserControl.PubHistTabEditInfo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="ComboDatepicker.ascx" TagName="ComboDatepicker" TagPrefix="uc1" %>

<div class="editboxrow">
    <div class="editbox">
        <asp:Label ID="lblDateReceived" runat="server" Text="Date Received *"></asp:Label>
        <asp:TextBox ID="txtReceivedDate" runat="server" MaxLength="10"></asp:TextBox>
        <cc1:CalendarExtender ID="CalExtReceivedDate" runat="server" TargetControlID="txtReceivedDate"
            CssClass="MyCalendar" Format="MM/dd/yyyy">
        </cc1:CalendarExtender>
        <cc1:MaskedEditExtender ID="MaskedEditExtReceivedDate" runat="server" TargetControlID="txtReceivedDate"
            Mask="99/99/9999" MaskType="Date">
        </cc1:MaskedEditExtender>
        <cc1:MaskedEditValidator ID="MaskedEditValReceivedDate" runat="server" ControlToValidate="txtReceivedDate"
            ControlExtender="MaskedEditExtReceivedDate" Display="Dynamic" TooltipMessage=""
            IsValidEmpty="true" EmptyValueMessage="A Date is Required" InvalidValueMessage="Ths date is invalid"></cc1:MaskedEditValidator>
        <asp:CustomValidator ID="CustValtxtReceivedDateAndQty" runat="server" ErrorMessage="require."
            Display="none" ClientValidationFunction="txtReceivedDateAndQtyVal" EnableClientScript="true"></asp:CustomValidator>
    </div>
    <div class="editbox">
        <asp:Label ID="lblQtyRec" runat="server" Text="Quantity Received *"></asp:Label>
        <asp:TextBox ID="txtQty" runat="server" MaxLength="6"></asp:TextBox>
        <cc1:FilteredTextBoxExtender ID="ftbeQty" runat="server" TargetControlID="txtQty"
            FilterType="Numbers">
        </cc1:FilteredTextBoxExtender>
    </div>
    <div class="editbox">
        <asp:Label ID="lblNoDateOnPub" runat="server" Text="No Date on Publication"></asp:Label>
        <asp:CheckBox ID="ckboxNoPubDate" runat="server" CssClass="recordckbox" Text="The Received Date will be used as Original Publication Date" />
    </div>
</div>
<div class="editboxrow">
    <div class="editbox datepicker">
        <asp:Label ID="lblOrigPubDate" runat="server" Text="Original Publication Date"></asp:Label>
        <uc1:ComboDatepicker ID="ComboDatepickerOrigPub" runat="server" LabelName="Original Publication Date" />
        <asp:CustomValidator ID="CustValComboDatepickerOrigPub" runat="server" ErrorMessage="Invalid Orig Pub Date format."
            ClientValidationFunction="ComboDatepickerOrigPubDateVal" Display="none" EnableClientScript="true"></asp:CustomValidator>
    </div>
    <div class="editbox datepicker">
        <asp:Label ID="lblDateLastPrint" runat="server" Text="Date Last Printed"></asp:Label>
        <uc1:ComboDatepicker ID="ComboDatepicker_PubPrint" runat="server" LabelName="Date Last Printed"  />
        <asp:CustomValidator ID="CustValComboDatepicker_PubPrint" runat="server" ErrorMessage="Invalid Date Last Printed format."
            ClientValidationFunction="ComboDatepicker_PubPrintDateVal" Display="none" EnableClientScript="true"></asp:CustomValidator>
    </div>
    <div class="editbox datepicker">
        <asp:Label ID="lblDatePubRev" runat="server" Text="Date Publication Revised"></asp:Label>
        <uc1:ComboDatepicker ID="ComboDatepicker_PubRevise" runat="server" LabelName="Date Publication Revised" />
        <asp:CustomValidator ID="CustValComboDatepicker_PubRevise" runat="server" ErrorMessage="Invalid Date Publication Revised format."
            ClientValidationFunction="ComboDatepicker_PubReviseDateVal" Display="none" EnableClientScript="true"></asp:CustomValidator>
    </div>
</div>
<div class="editboxrow">
    <div class="editbox">
        <asp:Label ID="lblNIH" runat="server" Text="NIH #"></asp:Label><asp:TextBox ID="txtNIHNum1" runat="server" MaxLength="2" Columns="4"></asp:TextBox>&nbsp;-&nbsp;<asp:TextBox
            ID="txtNIHNum2" runat="server" MaxLength="5" Columns="8"></asp:TextBox><asp:CustomValidator
                ID="cusValNIHNum" runat="server" Display="none" ErrorMessage="Incorrect NIH # format"
                ClientValidationFunction="ValNIH" EnableClientScript="true"></asp:CustomValidator>
    </div>
    <div class="editbox">
        <asp:Label ID="lblDateArc" runat="server" Text="Date Archived"></asp:Label>
        <asp:TextBox ID="txtArchiveDate" runat="server" MaxLength="10"></asp:TextBox>
        <cc1:CalendarExtender ID="CalExtArchiveDate" runat="server" TargetControlID="txtArchiveDate"
            CssClass="MyCalendar" Format="MM/dd/yyyy">
        </cc1:CalendarExtender>
        <cc1:MaskedEditExtender ID="MaskedEditExtArchiveDate" runat="server" TargetControlID="txtArchiveDate"
            Mask="99/99/9999" MaskType="Date">
        </cc1:MaskedEditExtender>
        <cc1:MaskedEditValidator ID="MaskedEditValArchiveDate" runat="server" ControlToValidate="txtArchiveDate"
            ControlExtender="MaskedEditExtArchiveDate" Display="Dynamic" TooltipMessage=""
            IsValidEmpty="true" EmptyValueMessage="A Date is Required" InvalidValueMessage="Ths date is invalid"></cc1:MaskedEditValidator>
        <asp:Label ID="lblErrmsg" runat="server" Text="" CssClass="error"></asp:Label>
        <asp:Button ID="btnAdd" runat="server" Text="Add Receiving" OnClick="btnAdd_Click"
            Visible="false" />
    </div>
</div>
<asp:DataGrid ID="gvResult" runat="server" AutoGenerateColumns="False"
    AllowSorting="false" AllowPaging="false"
    PageSize="4" ShowFooter="false" ShowHeader="true" OnItemDataBound="gvResult_ItemDataBound"
    OnItemCreated="gvResult_ItemCreated" UseAccessibleHeader="true" CssClass="gray-border valuestable">
    <EditItemStyle HorizontalAlign="Center"  />
    <HeaderStyle CssClass="rowHead" />
    <AlternatingItemStyle CssClass="rowOdd" />
    <ItemStyle CssClass="rowEven"  />
    <Columns>
        <asp:BoundColumn Visible="False" DataField="phdid" ></asp:BoundColumn>
        <asp:BoundColumn Visible="False" DataField="pubid" ></asp:BoundColumn>
        <asp:TemplateColumn HeaderText="Date Received" >
            <ItemTemplate>
                <input id="hiddenDrity_dg" type="hidden" runat="server" value="0" />
                <asp:TextBox ID="txtReceivedDate_dg" runat="server" MaxLength="14" Columns="8"></asp:TextBox>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="Qty Received" >
            <ItemTemplate>
                <asp:TextBox ID="txtQty_dg" runat="server" MaxLength="6" Columns="4"></asp:TextBox>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="NIH #" ItemStyle-Wrap="false" >
            <ItemTemplate>
                <asp:TextBox ID="txtNIHNum1_dg" runat="server" MaxLength="2" Columns="2"></asp:TextBox>&nbsp;-&nbsp;
                <asp:TextBox ID="txtNIHNum2_dg" runat="server" MaxLength="5" Columns="6"></asp:TextBox>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="Date Printed"  ItemStyle-CssClass="datepicker">
            <ItemTemplate>
                <uc1:ComboDatepicker ID="ComboDatepicker_PubPrint_dg" runat="server" LabelName="Date Last Printed" />
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="Date Revised" ItemStyle-CssClass="datepicker">
            <ItemTemplate>
                <uc1:ComboDatepicker ID="ComboDatepicker_PubRevise_dg" runat="server" LabelName="Date Publication Revised" />
            </ItemTemplate>
        </asp:TemplateColumn>
    </Columns>
</asp:DataGrid>