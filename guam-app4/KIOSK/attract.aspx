<%@ Page Language="C#" MasterPageFile="~/pubmasterAttract.Master" AutoEventWireup="true" CodeBehind="attract.aspx.cs" Inherits="PubEnt.attract" %>
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

function startloop(){
    j = 0;
    while (i < rack.length && i < k) {
      window.document.getElementById('Img'+j).src = rackImage[i];
      window.document.getElementById('Img'+j).alt = rack[i];
      window.document.getElementById('Img'+j).onclick = function(){goDetailPage(this)};
      
      j++;
      i++;
    }
    if (j < 12) {
      while (j < 12) {
        window.document.getElementById('Img'+j).src = imageSlot;
        window.document.getElementById('Img'+j).alt = "";
        
        j++;
      }
    }
    startRotate();
}

function startRotate() { 
    attractTimeout = setTimeout('doRotate();', <%=attracttimeout%>); 
}

function stopRotate() {
    clearTimeout(attractTimeout);
}
      
function goDetailPage(arg) {
    if (arg.alt != "") {
      window.location = "detail.aspx?ConfID=<%=confid%>&prodid=" + arg.alt;
    }
}
      
function doRotate() {
    if (k < rack.length) {
          i = k;
          k = k + 12;
    }
    else {
          i = 0;
          k = 12;
    }
    startloop();
}

</script>
<div class="attractmaincontent">
<div class="attractkioskgap">
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