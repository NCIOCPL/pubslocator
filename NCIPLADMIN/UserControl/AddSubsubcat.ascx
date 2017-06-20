<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddSubsubcat.ascx.cs"
    Inherits="PubEntAdmin.UserControl.AddSubsubcat" %>

<asp:Label ID="lblSubsubcatDecrp" runat="server" Text="Subsubcategory Description:" AssociatedControlID="txtSubSubcatDes" CssClass=""></asp:Label>
<asp:TextBox ID="txtSubSubcatDes" runat="server" MaxLength="100"></asp:TextBox>
<asp:Label ID="lblAssign_to_Subcategory" runat="server" Text="Assign to Subcategory:" AssociatedControlID="ddlSubCategory" CssClass=""></asp:Label>
<asp:DropDownList ID="ddlSubCategory" runat="server">
</asp:DropDownList>
<asp:Button ID="btnAddSubsubcat" runat="server" Text="Save" OnClick="btnAddSubsubcat_Click" />
<asp:Button ID="btnCancelSubsubcat" runat="server" Text="Cancel" OnClick="btnCancelSubsubcat_Click" />

