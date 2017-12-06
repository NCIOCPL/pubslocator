<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="PubEnt._default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" href="//static.cancer.gov/returnToNCI-bar/returnToNCI-bar--parent.css" />
	<script type="text/javascript" src="//static.cancer.gov/returnToNCI-bar/returnToNCI-bar.js" async></script>
    <title>NCI Virtual Publication Rack</title>
    <link rel="stylesheet" href="stylesheets/kiosk-styles.css" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <!-- ADD THE TOP BANNER AND SOME PADDING ON CONTENT -->
    <div id="kioskbanner">
        <h1>
            NCI Virtual Publication Rack Conference Selection
        </h1>
    </div>
    <div style="padding: 0 66px;">
        <p class="alert">
            <strong>Warning Notice</strong></p>
        <p class="notice">
            This is a U.S. Government computer system, which may be accessed and used only for
            authorized Government business by authorized personnel. Unauthorized access or use
            of this computer system may subject violators to criminal, civil, and/or administrative
            action.</p>
        <p class="notice">
            All information on this computer system may be intercepted, recorded, read, copied,
            and disclosed by and to authorized personnel for official purposes, including criminal
            investigations. Such information includes sensitive data encrypted to comply with
            confidentiality and privacy requirements. Access or use of this computer system
            by any person, whether authorized or unauthorized, constitutes consent to these
            terms. There is no right of privacy in this system.</p>

        <%-- Display conference dropdown if there are any active conferences. If there are no active conferences, do not proceed any further --%>
        <% if(ddlConfName.Items.Count > 0) { %>
            <h2>
                Select a conference, then press Continue.
            </h2>
            <asp:DropDownList ID="ddlConfName" runat="server">
                <asp:ListItem Value="0">[Select a Conference]</asp:ListItem>
                <asp:ListItem>Touchscreen Exhibit</asp:ListItem>
            </asp:DropDownList>
            &nbsp;
            <!--input id="Button1" type="button" value="Continue" onclick="window.open('attract.aspx?ConfID=' + ddlConfName.value + '<%//=kioskparams%>', 'ecwindow','status=1, left=0,top=0,screenX=0,screenY=0,width=screen.availWidth, height=screen.availHeight, resizable=1, scrollbars=1')"/-->
            <asp:Button ID="btnContinue" runat="server" Text="Continue" OnClick="btnContinue_Click" />
            <br />
            <br />
        <% } else { %>
            <h2>There is no conference scheduled at this time.</h2>
        <% } %>

        <table>
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Database Parameters"></asp:Label>
                    :
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    Rotate timeout (secs):
                </td>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    Page timeout (secs):
                </td>
                <td>
                    <asp:Label ID="Label3" runat="server" Text="Label"></asp:Label>
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    Session timeout (secs):
                </td>
                <td>
                    <asp:Label ID="Label4" runat="server" Text="Label"></asp:Label>
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
        <br />
    </div>
    </form>
    <hr />
</body>
</html>
