<%@ Page Title="" Language="C#" MasterPageFile="~/pubmaster.Master" MaintainScrollPositionOnPostback="True"
    AutoEventWireup="true" CodeBehind="cart.aspx.cs" Inherits="PubEnt.cart" %>

<%@ Register Src="usercontrols/steps.ascx" TagName="steps" TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/pubmaster.master" %>
<%@ Register Src="usercontrols/searchbar_search.ascx" TagName="searchbar" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <uc2:searchbar ID="searchbar" runat="server" />
    </div>
    <div class="indentwrap" id="cart">
        <a id="skiptocontent" tabindex="-1"></a>
        <div class="ctable clearFix">
            <h2>
                <asp:Label ID="Label1" CssClass="headPagehead" runat="server" Text="Your Shopping Cart"></asp:Label>
            </h2>
            <div class='steps'>
                <uc1:steps ID="steps1" runat="server" />
            </div>
        </div>
        <asp:Panel ID="Panel1" runat="server">
            <p>
                <strong>NCI publications are free.</strong>
            </p>
            <p class="carttext">
                <span id="divOrderingHelp" runat="server">Ordering more than 20 items? You will be asked
                    to log in or register, and provide a FedEx or UPS shipping number to pay actual
                    shipping costs. <a href='nciplhelp.aspx#register'>Learn more about registering for an
                        account</a></span>
            </p>
            <p class="carttext">
                We set <a href="nciplhelp.aspx#limits">limits on ordering</a> to help make publications
                available to all. Health professionals and organizations can place one order each
                calendar month. Need more copies? Learn about <a href="nciplselfprint.aspx">Self-Printing
                    Options</a> for printing publications.</p>
            <asp:GridView runat="server" ID="grdViewItems" AutoGenerateColumns="false" GridLines="None"
                CssClass="graphic gray-border" Width="100%" UseAccessibleHeader="true" OnRowCommand="grdViewItems_RowCommand"
                OnRowDataBound="grdViewItems_RowDataBound" OnPreRender="grdViewItems_PreRender">
                <RowStyle CssClass="rowOdd" />
                <AlternatingRowStyle CssClass="rowEven" />
                <HeaderStyle CssClass="rowHead" />
                <Columns>
                    <asp:TemplateField HeaderText="Items in Your Cart" HeaderStyle-CssClass="headSub">
                        <ItemTemplate>
                            <strong>
                                <asp:Label ID="lblCoverOnly" runat="server" Text="Cover Only: " CssClass="" Visible="false"></asp:Label></strong>
                            <asp:HyperLink ID="lnkItem" runat="server">HyperLink</asp:HyperLink>
                            <br />
                            <asp:Label ID="lblItem" runat="server" Text="Label"></asp:Label>
                            <asp:Panel ID="pnlNerdo" runat="server" Style="display: none;">
                                <asp:Label ID="Label2" runat="server" Text="You are ordering the cover only (in packs of 25)."></asp:Label>
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
                            <asp:Label ID="lblQuantity" runat="server" Text="Quantity" AssociatedControlID="txtQty"
                                CssClass="hidden-label" /><asp:TextBox ID="txtQty" CssClass="qty" runat="server"
                                    MaxLength="6"></asp:TextBox>
                            <asp:HiddenField ID="hdnPubID" runat="server" />
                            <br />
                            <asp:Label ID="lblQtyText" runat="server" Text="Label"></asp:Label>
                            <asp:Label ID="lblQty" runat="server" Text="Label"></asp:Label>
                            <br />
                            <asp:PlaceHolder ID="MessagePlaceHolder" runat="server"></asp:PlaceHolder>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="30%" />
                        <HeaderStyle HorizontalAlign="Right" CssClass="headSub" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:Button ID="Button2" runat="server" CommandName="Remove" Text="Remove" CausesValidation="false"
                                CssClass="btn removeItem" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="5%" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br />
            <asp:PlaceHolder ID="MessagePlaceHolder2" runat="server"></asp:PlaceHolder>
            <!--NCIPL_CC-->
            <div class="ctable clearFix">
                <asp:Button ID="btnUpdateQty" runat="server" OnClick="UpdateQty" Text="Update Quantity"
                    CssClass="btn updateQty" />
            </div>
            <div class="clearFix">
                <div class='cexp'>
                    <p>
                        Total items: <strong>
                            <asp:Label ID="lblTot" runat="server" CssClass="textLoud"></asp:Label></strong>
                        <asp:Label ID="lblShipping" runat="server" Text="Shipping Cost :" Visible="false"></asp:Label>
                        <asp:Label ID="lblCost" runat="server" CssClass="textLoud"></asp:Label></p>
                </div>
            </div>
            <div class="clearFix">
                <div id="divCCExplanation" runat="server" class="cexp">
                    <!--Begin CC Explanation Div-->
                    <p>
                        <strong><sup>*</sup>Shipping charge applies</strong></p>
                    <ul class="listHomePad">
                        <li>Be ready to provide a FedEx or UPS shipping number </li>
                        <li>Or, reduce total items to 20 or less </li>
                    </ul>
                </div>
            </div>
            <!--End CC Explanation Div-->
            <div class='ctable clearFix'>
                <asp:Button ID="Button3" runat="server" OnClick="Button3_Click" Text="Test FedEx "
                    Visible="False" />
                <asp:Button ID="btn2shipping" runat="server" OnClick="Button1_Click" Text="Check Out &gt;&gt;"
                    CssClass="btn goAction" />
                <asp:Button ID="btn2continueshop" runat="server" OnClick="Button2_Click" Text="Continue Shopping"
                    CssClass="btn continueShop" />
            </div>
        </asp:Panel>
        <asp:Panel ID="Panel2" runat="server">
            Your shopping cart is empty.<br />
            <br />
            <asp:HyperLink ID="lnk2search" runat="server" NavigateUrl="home.aspx">Find publications to read online or order</asp:HyperLink>
        </asp:Panel>
    </div>
</asp:Content>
