<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PubRecord.aspx.cs" Inherits="PubEntAdmin.PubRecord"
    EnableEventValidation="false" ValidateRequest="false" EnableViewStateMac="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="UserControl/SpellCkr.ascx" TagName="SpellCkr" TagPrefix="uc1" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>Publication Record</title>
    <link href="Css/Common.css" type="text/css" rel="stylesheet">
</head>
<body class="Admin">
    <div class="skip"><a href="#mainBody">Skip to content</a></div>
    <div id="container">
        <h1>Publications Enterprise Admin Tool</h1>
        <h2> Publication Record</h2>
        <noscript>
            This site requires Javascript. Please enable Javascript in your browser or switch to a browser 
			that supports Javascript in order to view this site.
        </noscript>
        <form id="formPubRecord" runat="server">
            <div id="header">
                <asp:PlaceHolder ID="plcHldMenu" runat="server"></asp:PlaceHolder>
            </div>
            <div id="mainBody">
                <asp:ScriptManager ID="ScriptManager_PubRcod" EnableScriptGlobalization="false" runat="Server"
                    LoadScriptsBeforeUI="true" EnableHistory="True"
                    OnNavigate="PubRecordScriptMgmtNavigate">
                    <Scripts>
                        <asp:ScriptReference Path="~/JS/PubRecord.js" />
                        <asp:ScriptReference Path="~/JS/charCount.js" />
                    </Scripts>
                </asp:ScriptManager>
                <h3>
                    <asp:Label ID="lblPageTitle" runat="server" Text="" CssClass="headPagehead"></asp:Label></h3>
                <asp:Label ID="lblMsg" runat="server" Text="" CssClass="PageTitle"></asp:Label>
                <div class="recordnav">
                    <asp:HyperLink ID="hplnkBakSechRes" runat="server">Back to Search Results</asp:HyperLink>
                    <asp:HyperLink ID="hplnkRefSech" runat="server">Refine Search</asp:HyperLink>
                    <uc1:SpellCkr ID="SpellCkr1" runat="server" />
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn"
                        OnClick="btnSave_Click"
                        CausesValidation="true" />
                    <asp:Button ID="btnCancel" runat="server" CssClass="btn"
                        Text="Cancel" OnClick="btnCancel_Click" />
                    <input id="btnEdit" type="button" value="Edit" class="btn" runat="server" />
                </div>
                <div id="recordcoresection">
                    <asp:PlaceHolder ID="plcHldGenData" runat="server"></asp:PlaceHolder>
                </div>
                <div id="recorddetailsection">
                    <asp:PlaceHolder ID="plcHldGenProjSetting" runat="server"></asp:PlaceHolder>
                </div>
                <div id="recordtabsection">
                    <table id="tbLiveInterfaces" runat="server">
                        <tr>
                            <td>
                                <strong>Live Interfaces</strong>
                            </td>
                        </tr>
                        <tr id="trLiveInterfaces" runat="server" valign="top">
                            <td>
                                <asp:PlaceHolder ID="plcHldLiveInt" runat="server"></asp:PlaceHolder>
                            </td>
                        </tr>
                    </table>
                    <div id="recordtabs">
                        <asp:PlaceHolder ID="plcHldLiveIntTab" runat="server"></asp:PlaceHolder>
                    </div>
                </div>
                <div class="recordnav">
                    <asp:HyperLink ID="hplnkBakSechRes2" runat="server">Back to Search Results</asp:HyperLink>
                    <asp:HyperLink ID="hplnkRefSech2" runat="server">Refine Search</asp:HyperLink>
                    <uc1:SpellCkr ID="SpellCkr2" runat="server" />
                    <asp:Button ID="btnSave2" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="btn" CausesValidation="true" />
                    <asp:Button ID="btnCancel2" runat="server" Text="Cancel" OnClick="btnCancel_Click" CssClass="btn" />
                    <input id="btnEdit2" type="button" value="Edit" runat="server" />
                </div>
            </div>
        </form>
    </div>
</body>
</html>
