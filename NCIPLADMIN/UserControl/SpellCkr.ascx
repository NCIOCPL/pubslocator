<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpellCkr.ascx.cs" Inherits="PubEntAdmin.UserControl.SpellCkr" %>

    <label for="<%= drpLanguage.ClientID%>">Spell Check Language:</label>&nbsp;<asp:DropDownList ID="drpLanguage" runat="server">
    <asp:ListItem Value="28001">English</asp:ListItem>
    <asp:ListItem Value="29552">Spanish</asp:ListItem>
</asp:DropDownList>
<img id="imgSpelChksrc" src="~/Image/btnspells.gif" runat="server" alt="Spell check"
    align="top" />


    

