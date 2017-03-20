<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FeaturedPubsSeq.ascx.cs" Inherits="PubEntAdmin.UserControl.FeaturedPubsSeq" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" /><asp:Button
    ID="btnSave" runat="server" Text="Save" OnClick="btnSave_OnClick" />
<asp:Label ID="lblFeaturedSec" runat="server" Text="Publication ID: " CssClass=""></asp:Label>
<asp:DataGrid ID="gvResult" runat="server" AutoGenerateColumns="False" CssClass="gray-border valuestable"
    Width="100%" AllowSorting="True"  AllowPaging="false"
    PageSize="4" OnItemDataBound="gvResult_ItemDataBound" ShowFooter="false"
    OnSortCommand="gvResult_SortCommand">   
    <HeaderStyle CssClass="rowHead" />
    <AlternatingItemStyle CssClass="rowOdd" />
    <ItemStyle CssClass="rowEven" />
    <Columns>
        <asp:BoundColumn Visible="False" DataField="Seqid"></asp:BoundColumn>
        <asp:BoundColumn Visible="True" DataField="SeqName" HeaderText="Section Name" SortExpression="PRODUCTID"
            ItemStyle-Width="80%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"></asp:BoundColumn>
        <asp:TemplateColumn HeaderText="Sequence" SortExpression="NCIPLFEATURED_SEQUENCE" ItemStyle-Width="15%"
            ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <asp:TextBox ID="txtSeq" runat="server" Columns="5" MaxLength="8"></asp:TextBox>
                <cc1:FilteredTextBoxExtender ID="ftbeSeq" runat="server" TargetControlID="txtSeq"
                    ValidChars="0123456789">
                </cc1:FilteredTextBoxExtender>
            </ItemTemplate>
        </asp:TemplateColumn>
    </Columns>
</asp:DataGrid>
