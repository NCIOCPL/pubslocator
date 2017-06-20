<%@ Page Title="" Language="C#" MasterPageFile="~/pubmaster.Master"
    AutoEventWireup="true" CodeBehind="privacypolicy.aspx.cs" Inherits="PubEnt.privacypolicy" %>

<%@ MasterType VirtualPath="~/pubmaster.Master" %>
<%@ Register Src="usercontrols/searchbar_search.ascx" TagName="searchbar" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta name="description" content="Learn about printing and ordering free publications about cancer from the National Cancer Institute. Contact us with questions or for help placing an order." />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <uc2:searchbar ID="searchbar" runat="server" />
    </div>
    <div id="help" class="indentwrap">
        <a id="skiptocontent" tabindex="-1"></a>
        <h2>
            <a name="top"></a>Privacy Policy - NCI Publications Locator
        </h2>
        <div class="contentdate">
            Updated: 6/1/2013</div>
        <p class="textSerpExplain">
            We do not collect personal information unless you choose to give us information
            about yourself.</p>
        <p class="textSerpExplain">
            This page explains our policies that protect your privacy and security when you
            visit the NCI Publications Locator, contact us, and place orders.</p>
        <p class="textSerpExplain">
            For more information about your privacy and security on the National Cancer Institute
            website as well as third party sites and applications that NCI uses (for example,
            Facebook, YouTube), please visit the overall NCI <a class="fontsizeinherit" href="http://www.cancer.gov/global/web/policies/privacy">
                Privacy and Security Policy</a>.
        </p>
        <h3 class="onthispage">
            On this page:</h3>
        <ul class="onthispage">
            <li><a href="#auto">What information do we collect automatically?</a></li>
            <li><a href="#contact">If you contact us, what information do we collect?</a></li>
            <li><a href="#order">If you place an order, what information do we collect?</a></li>
            <li><a href="#retention">How long do we keep your information?</a></li>
            <li><a href="#share">Do we share the information we collect?</a></li>
            <li><a href="#cookies">Do we use cookies or other tracking devices?</a></li>
            <li><a href="#security">Is this site secure?</a></li>
        </ul>
        <h3>
            <a name="auto"></a>What information do we collect automatically?</h3>
        <p>
            When you visit our website, we automatically collect information that does not identify
            you personally. We use Adobe Omniture Web Analytics and Google Analytics to help
            us collect this information.</p>
        <p>
            We know when you visited, what pages you visited, what you searched for on our site,
            and if you came from another website when you started your visit. We can also tell
            the type of browser and operating system you used, and the Internet address you
            used to visit our site.</p>
        <p>
            We use this information to measure the number of new and returning visitors to our
            site and its pages, to help us learn how to make our site more useful. We do not
            use this information to identify individual visitors.</p>
        <p class="backlink">
            <a href="#top">back to top</a>
        </p>
        <h3>
            <a name="contact"></a>If you contact us, what information do we collect?</h3>
        <p>
            If you contact us by phone, fax, mail, or e-mail, we collect only the information
            you give us.
        </p>
        <ul>
            <li>We will use the information you give us to respond to your request and to improve
                our service.</li>
            <li>We will contact you only to respond to your request.</li></ul>
        <p class="backlink">
            <a href="#top">back to top</a>
        </p>
        <h3>
            <a name="order"></a>If you place an order, what information do we collect?</h3>
        <p>
            If you place an order, we collect your shipping information.
        </p>
        <ul>
            <li>We use your shipping information only to process your order. </li>
            <li>We use your e-mail address and phone number only to contact you about your order.
            </li>
        </ul>
        <p>
            If you create an account to order for an organization, we also collect your billing
            information. When you order more than 20 items, we collect your FedEx or UPS shipping
            number.</p>
        <ul>
            <li>We use your billing information only to process your order. </li>
            <li>We use your shipping number only when you place an order, to tell your shipping
                vendor (FedEx or UPS) the actual shipping cost of your order. </li>
            <li>We use your e-mail address only to contact you about your account (for example,
                if you forget your password) and about your order. </li>
        </ul>
        <p class="backlink">
            <a href="#top">back to top</a>
        </p>
        <h3>
            <a name="retention"></a>How long do we keep your information?
        </h3>
        <ul>
            <li>We keep the information we collect automatically from Adobe Omniture and Google
                Analytics only as long as required by law or needed to support the mission of the
                NCI website. </li>
            <li>When you contact us, we keep your information only as long as we need to for the
                reason you contacted us - no longer than 90 days. </li>
            <li>When you order publications, we keep your name and shipping address for 90 days
                in order to follow up if necessary. Then we delete your personal information.
            </li>
            <li>If you create an account to order publications for an organization, we save your
                account information indefinitely to make it easier to order again. We do not save
                FedEx or UPS shipping numbers in your account. </li>
        </ul>
        <p class="backlink">
            <a href="#top">back to top</a>
        </p>
        <h3>
            <a name="share"></a>Do we share the information we collect?</h3>
        <p>
            We report on the information we collect automatically and the publications that
            we send. These reports are only available to staff who need this information to
            do their jobs.</p>
        <p>
            We do not share any personal information about our visitors, unless we are required
            to by law.</p>
        <p class="backlink">
            <a href="#top">back to top</a>
        </p>
        <h3>
            <a name="cookies"></a>Do we use cookies or other tracking devices?</h3>
        <p>
            A cookie is a small text file stored on your computer. We use a session cookie on
            this website to improve navigation and keep track of items you add to your shopping
            cart. It expires when you close your browser.
        </p>
        <p>
            We do not use persistent cookies (cookies that are stored on your computer even
            after you close your browser).</p>
        <p>
            You can block cookies in your browser by <a href="http://www.usa.gov/optout-instructions.shtml">
                opting out</a>. If you block cookies, you might not be able to order publications
            online. You can still <a href="nciplhelp.aspx#contact">contact us</a> to place an
            order.</p>
        <p class="backlink">
            <a href="#top">back to top</a>
        </p>
        <h3>
            <a name="security"></a>Is this site secure?</h3>
        <p>
            Our site is secure. When you place an order, your name, address, shipping number
            and other information is encrypted, which means that no one else can read it as
            it is sent to us over the Internet.</p>
        <p>
            You can be sure that you are on our secure site by checking that the website address
            starts with https://pubs.cancer.gov/. You can also look for a lock icon in the address
            bar in most browsers.</p>
        <p class="backlink">
            <a href="#top">back to top</a>
        </p>
    </div>
</asp:Content>
