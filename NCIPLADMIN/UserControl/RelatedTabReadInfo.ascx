<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RelatedTabReadInfo.ascx.cs" Inherits="PubEntAdmin.UserControl.RelatedTabReadInfo" %>
<asp:GridView ID="gvResult" runat="server" AutoGenerateColumns="False" CssClass="gray-border valuestable"
    Width="100%" AllowSorting="True"  AllowPaging="false">
     <HeaderStyle CssClass="rowHead" />
    <AlternatingRowStyle CssClass="rowOdd" />
    <RowStyle CssClass="rowEven" />
    <Columns>
        <asp:BoundField Visible="False" DataField="PubID"></asp:BoundField>
        <asp:BoundField HeaderText="Publication ID" ItemStyle-Width="40%" DataField="ProdID" ></asp:BoundField>
        <asp:BoundField HeaderText="Publication Title" ItemStyle-Width="60%" DataField="PubName"></asp:BoundField>
    </Columns>
</asp:GridView>
