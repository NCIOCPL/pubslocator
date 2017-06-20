<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PubHistTabReadInfo.ascx.cs"
    Inherits="PubEntAdmin.UserControl.PubHistTabReadInfo" %>
<asp:GridView ID="gvResult" runat="server" AutoGenerateColumns="False"
    Width="100%" AllowSorting="True" HorizontalAlign="Center" AllowPaging="false"
    OnRowDataBound="gvResult_RowDataBound" UseAccessibleHeader="true" CssClass="gray-border valuestable">
    <HeaderStyle CssClass="rowHead" />
    <AlternatingRowStyle CssClass="rowOdd" />
    <RowStyle CssClass="rowEven" />
    <Columns>
        <asp:BoundField Visible="False" DataField="pubid"></asp:BoundField>
        <asp:BoundField Visible="True" DataField="ReceivedDate" HeaderText="Date Received"
            ItemStyle-Width="20%"></asp:BoundField>
        <asp:BoundField Visible="True" DataField="QtyReceived" HeaderText="Qty Received"
            ItemStyle-Width="20%"></asp:BoundField>
        <asp:TemplateField HeaderText="NIH #" ItemStyle-Width="20%">
            <ItemTemplate>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Date Printed" ItemStyle-Width="20%">
            <ItemTemplate>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Date Revised" ItemStyle-Width="20%">
            <ItemTemplate>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>

