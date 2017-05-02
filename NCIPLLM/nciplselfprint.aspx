<%@ Page Title=""
    Language="C#" MasterPageFile="~/pubmaster.Master" AutoEventWireup="true" CodeBehind="nciplselfprint.aspx.cs"
    Inherits="PubEnt.help.nciplselfprint" %>

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
            <a id="top"></a>Self-Printing Options - NCI Publications Locator</h2>
        <div class="contentdate">
            Updated: 6/15/2017</div>
        <p class="textSerpExplain">
            To print most items from your own printer, just look for the Print link.</p>
        <p class="textSerpExplain">
            Need more copies? You have several options for printing or purchasing your own copies
            of NCI publications.</p>
        <h3 class="onthispage">
            On this page:</h3>
        <ul class="onthispage">
            <li><a href="#compare">Printing Options Comparison</a></li>
            <li><a href="#printsingle">Print Individual Copies (PDF, original size)</a></li>
            <li><a href="#printcc">Print <em>Contents &amp; Covers</em> (PDF, 8.5x11 inches)</a></li>
            <li><a href="#getprintfiles">Request Print Files</a></li>
        </ul>
        <h3>
            <a id="compare"></a>Printing Options Comparison</h3>
        <table class="graphic gray-border table-default printops" >
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
                    <td class="rowOdd">
                        Print as many copies as you need, when you need them
                    </td>
                    <td class="rowOdd">
                        Print as many copies as you need, when you need them
                    </td>
                    <td class="rowOdd">
                        Order quantity and timing is up to you
                    </td>
                </tr>
                <tr>
                    <th class="rowEvenLeft" scope="row">
                        <strong>What do the products look like?</strong>
                    </th>
                    <td class="rowEven">
                        Products look like photocopies, and are not always designed for 8.5x11 inch paper
                    </td>
                    <td class="rowEven">
                        Products have attractive color covers and are designed for 8.5x11 inch paper
                    </td>
                    <td class="rowEven">
                        Products look exactly like the publications you order from NCI
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
        <p class="backlink">
            <a href="#top">back to top</a></p>
        <h3>
            <a id="printsingle"></a>Print Individual Copies (PDF, original size)</h3>
        <ol>
            <li>From your search results, click <strong>Print</strong>. </li>
            <li>The item opens in a new window or tab.</li>
            <li>Print as you would any PDF, using the Print icon or your Web browser's Print option.
            </li>
        </ol>
        <p>
            Some items do not have the Print option. To print these publications, from your
            search results, click <strong>View</strong>, then look for your printing options
            in the Page Options box.</p>
        <p class="backlink">
            <a href="#top">back to top</a>
        </p>
        <h3>
            <a id="printcc"></a>Print <em>Contents &amp; Covers</em> (PDF, 8.5x11 inches)</h3>
        <h4>
            What is <em>Contents &amp; Covers</em>?</h4>
        <p>
            <em>Contents &amp; Covers</em> is a way to print and assemble select patient education
            publications <strong>when you need them</strong>. <em>Contents &amp; Covers</em>
            publications are formatted for easy copying on 8.5x11 inch paper.
        </p>
        <ul>
            <li>Create more attractive photocopies.</li>
            <li>Take control of your inventory.</li>
            <li>Fill the gap between how many copies you need and how many you can order from NCI.</li>
        </ul>
        <p>
            From the <a href="home.aspx">NCI Publications Locator homepage</a>, click <em>Contents
                &amp; Covers</em> under <strong>Collections</strong> to find publications currently
            available in this format.</p>
        <h4>
            How does <em>Contents &amp; Covers</em> work?</h4>
        <p>
            Your organization can print both parts of the publication or order covers from NCI:</p>
        <p>
        </p>
        <p>
            <asp:Image ID="Image1" runat="server" AlternateText="Print or order covers, print contents, and assemble your copies"
                ImageUrl="images/helpcontentsandcoversgraphi.jpg" />
        </p>
        <p>
        </p>
        <h4>
            To print both <em>Contents &amp; Covers</em></h4>
        <ol>
            <li>From your search results, click the publication title to go to Publication Details.
            </li>
            <li>On the <strong>Publication Details</strong> page, look for the <strong><em>Contents
                &amp; Covers</em></strong> section.</li>
            <li>Click the <strong>Print Contents</strong> link to print the 8.5x11 inch contents.</li>
            <li>Then, click <strong>Print Cover</strong> to print the 8.5x11 inch cover.</li>
        </ol>
        <h4>
            To order professionally printed covers in packs of 25 from NCI</h4>
        <ol>
            <li>From your search results, click Order Covers. </li>
            <li>Add the cover to your cart.</li>
            <li>Reminders throughout the ordering process will guide you to <strong>download the
                Contents</strong>.
                <!--If you provide your
    e-mail address when ordering, we will also send you a link to the Contents
    PDF.-->
            </li>
        </ol>
        <h4>
            How can I get more information?</h4>
        <p>
            <a href="ContentsAndCoversFactSheet.pdf">The <em>Contents &amp; Covers</em> Fact Sheet</a>
            (PDF) contains more information about this program.</p>
        <p class="backlink">
            <a href="#top">back to top</a>
        </p>
        <h3>
            <a id="getprintfiles"></a>Request Print Files</h3>
        <p>
            To have a printer in your area professionally print copies of NCI publications,
            you may need specially formatted print files (different from the PDF versions on
            our website). We can provide these print files for most of our publications.</p>
        <h4>
            How can I get started?</h4>
        <ul>
            <li>To request print files, e-mail <a href="mailto:ncipoetinfo@mail.nih.gov">ncipoetinfo@mail.nih.gov</a>.
                Remember to include your organization's contact information and the name of the
                publication you want to print. </li>
        </ul>
        <p class="backlink">
            <a href="#top">back to top</a>
        </p>
    </div>
</asp:Content>
