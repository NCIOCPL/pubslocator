﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default2.aspx.cs" Inherits="PubEnt.default2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <script src="//assets.adobedtm.com/f1bfa9f7170c81b1a9a9ecdcc6c5215ee0b03c84/satelliteLib-e63d05a8f53e643e9e80f6ace129c3cf2b49d7bc.js"></script>    
    <title>NCI Virtual Publication Rack</title>
<link rel="stylesheet" href="stylesheets/kiosk-styles.css" type="text/css" /> 
<!-- NEED TO ADD APPLICATION TITLE AND STYLESHEEET -->   
</head>
<body>
    <form id="form1" runat="server">
    <!-- ADD THE TOP BANNER AND SOME PADDING ON CONTENT -->
    <div id="kioskbanner"><h1>NCI Virtual Publication Rack Conference Selection </h1></div>
    <div style="padding: 0 66px;">

    <%-- Display conference dropdown if there are any active conferences. If there are no active conferences, do not proceed any further --%>
    <% if(ddlConfName.Items.Count > 0) { %>
        <h2>Select a conference, then press Continue.</h2>
        <asp:DropDownList ID="ddlConfName" runat="server">
            <asp:ListItem Value="0">[Select a Conference]</asp:ListItem>
            <asp:ListItem>Touchscreen Exhibit</asp:ListItem>
        </asp:DropDownList>
        &nbsp;
        <input id="Button1" type="button" value="Continue" onclick="window.open('attract.aspx?ConfID=' + ddlConfName.value + '<%=kioskparams%>', 'ecwindow','status=0, left=0,top=0,screenX=0,screenY=0,width=screen.availWidth, height=screen.availHeight, resizable=yes,  scrollbars=0, toolbar=0')"/>
        <br />
        <br />
    <% } else { %>
        <h2>There is no conference scheduled at this time.</h2>
    <% } %>

    <table>
        <tr>
            <td>
    <asp:Label ID="Label1" runat="server" Text="Database Parameters"></asp:Label>
    :</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                Rotate timeout (secs):</td>
            <td>
                <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
    Page timeout (secs):
            </td>
            <td>
    <asp:Label ID="Label3" runat="server" Text="Label"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
    Session timeout (secs):     
            </td>
            <td>
                <asp:Label ID="Label4" runat="server" Text="Label"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
    <br />
    </form>
<hr />
 
</div>
    <script src="https://static.cancer.gov/webanalytics/wa_pubs_pageload.js"></script>
    <script type="text/javascript">_satellite.pageBottom();</script>
</body>
</html>
