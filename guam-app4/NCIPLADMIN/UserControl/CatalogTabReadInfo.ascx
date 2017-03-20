<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CatalogTabReadInfo.ascx.cs" Inherits="PubEntAdmin.UserControl.CatalogTabReadInfo" %>
<asp:DataGrid ID="dgInfoView" runat="server" Width="100%"  ShowHeader="False" 
    AutoGenerateColumns="False" OnItemDataBound="dgInfoView_ItemDataBound" UseAccessibleHeader="true" CssClass="gray-border valuestable">
    <AlternatingItemStyle CssClass="rowOdd" />
    <ItemStyle CssClass="rowEven" />
    <Columns>
        <asp:TemplateColumn ItemStyle-Width="40%" ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="tabrlabel">
            <ItemTemplate>
                <%# DataBinder.Eval(Container.DataItem, "0") %>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn ItemStyle-Width="60%" ItemStyle-HorizontalAlign="Left">
            <ItemStyle VerticalAlign="Top"></ItemStyle>
            <ItemTemplate>
                <%# DataBinder.Eval(Container.DataItem, "1") %>
            </ItemTemplate>
        </asp:TemplateColumn>
    </Columns>
</asp:DataGrid>
