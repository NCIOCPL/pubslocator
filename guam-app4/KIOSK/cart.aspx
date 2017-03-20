<%@ Page Title="" Language="C#" MasterPageFile="~/pubmasterCart.Master" AutoEventWireup="true" CodeBehind="cart.aspx.cs" Inherits="PubEnt.cart" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="kioskgap">
</div>
<script type="text/javascript" language="javascript" src="scripts/cart.js"></script>
<!--Begin hidden Variables to pass values to Kiosk Search-->
<p><asp:HiddenField ID="hidCancerType" Value="" runat="server" /></p>
<p><asp:HiddenField ID="hidSubject" Value="" runat="server" /></p>
<p><asp:HiddenField ID="hidAudience" Value="" runat="server" /></p>
<p><asp:HiddenField ID="hidProductFormat" Value="" runat="server" /></p>
<p><asp:HiddenField ID="hidSeries" Value="" runat="server" /></p>
<p><asp:HiddenField ID="hidLanguages" Value="" runat="server" /></p>
<p><asp:HiddenField ID="hidst" Value="" runat="server" /></p>
<!--End hidden Variables-->
<div id="dvCartOuter" class="checkoutbox">
    <div id="Panel1" runat="server">
        <div id="dvCartInner" class="checkoutinnerbox">
            <div>
                <h2>Shopping Cart</h2>
            </div>
            
            <div id="divErrMsg" style="overflow: auto; display: none;" class="carterror">
                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                <asp:HiddenField ID="hdnTotalLimit" runat="server" />
            </div>
            
            <div></div><!--separator div-->
            <div>
                <h3 id="h3free" runat="server">NCI publications are free. We may limit the number of copies you can order.</h3>
            </div>
            <div style="margin-top:10px;"><!--Header Div-->
                <table style="border-collapse: collapse;"  class="carthead" width="100%" border="0" cellpadding="0px" cellspacing="0px">
                    <tbody>
                        <tr class="rowHead">
                            <th class="headercell" width="75%"><span>Items in your cart</span></th>
                            <th class="headercell"><span runat="server" id="spanQty">Quantity</span></th>
                        </tr>
                    </tbody>
                </table>
            </div><!--End Header Div-->
            <!--div class="cartwhitebox"-->
                <div id="cartgriddiv"><!--Grid Div-->     
                    <div id="checkoutTable"><!-- this can be on the div above if ID is ok, needs to use ID for selectors in CSS -->
                        <asp:GridView ID="grdViewItems" runat="server" AutoGenerateColumns="False" 
                            CellPadding="0" GridLines="None" 
                            onrowcommand="grdViewItems_RowCommand" 
                            onrowdatabound="grdViewItems_RowDataBound"
                            Width="100%" ShowHeader="False" onrowcreated="grdViewItems_RowCreated">
                            <RowStyle CssClass="rowEven" />
                            <Columns>
                                <asp:TemplateField HeaderText="Items in Your Cart">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCoverOnly" runat="server"  
                                            Text="Cover Only: " Visible="false"></asp:Label>
                                        <asp:LinkButton ID="lnkBtnItem" runat="server" CommandName="lnkBtn" 
                                            OnClick="lnkBtnClick" oncommand="lnkBtnItem_Command">LinkButton</asp:LinkButton>
                                        
                                        <br />
                                        <!--<asp:Label ID="lblItem" runat="server" Text="Label"></asp:Label>-->
                                        <asp:Panel ID="pnlNerdo" runat="server" style="display:none;">
                                            <asp:Label ID="Label2" runat="server" 
                                                Text="You are ordering the cover only (in packs of 25)."></asp:Label>
                                            <table cellpadding="0px" cellspacing="0px">
                                                <tr>
                                                    <td>
                                                        <asp:Image ID="Image1" runat="server" AlternateText="Download" 
                                                            ImageUrl="images/download.gif" />
                                                    </td>
                                                    <td valign="top">
                                                        <asp:HyperLink ID="NerdoContentlink" runat="server" Target="_blank"></asp:HyperLink>
                                                        <asp:Label ID="Label5" runat="server" 
                                                            Text=" now or from the link on your order confirmation page."></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" Width="335px"/>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <!--<asp:Button ID="btnDown" runat="server" Text="" CssClass="stylebuttondown"/>-->
                                        <!--<asp:Button ID="btnUp" runat="server" Text="" CssClass="stylebuttonup"/>-->
                                        <asp:ImageButton ID="ImgBtnDown" runat="server" 
                                            ImageUrl="images/arrowdown_off.jpg" />
                                        <asp:ImageButton ID="ImgBtnUp" runat="server" 
                                            ImageUrl="images/arrowup_off.jpg" />
                                        <asp:Label ID="lblEmailOnly" runat="server" Font-Italic="True" 
                                            Text="(E-mail)"></asp:Label>
                                    </ItemTemplate>

                                <HeaderStyle></HeaderStyle>

                                    <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="20%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Quantity">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnPubID" runat="server" />
                                        <asp:HiddenField ID="hdnPubLimit" runat="server" />
                                        <asp:Label ID="lblQty" runat="server" style="text-align:right" Text="" EnableViewState="False"></asp:Label>
                                        <asp:HiddenField ID="hdnQty" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="10%" 
                                        CssClass="alignr" />
                                    <HeaderStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:Button ID="btnRemove" runat="server" CausesValidation="false" 
                                            CommandName="Remove" CssClass="btn removeItem" Text="Remove" Visible="false" />
                                        <asp:ImageButton ID="ImgBtnRemove" runat="server" ImageUrl="images/remove_off.jpg" CommandName="Remove" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="10%" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div><!-- End checkouttable -->
                </div><!--End Grid Div-->
                <div style="background-color:#fff;">
                    <table width="100%">
                        <tbody>
                            <tr id="trTotalItems" runat="server">
                                <td width="40%">&nbsp;</td>
                                <td class="tablecell"><span>Total items: </span></td>
                                <td class="qtycellcart" width="10%"><div><asp:Label ID="lblTot" runat="server" CssClass="textLoud"></asp:Label></div></td>
                                <td class="tablecell" width="15%">&nbsp;</td>
                            </tr>
                            <tr id="trFreeRemain" runat="server" >
                                <td width="40%">&nbsp;</td>
                                <td class="tablecell"><span>Free copies remaining: </span></td>
                                <td class="qtycellcart" width="10%"><div><asp:Label ID="lblFreeRemaining" runat="server" CssClass="textLoud"></asp:Label></div></td>
                                <td class="tablecell" width="15%">&nbsp;</td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            <!--/div-->
        </div>
            
         
            
              
    </div>
    
    <asp:Panel ID="Panel2" runat="server">
       <div id="Div1" class="checkoutinnerbox">
       <h2>Your shopping cart is empty.</h2>
       <p>To start over from the main screen, touch <b>Cancel Order</b>.</p>
       <p>To go back to your search results, touch <b>Continue Search</b>.</p>
       <p>To search again, touch <b>Search for Other Publications</b>.</p>
       <asp:RadioButton ID="RadioButton1" AutoPostBack="true" runat="server" style="visibility:hidden; display:none;" />
       <div>
        </div>
        </div>
    </asp:Panel>
    
    <div class="contentbuttons"><asp:Button ID="btnContinueSearch" runat="server" Text="Continue Search" onclick="btnContinueSearch_Click" 
                Visible="false"/><asp:ImageButton ID="ImgBtnContinueSearch" runat="server"  
                ImageUrl="images/continuered_off.jpg" onclick="btnContinueSearch_Click"/><asp:Button ID="btnCancelOrder" runat="server" Text="Cancel Order" 
                onclick="btnCancelOrder_Click" Visible="false" /><asp:ImageButton ID="ImgBtnCancelOrder" runat="server" 
                ImageUrl="images/cancel_off.jpg" onclick="btnCancelOrder_Click"/><asp:Button ID="btnCheckOut" runat="server" 
                Text="Check Out" onclick="btnCheckOut_Click" Visible="false" /><asp:ImageButton ID="ImgBtnCheckOut" runat="server" 
                ImageUrl="images/checkout_off.jpg" onclick="btnCheckOut_Click"/></div>
    <asp:Button ID="btnKioskSearch" runat="server" Text="Kiosk Search" onclick="btnKioskSearch_Click" style="visibility:hidden; display:none;"/>               
