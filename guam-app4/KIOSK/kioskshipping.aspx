<%@ Page Title="" Language="C#" MasterPageFile="~/pubmasterShipping.Master" AutoEventWireup="true" CodeBehind="kioskshipping.aspx.cs" Inherits="Kiosk.kioskshipping" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">
    
    function ordercancel_prompt() {
        windowconfirm = dhtmlmodal.open('ordercancelbox', 'div', 'modalconfirmdiv', '', 'width=500px,height=300px,left=450px,top=200px,resize=0,scrolling=0')
    }
    function process_ordercancel(whichbutton) {
        if (window.idler2) clearTimeout(idler2);
        if (whichbutton == "yes") {
            __doPostBack("<%= btnCancelOrder.UniqueID%>", "");
        }
        if (whichbutton == "no") {
            
        }
        windowconfirm.hide()
    }


    
</script>
<div id="modalconfirmdiv" style="display:none;">
    <div style="background-color: #404040; height: 100%; padding: 20px;">
    <center>
        <br /><br />
        <span class="TODO">Do you wish to cancel the order?</span>
        <br /><br />

        <input type="button" id="btnConfirmYes" name="btnConfirmYes" value="Yes" onclick="process_ordercancel('yes')"/>
        &nbsp;&nbsp;<input type="button" id="btnConfirmNo" name="btnConfirmNo" value="No" onclick="process_ordercancel('no')"/>

        <br /><br />
        <br />
    </center>
    </div>
