<%@ Page Title="" Language="C#" MasterPageFile="~/pubmaster.Master" AutoEventWireup="true"
    CodeBehind="detail.aspx.cs" Inherits="NCIPLex.detail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ MasterType VirtualPath="~/pubmaster.master" %>
<%@ Register Src="usercontrols/searchbar_search.ascx" TagName="searchbar" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!--[if lt IE 9]>
                    <style>a.openimg:hover{cursor:default}       
                    .modalBackground {filter: alpha(opacity=50); -ms-filter: "progid:DXImageTransform.Microsoft.Alpha(opacity=50)"}  
                    </style>
    <![endif]-->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hdnReferrer" runat="server" />
    <div>
        <uc2:searchbar ID="searchbar" runat="server" />
    </div>
    <div id="detailscontainer">
        <a id="skiptocontent" tabindex="-1"></a>
        <div id="detailsright">
            <asp:Label ID="labelOrderMsg" runat="server" Text="Order Pub" Visible="False"></asp:Label>
            <asp:ImageButton ID="OrderPublication" runat="server" Text="Order Publication" Visible="False"
                AlternateText="Order Publication" CommandName="OrderPub" OnCommand='DisplayModalPopUp'
                CssClass="DetailOrderButton" />
            <div class="DetailOrderLimit">
                <asp:Label ID="lblOrderLimitText" runat="server" Text="Order Limit&nbsp;"></asp:Label>
                <asp:Label ID="lblOrderLimit" runat="server" CssClass="detailemphasis"></asp:Label></div>
            <asp:HyperLink ID="HtmlLink" runat="server" Visible="false" Target="_blank" Text="View"
                CssClass="DetailViewLink" />
            <asp:HyperLink ID="PdfLink" runat="server" Visible="false" Target="_blank" Text="Print"
                CssClass="DetailViewLink" />
            <asp:Label ID="labelCoverMsg" runat="server" Text="Order Cover Pub" Visible="False"></asp:Label>
            <asp:HyperLink ID="KindleLink" runat="server" Visible="False" Text="Download to Kindle"
                CssClass="DetailViewLink" />
            <asp:HyperLink ID="EpubLink" runat="server" Visible="false" Text="Download to other E-readers"
                CssClass="DetailViewLink" />
            <asp:ImageButton ID="OrderCover" runat="server" Text="Order Cover" Visible="False"
                AlternateText="Order Cover" CommandName="OrderCoverPub" OnCommand='DisplayModalPopUpCover'
                CssClass="DetailOrderButton" />
            <!--<asp:HyperLink ID="NerdoContentsLink" runat="server" Target="_blank" CssClass="DetailViewLink"></asp:HyperLink>
            <asp:HyperLink ID="NerdoCoverLink" runat="server" Target="_blank" CssClass="DetailViewLink"></asp:HyperLink> -->
            <asp:LinkButton ID="lnkBtnSearchRes" runat="server" OnClick="lnkBtnSearchRes_Click"
                Visible="false" CssClass="DetailViewLink morespace">Back to Search Results</asp:LinkButton>
        </div>
        <div id="detailsleft">
            <asp:Panel ID="panelThumbImg" CssClass="DetailsImg" runat="server" Visible="false">
                <asp:Image ID="Image1" runat="server" />
            </asp:Panel>
            <asp:Panel ID="panelLargeImg" CssClass="DetailsImg" runat="server" Visible="false">
                <a href="#magnify" class="openimg">
                    <asp:Image ID="Image2" runat="server" /></a> <span id="magnify"><a class="closeimg"
                        href="#">
                        <asp:Image ID="Image3" runat="server" /></a></span>
                <!--[if lt IE 9]>
                    <asp:HyperLink ID="MagnifierLink"  runat="server" CssClass="magglass"
                    ImageUrl="images/magglass.gif" Text="View larger cover image" Target="_blank" />
                    <![endif]-->
            </asp:Panel>
            <div class="physformatsection">
                <asp:Label ID="lblNumPages" runat="server" Text="Label" CssClass="detailemphasis"></asp:Label>
                <asp:Label ID="lblNumPagesText" runat="server" Text="pages"></asp:Label></div>
            <div class="physformatsection">
                <asp:Label ID="lblFormat" runat="server" CssClass="detailemphasis"></asp:Label> <asp:Label
                    ID="lblFormatText" runat="server" Text="format"></asp:Label></div>
            <div class="physformatsection">
                <asp:Label ID="lblPhysicalDesc" runat="server" Text="" CssClass="detailemphasis"></asp:Label>
            </div>
        </div>
        <div id="detailsctr">
            <asp:PlaceHolder ID="MessagePlaceHolder" runat="server"></asp:PlaceHolder>
            <h2>
                <asp:Label ID="lblTitle" runat="server" CssClass="headPagehead detailsection"></asp:Label></h2>
            <asp:Label ID="lblDesc" runat="server" CssClass="detailsection"></asp:Label>
            <!-- put everything else in the center  -->
            <div class="detailrow">
                <asp:Label ID="lblLangText" runat="server" Text="written in&nbsp;"></asp:Label><asp:Label
                    ID="lblLang" runat="server" CssClass="detailemphasis"></asp:Label>
                <asp:Label ID="lblTranslations" runat="server" Text="- also available in&nbsp;"></asp:Label><asp:PlaceHolder
                    ID="plcTranslations" runat="server"></asp:PlaceHolder>
            </div>
            <div class="detailrow">
                <asp:Label ID="lblAudText" runat="server" Text="for the audience&nbsp;"></asp:Label><asp:Label
                    ID="lblAud" runat="server" CssClass="detailemphasis"></asp:Label></div>
            <div class="detailrow">
                <asp:Label ID="lblLastupdText" runat="server"></asp:Label><asp:Label ID="lblLastupd"
                    runat="server" CssClass="detailemphasis"></asp:Label></div>
            <div class="detailrow">
                <asp:Label ID="lblAwards" runat="server" CssClass="detailemphasis"></asp:Label></div>
            <div class="detailrow">
                <asp:Label ID="lblProductIDText" runat="server" Text="publication number:&nbsp;"></asp:Label><asp:Label
                    ID="lblProductID" runat="server" CssClass="detailemphasis"></asp:Label></div>
            <div class="detailrow">
                <asp:Label ID="lblNIHText" runat="server" Text="NIH number:&nbsp;"></asp:Label><asp:Label
                    ID="lblNIH" runat="server" CssClass="detailemphasis"></asp:Label></div>
            <div class="detailrow collections">
                <div><asp:Label ID="lblPubCollections" runat="server"></asp:Label></div>
                <asp:GridView ID="grdViewPubCollections" runat="server" AutoGenerateColumns="false"
                    EnableTheming="False" EnableViewState="True" GridLines="None" 
                    OnRowDataBound="grdViewPubCollections_RowDataBound" ShowHeader="False">
                    <Columns>
                        <asp:TemplateField ShowHeader="false">
                            <ItemTemplate>
                                <asp:HyperLink ID="PubCollectionLink" NavigateUrl="" Text="" runat="server" CssClass="linkDefault"></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
    <!-- end of detailsctr -->
    <div id="detailsfooter">
        <div id="divRelatedProducts" class='clearFix indentwrap' runat="server">
            <h3>
                Related Products:</h3>
            <asp:DataList ID="dlistRelatedProducs" runat="server" OnItemDataBound="dlistRelatedProducs_ItemDataBound"
                RepeatDirection="Horizontal" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left" ShowFooter="False"
                ShowHeader="False">
                <ItemTemplate>
                    <asp:Image ID="PubImage" runat="server" />
                    <asp:HyperLink ID="DetailLink" runat="server" CssClass="linkProdTitle">
                        <asp:Label ID="PubTitle" runat="server" Text="Label"></asp:Label>
                    </asp:HyperLink>
                </ItemTemplate>
                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                    Font-Underline="False" />
            </asp:DataList>
        </div>
    </div>
    <div>
        <!--Begin Modal Popup and Span-->
        <asp:Button ID="HiddenPopUpButton" runat="server" Text="Button" Style="display: none;
            visibility: hidden;" />
        <cc1:ModalPopupExtender ID="PubOrderModalPopup" runat="server" PopupControlID="PubOrderPanel"
            TargetControlID="HiddenPopUpButton" OkControlID="PubOrderOK" CancelControlID="PubOrderCancel"
            BackgroundCssClass="modalBackground" BehaviorID="TestBeh">
        </cc1:ModalPopupExtender>
        <asp:Panel ID="PubOrderPanel" runat="server" CssClass="modalPopup" Style="display: none">
            <div class="poptitle">
                Add item to cart
            </div>
            <asp:UpdatePanel ID="UpdatePanelOrderPub" runat="server">
                <ContentTemplate>
                    <asp:Label ID="labelErrMsgPubOrder" runat="server" Text="" EnableViewState="False"
                        CssClass="err"></asp:Label><div class="poporder">
                            <asp:Label ID="labelPubTitle" runat="server" CssClass="textProdTitle"></asp:Label>
                            <div class="qty">
                                <span class="labelQty">
                                    <asp:Label runat="server" ID="lblQuantityOrdered" AssociatedControlID="QuantityOrdered"
                                        Text="Quantity" /></span>
                                <asp:Panel ID="pnlPubOrderQuantity" runat="server" DefaultButton="PubOrderOK">
                                    <asp:TextBox ID="QuantityOrdered" runat="server" MaxLength="4" EnableViewState="False"
                                        Text="1" Width="40%"></asp:TextBox>
                                </asp:Panel>
                                <span style="display: none; visibility: hidden;">
                                    <asp:Label runat="server" ID="lblPubQtyLimit" AssociatedControlID="PubQtyLimit" Text="Limit" />
                                </span>
                                <asp:TextBox ID="PubQtyLimit" CssClass="textQtyLimit" runat="server" ReadOnly="True"
                                    Style="display: none; visibility: hidden;" MaxLength="5" Width="35%"></asp:TextBox>
                                <asp:Label ID="PubLimitLabel" runat="server" Text="" CssClass="textQtyLimit"></asp:Label>
                            </div>
                        </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="orderbtns">
                <asp:HyperLink ID="PubOrderLink" runat="server" CssClass="btn goAction">
                      
                        Add to Cart</asp:HyperLink>
                <asp:Button ID="PubOrderOK" runat="server" Text="Add to Cart" OnClick="PubOrderOK_Click"
                    Style="display: none;" />
                <asp:Button ID="PubOrderCancel" runat="server" Text="Cancel" CssClass="btn cancelAction" />
            </div>
        </asp:Panel>
        <asp:Button ID="HiddenCoverPopUpButton" runat="server" Text="Button" Style="display: none" />
        <cc1:ModalPopupExtender ID="PubCoverOrderModalPopup" runat="server" PopupControlID="PubCoverOrderPanel"
            TargetControlID="HiddenCoverPopUpButton" OkControlID="PubCoverOrderOK" CancelControlID="PubCOverOrderCancel"
            BackgroundCssClass="modalBackground" BehaviorID="TestBeh2">
        </cc1:ModalPopupExtender>
        <asp:Panel ID="PubCoverOrderPanel" runat="server" CssClass="modalPopup" Style="display: none">
            <div class="poptitle">
                Add item to cart
            </div>
            <asp:UpdatePanel ID="UpdatePanelOrderCover" runat="server">
                <ContentTemplate>
                    <asp:Label ID="labelErrMsgPubCover" runat="server" Text="" CssClass="err" EnableViewState="False"></asp:Label>
                    <div id="divtitle" class="msg">
                        <strong>Cover Only: </strong>
                        <asp:Label ID="labelCoverPubTitle" runat="server" CssClass=""></asp:Label>
                    </div>
                    <div id="divoutermessage" class="">
                        <div id="divmessage" class="textContentsmsg clearFix">
                            <p>
                                You are ordering the cover only (in packs of 25).</p>
                            <img alt="Download" src="images/download.gif" />
                            <p>
                                <asp:HyperLink ID="linkCoverPubUrl" Target="_blank" runat="server">Print separate contents</asp:HyperLink>
                                now or from the link on your order confirmation page.
                            </p>
                        </div>
                    </div>
                    <div class="msg">
                        Contents &amp; Covers is a way to print and assemble patient education titles when
                        you need them.
                    </div>
                    <table role="presentation" class="cpoporder">
                        <tr>
                            <td class="cimg">
                                <img alt="Contents & Covers" src="images/contentsandcoversgraphic.jpg" />
                            </td>
                            <td class="cqty">
                                <span class="labelQty">
                                    <asp:Label runat="server" ID="lblQuantityOrderedCover" AssociatedControlID="QuantityOrderedCover"
                                        Text="Quantity" /></span>
                                <asp:Panel ID="pnlPubCoverOrderQuantity" runat="server" DefaultButton="PubCoverOrderOK">
                                    <asp:TextBox ID="QuantityOrderedCover" runat="server" MaxLength="4" Text="1" EnableViewState="False"
                                        Width="20%"></asp:TextBox>
                                </asp:Panel>
                                <span class="textProdItemtype" style="display: none; visibility: hidden;">Pack of 25
                                    covers</span> <span style="display: none; visibility: hidden;">
                                        <asp:Label runat="server" ID="lblCoverQtyLimit" AssociatedControlID="CoverQtyLimit"
                                            Text="Limit:" /></span>
                                <asp:TextBox ID="CoverQtyLimit" CssClass="textQtyLimit" runat="server" ReadOnly="True"
                                    BorderStyle="None" Style="display: none; visibility: hidden;" MaxLength="5"></asp:TextBox>
                                <asp:Label ID="CoverLimitLabel" runat="server" Text="" CssClass="textProdItemtype"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="orderbtns">
                <asp:HyperLink ID="PubCoverOrderLink" CssClass="btn goAction" runat="server" >
                    
                    Add to Cart
                </asp:HyperLink>
                <asp:Button ID="PubCoverOrderOK" runat="server" Text="Add to Cart" OnClick="PubCoverOrderOK_Click"
                    Style="display: none" />
                <asp:Button ID="PubCoverOrderCancel" runat="server" Text="Cancel" CssClass="btn cancelAction" />
            </div>
        </asp:Panel>
        <!--End Modal Popup and Span-->
    </div>
</asp:Content>
