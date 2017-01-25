<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TextCtrl_SpellCk.ascx.cs"
    Inherits="PubEntAdmin.UserControl.TextCtrl_SpellCk" %>
<asp:Literal ID="JScriptSheet" runat="Server" />
<table id="SpellCheckTB" cellspacing="0" cellpadding="2" border="0" style="clip: rect(auto auto auto auto)" ms_2d_layout="TRUE" runat="server">
    <tr>
        <td>
            <asp:TextBox ID="txtComment" runat="server" Height="100%"></asp:TextBox>
            <asp:RequiredFieldValidator ID="ReqFieldVal" Text="(required)" runat="Server" Visible="true"
                CssClass="fieldAlert" ControlToValidate="txtComment" Display="Dynamic"></asp:RequiredFieldValidator>
        </td>
    </tr>
</table>
