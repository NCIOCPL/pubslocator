<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="steps.ascx.cs" Inherits="PubEnt.usercontrols.steps" %>
<asp:Table ID="Table1" runat="server">
    <asp:TableRow runat="server">
        <asp:TableCell CssClass="progress1" Width="25%" HorizontalAlign="Center" Text=""
            ID="cell1"><div class="clearFix"><div class="progressLine"></div><div class="progressLine progressLineR"></div></div><div class="progressContents"><img src="images/shopcart.gif" alt="" />Shopping Cart</div></asp:TableCell>
        <asp:TableCell CssClass="progress2" Width="19%" HorizontalAlign="Center" Text=""
            ID="cell3"><div class="clearFix"><div class="progressLine"></div><div class="progressLine progressLineR"></div></div><div class="progressContents"><img src="images/shopcart.gif" alt="" />Shipping</div></asp:TableCell>
        <asp:TableCell CssClass="progress3" Width="32%" HorizontalAlign="Center" Text=""
            ID="cell4"><div class="clearFix"><div class="progressLine"></div><div class="progressLine progressLineR"></div></div><div class="progressContents"><img src="images/shopcart.gif" alt="" />Verify & Place Order</div></asp:TableCell>
        <asp:TableCell CssClass="progress4" Width="24%" HorizontalAlign="Center" Text=""
            ID="cell5"><div class="clearFix"><div class="progressLine"></div><div class="progressLine progressLineR"></div></div><div class="progressContents"><img class="progressConf" src="images/shopconf.gif" alt="" />Confirmation</div></asp:TableCell>
    </asp:TableRow>
</asp:Table>
