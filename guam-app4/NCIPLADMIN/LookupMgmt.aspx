<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LookupMgmt.aspx.cs" Inherits="PubEntAdmin.LookupMgmt" MasterPageFile="~/Template/Default2.Master" ValidateRequest="false" %>

<%@ Register Src="UserControl/SpellCkr.ascx" TagName="SpellCkr" TagPrefix="uc1" %>
<asp:Content ID="contentMain" ContentPlaceHolderID="BodyContent" runat="server">
    <table class="lookupmgmt">
        <tr id="trSpellCkr2" runat="server">
            <td class="spellcheck">
                <uc1:SpellCkr ID="SpellCkr2" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="lookupmgmtcontent">
                <asp:PlaceHolder ID="plcHldContent" runat="server"></asp:PlaceHolder>
            </td>
        </tr>
        <tr id="trSpellCkr1" runat="server">
            <td class="spellcheck spellbtm">
                <uc1:SpellCkr ID="SpellCkr1" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>


