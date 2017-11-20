<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="SearchKeywords.aspx.cs"
    Inherits="PubEntAdmin.SearchKeywords" MasterPageFile="~/Template/Default2.Master"
    ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="contentHead" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="contentMain" ContentPlaceHolderID="BodyContent" runat="server">
    <title>Search Keywords</title>
    <link href="Css/Common.css" type="text/css" rel="stylesheet">
    <style type="text/css">
        #city { text-align: center; }
    </style>
    <script type="text/javascript">
        function CallPrint() {
            var myForm = document.forms[0];
            myForm.oldTarget = myForm.target;
            myForm.target = '_blank';
            setTimeout(ResetTarget, 1000);
        }
        function ResetTarget() {
            var myForm = document.forms[0];
            myForm.target = myForm.oldTarget;
        }
    </script>
    <asp:PlaceHolder ID="plcHldMenu" runat="server"></asp:PlaceHolder>
    <h3>
        <asp:Label ID="lbKeywordReport" runat="server" Width="250px" Text="Search Keywords Report" CssClass="pagetitle-border"></asp:Label>
    </h3>
    <asp:Label ID="Label5" runat="server" AssociatedControlID="txtStartDate" CssClass="LabelDefault"
        Width="110px" Text="Select Start Date"></asp:Label>
    <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
    <cc1:CalendarExtender ID="CalExtStartDate" runat="server" TargetControlID="txtStartDate"
        CssClass="MyCalendar" Format="MM/dd/yyyy">
    </cc1:CalendarExtender>
    <cc1:MaskedEditExtender ID="MaskedEditExtStartDate" runat="server" TargetControlID="txtStartDate"
        Mask="99/99/9999" MaskType="Date">
    </cc1:MaskedEditExtender>
    <cc1:MaskedEditValidator ID="MaskedEditValStartDate" runat="server" ControlToValidate="txtStartDate"
        ControlExtender="MaskedEditExtStartDate" Display="Dynamic" TooltipMessage="Enter Start Date"
        IsValidEmpty="true" EmptyValueMessage="A Date is Required" InvalidValueMessage="Ths date is invalid"></cc1:MaskedEditValidator>
    <asp:Label ID="Label1" runat="server" AssociatedControlID="txtEndDate" CssClass="LabelDefault"
        Width="110px" Text="Select End Date"></asp:Label>
    <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
    <cc1:CalendarExtender ID="CalExtEndDate" runat="server" TargetControlID="txtEndDate"
        CssClass="MyCalendar" Format="MM/dd/yyyy">
    </cc1:CalendarExtender>
    <cc1:MaskedEditExtender ID="MaskedEditExtEndDate" runat="server" TargetControlID="txtEndDate"
        Mask="99/99/9999" MaskType="Date">
    </cc1:MaskedEditExtender>
    <cc1:MaskedEditValidator ID="MaskedEditValEndDate" runat="server" ControlToValidate="txtEndDate"
        ControlExtender="MaskedEditExtEndDate" Display="Dynamic" TooltipMessage="Enter End Date"
        IsValidEmpty="true" EmptyValueMessage="A Date is Required" InvalidValueMessage="Ths date is invalid"></cc1:MaskedEditValidator>
    <asp:Button ID="btQuery" runat="server" Text="Go" OnClick="btQuery_Click" Style="text-align: left" />
    <asp:Label ID="Message" runat="server" CssClass="LabelDefault" Text="" Width="296px"
        Visible="False"></asp:Label>
    <asp:Label ID="Label8" runat="server" CssClass="LabelDefault" Text="Export to Excel    "
        Visible="False"></asp:Label>
    <asp:ImageButton ID="ButtonExcel_Click" runat="server" OnClick="ButtonExcel_Click_Click"
        ImageUrl="~/Image/excelicon.gif" Visible="False" />
    <asp:Label ID="Label9" runat="server" Font-Names="Arial" Font-Size="Small" Text="Keywords were searched for "
        Font-Bold="True" Visible="False"></asp:Label>
    <asp:Label ID="lbtextDates" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small"></asp:Label>
    <asp:GridView ID="KwGridView" runat="server" AllowSorting="False" CssClass="gray-border, Grid"
        Width="608px">
        <RowStyle CssClass="rowEven" />
        <HeaderStyle CssClass="rowHead" />
        <AlternatingRowStyle CssClass="rowOdd" />
    </asp:GridView>
</asp:Content>
