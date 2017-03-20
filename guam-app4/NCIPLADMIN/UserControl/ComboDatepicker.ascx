<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ComboDatepicker.ascx.cs"
    Inherits="PubEntAdmin.UserControl.ComboDatepicker" %>
<table>
    <tr>
        <td>Month
        </td>
        <td>Day
        </td>
        <td>Year
        </td>
    </tr>
    <tr>
        <td>
            <asp:DropDownList ID="ddlM" runat="server">
            </asp:DropDownList>
        </td>
        <td>
            <asp:DropDownList ID="ddld" runat="server">
            </asp:DropDownList>
        </td>
        <td>
            <asp:DropDownList ID="ddly" runat="server">
            </asp:DropDownList>
        </td>
    </tr>
</table>

