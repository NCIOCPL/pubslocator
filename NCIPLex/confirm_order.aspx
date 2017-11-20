<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="confirm_order.aspx.cs"
    Inherits="NCIPLex.confirm_order" %>

<!DOCTYPE html />
<html>
<head runat="server">
    <title>NCIPLex</title>
    <link href="stylesheets/modaldialog.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="stylesheets/pubsent-styles.css" type="text/css" />
    <style>
        body { background: #fff; }
        #contentdiv { margin: 0 auto; width: 80%; }
    </style>
    <!--Begin timer section-->
    <script type="text/javascript" src="scripts/dhtmlwindow.js"></script>
    <script type="text/javascript" src="scripts/modal.js"></script>
    <script type="text/javascript" src="scripts/timer.js"></script>
    <script type="text/javascript">
        function startIdleTimer(){
            idler = setInterval("expireIdleTimer()",<%=idletimeout%>)
        }
        function expireIdle2Timer(){
            document.getElementById("progressbar").innerHTML = ++timer2 + '/' + '<%=idle2timeout%>';
            if (timer2 == <%=idle2timeout%>){
                gobackAttractScreen();
            }
            idler2 = setTimeout("expireIdle2Timer()",1000)
        } 
        //startIdleTimer(); /*This one works well too*/
        //TEMP - STOPPED FOR NOW 
        window.onload = startIdleTimer;
    </script>
    <!--End timer section-->
</head>
<body>
    <div id="bannerouterdiv">
        <!--Begin Header Divs-->
        <div class="skip">
            <a href="#skiptocontent" title="Skip to content">Skip to content</a>
        </div>
        <div id="ncibanner" class="clearFix red">
            <ul>
                <li class="nciLogo"><a href="http://www.cancer.gov" title="The National Cancer Institute">
                    The National Cancer Institute</a> </li>
                <li class="nciURL"><a href="http://www.cancer.gov" title="www.cancer.gov">www.cancer.gov</a>
                </li>
                <li class="nihText"><a href="http://www.nih.gov" title="The U.S. National Institutes of Health">
                    The National Institutes of Health</a> </li>
            </ul>
        </div>
    </div>
    <div id="contentdiv">
        <img alt="NCI Publications Locator at Exhibits" src="images/logoconf.gif" />
        <hr />
        <form id="form1" runat="server">
        <div onclick="resetTimers(event)" id="verify">
            <!--Begin Outer Div-->
            <!--Begin Timer Modal Pop-up Div-->
            <div id="modalalertdiv" style="display: none;">
                <div class="modalalert">
                    <span class="dialoghead2">Do you need more time?</span> <span>
                        <input type="button" value="Yes" class="btn" onclick="processit('yes')" />
                    </span><span id="progressbar">&nbsp;</span> <span class="timeouthead3" onclick="processit('no')">
                        If you do nothing, we'll take you back to the main screen.</span>
                </div>
            </div>
            <!--End Timer Modal Pop-up Div-->
            <a id="skiptocontent"></a>
            <div class="ctable clearFix">
                <div class="ctable printconf">
                    <div class="printconfb">
                        <input type="button" class="btn endAction" value="End This Visit" onclick="location.href='location.aspx'" />
                    </div>
                </div>
                <div class="ctable">
                    <h2 class="vh2">
                        <asp:Label ID="labelHeading" runat="server" Text="Confirmation" CssClass=""></asp:Label></h2>
                </div>
                <p>
                    <asp:Label ID="labelText1" runat="server" Text="Your order from the National Cancer Institute will be shipped within the next 2 business days."></asp:Label></p>
                <%--    <div id="divCCPrompt" name="divCCPrompt" runat="server">
                <asp:Label ID="labelText2" runat="server" Text="Your credit card will be charged when your order is shipped."></asp:Label></div>--%>
                <%--    <div id="divNerdo" name="divNerdo" runat="server">
                <!--Nerdo Contents Div-->
                <asp:Label ID="Label8" runat="server" CssClass="headSub" Text="Contents to Download"></asp:Label>
                <br />
                <ul>
                    <asp:DataList ID="ListNerdos" runat="server" OnItemDataBound="Nerdos_IDB" Width="100%">
                        <ItemTemplate>
                            <asp:Label ID="lblNerdoTitle" runat="server" Text="Label" CssClass="labelDetailFieldLeft"></asp:Label>:&nbsp;You
                            have ordered the cover only.<br />
                            <img src="images/download.gif" alt="Download" />
                            <span class="textLoud">Print separate contents: </span>
                            <asp:HyperLink ID="lnkNerdo" runat="server">HyperLink</asp:HyperLink>
                            <br />
                        </ItemTemplate>
                    </asp:DataList>
                    <br />
                    <a href="http://www.cancer.gov/viewing-files" class="linkNavFooter">Viewing Files</a>
                </ul>
            </div>--%>
            </div>
            <h3>
                Order Date</h3>
            <p class="vdate">
                <asp:Label ID="lblorderdt" runat="server" Text="Label"></asp:Label></p>
            <div id="divShipping" name="divShipping" runat="server">
            </div>
            <%-- <div id="divBilling" name="divBilling" runat="server">
                </div>
                <div id="divPayment" name="divPayment" runat="server">
                </div>--%>
            <!--Ordered Items Grid-->
            <div class="vtotal clearFix">
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
            <div class="vtotal ctable">
                <div id="divTotalItems" name="divTotalItems" runat="server">
                    <p>
                        Total items: <strong>
                            <asp:Label ID="lblTotalItems" runat="server" Text="Label" CssClass="textLoud"></asp:Label>
                        </strong>
                    </p>
                </div>
            </div>
            <%-- <div id="divCost" name="divCost" runat="server" >
                Shipping and Handling:&nbsp;<asp:Label ID="lblCost" runat="server" CssClass="textLoud"></asp:Label>
            </div>--%>
            <div class="ctable clearFix printconf">
                <p>
                    <strong>
                        <asp:Label ID="LblThank" runat="server" CssClass="textLoud" Text="Thank you for ordering from the National Cancer Institute."></asp:Label></strong></p>
                <div class="confirm">
                    <strong>
                        <asp:Label ID="Label7" runat="server" Text="To speak with a Publications Ordering Specialist, 
                call 1-800-4-CANCER (1-800-422-6237) and choose the option to order publications.  
                Please call Monday through Friday, 8:00 a.m. to 8:00 p.m. Eastern Time."></asp:Label></strong></div>
                <div class="printconfb">
                    <input type="button" class="btn endAction" value="End This Visit" onclick="location.href='location.aspx'" /></div>
            </div>
        </div>
        <!--End Outer Div-->
        </form>
    </div>
</body>
</html>
