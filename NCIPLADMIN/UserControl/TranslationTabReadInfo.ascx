<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TranslationTabReadInfo.ascx.cs" Inherits="PubEntAdmin.UserControl.TranslationTabReadInfo" %>
<asp:GridView ID="gvResult" runat="server" AutoGenerateColumns="False" CssClass="gray-border valuestable"
    Width="100%" AllowSorting="True" 
    AllowPaging="false" UseAccessibleHeader="true">    
    <HeaderStyle CssClass="rowHead" />
    <AlternatingRowStyle CssClass="rowOdd" />
    <RowStyle CssClass="rowEven" />
    <Columns>
        <asp:BoundField Visible="False" DataField="PubID"></asp:BoundField>
        <asp:BoundField HeaderText="Publication ID" ItemStyle-Width="25%" DataField="ProdID" ></asp:BoundField>
        <asp:BoundField HeaderText="Publication Title" ItemStyle-Width="50%" DataField="PubName"></asp:BoundField>
        <asp:BoundField HeaderText="Language" ItemStyle-Width="25%" DataField="PubLang" ></asp:BoundField>
    </Columns>
</asp:GridView>
