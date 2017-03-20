<%@ Page Title="" Language="C#" MasterPageFile="~/pubmaster.Master" AutoEventWireup="true"
    CodeBehind="OrderDetail.aspx.cs" Inherits="PubEnt.OrderDetail" %>

<%@ MasterType VirtualPath="~/pubmaster.master" %>
<%@ Register Src="usercontrols/searchbar_search.ascx" TagName="searchbar" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <uc2:searchbar ID="searchbar" runat="server" />
    </div>
    <div class="indentwrap" id="orderdetail">
        <a id="skiptocontent" tabindex="-1"></a>
        <h2>
            <asp:Label ID="Label1" runat="server" Text="Order Details" CssClass=""></asp:Label></h2>
        <div class="orderdetails">
            <asp:Label runat="server" ID="lbltxtInvoice" class="orderdetailslabel"  Text="Invoice" />
            <asp:Label runat="server" ID="lblInvoice" Text="" />
            <asp:Label runat="server" ID="lbltxtOrderSource" class="orderdetailslabel"  Text="Order Source" />
            <asp:Label runat="server" ID="lblOrderSource" Text="" />
        </div>
        <div class="orderdetails">
            <asp:Label runat="server" ID="lbltxtOrderCreator"  class="orderdetailslabel" Text="Order Creator" />
            <asp:Label runat="server" ID="lblOrderCreator" Text="" />
            <asp:Label runat="server" ID="lbltxtTypeofCustomer"  class="orderdetailslabel" Text="Type of Customer" />
            <asp:Label runat="server" ID="lblTypeofCustomer" Text="" /></div>
        <div class="clearFix">
            <asp:Panel ID="Panel1" runat="server" CssClass="addr">
                <h3>
                    <asp:Label ID="lblShip" runat="server" Text="Shipping Address" CssClass=""></asp:Label></h3>
                <table>
                    <tr>
                        <td colspan="2" class="ckbox">
                            <asp:CheckBox ID="chkUseAddress4NewOrder" runat="server" Text="Select this customer to place the new order"
                                AutoPostBack="True" OnCheckedChanged="chkUseAddress4NewOrder_CheckedChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td class="labelDefault">
                            <asp:Label runat="server" ID="lbltxtname" Text="Name" />
                        </td>
                        <td>
                            <asp:Label ID="lblShipToName" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="labelDefault">
                            <asp:Label runat="server" ID="lbltxtorg" Text="Organization" />
                        </td>
                        <td>
                            <asp:Label ID="lblShipToOrg" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="labelDefault">
                            <asp:Label runat="server" ID="lbltxtaddr1" Text="Address 1" />
                        </td>
                        <td>
                            <asp:Label ID="lblShipToAddr1" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="labelDefault">
                            <asp:Label runat="server" ID="lbltxtaddr2" Text="Address 2" />
                        </td>
                        <td>
                            <asp:Label ID="lblShipToAddr2" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="labelDefault">
                            <asp:Label runat="server" ID="lbltxtzip5" Text="ZIP Code" />
                        </td>
                        <td>
                            <asp:Label ID="lblShipToZip5" runat="server" Text=""></asp:Label>
                            <asp:Label ID="lbltxtzip4" runat="server" Text="-" />
                            &nbsp;
                            <asp:Label ID="lblShipToZip4" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="labelDefault">
                            <asp:Label runat="server" ID="lbltxtcity" Text="City" />
                        </td>
                        <td>
                            <asp:Label ID="lblShipToCity" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="labelDefault">
                            <asp:Label runat="server" ID="lbltxtstate" Text="State" />
                        </td>
                        <td>
                            <asp:Label ID="lblShipToState" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="labelDefault">
                            <asp:Label runat="server" ID="lbltxtphone" Text="Phone" />
                        </td>
                        <td>
                            <asp:Label ID="lblShipToPhone" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="labelDefault">
                            <asp:Label ID="lbltxtemail" runat="server" Text="E-mail" />
                        </td>
                        <td>
                            <asp:Label ID="lblShipToEmail" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlBIllingInfo" runat="server" CssClass="addr billaddr">
                <h3>
                    <asp:Label ID="lblBill" runat="server" Text="Billing Address"></asp:Label></h3>
                <table>
                    <tr>
                        <td colspan="2" class="ckbox">
                        </td>
                    </tr>
                    <tr>
                        <td class="labelDefault">
                            <asp:Label runat="server" ID="lbltxt2name" Text="Name" />
                        </td>
                        <td>
                            <asp:Label ID="lblBillToName" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="labelDefault">
                            <asp:Label runat="server" ID="lbltxt2org" Text="Organization" />
                        </td>
                        <td>
                            <asp:Label ID="lblBillToOrg" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="labelDefault">
                            <asp:Label runat="server" ID="lbltxt2addr1" Text="Address 1" />
                        </td>
                        <td>
                            <asp:Label ID="lblBillToAddr1" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="labelDefault">
                            <asp:Label runat="server" ID="lbltxt2addr2" Text="Address 2" />
                        </td>
                        <td>
                            <asp:Label ID="lblBillToAddr2" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="labelDefault">
                            <asp:Label runat="server" ID="lbltxt2zip5" Text="ZIP Code" />
                        </td>
                        <td>
                            <asp:Label ID="lblBillToZip5" runat="server" Text=""></asp:Label>
                            <asp:Label runat="server" ID="lbltxt2zip4" Text="-" />&nbsp;
                            <asp:Label ID="lblBillToZip4" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="labelDefault">
                            <asp:Label runat="server" ID="lbltxt2city" Text="City" />
                        </td>
                        <td>
                            <asp:Label ID="lblBillToCity" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="labelDefault">
                            <asp:Label runat="server" ID="lbltxt2state" Text="State" />
                        </td>
                        <td>
                            <asp:Label ID="lblBillToState" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="labelDefault">
                            <asp:Label runat="server" ID="lbltxt2phone" Text="Phone" />
                        </td>
                        <td>
                            <asp:Label ID="lblBillToPhone" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="labelDefault">
                            <asp:Label runat="server" ID="lbltxt2email" Text="E-mail" />
                        </td>
                        <td>
                            <asp:Label ID="lblBillToEmail" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
        <div class="orderdetails">
            <asp:Label runat="server" ID="lbltxtCreated" class="orderdetailslabel" Text="Order Created" />
            <asp:Label runat="server" ID="lblCreated" Text="" />
            <asp:Label runat="server" ID="lbltxtDownload" class="orderdetailslabel" Text="Downloaded" />
            <asp:Label runat="server" ID="lblDownload" Text="" />
        </div>
        <div class="orderdetails">
            <asp:Label runat="server" ID="lbltxtShipped" class="orderdetailslabel" Text="Order Status" />
            <asp:Label runat="server" ID="lblShipped" Text="" />
            <asp:Label runat="server" ID="lbltxtTracking" class="orderdetailslabel" Text="Tracking Number" />
            <asp:Label runat="server" ID="lblTracking" Text="" /></div>
        <div class="orderdetails">
            <asp:Label runat="server" ID="lbltxtShipMethod" class="orderdetailslabel" Text="Ship Method" />
            <asp:Label runat="server" ID="lblShipMethod" Text="" />
            <asp:CheckBox ID="chkReOrder" runat="server" Text="Re-order" AutoPostBack="True"
                OnCheckedChanged="chkReOrder_CheckedChanged" /></div>
        <asp:Panel ID="Panel2" runat="server" CssClass="ordersummary">
            <asp:GridView runat="server" ID="grdViewItems" AutoGenerateColumns="false" GridLines="None"
                CssClass="graphic gray-border" UseAccessibleHeader="true" summary="" OnRowCommand="grdViewItems_RowCommand"
                OnRowDataBound="grdViewItems_RowDataBound" OnPreRender="grdViewItems_PreRender">
                <RowStyle CssClass="rowOdd" />
                <AlternatingRowStyle CssClass="rowEven" />
                <HeaderStyle CssClass="rowHead" />
                <Columns>
                    <asp:TemplateField HeaderText="Title" HeaderStyle-CssClass="headSub">
                        <ItemTemplate>
                            <strong>
                                <asp:Label ID="lblCoverOnly" runat="server" Text="Cover Only: " CssClass="" Visible="false"></asp:Label></strong>
                            <asp:HyperLink ID="lnkItem" runat="server">HyperLink</asp:HyperLink>
                            <br />
                            <asp:Label ID="lblItem" runat="server" Text="Label"></asp:Label>
                            <asp:Panel ID="pnlNerdo" runat="server" Style="display: none;">
                                <asp:Label ID="Label2" runat="server" Text="You have ordered the cover only (in packs of 25)."></asp:Label>
                                <div class="textContentsmsg clearFix">
                                    <p>
                                        <asp:Image ID="Image1" runat="server" AlternateText="Download" ImageUrl="images/download.gif" />
                                    </p>
                                    <p>
                                        <asp:HyperLink ID="NerdoContentlink" runat="server" Target="_blank"></asp:HyperLink><asp:Label
                                            ID="Label5" runat="server" Text=" now or from the link on your order confirmation page."></asp:Label>
                                    </p>
                                </div>
                            </asp:Panel>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Quantity" HeaderStyle-CssClass="headSub">
                        <ItemTemplate>
                            <asp:Label ID="lblQuantity" runat="server" Text=""></asp:Label>
                            <asp:TextBox ID="txtQty" runat="server" MaxLength="6" Visible="false"></asp:TextBox>
                            <asp:HiddenField ID="hdnPubID" runat="server" />
                            <br />
                            <asp:PlaceHolder ID="MessagePlaceHolder" runat="server"></asp:PlaceHolder>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="30%" />
                        <HeaderStyle HorizontalAlign="Right" CssClass="headSub" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </asp:Panel>
        <asp:Button ID="btnReturn" runat="server" class="btn goAction" OnClick="btnReturn_Click"
            Text="Previous Page" />
    </div>
</asp:Content>
