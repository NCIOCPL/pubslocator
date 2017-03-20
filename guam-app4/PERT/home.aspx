<%@ Page Title="" Language="C#" MasterPageFile="~/pubmaster.Master" AutoEventWireup="true"
    CodeBehind="home.aspx.cs" Inherits="PubEnt.home" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ MasterType VirtualPath="~/pubmaster.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!--[if lt IE 10]>
<style>#idViewer table {width:1% !important; }
        #idViewer table td {padding-right:2px !important;}</style>
<![endif]-->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        $(document).ready(function () {
            showhide();
            $('.make_calendar').datepicker({ minDate: new Date(2009, 0, 1) });
        });
        function jsOnClick(i) {
            $('#<%=hidRpt.ClientID%>').val(i);
            showhide();
            $('#idViewer').hide();
        }
        function showhide() {
            /*for criteria*/
            $('.criteria').hide();
            $($('#<%=hidRpt.ClientID%>').val()).show();
            /*for href link background*/
            $('.hrefclass').css('background-color', '#ffffff');
            var v = $('#<%=hidRpt.ClientID%>').val() + '_href';
            $(v).css('background-color', '#f9f5c6');
        }
    </script>

    <div class="leftcol rptcolumn">
        <h2>Available Reports</h2>
        <asp:Panel ID="pnStandardRpt" runat="server">
            <h3>Standard Reports</h3>
            <div id="menu">
                <div id="menubody">
                    <ul class="menulist">
                        <li><a href="#" id="idMonthDis_href" onclick="javascript:jsOnClick('#idMonthDis');return false;"
                            class="hrefclass">Monthly Distribution & Inventory Report</a></li>
                        <li><a href="#" id="idOrderBySource_href" onclick="javascript:jsOnClick('#idOrderBySource');return false;"
                            class="hrefclass">Orders by Source</a></li>
                        <li><a href="#" id="idOrderByType_href" onclick="javascript:jsOnClick('#idOrderByType');return false;"
                            class="hrefclass">Orders by Type of Customer</a></li>
                        <li><a href="#" id="idOrderBySource_Intl" onclick="javascript:jsOnClick('#idOrderByIntl');return false;"
                            class="hrefclass">International Orders</a></li>
                    </ul>
                </div>
            </div>
        </asp:Panel>
        <h3>NCI Special Reports</h3>
        <div id="Div1">
            <div id="Div2">
                <ul id="Ul1" class="menulist">
                    <li><a href="#" id="A5" onclick="javascript:jsOnClick('#idAnn');return false;" class="hrefclass">NCIDC Annual Product Distribution</a></li>
                    <li><a href="#" id="A1" onclick="javascript:jsOnClick('#idCancerType');return false;" class="hrefclass">NCIDC Cancer Type Report</a></li>
                    <li><a href="#" id="A2" onclick="javascript:jsOnClick('#idAnnOrder');return false;" class="hrefclass">NCIDC Annual Order Distribution Report</a></li>
                </ul>
            </div>


        </div>
    </div>
     <div class="rightcol rptcolumn">
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="gp_source"
            ForeColor="Red" />
        <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="gp_type"
            ForeColor="Red" />
        <asp:ValidationSummary ID="ValidationSummary3" runat="server" ValidationGroup="gp_Intl"
            ForeColor="Red" />
        <div id="idMonthDis" class="criteria">
            <h3>Monthly Distribution & Inventory Report Criteria</h3>
            <asp:Label ID="Label1" runat="server" AssociatedControlID="ddlMonth" Text="Month"  CssClass="hidden-label" />
            <asp:DropDownList ID="ddlMonth" runat="server" Enabled="false">
                <asp:ListItem Text="January" Value="1"></asp:ListItem>
                <asp:ListItem Text="February" Value="2"></asp:ListItem>
                <asp:ListItem Text="March" Value="3"></asp:ListItem>
                <asp:ListItem Text="April" Value="4"></asp:ListItem>
                <asp:ListItem Text="May" Value="5"></asp:ListItem>
                <asp:ListItem Text="June" Value="6"></asp:ListItem>
                <asp:ListItem Text="July" Value="7"></asp:ListItem>
                <asp:ListItem Text="August" Value="8"></asp:ListItem>
                <asp:ListItem Text="September" Value="9"></asp:ListItem>
                <asp:ListItem Text="October" Value="10"></asp:ListItem>
                <asp:ListItem Text="November" Value="11"></asp:ListItem>
                <asp:ListItem Text="December" Value="12"></asp:ListItem>
            </asp:DropDownList>
         <asp:Label ID="Label2" runat="server" AssociatedControlID="ddlYear" Text="Year" CssClass="hidden-label" />

            <asp:DropDownList ID="ddlYear" runat="server" Enabled="false">
                <asp:ListItem Text="2010" Value="2010"></asp:ListItem>
                <asp:ListItem Text="2011" Value="2011"></asp:ListItem>
                <asp:ListItem Text="2012" Value="2012"></asp:ListItem>
                <asp:ListItem Text="2013" Value="2013"></asp:ListItem>
                <asp:ListItem Text="2014" Value="2014"></asp:ListItem>
                <asp:ListItem Text="2015" Value="2015"></asp:ListItem>
            </asp:DropDownList>
            <asp:Button ID="btnGenMonthDis" runat="server" Text="Generate Report" OnClick="btnGenMonthDis_Click" />
        </div>
        <div id="idOrderBySource" class="criteria">
            <h3>Orders by Source Report Criteria</h3>
            <asp:Label ID="Label3" runat="server" Text="Start Date:" AssociatedControlID="txtStartDate_source" />
            <asp:TextBox ID="txtStartDate_source" runat="server" Width="100" MaxLength="10" CssClass="make_calendar"></asp:TextBox>
            <asp:Label ID="Label4" runat="server" Text="End Date:" AssociatedControlID="txtEndDate_source" />
            <asp:TextBox ID="txtEndDate_source" runat="server" Width="100" MaxLength="10" CssClass="make_calendar"></asp:TextBox>
            <asp:Button ID="btnGenOrderBySource" runat="server" Text="Generate Report" OnClick="btnGenOrderBySource_Click"
                ValidationGroup="gp_source" />
            <asp:CompareValidator ID="RangeValidator1" runat="server" ErrorMessage="Please enter valid date"
                Type="Date" Operator="DataTypeCheck" ControlToValidate="txtStartDate_source"
                Display="None" ValidationGroup="gp_source"></asp:CompareValidator>
            <asp:CompareValidator ID="RangeValidator2" runat="server" ErrorMessage="Please enter valid date"
                Type="Date" Operator="DataTypeCheck" ControlToValidate="txtEndDate_source" Display="None"
                ValidationGroup="gp_source"></asp:CompareValidator>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Start date is required"
                ControlToValidate="txtStartDate_source" Display="None" ValidationGroup="gp_source"></asp:RequiredFieldValidator>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="End date is required"
                ControlToValidate="txtEndDate_source" Display="None" ValidationGroup="gp_source"></asp:RequiredFieldValidator>
            <asp:CustomValidator ID="valDateSource" runat="server" ErrorMessage="" Display="none"
                OnServerValidate="validateCustom_source" ValidationGroup="gp_source"></asp:CustomValidator>
        </div>
        <div id="idOrderByType" class="criteria">
            <h3>Orders by Type of Customer Report Criteria</h3>
            <asp:Label ID="Label5" runat="server" Text="Start Date:" AssociatedControlID="txtStartDate_type" />
            <asp:TextBox ID="txtStartDate_type" runat="server" Width="100" MaxLength="10" CssClass="make_calendar"></asp:TextBox>
            <asp:Label ID="Label6" runat="server" Text="End Date:" AssociatedControlID="txtEndDate_type" />
            <asp:TextBox ID="txtEndDate_type" runat="server" Width="100" MaxLength="10" CssClass="make_calendar"></asp:TextBox>
            <asp:Button ID="btnGenOrderByType" runat="server" Text="Generate Report" OnClick="btnGenOrderByType_Click"
                ValidationGroup="gp_type" />
            <asp:CompareValidator ID="RangeValidator3" runat="server" ErrorMessage="Please enter valid date"
                Type="Date" Operator="DataTypeCheck" ControlToValidate="txtStartDate_type" Display="None"
                ValidationGroup="gp_type"></asp:CompareValidator>
            <asp:CompareValidator ID="RangeValidator4" runat="server" ErrorMessage="Please enter valid date"
                Type="Date" Operator="DataTypeCheck" ControlToValidate="txtEndDate_type" Display="None"
                ValidationGroup="gp_type"></asp:CompareValidator>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Start date is required"
                ControlToValidate="txtStartDate_type" Display="None" ValidationGroup="gp_type"></asp:RequiredFieldValidator>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="End date is required"
                ControlToValidate="txtEndDate_type" Display="None" ValidationGroup="gp_type"></asp:RequiredFieldValidator>
            <asp:CustomValidator ID="valDateType" runat="server" ErrorMessage="" Display="none"
                OnServerValidate="validateCustom_type" ValidationGroup="gp_type"></asp:CustomValidator>
        </div>
        <div id="idOrderByIntl" class="criteria">
            <h3>Orders by Source International Report Criteria</h3>
            <asp:Label ID="Label7" runat="server" Text="Start Date:" AssociatedControlID="txtStartDate_Intl" />
            <asp:TextBox ID="txtStartDate_Intl" runat="server" Width="100" MaxLength="10" CssClass="make_calendar"></asp:TextBox>
            <asp:Label ID="Label8" runat="server" Text="End Date:" AssociatedControlID="txtEndDate_Intl" />
            <asp:TextBox ID="txtEndDate_Intl" runat="server" Width="100" MaxLength="10" CssClass="make_calendar"></asp:TextBox>
            <asp:Button ID="btnGenOrderIntl" runat="server" Text="Generate Report" OnClick="btnGenOrderIntl_Click"
                ValidationGroup="gp_Intl" />
            <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Please enter valid date"
                Type="Date" Operator="DataTypeCheck" ControlToValidate="txtStartDate_Intl" Display="None"
                ValidationGroup="gp_Intl"></asp:CompareValidator>
            <asp:CompareValidator ID="CompareValidator2" runat="server" ErrorMessage="Please enter valid date"
                Type="Date" Operator="DataTypeCheck" ControlToValidate="txtEndDate_Intl" Display="None"
                ValidationGroup="gp_Intl"></asp:CompareValidator>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Start date is required"
                ControlToValidate="txtStartDate_Intl" Display="None" ValidationGroup="gp_Intl"></asp:RequiredFieldValidator>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="End date is required"
                ControlToValidate="txtEndDate_Intl" Display="None" ValidationGroup="gp_Intl"></asp:RequiredFieldValidator>
            <asp:CustomValidator ID="valDateIntl" runat="server" ErrorMessage="" Display="none"
                OnServerValidate="validateCustom_Intl" ValidationGroup="gp_Intl"></asp:CustomValidator>
        </div>
        <div id="idAnn" class="criteria">
            <h3>NCIDC Annual Product Distribution Report Criteria</h3>
            <asp:Label ID="Label9" runat="server" Text="Start Date:" AssociatedControlID="txtStartDate_Ann" />
            <asp:TextBox ID="txtStartDate_Ann" runat="server" Width="100" MaxLength="10" CssClass="make_calendar"></asp:TextBox>
            <asp:Label ID="Label10" runat="server" Text="End Date:" AssociatedControlID="txtEndDate_Ann" />
            <asp:TextBox ID="txtEndDate_Ann" runat="server" Width="100" MaxLength="10" CssClass="make_calendar"></asp:TextBox>
            <asp:Button ID="Button1" runat="server" Text="Generate Report" OnClick="btnGenOrderAnn_Click"
                ValidationGroup="gp_Ann" />
            <asp:CompareValidator ID="CompareValidator3" runat="server" ErrorMessage="Please enter valid date"
                Type="Date" Operator="DataTypeCheck" ControlToValidate="txtStartDate_Ann" Display="None"
                ValidationGroup="gp_Ann"></asp:CompareValidator>
            <asp:CompareValidator ID="CompareValidator4" runat="server" ErrorMessage="Please enter valid date"
                Type="Date" Operator="DataTypeCheck" ControlToValidate="txtEndDate_Ann" Display="None"
                ValidationGroup="gp_Ann"></asp:CompareValidator>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Start date is required"
                ControlToValidate="txtStartDate_Ann" Display="None" ValidationGroup="gp_Ann"></asp:RequiredFieldValidator>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="End date is required"
                ControlToValidate="txtEndDate_Ann" Display="None" ValidationGroup="gp_Ann"></asp:RequiredFieldValidator>
            <asp:CustomValidator ID="valDateAnn" runat="server" ErrorMessage="" Display="none"
                OnServerValidate="validateCustom_Ann" ValidationGroup="gp_Ann"></asp:CustomValidator>
        </div>
        <div id="idCancerType" class="criteria">
            <h3>NCIDC Annual Cancer Type Report Criteria</h3>
            <asp:Label ID="Label11" runat="server" Text="Start Date:" AssociatedControlID="txtStartDate_CancerType" />
            <asp:TextBox ID="txtStartDate_CancerType" runat="server" Width="100" MaxLength="10" CssClass="make_calendar"></asp:TextBox>
            <asp:Label ID="Label12" runat="server" Text="End Date:" AssociatedControlID="txtEndDate_CancerType" />
            <asp:TextBox ID="txtEndDate_CancerType" runat="server" Width="100" MaxLength="10" CssClass="make_calendar"></asp:TextBox>
            <asp:Button ID="Button2" runat="server" Text="Generate Report" OnClick="btnGenOrderCancerType_Click"
                ValidationGroup="gp_CancerType" />
            <asp:CompareValidator ID="CompareValidator5" runat="server" ErrorMessage="Please enter valid date"
                Type="Date" Operator="DataTypeCheck" ControlToValidate="txtStartDate_CancerType" Display="None"
                ValidationGroup="gp_CancerType"></asp:CompareValidator>
            <asp:CompareValidator ID="CompareValidator6" runat="server" ErrorMessage="Please enter valid date"
                Type="Date" Operator="DataTypeCheck" ControlToValidate="txtEndDate_CancerType" Display="None"
                ValidationGroup="gp_CancerType"></asp:CompareValidator>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="Start date is required"
                ControlToValidate="txtStartDate_CancerType" Display="None" ValidationGroup="gp_CancerType"></asp:RequiredFieldValidator>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="End date is required"
                ControlToValidate="txtEndDate_CancerType" Display="None" ValidationGroup="gp_CancerType"></asp:RequiredFieldValidator>
            <asp:CustomValidator ID="valDateCancerType" runat="server" ErrorMessage="" Display="none"
                OnServerValidate="validateCustom_CancerType" ValidationGroup="gp_CancerType"></asp:CustomValidator>
        </div>
        <div id="idAnnOrder" class="criteria">
            <h3>NCIDC Annual Order Distribution Report Criteria</h3>
            <asp:Label ID="Label13" runat="server" Text="Start Date:" AssociatedControlID="txtStartDate_AnnOrder" />
            <asp:TextBox ID="txtStartDate_AnnOrder" runat="server" Width="100" MaxLength="10" CssClass="make_calendar"></asp:TextBox>
            <asp:Label ID="Label14" runat="server" Text="End Date:" AssociatedControlID="txtEndDate_AnnOrder" />
            <asp:TextBox ID="txtEndDate_AnnOrder" runat="server" Width="100" MaxLength="10" CssClass="make_calendar"></asp:TextBox>
            <asp:Button ID="Button3" runat="server" Text="Generate Report" OnClick="btnGenOrderAnnOrder_Click"
                ValidationGroup="gp_AnnOrder" />
            <asp:CompareValidator ID="CompareValidator7" runat="server" ErrorMessage="Please enter valid date"
                Type="Date" Operator="DataTypeCheck" ControlToValidate="txtStartDate_AnnOrder" Display="None"
                ValidationGroup="gp_AnnOrder"></asp:CompareValidator>
            <asp:CompareValidator ID="CompareValidator8" runat="server" ErrorMessage="Please enter valid date"
                Type="Date" Operator="DataTypeCheck" ControlToValidate="txtEndDate_AnnOrder" Display="None"
                ValidationGroup="gp_AnnOrder"></asp:CompareValidator>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="Start date is required"
                ControlToValidate="txtStartDate_AnnOrder" Display="None" ValidationGroup="gp_AnnOrder"></asp:RequiredFieldValidator>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="End date is required"
                ControlToValidate="txtEndDate_AnnOrder" Display="None" ValidationGroup="gp_AnnOrder"></asp:RequiredFieldValidator>
            <asp:CustomValidator ID="valDateAnnOrder" runat="server" ErrorMessage="" Display="none"
                OnServerValidate="validateCustom_AnnOrder" ValidationGroup="gp_AnnOrder"></asp:CustomValidator>
        </div>
        <div id="idViewer">
            <rsweb:ReportViewer ID="rptReport" runat="server" Font-Names="Verdana" Font-Size="8pt"
                Title="NCI Publications Enterprise Report" Width="98%" ProcessingMode="Remote"
                Visible="true" ShowParameterPrompts="false" ShowPrintButton="false" Height="500px" >
            </rsweb:ReportViewer>
        </div>
    </div>
    <input type="hidden" id="hidRpt" runat="server" />
</asp:Content>
