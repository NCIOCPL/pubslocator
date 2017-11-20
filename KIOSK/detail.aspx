<%@ Page Title="" Language="C#" MasterPageFile="~/pubmaster.Master" AutoEventWireup="true" CodeBehind="detail.aspx.cs" Inherits="PubEnt.detail" %>
<%@ MasterType  virtualPath="~/pubmaster.master"%> 

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="kioskgap">
</div>
<link rel="stylesheet" href="stylesheets/flexslider.css" type="text/css">
<%--<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.6.2/jquery.min.js"></script>--%>
<script type="text/javascript" src="scripts/jquery.min.js"></script>
<script type="text/javascript" src="scripts/jquery.flexslider.js"></script>
<script type="text/javascript" charset="utf-8">
  $(window).load(function() {
    $('.flexslider').flexslider({
        animation:"slide"
    });
  });
</script>
<center>
    <table>
        <tr>
            <td valign="top" nowrap="nowrap">
            <div class="pubdetailsboxl">
                <div class="pubdetailsinnerboxl">
                    <%=flexslider%>
                </div>
            </div>
            </td>
            <td>
                 <div class="pubdetailsinnerboxm">&nbsp;</div>
            </td>
            <td>
            <div class="pubdetailsboxr" >
                <div class="pubdetailsinnerboxr">
                <table border="0px" style="width:100%;">
                    <tr>
                        <td><h2><asp:Label ID="Label2" runat="server" Text="Publication Details"></asp:Label> &mdash; 
                        <asp:Label ID="lblProductID" CssClass="" runat="server"></asp:Label>
                        <asp:Label ID="lblNIHText" runat="server" Text="&mdash;&nbsp;NIH&nbsp;#:&nbsp;"></asp:Label><asp:Label ID="lblNIH" CssClass="" runat="server"></asp:Label>
                        </h2></td>
                    </tr>
                    <tr>
                        <td>
                        <!-- MAKE TABLE WIDTH 100% -->
                        <table class="detailtable"  width="100%" align="right"  cellpadding="0px" cellspacing="0px">
                            <tr valign="top">
                                <td align="right" class="labelDetailField" >Title:&nbsp;</td>
                                <td align="left">
                                    <div class="pubtitle"><asp:Label ID="lblTitle" CssClass="detailstitle" runat="server" ></asp:Label></div>
                                </td>
                                <!--<td></td>-->
                            </tr>
                            <tr valign="top">
                                <td align="right" class="labelDetailField">
                                    <asp:Label ID="lblFormatText" runat="server" Text="Format:&nbsp;"></asp:Label>
                                </td>
                                <td align="left">
                                    <div class="detailsinfowrapper"><asp:Label ID="lblFormat" CssClass="detailsinfo" runat="server"></asp:Label></div>
                                </td>
                                <!--<td></td>-->
                            </tr>
                            <tr valign="top">
                                <td align="right" class="labelDetailField">
                                    <asp:Label ID="lblNumPagesText" runat="server" Text="No. of Pages:&nbsp;"></asp:Label>
                                </td>
                                <td align="left">
                                    <div class="detailsinfowrapper"><asp:Label ID="lblNumPages" CssClass="detailsinfo" runat="server" Text="Label"></asp:Label></div>
                                    <asp:Image ID="imgTOC" runat="server" Visible="False" />
                                </td>
                                <!--<td></td>-->
                            </tr>
                            <tr valign="top">
                                <td align="right" class="labelDetailField">
                                    <asp:Label ID="lblAudText" runat="server" Text="Audience:&nbsp;"></asp:Label>
                                </td>
                                <td align="left">
                                    <div class="detailsinfowrapper"><asp:Label ID="lblAud" CssClass="detailsinfo" runat="server"></asp:Label></div>
                                </td>
                                <!--<td></td>-->
                            </tr>
                            <tr valign="top">
                                <td align="right" class="labelDetailField">
                                    <asp:Label ID="lblLangText" runat="server" Text="Language:&nbsp;"></asp:Label>
                                </td>
                                <td align="left">
                                    <div class="detailsinfowrapper"><asp:Label ID="lblLang" CssClass="detailsinfo" runat="server"></asp:Label></div>
                                </td>
                                <!--<td></td>-->
                            </tr>
                            <tr valign="top">
                                <td align="right" class="labelDetailField">
                                    <asp:Label ID="lblDescText" runat="server" Text="Description:&nbsp;"></asp:Label>
                                </td>
                                <td align="left">
                                    <div class="pubdescription"><asp:Label ID="lblDesc" CssClass="detailsinfo" runat="server"></asp:Label></div>
                                </td>
                                <!--<td></td>-->
                            </tr>
                            <tr valign="top">
                                <td align="right" class="labelDetailField">
                                    <asp:Label ID="lblLastupdText" runat="server"></asp:Label>
                                </td>
                                <td align="left">
                                    <div class="detailsinfowrapper"><asp:Label ID="lblLastupd" CssClass="detailsinfo" runat="server"></asp:Label></div>
                                </td>
                                <!--<td></td>-->
                            </tr>
                            <!--<tr valign="top">
                                <td align="right" class="labelDetailField">
                                    <asp:Label ID="lblProductIDText" runat="server" Text="Pub. Number:&nbsp;"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblProductIDx" CssClass="detailsinfo" runat="server"></asp:Label>
                                </td>
                                I see 
                            </tr> -->
                            <!--<tr valign="top">
                                <td align="right" class="labelDetailField">
                                    <asp:Label ID="lblOrderLimitText" runat="server" Text="Order Limit:&nbsp;"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblOrderLimit" CssClass="detailsinfo" runat="server"></asp:Label>
                                </td>
                                
                            </tr>-->
                           <!-- <tr valign="top">
                                <td align="right" class="labelDetailField">
                                    <asp:Label ID="lblNIHTextx" runat="server" Text="NIH Number:&nbsp;"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblNIHx" CssClass="detailsinfo" runat="server"></asp:Label>
                                </td>
                                
                            </tr>-->
                            <!--<tr valign="top">
                                <td align="right" class="labelDetailField">
                                    <asp:Label ID="lblPhysicalDescText" runat="server" Text="Phys. Desc:&nbsp;"></asp:Label>
                                </td>
                                <td  align="left">
                                    <asp:Label ID="lblPhysicalDesc" CssClass="detailsinfo" runat="server" Text=""></asp:Label>
                                </td>
                                
                            </tr>-->
                            <tr>
                                <td colspan="2">

                                <asp:Panel ID="pnlHtmlPdf" class="qr qr_web" runat="server">
                                    <asp:Image ID="imgHtmlPdf" runat="server" 
                                        ImageUrl="" /><div class="qr_caption">Webpage</div>
                                    </asp:Panel>
                                    
                                <asp:Panel ID="pnlKindle" class="qr qr_kindle" runat="server">
                                    <asp:Image ID="imgKindle" runat="server" 
                                        ImageUrl="" /><div class="qr_caption">Kindle</div>
                                    </asp:Panel>
                                                                         
                                <asp:Panel ID="pnlOther" class="qr qr_ereader" runat="server">                                 
                                     <asp:Image ID="imgOther" runat="server" 
                                         ImageUrl="" /><div class="qr_caption">Other E-readers</div>
                                     </asp:Panel>
                                             
                                </td>
                                <!--<td></td>-->
                            </tr>
                        </table>
                
                        </td>
                    </tr>
        
                </table>
                </div><!-- end pubdetailsinnerboxr -->
   
                <div class="pubdetailsbuttons"><asp:ImageButton ID="ContinueSearch" runat="server" ImageUrl="~/images/continuered_off.jpg" onclick="ContinueClick" 
                                     /><asp:ImageButton ID="OrderPublication" runat="server" CommandName="OrderPub" 
                                    ImageUrl="~/images/addtocart_off.jpg" onclick="OrderPublication_Click" /><asp:ImageButton ID="URLPublication" runat="server" CommandName="EmailPub" 
                                    ImageUrl="~/images/addurl_off.jpg" onclick="URLPublication_Click" /></div>    
            </div>
            </td>
            </tr>
        </table>
        </center>
    
</asp:Content>

