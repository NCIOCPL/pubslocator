<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Attachments.ascx.cs"
    Inherits="PubEntAdmin.UserControl.Attachments" %>
<asp:DataGrid ID="grdAttachments" AutoGenerateColumns="false"
    runat="Server" Width="100%" OnItemCommand="grdAttachments_ItemCommand"
    OnItemDataBound="grdAttachments_ItemDataBound" UseAccessibleHeader="true"
    CssClass="gray-border valuestable">
    <HeaderStyle CssClass="rowHead" />
    <AlternatingItemStyle CssClass="rowOdd" />
    <ItemStyle CssClass="rowEven" />
    <Columns>
        <asp:TemplateColumn>
            <HeaderStyle HorizontalAlign="Left" CssClass="gridHeader"></HeaderStyle>
            <ItemStyle CssClass="gridFirstItem"></ItemStyle>
            <ItemTemplate>
                <img id="FileOpenImage" alt="Open File" src="~/Image/fileicon.gif" runat="server">
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:BoundColumn DataField="FileName" SortExpression="FileName" HeaderText="Filename">
            <HeaderStyle CssClass="gridHeader"></HeaderStyle>
            <ItemStyle CssClass="gridItem"></ItemStyle>
        </asp:BoundColumn>
        <asp:TemplateColumn HeaderText="Size">
            <HeaderStyle HorizontalAlign="Left" CssClass="gridHeader"></HeaderStyle>
            <ItemStyle CssClass="gridItem"></ItemStyle>
            <ItemTemplate>
                <asp:Label ID="lblFileSize" runat="server" Text=""></asp:Label>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:BoundColumn DataField="CreatorUserName" SortExpression="CreatorUserName" HeaderText="Created By">
            <HeaderStyle CssClass="gridHeader"></HeaderStyle>
            <ItemStyle CssClass="gridItem"></ItemStyle>
        </asp:BoundColumn>
        <asp:BoundColumn DataField="DateCreated" SortExpression="DateCreated" HeaderText="Date Uploaded"
            DataFormatString="{0:g}">
            <HeaderStyle CssClass="gridHeader"></HeaderStyle>
            <ItemStyle CssClass="gridItem"></ItemStyle>
        </asp:BoundColumn>
        <asp:TemplateColumn>
            <HeaderStyle HorizontalAlign="Left" CssClass="gridHeader"></HeaderStyle>
            <ItemStyle CssClass="gridLastItem"></ItemStyle>
            <ItemTemplate>
                <asp:Button ID="btnDelete" runat="Server" Text="Delete" CommandName="Delete" CssClass="standardText"></asp:Button>
            </ItemTemplate>
        </asp:TemplateColumn>
    </Columns>
</asp:DataGrid><div class="editboxrow">
    <div class="editbox">
        <p id="pAttach" runat="server">
            <label for="txtfilAttach" class="">Upload:</label>
            <input id="txtfilAttach" runat="server" type="file" name="txtfilAttach" class="">
            <asp:Label ID="lbl2big" Width="136px" runat="server" ForeColor="Red" Visible="False">Filesize > 4Mb</asp:Label>
        </p>
    </div>
</div>
