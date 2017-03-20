<%@ Page Title="Order free National Cancer Institute publications - NCI Publications Locator"
    Language="C#" MasterPageFile="~/pubmaster.Master" AutoEventWireup="true" CodeBehind="detail.aspx.cs"
    Inherits="PubEnt.detail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ MasterType VirtualPath="~/pubmaster.master" %>
<%@ Register Src="usercontrols/searchbar_search.ascx" TagName="searchbar" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta name="description" content="Order free educational and support publications about cancer. Find resources for patients and their families, health care providers, and the public." />
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
    <a id="skiptocontent" tabindex="-1"></a>
    <div class="row show-for-medium-down smtitle">
        <h2 class="columns ">
            <asp:Label ID="lblTitlesm" runat="server" CssClass=""></asp:Label></h2>
    </div>
    <div id="detailscontainer" class="row">
        <div id="detailstitledesc" class="columns small-9 small-push-3 medium-9 medium-push-3 large-6 large-push-3 ">
            <asp:PlaceHolder ID="MessagePlaceHolder" runat="server"></asp:PlaceHolder>
            <h2 class="show-for-large-up">
                <asp:Label ID="lblTitle" runat="server" CssClass=" detailsection"></asp:Label></h2>
            <asp:Label ID="lblDesc" runat="server" CssClass="detailsection show-for-large-up"></asp:Label>
            <!-- put everything else in the center  -->
            <div class="detailrow show-for-medium-down">
                <asp:Label
                    ID="lblFormatTextsm" runat="server" Text="format: "></asp:Label>
                <asp:Label ID="lblFormatsm" runat="server" CssClass="detailemphasis"></asp:Label>
            </div>
            <div class="detailrow">
                <asp:Label ID="lblLangText" runat="server" Text="written in&nbsp;"></asp:Label><asp:Label
                    ID="lblLang" runat="server" CssClass="detailemphasis"></asp:Label>
                <asp:Label ID="lblTranslations" runat="server" Text="- also available in "></asp:Label><asp:PlaceHolder
                    ID="plcTranslations" runat="server"></asp:PlaceHolder>
            </div>
            <div class="detailrow show-for-medium-up">
                <asp:Label ID="lblAudText" runat="server" Text="for the audience&nbsp;"></asp:Label><asp:Label
                    ID="lblAud" runat="server" CssClass="detailemphasis"></asp:Label>
            </div>
            <div class="detailrow">
                <asp:Label ID="lblLastupdText" runat="server"></asp:Label><asp:Label ID="lblLastupd"
                    runat="server" CssClass="detailemphasis"></asp:Label>
            </div>
            <div class="detailrow show-for-medium-up">
                <asp:Label ID="lblAwards" runat="server" CssClass="detailemphasis"></asp:Label>
            </div>
            <div class="detailrow show-for-large-up">
                <asp:Label ID="lblProductIDText" runat="server" Text="publication number:&nbsp;"></asp:Label><asp:Label
                    ID="lblProductID" runat="server" CssClass="detailemphasis"></asp:Label>
            </div>
            <div class="detailrow show-for-large-up">
                <asp:Label ID="lblNIHText" runat="server" Text="NIH number:&nbsp;"></asp:Label><asp:Label
                    ID="lblNIH" runat="server" CssClass="detailemphasis"></asp:Label>
            </div>
            <div class="detailrow collections show-for-medium-up">
                <div>
                    <asp:Label ID="lblPubCollections" runat="server"></asp:Label>
                </div>
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
        <div id="detailsimages" class="columns small-3 small-pull-9 large-3 medium-3 medium-pull-9 large-pull-6">
            <asp:Panel ID="panelThumbImg" CssClass="DetailsImg" runat="server" Visible="false">
                <asp:Image ID="Image1" runat="server" />
            </asp:Panel>
            <asp:Panel ID="panelLargeImg" CssClass="DetailsImg" runat="server" Visible="false">
               <%-- <a href="#" data-reveal-id="myModal">--%>
                    <asp:Image ID="Image2" runat="server" /><%--</a>--%>
            </asp:Panel>
          <%--  <div id="myModal" class="reveal-modal medium" data-reveal aria-labelledby="modalTitle" aria-hidden="true" role="dialog">
                <h2 id="modalTitle" class="hidden-label">Publication Image</h2>
                <asp:Image ID="Image3" runat="server" />
                <a class="close-reveal-modal" aria-label="Close">&#215;</a>
            </div>--%>
            <div class="physformatsection show-for-large-up">
                <asp:Label ID="lblNumPages" runat="server" Text="Label" CssClass="detailemphasis"></asp:Label>
                <asp:Label ID="lblNumPagesText" runat="server" Text="pages"></asp:Label>
            </div>
            <div class="physformatsection show-for-large-up">
                <asp:Label ID="lblFormat" runat="server" CssClass="detailemphasis"></asp:Label>
                <asp:Label
                    ID="lblFormatText" runat="server" Text="format"></asp:Label>
            </div>
            <div class="physformatsection show-for-large-up">
                <asp:Label ID="lblPhysicalDesc" runat="server" Text="" CssClass="detailemphasis"></asp:Label>
            </div>
        </div>
        <div id="detailsorderoptions" class="columns large-3">
            <asp:Label ID="labelOrderMsg" runat="server" Text="Order Pub" Visible="False"></asp:Label>
            <asp:Button ID="OrderPublication" runat="server" Text="Order Publication" Visible="False"
                CommandName="OrderPub" OnCommand='DisplayModalPopUp' CssClass="btn goAction" />
            <div class="DetailOrderLimit show-for-large-up">
                <asp:Label ID="lblOrderLimitText" runat="server" Text="Order Limit&nbsp;"></asp:Label>
                <asp:Label ID="lblOrderLimit" runat="server" CssClass="detailemphasis"></asp:Label>
            </div>
            <script type="text/javascript">
                var HtmlLink = '<%=varHtmlLink %>';  
            </script>
            <asp:HyperLink ID="HtmlLink" runat="server" Visible="false" Target="_blank" Text="<span>View Online</span>"
                CssClass="DetailViewLink show-for-large-up pub-button icon html" onclick="javascript:_gaq.push(['_trackEvent','External','HTML', HtmlLink]);NCIAnalytics.PubsLinkTrack('View Publication', this);" />
            <asp:HyperLink ID="PdfLink" runat="server" Visible="false" Target="_blank" Text="<span>Print or View PDF</span>"
                CssClass="DetailViewLink show-for-large-up pub-button icon pdf" onclick="javascript:_gaq.push(['_trackEvent','External','PDF','PdfLink']);NCIAnalytics.PubsLinkTrack('Print Publication', this);" />
            <asp:Label ID="labelCoverMsg" runat="server" Text="Order Cover Pub" Visible="False"></asp:Label>
            <asp:HyperLink ID="KindleLink" runat="server" Visible="False" Text="<span>Download Kindle</span>"
                CssClass="DetailViewLink show-for-large-up pub-button icon kindle" onclick="javascript:_gaq.push(['_trackEvent','External','Kindle','KindleLink']);NCIAnalytics.PubsLinkTrack('Download to Kindle', this);" />
            <asp:HyperLink ID="EpubLink" runat="server" Visible="false" Text="<span>Download ePub</span>"
                CssClass="DetailViewLink show-for-large-up pub-button icon epub" onclick="javascript:_gaq.push(['_trackEvent','External','Epub','EpubLink']);NCIAnalytics.PubsLinkTrack('Download to other E-readers', this);" />
            <br />
            <asp:Button ID="OrderCover" runat="server" Text="Order Covers" Visible="False"
                CommandName="OrderCoverPub" OnCommand='DisplayModalPopUpCover'
                CssClass="btn goAction show-for-large-up DetailOrderCover" />
            <asp:HyperLink ID="NerdoContentsLink" runat="server" Target="_blank" CssClass="DetailViewLink cclink show-for-large-up"></asp:HyperLink>
            <asp:HyperLink ID="NerdoCoverLink" runat="server" Target="_blank" CssClass="DetailViewLink cclink show-for-large-up"></asp:HyperLink>
            <asp:LinkButton ID="lnkBtnSearchRes" runat="server" OnClick="lnkBtnSearchRes_Click"
                Visible="false" CssClass="DetailViewLink morespace backsearch show-for-large-up">Back to Search Results</asp:LinkButton>
        </div>
        <div class="row collapse moredet show-for-medium-down">
            <div class="columns">
                <ul class="accordion " data-accordion role="tablist">
                    <li class="accordion-navigation viewlinks">
                        <a href="#panel1c" role="tab" id="panel1c-heading" aria-controls="panel1c">View or Print</a>
                        <div id="panel1c" class="content" role="tabpanel" aria-labelledby="panel1c-heading">
                            <asp:HyperLink ID="HtmlLinksm" runat="server" Visible="false" Target="_blank" Text="<span>View Online</span>"
                                CssClass="DetailViewLink vlink pub-button icon html" onclick="javascript:_gaq.push(['_trackEvent','External','HTML', HtmlLink]);NCIAnalytics.PubsLinkTrack('View Publication', this);" />
                            <asp:HyperLink ID="PdfLinksm" runat="server" Visible="false" Target="_blank" Text="<span>Print or View PDF</span>"
                                CssClass="DetailViewLink plink pub-button icon pdf" onclick="javascript:_gaq.push(['_trackEvent','External','PDF','PdfLink']);NCIAnalytics.PubsLinkTrack('Print Publication', this);" />
                        </div>
                    </li>
                    <li class="accordion-navigation ebooklinks">
                        <a href="#panel4c" role="tab" id="panel4c-heading" aria-controls="panel4c">Download eBook</a>
                        <div id="panel4c" class="content" role="tabpanel" aria-labelledby="panel4c-heading">
                            <asp:HyperLink ID="KindleLinksm" runat="server" Visible="False" Text="<span>Download Kindle</span>"
                                CssClass="DetailViewLink klink pub-button icon kindle" onclick="javascript:_gaq.push(['_trackEvent','External','Kindle','KindleLink']);NCIAnalytics.PubsLinkTrack('Download to Kindle', this);" />
                            <asp:HyperLink ID="EpubLinksm" runat="server" Visible="false" Text="<span>Download ePub</span>"
                                CssClass="DetailViewLink pub-button icon epub" onclick="javascript:_gaq.push(['_trackEvent','External','Epub','EpubLink']);NCIAnalytics.PubsLinkTrack('Download to other E-readers', this);" />

                        </div>
                    </li>
                    <li class="accordion-navigation descpanel">
                        <a href="#panel5c" role="tab" id="panel5c-heading" aria-controls="panel5c">Description</a>
                        <div id="panel5c" class="content" role="tabpanel" aria-labelledby="panel5c-heading">
                            <asp:Label ID="lblDescsm" runat="server" CssClass=""></asp:Label>
                        </div>
                    </li>
                    <li class="accordion-navigation moredetpanel">
                        <a href="#panel7c" role="tab" id="panel7c-heading" aria-controls="panel7c">More Details</a>
                        <div id="panel7c" class="content" role="tabpanel" aria-labelledby="panel7c-heading">
                            <div class="detailrow show-for-small-only">
                                <asp:Label ID="lblAudTextsm" runat="server"
                                    Text="for the audience&nbsp;" ></asp:Label>
                                <asp:Label
                                    ID="lblAudsm" runat="server" CssClass="detailemphasis ">
                                </asp:Label>
                            </div>
                            <div class="detailrow  show-for-small-only">
                                <asp:Label ID="lblAwardssm" runat="server" CssClass="detailemphasis"></asp:Label>
                            </div>
                            <div class="detailrow">
                                <asp:Label ID="lblProductIDTextsm" runat="server" Text="publication number:&nbsp;"></asp:Label><asp:Label
                                    ID="lblProductIDsm" runat="server" CssClass="detailemphasis"></asp:Label>
                            </div>
                            <div class="detailrow">
                                <asp:Label ID="lblNIHTextsm" runat="server" Text="NIH number:&nbsp;"></asp:Label><asp:Label
                                    ID="lblNIHsm" runat="server" CssClass="detailemphasis"></asp:Label>
                            </div>
                        </div>
                    </li>
                </ul>
            </div>
        </div>
    </div>
    <!-- end of detailsctr -->
    <div id="detailsfooter" class="row show-for-large-up">
        <div id="divRelatedProducts" class='clearFix indentwrap' runat="server">
            <h3>Related Products:</h3>
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
    <div id="modalholder">
        <!--Begin Modal Popup and Span-->
        <asp:Button ID="HiddenPopUpButton" runat="server" Text="Button" Style="display: none; visibility: hidden;" />
        <cc1:ModalPopupExtender ID="PubOrderModalPopup" runat="server" PopupControlID="PubOrderPanel"
            TargetControlID="HiddenPopUpButton" OkControlID="PubOrderOK" CancelControlID="PubOrderCancel"
            BackgroundCssClass="modalBackground" BehaviorID="TestBeh" RepositionMode="RepositionOnWindowResizeAndScroll" X="0" Y="0">
        </cc1:ModalPopupExtender>
        <asp:Panel ID="PubOrderPanel" runat="server" CssClass="modalPopup" Style="display: none">
            <h3 class="poptitle">Add item to cart
            </h3>
            <asp:UpdatePanel ID="UpdatePanelOrderPub" runat="server">
                <ContentTemplate>
                    <asp:Label ID="labelErrMsgPubOrder" runat="server" Text="" EnableViewState="False"
                        CssClass="err"></asp:Label><div class="poporder ">
                            <asp:Label ID="labelPubTitle" runat="server" CssClass="textProdTitle"></asp:Label>
                            <div class="qty">
                                <span class="labelQty">
                                    <asp:Label runat="server" ID="lblQuantityOrdered" AssociatedControlID="QuantityOrdered"
                                        Text="Quantity" /></span>
                                <asp:Panel ID="pnlPubOrderQuantity" runat="server" DefaultButton="PubOrderOK">
                                    <asp:TextBox ID="QuantityOrdered" runat="server" MaxLength="4" EnableViewState="False"
                                        Text="1" CssClass="qtyfield"></asp:TextBox>
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
                <asp:HyperLink ID="PubOrderLink" runat="server" CssClass="btn goAction" onclick="javascript:_gaq.push(['_trackEvent','Ordering','Submit','Add to Cart']);NCIAnalytics.PubsLinkTrack('Add to Cart', this);">
                                              Add to Cart</asp:HyperLink>
                <asp:Button ID="PubOrderOK" runat="server" Text="Add to Cart" OnClick="PubOrderOK_Click"
                    Style="display: none;" />
                <asp:Button ID="PubOrderCancel" runat="server" Text="Cancel" CssClass="btn cancelAction" />
            </div>
        </asp:Panel>
        <asp:Button ID="HiddenCoverPopUpButton" runat="server" Text="Button" Style="display: none" />
        <cc1:ModalPopupExtender ID="PubCoverOrderModalPopup" runat="server" PopupControlID="PubCoverOrderPanel"
            TargetControlID="HiddenCoverPopUpButton" OkControlID="PubCoverOrderOK" CancelControlID="PubCoverOrderCancel"
            BackgroundCssClass="modalBackground" BehaviorID="TestBeh2" RepositionMode="RepositionOnWindowResizeAndScroll" X="0" Y="0">
        </cc1:ModalPopupExtender>
        <asp:Panel ID="PubCoverOrderPanel" runat="server" CssClass="modalPopup modalPopupc" Style="display: none">
            <h3 class="poptitle">Add item to cart
            </h3>
            <asp:UpdatePanel ID="UpdatePanelOrderCover" runat="server">
                <ContentTemplate>
                    <asp:Label ID="labelErrMsgPubCover" runat="server" Text="" CssClass="err" EnableViewState="False"></asp:Label>
                    <div id="divtitle" class="msg">
                        <strong>Cover Only: </strong>
                        <asp:Label ID="labelCoverPubTitle" runat="server" CssClass=""></asp:Label>
                    </div>
                    <div id="divoutermessage" class="">
                        <div id="divmessage" class="textContentsmsg clearFix show-for-large-up">
                            <div class="msg">
                                Contents &amp; Covers is a way to print and assemble patient education titles when
                        you need them.
                            </div>
                            <img alt="Contents & Covers" src="images/contentsandcoversgraphic.jpg" />
                        </div>
                    </div>
                    <div class="contentsnotif">
                        <p>
                            You are ordering the cover only (in packs of 25).
                        </p>
                        <img alt="Download" src="images/download.gif" />
                        <p>
                            <asp:HyperLink ID="linkCoverPubUrl" Target="_blank" runat="server">Print separate contents</asp:HyperLink>
                            now or from the link on your order confirmation page.
                        </p>
                    </div>
                    <div class="qty cqty">
                        <span class="labelQty">
                            <asp:Label runat="server" ID="lblQuantityOrderedCover"
                                AssociatedControlID="QuantityOrderedCover"
                                Text="Quantity" /></span>
                        <asp:Panel ID="pnlPubCoverOrderQuantity" runat="server" DefaultButton="PubCoverOrderOK">
                            <asp:TextBox ID="QuantityOrderedCover" runat="server" MaxLength="4" Text="1" EnableViewState="False"
                                CssClass="qtyfield"></asp:TextBox>
                        </asp:Panel>
                        <div class="textQtyLimit">
                            <span class="textProdItemtype" style="display: none; visibility: hidden;">Pack of 25
                                    covers</span> <span style="display: none; visibility: hidden;">
                                        <asp:Label runat="server" ID="lblCoverQtyLimit" AssociatedControlID="CoverQtyLimit"
                                            Text="Limit:" /></span>
                            <asp:TextBox ID="CoverQtyLimit" CssClass="textQtyLimit" runat="server" ReadOnly="True"
                                BorderStyle="None" Style="display: none; visibility: hidden;" MaxLength="5"></asp:TextBox>
                            <asp:Label ID="CoverLimitLabel" runat="server" Text="" CssClass="textProdItemtype"></asp:Label>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="orderbtns">
                <asp:HyperLink ID="PubCoverOrderLink" CssClass="btn goAction" runat="server" onclick="javascript:_gaq.push(['_trackEvent','Ordering','Submit','Add to Cart']);NCIAnalytics.PubsLinkTrack('Add to Cart', this);">
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