</div><!-- end of checkoutbox -->
<script type="text/javascript">
    function goSearch(arg) {
        if (arg.name == "optCancerType") {
            window.document.getElementById("<%=hidCancerType.ClientID%>").value = arg.options[arg.selectedIndex].value;
            //window.document.getElementById("<%=hidst.ClientID%>").value = escape(arg.options[arg.selectedIndex].text);
            window.document.getElementById("<%=hidst.ClientID%>").value = arg.options[arg.selectedIndex].text;
        }
        else {
            window.document.getElementById("<%=hidCancerType.ClientID%>").value = "";
        }

        if (arg.name == "optSubject") {
            window.document.getElementById("<%=hidSubject.ClientID%>").value = arg.options[arg.selectedIndex].value;
            //window.document.getElementById("<%=hidst.ClientID%>").value = escape(arg.options[arg.selectedIndex].text);
            window.document.getElementById("<%=hidst.ClientID%>").value = arg.options[arg.selectedIndex].text;
        }
        else {
            window.document.getElementById("<%=hidSubject.ClientID%>").value = "";
        }

        if (arg.name == "optAudience") {
            window.document.getElementById("<%=hidAudience.ClientID%>").value = arg.options[arg.selectedIndex].value;
            //window.document.getElementById("<%=hidst.ClientID%>").value = escape(arg.options[arg.selectedIndex].text);
            window.document.getElementById("<%=hidst.ClientID%>").value = arg.options[arg.selectedIndex].text;
        }
        else {
            window.document.getElementById("<%=hidAudience.ClientID%>").value = "";
        }

        if (arg.name == "optPublicationFormat") {
            window.document.getElementById("<%=hidProductFormat.ClientID%>").value = arg.options[arg.selectedIndex].value;
            //window.document.getElementById("<%=hidst.ClientID%>").value = escape(arg.options[arg.selectedIndex].text);
            window.document.getElementById("<%=hidst.ClientID%>").value = arg.options[arg.selectedIndex].text;
        }
        else {
            window.document.getElementById("<%=hidProductFormat.ClientID%>").value = "";
        }

        if (arg.name == "optSeries") {
            window.document.getElementById("<%=hidSeries.ClientID%>").value = arg.options[arg.selectedIndex].value;
            //window.document.getElementById("<%=hidst.ClientID%>").value = escape(arg.options[arg.selectedIndex].text);
            window.document.getElementById("<%=hidst.ClientID%>").value = arg.options[arg.selectedIndex].text;
        }
        else {
            window.document.getElementById("<%=hidSeries.ClientID%>").value = "";
        }

        if (arg.name == "optLanguages") {
            window.document.getElementById("<%=hidLanguages.ClientID%>").value = arg.options[arg.selectedIndex].value;
            //window.document.getElementById("<%=hidst.ClientID%>").value = escape(arg.options[arg.selectedIndex].text);
            window.document.getElementById("<%=hidst.ClientID%>").value = arg.options[arg.selectedIndex].text;
        }
        else {
            window.document.getElementById("<%=hidLanguages.ClientID%>").value = "";
        }

        __doPostBack("<%= btnKioskSearch.UniqueID%>", "");
    }
</script>

</asp:Content>
