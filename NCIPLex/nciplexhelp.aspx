<%@ Page Title="" Language="C#" MasterPageFile="~/pubmaster.Master" AutoEventWireup="true"
    CodeBehind="nciplexhelp.aspx.cs" Inherits="NCIPLex.help.nciplexhelp" %>

<%@ MasterType VirtualPath="~/pubmaster.master" %>
<%@ Register Src="usercontrols/searchbar_search.ascx" TagName="searchbar" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <uc2:searchbar ID="searchbar" runat="server" />
    </div>
    <div id="help" class="indentwrap">
        <a id="skiptocontent" tabindex="-1"></a>
        <h2>
            <a id="top"></a>Help Using NCI Publications Locator for Exhibits
        </h2>
        <div class="contentdate">
            Updated: 9/16/2014</div>
        <p class="textSerpExplain">
            NCI Publications Locator for Exhibits provides free NCI educational and support
            publications about cancer for patients and their families, health care providers,
            and the public.</p>
        <h3 class="onthispage">
            On this page:</h3>
        <ul class="onthispage">
            <li><a href="#contact">Contact us</a></li>
            <li><a href="#domesticintl">United States and international addresses</a></li>
            <li><a href="#ordering">Ordering</a></li>
            <li><a href="#shipping">Shipping and handling</a></li>
            <li><a href="#apo">APO/FPO shipping addresses</a></li>
        </ul>
        <h3>
            <a id="contact"></a>Contact us</h3>
        <p>
            If you need to order larger quantities or order again after a conference, you have
            a variety of options:
        </p>
        <h4>
            Fax</h4>
        <p>
            Fax orders to us at <strong>719&#45;948&#45;9724</strong>.</p>
        <h4>
            Mail</h4>
        <p>
            National Cancer Institute, NIH, DHHS<br />
            Publications Ordering Service<br />
            PO Box 100<br />
            Pueblo, CO 81002
        <h4>
            Order from home or work</h4>
        <p>
            Within the United States, order through the NCI Publications Locator at <a href="https://pubs.cancer.gov/">
                https://pubs.cancer.gov/</a>.</p>
        <h4>
            Questions about cancer?</h4>
        <p>
            NCI information specialists can help answer your cancer-related questions. Visit
            <a href="http://www.cancer.gov/help">http://www.cancer.gov/help</a> for options
            including phone, live chat and e-mail.
        </p>
        <p class="backlink">
            <a href="#top">back to top</a>
        </p>
        <h3>
            <a id="domesticintl"></a>United States and international addresses</h3>
        <ul>
            <li>The United States includes Puerto Rico, U.S. Virgin Islands, Guam, American Samoa
                and APO/FPO addresses. Orders to other addresses are considered international orders.</li>
            <li>You can place international orders through this website. You must select whether
                you live inside or outside the United States when you start your visit. If you are
                ordering for someone else, choose based on the shipping address you will use.</li>
            <li>If you choose the wrong location or want to clear a selection made by someone using
                the system before you, click the <strong>End This Visit</strong> button to return
                to the main page.</li>
        </ul>
        <p class="backlink">
            <a href="#top">back to top</a>
        </p>
        <h3>
            <a id="ordering"></a>Ordering</h3>
        <h4>
            Ordering publications
        </h4>
        <ol>
            <li>Search or Browse for a publication.</li>
            <li>Click the <strong>Order</strong> button.</li>
            <li>A pop-up window appears. In the Quantity box, type the number of copies you need,
                then click <strong>Add to Cart</strong>.
                <ul>
                    <li>The Limit displayed is the maximum number of copies of an item you can order.</li>
                </ul>
            </li>
            <li>Click the <strong>View Cart</strong> button in the upper right part of the page
                when you are ready to order. In your Shopping Cart, click <strong>Check Out</strong>.</li>
            <li>Enter your shipping information. </li>
            <li>Click <strong>Verify Order</strong> to review your information.</li>
            <li>At this step you can review your entire order before it is submitted. If your order
                is correct, click <strong>Submit Order</strong>.</li>
            <li>Your order is complete and will ship within 2 business days.</li>
        </ol>
        <h4>
            Order limits (total items per order)</h4>
        <ul>
            <li>United States orders are limited to 20 total items.</li>
            <li>International order limits vary per conference.</li>
            <li>You cannot place orders exceeding these limits through this website.</li>
            <li>Order buttons change to read <strong>Limit Reached</strong> if the number of items
                in your cart reaches the order limit. To add additional titles, remove an item from
                your cart or decrease the quantity of an item in your cart.</li>
        </ul>
        <h4>
            Product limits (number of copies)</h4>
        <ul>
            <li>To ensure availability to all, we may limit the number of copies you can order.</li>
            <li>The system will display a message if the quantity you enter exceeds the limit.</li>
        </ul>
        <p class="backlink">
            <a href="#top">back to top</a>
        </p>
        <h3>
            <a id="shipping"></a>Shipping and handling
        </h3>
        <h4>
            Are there shipping and handling charges?
        </h4>
        <ul>
            <li>NCI publications are free.</li>
            <li>There are no shipping and handling charges for orders placed through this website.</li>
            <li>You may place bulk orders by contacting us using other methods. When you place a
                bulk order (usually, more than 20 items), you must provide a FedEx or UPS shipping
                number to pay the actual shipping cost.</li>
        </ul>
        <h4>
            Shipping methods</h4>
        <ul>
            <li>All orders, United States and international, ship within 2 business days via U.S.
                Postal Service.</li>
        </ul>
        <p class="backlink">
            <a href="#top">back to top</a>
        </p>
        <h3>
            <a id="apo"></a>APO/FPO addresses
        </h3>
        <p>
            You can place your order to an APO/FPO address through this website. Follow these
            instructions to enter your shipping information:</p>
        <ol>
            <li>Enter your <strong>ZIP Code</strong>.</li>
            <li><strong>City</strong> field: choose APO or FPO.</li>
            <li><strong>State</strong> field: choose
                <ul>
                    <li>Armed Forces Europe, Middle East, Africa, and Canada</li>
                    <li>Armed Forces Americas, or</li>
                    <li>Armed Forces Pacific.</li>
                </ul>
            </li>
        </ol>
        <p class="backlink">
            <a href="#top">back to top</a>
        </p>
    </div>
</asp:Content>
