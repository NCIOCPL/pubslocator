<%@ Page Title="" Language="C#" MasterPageFile="~/pubmaster.Master" AutoEventWireup="true" CodeBehind="outofstock.aspx.cs" Inherits="PubEnt.outofstock" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ MasterType VirtualPath="~/pubmaster.master" %>
<%@ Register Src="usercontrols/searchbar_search.ascx" TagName="searchbar" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!--[if lt IE 9]>
                    <style>a.openimg:hover{cursor:default}       
                    .modalBackground {filter: alpha(opacity=50); -ms-filter: "progid:DXImageTransform.Microsoft.Alpha(opacity=50)"; }  
                    </style>
    <![endif]-->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:PlaceHolder ID="MessagePlaceHolder" runat="server"></asp:PlaceHolder>
    <div>
        <uc2:searchbar ID="searchbar" runat="server" />
    </div>
    <div id="serp">
        <a id="skiptocontent" tabindex="-1"></a>
        <div>
            <!--Below literal can be dynamically populated if needed-->
            <asp:Literal ID="literalBreadCrumb" runat="server"></asp:Literal>
        </div>
        <div id="divNewUpdated" runat="server" visible="false" class="indentwrap">
            <!--Display explanation for Series/New and Updated pubs in the below literal-->
            <h2>
                <span class="headSerpCriteria">
                    <asp:Literal ID="literalDesc" runat="server"></asp:Literal></span></h2>
            <div class="borderline">
            </div>
            <p>
                               Publications that are currently out of stock.
        </div>
        <div id="divResultsInfo" runat="server" class="indentwrap">
            <h2>
                <asp:Label ID="labelResultsCount" runat="server" Text="" CssClass="headSerpNumresults"></asp:Label>
                <asp:Label ID="labelDisplayText" runat="server" Text="Search Results for:" CssClass="headPageheadSerp"></asp:Label>
                <asp:Label ID="labelSearchCriteria" runat="server" Text="" CssClass="headSerpCriteria"></asp:Label>
            </h2>
        </div>
        <div id="divZeroResults" runat="server" visible="false" class="serpzero">
            <ul>
                <li>Check the spelling of your search criteria</li>
                <li>Try modifying the criteria for this search</li>
                <li>Begin a new search using different keywords</li>
                <li><a href="home.aspx">Browse</a> through a listing of a variety of topics</li>
                <li>Call 1-800-4-CANCER (1-800-422-6237) and choose the option to order publications</li>
            </ul>
            <p>
                If you still can't find what you are seeking, search for your topic on <a href="http://www.cancer.gov/">
                    Cancer.gov</a>.</p>
        </div>
        <div id="divPagerTop" runat="server" class="serppager">
            <div class="refine">
                <!--Refine Search-->
                <%//<asp:HyperLink ID="RefSearchLink" runat="server" NavigateUrl="~/refsearch.aspx">Refine Search</asp:HyperLink>%>
            </div>
            <div class="pager">
                <!--Pager-->
                <!--Begin Sort By-->
                <asp:Label ID="labelSortTop" runat="server" CssClass="textSortControl" Text="Sort by: "
                    AssociatedControlID="DropDownSortResultsTop"></asp:Label>
                <asp:DropDownList ID="DropDownSortResultsTop" runat="server" AutoPostBack="True"
                    OnSelectedIndexChanged="DropDownSortResultsTop_SelectedIndexChanged">
                    <asp:ListItem Selected="True">[Select a value]</asp:ListItem>
                    <asp:ListItem Value="LONGTITLE">Title</asp:ListItem>
                    <asp:ListItem Value="REVISEDDATE">Date</asp:ListItem>
                </asp:DropDownList>
                <!--End Sort By-->
                <!--Begin Number of Results-->
                <asp:Label ID="labelShowTop" runat="server" Text="Show: " AssociatedControlID="DropDownNumResultsTop"></asp:Label>
                <asp:DropDownList ID="DropDownNumResultsTop" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownNumResultsTop_SelectedIndexChanged">
                    <asp:ListItem Selected="True">10</asp:ListItem>
                    <asp:ListItem>20</asp:ListItem>
                    <asp:ListItem>30</asp:ListItem>
                    <asp:ListItem>50</asp:ListItem>
                    <asp:ListItem>100</asp:ListItem>
                </asp:DropDownList>
                <!--End Number of Results-->
                <!--Begin top datapager-->
                <asp:Label ID="labelPageTop" runat="server" Text="Page " CssClass="textSortControl"></asp:Label>
                <asp:DataPager ID="DataPagerTop" runat="server" PagedControlID="ListViewSearchResults"
                    PageSize="10">
                    <Fields>
                        <asp:NextPreviousPagerField ButtonType="Link" ShowLastPageButton="False" ShowNextPageButton="False"
                            ShowPreviousPageButton="True" ShowFirstPageButton="True" ButtonCssClass="textSortControl"
                            PreviousPageText="<< Previous" />
                        <asp:NumericPagerField CurrentPageLabelCssClass="linkPagenavThispage" NumericButtonCssClass="linkPagenav"
                            NextPreviousButtonCssClass="linkPagenavLoud" />
                        <asp:NextPreviousPagerField ButtonType="Link" ShowLastPageButton="True" ShowNextPageButton="True"
                            ShowPreviousPageButton="False" ShowFirstPageButton="False" ButtonCssClass="textSortControl"
                            NextPageText="Next >>" />
                    </Fields>
                </asp:DataPager>
                <!--End top datapager-->
            </div>
        </div>
        <div class="">
            <!--Begin UpdatePanel-->
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <!--Begin ListView-->
                    <asp:ListView ID="ListViewSearchResults" runat="server" OnPagePropertiesChanging="ListViewSearchResults_PagePropertiesChanging"
                        OnItemDataBound="ListViewSearchResults_ItemDataBound" OnPreRender="ListViewSearchResults_PreRender">
                        <LayoutTemplate>
                            <div id="itemPlaceholderContainer" runat="server">
                                <div class="borderline">
                                </div>
                                <span id="itemPlaceholder" runat="server" />
                            </div>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <div class="serpcols">
                                <div class="col serpimage">
                                    <asp:HiddenField ID="PubId" runat="server" />
                                    <asp:HiddenField ID="ProdId" runat="server" />
                                    <asp:HiddenField ID="PubIdCover" runat="server" />
                                    <asp:Image ID="PubImage" runat="server" />
                                    <asp:Panel ID="panelLargeImg" runat="server" CssClass="serpmag" Visible="false">
                                        <asp:HyperLink ID="MagnifierLink" runat="server" ImageUrl="images/magglass.gif" Text="View larger cover image"
                                            Target="_blank" />
                                    </asp:Panel>
                                    <asp:Image ID="NewOrUpdatedImage" runat="server" />
                                </div>
                                <div class="col serptitle">
                                    <asp:HyperLink ID="DetailLink" runat="server" CssClass="linkProdTitle">
                                        <asp:Label ID="PubTitle" runat="server" Text="Label"></asp:Label>
                                    </asp:HyperLink>
                                </div>
                                <div class="col serpdet">
                                    <asp:Label ID="PublicationNumber" runat="server" Text="" CssClass="textDefault"></asp:Label>
                                    <asp:Label ID="NumOfPubPages" runat="server" Text="" CssClass="textDefault"></asp:Label>
                                    <asp:Label ID="PubLastUpdateDate" runat="server" Text="" CssClass="textDefault"></asp:Label>
                                    <asp:Label ID="ProductFormat" runat="server" Text="" CssClass="textDefault"></asp:Label>
                                    <asp:PlaceHolder ID="plcTranslations" runat="server"></asp:PlaceHolder>
                                </div>
                                <div class="col serporder">
                                    <asp:Label ID="labelOrderMsg" runat="server" Text="Order Pub" Visible="False"></asp:Label>
                                    <asp:ImageButton ID="OrderPublication" runat="server" Text="Order Publication" Visible="False"
                                        AlternateText="Order Publication" CommandName="OrderPub" CommandArgument='<%#Eval("PubId")%>'
                                        OnCommand='DisplayModalPopUp' />
                                    <asp:Label ID="textbreaker1" runat="server" Text="<br />" Visible="false"></asp:Label>
                                    <asp:HyperLink ID="HtmlLink" runat="server" Visible="false" Target="_blank" Text="View"
                                        CssClass="readaslinks" />
                                    <asp:Label ID="textbreaker2" runat="server" Text="<br />" Visible="false"></asp:Label>
                                    <asp:HyperLink ID="PdfLink" runat="server" Visible="false" Target="_blank" Text="Print"
                                        CssClass="readaslinks" />
                                    <asp:HyperLink ID="KindleLink" runat="server" Visible="false" Text="<br>Download to Kindle"
                                        CssClass="readaslinks" />
                                    <asp:HyperLink ID="EpubLink" runat="server" Visible="false" Text="<br>Download to other E-readers"
                                        CssClass="readaslinks" />
                                    <br />
                                    <asp:Label ID="labelCoverMsg" runat="server" Text="Order Cover Pub" Visible="False"></asp:Label>
                                    <asp:ImageButton ID="OrderCover" runat="server" Text="Order Cover" Visible="False" CssClass="ordercovers"
                                        AlternateText="Order Cover" CommandName="OrderCoverPub" CommandArgument='<%#Eval("PubIdCover")%>'
                                        OnCommand='DisplayModalPopUpCover' />
                                </div>
                            </div>
                            <div class="borderline">
                            </div>
                        </ItemTemplate>
                        <ItemSeparatorTemplate>
                        </ItemSeparatorTemplate>
                        <EmptyDataTemplate>
                        </EmptyDataTemplate>
                    </asp:ListView>
                    <!--End ListView-->
                </ContentTemplate>
            </asp:UpdatePanel>
            <!--End UpdatePanel-->
        </div>
        <div id="divPagerBottom" runat="server" class="serppager">
            <div class="pager">
                <!--Begin Number of Results-->
                <asp:Label ID="labelShowBottom" runat="server" Text="Show: " AssociatedControlID="DropDownNumResultsBottom"></asp:Label>
                <asp:DropDownList ID="DropDownNumResultsBottom" runat="server" AutoPostBack="True"
                    OnSelectedIndexChanged="DropDownNumResultsBottom_SelectedIndexChanged">
                    <asp:ListItem Selected="True">10</asp:ListItem>
                    <asp:ListItem>20</asp:ListItem>
                    <asp:ListItem>30</asp:ListItem>
                    <asp:ListItem>50</asp:ListItem>
                    <asp:ListItem>100</asp:ListItem>
                </asp:DropDownList>
                <!--End Number of Results-->
                <!--Begin botton datapager-->
                <asp:Label ID="labelPageBottom" runat="server" Text="Page " CssClass="textSortControl"></asp:Label>
                <asp:DataPager ID="DataPagerBottom" runat="server" PagedControlID="ListViewSearchResults"
                    PageSize="10">
                    <Fields>
                        <asp:NextPreviousPagerField ButtonType="Link" ShowLastPageButton="False" ShowNextPageButton="False"
                            ShowPreviousPageButton="True" ShowFirstPageButton="True" ButtonCssClass="textSortControl"
                            PreviousPageText="<< Previous" />
                        <asp:NumericPagerField CurrentPageLabelCssClass="linkPagenavThispage" NumericButtonCssClass="linkPagenav"
                            NextPreviousButtonCssClass="linkPagenavLoud" />
                        <asp:NextPreviousPagerField ButtonType="Link" ShowLastPageButton="True" ShowNextPageButton="True"
                            ShowPreviousPageButton="False" ShowFirstPageButton="False" ButtonCssClass="textSortControl"
                            NextPageText="Next >>" />
                    </Fields>
                </asp:DataPager>
                <!--End botton datapager-->
            </div>
        </div>
    </div>
    <!--end serp-->
    <div>
        <!--Panel and Modal Pop-ups-->
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
                                    <asp:TextBox ID="QuantityOrdered" runat="server" MaxLength="6" EnableViewState="False"
                                        Text="1" Width="50%"></asp:TextBox>
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
                                    <asp:TextBox ID="QuantityOrderedCover" runat="server" MaxLength="6" Text="1" EnableViewState="False"
                                        Width="30%"></asp:TextBox>
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
                <asp:HyperLink ID="PubCoverOrderLink" CssClass="btn goAction" runat="server">
                    
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
