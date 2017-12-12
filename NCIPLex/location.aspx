<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="location.aspx.cs" Inherits="NCIPLex.location"
    EnableViewState="false" EnableViewStateMac="false" %>

<!DOCTYPE html />
<html>
<head runat="server">
    <script src="//assets.adobedtm.com/f1bfa9f7170c81b1a9a9ecdcc6c5215ee0b03c84/satelliteLib-e63d05a8f53e643e9e80f6ace129c3cf2b49d7bc.js"></script>
    <title>NCIPLex</title>
    <link rel="stylesheet" href="stylesheets/nciplex-styles.css" type="text/css" />
    <link rel="stylesheet" href="stylesheets/banner-only.css" type="text/css" />
    <script type="text/javascript">        //For HS
        if (top.frames.length != 0)
            top.location = self.document.location;
    </script>
</head>
<body id="locationpage">
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
        <div class="locationcontentdiv">
            <div class="welcomediv">
                <a id="skiptocontent"></a>
                <div class="welcometext1">
                    Welcome!</div>
                <div class="welcometext2">
                    Please make a selection to start your NCI Publications visit.</div>
            </div>
            <div class="dialogdiv">
                <div class="selectlocationdiv">
                    <div class="selectlocationhead">
                        Help us display the correct shipping and ordering information:</div>
                    <table>
                        <tr>
                            <td>
                                <div class="inusdiv">
                                    <asp:ImageButton ID="btnUS" ImageUrl="images/inus_off.jpg" runat="server" OnClick="btnUS_Click"
                                        AlternateText="I live in the United States" />
                                </div>
                            </td>
                            <td>
                                <div class="outsideusdiv">
                                    <asp:ImageButton ID="btnInternational" ImageUrl="images/outsideus_off.jpg" runat="server"
                                        OnClick="btnInternational_Click" AlternateText="I live outside the United States" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="usaexplanationtext">Includes Puerto Rico, U.S. Virgin Islands, Guam, American
                                    Samoa, and APO/FPO addresses</span>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <iframe id="blankframe" frameborder="0" width="0" height="0" runat="server" src="keepactive.aspx">
            This web site requires usage of inline frames to function properly. Please enable
            inline frames or use a browser that supports inline frames. </iframe>
    </div>
    </form>
    <script type="text/javascript">_satellite.pageBottom();</script>    
</body>
</html>
