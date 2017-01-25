<%@ Page Title="" Language="C#" MasterPageFile="~/pubmaster.Master" AutoEventWireup="true" CodeBehind="verify_shipping.aspx.cs" Inherits="Kiosk.verify_shipping" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script type="text/javascript" language="javascript" src="scripts/cart.js"></script>
<div class="kioskgap">
</div>
    <script type="text/javascript"><%=overridejavascript%></script>
<div id="dvCartOuter" class="checkoutbox">
    <table border="0" cellpadding="0" cellspacing="0"><!--wrapper table-->
    <tr>
        <td><!--wrapper table cell-->
        
    <div id="dvCartInner" class="checkoutinnerbox">
        
         <div>
                <asp:Panel ID="pnlVerifyHeader" runat="server">
                    <h2>Verify Order</h2>
                    <div style="overflow: auto;" class="verifyerror" id="divVerifyErrDisplay" runat="server">
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                        <asp:Label ID="lblAVS_Shipping" runat="server" CssClass="avsresult"></asp:Label>
                    </div>
                    
                </asp:Panel>
                <asp:Panel ID="pnlConfirmHeader" runat="server" Visible="false">
                    <table>
                        <tr>
                            <td>
                                <h2>Thank you. Your order will be shipped within 2 business days.</h2>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="thankshead3">Summary</div>
                            </td>
                        </tr>
                    </table>
                    
                    
                </asp:Panel>
        </div>
        
        <div>
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                 <tbody><tr>
                    <td width="60%" valign="top">
                        <table width="100%" border="0px" cellpadding="2px" cellspacing="0px" id="verifyTable">
                             <tbody><tr>
                                <td>
                                    <div runat="server" id="divName">
                                        <asp:Label ID="NameLabel" runat="server" Text="Name" CssClass="labelDefault"></asp:Label>
                                    </div>
                                </td>
                                <td><asp:Label ID="NameText" runat="server" Text="" CssClass="labelDefault"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <div runat="server" id="divOrg">
                                        <asp:Label ID="OrgLabel" runat="server" Text="Organization" CssClass="labelDefault"></asp:Label>
                                        
                                    </div>
                                </td>
                                <td><asp:Label ID="OrgText" runat="server" Text="" CssClass="labelDefault"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div runat="server" id="divAddress1">
                                        <asp:Label ID="Addr1Label" runat="server" Text="Address" CssClass="labelDefault"></asp:Label>
                                        
                                    </div>
                                </td>
                                <td><asp:Label ID="Addr1Text" runat="server" Text="" CssClass="labelDefault"></asp:Label>                       
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div runat="server" id="divAddress2">
                                        <asp:Label ID="Addr2Label" runat="server" Text="" CssClass="labelDefault"></asp:Label>
                                        
                                    </div>
                                </td>
                                <td><asp:Label ID="Addr2Text" runat="server" Text="" CssClass="labelDefault"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div runat="server" id="divState">
                                        <asp:Label ID="StateLabel" runat="server" Text="" CssClass="labelDefault"></asp:Label>                                        
                                    </div>
                                </td>
                                <td>
                                    <asp:Label ID="CityText" runat="server" Text="" CssClass="labelDefault"></asp:Label>,
                                    <asp:Label ID="StateText" runat="server" Text="" CssClass="labelDefault"></asp:Label>
                                    <asp:Label ID="ZipText" runat="server" Text="" CssClass="labelDefault"></asp:Label>
                                    <asp:Label ID="Zip4Text" runat="server" Text="" CssClass="labelDefault"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div runat="server" id="divCountry">
                                        <asp:Label ID="CountyLabel" runat="server" Text="" CssClass="labelDefault"></asp:Label>
                                        
                                    </div>
                                </td>
                                <td><asp:Label ID="CountryText" runat="server" Text="" CssClass="labelDefault"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div runat="server" id="divPhone">
                                        <asp:Label ID="PhoneLabel" runat="server" Text="Phone" CssClass="labelDefault"></asp:Label>
                                        
                                        
                                    </div>
                                </td>
                                <td>
                                <asp:Label ID="PhoneText" runat="server" Text="" CssClass="labelDefault"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div runat="server" id="divEmail">
                                        <asp:Label ID="EmailLabel" runat="server" Text="E-mail" CssClass="labelDefault"></asp:Label>
                                        
                                        
                                    </div>
                                    
                                </td>
                                <td><asp:Label ID="EmailText" runat="server" Text="" CssClass="labelDefault"></asp:Label>
                                </td>
                            </tr>
                        </tbody></table>
                    </td>
                    <td>&nbsp;</td>
                    <td valign="top" class="aligntop">
                        <asp:Button ID="btnChangeShippingInfo" runat="server" 
                            Text="Change Shipping Information" onclick="btnChangeShippingInfo_Click" visible="false"/>
                        <asp:ImageButton ID="ImgBtnChangeShipInfo" runat="server" ImageUrl="images/changeship_off.jpg" title="" AlternateText="Change Shipping Information" onclick="btnChangeShippingInfo_Click"/>
                    </td>
                </tr>
            </tbody></table>
        </div>
        
        <div><!--Nerdos-->
            <asp:Panel ID="pnlContentDownload" runat="server" Visible="False">
                <asp:Label ID="Label8" runat="server" CssClass="headSub" 
                    Text="Contents to Download"></asp:Label>
                <br />
                <ul>
                <asp:DataList ID="ListNerdos" runat="server" 
                    onitemdatabound="Nerdos_IDB" Width="100%" >
                    <ItemTemplate>
                        
                         <asp:Label ID="lblNerdoTitle" runat="server" Text="Label" 
                             CssClass="labelDetailFieldLeft"></asp:Label>:&nbsp;You have ordered the cover only.<br />
                         <img src="https://cissecuretrain.nci.nih.gov/ncipubs/images/download.gif" /><span class="textLoud">Print separate contents: </span><asp:HyperLink 
                             ID="lnkNerdo" runat="server">HyperLink</asp:HyperLink>
                         
                        
                        
                         <br />
                         
                        
                        
                    </ItemTemplate>
                </asp:DataList>
                </ul>
                <br />
            </asp:Panel>
        </div><!--Nerdos-->
        
        <div style="margin-top:30px;"><!--Grid Header-->
            <table style="border-collapse: collapse;"  class="carthead" width="100%" border="0" cellpadding="0px" cellspacing="0px">
                <tbody><tr class="rowHead">
                    <th width="90%" class="headercell"><span>Items in your cart</span></th>
                    <th class="headercell"><span>Quantity</span></th>
                </tr>
            </tbody></table>
        </div><!--Grid Header-->
        
        <div style="overflow: auto; width: 100%; height: 300px;"><!--Grid-->
            <div id="checkoutTable">
            <asp:DataGrid ID="grdItems" runat="server" AutoGenerateColumns="False" 
            onitemdatabound="grdItems_IDB" BorderColor="#BDBDBD" Width="100%"
            BorderStyle="Solid" BorderWidth="0px" 
            GridLines="None" CssClass="graphic gray-border" CellPadding="3" 
                ShowHeader="False">
                <HeaderStyle CssClass="rowHead"/>
                <AlternatingItemStyle CssClass="rowOdd" />
                <ItemStyle CssClass ="rowEven" Font-Bold="False" Font-Italic="False" Font-Overline="False" 
                                 Font-Strikeout="False" Font-Underline="False" 
                    HorizontalAlign="Right" />
                <Columns>
                    <asp:TemplateColumn HeaderText="Title" SortExpression="Title" HeaderStyle-CssClass="headSub">
                        <ItemTemplate>
                            <asp:Label ID="lblTitle" runat="server" Text="Label" CssClass="textProdTitle"></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                        </EditItemTemplate>
                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" 
                            Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left" />
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Item Details" SortExpression="Qty" Visible="false">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblDetails" runat="server" Text="Label" 
                                CssClass="textProdItemtype"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Quantity" SortExpression="Remove" HeaderStyle-CssClass="headSub">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <div class="alignr"><asp:Label ID="lblQty" runat="server" Text="Label"></asp:Label></div>
                            
                        </ItemTemplate>
                        <HeaderStyle Width="60px" />
                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" 
                            Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Right" />
                    </asp:TemplateColumn>
                </Columns>
            </asp:DataGrid>
            
            <table width="100%">
                <tbody><tr>
                    <td width="55%">&nbsp;</td>
                    <td class="tablecell"><span>Total items: </span></td>
                    <td class="qtycell"><div style="text-align:right"><asp:Label ID="lblTot" runat="server" CssClass="textLoud"></asp:Label></div></td>
                    <!-- <td width="7%">&nbsp;</td> -->
                </tr>
            </tbody></table>
            </div>
        </div><!--Grid-->
      </div>  
      </td><!--wrapper table cell-->
        </tr>
    </table><!--wrapper table-->
      
        <div ID="pnlVerifyButtons" runat="server" class="contentbuttons"><asp:ImageButton ID="ImgBtnBacktoCart" runat="server" ImageUrl="images/backcart_off.jpg" onclick="ImgBtnBacktoCart_Click" /><asp:ImageButton ID="ImgBtnCancelOrder" runat="server" ImageUrl="images/cancel_off.jpg" onclick="btnCancelOrder_Click"/><asp:ImageButton ID="ImgBtnPlaceOrder" runat="server" ImageUrl="images/placeorder_off.jpg" OnClick="btnPlaceOrder_Click"/></div>
</div>
</asp:Content>
