<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ListMultiSelect.ascx.cs" Inherits="PubEntAdmin.UserControl.ListMultiSelect" %>
<%@ Register TagPrefix="cc" Namespace="PubEntAdmin.CustomControl" Assembly="PubEntAdmin" %>
<cc:LISTBOXBASE id="lstMultiSelect" runat="server"></cc:LISTBOXBASE>
<asp:requiredfieldvalidator id="ReqFieldVal" Text="(required)" Runat="Server" Visible="true" CssClass="fieldAlert"
	ControlToValidate="lstMultiSelect" Display="Dynamic">(Required)</asp:requiredfieldvalidator>
<asp:CustomValidator id="CustValidator" runat="server" ErrorMessage="(One of your selection is undefined)"
	OnServerValidate="CustVal_MultiSelect_ServerValidate" ControlToValidate="lstMultiSelect"
	Visible="true" CssClass="fieldAlert" Display="Dynamic"></asp:CustomValidator>
