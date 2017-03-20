<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExhTabReadInfo.ascx.cs" 
    Inherits="PubEntAdmin.UserControl.ExhTabReadInfo" %>
<asp:DataGrid ID="dgInfoView" runat="server" 
    AutoGenerateColumns="False" OnItemDataBound="dgInfoView_ItemDataBound"
     UseAccessibleHeader="true" CssClass="gray-border valuestable" ShowHeader="false">
    <AlternatingItemStyle CssClass="rowOdd" />
    <ItemStyle CssClass="rowEven" />
    <Columns>
        <asp:TemplateColumn ItemStyle-Width="40%" ItemStyle-CssClass="tabrlabel">
            <ItemTemplate>
                <%# DataBinder.Eval(Container.DataItem, "0") %>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn ItemStyle-Width="60%" >            
            <ItemTemplate>
                <%# DataBinder.Eval(Container.DataItem, "1") %>
            </ItemTemplate>
        </asp:TemplateColumn>
    </Columns>
</asp:DataGrid>
