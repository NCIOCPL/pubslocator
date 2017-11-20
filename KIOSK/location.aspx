<%@ Page Title="" Language="C#" MasterPageFile="~/pubmaster.Master" AutoEventWireup="true" CodeBehind="location.aspx.cs" Inherits="Kiosk.location" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="kioskgap">
</div>
<div class="dialogbox">
<table border="1"  >
  <tr>
    <td><asp:Label CssClass="dialoghead2"
            ID="Label1" runat="server" 
            Text="Please make a selection to start your visit: <br />&nbsp"></asp:Label>
                                                            </td>
  </tr>
  <tr>
    <td></td>
  </tr>
  <tr>
    <td width="100%">
        <asp:ImageButton ID="btnUS" ImageUrl="images/inus_off.jpg" runat="server" 
            Text="I live in the United States" onclick="btnUS_Click" />
        <p><asp:Label CssClass="dialoghead3"
                ID="Label2" runat="server" 
                Text="includes Puerto Rico, U.S. Virgin Islands, Guam, <br />American Samoa, and APO/FPO addresses"></asp:Label>
        </p>
      </td>
  </tr>
  <tr>
    <td></td>
  </tr>
  <tr>
    <td width="100%">
        <asp:ImageButton ID="btnInternational" ImageUrl="images/outsideus_off.jpg" runat="server"  
            Text="I live outside of the United States"  
            onclick="btnInternational_Click" />
      </td>
  </tr>
  <tr>
    <td width="100%">&nbsp;</td>
  </tr>
  <tr>
    <td width="100%">&nbsp;</td>
  </tr>
</table>
</div>

</asp:Content>
