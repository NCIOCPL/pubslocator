<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InvalidInput.aspx.cs" Inherits="PubEntAdmin.InvalidInput" MasterPageFile="~/Template/Default2.Master" %>

<asp:Content ID="contentMain" ContentPlaceHolderID="BodyContent" runat="server">
    <asp:Label ID="Label1" runat="server">Some combinations of the characters you entered are not accepted by this application. 
       Please try again or contact Web administrator. </asp:Label><a href='javascript:window.history.back(-1);'>Go 
			back to make correction.</a>
</asp:Content>

