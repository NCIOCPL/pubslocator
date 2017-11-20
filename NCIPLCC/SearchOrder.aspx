<%@ Page Title="" Language="C#" MasterPageFile="~/pubmaster.Master" AutoEventWireup="true"
    CodeBehind="SearchOrder.aspx.cs" Inherits="PubEnt.SearchOrder" %>

<%@ MasterType VirtualPath="~/pubmaster.master" %>
<%@ Register Src="usercontrols/searchbar_search.ascx" TagName="searchbar" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link rel="stylesheet" href="css/calendar.css" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <uc2:searchbar ID="searchbar" runat="server" />
    </div>
    <div id="searchorder" class="indentwrap">
        <a id="skiptocontent" tabindex="-1"></a>
        <h2>
            <asp:Label ID="Label1" runat="server" Text="Order Lookup"></asp:Label>
        </h2>
        <!--NCIPL_CC-->
        <asp:Panel ID="pnlSource" runat="server">
            <asp:Label ID="lblErrorMsg" runat="server" Text="" CssClass="err"></asp:Label>
            <div class="clearFix" >
                <div class="datepicker">
                    <asp:Label ID="Label2" runat="server" Text="Start Date" AssociatedControlID="txtStartDate" ></asp:Label>
                    <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
                    <cc1:CalendarExtender ID="CalExtStartDate" runat="server" TargetControlID="txtStartDate"
                        CssClass="MyCalendar" Format="MM/dd/yyyy">
                    </cc1:CalendarExtender>
                    <cc1:MaskedEditExtender ID="MaskedEditExtStartDate" runat="server" TargetControlID="txtStartDate"
                        Mask="99/99/9999" MaskType="Date">
                    </cc1:MaskedEditExtender>
                    <cc1:MaskedEditValidator ID="MaskedEditValStartDate" runat="server" ControlToValidate="txtStartDate"
                        ControlExtender="MaskedEditExtStartDate" Display="Dynamic" IsValidEmpty="true"
                        EmptyValueMessage="A Date is Required." InvalidValueMessage="The date is invalid."></cc1:MaskedEditValidator>
                </div>
                <div class="datepicker">
                    <asp:Label ID="Label3" runat="server" Text="End Date" AssociatedControlID="txtEndDate"></asp:Label>
                    <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                    <cc1:CalendarExtender ID="CalExtEndDate" runat="server" TargetControlID="txtEndDate"
                        CssClass="MyCalendar" Format="MM/dd/yyyy">
                    </cc1:CalendarExtender>
                    <cc1:MaskedEditExtender ID="MaskedEditExtEndDate" runat="server" TargetControlID="txtEndDate"
                        Mask="99/99/9999" MaskType="Date">
                    </cc1:MaskedEditExtender>
                    <cc1:MaskedEditValidator ID="MaskedEditValEndDate" runat="server" ControlToValidate="txtEndDate"
                        ControlExtender="MaskedEditExtEndDate" Display="Dynamic" IsValidEmpty="true"
                        EmptyValueMessage="A Date is Required." InvalidValueMessage="The date is invalid."></cc1:MaskedEditValidator>
                </div>
            </div>
            <div class="clearFix" >
                <div class="custtype">
                    <asp:Label ID="lblCustomerType" runat="server" Text="Type of Customer" AssociatedControlID="drpCustomerType" CssClass="shiplabel"></asp:Label>
                   
                    <asp:DropDownList ID="drpCustomerType" runat="server">
                        <asp:ListItem Value="" Text="[Please Select]"></asp:ListItem>
                        <asp:ListItem Value="All" Text="All"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="keywords">
                 <asp:Label ID="lblKeywords" runat="server" Text="Search by invoice #, e-mail, shipping or billing info"
                        AssociatedControlID="txtKeywords" CssClass="shiplabel"></asp:Label>
                    <asp:Panel ID="pnlKeywords" runat="server" DefaultButton="btnSearchOrder">
                        <asp:TextBox ID="txtKeywords" runat="server" MaxLength="100" OnTextChanged="txtKeywords_TextChanged"></asp:TextBox>
                    </asp:Panel>
                </div>
                <div class="searchorderbtn">
                    <asp:Button ID="btnSearchOrder" runat="server" OnClick="clickSearchOrder" Text="Search Order"
                        CssClass="btn" /></div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
