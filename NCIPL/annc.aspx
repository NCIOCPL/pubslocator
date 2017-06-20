<%@ Page Title="Notice of Publication Availability - NCI Publications Locator" Language="C#"
    MasterPageFile="~/pubmaster.Master" AutoEventWireup="true" CodeBehind="nciplhelp.aspx.cs"
    Inherits="PubEnt.help.nciplhelp" %>

<%@ MasterType VirtualPath="~/pubmaster.master" %>
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
            <a id="top"></a>Notice of Publication Availability</h2>
        <p class="textSerpExplain">
            Like all areas of the federal government, the National Cancer Institute has had
            to reduce overall spending. These spending cuts have affected how we distribute
            NCI publications.
        </p>
        <ul class="txt">
            <li>You can view and print most publications from our <a href="http://www.cancer.gov">Cancer.gov</a> website.</li>
            <li>We are adding more publications to our <a href="http://www.cancer.gov/publications/patient-education">eBooks library</a> for download and use on smartphones, tablets, and eBook readers.</li>
            <li>Some publications will no longer be available in hard copy.</li>
            <li>Some publications will be available for ordering in smaller amounts.</li>
            <li>We can only send publications to locations in the U.S., its territories, and military
                and diplomatic posts.</li>
        </ul>
        <h3 class="onthispage">What does this mean for...</h3>
        <ul class="txt">
            <li><a href="#indivs">Patients and their friends or family, and the public</a></li>
            <li><a href="#orgs">Health care providers and organizations</a></li>
            <li><a href="#intlreqs">People outside of the United States</a></li>
        </ul>
        <h3>
            <a id="indivs"></a>Patients and their friends or family, and the public</h3>
        <p>
            If you need hard copy of a publication but do not see the Order option:
        </p>
        <ul class="txt">
            <li>Look for the <strong>View</strong> or <strong>Print</strong> option. Click the link
                and a new browser window or tab will open displaying the Web page or PDF file for
                the publication.</li>
            <li>Use your browser's Print option to print as many copies as you need.</li>
        </ul>
        <p>
            If you cannot print a publication, please <a href="http://www.cancer.gov/global/contact">contact us for help</a>.
        </p>
        <h3>
            <a id="orgs"></a>Health care providers and organizations</h3>
        <p>
            If the publications you give to patients are no longer available for ordering, or
            are not available in the quantity you need, you have several options:
        </p>
        <ul class="txt">
            <li>Visit the NCI Publications Locator website with your patient. Print publications
                or place an order to the patient’s home.</li>
            <li>Give patients a list of publications you reviewed during their visit. Encourage
                them to order or print these items from home.</li>
            <li>Order copies of <em><a href="detail.aspx?prodid=Z207">How Can We Help?, National
                Cancer Institute Points of Access Bookmark</a></em>. This two-sided English and
                Spanish language bookmark lists ways your patients can contact NCI for information
                and publications. There are no shipping charges for this item, so you can order
                enough copies to share while supplies last.</li>
        </ul>
        <p>
            If you still prefer to distribute hard copies of publications directly to your patients,
            use our comparison chart to determine the best printing option. Click the name of
            an option for a detailed description.
        </p>
        <h4>
            <a id="compare"></a>Printing Options Comparison</h4>
        <table class="graphic gray-border table-default printops">
            <thead>
                <tr>
                    <th class="rowHeadLeft">
                        <strong></strong>
                    </th>
                    <th class="rowHead" scope="col">
                        <strong><a href="nciplselfprint.aspx#printsingle">Individual PDF</a></strong>
                    </th>
                    <th class="rowHead" scope="col">
                        <strong><em><a href="nciplselfprint.aspx#printcc">Contents &amp; Covers</a></em></strong>
                    </th>
                    <th class="rowHead" scope="col">
                        <strong><a href="nciplselfprint.aspx#getprintfiles">Print Files (print shop)</a></strong>
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <th class="rowOddLeft" scope="row">
                        <strong>How many can I print &amp; when?</strong>
                    </th>
                    <td class="rowOdd">Print as many copies as you need, when you need them
                    </td>
                    <td class="rowOdd">Print as many copies as you need, when you need them
                    </td>
                    <td class="rowOdd">Order quantity and timing is up to you
                    </td>
                </tr>
                <tr>
                    <th class="rowEvenLeft" scope="row">
                        <strong>What do the products look like?</strong>
                    </th>
                    <td class="rowEven">Products look like photocopies, and are not always designed for 8.5x11 inch paper
                    </td>
                    <td class="rowEven">Products have attractive color covers and are designed for 8.5x11 inch paper
                    </td>
                    <td class="rowEven">Products look exactly like the publications you order from NCI
                    </td>
                </tr>
                <tr>
                    <th class="rowOddLeft" scope="row">
                        <strong>Does my organization actually print the publications?</strong>
                    </th>
                    <td class="rowOdd">
                        <strong>Yes</strong>, staff or volunteers make and assemble copies
                    </td>
                    <td class="rowOdd">
                        <strong>Yes</strong>, staff or volunteers make and assemble copies
                    </td>
                    <td class="rowOdd">
                        <strong>No</strong>, publications come already printed and assembled from a professional
                        print shop
                    </td>
                </tr>
                <tr>
                    <th class="rowEvenLeft" scope="row">
                        <strong>Do I need a place to store the publications?</strong>
                    </th>
                    <td class="rowEven">
                        <strong>No</strong>, no need for storage
                    </td>
                    <td class="rowEven">
                        <strong>No</strong>, no need for storage unless you order covers and store them
                        while you print your own contents
                    </td>
                    <td class="rowEven">
                        <strong>Yes</strong>, you need a place to store all the copies you order
                    </td>
                </tr>
                <tr>
                    <th class="rowOddLeft" scope="row">
                        <strong>Are there shipping costs?</strong>
                    </th>
                    <td class="rowOdd">
                        <strong>No</strong>, no shipping costs
                    </td>
                    <td class="rowOdd">
                        <strong>No</strong>, no shipping costs for orders of 20 or fewer items (each pack
                        of 25 covers is one item)
                    </td>
                    <td class="rowOdd">
                        <strong>Maybe</strong>, depending on the print shop's location and policies
                    </td>
                </tr>
            </tbody>
        </table>
        <h3>
            <a id="intlreqs"></a>People outside of the United States</h3>
        <p>
            We no longer send hard copies of our publications to locations outside of the United
            States, but you can view and print most publications from our <a href="http://www.cancer.gov">Cancer.gov</a> website.
        </p>
        <p>
            If you need hard copy materials and cannot print your own copies of NCI publications,
            you may wish to contact the following organizations to find resources in your country:
        </p>
        <ul class="txt">
            <li><a href="http://www.uicc.org/membership">Union for International Cancer Control
                (UICC)</a> is an international body made of up organizations from around the world
                to help coordinate the global fight against cancer.</li>
            <li><a href="http://icisg.org/membership/membership-list/">International Cancer Information
                Service (ICISG)</a> is a worldwide network of organizations that provide cancer
                information to patients, their family and friends, or public.</li>
        </ul>
        <p class="contentdate">
            <strong>Updated:</strong> 9/8/2014
        </p>
    </div>
</asp:Content>
