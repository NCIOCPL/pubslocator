<%@ Page Language="C#" MasterPageFile="~/pubmaster.Master" AutoEventWireup="true" CodeBehind="kiosksearch.aspx.cs" Inherits="Kiosk.kiosksearch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script type="text/javascript">

var rack = new Array();
var rackImage = new Array();
<%=rackjavascript%>
<%=rackImagejavascript%>

var imagePath = "pubimages/kioskimages/";
var imageSlot = "pubimages/kioskimages/blank_c.jpg";

var i = 0;
var j;
var k = 12;
var attractTimeout;

var numerator = rack.length
var denominator = 12;
var remainder = rack.length  % denominator;
var quotient = ( numerator - remainder ) / denominator;
var totalPage = 0;
var currPage = 1;

if (remainder > 0) {
  totalPage = quotient + 1;
}
else {
  totalPage = quotient;
}

function PageControl() {
  window.document.getElementById('btnPrevious').style.visibility = "visible";
  window.document.getElementById('btnNext').style.visibility = "visible";
  
  if (totalPage > 1) {
    if (currPage > 1) {
      //window.document.getElementById('btnPrevious').style.visibility = "visible";
      window.document.getElementById('btnPrevious').src = "images/arrowleft_off.jpg";
    }
    else {
      //window.document.getElementById('btnPrevious').style.visibility = "hidden";
      window.document.getElementById('btnPrevious').src = "images/arrowleft_disabled.jpg";
    }
    
    if (currPage < totalPage) {
      //window.document.getElementById('btnNext').style.visibility = "visible";
      window.document.getElementById('btnNext').src = "images/arrowright_off.jpg";
    }
    else {
      //window.document.getElementById('btnNext').style.visibility = "hidden";
      window.document.getElementById('btnNext').src = "images/arrowright_disabled.jpg";
    }
  }
  else {
    //window.document.getElementById('btnPrevious').style.visibility = "hidden";
    //window.document.getElementById('btnNext').style.visibility = "hidden";
    
    window.document.getElementById('btnPrevious').src = "images/arrowleft_disabled.jpg";
    window.document.getElementById('btnNext').src = "images/arrowright_disabled.jpg";
  }
}
var extratext = '<%=extratext%>';
function startloop(){
    j = 0;
    while (i < rack.length && i < k) {
      window.document.getElementById('Img'+j).src = rackImage[i];
      window.document.getElementById('Img'+j).alt = rack[i];
      window.document.getElementById('Img'+j).onclick = function(){goDetailPage(this)};
      i++;
      j++;
    }
    if (j < 12) {
      while (j < 12) {
        window.document.getElementById('Img'+j).src = imageSlot;
        window.document.getElementById('Img'+j).alt = "";
        
        j++;
      }
    }
    var from,to;
    from = ((currPage-1)*12)+1;
    to = currPage * 12;
    if (to>rack.length) to = rack.length;
    //window.document.getElementById('restext').innerHTML = 'Results ' + from + ' - ' + to + ' of ' + rack.length  + extratext;
    //if (from == 1) {
      //window.document.getElementById('restext').innerHTML = 'Results&nbsp;&nbsp;&nbsp;' + from + '-' + to + '&nbsp;of&nbsp;' + rack.length + extratext;
    //}
    //else {
      window.document.getElementById('restext').innerHTML = 'Results&nbsp;' + from + '-' + to + '&nbsp;of&nbsp;' + rack.length + '<%= extratext%>';
    //}
    PageControl();
    
}



      
function goDetailPage(arg) {
    if (arg.alt != "") {
      window.location = "detail.aspx?ConfID=<%=confid%>&prodid=" + arg.alt;
    }
}
      

function btnPrevious_onclick() {
  if (k > 12) {
    k = k - 12;
    i = k - 12;
    currPage = currPage - 1;
    startloop();
  }
  else {
    window.document.getElementById('btnPrevious').src = "images/arrowleft_disabled.jpg";
  }
  updButtons();
}

function btnNext_onclick() {
  if (k < rack.length) {
    i = k;
    k = k + 12;
    currPage = currPage + 1;
    startloop ();
    
    if (currPage < totalPage) {
      window.document.getElementById('btnNext').src = "images/arrowright_off.jpg";
    }
    else {
      window.document.getElementById('btnNext').src = "images/arrowright_disabled.jpg";
    }
  }
  else {
    window.document.getElementById('btnNext').src = "images/arrowright_disabled.jpg";
  }
  updButtons();
}
function updButtons(){
    try{
        if (document.btnPrevious.src.indexOf('disabled')>=0){
            document.btnPrevious.onmousedown = null;
            document.btnPrevious.onmouseup = null;
        }
        else {
            document.btnPrevious.onmousedown = function(){document.btnPrevious.src='images/arrowleft_on.jpg'};
            document.btnPrevious.onmouseup = function(){document.btnPrevious.src='images/arrowleft_off.jpg'};
        }

        if (document.btnNext.src.indexOf('disabled')>=0){
            document.btnNext.onmousedown = null;
            document.btnNext.onmouseup = null;
        }
        else {
            document.btnNext.onmousedown = function(){document.btnNext.src='images/arrowright_on.jpg'};
            document.btnNext.onmouseup = function(){document.btnNext.src='images/arrowright_off.jpg'};
        }

    }
    catch(e){
        //dont do anything
    }
}
setTimeout("updButtons()",200)//***EAC cheating...
</script>
<div class="attractmaincontent">
<div class="attractkioskgap">
<img src="images/arrowleft_off.jpg" id="btnPrevious" name="btnPrevious" onmouseup="this.src='images/arrowleft_off.jpg'" onmousedown="this.src='images/arrowleft_on.jpg'" onclick="btnPrevious_onclick();" style="visibility:hidden" />&nbsp;
<span id="restext" name="restext" class="results"></span>
&nbsp;<img src="images/arrowright_off.jpg" id="btnNext" name="btnNext" onmouseup="this.src='images/arrowright_off.jpg'" onmousedown="this.src='images/arrowright_on.jpg'" onclick="btnNext_onclick();" style="visibility:hidden" />
</div>

