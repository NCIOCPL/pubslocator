<%@ Page Title=""
    Language="C#" MasterPageFile="~/pubmaster.Master" AutoEventWireup="true" CodeBehind="nciplhelp.aspx.cs"
    Inherits="PubEnt.help.nciplhelp" %>

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
            <a id="top"></a>Help Using NCI Publications Locator
        </h2>
        <div class="contentdate">
            Updated: 9/8/2014</div>
        <p class="textSerpExplain">
            The National Cancer Institute Publications Locator provides free NCI educational
            and support publications about cancer for patients and their families, health care
            providers, and the public.</p>
        <h3 class="onthispage">
            On this page:</h3>
        <ul class="onthispage">
            <li><a href="#ordering">Order online</a></li>
            <li><a href="#limits">Limits on ordering</a></li>
            <li><a href="#charges">Shipping and handling charges</a></li>
            <li><a href="#register">Register for an NCI Publications Locator account</a></li>
            <li><a href="#shipping">Find out when publications will arrive</a></li>
            <li><a href="#intl">Order to an address outside the United States</a></li>
            <li><a href="#problems">Problems with an order</a></li>
            <li><a href="#contact">Contact us for help with ordering</a></li>
        </ul>
        <h3>
            <a id="ordering"></a>Order online</h3>
        <p>
            You can place orders to addresses in the United States through the NCI Publications
            Locator, <a href="https://pubs.cancer.gov">https://pubs.cancer.gov</a>.</p>
        <h4>
            How do I order?</h4>
        <ol>
            <li>Search or browse for a publication. Many cancer topics are listed on the main page.</li>
            <li>Click <strong>Order</strong>.</li>
            <li>A pop-up window appears. If you need more than 1 copy, type the number of copies
                you need. The limit displayed is the maximum number of copies you can order of that
                title.</li>
            <li>Click <strong>Add to Cart</strong>.</li>
            <li>Click <strong>View Cart</strong> (in the top right of the site) when you are ready
                to order.</li>
            <li>In your Shopping Cart, click <strong>Check Out</strong>.</li>
            <li>Enter your shipping information. If you are <a href="#bulkorders">ordering more
                than 20 items</a>, a shipping charge applies and you will be asked to <a href="#register">
                    log in or register</a>.</li>
            <li>Click <strong>Verify Order</strong> to review your entire order before it is submitted.</li>
            <li>If your order is correct, click <strong>Submit Order</strong>.</li>
            <li>Your order is complete and will ship within 2 business days.</li>
        </ol>
        <p class="backlink">
            <a href="#top">back to top</a></p>
        <h3>
            <a id="limits"></a>Limits on ordering</h3>
        <h4>
            How many copies can I order?</h4>
        <ul>
            <li>To make sure publications are available to everyone, we limit the number of copies
                of each title you can order.</li>
            <li>Publications have different order limits based on our inventory. Please check online
                for the most current limit, which is displayed when you order a publication, or
                <a href="#contact">contact us</a> for help.</li>
        </ul>
        <h4>
            How often can I order?</h4>
        <ul>
            <li>Cancer patients, their families and friends, and the general public can order as
                often as necessary to fulfill their individual needs.</li>
            <li>Health professionals and organizations can place one order each calendar month.</li>
        </ul>
        <h4>
            If my organization’s needs exceed the order limit, how can we get more copies?</h4>
        <ul>
            <li>You can print most publications using the <strong>View</strong> or <strong>Print</strong>
                options.</li>
            <li>Selected materials are available with separate <strong>Contents & Covers</strong>,
                for easy 8.5x11 inch copying as you need them.</li>
            <li>To learn more, please visit the <a href="nciplselfprint.aspx">Self-Printing Options</a>
                page on NCI Publications Locator.</li>
        </ul>
        <p class="backlink">
            <a href="#top">back to top</a></p>
        <h3>
            <a id="charges"></a>Shipping and handling charges</h3>
        <p>
            NCI publications are free. There are no handling charges for NCI publications.</p>
        <h4>
            Orders up to 20 items</h4>
        <ul>
            <li>There are no shipping charges for orders up to 20 items. 20 items means 20 copies
                of one publication title, or any combination of titles up to 20 copies in total.</li>
        </ul>
        <h4>
            <a id="bulkorders"></a>Orders of more than 20 items</h4>
        <ul>
            <li>There are shipping charges for orders of more than 20 items.</li>
            <li>You must log in or <a href="#register">register for an NCI Publications Locator
                account</a> and provide a FedEx or UPS shipping number. We will charge the actual
                shipping cost for your order to this number, after the order ships. The FedEx or
                UPS number must be in good standing with the shipping vendor in order to continue
                placing orders with NCI.</li>
            <li>When you place an order, we display the estimated shipping cost based on the shipping
                vendor’s published rate, delivery zone, delivery method, weight of the order, and
                estimated number of packages in the shipment. The actual shipping charge to your
                FedEx or UPS number can be different from our estimate. It can change based on your
                negotiated rate with the shipping vendor and the final number of packages in the
                shipment. </li>
        </ul>
        <p class="backlink">
            <a href="#top">back to top</a></p>
        <h3>
            <a id="register"></a>Register for an NCI Publications Locator account</h3>
        <p>
            If you have more than 20 items in your cart, you will be asked to log in or register
            when you check out.</p>
        <ul>
            <li>Want to log in or register before starting your order? Click the <strong>Login</strong>
                link (top right of the site). </li>
        </ul>
        <h4>
            I already have an account</h4>
        <ul>
            <li>Enter your user name (e-mail address) and password, then click <strong>Login</strong>.</li>
            <li>Trouble? Click <strong>Forgot Password</strong> and follow the steps to get a new
                password by e-mail.</li>
            <li>If you think you have an account but cannot log in, please <a href="#contact">contact
                us</a> for help.</li>
        </ul>
        <h4>
            I want to register for a new account</h4>
        <ul>
            <li>Click <strong>Register</strong> to create a new account.</li>
            <li>Enter an e-mail address as the user name, and choose a security question and answer.
                You'll use this to reset your password if you forget it.</li>
            <li>Use the shipping address where we should send publications, and the billing address,
                if it is different, that will be associated with your FedEx or UPS shipping number.
                (Each time you check out, you will have a chance to review your address information,
                choose a shipping method and provide your FedEx or UPS shipping number.)</li>
            <li>Click <strong>Submit</strong>.</li>
            <li>Your initial password displays on the confirmation page. You can change it now or
                on a subsequent login: click on your e-mail address (top right of the site), click
                <strong>Change Password</strong>, and follow the steps to choose a new password.</li>
        </ul>
        <p class="backlink">
            <a href="#top">back to top</a></p>
        <h3>
            <a id="shipping"></a>Find out when publications will arrive</h3>
        <p>
            Orders ship from the Government Printing Office (GPO) within 2 business days. Please contact <a href="mailto:NCI@gpo.gov">NCI@gpo.gov</a> with any inquiries regarding your order.</p>
        <h4>
            Orders up to 20 items</h4>
        <p>
            Orders up to 20 items ship by U.S. Postal Service. You can expect publications to
            arrive 3-15 business days after shipment.</p>
        <h4>
            Orders of more than 20 items</h4>
        <p>
            Orders of more than 20 items in the United States ship by FedEx or UPS. Use the
            following table to find out when publications will arrive with each shipping option:</p>
        <table class="table-default">
            <thead>
                <tr>
                    <th scope="col">
                        Number of days after shipment to expect publications
                    </th>
                    <th scope="col">
                        Shipping option
                    </th>
                    <th scope="col">
                        Destinations available
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <th scope="row">
                        10:30 a.m. the next day
                    </th>
                    <td>
                        FedEx Priority Overnight or
                        <br />
                        UPS Next Day Air
                    </td>
                    <td>
                        50 United States; Alaska and Hawaii may need extra time
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        End of the 2<sup>nd</sup> business day
                        <br />
                        (4:30 p.m. for FedEx)
                    </th>
                    <td>
                        FedEx 2Day or
                        <br />
                        UPS 2<sup>nd</sup> Day Air
                    </td>
                    <td>
                        50 United States; Alaska and Hawaii may need extra time
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        End of the 3<sup>rd</sup> business day
                        <br />
                        (4:30 p.m. for FedEx)
                    </th>
                    <td>
                        FedEx Express Saver or
                        <br />
                        UPS 3 Day Select
                    </td>
                    <td>
                        Continental 48 United States
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        1–5 business days
                    </th>
                    <td>
                        FedEx Ground or
                        <br />
                        UPS Ground
                    </td>
                    <td>
                        Continental 48 United States
                    </td>
                </tr>
            </tbody>
        </table>
        <p class="backlink">
            <a href="#top">back to top</a></p>
        <h3>
            <a id="intl"></a>Order to an address outside the United States</h3>
        <h4>
            Can people outside the United States order NCI publications?</h4>
        <p>
            No. We cannot send international orders. But, we can send orders to some places
            outside the 50 United States that are not considered international addresses. Use
            the table below to <a href="#intltable">find out if we can send publications</a>
            to your address.</p>
        <h4>
            <a id="apo"></a>Can people in Army Post Office or Fleet Post Office (APO
            or FPO) locations, U.S. territories and possessions order NCI publications?</h4>
        <p>
            Yes. There are some limitations for these orders:</p>
        <ul>
            <li>Orders to these locations ship by U.S. Postal Service and may take longer than 3-15
                business days to arrive.</li>
            <li>There are no shipping charges for these orders, but we cannot send large orders
                or send items by FedEx or UPS.</li>
        </ul>
        <p>
            Use the following table to find out how many publications we can send and how to
            order.</p>
        <table class="table-default" id="intltable">
            <thead>
                <tr>
                    <th class="hthsmall" scope="col">
                        If your address is
                    </th>
                    <th class="hthsmall" scope="col">
                        Number of publications you can order
                    </th>
                    <th class="hthbig" scope="col">
                        How to order
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <th scope="row">
                        APO or FPO
                    </th>
                    <td>
                        30 items total
                    </td>
                    <td>
                        You can order online. To order more than 20 items, you must <a href="#register">register</a>.
                        (After you enter your address, you will no longer be asked for a FedEx or UPS shipping
                        number.)<br />
                        <br />
                        To enter your address, enter the ZIP Code first. Then, choose City APO or FPO and
                        State AE (Armed Forces Europe, Middle East, Africa, and Canada), AA (Armed Forces
                        Americas) or AP (Armed Forces Pacific).
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        U.S. territories and possessions, such as Puerto Rico, U.S. Virgin Islands, Guam,
                        and American Samoa
                    </th>
                    <td>
                        30 items total
                    </td>
                    <td>
                        You can order online. To order more than 20 items, you must <a href="#register">register</a>.
                        (After you enter your address, you will no longer be asked for a FedEx or UPS shipping
                        number.)
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        Another place outside the United States (international orders)
                    </th>
                    <td>
                        None
                    </td>
                    <td>
                        We cannot send international orders. Please visit <a href="nciplselfprint.aspx">Self-Printing
                            Options</a> to learn about printing your own copies.
                    </td>
                </tr>
            </tbody>
        </table>
        <p class="backlink">
            <a href="#top">back to top</a></p>
        <h3>
            <a id="problems"></a>Problems with an order</h3>
        <table class="table-default">
            <thead>
                <tr>
                    <th scope="col">
                        The problem
                    </th>
                    <th scope="col">
                        What will happen
                    </th>
                    <th scope="col">
                        What to do
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <th scope="row">
                        We cannot validate your FedEx or UPS shipping number
                    </th>
                    <td>
                        We will hold your order for 48 hours and try to contact you.
                        <br />
                        We will cancel the order if we cannot validate your shipping number after that time.
                    </td>
                    <td>
                        Respond to our e-mail or phone call so we can correct your shipping number. If 48
                        hours has already passed, place a new order.
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        Questions about a shipping charge
                    </th>
                    <td>
                        Shipping charges are billed directly to your FedEx or UPS shipping number.
                        <br />
                        We cannot resolve billing questions.
                    </td>
                    <td>
                        Please contact the person who handles your FedEx or UPS account.
                    </td>
                </tr>
                <tr>
                    <th scope="row">
                        A mistake with my order
                    </th>
                    <td>
                        We cannot make refunds or offer exchanges on orders after they have been shipped.
                        <br />
                        If a shipping mistake is our fault, we will fix it or replace the order for free.
                    </td>
                    <td>
                        <a href="#contact">Contact us</a> right away if you think you made a mistake or
                        if you do not receive your order as expected.
                    </td>
                </tr>
            </tbody>
        </table>
        <p class="backlink">
            <a href="#top">back to top</a></p>
        <h3>
            <a id="contact"></a>Contact us for help with ordering</h3>
        <h4>
            Fax</h4>
        <p>
            Fax orders to us at <strong>410&#8211;646&#8211;3117</strong>.</p>
        <h4>
            Mail</h4>
        <p>
            National Cancer Institute, NIH, DHHS<br />
            Publications Ordering Service<br />
            PO Box 100<br />
            Pueblo, CO 81002</p>
        <h4>
            E-mail</h4>
        <p>
            E-mail us at <a href="mailto:ncioceocs@mail.nih.gov">ncioceocs@mail.nih.gov</a>
            with questions about ordering, or to submit an order.</p>
        <h4>
            Questions about cancer?</h4>
        <p>
            NCI information specialists can help answer your cancer-related questions. Visit
            <a href="http://www.cancer.gov/help">http://www.cancer.gov/help</a> for options
            including phone, live chat and e-mail.
        </p>
        <p class="backlink">
            <a href="#top">back to top</a></p>
    </div>
</asp:Content>