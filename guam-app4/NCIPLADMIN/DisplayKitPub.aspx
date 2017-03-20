<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DisplayKitPub.aspx.cs"
    Inherits="PubEntAdmin.DisplayKitPub" ValidateRequest="false" %>

<%@ Register Src="UserControl/ListMultiSelect.ascx" TagName="ListMultiSelect" TagPrefix="uc1" %>
<!DOCTYPE html >
<html>
<head runat="server">
    <title></title>
    <link href="CSS/Common.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function start(Obj) {
            var vartext = null;
            var varArr = null;
            var varbox = Obj;
            vartext = document.getElementById('HidCmd')
            if (vartext.value.search(Obj.value) != -1) {
                vartext.value = vartext.value.replace(Obj.value + "|", '');
            }
            else {
                vartext.value += Obj.value + "|";
            }
            varArr = vartext.value.split("|")
            for (var v = 0; v < varArr.length; v++) {
                for (var f = 0; f < Obj.options.length; f++) {
                    Obj.options[f].selected = false;
                }
            }
            for (var v = 0; v < varArr.length; v++) {
                for (var f = 0; f < Obj.options.length; f++) {
                    if (Obj.options[f].value == varArr[v]) {
                        Obj.options[f].selected = true;
                    }
                }
            }
            ProIDInterfaceVal();
        }
        function contains(one, element) {
            var len = one.length;
            for (var i = 0; i < len; i++) {
                if (one[i] == element) {
                    return true;
                }
            }
            return false;
        }
        function containsAny(one, another) {
            for (var i = 0; i < another.length; i++) {
                if (contains(one, another[i]))
                    return true;
            }
            return false;
        }
        function containsAll(one, another) {
            for (var i = 0; i < another.length; i++) {
                if (!contains(one, another[i]))
                    return false;
            }
            return true;
        }
        function val() {
            if ($get("HiddenVal").value == '1')
                return true;
            else if ($get("HiddenVal").value == '0')
                return false;
            else {
                setTimeout(val(), 1000);
            }
        }
        function Rolling() {
            //alert('on rolling');
            if ($get("HiddenVal").value == '0') {
                alert('The publication you provide has not been assigned to the interface(s) you selected.');
                $get("HiddenVal").value = '1';
                return false;
            }
            else {
                return true;
            }
        }
        function ProIDInterfaceVal() {
            document.getElementById('lblErrmsg').value = '';
            var pub = document.getElementById('txtKitID').value;
            var obj = document.getElementById("lstboxKitPubInt");
            var srcs = new Array();
            var i = 0;
            var srcs_i = 0;
            for (; i < obj.length; i++) {
                if (obj[i].selected) {
                    srcs[srcs_i] = obj[i].value;
                    srcs_i++;
                }
            }
            var ret = false;
            if ((pub.length > 0) && (srcs.length > 0)) {
                PubEntAdmin.ws_category.GetProdInterfaceByProdID(pub, Number($get("HiddenIsVK").value), GetProdInterfaceByProdIDCallback, ErrorHandler, TimeOutHandler);
            }
        }
        function GetProdInterfaceByProdIDCallback(result) {
            var obj = document.getElementById("lstboxKitPubInt");
            var i = 0;
            var srcs = new Array();
            var srcs_i = 0;
            for (; i < obj.length; i++) {
                if (obj[i].selected) {
                    srcs[srcs_i] = obj[i].value;
                    srcs_i++;
                }
            }
            //alert(result.length);
            if (result.length == 0) {
                alert('The Publication you provide does not exist.');
            }
            else if (result.length == 1) {
                if (result[0] == "InvalidInput")
                    window.location = "InvalidInput.aspx";
            }
            else if (result.length != 1) {
                var l_result = new Array();
                if (Number(result[0]) > 0) {
                    for (i = 3; i < result.length; i++) {
                        l_result[(i - 3)] = result[i];
                    }
                    if (containsAny(l_result, srcs)) {
                        $get("HiddenVal").value = '0';
                        if ($get("HiddenIsVK").value == 1)
                            alert("This Virtual Publication ID already exists for the interface(s) selected");
                        else
                            alert("This Linked Publication ID already exists for the interface(s) selected.");
                    }
                    else {
                        $get("HiddenVal").value = '1';
                    }
                }
                else {
                    for (i = 1; i < result.length; i++) {
                        l_result[(i - 1)] = result[i];
                    }

                    if (!containsAll(l_result, srcs)) {
                        $get("HiddenVal").value = '0';
                        alert('The publication you provide has not been assigned to the interface(s) you selected.');
                    }
                    else
                        $get("HiddenVal").value = '1';
                }
            }
        }
        function TimeOutHandler(result) {
            alert("Timeout :" + result);
        }
        function ErrorHandler(result) {
            var msg = result.get_exceptionType() + "\r\n";
            msg += result.get_message() + "\r\n";
            msg += result.get_stackTrace();
            //alert(msg);
        }
    </script>
    <noscript>
        This site requires Javascript. Please enable Javascript in your browser or switch to a browser 
			that supports Javascript in order to view this site.
    </noscript>
</head>
<body class="Admin">
    <div class="skip"><a href="#mainBody">Skip to content</a></div>
    <div id="container">
        <h1>Publications Enterprise Admin Tool</h1>
        <h2>
            <asp:Label ID="lblPageTitle" runat="server" Text=""></asp:Label></h2>
        <form id="formDisplayKits" runat="server">
            <asp:ScriptManager ID="ScriptManager_DisplayKits" runat="Server">
                <Services>
                    <asp:ServiceReference Path="~/WS/ws_category.asmx" />
                </Services>
            </asp:ScriptManager>
            <div id="header">
                <asp:PlaceHolder ID="plcHldMenu" runat="server"></asp:PlaceHolder>
            </div>
            <div id="mainBody">
                <asp:Label ID="lblInstruction" runat="server" CssClass="labelDefault" Text=""></asp:Label>
                <asp:Label ID="lblErrmsg" runat="server" Text="" CssClass="errorText"></asp:Label>
                <asp:Label ID="lblMainPubID" runat="server" Text="Main Publication ID" AssociatedControlID="txtKitID" CssClass=""></asp:Label>
                <asp:Label ID="lblInt" runat="server" Text="Interface(s)" AssociatedControlID="lstboxKitPubInt" CssClass=""></asp:Label>

                <input id="HiddenIsVK" type="hidden" runat="server" />
                <asp:TextBox ID="txtKitID" runat="server" MaxLength="10"></asp:TextBox>
                <input id="HidCmd" type="hidden" />
                <asp:ListBox ID="lstboxKitPubInt" runat="server" SelectionMode="Multiple" Rows="3">
                    <asp:ListItem Text="NCIPL" Value="NCIPL"></asp:ListItem>
                    <asp:ListItem Text="ROO" Value="ROO"></asp:ListItem>
                </asp:ListBox>
                <input id="HiddenVal" type="hidden" value="1" runat="server" />
                <asp:Button ID="btnCreate" runat="server" Text="Create New" OnClick="btnCreate_Click" />
                <asp:PlaceHolder ID="plcHldKits" runat="server" />
            </div>
        </form>
    </div>
</body>
</html>
