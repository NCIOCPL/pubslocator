<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddSubcat.ascx.cs"
    Inherits="PubEntAdmin.UserControl.AddSubcat" %>
<asp:Label ID="lblSubcatDecrp" runat="server" Text="Subcategory Description:" AssociatedControlID="txtSubcatDes" CssClass=""></asp:Label>
<asp:TextBox ID="txtSubcatDes" runat="server" MaxLength="100"></asp:TextBox>
<asp:CheckBox ID="ckboxAllowSubsubcat" runat="server" Text="Allow Subsubcategories" CssClass="" />
<asp:Button ID="btnAddSubcat" runat="server" Text="Save"
    OnClick="btnAddSubcat_Click" />
<asp:Button ID="btnCancelSubcat" runat="server" Text="Cancel"
    OnClick="btnCancelSubcat_Click" />

