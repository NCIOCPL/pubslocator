<%@ Page Title="Search Results - NCI Publications Locator" Language="C#" MasterPageFile="~/pubmaster.Master"
    AutoEventWireup="true" CodeBehind="searchres.aspx.cs" Inherits="PubEnt.searchres" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ MasterType VirtualPath="~/pubmaster.master" %>
<%@ Register Src="usercontrols/searchbar_search.ascx" TagName="searchbar" TagPrefix="uc2" %>
<%@ Import Namespace="System.Configuration" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta name="description" content="Order free educational and support publications about cancer. Find resources for patients and their families, health care providers, and the public." />
    <!--[if lt IE 9]>
                    <style>a.openimg:hover{cursor:default}       
                    .modalBackground {filter: alpha(opacity=50); -ms-filter: "progid:DXImageTransform.Microsoft.Alpha(opacity=50)"; }  
                    </style>
    <![endif]-->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="js/jquery-1.6.2.min.js"></script>
    <script type="text/javascript">
        var pageIndex = 0;
        var pageCount = 1;
        var count = 0;
        var sortOrder = 0;

        function GetNextPage() {
            sortOrder = $("#<%=DropDownSortResultsTop.ClientID%>").val();
            if (sortOrder == 'REVISEDDATE') sortOrder = 2;
            else if (sortOrder == 'LONGTITLE') sortOrder = 1;
            else sortOrder = 0;
            pageIndex++;

            if (pageIndex <= pageCount) {
                $("#loader").show();
                $.ajax({
                    type: "POST",
                    url: "searchres.aspx/GetNextPage",
                    data: "{'pageIndex':'" + pageIndex + "','sortOrder':" + sortOrder + "}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: OnSuccess,
                    failure: function (response) {
                        $("#loader").hide();
                        showZeroResult();
                    },
                    error: function (response) {
                        $("#loader").hide();
                        showZeroResult();
                    }
                });
            }

        }

        function showZeroResult() {
            $("#<%=divPagerTop.ClientID%>").hide();
            var criteria = $("#<%=labelSearchCriteria.ClientID%>").html();
            if (criteria == "") {
                $("#<%=labelResultsCount.ClientID%>").show();
                $("#<%=labelDisplayText.ClientID%>").html("Please enter a search term.");
            }
            else {
                $("#<%=labelResultsCount.ClientID%>").html("0");
                $("#<%=labelDisplayText.ClientID%>").show();
            }
            document.getElementById("divZeroResults").style.display = 'block';
        }

        function IsItemInCart(currentPub) {
            var IsInCart = false;
            var PubIdsInCart = '<%=Session["NCIPL_Pubs"]%>';
            if (PubIdsInCart == null)
                return IsInCart;
            var pubs = PubIdsInCart.split(',');
            for (var i in pubs) {
                if (pubs[i] != "") {
                    if (pubs[i] == currentPub) {
                        IsInCart = true;
                        break;
                    }
                }
            }
            return IsInCart;
        }

        function SetBtnStyle() {
            var PubIdsInCart = '<%=Session["NCIPL_Pubs"]%>';
            if (PubIdsInCart == null)
                return;
            var pubs = PubIdsInCart.split(',');
            for (var i in pubs) {
                if (pubs[i] != "") {
                    $("#" + pubs[i]).removeClass("btn goAction").addClass("btn");
                    var v = document.getElementById(pubs[i]).value;
                    if (v == "Order Publication")
                        document.getElementById(pubs[i]).value = "Publication - In Your Cart";
                    else
                        document.getElementById(pubs[i]).value = "Covers Only - In Your Cart";
                }
            }
            return;
        }




        function IsQtyValueValid(val, limit) {
            var boolValidVal = false;
            if (val != '') {
                if (parseInt(val) <= parseInt(limit)) {
                    boolValidVal = true;
                }
                else {
                    boolValidVal = false;
                }
            }
            else
                boolValidVal = false;

            return boolValidVal;
        }

        function showHideFooter() {
            if (pageIndex >= pageCount) {
                $("#cgvFooter").show();
            }
            else {
                $("#cgvFooter").hide();
            }
        }

        function OnSuccess(response) {

            //document.getElementById("msgTest").innerText=response.d;

            var xmlDoc = $.parseXML(response.d);
            var xml = $(xmlDoc);
            pageCount = parseInt(xml.find("PageCount").eq(0).find("PageCount").text());
            Count = parseInt(xml.find("Count").eq(0).find("Count").text());

            var pubs = xml.find("Product");
            if (Count == 1) { $("#<%=labelDisplayText.ClientID%>").html("Search Result for:"); }
            else { $("#<%=labelDisplayText.ClientID%>").html("Search Results for:"); }

            $("#<%=labelResultsCount.ClientID%>").html(Count);

            pubs.each(function () {
                var pub = $(this);
                //create TOP link alongside each pub to ensure it will be present since footer is 
                //hidden while content still loading while scrolling down.
                //in pubmaster script use .eq(0) to show only the first TOP link.
                var content = "<div class=\"serpcols row\"> <a href=\"#top\" class=\"back-to-top\">TOP</a>";
                content += "<div class=\" serpimage columns small-2 medium-1 \">";
                if (pub.find("publargeimage").text() != "") {
                    content += "<a class=\"serpimglink\" href=\"dispimage.aspx?prodid=" + pub.find("productid").text() +
                        "\" target=\"_blank\">";
                }
                content += "<img src=\"<%=ConfigurationManager.AppSettings["PubImagesURL"]%>/" + pub.find("pubimage").text() + "\" alt=\"" + pub.find("shorttitle").text() + " (" + pub.find("numqtyavailable").text() + ")\" />";
                if (pub.find("publargeimage").text() != "") {
                    content += "<div class=\"serpimghover\"><span class=\"magnif\"></span>";
                    content += "</div></a>";
                }
                //content += "<div class=\"serpmag show-for-medium-up\">";
                //if (pub.find("publargeimage").text() != "")
                //    content += "<a href=\"dispimage.aspx?prodid=" + pub.find("productid").text() + "\" target=\"_blank\"><img src=\"images/magglass.gif\" alt=\"View larger cover image\" /></a>";
                if (pub.find("pubstatus").text() != "") {
                    if (pub.find("pubstatus").text() == "UPDATED")
                        content += "<img src=\"images/updated.gif\" alt=\"Updated\" />";
                    else if (pub.find("pubstatus").text() == "NEW")
                        content += "<img src=\"images/new.gif\" alt=\"New\" />";
                }
                //content += "</div></div>";
                content += "</div>";

                content += "<div class=\" serptitle columns small-10 medium-6 large-3 \">";
                content += "<a class=\"linkProdTitle\" onclick=\"fnSetScroll()\" href=\"detail.aspx?prodid=" + pub.find("productid").text() + "\">" + pub.find("longtitle").text() + "</a>";
                content += "</div>";
                content += "<div class=\" serpdet columns large-4 show-for-large-up\">";
                content += "<span class=\"textDefault\">#" + pub.find("productid").text() + "</span>";

                var NumPages = pub.find("numpages").text();

                if (NumPages != "0" && NumPages != "") {
                    if (NumPages == "1") {
                        NumPages = " - " + NumPages + " page";
                    }
                    else {
                        NumPages = " - " + NumPages + " pages";
                    }
                }
                else
                    NumPages = "";
                var RevisedMonth, RevisedDay, RevisedYear, PubLastUpdatedDate;
                RevisedMonth = pub.find("revisedmonth").text();
                RevisedDay = pub.find("revisedday").text();
                RevisedYear = pub.find("revisedyear").text();

                if (RevisedMonth.length > 0 && RevisedDay.length > 0 && RevisedYear.length > 0)
                    PubLastUpdatedDate = RevisedMonth + " " + RevisedDay + ", " + RevisedYear;
                else if (RevisedMonth.length > 0 && RevisedYear.length > 0)
                    PubLastUpdatedDate = RevisedMonth + " " + RevisedYear;
                else if (RevisedYear.length > 0)
                    PubLastUpdatedDate = RevisedYear;
                else
                    PubLastUpdatedDate = "";

                if (PubLastUpdatedDate.length > 0)
                    PubLastUpdatedDate = " - " + PubLastUpdatedDate;

                Format = pub.find("format").text();
                if (Format.length > 0)
                    Format = "<br>Format: " + Format;

                content += "<span class=\"textDefault\">" + NumPages + "</span>";
                content += "<span class=\"textDefault\">" + PubLastUpdatedDate + "</span>";
                content += "<span class=\"textDefault\">" + Format + "</span>";
                content += "<div>";


                if (pub.find("translation").text() != "")
                    content += "<span class=\"textDefault\">Also Available In: </span>" + pub.find("translation").text() + "<span>&nbsp;</span>";

                content += "</div>";
                content += "</div>";
                content += "<div class=\" serporder columns medium-5 large-4 show-for-medium-up\">";

                if (pub.find("CanOrder").text() != "0") {
                    if (!IsItemInCart(pub.find("pubid").text())) {
                        content += "<input type=\"button\" class=\"btn goAction\" id=\"" + pub.find("pubid").text() + "\" value=\"Order Publication\" pubid='" + pub.find("pubid").text() + "' prodid='" + pub.find("productid").text() + "' onclick=\"javascript:_gaq.push(['_trackEvent','Ordering','Submit','Order Item']);NCIAnalytics.PubsLinkTrack('Order Item', this);NCIAnalytics.CartStarted(this.getAttribute('prodid'));fnSetScroll();showOrderWindow(1, this.getAttribute('pubid'),this.id);return false;\" />";
                    }
                    else {
                        content += "<input type=\"button\" class=\"btn\" value=\"Publication - In Your Cart\" onclick=\"javascript:document.location.href='cart.aspx';return false;\" />";
                    }
                    content += "<span><br /></span>";
                }

                if (pub.find("CanView").text() != "0") {

                    if (pub.find("url").text() != "")
                        content += "<a class=\"readaslinksview pub-button icon html\" onclick=\"javascript:_gaq.push(['_trackEvent','External','HTML','HtmlLink']);NCIAnalytics.PubsLinkTrack('View Publication', this);\" href=\"" + pub.find("url").text() + "\" target=\"_blank\"><span>View Online</span></a>&nbsp;";

                    if (pub.find("pdfurl").text() != "")
                        content += "<a class=\"pub-button icon pdf\" onclick=\"javascript:_gaq.push(['_trackEvent','External','PDF','PdfLink']);NCIAnalytics.PubsLinkTrack('Print Publication', this);\" href=\"" + pub.find("pdfurl").text() + "\" target=\"_blank\"><span>Print or View PDF</span></a>";

                    if (pub.find("kindleurl").text() != "")
                        content += "<br><a class=\"pub-button icon kindle\" onclick=\"javascript:_gaq.push(['_trackEvent','External','Kindle','KindleLink']);NCIAnalytics.PubsLinkTrack('Download to Kindle', this);\" href=\"" + pub.find("kindleurl").text() + "\" target=\"_blank\"><span>Download Kindle</span></a>";

                    if (pub.find("epuburl").text() != "")
                        content += "<br><a class=\"pub-button icon epub\" onclick=\"javascript:_gaq.push(['_trackEvent','External','Epub','EpubLink']);NCIAnalytics.PubsLinkTrack('Download to other E-readers', this);\" href=\"" + pub.find("epuburl").text() + "\" target=\"_blank\"><span>Download ePub</span></a>";
                }

                if (pub.find("CanOrderCover").text() != "0") {
                    if (!IsItemInCart(pub.find("pubidcover").text()))
                        content += "<br><input type=\"button\" class=\"btn goAction show-for-large-up\" id=\"" + pub.find("pubidcover").text() + "\" value=\"Order Covers\" pubid='" + pub.find("pubidcover").text() + "' prodid='" + pub.find("productid").text() + 'C' + "' onclick=\"javascript:NCIAnalytics.CartStarted(this.getAttribute('prodid'));fnSetScroll();showOrderWindow(2, this.getAttribute('pubid'),this.id);return false;\" style=\"margin-top:.77em\"/>";
                    else
                        content += "<br><input type=\"button\" class=\"btn show-for-large-up\" value=\"Covers Only - In Your Cart\" onclick=\"javascript:document.location.href='cart.aspx';return false;\" style=\"margin-top:.77em\"/>";
                }
                content += " <br />";
                content += "</div>";
                content += "</div>";
                content += "<div class=\"borderline\">";
                content += "</div>";

                $("#divResult").append(content);
                saveScreenContentToSession();

                showHideFooter();

                $("#loader").hide();
            });


        }

        function showOrderWindow(type, pubid, triggerBtnId) {

            $.ajax({
                type: "POST",
                url: "searchres.aspx/GetOrderWindowData",
                data: "{'pubid':'" + pubid + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    tmp1 = response.d.split("~");
                    document.getElementById('<%=txtBtnOrderTrigger.ClientID %>').value = triggerBtnId;
                    document.getElementById('<%=txtPubId.ClientID %>').value = pubid;

                    if (type == 1) {
                        $("#<%=labelErrMsgPubOrder.ClientID%>").html("");
                        $("#<%=QuantityOrdered.ClientID%>").val(1);
                        $("#<%=labelPubTitle.ClientID%>").html(tmp1[0]);
                        $("#<%=PubLimitLabel.ClientID%>").html('Limit ' + tmp1[1]);
                        $("#<%=PubQtyLimit.ClientID%>").val(tmp1[1]);
                        $find('TestBeh').show();
                    }
                    else if (type == 2) {
                        $("#<%=labelErrMsgPubCover.ClientID%>").html("");
                        $("#<%=QuantityOrderedCover.ClientID%>").val(1);
                        $("#<%=labelCoverPubTitle.ClientID%>").html(tmp1[0]);
                        $("#<%=CoverLimitLabel.ClientID%>").html('Pack of 25 covers&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Limit ' + tmp1[1]);
                        $("#<%=CoverQtyLimit.ClientID%>").val(tmp1[1]);
                        $find('TestBeh2').show();
                    }
                },
                failure: function (response) {
                    alert(response.d);
                },
                error: function (response) {
                    alert(response.d);
                }
            });
    }

    function saveScreenContentToSession() {
        sessionStorage.setItem('content', $("#divResult").html());
        sessionStorage.setItem('pageIndex', pageIndex);
        sessionStorage.setItem('pageCount', pageCount);
    }

    function getScreenContentFromSession() {
        if (sessionStorage && sessionStorage.getItem('content')) {
            pageIndex = sessionStorage.getItem('pageIndex');
            pageCount = sessionStorage.getItem('pageCount')
            return sessionStorage.getItem('content');
        }
        else
            return "";
    }

        <%-- type: 1-pub 2-cover--%>
        function fnOrderClick(type) {

            var pubid, orderqty, orderlimit, strModelName;
            pubid = $("#<%=txtPubId.ClientID%>").val();
            if (type == 1) {
                orderqty = $("#<%=QuantityOrdered.ClientID%>").val();
                orderlimit = $("#<%=PubQtyLimit.ClientID%>").val();
                lblErrMsgID = "<%=labelErrMsgPubOrder.ClientID%>";
                strModelName = 'TestBeh';
            }
            else {
                orderqty = $("#<%=QuantityOrderedCover.ClientID%>").val();
                orderlimit = $("#<%=CoverQtyLimit.ClientID%>").val();
                lblErrMsgID = "<%=labelErrMsgPubCover.ClientID%>";
                strModelName = 'TestBeh2';
            }
       
            //yma make change here to limit the total order qty less than 20
            var cur = $("#shopcarditem").html().replace('(', '').replace(')', '');       
            if (parseInt(cur) + parseInt(orderqty) > 20) {
                alert('The current order quantity in your shopping cart is ' + cur + '. The total order quantity cannot exceed 20 items.');
                return;
            }

            if (!IsQtyValueValid(orderqty, orderlimit)) {
                document.getElementById(lblErrMsgID).innerHTML = "Please enter a valid quantity.";
                return false;
            }
            else {
                $.ajax({
                    type: "POST",
                    url: "searchres.aspx/btnOrderClick",
                    data: "{'pubid':'" + pubid + "', 'orderqty':'" + orderqty + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        btnTriggerId = document.getElementById('<%=txtBtnOrderTrigger.ClientID %>').value;
                        btnTrigger = document.getElementById(btnTriggerId);
                        if (type == 1) {
                            $("#" + btnTriggerId).removeClass("btn goAction").addClass("btn");
                            btnTrigger.value = "Publication - In Your Cart";
                        }
                        else {
                            $("#" + btnTriggerId).removeClass("btn goAction").addClass("btn");
                            btnTrigger.value = "Covers Only - In Your Cart";
                        }
                        btnTrigger.onclick = function (event) {
                            window.location.href = 'cart.aspx';
                            return false;
                        }

                        var PubOrderQtysInCart = response.d;



                        var intTotalNum = 0;

                        if (PubOrderQtysInCart == null)
                            intTotalNum = 0;
                        else {
                            var qtys = PubOrderQtysInCart.split(',');
                            for (var i in qtys) {
                                if (qtys[i] != "") {
                                    intTotalNum += parseInt(qtys[i]);
                                }
                            }
                        }
                        //var md = $(".show-for-medium-only");
                        //if (intTotalNum == 1) {
                        //    if (md.is(":visible")) carttext = "1";
                        //    else carttext = "1 item in your cart ";
                        //}
                        //else {
                        //    if (md.is(":visible")) carttext = intTotalNum;
                        //    else carttext = intTotalNum + " items in your cart ";
                        //}
                        carttext = " (";
                        carttext += intTotalNum;
                        carttext += ")";
                        $("#shopcarditem").html(carttext);

                        saveScreenContentToSession();
                    },
                    failure: function (response) {
                        alert(response.d);
                    },
                    error: function (response) {
                        alert(response.d);
                    }
                });

                    $find(strModelName).hide();
                    return true;
                }
            }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {

            if (document.referrer.indexOf('detail.aspx') != -1 && document.location.href.indexOf('back2result') != -1) {
                var content = getScreenContentFromSession();

                if (content == "") {
                    GetNextPage();
                }
                else {
                    $("#divResult").html(content);
                    SetBtnStyle();
                }
            }
            else {
                GetNextPage();
            }
            showHideFooter();
            $(window).scroll(function (e) {
                if ($(window).scrollTop() + $(window).height() >= $(document).height()) {
                    GetNextPage();
                }
            });

        })
    </script>
    <div id="msgTest">
    </div>
    <asp:PlaceHolder ID="MessagePlaceHolder" runat="server"></asp:PlaceHolder>
    <div>
        <uc2:searchbar ID="searchbar" runat="server" />
    </div>
    <div id="serp">
        <a id="skiptocontent" tabindex="-1"></a>
        <div>
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
                New and updated publications from NCI released in the <strong>last month</strong>.
            </p>
        </div>
        <div id="divResultsInfo" runat="server" class="indentwrap">
            <h2>
                <asp:Label ID="labelResultsCount" runat="server" Text="" CssClass="headSerpNumresults"></asp:Label>
                <asp:Label ID="labelDisplayText" runat="server" Text="Search Results for:" CssClass="headPageheadSerp"></asp:Label>
                <asp:Label ID="labelSearchCriteria" runat="server" Text="" CssClass="headSerpCriteria"></asp:Label>
            </h2>
        </div>
        <div id="divZeroResults" class="serpzero" style="display: none">
            <ul>
                <li>Check the spelling of your search criteria</li>
                <li>Try <a href="refsearch.aspx">modifying</a> the criteria for this search</li>
                <li>Begin a new search using different keywords</li>
                <li><a href="default.aspx">Browse</a> through a listing of a variety of topics</li>
            </ul>
            <p>
                If you still can't find what you are seeking, search for your topic on <a href="http://www.cancer.gov/">Cancer.gov</a>.
            </p>
        </div>
        <div class="show-for-medium-up">
            <div id="divPagerTop" runat="server" class="serppager row">
                <div class="refine columns medium-3 ">
                    <asp:HyperLink ID="RefSearchLink" runat="server" NavigateUrl="~/refsearch.aspx">Refine Search</asp:HyperLink>
                </div>
                <div class="pager columns medium-5 medium-offset-4 large-4 large-offset-5">
                    <asp:Label ID="labelSortTop" runat="server" CssClass="textSortControl" Text="Sort by: "
                        AssociatedControlID="DropDownSortResultsTop"></asp:Label>
                    <asp:DropDownList ID="DropDownSortResultsTop" runat="server" AutoPostBack="True"
                        OnSelectedIndexChanged="DropDownSortResultsTop_SelectedIndexChanged" CssClass="sm_sort">
                        <asp:ListItem Selected="True">[Select a value]</asp:ListItem>
                        <asp:ListItem Value="LONGTITLE">Title</asp:ListItem>
                        <asp:ListItem Value="REVISEDDATE">Date</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div id="divResult">
        </div>
        <img id="loader" alt="" src="Images/loading.gif" style="display: none" />
    </div>
    <div id="divPagerBottom" runat="server" class="serppager">
        <div class="pager">
        </div>
    </div>
    <div id="modalholder">
        <asp:Button ID="HiddenPopUpButton" runat="server" Text="Button" Style="display: none; visibility: hidden;" />
        <cc1:ModalPopupExtender ID="PubOrderModalPopup" runat="server" PopupControlID="PubOrderPanel"
            TargetControlID="HiddenPopUpButton" CancelControlID="PubOrderCancel" BackgroundCssClass="modalBackground"
            BehaviorID="TestBeh" RepositionMode="RepositionOnWindowResizeAndScroll" X="0" Y="0">
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
                                <asp:Panel ID="pnlPubOrderQuantity" runat="server">
                                    <asp:TextBox ID="QuantityOrdered" runat="server" MaxLength="4" EnableViewState="False"
                                        Text="1" CssClass="qtyfield"></asp:TextBox>
                                </asp:Panel>
                                <span style="display: none; visibility: hidden;">
                                    <asp:Label runat="server" ID="lblPubQtyLimit" AssociatedControlID="PubQtyLimit" Text="Limit" />
                                </span>
                                <asp:TextBox ID="PubQtyLimit" CssClass="textQtyLimit" runat="server" Style="display: none; visibility: hidden;"
                                    MaxLength="5" Width="35%"></asp:TextBox>
                                <asp:Label ID="PubLimitLabel" runat="server" Text="" CssClass="textQtyLimit"></asp:Label>
                            </div>
                        </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="orderbtns">
                <input type="button" value="Add to Cart" class="btn goAction" onclick="javascript: _gaq.push(['_trackEvent', 'Ordering', 'Submit', 'Add to Cart']); NCIAnalytics.PubsLinkTrack('Add to Cart', this); return fnOrderClick(1);" />
                <asp:Button ID="PubOrderCancel" runat="server" Text="Cancel" CssClass="btn cancelAction" />
            </div>
        </asp:Panel>
        <asp:Button ID="HiddenCoverPopUpButton" runat="server" Text="Button" Style="display: none" />
        <cc1:ModalPopupExtender ID="PubCoverOrderModalPopup" runat="server" PopupControlID="PubCoverOrderPanel"
            TargetControlID="HiddenCoverPopUpButton" CancelControlID="PubCoverOrderCancel"
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
                            <asp:Label runat="server" ID="lblQuantityOrderedCover" AssociatedControlID="QuantityOrderedCover"
                                Text="Quantity" /></span>
                        <asp:Panel ID="pnlPubCoverOrderQuantity" runat="server">
                            <asp:TextBox ID="QuantityOrderedCover" runat="server" MaxLength="4" Text="1" EnableViewState="False"
                                CssClass="qtyfield"></asp:TextBox>
                        </asp:Panel>
                        <div class="textQtyLimit">
                            <span class="textProdItemtype" style="display: none; visibility: hidden;">Pack of 25
                                    covers</span> <span style="display: none; visibility: hidden;">
                                        <asp:Label runat="server" ID="lblCoverQtyLimit" AssociatedControlID="CoverQtyLimit"
                                            Text="Limit:" /></span>
                            <asp:TextBox ID="CoverQtyLimit" CssClass="textQtyLimit" runat="server" BorderStyle="None"
                                Style="display: none; visibility: hidden;" MaxLength="5"></asp:TextBox>
                            <asp:Label ID="CoverLimitLabel" runat="server" Text="" CssClass="textProdItemtype"></asp:Label>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="orderbtns">
                <input type="button" value="Add to Cart" class="btn goAction" onclick="javascript: _gaq.push(['_trackEvent', 'Ordering', 'Submit', 'Add to Cart']); NCIAnalytics.PubsLinkTrack('Add to Cart', this); return fnOrderClick(2);" />
                <asp:Button ID="PubCoverOrderCancel" runat="server" Text="Cancel" CssClass="btn cancelAction" />
            </div>
        </asp:Panel>
        <asp:HiddenField ID="txtPubId" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="txtBtnOrderTrigger" runat="server"></asp:HiddenField>
    </div>
</asp:Content>
