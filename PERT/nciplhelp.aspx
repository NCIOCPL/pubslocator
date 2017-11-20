<%@ Page Title="" Language="C#" MasterPageFile="~/pubmaster.Master" AutoEventWireup="true" CodeBehind="nciplhelp.aspx.cs" Inherits="PubEnt.help.nciplhelp" %>

<%@ MasterType VirtualPath="~/pubmaster.master" %>
<%--<%@ Register src="usercontrols/searchbar_search.ascx" tagname="searchbar" tagprefix="uc2" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<div>
    <uc2:searchbar ID="searchbar" runat="server" />
</div>--%>
    <div id="divContactBlock">
        <div class="contactustextdiv">
            <span class="contactustext">&nbsp;</span>
        </div>
    </div>
    <div id="help" class="indentwrap">
        <h2><a id="top"></a>Help Using Publications Enterprise Reporting Tool </h2>
        <div class="contentdate">Updated: 12/4/2013</div>
        <p class="textSerpExplain">
            The NCI Publications Enterprise Reporting Tool (PERT) is a way for NCI and LM staff to view customized reports about the publications that people ordered from 
all Publications Enterprise ordering sources.
        </p>

        <h3 class="onthispage">On this page:</h3>
        <ul class="onthispage">
            <li><a href="#viewing">Viewing reports</a></li>
            <li><a href="#criteria">Report criteria</a></li>
            <li><a href="#values">List of customer types</a></li>
            <li><a href="#export">Exporting reports</a></li>
            <li><a href="#problems">Help with your account</a></li>
            <li><a href="#contact">Contact us for help</a></li>
        </ul>


        <h3><a id="viewing"></a>Viewing reports</h3>
        <p>
            Several Standard Reports and several NCI Special Reports are available. Click one of the report links to display the report criteria. Enter the criteria, then click <strong>Generate
Report</strong>.
        </p>
        <p class="backlink"><a href="#top">back to top</a></p>
        <h3><a id="criteria"></a>Report criteria</h3>
        <ul>
            <li>For the Monthly Distribution and Inventory Report, you cannot enter criteria. This report can be generated only for the most recent complete month. For example, the July 2013 report, which
includes data for the reporting period of July 1 to July 31, 2013, can be generated in August 2013.</li>
            <li>For other Standard Reports, the most recent full month displays in the criteria the first time you visit the report.
You can enter different criteria to view the report for a short or long date range, like a single day or many months.</li>
            <li>For NCI Special Reports, the year-to-date displays in the criteria the first time you visit the report. For example, if it is August 2013, the report will display the criteria 1/1/2013 to 8/1/2013 
when you first visit it, and columns for the first seven months of the year will be included when you generate the report. You can change the criteria to generate the report for one or more months.</li>
        </ul>
        <p class="backlink"><a href="#top">back to top</a></p>
        <h3><a id="values"></a>List of customer types</h3>
        <p>Customer types that do not have any orders during a report period do not display in reports, so all current type of customer values are listed below for reference. </p>
        <p>
            For example, if zero orders were recorded with the 
Hospice Type of Customer during June 2013, when you run the Orders by Type of Customer report 
for June 1-30, then a row for "Hospice" will not display in the generated report.
        </p>
        <h4>Type of Customer values</h4>
        <ul>
            <li>Advocacy Organization</li>
            <li>Cancer Information Service</li>
            <li>Commercial Organization</li>
            <li>Education</li>
            <li>Family/Friend of Patient</li>
            <li>General Public</li>
            <li>Health Professionals</li>
            <li>Hospice</li>
            <li>Information Services/Clearinghouse</li>
            <li>Legislators</li>
            <li>Media</li>
            <li>Medical Center</li>
            <li>NCI Cancer Center</li>
            <li>NCI Exhibit Team</li>
            <li>NCI/NIH Staff</li>
            <li>Other Government Agencies</li>
            <li>Patient</li>
            <li>Patient Service Organization</li>
            <li>Students</li>
        </ul>
        <!--<h4>Order Source values</h4>
<ul><li>CC Email</li>
<li>CC LiveHelp</li>
<li>CC Phone</li>
<li>CC TTY</li>
<li>NCIDC Email</li>
<li>NCIDC Fax</li><li>NCIDC Phone</li>
<li>POBOX Mail</li>
<li>PUB Catalog</li>
<li>Returned Mail</li></ul> -->
        <p class="backlink"><a href="#top">back to top</a></p>

        <h3><a id="export"></a>Exporting reports</h3>
        <p>
            After a report is generated, select a format and click the Export button to save a copy of the report in a format you can view locally in Microsoft Excel or other programs. 
The report you are viewing is the one that will be exported.
        </p>

        <p class="backlink"><a href="#top">back to top</a></p>
        <h3><a id="problems"></a>Help with your account</h3>
        <h4>I can't log in</h4>
        <ul>
            <li>Click <strong>Forgot Password</strong> and follow the steps to get a new password by e-mail.</li>
            <li>If you still can't log in, please <a href="#contact">contact us</a> for help.</li>
        </ul>
        <h4>I want to change my security question</h4>
        <ul>
            <li>Click on your e-mail address (top right of the site).</li>
            <li>Enter your current password.</li>
            <li>Choose your new security question and type the answer.</li>
            <li>Click <strong>Update</strong>.</li>
        </ul>
        <h4>I want to change my password</h4>
        <ul>
            <li>Click on your e-mail address (top right of the site).</li>
            <li>Enter your current password and a new password, then
        click <strong>Change Password</strong>.</li>
        </ul>

        <p class="backlink"><a href="#top">back to top</a></p>
        <h3><a id="contact" ></a>Contact us for help</h3>
        <!--
        <h4>Phone</h4>
        Call us at <strong>1-800-4-CANCER (1-800-422-6237)</strong>. We are available Monday through Friday, from 8:30 a.m. to 5:15 p.m. Eastern time.
        -->
    <h4>E-mail</h4>
        <p>
            E-mail us at <a href="mailto:nci@gpo.gov">nci@gpo.gov</a> if you have a problem using PERT, with questions about the data displayed
    in reports, or if you have updates to information about your publications.
        </p>

        <p class="backlink"><a href="#top">back to top</a></p>
    </div>


</asp:Content>
