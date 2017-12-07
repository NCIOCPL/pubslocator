<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="conf.aspx.cs" Inherits="NCIPLex.conf" %>

<!DOCTYPE html />
<html>
<head runat="server">
    <title>NCIPLex</title>
    <link rel="stylesheet" href="stylesheets/nciplex-styles.css" type="text/css" />
    <link rel="stylesheet" href="stylesheets/banner-only.css" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="outerdiv">
        <!--Begin Header Divs-->
        <div class="skip">
            <a href="#skiptocontent" title="Skip to content">Skip to content</a>
        </div>
        <div id="ncibanner" class="clearFix red">
            <ul class="">
                <li class="nciLogo"><a href="http://www.cancer.gov" title="The National Cancer Institute">
                    The National Cancer Institute</a> </li>
                <li class="nciURL"><a href="http://www.cancer.gov" title="www.cancer.gov">www.cancer.gov</a>
                </li>
                <li class="nihText"><a href="http://www.nih.gov" title="The U.S. National Institutes of Health">
                    The National Institutes of Health</a> </li>
            </ul>
        </div>
        <!-- end header -->
        <!--begin content area-->
        <div class="selectconfdiv">
            <a id="skiptocontent"></a>
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
                <h2 class="selectconfhead">
                    <asp:Label ID="lblConf" runat="server" AssociatedControlID="ddlConfName">Select a conference, then press Continue.</asp:Label>
                </h2>
                <asp:DropDownList ID="ddlConfName" runat="server">
                    <asp:ListItem Value="0">[Select a Conference]</asp:ListItem>
                </asp:DropDownList>
                &nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnContinue" runat="server" Text="Continue" OnClick="btnContinue_Click" CssClass="btn" />
            <% } else { %>
                <h2 class="selectconfhead">
                    <asp:Label ID="lblConfAvailability" runat="server" Text="There is no conference scheduled at this time."></asp:Label>
                </h2>
            <% } %>

            <!--            
                <br /><br />
                <table >
                    <tr>
                        <td>Database Parameters:</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>Page timeout (secs):</td>
                        <td><asp:Label ID="labelPageTimeOut" runat="server" Text="Page timeout" Visible="false"></asp:Label></td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>Session timeout (secs):</td>
                        <td><asp:Label ID="labelSessionTimeOut" runat="server" Text="Session timeout" Visible="false"></asp:Label></td>
                        <td>&nbsp;</td>
                    </tr>
                </table>
                -->
        </div>
        <!--end content area-->
    </div>
    </form>
</body>
</html>
