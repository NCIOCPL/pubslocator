<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GenDataE.ascx.cs" Inherits="PubEntAdmin.UserControl.GenDataE" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="NCILibrary.Web.UI.WebControls" Namespace="NCI.Web.UI.WebControls" TagPrefix="nci" %>
<table>
    <tr>
        <td class="rlabel">
            <asp:Label ID="lblPubID" runat="server" Text="Publication # *" AssociatedControlID="txtCPJNum"></asp:Label>
        <td>
            <asp:TextBox ID="txtCPJNum" runat="server" CssClass="" onkeypress="return handleEnter(this, event);"
                TabIndex="1" MaxLength="10"></asp:TextBox>
            <asp:CustomValidator ID="CustValtxtCPJNum" runat="server" ErrorMessage="" Display="None"
                ClientValidationFunction="txtCPJNumVal"></asp:CustomValidator>
            <nci:NoTagsValidator ControlToValidate="txtCPJNum" runat="server" ErrorMessage="Invalid input"></nci:NoTagsValidator>
            <nci:NoScriptValidator ControlToValidate="txtCPJNum" runat="server" ErrorMessage="Invalid input"></nci:NoScriptValidator>
        <td class="rlabel">
            <asp:Label ID="lblNIH" runat="server" Text="NIH Publication #"></asp:Label>
        <td>
            <asp:Label ID="lblNIHNum" runat="server" CssClass=""></asp:Label>
        <td class="rlabel">
            <asp:Label ID="lblFS" runat="server" Text="FS Number" AssociatedControlID="txtFS"></asp:Label>
        <td>
            <asp:TextBox ID="txtFS" runat="server" onkeypress="return handleEnter(this, event);"
                TabIndex="4" MaxLength="6" Columns="8"></asp:TextBox>
            <nci:NoTagsValidator ControlToValidate="txtFS" runat="server" ErrorMessage="Invalid input"></nci:NoTagsValidator>
            <nci:NoScriptValidator ControlToValidate="txtFS" runat="server" ErrorMessage="Invalid input"></nci:NoScriptValidator>
    </tr>
    <tr>
        <td class="rlabel">
            <asp:Label ID="lblLongTitle" runat="server" Text="Long Title *" AssociatedControlID="txtLongTitle"></asp:Label>
        <td colspan="2" class="two">
            <asp:TextBox ID="txtLongTitle" runat="server" Width="97.5%" MaxLength="500"
                onkeypress="return handleEnter(this, event);" TabIndex="3"></asp:TextBox>
            <asp:CustomValidator ID="CustValtxtLongTitle" runat="server" ErrorMessage="" Display="None"
                ClientValidationFunction="txtLongTitleVal"></asp:CustomValidator>
            <nci:NoTagsValidator ControlToValidate="txtLongTitle" runat="server" ErrorMessage="Invalid input"></nci:NoTagsValidator>
            <nci:NoScriptValidator ControlToValidate="txtLongTitle" runat="server" ErrorMessage="Invalid input"></nci:NoScriptValidator>
        <td class="rlabel">
            <asp:Label ID="lblShortTitle" runat="server" Text="Short Title *" AssociatedControlID="txtShortTitle"></asp:Label>
        <td colspan="2" class="two">
            <asp:TextBox ID="txtShortTitle" runat="server" Width="97.5%" MaxLength="42"
                onkeypress="return handleEnter(this, event);" TabIndex="2"></asp:TextBox>
            <asp:CustomValidator ID="CustValtxtShortTitle" runat="server" ErrorMessage="" Display="None"
                ClientValidationFunction="txtShortTitleVal"></asp:CustomValidator>
            <nci:NoTagsValidator ControlToValidate="txtShortTitle" runat="server" ErrorMessage="Invalid input"></nci:NoTagsValidator>
            <nci:NoScriptValidator ControlToValidate="txtShortTitle" runat="server" ErrorMessage="Invalid input"></nci:NoScriptValidator>
    </tr>
    <tr>
        <td class="rlabel">
            <asp:Label ID="lblSpanTitleAccents" runat="server" Text="Spanish Title - Accents"
                AssociatedControlID="txtSpanishAccentLongTitle"></asp:Label>
        <td colspan="2" class="two">
            <asp:TextBox ID="txtSpanishAccentLongTitle" runat="server"  Width="97.5%" MaxLength="300"
                onkeypress="return handleEnter(this, event);" TabIndex="5" OnTextChanged="txtSpanishAccentLongTitle_TextChanged"></asp:TextBox>
            <nci:NoTagsValidator ControlToValidate="txtSpanishAccentLongTitle" runat="server" ErrorMessage="Invalid input"></nci:NoTagsValidator>
            <nci:NoScriptValidator ControlToValidate="txtSpanishAccentLongTitle" runat="server" ErrorMessage="Invalid input"></nci:NoScriptValidator>
        <td class="rlabel">
            <asp:Label ID="lblSpanTitleNoAccents" runat="server" Text="Spanish Title - No Accents"
                AssociatedControlID="txtSpanishNoAccentLongTitle"></asp:Label>
        <td colspan="2" class="two">
            <asp:TextBox ID="txtSpanishNoAccentLongTitle" runat="server"  Width="97.5%" MaxLength="300"
                onkeypress="return handleEnter(this, event);" TabIndex="6" OnTextChanged="txtSpanishNoAccentLongTitle_TextChanged"></asp:TextBox>
            <nci:NoTagsValidator ControlToValidate="txtSpanishNoAccentLongTitle" runat="server" ErrorMessage="Invalid input"></nci:NoTagsValidator>
            <nci:NoScriptValidator ControlToValidate="txtSpanishNoAccentLongTitle" runat="server" ErrorMessage="Invalid input"></nci:NoScriptValidator>
    </tr>
    <tr>
        <td class="rlabel">
            <asp:Label ID="lblURL" runat="server" Text="URL" AssociatedControlID="txtURL"></asp:Label>
        <td colspan="5" class="five">
            <asp:TextBox ID="txtURL" runat="server" Width="99%" MaxLength="500" onkeypress="return handleEnter(this, event);"
                TabIndex="7" OnTextChanged="txtURL_TextChanged"></asp:TextBox>
            <nci:NoTagsValidator ControlToValidate="txtURL" runat="server" ErrorMessage="Invalid input"></nci:NoTagsValidator>
            <nci:NoScriptValidator ControlToValidate="txtURL" runat="server" ErrorMessage="Invalid input"></nci:NoScriptValidator>
    </tr>
    <tr>
        <td class="rlabel">
            <asp:Label ID="lblPDFURL" runat="server" Text="PDF URL" AssociatedControlID="txtPDFURL"></asp:Label>
        <td colspan="5" class="five">
            <asp:TextBox ID="txtPDFURL" runat="server" Width="99%" MaxLength="500" onkeypress="return handleEnter(this, event);"
                TabIndex="8" OnTextChanged="txtPDFURL_TextChanged"></asp:TextBox>
            <nci:NoTagsValidator ControlToValidate="txtPDFURL" runat="server" ErrorMessage="Invalid input"></nci:NoTagsValidator>
            <nci:NoScriptValidator ControlToValidate="txtPDFURL" runat="server" ErrorMessage="Invalid input"></nci:NoScriptValidator>
    </tr>
    <tr>
        <td class="rlabel">
            <asp:Label ID="lblKindleURL" runat="server" Text="Kindle (.mobi) URL" AssociatedControlID="txtKindleURL"></asp:Label>
        <td colspan="5" class="five">
            <asp:TextBox ID="txtKindleURL" runat="server" Width="99%" MaxLength="500" onkeypress="return handleEnter(this, event);"
                TabIndex="9" OnTextChanged="txtKindleURL_TextChanged"></asp:TextBox>
            <nci:NoTagsValidator ControlToValidate="txtKindleURL" runat="server" ErrorMessage="Invalid input"></nci:NoTagsValidator>
            <nci:NoScriptValidator ControlToValidate="txtKindleURL" runat="server" ErrorMessage="Invalid input"></nci:NoScriptValidator>
    </tr>
    <tr>
        <td class="rlabel">
            <asp:Label ID="lblePubURL" runat="server" Text="E-reader (.epub) URL" AssociatedControlID="txtePubURL"></asp:Label>
        <td colspan="5" class="five">
            <asp:TextBox ID="txtePubURL" runat="server" Width="99%" MaxLength="500" onkeypress="return handleEnter(this, event);"
                TabIndex="10" OnTextChanged="txtePubURL_TextChanged"></asp:TextBox>
            <nci:NoTagsValidator ControlToValidate="txtePubURL" runat="server" ErrorMessage="Invalid input"></nci:NoTagsValidator>
            <nci:NoScriptValidator ControlToValidate="txtePubURL" runat="server" ErrorMessage="Invalid input"></nci:NoScriptValidator>
    </tr>
    <tr>
        <td class="rlabel">
            <asp:Label ID="lblPrintFileURL" runat="server" Text="Print File URL" AssociatedControlID="txtPrintFileURL"></asp:Label>
        <td colspan="5" class="five">
            <asp:TextBox ID="txtPrintFileURL" runat="server" Width="99%" MaxLength="500" onkeypress="return handleEnter(this, event);"
                TabIndex="11" OnTextChanged="txtPrintFileURL_TextChanged"></asp:TextBox>
            <nci:NoTagsValidator ControlToValidate="txtPrintFileURL" runat="server" ErrorMessage="Invalid input"></nci:NoTagsValidator>
            <nci:NoScriptValidator ControlToValidate="txtPrintFileURL" runat="server" ErrorMessage="Invalid input"></nci:NoScriptValidator>
    </tr>
    <tr>
        <td class="rlabel">
            <asp:Label ID="lblNerdos" runat="server" Text="Contents & Covers" AssociatedControlID="txtNerdoURL"></asp:Label>
        <td colspan="5" class="five">
            <asp:TextBox ID="txtNerdoURL" runat="server" Width="99%" MaxLength="500" onkeypress="return handleEnter(this, event);"
                TabIndex="12"></asp:TextBox>
            <nci:NoTagsValidator ControlToValidate="txtNerdoURL" runat="server" ErrorMessage="Invalid input"></nci:NoTagsValidator>
            <nci:NoScriptValidator ControlToValidate="txtNerdoURL" runat="server" ErrorMessage="Invalid input"></nci:NoScriptValidator>
    </tr>
    <tr>
        <td class="rlabel">
            <asp:Label ID="lblQCReport" runat="server" Text="QC Report"></asp:Label>
        <td colspan="5" class="five">
            <asp:HyperLink ID="QCLink" runat="server" NavigateUrl="" Target="_blank" Text="">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/Image/wordicon.gif" AlternateText="QC Report" />
            </asp:HyperLink>
    </tr>