<table width="100%" cellspacing=0 cellpadding=0>
<tr><td><center>
    <table cellspacing=0 cellpadding=0  class="kioskpubsdisplay">
        <tr>
            <td class="attractgap"><img title="" alt="" src="images/attractspacer.gif" /></td>
            <td><img id="Img0" title="" alt=""  name="Img0" src="" style="filter:alpha(opacity=100)" onmousedown="this.filters.alpha.opacity=50" onmouseup="this.filters.alpha.opacity=100"/></td>
            <td class="attractgap"><img title="" alt="" src="images/attractspacer.gif" /></td>
            <td><img id="Img1" title="" alt=""  name="Img1" src="" style="filter:alpha(opacity=100)" onmousedown="this.filters.alpha.opacity=50" onmouseup="this.filters.alpha.opacity=100"/></td>
            <td class="attractgap"><img title="" alt="" src="images/attractspacer.gif" /></td>
            <td><img id="Img2" title="" alt=""  name="Img2" src="" style="filter:alpha(opacity=100)" onmousedown="this.filters.alpha.opacity=50" onmouseup="this.filters.alpha.opacity=100"/></td>
            <td class="attractgap"><img title="" alt="" src="images/attractspacer.gif" /></td>
            <td><img id="Img3" title="" alt=""  name="Img3" src="" style="filter:alpha(opacity=100)" onmousedown="this.filters.alpha.opacity=50" onmouseup="this.filters.alpha.opacity=100"/></td>
            <td class="attractgap"><img title="" alt="" src="images/attractspacer.gif" /></td>
            <td><img id="Img4" title="" alt=""  name="Img4" src="" style="filter:alpha(opacity=100)" onmousedown="this.filters.alpha.opacity=50" onmouseup="this.filters.alpha.opacity=100"/></td>
            <td class="attractgap"><img title="" alt="" src="images/attractspacer.gif" /></td>
            <td><img id="Img5" title="" alt=""  name="Img5" src="" style="filter:alpha(opacity=100)" onmousedown="this.filters.alpha.opacity=50" onmouseup="this.filters.alpha.opacity=100"/></td>
            <td class="attractgap"><img title="" alt="" src="images/attractspacer.gif" /></td>
        </tr>
    </table></center>
    </td>
</tr>  
<tr><td><img src="images/rack_mid.jpg" width="100%" height="54px"/></td></tr> 
<tr><td>
    <div class="attractkioskgap2">
    </div>
</td></tr>

<tr><td><center>
        <table cellspacing=0 cellpadding=0 class="kioskpubsdisplay">
        <tr>
            <td class="attractgap"><img title="" alt="" src="images/attractspacer.gif" /></td>
            <td><img id="Img6" title="" alt=""  name="Img6" src="" style="filter:alpha(opacity=100)" onmousedown="this.filters.alpha.opacity=50" onmouseup="this.filters.alpha.opacity=100"/></td>
            <td class="attractgap"><img title="" alt="" src="images/attractspacer.gif" /></td>
            <td><img id="Img7" title="" alt=""  name="Img7" src="" style="filter:alpha(opacity=100)" onmousedown="this.filters.alpha.opacity=50" onmouseup="this.filters.alpha.opacity=100"/></td>
            <td class="attractgap"><img title="" alt="" src="images/attractspacer.gif" /></td>
            <td><img id="Img8" title="" alt=""  name="Img8" src="" style="filter:alpha(opacity=100)" onmousedown="this.filters.alpha.opacity=50" onmouseup="this.filters.alpha.opacity=100"/></td>
            <td class="attractgap"><img title="" alt="" src="images/attractspacer.gif" /></td>
            <td><img id="Img9" title="" alt=""  name="Img9" src="" style="filter:alpha(opacity=100)" onmousedown="this.filters.alpha.opacity=50" onmouseup="this.filters.alpha.opacity=100"/></td>
            <td class="attractgap"><img title="" alt="" src="images/attractspacer.gif" /></td>
            <td><img id="Img10" title="" alt="" name="Img10" src="" style="filter:alpha(opacity=100)" onmousedown="this.filters.alpha.opacity=50" onmouseup="this.filters.alpha.opacity=100"/></td>
            <td class="attractgap"><img title="" alt="" src="images/attractspacer.gif" /></td>
            <td><img id="Img11" title="" alt="" name="Img11" src="" style="filter:alpha(opacity=100)" onmousedown="this.filters.alpha.opacity=50" onmouseup="this.filters.alpha.opacity=100"/></td>
            <td class="attractgap"><img title="" alt="" src="images/attractspacer.gif" /></td>
        </tr>
    </table></center>
    </td>
</tr>   
<tr><td><img src="images/rack.jpg" width="100%" height="44px"/></td></tr> 
</table>
</div>    
    <script language="javascript">
        startloop();
    </script>
</asp:Content>
