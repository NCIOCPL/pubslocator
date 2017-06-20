<%@ Page Title="Order free National Cancer Institute publications - NCI Publications Locator"
    Language="C#" MasterPageFile="~/pubmaster.Master" AutoEventWireup="true" CodeBehind="verify.aspx.cs"
    Inherits="PubEnt.verify" %>

<%@ MasterType VirtualPath="~/pubmaster.master" %>
<%@ Register Src="usercontrols/steps.ascx" TagName="steps" TagPrefix="uc1" %>
<%@ Register Src="usercontrols/searchbar_search.ascx" TagName="searchbar" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta name="description" content="Order free educational and support publications about cancer. Find resources for patients and their families, health care providers, and the public." />
    <script type="text/javascript">
        // <!CDATA[
        function Button3_onclick() {
            window.print();
        }
        function printconf() {
            window.print();
        }
        // ]]>
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <uc2:searchbar ID="searchbar" runat="server" />
    </div>
    <div class="indentwrap" id="verify">
        <a id="skiptocontent" tabindex="-1"></a>
        <div class="ctable clearFix">
            <asp:Literal ID="label100" runat="server" Text="<h2>Verify and Place Order</h2>"
                Mode="PassThrough"></asp:Literal>
            <div class='steps show-for-large-up'>
                <uc1:steps ID="steps1" runat="server" />
            </div>
        </div>
        <p>
            <asp:Label ID="Label2" runat="server" Text="Please verify your information below."></asp:Label>
        </p>
        <div class="ctable clearFix">
            <asp:Panel ID="pnlConfirm1" runat="server" Visible="False">
                <div class="ctable printconf show-for-large-up">
                    <div class="printconfb">
                        <asp:Button ID="Button2" runat="server" CssClass="btn" OnClientClick="javascript:printconf();"
                            Text="Print Confirmation" />
                    </div>
                </div>
                <div class="ctable">
                    <h2 class="vh2">Thank you for your order</h2>
                    <p>
                        Your order from the National Cancer Institute will be shipped within the next 2
                        business days. You will receive an e-mail shipping confirmation.
                    </p>
                </div>
                <asp:Panel ID="pnlContentDownload" runat="server" Visible="False">
                    <asp:Label ID="Label8" runat="server" CssClass="headSub" Text="Contents to Download"></asp:Label>
                    <br />
                    <ul>
                        <asp:DataList ID="ListNerdos" runat="server" OnItemDataBound="Nerdos_IDB" Width="100%">
                            <ItemTemplate>
                                <asp:Label ID="lblNerdoTitle" runat="server" Text="Label" CssClass="labelDetailFieldLeft"></asp:Label>:&nbsp;You
                                have ordered the cover only.<br />
                                <img src="images/download.gif" alt="Download" /><span class="textLoud">Print separate
                                    contents: </span>
                                <asp:HyperLink ID="lnkNerdo" runat="server">HyperLink</asp:HyperLink>
                                <br />
                            </ItemTemplate>
                        </asp:DataList>
                    </ul>
                </asp:Panel>
            </asp:Panel>
        </div>
        <div id="divCCPrompt" runat="server">
            <asp:Label ID="labelText2" runat="server" Text="<p>&lt;b&gt;&lt;span class='headSub'&gt;Note about shipping charges:&lt;/span&gt;&lt;/b&gt;&lt;/p&gt;&lt;p&gt;The estimated shipping charge shown below is based on your selected shipping vendor’s published rate, delivery zone, delivery method, weight of the order, and estimated number of packages in the shipment. Actual shipping charge may vary based on any special rate you negotiated with the shipping vendor and final number of packages in the shipment.&lt;/p&gt;"></asp:Label>
        </div>
        <h3>
            <asp:Label ID="Label12" runat="server" CssClass="" Text="Order Date"></asp:Label></h3>
        <p class="vdate">
            <asp:Label ID="lblorderdt" runat="server" Text="Label"></asp:Label>
        </p>
        <div class="row small-collapse">
            <asp:Panel ID="Panel1" runat="server" CssClass="addr columns large-6">
                <h3>
                    <asp:Label ID="Label9" runat="server" CssClass="" Text="Shipping Address"></asp:Label></h3>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblname" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblOrg" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbladdr1" runat="server" Text="Label"></asp:Label>
                            &nbsp;<asp:Label ID="lbladdr2" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblcity" runat="server" Text="Label"></asp:Label>,
                            <asp:Label ID="lblst" runat="server" Text="Label"></asp:Label>
                            &nbsp;<asp:Label ID="lblzip" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblemail" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblphone" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblAVS_Shipping" runat="server" CssClass="avsresult"></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlBillingInfo" CssClass='addr billaddr columns large-6' runat="server">
                <h3>
                    <asp:Label ID="Label10" runat="server" CssClass="" Text="Billing Address"></asp:Label></h3>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lbl2name" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl2org" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl2addr1" runat="server" Text="Label"></asp:Label>
                            &nbsp;<asp:Label ID="lbl2addr2" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl2city" runat="server" Text="Label"></asp:Label>,
                            <asp:Label ID="lbl2st" runat="server" Text="Label"></asp:Label>
                            &nbsp;<asp:Label ID="lbl2zip" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl2email" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl2phone" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
        <div class="ctable row">
            <asp:Panel Style="display: none" ID="pnlPaymentInfo" runat="server">
                <asp:Label ID="Label11" runat="server" CssClass="headSub" Text="Payment Information"></asp:Label>
            </asp:Panel>
            <asp:Button ID="btn2Shipping" CssClass="btn changeAddr" runat="server" Text="Change Addresses or Payment Information"
                OnClick="Button1_Click" />
        </div>
        <h3 class="vorder">
            <asp:Label ID="Label3" runat="server" Text="Order Summary" CssClass=""></asp:Label></h3>
        <asp:DataGrid ID="grdItems" runat="server" AutoGenerateColumns="False" UseAccessibleHeader="true"
            OnItemDataBound="grdItems_IDB" CssClass="graphic gray-border">
            <HeaderStyle CssClass="rowHead" />
            <AlternatingItemStyle CssClass="rowOdd" />
            <ItemStyle CssClass="rowEven" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left" />
            <Columns>
                <asp:TemplateColumn HeaderText="Title" SortExpression="Title" HeaderStyle-CssClass="headSub">
                    <ItemTemplate>
                        <asp:Label ID="lblTitle" runat="server" Text="Label" CssClass="textProdTitle"></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Item Details" SortExpression="Qty" Visible="false">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblDetails" runat="server" Text="Label" CssClass="textProdItemtype"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Quantity" SortExpression="Remove" HeaderStyle-CssClass="headSub qtyHead">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblQty" runat="server" Text="Label"></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle />
                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                        Font-Underline="False" HorizontalAlign="Right" />
                </asp:TemplateColumn>
            </Columns>
            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Right" Mode="NumericPages" />
        </asp:DataGrid>
        <div class="ctable">
            <asp:Button ID="btn2Cart" runat="server" OnClick="Button2_Click" CssClass="btn changeQty"
                Text="Change Quantity or Remove Items" />
        </div>
        <div class="row">
            <div class="cexp columns large-4 large-offset-8 medium-6 medium-offset-6">
                <p>
                    <span>Total items:&nbsp;</span> <strong>
                        <asp:Label ID="lblTotalItems" runat="server" Text="Label" CssClass="textLoud"></asp:Label></strong>
                </p>
                <asp:Panel ID="pnlCost" runat="server">
                </asp:Panel>
                <div id="divCost" runat="server" class="estcost">
                    <span>Estimated shipping charge:&nbsp;</span> <strong>
                        <asp:Label ID="lblCost" runat="server" CssClass="textLoud"></asp:Label></strong>
                </div>
            </div>
        </div>
        <div class="row">
            <div id="divCCExplanation" runat="server" class="cexp columns large-4 large-offset-8 medium-6 medium-offset-6 ">
                <!--Begin CC Explanation Div-->
                <ul class="listHomePad">
                    <li>NCI publications are <strong>free</strong>. </li>
                    <li>For orders of more than 20 items, we charge the actual shipping cost to the FedEx
                        or UPS shipping number you provide.</li>
                </ul>
            </div>
        </div>
        <!--End CC Explanation Div-->
        <div class='ctable clearFix'>
            <asp:Button ID="btn2Submit" runat="server" CssClass="btn goAction" OnClick="Button4_Click"
                Text="Submit Order &gt;&gt;" OnClientClick="javascript:_gaq.push(['_trackEvent','Ordering','Submit','Submit Order']);NCIAnalytics.PubsLinkTrack('Submit Order', this);" />
            <asp:Button ID="btn2Cancel" runat="server" CssClass="btn endAction" Text="Cancel Order"
                OnClick="CancelOrder" OnClientClick="javascript:_gaq.push(['_trackEvent','Ordering','Cancel','Cancel Order']);NCIAnalytics.PubsLinkTrack('Cancel Order', this);NCIAnalytics.PubsLinkTrack('Cancel Order', this);" />
            <asp:LinkButton ID="LinkButton1" runat="server" PostBackUrl="cart.aspx" CausesValidation="False"
                CssClass="linkCheckout">Back to Shopping Cart</asp:LinkButton>
        </div>
        <div class="ctable clearFix printconf">
            <asp:Panel ID="pnlConfirm2" runat="server" Visible="False">
                <asp:Label ID="LblThank" runat="server" CssClass="textLoud" Visible="False"></asp:Label>
                <div class="confirm show-for-large-up">
                    <asp:Label ID="Label7" runat="server" Text="&lt;strong&gt;Questions?&lt;/strong&gt;  To speak with a Publications Ordering Specialist, 
                call 1-800-4-CANCER (1-800-422-6237) and choose the option to order publications.  
                Please call Monday through Friday, 8:00 a.m. to 8:00 p.m. Eastern Time.&lt;br /&gt;&lt;br /&gt;To protect your privacy, 
                close all Web browser windows after printing this page."></asp:Label>
                </div>
                <div class="printconfb show-for-large-up">
                    <asp:Button ID="Button3" runat="server" class="btn" OnClientClick="javascript:printconf();"
                        Text="Print Confirmation" />
                </div>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