</table>
<h4>CPJ</h4>
<table>
    <tr>
        <td class="rlabel">
            <asp:Label ID="lblMaxQty" runat="server" Text="Max Qty./Title" AssociatedControlID="txtMaxQtyTile"></asp:Label>
            <asp:Label ID="lblMaxQtyTile" runat="server" Text="" ></asp:Label>
        <td>
            <asp:TextBox ID="txtMaxQtyTile" runat="server" onkeypress="return handleEnter(this, event);"
                MaxLength="8" TabIndex="18"></asp:TextBox>
            <cc1:FilteredTextBoxExtender ID="ftbetxtMaxQtyTile" runat="server" TargetControlID="txtMaxQtyTile"
                ValidChars="0123456789">
            </cc1:FilteredTextBoxExtender>
            <td class="rlabel">
                <asp:Label ID="lblQntyAvai" runat="server" Text="Quantity Available" AssociatedControlID="txtQtyAvai"></asp:Label>
        <td>
            <asp:TextBox ID="txtQtyAvai" runat="server" onkeypress="return handleEnter(this, event);"
                MaxLength="8" TabIndex="19"></asp:TextBox>
            <cc1:FilteredTextBoxExtender ID="ftbetxtQtyAvai" runat="server" TargetControlID="txtQtyAvai"
                ValidChars="0123456789">
            </cc1:FilteredTextBoxExtender>
            <td class="rlabel">
                <asp:Label ID="lblBookStatus" runat="server" Text="Book Status" AssociatedControlID="ddlBookStatus"></asp:Label>
        <td>
            <asp:DropDownList ID="ddlBookStatus" runat="server" TabIndex="20" EnableViewState="false">
            </asp:DropDownList>
    </tr>
    <tr>
        <td class="rlabel">
            <asp:Label ID="lblWei" runat="server" Text="Weight" AssociatedControlID="txtWeight"></asp:Label>
            <asp:Label ID="lblWeight" runat="server" Text=""></asp:Label>
        <td>
            <asp:TextBox ID="txtWeight" runat="server" onkeypress="return handleEnter(this, event);"
                TabIndex="21"></asp:TextBox>
            <cc1:FilteredTextBoxExtender ID="ftbetxtWeight" runat="server" TargetControlID="txtWeight"
                ValidChars=".0123456789">
            </cc1:FilteredTextBoxExtender>
            <asp:RegularExpressionValidator ID="RegValtxtWeight" runat="server" ControlToValidate="txtWeight"
                ErrorMessage="Invalid Weight Format." ValidationExpression="^(\d|-)?(\d|,)*\.?\d*$"
                Display="Dynamic"></asp:RegularExpressionValidator>
        <td class="rlabel">
            <asp:Label ID="lblQtyThresh" runat="server" Text="Quantity Threshold" AssociatedControlID="txtQtyThresh"></asp:Label>
        <td colspan="3" class="three">
            <asp:TextBox ID="txtQtyThresh" runat="server" onkeypress="return handleEnter(this, event);"
                MaxLength="8" TabIndex="22"></asp:TextBox>
            <cc1:FilteredTextBoxExtender ID="txtQtyThresh_FilteredTextBoxExtender" runat="server"
                TargetControlID="txtQtyThresh" ValidChars="0123456789">
            </cc1:FilteredTextBoxExtender>
    </tr>