</div>
<div class="kioskgap">
</div>
<script type="text/javascript" language="javascript">
    //var text;

    /*function setCurrentFocus(arg) {
        text = arg;
    }*/
    
    // Minimal callback function:
    /*function keyb_callback(ch) {
        // Let's bind vkeyboard to the <TEXTAREA>
        // with id="textfield":
        var val = text.value;
        switch (ch) {
            case "BackSpace":
                var min = (val.charCodeAt(val.length - 1) == 10) ? 2 : 1;
                text.value = val.substr(0, val.length - min);
                break;

            case "Enter":
                text.value += "\n";
                break;

            default:
                text.value += ch;
        }
    }*/

    /*function displayKB() {
        var vkb =
              new VKeyboard("keyboard",   // container's id, mandatory
                      keyb_callback, // reference to callback function, mandatory
        // (this & following parameters are optional)
                      false,         // create the arrow keys or not?
                      false,         // create up and down arrow keys?
                      false,        // reserved
                      false,         // create the numpad or not?
                      "",           // font name ("" == system default)
                      "20px",       // font size in px
                      "#000",       // font color
                      "#F00",       // font color for the dead keys
                      "#BDBDBD",    // keyboard base background color #FFF
                      "#BDBDBD",    // keys' background color #FFF
                      "#DDD",       // background color of switched/selected item
                      "#777",       // border color
                      "#CCC",       // border/font color of "inactive" key
        // (key with no value/disabled)
                      "#BDBDBD",    // background color of "inactive" key #FFF
        // (key with no value/disabled)
                      "#F77",       // border color of language selector's cell
                      true,         // show key flash on click? (false by default)
                      "#CC3300",    // font color during flash
                      "#FF9966",    // key background color during flash
                      "#CC3300",    // key border color during flash
                      true,        // embed VKeyboard into the page?
                      true,         // use 1-pixel gap between the keys?
                      0);           // index (0-based) of the initial layout
    }*/


    var lineHTML = "";
    var lock = 1;
    var uca = 0;
    var initial = 0;
    var filter = /[a-z]/

    var currFocusInput;
    var currFocusInputSize;
    var currFocusInputNumOnly;

    var shipLocation = "<%=shipLocation %>";


    /*function rkey() {
        lineHTML = "";
        currFocusInput.value = lineHTML;
    }*/

    /*function skey(i)   // Großschreibung
    {
        if (i == "uc") {
            uca = 1;
            //window.document.getElementById('capsdisp').value = "Abc";
            window.document.getElementById('capsdisp').innerHTML = "Abc";
        }

        if (i == "caps") {
            lock = (lock * -1);
            if (lock < 0) {
                window.document.getElementById('lockdisp').value = "ABC";
            }

            if (lock > 0) {
                window.document.getElementById('lockdisp').value = "";
            }
        }
    }*/

    /*function zkey(i)   // Zeichen
    {
        if ((i == ",") && (uca == 1)) {
            i = ";"
        }

        if ((i == ".") && (uca == 1)) {
            i = ":"
        }

        if ((i == "-") && (uca == 1)) {
            i = "_"
        }

        if ((i == "+") && (uca == 1)) {
            i = "*"
        }

        if (!currFocusInputNumOnly) {
            lineHTML += i;
            setCurrInputVal();
            //currFocusInput.value = lineHTML;
        }
        
        uca = 0;
        //window.document.getElementById('capsdisp').value = "";
        window.document.getElementById('capsdisp').innerHTML = "";
    }*/

    /*function ukey(i) {
        if ((i == "ä") && ((uca == 1) || (lock < 0))) {
            i = "Ä";
        }
        if ((i == "ö") && ((uca == 1) || (lock < 0))) {
            i = "Ö";
        }
        if ((i == "ü") && ((uca == 1) || (lock < 0))) {
            i = "Ü";
        }

        lineHTML += i;
        setCurrInputVal();       
        //currFocusInput.value = lineHTML;
        
        uca = 0;
        //window.document.getElementById('capsdisp').value = "";
        window.document.getElementById('capsdisp').innerHTML = "";
    }*/

    function nkey(i)  
    {
        /*if ((uca == 1) && (filter.test(i))) {
           /i = i.toUpperCase();
        }
        if ((lock < 0) && (filter.test(i))) {
            i = i.toUpperCase();
        }*/

        if (filter.test(i)) {
            i = i.toUpperCase();
        }
        
        if (!currFocusInputNumOnly) {
            lineHTML += i;
            setCurrInputVal();
            //currFocusInput.value = lineHTML;
        }
        else {
            if (!isNaN(i)) {
                lineHTML += i;
                setCurrInputVal();
                //currFocusInput.value = lineHTML;
            }
        }
        
        //uca = 0;
        //window.document.getElementById('capsdisp').value = "";
        //window.document.getElementById('capsdisp').innerHTML = "";
    }

    function fkey(i) {
        if (i == "bs") {
            lineHTML = lineHTML.slice(0, (lineHTML.length - 1));
        }
        setCurrInputVal();
        //currFocusInput.value = lineHTML;
    }

    /*function ekey() {
        opener.vollsuche.test.value = lineHTML;
        window.close();
    }*/

    function setCurrentFocus(arg, argSize, argNumOnly) {
        currFocusInput = arg;
        currFocusInputSize = argSize;
        currFocusInputNumOnly = argNumOnly;
        lineHTML = currFocusInput.value;
    }

    function setDefaultCurrentFocus(arg) {
        window.document.getElementById(arg).focus();
    }

    function ckey(i) {
        if (!currFocusInputNumOnly) {
          lineHTML += i;
          setCurrInputVal();
          //currFocusInput.value = lineHTML;
        }
    }

    function setCurrInputVal() {
        if (lineHTML.length <= currFocusInputSize) {
            currFocusInput.value = lineHTML;

            if (currFocusInput.id == "<%=txtZip.ClientID%>" && currFocusInput.value.length == 5) {
                getCityState();
            }
        }
    }

    function getCityState() {
        __doPostBack("<%= txtZip.UniqueID%>", "");
    }

    // returns true if the string is a US phone number formatted as...
    // (000)000-0000, (000) 000-0000, 000-000-0000, 000.000.0000, 000 000 0000, 0000000000
    function isPhoneNumber(str) {
        var re = /^\(?[2-9]\d{2}[\)\.-]?\s?\d{3}[\s\.-]?\d{4}$/
        return re.test(str);
    } 

    function checkEmail(arg) {
        //var email = arg;
        var filter = /^([a-zA-Z0-9_.-])+@(([a-zA-Z0-9-])+.)+([a-zA-Z0-9]{2,4})+$/;
        if (!filter.test(arg)) {
            //alert('Please provide a valid email address');
            //email.focus
            return false;
        }
        else {
            return true;
        }
    }

    function ValidateShipping() {
        var strErrMsg = "Missing";
        
        if ((trim(window.document.getElementById('<%=txtContRef.ClientID %>').value)) == "") {
            strErrMsg = strErrMsg + " Name";
        }

        if ((trim(window.document.getElementById('<%=txtAddr1.ClientID %>').value)) == "") {
            if (strErrMsg != "Missing") {
                strErrMsg = strErrMsg + ", Address";
            }
            else {
                strErrMsg = strErrMsg + " Address";
            }
        }
        
        if (shipLocation == "Domestic") {
            if ((trim(window.document.getElementById('<%=txtZip.ClientID %>').value)) == "") {
                if (strErrMsg != "Missing") {
                    strErrMsg = strErrMsg + ", ZIP Code";
                }
                else {
                    strErrMsg = strErrMsg + " ZIP Code";
                }
            }

            if (window.document.getElementById('<%=selCity.ClientID %>').options.length == 0) {
                if (strErrMsg != "Missing") {
                    strErrMsg = strErrMsg + ", City";
                }
                else {
                    strErrMsg = strErrMsg + " City";
                }
            }

            if ((trim(window.document.getElementById('<%=txtState.ClientID %>').value)) == "") {
                if (strErrMsg != "Missing") {
                    strErrMsg = strErrMsg + ", State";
                }
                else {
                    strErrMsg = strErrMsg + " State";
                }
            }
        }
        else if (shipLocation == "International") {
            if ((trim(window.document.getElementById('<%=txtPostal.ClientID %>').value)) == "") {
                if (strErrMsg != "Missing") {
                    strErrMsg = strErrMsg + ", Postal";
                }
                else {
                    strErrMsg = strErrMsg + " Postal";
                }
            }

            if ((trim(window.document.getElementById('<%=txtCity.ClientID %>').value)) == "") {
                if (strErrMsg != "Missing") {
                    strErrMsg = strErrMsg + ", City";
                }
                else {
                    strErrMsg = strErrMsg + " City";
                }
            }

            /*if ((trim(window.document.getElementById('<%//=txtState.ClientID %>').value)) == "") {
                if (strErrMsg != "Missing") {
                    strErrMsg = strErrMsg + ", Province";
                }
                else {
                    strErrMsg = strErrMsg + " Province";
                }
            }*/

            if ((trim(window.document.getElementById('<%=txtCountry.ClientID %>').value)) == "") {
                if (strErrMsg != "Missing") {
                    strErrMsg = strErrMsg + ", Country";
                }
                else {
                    strErrMsg = strErrMsg + " Country";
                }
            }
        }

        if (shipLocation == "Domestic") {
            if (trim(window.document.getElementById('<%=txtPhone.ClientID%>').value) != "") {
                if (isPhoneNumber(trim(window.document.getElementById('<%=txtPhone.ClientID%>').value))) {

                }
                else {
                    if (strErrMsg != "Missing") {
                        strErrMsg = strErrMsg + "<br/>" + "This phone number is not a 10-digit format like 555-555-5555.";
                    }
                    else {
                        strErrMsg = "This phone number is not a 10-digit format like 555-555-5555.";
                    }
                }
            }
        }
        
        if (trim(window.document.getElementById('<%=txtEMail.ClientID%>').value) != "") {
            if (checkEmail(trim(window.document.getElementById('<%=txtEMail.ClientID%>').value))) {
                    
            }
            else {
                if (strErrMsg != "Missing") {
                    strErrMsg = strErrMsg + "<br/>" + "This e-mail address may not be a valid format (like name@example.com).";
                }
                else {
                    strErrMsg = "This e-mail address may not be a valid format (like name@example.com).";
                }           
            }
        }
        else{
            if (<%=boolEmail%>)
            {
            
                if (strErrMsg != "Missing") {
                    strErrMsg = strErrMsg + ", E-mail";
                }
                else {
                    strErrMsg = strErrMsg + " E-mail";
                }            
             }
        }
        
        if (strErrMsg != "Missing") {
            window.document.getElementById('divErrMsg').innerHTML = strErrMsg;
            window.document.getElementById('divErrMsg').style.display = "";
            return false;
        }
        else {
            window.document.getElementById('divErrMsg').style.display = "none";
        }
        
    }

    function trim(stringToTrim) {
        return stringToTrim.replace(/^\s+|\s+$/g, "");
    }
    function ltrim(stringToTrim) {
        return stringToTrim.replace(/^\s+/, "");
    }
    function rtrim(stringToTrim) {
        return stringToTrim.replace(/\s+$/, "");
    }

    
