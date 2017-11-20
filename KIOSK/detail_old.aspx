<%@ Page Title="" Language="C#" MasterPageFile="~/pubmaster.Master" AutoEventWireup="true" CodeBehind="detail_old.aspx.cs" Inherits="PubEnt.detail_old" %>
<%@ MasterType  virtualPath="~/pubmaster.master"%> 

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="kioskgap">
</div>
    <script type="text/javascript">
<%=tocjavascript%>

function gotoPage(o){
    if (o=='left')
        p = -1;
    else
        p = +1;
        
    i = i + p;
    if (i>=tocmin && i <= tocmax){
        document.toc.src=toc[i];
    }
    if (i <= tocmin){
        i = tocmin; 
        document.tocleft.src = 'images/arrowleft_disabled.jpg';
    }
    if (i >= tocmax){
        i = tocmax;
        document.tocright.src = 'images/arrowright_disabled.jpg';
    }
    if (i>tocmin) 
        document.tocleft.src = 'images/arrowleft_off.jpg';
    if (i<tocmax) 
        document.tocright.src = 'images/arrowright_off.jpg';

    window.status = toc[i];   
    updButtons();   //***EAC dynamically assign the mouse over events
}

function updButtons(){
    try{
        if (document.tocleft.src.indexOf('disabled')>=0){
            document.tocleft.onmousedown = null;
            document.tocleft.onmouseup = null;
        }
        else {
            document.tocleft.onmousedown = function(){document.tocleft.src='images/arrowleft_on.jpg'};
            document.tocleft.onmouseup = function(){document.tocleft.src='images/arrowleft_off.jpg'};
        }

        if (document.tocright.src.indexOf('disabled')>=0){
            document.tocright.onmousedown = null;
            document.tocright.onmouseup = null;
        }
        else {
            document.tocright.onmousedown = function(){document.tocright.src='images/arrowright_on.jpg'};
            document.tocright.onmouseup = function(){document.tocright.src='images/arrowright_off.jpg'};
        }

    }
    catch(e){
        //dont do anything
    }
}
setTimeout("updButtons()",200)//***EAC cheating...


</script><center>
    <table>
        <tr>
            <td valign="top" nowrap="nowrap">
            <div class="pubdetailsboxl">
                <div class="pubdetailsinnerboxl">
                <img id="tocleft" name="tocleft" alt="" onclick="Javascript:gotoPage('left');" src="images/arrowleft_disabled.jpg" />&nbsp;&nbsp;
                <img id="toc" name="toc" alt="" src="<%=tocfirst%>" />&nbsp;&nbsp;
                <img id="tocright" name="tocright" alt="" onclick="Javascript:gotoPage('right');" src="images/<%=hiddenornot%>" />
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
                                    ImageUrl="~/images/addtocart_off.jpg" onclick="OrderPublication_Click" /><asp:ImageButton ID="EmailPublication" runat="server" CommandName="EmailPub" 
                                    ImageUrl="~/images/addurl_off.jpg" onclick="EmailPublication_Click" /></div>    
            </div>
            </td>
            </tr>
        </table>
        </center>
    
</asp:Content>