</table>
<h4>Owner and Sponsor</h4>
<table>
    <tr>
        <td class="rlabel">
            <asp:Label ID="lblOwner" runat="server" Text="Owner" AssociatedControlID="ddlOwner"></asp:Label>
        <td>
            <asp:DropDownList ID="ddlOwner" runat="server">
            </asp:DropDownList>
        <td class="rlabel">
            <asp:Label ID="lblSponsor" runat="server" Text="Sponsor" AssociatedControlID="ddlSponsor"></asp:Label>
        <td colspan="3" class="three">
            <asp:DropDownList ID="ddlSponsor" runat="server">
            </asp:DropDownList>
            <asp:Label ID="lblOwnerEr" runat="server" CssClass="errorText" Font-Size="Small"
                Visible="false"></asp:Label>
            <asp:Label ID="lblSponsorEr" runat="server" CssClass="errorText" Font-Size="Small"
                Visible="false"></asp:Label>
    </tr>
</table>
<h4>Prod History</h4>
<table>
    <tr>
        <td class="rlabel">
            <asp:Label ID="lblDateLastPubRec" runat="server" Text="Date Last Received"></asp:Label>
        <td>
            <asp:Label ID="lblRecPubDate" runat="server" Text=""></asp:Label>
        <td class="rlabel">
            <asp:Label ID="lblOriPubDate" runat="server" Text="Original Date"></asp:Label>
        <td>
            <asp:Label ID="lblOrigPubDate" runat="server" Text=""></asp:Label>
        <td class="rlabel">
            <asp:Label ID="lblDateLastPrint" runat="server" Text="Date Last Printed"></asp:Label>
        <td>
            <asp:Label ID="lblLatestPrintDate" runat="server" Text=""></asp:Label>
    </tr>
    <tr>
        <td class="rlabel">
            <asp:Label ID="lblDateLastPubRev" runat="server" Text="Date Last Revised"></asp:Label>
        <td>
            <asp:Label ID="lblRevPubDate" runat="server" Text=""></asp:Label>
        <td class="rlabel">
            <asp:Label ID="lblDateArch" runat="server" Text="Date Archived"></asp:Label>
        <td colspan="3" class="three">
            <asp:Label ID="lblArchiveDate" runat="server" Text=""></asp:Label>
    </tr>
</table>
