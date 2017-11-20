<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CatSeq.ascx.cs" Inherits="PubEntAdmin.UserControl.CatSeq" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" /><asp:Button
    ID="btnSave" runat="server" Text="Save" OnClick="btnSave_OnClick" />
<asp:Label ID="lblCatSec" runat="server" AssociatedControlID="ddlCat" Text="Catalog Section: " CssClass=""></asp:Label>
<asp:DropDownList ID="ddlCat" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCatSelectedIndexChanged">
    <asp:ListItem Text="Category" Value="Category"></asp:ListItem>
    <asp:ListItem Text="Subcategory" Value="Subcategory"></asp:ListItem>
    <asp:ListItem Text="Subsubcategory" Value="Subsubcategory"></asp:ListItem>
</asp:DropDownList>
<asp:Button ID="btnUpdateCatSelection" runat="server" Text="Get Catalog selection"
    OnClick="btnUpdateCatSelection_Click" />
<asp:DropDownList ID="ddlValue" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlValueSelectedIndexChanged">
</asp:DropDownList>
<asp:Button ID="btnUpdateValueSection" runat="server" Text="Get value for update"
    OnClick="btnUpdateValueSection_Click" />
<asp:DataGrid ID="gvResult" runat="server" AutoGenerateColumns="False" CssClass="gray-border valuestable"
    Width="100%" AllowSorting="True"  AllowPaging="false"
    PageSize="4" OnItemDataBound="gvResult_ItemDataBound" ShowFooter="false" OnItemCommand="gvResult_ItemCommand"
    OnSortCommand="gvResult_SortCommand">    
    <HeaderStyle CssClass="rowHead" />
    <AlternatingItemStyle CssClass="rowOdd" />
    <ItemStyle CssClass="rowEven" />
    <Columns>
        <asp:BoundColumn Visible="False" DataField="Seqid"></asp:BoundColumn>
        <asp:BoundColumn Visible="True" DataField="SeqName" HeaderText="Section Name" SortExpression="DESCRIPTION"
            ItemStyle-Width="80%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"></asp:BoundColumn>
        <asp:TemplateColumn HeaderText="Sequence" SortExpression="SORT_SEQ" ItemStyle-Width="15%"
            ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <label for="<%#((DataGridItem)(Container)).FindControl("txtSeq").ClientID%>" style="display: none">Sequence</label><asp:TextBox ID="txtSeq" runat="server" Columns="5" MaxLength="8"></asp:TextBox>
                <cc1:FilteredTextBoxExtender ID="ftbeSeq" runat="server" TargetControlID="txtSeq"
                    ValidChars="0123456789">
                </cc1:FilteredTextBoxExtender>
            </ItemTemplate>
        </asp:TemplateColumn>
    </Columns>
</asp:DataGrid>
