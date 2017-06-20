<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TabStrip.ascx.cs" Inherits="PubEntAdmin.UserControl.TabStrip" %>
<asp:DataList ID="lstTabs" CellSpacing="0" CellPadding="0" RepeatDirection="Horizontal"
    EnableViewState="false" runat="Server" ItemStyle-Wrap="true" OnItemCommand="lstTabs_ItemCommand">
    <ItemTemplate>
        <table border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td bgcolor="#ffffff" valign="top"><img src="Image/dot_t.gif" alt="" width="1" height="1" border="0" /></td>
				<td bgcolor="#999999" align="right"><img src="Image/dot_t.gif" alt="" width="1" height="1" border="0"></td>
				<td bgcolor="#FFFFFF"><img src="Image/dot_t.gif" alt="" width="1" height="1" border="0"></td>
            </tr>
            <tr>
                <td bgcolor="#999999"><img src="Image/prd_crn_lft2off.gif" alt="" width="2" height="23" border="0"></td>
				<td nowrap="nowrap" valign="middle" width="100%">
					<asp:Button Runat=server width="100%" Text="<%# GetTabName(Container) %>" CssClass="<%# SetCssClass(Container) %>" ID="btnTab" name="btnTab" />
				</td>
				<td bgcolor="#999999"><img src="Image/prd_crn_rht2off.gif" alt="" width="2" height="23" border="0"></td>
            </tr>
        </table>
    </ItemTemplate>
</asp:DataList>