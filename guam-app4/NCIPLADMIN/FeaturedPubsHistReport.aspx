<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Default2.Master" AutoEventWireup="true" CodeBehind="FeaturedPubsHistReport.aspx.cs" Inherits="PubEntAdmin.FeaturedPubsHistReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Featured Stack History Report</title>    
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
    <asp:Label ID="lblStackHistReport" runat="server" Text="Featured Stacks History Report" CssClass="pagetitle-border"></asp:Label>
    <asp:Label ID="Label1" runat="server" Text="Select Start Date" AssociatedControlID="txtStartDate"></asp:Label>
    <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
    <cc1:CalendarExtender ID="CalExtStartDate" runat="server" TargetControlID="txtStartDate" CssClass="MyCalendar" Format="MM/dd/yyyy"></cc1:CalendarExtender>
    <cc1:MaskedEditExtender ID="MaskedEditExtStartDate" runat="server" TargetControlID="txtStartDate" Mask="99/99/9999" MaskType="Date"></cc1:MaskedEditExtender>
    <cc1:MaskedEditValidator ID="MaskedEditValStartDate" runat="server" ControlToValidate="txtStartDate" ControlExtender="MaskedEditExtStartDate" Display="Dynamic" IsValidEmpty="true" EmptyValueMessage="A Date is Required." InvalidValueMessage="The date is invalid."></cc1:MaskedEditValidator>
    <asp:Label ID="Label2" runat="server" Text="Select End Date" AssociatedControlID="txtEndDate"></asp:Label>
    <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
    <cc1:CalendarExtender ID="CalExtEndDate" runat="server" TargetControlID="txtEndDate" CssClass="MyCalendar" Format="MM/dd/yyyy"></cc1:CalendarExtender>
    <cc1:MaskedEditExtender ID="MaskedEditExtEndDate" runat="server" TargetControlID="txtEndDate" Mask="99/99/9999" MaskType="Date"></cc1:MaskedEditExtender>
    <cc1:MaskedEditValidator ID="MaskedEditValEndDate" runat="server" ControlToValidate="txtEndDate" ControlExtender="MaskedEditExtEndDate" Display="Dynamic" IsValidEmpty="true" EmptyValueMessage="A Date is Required." InvalidValueMessage="The date is invalid."></cc1:MaskedEditValidator>
    <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click" />
    <asp:Label ID="lblMessage" runat="server" CssClass="LabelDefault" Visible="False"
        EnableViewState="False" ForeColor="Red"></asp:Label>
    <asp:Label ID="lblExporttoExcel" runat="server" Text="Export to Excel"
        Visible="False" EnableViewState="False"></asp:Label>
    <asp:ImageButton ID="ImgBtnExporttoExcel" runat="server"
        ImageUrl="~/Image/excelicon.gif" OnClick="ImgBtnExporttoExcel_Click"
        AlternateText="Export to Excel" Visible="false" EnableViewState="False" />
    <asp:Label ID="lblTextandDates" runat="server" CssClass=""></asp:Label>
    <asp:GridView ID="gvStackHistory" runat="server" AutoGenerateColumns="False"
        CellPadding="4" OnRowDataBound="gvStackHistory_RowDataBound" Width="90%">
        <HeaderStyle CssClass="rowHead" />
        <RowStyle CssClass="rowEven" />
        <AlternatingRowStyle CssClass="rowOdd" />
        <Columns>
            <asp:TemplateField HeaderText=" " ItemStyle-VerticalAlign="Top">
                <ItemStyle Width="70%" />
                <ItemTemplate>
                    <asp:Literal ID="litPubTitles" runat="server"></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Active Start Date" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top">
                <ItemStyle />
                <ItemTemplate>
                    <asp:Literal ID="litActiveSDates" runat="server"></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Active End Date" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top">
                <ItemStyle />
                <ItemTemplate>
                    <asp:Literal ID="litActiveEDates" runat="server"></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
