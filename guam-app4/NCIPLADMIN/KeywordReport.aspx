<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="KeywordReport.aspx.cs" Inherits="PubEntAdmin.KeywordReport" MasterPageFile="~/Template/Default2.Master" ValidateRequest="false" %>

<asp:Content ID="contentHead" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="contentMain" ContentPlaceHolderID="BodyContent" runat="server">
    <h2>Keywords Report</h2>
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
    <asp:Label ID="Label1" Width="250px" runat="server" Text="Keywords Report" CssClass="pagetitle-border"></asp:Label>
    <asp:Label ID="Label5" runat="server" Text="Enter Publication ID" AssociatedControlID="TxtPubid" CssClass="labelDefaultLeft"></asp:Label>
            <asp:TextBox ID="TxtPubid" runat="server" Width="245px"></asp:TextBox>
    <asp:Button ID="btQuery" runat="server" Text="Go"
        OnClick="btQuery_Click" Height="19px" Width="41px" />
    <asp:Label ID="Message" runat="server" CssClass="errorText"
        Text="   Please enter Publication IDs"
        Width="211px" Visible="False"></asp:Label>
    <asp:Label ID="Label7" runat="server" CssClass="textDefault" Width="340px"
        Text="To search for multiple Publication ID separate them by using comma."></asp:Label>
    <asp:Label ID="Labelor" runat="server" CssClass="textDefault">or</asp:Label>
    <asp:Label ID="Label6" runat="server" CssClass="labelDefault"
        Text="Click the “All” button to see all the keywords for all publications"
        Width="361px" Style="text-align: left" Height="16px"></asp:Label><asp:Button ID="btAll" runat="server" OnClick="btAll_Click" Text="All" Height="19px"
            Width="42px" />
    <asp:Label ID="Label8" runat="server" CssClass="labelDefault"
        Text="Export to Excel"
        Visible="False"></asp:Label><asp:ImageButton ID="ButtonExcel_Click" runat="server"
            OnClick="ButtonExcel_Click_Click" ImageUrl="~/Image/excelicon.gif"
            Visible="False" />
    <asp:GridView ID="KwGridView" runat="server" AllowSorting="False"
        CssClass="gray-border, Grid" Width="508px">
        <RowStyle CssClass="rowEven" />
        <HeaderStyle CssClass="rowHead" />
        <AlternatingRowStyle CssClass="rowOdd" />
    </asp:GridView>
</asp:Content>
