<%@ Page Title="" Language="C#" MasterPageFile="~/pubmaster.Master" AutoEventWireup="true"
    CodeBehind="searchres.aspx.cs" Inherits="PubEnt.searchres" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ MasterType VirtualPath="~/pubmaster.master" %>
<%@ Register Src="usercontrols/searchbar_search.ascx" TagName="searchbar" TagPrefix="uc2" %>
<%@ Import Namespace="System.Configuration" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
        var ccOrderLimt = '<%=ConfigurationManager.AppSettings["CCRoleOrderLimit"]%>';  

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

        function IsQtyValueValidForRoleCC(val) {
            var boolValidVal = true;          

            //now check for role cc
            if ('<%=HttpContext.Current.Session["NCIPL_Role"].ToString()%>' == 'NCIPL_CC') {
                var currOrderNum = $("#shopcarditem").text().replace(" Items in your cart", "").replace(" Item in your cart", "");
                if (parseInt(val) + parseInt(currOrderNum) > parseInt(ccOrderLimt))
                    boolValidVal = false;
            }

            return boolValidVal;
        }



        function showHideFooter() {
            if (pageIndex >= pageCount) {
                $("#divContactBlock").show();
                $("#cgvFooter").show();
            }
            else {
                $("#divContactBlock").hide();
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

                var content = "<div class=\"serpcols\">";
                content += "<div class=\"col serpimage\">";
                content += "<img src=\"<%=ConfigurationManager.AppSettings["PubImagesURL"]%>/" + pub.find("pubimage").text() + "\" alt=\"" + pub.find("shorttitle").text() + "(" + pub.find("numqtyavailable").text() + ")\" />";
                content += "<div class=\"serpmag\">";
                if (pub.find("publargeimage").text() != "")
                    content += "<a href=\"dispimage.aspx?prodid=" + pub.find("productid").text() + "\" target=\"_blank\"><img src=\"images/magglass.gif\" alt=\"View larger cover image\" /></a>";
                if (pub.find("pubstatus").text() != "") {
                    if (pub.find("pubstatus").text() == "UPDATED")
                        content += "<img src=\"images/updated.gif\" alt=\"Updated\" /></a>";
                    else if (pub.find("pubstatus").text() == "NEW")
                        content += "<img src=\"images/new.gif\" alt=\"New\" /></a>";
                }
                content += "</div></div>";
                content += "<div class=\"col serptitle\">";
                content += "<a class=\"linkProdTitle\" onclick=\"fnSetScroll()\" href=\"detail.aspx?prodid=" + pub.find("productid").text() + "\">" + pub.find("longtitle").text() + "</a>";
                content += "</div>";
                content += "<div class=\"col serpdet\">";
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
                content += "<div class=\"col serporder\">";

                if (pub.find("CanOrder").text() != "0") {                    

                    //now check for role cc first
                    if ('<%=HttpContext.Current.Session["NCIPL_Role"].ToString()%>' == 'NCIPL_CC') {
                        var currOrderNum = $("#shopcarditem").text().replace(" Items in your cart", "").replace(" Item in your cart", "");
                        if (parseInt(currOrderNum) == parseInt(ccOrderLimt))
                            content += "<input type=\"button\" class=\"btn\" value=\"Limit Reached\" />";
                        else if (!IsItemInCart(pub.find("pubid").text())) {
                            content += "<input type=\"button\" class=\"btn goAction\" id=\"" + pub.find("pubid").text() + "\" value=\"Order Publication\" pubid='" + pub.find("pubid").text() + "' prodid='" + pub.find("productid").text() + "' onclick=\"javascript:fnSetScroll();showOrderWindow(1, this.getAttribute('pubid'),this.id);return false;\" />";
                        }
                        else {                            
                            content += "<input type=\"button\" class=\"btn\" value=\"Publication - In Your Cart\" onclick=\"javascript:document.location.href='cart.aspx';return false;\" />";
                        }
                    }                
                    else {
                        //for other roles other than NCIPL_CC
                        if (!IsItemInCart(pub.find("pubid").text())) {
                            content += "<input type=\"button\" class=\"btn goAction\" id=\"" + pub.find("pubid").text() + "\" value=\"Order Publication\" pubid='" + pub.find("pubid").text() + "' prodid='" + pub.find("productid").text() + "' onclick=\"javascript:fnSetScroll();showOrderWindow(1, this.getAttribute('pubid'),this.id);return false;\" />";
                        }
                        else {                            
                                content += "<input type=\"button\" class=\"btn\" value=\"Publication - In Your Cart\" onclick=\"javascript:document.location.href='cart.aspx';return false;\" />";                            
                        }                
                    }

                    content += "<span><br /></span>";
                }

                if (pub.find("CanView").text() != "0") {
                    if (pub.find("url").text() != "")
                        content += "<a class=\"readaslinksview\" href=\"" + pub.find("url").text() + "\" target=\"_blank\">View</a>&nbsp;";

                    if (pub.find("pdfurl").text() != "")
                        content += "<a class=\"readaslinks\"  href=\"" + pub.find("pdfurl").text() + "\" target=\"_blank\">Print</a>";

                    if (pub.find("kindleurl").text() != "")
                        content += "<br><a class=\"readaslinks\"  href=\"" + pub.find("kindleurl").text() + "\" target=\"_blank\">Download to Kindle</a>";

                    if (pub.find("epuburl").text() != "")
                        content += "<br><a class=\"readaslinks\"  href=\"" + pub.find("epuburl").text() + "\" target=\"_blank\">Download to other E-readers</a>";
                }
                if (pub.find("CanOrderCover").text() != "0") {
                    if (!IsItemInCart(pub.find("pubidcover").text()))
                        content += "<br><input type=\"button\" class=\"btn goAction\" id=\"" + pub.find("pubidcover").text() + "\" value=\"Order Covers\" pubid='" + pub.find("pubidcover").text() + "' prodid='" + pub.find("productid").text() + 'C' + "' onclick=\"javascript:fnSetScroll();showOrderWindow(2, this.getAttribute('pubid'),this.id);return false;\" style=\"margin-top:.77em\"/>";
                    else
                        content += "<br><input type=\"button\" class=\"btn\" value=\"Covers Only - In Your Cart\" onclick=\"javascript:document.location.href='cart.aspx';return false;\" style=\"margin-top:.77em\"/>";
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

            if (!IsQtyValueValid(orderqty, orderlimit)) {
                document.getElementById(lblErrMsgID).innerHTML = "Please enter a valid quantity.";
                return false;
            }
            else if (!IsQtyValueValidForRoleCC(orderqty)) {
                document.getElementById(lblErrMsgID).innerHTML = "Please enter a valid quantity. Limit is " + ccOrderLimt + " total items per order.";
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

                        if (intTotalNum == 1)
                            carttext = "1 Item in your cart ";
                        else
                            carttext = intTotalNum + " Items in your cart ";

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
        <%--<div id="divZeroResults" runat="server" visible="false" class="serpzero">--%>
        <div id="divZeroResults" class="serpzero" style="display: none">
            <ul>
                <li>Check the spelling of your search criteria</li>
                <li>Try <a href="refsearch.aspx">modifying</a> the criteria for this search</li>
                <li>Begin a new search using different keywords</li>
                <li><a href="home.aspx">Browse</a> through a listing of a variety of topics</li>
                <li>Call 1-800-4-CANCER (1-800-422-6237) and choose the option to order publications</li>
            </ul>
            <p>
                If you still can't find what you are seeking, search for your topic on <a href="http://www.cancer.gov/">Cancer.gov</a>.
            </p>
        </div>
        <div id="divPagerTop" runat="server" class="serppager">
            <div class="refine">
                <!--Refine Search-->
                <asp:HyperLink ID="RefSearchLink" runat="server" NavigateUrl="~/refsearch.aspx">Refine Search</asp:HyperLink>
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
                <%--<!--End Sort By-->
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
                </asp:DataPager>--%>
                <!--End top datapager-->
            </div>
        </div>
        <div id="divResult">
        </div>
        <img id="loader" alt="" src="Images/loading.gif" style="display: none" />
    </div>
    <%-- <div class="">
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
        </div>--%>
    <div id="divPagerBottom" runat="server" class="serppager">
        <div class="pager">
            <%--<!--Begin Number of Results-->
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
                <!--End botton datapager-->--%>
        </div>
    </div>
    <div>
        <!--Panel and Modal Pop-ups-->
        <!--Begin Modal Popup and Span-->
        <asp:Button ID="HiddenPopUpButton" runat="server" Text="Button" Style="display: none; visibility: hidden;" />
        <%--<cc1:ModalPopupExtender ID="PubOrderModalPopup" runat="server" PopupControlID="PubOrderPanel"
            TargetControlID="HiddenPopUpButton" OkControlID="PubOrderOK" CancelControlID="PubOrderCancel"
            BackgroundCssClass="modalBackground" BehaviorID="TestBeh">
        </cc1:ModalPopupExtender>--%>
        <cc1:ModalPopupExtender ID="PubOrderModalPopup" runat="server" PopupControlID="PubOrderPanel"
            TargetControlID="HiddenPopUpButton" CancelControlID="PubOrderCancel"
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
                                <%--<asp:Panel ID="pnlPubOrderQuantity" runat="server" DefaultButton="PubOrderOK">--%>
                                <asp:Panel ID="pnlPubOrderQuantity" runat="server">
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
                       
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="orderbtns">
                <%--<asp:HyperLink ID="PubOrderLink" runat="server" CssClass="btn goAction">
                        Add to Cart</asp:HyperLink>--%>
                <input type="button" value="Add to Cart" class="btn goAction" onclick="javascript:return fnOrderClick(1);" />
                <%--<asp:Button ID="PubOrderOK" runat="server" Text="Add to Cart" OnClick="PubOrderOK_Click"
                    Style="display: none;" />--%>
                <asp:Button ID="PubOrderCancel" runat="server" Text="Cancel" CssClass="btn cancelAction" />
            </div>
        </asp:Panel>
    </div>
    <div>
        <asp:Button ID="HiddenCoverPopUpButton" runat="server" Text="Button" Style="display: none" />
        <%--<cc1:ModalPopupExtender ID="PubCoverOrderModalPopup" runat="server" PopupControlID="PubCoverOrderPanel"
            TargetControlID="HiddenCoverPopUpButton" OkControlID="PubCoverOrderOK" CancelControlID="PubCOverOrderCancel"
            BackgroundCssClass="modalBackground" BehaviorID="TestBeh2">
        </cc1:ModalPopupExtender>--%>
        <cc1:ModalPopupExtender ID="PubCoverOrderModalPopup" runat="server" PopupControlID="PubCoverOrderPanel"
            TargetControlID="HiddenCoverPopUpButton" CancelControlID="PubCOverOrderCancel"
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
                                You are ordering the cover only (in packs of 25).
                            </p>
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
                                <%--<asp:Panel ID="pnlPubCoverOrderQuantity" runat="server" DefaultButton="PubCoverOrderOK">--%>
                                <asp:Panel ID="pnlPubCoverOrderQuantity" runat="server">
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
                <%-- <asp:HyperLink ID="PubCoverOrderLink" CssClass="btn goAction" runat="server">
                    
                    Add to Cart
                </asp:HyperLink>--%>
                <input type="button" value="Add to Cart" class="btn goAction" onclick="return fnOrderClick(2);" />
                <%--<asp:Button ID="PubCoverOrderOK" runat="server" Text="Add to Cart" OnClick="PubCoverOrderOK_Click"
                    Style="display: none" />--%>
                <asp:Button ID="PubCoverOrderCancel" runat="server" Text="Cancel" CssClass="btn cancelAction" />
            </div>
        </asp:Panel>
        <!--End Modal Popup and Span-->
        <asp:HiddenField ID="txtPubId" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="txtBtnOrderTrigger" runat="server"></asp:HiddenField>
    </div>
</asp:Content>
