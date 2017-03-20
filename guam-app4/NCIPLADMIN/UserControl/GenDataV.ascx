<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GenDataV.ascx.cs" Inherits="PubEntAdmin.UserControl.GenDataV" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<table>
    <tr>
        <td class="rlabel">
            <asp:Label ID="lblPubID" runat="server" Text="Publication #"></asp:Label>
        <td>
            <asp:Label ID="lblCPJNum" runat="server" Text="" CssClass="shortBox"></asp:Label>
        <td class="rlabel">
            <asp:Label ID="lblNIH" runat="server" Text="NIH Publication #"></asp:Label>
        <td>
            <asp:Label ID="lblNIHNum" runat="server" Text=""></asp:Label>
        <td class="rlabel">
            <asp:Label ID="lblFS" runat="server" Text="FS Number"></asp:Label>
        <td>
            <asp:Label ID="lblFSNum" runat="server" Text=""></asp:Label>
    </tr>
    <tr>
        <td class="rlabel">
            <asp:Label ID="Label2" runat="server" Text="Long Title" CssClass=""></asp:Label>
        <td colspan="2" class="two">
            <asp:Label ID="lblLongTitle" runat="server" Text="" CssClass="longBox"></asp:Label>
        <td class="rlabel">
            <asp:Label ID="Label1" runat="server" Text="Short Title"></asp:Label>
        <td colspan="2" class="two">
            <asp:Label ID="lblShortTitle" runat="server" Text="" CssClass="longBox"></asp:Label>
    </tr>
    <tr>
        <td class="rlabel">
            <asp:Label ID="lblSpanTitleAccents" runat="server" Text="Spanish Title - Accents"></asp:Label>
        <td colspan="2" class="two">
            <asp:Label ID="lblSpanishAccentLongTitle" runat="server" Text="" CssClass="longBox"></asp:Label>
        <td class="rlabel">
            <asp:Label ID="lblSpanTitleNoAccents" runat="server" Text="Spanish Title - No Accents"></asp:Label>
         <td colspan="2" class="two">
            <asp:Label ID="lblSpanishNoAccentLongTitle" runat="server" Text="" CssClass="longBox"></asp:Label>
    </tr>
    <tr>
        <td class="rlabel">
            <asp:Label ID="lblURL" runat="server" Text="URL"></asp:Label>
        <td colspan="5" class="five">
            <asp:HyperLink ID="hylnkURL" runat="server" CssClass="longBox" Target="_blank"></asp:HyperLink>
    </tr>
    <tr>
        <td class="rlabel">
            <asp:Label ID="lblPDFURL" runat="server" Text="PDF URL"></asp:Label>
        <td colspan="5" class="five">
            <asp:HyperLink ID="hylnkPDFURL" runat="server" CssClass="longBox" Target="_blank"></asp:HyperLink>
    </tr>
    <tr>
        <td class="rlabel">
            <asp:Label ID="lblKindleURL" runat="server" Text="Kindle (.mobi) URL"></asp:Label>
        <td colspan="5" class="five">
            <asp:HyperLink ID="hylnkKindleURL" runat="server" CssClass="longBox" Target="_blank"></asp:HyperLink>
    </tr>
    <tr>
        <td class="rlabel">
            <asp:Label ID="lblePubURL" runat="server" Text="E-reader (.epub) URL"></asp:Label>
        <td colspan="5" class="five">
            <asp:HyperLink ID="hylnkePubURL" runat="server" CssClass="longBox" Target="_blank"></asp:HyperLink>
    </tr>
    <tr>
        <td class="rlabel">
            <asp:Label ID="lblPrintFileURL" runat="server" Text="Print File URL"></asp:Label>
        <td colspan="5" class="five">
            <asp:HyperLink ID="hylnkPrintFileURL" runat="server" CssClass="longBox"></asp:HyperLink>
    </tr>
    <tr>
        <td class="rlabel">
            <asp:Label ID="lblNerdos" runat="server" Text="Contents & Covers"></asp:Label>
        <td colspan="5" class="five">
            <asp:HyperLink ID="hylnkNerdoURL" runat="server" CssClass="longBox" Target="_blank"></asp:HyperLink>
    </tr>
    <tr>
        <td class="rlabel">
            <asp:Label ID="lblQCReport" runat="server" Text="QC Report"></asp:Label>
        <td colspan="5" class="five">
            <asp:HyperLink runat="server" NavigateUrl="" Target="_blank" Text="" ID="QCLink">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/Image/wordicon.gif" AlternateText="QC Report" />
            </asp:HyperLink>
    </tr>
</table>
<h4>CPJ</h4>
<table>
    <tr>
        <td class="rlabel">
            <asp:Label ID="lblMaxQty" runat="server" Text="Max Qty./Title"></asp:Label>
        <td>
            <asp:Label ID="lblMaxQtyTile" runat="server" Text="" Style="text-align: left"></asp:Label>
        <td class="rlabel">
            <asp:Label ID="lblQntyAvai" runat="server" Text="Quantity Available"></asp:Label>
        <td>
            <asp:Label ID="lblQtyAvai" runat="server" Text=""></asp:Label>
        <td class="rlabel">
            <asp:Label ID="lblBookStatus" runat="server" Text="Book Status"></asp:Label>
        <td>
            <asp:Label ID="lblBkStatus" runat="server" Text=""></asp:Label>
    </tr>
    <tr>
        <td class="rlabel">
            <asp:Label ID="lblWei" runat="server" Text="Weight"></asp:Label>
        <td>
            <asp:Label ID="lblWeight" runat="server" Text=""></asp:Label>
        <td class="rlabel">
            <asp:Label ID="lblQtyThresh" runat="server" Text="Quantity Threshold"></asp:Label>
        <td colspan="3" class="three">
            <asp:Label ID="lblQtyThreshold" runat="server"></asp:Label>
    </tr>
</table>
<h4>Owner and Sponsor</h4>
<table>
    <tr>
        <td class="rlabel">
            <asp:Label ID="lblOwner" runat="server" Text="Owner"></asp:Label>
        <td>
            <asp:Label ID="lblOwnervalue" runat="server" Text=""></asp:Label>
        <td class="rlabel">
            <asp:Label ID="lblSponsor" runat="server" Text="Sponsor"></asp:Label>
        <td colspan="3" class="three">
            <asp:Label ID="lblSponsorvalue" runat="server" Text=""></asp:Label>
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
