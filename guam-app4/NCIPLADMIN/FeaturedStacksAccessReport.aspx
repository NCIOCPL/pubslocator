<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Default2.Master" AutoEventWireup="true"
    CodeBehind="FeaturedStacksAccessReport.aspx.cs" Inherits="PubEntAdmin.FeaturedStacksAccessReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Featured Stacks Access Report</title>    
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="topContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" runat="server">
   <h3><asp:Label ID="lblStackAccessReport" runat="server" Text="Featured Stacks Access Report" /></h3> 
    <asp:Label ID="Label1" runat="server" Text="Select Start Date" AssociatedControlID="txtStartDate" />
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
    <asp:Label ID="Label2" runat="server" Text="Select End Date" AssociatedControlID="txtEndDate" />
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
    <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click" />
    <asp:Label ID="lblMessage" runat="server" CssClass="LabelDefault" Visible="False"
        EnableViewState="False" ForeColor="Red" />
    <asp:Label ID="lblExporttoExcel" runat="server" Text="Export to Excel" Visible="False"
        EnableViewState="False"></asp:Label>
    <asp:ImageButton ID="ImgBtnExporttoExcel" runat="server" ImageUrl="~/Image/excelicon.gif"
        AlternateText="Export to Excel" Visible="false" EnableViewState="False" OnClick="ImgBtnExporttoExcel_Click" />
    <asp:Label ID="lblTextandDates" runat="server" CssClass="" />
    <asp:GridView ID="gvStackAccess" runat="server" AutoGenerateColumns="False" CellPadding="4"
        Width="50%">
        <HeaderStyle CssClass="rowHead" />
        <RowStyle CssClass="rowEven" />
        <AlternatingRowStyle CssClass="rowOdd" />
        <Columns>
            <asp:BoundField DataField="stacktitle" HeaderText="Stack Title" HeaderStyle-Width="90%">
                <HeaderStyle Width="90%" />
            </asp:BoundField>
            <asp:BoundField DataField="total" HeaderText="Total">
                <HeaderStyle HorizontalAlign="Right" Width="10%" />
                <ItemStyle HorizontalAlign="Right" />
            </asp:BoundField>
        </Columns>
    </asp:GridView>
</asp:Content>