</script>

<!--<style> 
    .cabutt
    {background-color: grey; color: black; width: 65px; height: 40px; 
     position: relative; border-style: outset; border-color: whitesmoke; font-size:24px;}
     
    .cabutt2
    {background-color: grey; color: black; width: 130px; height: 40px; 
     position: relative; border-style: outset; border-color: whitesmoke; font-size:24px;}
     
    .cabutt3
    {background-color: grey; color: black; width: 130px; height: 40px; 
     position: relative; border-style: outset; border-color: whitesmoke; font-size:24px;}  
</style>-->
<div class="shippingbox">
  <asp:Panel ID="Panel1" runat="server" >
    <div id="div1">
      <div class="shippinginnerbox"><!-- USE DIFFERENT INNER BOX HERE -->
      <div id="div2">
      <table class="style1" width="100%">
        <tr>
          <td width="50%" style="vertical-align:top;"><h2>Shipping</h2></td>
          <td width="50%" style="vertical-align:bottom; text-align:right;"><h3 class="requirednote">*Required</h3></td>
        </tr>
      </table>
      </div>
      
      <div id="divErrMsg" class="shippingerror" style="display:none; overflow:auto; height: 20px;">
            <!-- ZIP Code -->
            <asp:CustomValidator ID="cusvaltxtZIP" runat="server" ErrorMessage="Invalid ZIP Code" Display="Dynamic" ControlToValidate="txtZip" onservervalidate="cusvaltxtZIP_ServerValidate" CssClass="errorText" ForeColor=""></asp:CustomValidator>
      </div>
      
      <div id="div4">
        <table id="Table2">
		  <tr align="left">
		    <td class="labelDefault"><label for="<%=txtContRef.ClientID%>">&nbsp;Name*</label></td>
			<td id="inf_name" width="10"><!--*--></td>
			<td colSpan="2">
			  <asp:textbox id="txtContRef" CssClass="inputtextlong" tabIndex="0" runat="server" MaxLength="30" onfocus="setCurrentFocus(this, 30, false);"></asp:textbox>
            </td>
		  </tr>
		  <tr align="left">
			<td class="labelDefault"><label for="<%=txtOrg.ClientID%>">&nbsp;Organization</label></td>
		    <td width="10"></td>
			<td colSpan="2">
			  <asp:textbox id="txtOrg" CssClass="inputtextlong" tabIndex="1" runat="server" MaxLength="30" onfocus="setCurrentFocus(this, 30, false);"></asp:textbox>
			</td>
		  </tr>
		  <tr align="left">
			<td class="labelDefault"><label for="<%=txtAddr1.ClientID%>">&nbsp;Address*</label></td>
			<td id="inf_txtAddr1" width="10"><!--*--></td>
			<td colSpan="2">
			  <asp:textbox id="txtAddr1" CssClass="inputtextlong" tabIndex="2" runat="server" MaxLength="30" onfocus="setCurrentFocus(this, 30, false);"></asp:textbox>
            </td>
	      </tr>
		  <tr align="left">
		    <td class="labelDefault"><label for="<%=txtAddr2.ClientID%>"></label></td>
			<td width="10"></td>
			<td colSpan="2">
			  <asp:textbox id="txtAddr2" CssClass="inputtextlong" tabIndex="3" runat="server" MaxLength="30" onfocus="setCurrentFocus(this, 30, false);"></asp:textbox></td>
		  </tr>
		  <tr id="trZIP" runat="server" align="left">
		    <td class="labelDefault">&nbsp;<asp:label id="lblZip" runat="server">ZIP Code*</asp:label></td>
			<td id="inf_txtZip" width="10"><!--*--></td>
			<td width="35%">
			  <label for="<%=txtZip.ClientID%>"></label>
			  <asp:TextBox ID="txtZip" CssClass="inputtextzip" tabIndex="4" runat="server" 
                    onkeypress="if ((event.keyCode < 48) || (event.keyCode    > 57)) event.returnValue = false;" 
                    MaxLength="5" onfocus="setCurrentFocus(this, 5, true);" 
                    ontextchanged="txtZip_TextChanged" AutoPostBack="True" Width="70px"></asp:TextBox>
			  &nbsp;-&nbsp;</span>
			  <label for="<%=txtZipPlus4.ClientID%>"></label>
			  <input onkeypress="if ((event.keyCode < 48) || (event.keyCode    > 57)) event.returnValue = false;" 
                    id="txtZipPlus4" tabIndex="5" type="text" alt="zipPlus4" maxLength="4" 
                    size="15" name="txtZipPlus4" onfocus="setCurrentFocus(this, 4, true);" 
                    class="inputtextzip" runat="server">
			  <label for="<%=txtPostal.ClientID%>"></label>
			</td>
			<td><span class="requirednote">Please enter ZIP Code first to auto-complete City and State.</span></td>
          </tr>
		  <tr id="trPostal" runat="server" align="left">
		    <td class="labelDefault">&nbsp;<asp:label id="lblPostal" runat="server">Postal*</asp:label></td>
			<td id="Td1" width="10"><!--*--></td>
			<td colSpan="2">
			  <asp:textbox id="txtPostal" CssClass="inputtextlong" tabIndex="6" runat="server" MaxLength="10" onfocus="setCurrentFocus(this, 10, false);" alt="postal code"></asp:textbox>
			</td>
		  </tr>
		  <tr id="trCityDomestic" runat="server" align="left">
		    <td class="labelDefault">&nbsp;City*</td>
			<td id="inf_txtCityDomestic" width="10"><!--*--></td>
			<td colSpan="2"><label for="<%=selCity.ClientID%>"></label>
              <asp:DropDownList ID="selCity" CssClass="citydropdown" runat="server" 
                    TabIndex="7" Width="300px"></asp:DropDownList>
			</td>
			
		  </tr>
		  <tr id="trCityIntl" runat="server" align="left">
		    <td class="labelDefault">&nbsp;City*</td>
			<td id="inf_txtCity" width="10"><!--*--></td>
			<td colSpan="2"><label for="<%=txtCity.ClientID%>"></label>
			  <input id="txtCity" class="inputtextlong" tabIndex="8" type="text" alt="City" maxLength="20" name="txtCity" onfocus="setCurrentFocus(this, 20, false);" runat="server">
			</td>
			
	      </tr>
		  <tr align="left">
		    <td class="labelDefault"><label for="<%=txtState.ClientID%>">&nbsp;<asp:label id="lblState" runat="server"></asp:label></label></td>
			<td width="10"><!--*--></td>
			<td colSpan="2">
			  <asp:textbox id="txtState" tabIndex="9" runat="server" 
                    onfocus="setCurrentFocus(this, 30, false);"></asp:textbox>
			</td>
	      </tr>
		  <tr id="countryRow" align="left" runat="server">
		    <td class="labelDefault"><label for="<%=txtCountry.ClientID%>">&nbsp;Country*</label></td>
			<td id="inf_txtCountry" width="10"><!--*--></td>
			<td colSpan="2">
			  <asp:textbox id="txtCountry" CssClass="inputtextlong" tabIndex="10" runat="server" MaxLength="50" onfocus="setCurrentFocus(this, 50, false);"></asp:textbox>
            </td>
		  </tr>
		  
		  <tr id="phoneRow" align="left" runat="server">
		    <td class="labelDefault">&nbsp;Phone</td>
			<td width="10">&nbsp;</td>
			<td colSpan="2">
			  <label for="<%=txtPhone.ClientID%>"></label><input id="txtPhone" class="inputtextphone" tabIndex="11" type="text" alt="phone" maxLength="30" name="txtPhone" onblur="setCurrentFocus(this, 30, false);"  runat="server">
			  <label for="<%=txtPhone2.ClientID%>"></label><input id="txtPhone2" class="inputtextphone" tabIndex="12" type="text" alt="phone" maxLength="30" name="txtPhone2" onblur="setCurrentFocus(this, 30, false);"  runat="server">
            </td>
		  </tr>
		  <tr align="left" runat="server" id="trEmail">
		    <td class="labelDefault"><label for="<%=txtEMail.ClientID%>">&nbsp;<asp:Label 
                    ID="labelEmail" runat="server"></asp:Label>
                </label></td>
			<td width="10">&nbsp;</td>
			<td colSpan="2">
			  <input id="txtEMail" class="inputtextlong" tabIndex="13" type="text" maxLength="40" name="txtEMail" onfocus="setCurrentFocus(this, 40, false);" runat="server">
            </td>
	      </tr>
	      <tr>
	        <td colspan="2"></td>
	        <td colspan="2"><span class="emailnote">&nbsp;</span></td>
	      </tr>
		</table>
      </div>
      </div>
      <!-- End checkoutinnerbox div that holds the content but not keyboard or buttons at bottom -->
      
      <div>
        <table border="0" cellpadding="1" cellspacing="0">
    <tbody>
	<!--tr>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;~&nbsp;&nbsp;&nbsp;" onClick="ckey('~')" /></td>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;!&nbsp;&nbsp;&nbsp;" onClick="ckey('!')" /></td>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;@&nbsp;&nbsp;&nbsp;" onClick="ckey('@')" /></td>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;#&nbsp;&nbsp;&nbsp;" onClick="ckey('#')" /></td>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;$&nbsp;&nbsp;&nbsp;" onClick="ckey('$')" /></td>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;%&nbsp;&nbsp;&nbsp;" onClick="ckey('%')" /></td>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;^&nbsp;&nbsp;&nbsp;" onClick="ckey('^')" /></td>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;&amp;&nbsp;&nbsp;&nbsp;" onClick="ckey('&')" /></td>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;*&nbsp;&nbsp;&nbsp;" onClick="ckey('*')" /></td>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;(&nbsp;&nbsp;&nbsp;" onClick="ckey('(')" /></td>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;)&nbsp;&nbsp;&nbsp;" onClick="ckey(')')" /></td>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;_&nbsp;&nbsp;&nbsp;" onClick="ckey('_')" /></td>
	</tr-->		
	<tr>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;1&nbsp;&nbsp;&nbsp;" onClick="nkey(1)" /></td>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;2&nbsp;&nbsp;&nbsp;" onClick="nkey(2)" /></td>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;3&nbsp;&nbsp;&nbsp;" onClick="nkey(3)" /></td>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;4&nbsp;&nbsp;&nbsp;" onClick="nkey(4)" /></td>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;5&nbsp;&nbsp;&nbsp;" onClick="nkey(5)" /></td>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;6&nbsp;&nbsp;&nbsp;" onClick="nkey(6)" /></td>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;7&nbsp;&nbsp;&nbsp;" onClick="nkey(7)" /></td>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;8&nbsp;&nbsp;&nbsp;" onClick="nkey(8)" /></td>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;9&nbsp;&nbsp;&nbsp;" onClick="nkey(9)" /></td>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;0&nbsp;&nbsp;&nbsp;" onClick="nkey(0)" /></td>
	  <!--td colspan=2><input class="cabutt2" type="button" value="&nbsp;&nbsp;&nbsp;&larr;&nbsp;&nbsp;&nbsp;" onClick="fkey('bs')"></td-->
	</tr>
	
	<tr>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;Q&nbsp;&nbsp;&nbsp;" onClick="nkey('q')" /></td>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;W&nbsp;&nbsp;&nbsp;" onClick="nkey('w')" /></td>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;E&nbsp;&nbsp;&nbsp;" onClick="nkey('e')" /></td>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;R&nbsp;&nbsp;&nbsp;" onClick="nkey('r')" /></td>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;T&nbsp;&nbsp;&nbsp;" onClick="nkey('t')" /></td>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;Y&nbsp;&nbsp;&nbsp;" onClick="nkey('y')" /></td>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;U&nbsp;&nbsp;&nbsp;" onClick="nkey('u')" /></td>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;I&nbsp;&nbsp;&nbsp;" onClick="nkey('i')" /></td>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;O&nbsp;&nbsp;&nbsp;" onClick="nkey('o')" /></td>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;P&nbsp;&nbsp;&nbsp;" onClick="nkey('p')" /></td>
	  <!--td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;Ü&nbsp;&nbsp;&nbsp;" onClick="ukey('ü')" /></td-->
	  <!--td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;+&nbsp;&nbsp;&nbsp;" onClick="ckey('+')" /></td-->
	  <!--td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;=&nbsp;&nbsp;&nbsp;" onClick="ckey('=')" /></td-->
	</tr>
	
	<tr>
	  <!--td><input class="cabutt" type="button" value="Caps" onClick="skey('caps')"></td-->
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;A&nbsp;&nbsp;&nbsp;" onClick="nkey('a')" /></td>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;S&nbsp;&nbsp;&nbsp;" onClick="nkey('s')" /></td>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;D&nbsp;&nbsp;&nbsp;" onClick="nkey('d')" /></td>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;F&nbsp;&nbsp;&nbsp;" onClick="nkey('f')" /></td>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;G&nbsp;&nbsp;&nbsp;" onClick="nkey('g')" /></td>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;H&nbsp;&nbsp;&nbsp;" onClick="nkey('h')" /></td>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;J&nbsp;&nbsp;&nbsp;" onClick="nkey('j')" /></td>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;K&nbsp;&nbsp;&nbsp;" onClick="nkey('k')" /></td>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;L&nbsp;&nbsp;&nbsp;" onClick="nkey('l')" /></td>
	  <!--td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;Ö&nbsp;&nbsp;&nbsp;" onClick="ukey('ö')" /></td-->
	  <!--td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;Ä&nbsp;&nbsp;&nbsp;" onClick="ukey('ä')" /></td-->
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;-&nbsp;&nbsp;&nbsp;" onClick="ckey('-')" /></td>
	  <!--td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;{&nbsp;&nbsp;&nbsp;" onClick="ckey('{')" /></td-->
	  <!--td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;}&nbsp;&nbsp;&nbsp;" onClick="ckey('}')" /></td-->
	</tr>
			
	<tr>
	  <!--td colspan="2"><input class="cabutt2" type="button" value="&nbsp;&nbsp;&nbsp;&uarr;&nbsp;&nbsp;&nbsp;" onClick="skey('uc')" /></td-->
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;Z&nbsp;&nbsp;&nbsp;" onClick="nkey('z')" /></td>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;X&nbsp;&nbsp;&nbsp;" onClick="nkey('x')" /></td>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;C&nbsp;&nbsp;&nbsp;" onClick="nkey('c')" /></td>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;V&nbsp;&nbsp;&nbsp;" onClick="nkey('v')" /></td>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;B&nbsp;&nbsp;&nbsp;" onClick="nkey('b')" /></td>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;N&nbsp;&nbsp;&nbsp;" onClick="nkey('n')" /></td>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;M&nbsp;&nbsp;&nbsp;" onClick="nkey('m')" /></td>
	  <!--td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;&#39;&nbsp;&nbsp;&nbsp;" onClick="ckey('\'')" /></td-->
	  <!--td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;,&nbsp;&nbsp;&nbsp;" onClick="ckey(',')" /></td-->
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;,&nbsp;&nbsp;&nbsp;" onClick="ckey(',')" /></td>
	  <!--td><input class="cabutt" value="&nbsp;&nbsp;&nbsp;/&nbsp;&nbsp;&nbsp;" onclick="ckey('/')" type="button"></td-->
	  <!--td><input class="cabutt" value="&nbsp;&nbsp;&nbsp;?&nbsp;&nbsp;&nbsp;" onclick="ckey('?')" type="button"></td-->
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;.&nbsp;&nbsp;&nbsp;" onClick="ckey('.')" /></td>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;_&nbsp;&nbsp;&nbsp;" onClick="ckey('_')" /></td>
	</tr>
	
	<tr>
	  <!--td><center><input type="text" name="capsdisp" size="3" maxlength="4" value="" readonly /></center></td-->
	  <!--td><center><label id="capsdisp"></label></center></td-->
	  <!--td><center><input type="text" name="lockdisp" size="3" maxlength="4" value="&nbsp;" readonly /></center></td-->
	  <!--td colspan="7"><input class="cabutt3" type=button value="&nbsp;" onClick="nkey(' ')" /></td-->
	  <!--td colspan=2><input class="cabutt2" type=button value="Enter" onClick="ekey()" /></td-->
	  <!--td colspan="2">&nbsp;</td-->
	  <!--td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp`&nbsp;&nbsp;&nbsp;" onClick="ckey('`')" /></td>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp:&nbsp;&nbsp;&nbsp;" onClick="ckey(':')" /></td>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp&quot;&nbsp;&nbsp;&nbsp;" onClick='ckey("\"")' /></td>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp<&nbsp;&nbsp;&nbsp;" onClick="ckey('<')" /></td>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp>&nbsp;&nbsp;&nbsp;" onClick="ckey('>')" /></td-->
	  
	  <td colspan="6"><input class="cabutt3" value="&nbsp;[space]&nbsp;" onclick="nkey(' ')" type="button"></td>
	  <td><input class="cabutt" type="button" value="&nbsp;&nbsp;&nbsp;@&nbsp;&nbsp;&nbsp;" onClick="ckey('@')" /></td>
	  <td colspan="3"><input class="cabutt2" value="&nbsp;&nbsp;&nbsp;←&nbsp;&nbsp;&nbsp;" onclick="fkey('bs')" type="button"></td>
	</tr>
	</tbody></table>
	</div>
	
	<div class="contentbuttons"><asp:ImageButton ID="btnBacktoCart" runat="server" 
              ImageUrl="images/backcart_off.jpg" onclick="btnBacktoCart_Click" /><asp:ImageButton ID="btnCancelOrder" runat="server" 
              ImageUrl="images/cancel_off.jpg" onclick="btnCancelOrder_Click" /><asp:ImageButton ID="btnVerifyOrder" runat="server" 
              ImageUrl="images/verify_off.jpg" OnClientClick="return ValidateShipping();" 
              onclick="btnVerifyOrder_Click" /></div>
  
  <script type="text/javascript" language="javascript">
      setDefaultCurrentFocus('<%=txtContRef.ClientID%>');
  </script>
      </div>
      
    </asp:Panel>
</div>
</asp:Content>
