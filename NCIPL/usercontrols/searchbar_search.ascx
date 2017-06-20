<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="searchbar_search.ascx.cs"
    Inherits="PubEnt.usercontrols.searchbar_search" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
<div id="searchboxregion">
    <div id="divSearchText" class="row" runat="server">
        <!--search text box and button-->
        <div class="searchtxtbox small-6 medium-8 large-9 columns ">
            <asp:Label ID="lbltxtSearch" runat="server" Text="Search" AssociatedControlID="txtSearch"
                CssClass="hidden-label"></asp:Label>
            <cc1:TextBoxWatermarkExtender ID="SearchTextBoxWatermarkExtender" runat="server"
                TargetControlID="txtSearch" WatermarkText=" Search by title, keyword, publication number"
                WatermarkCssClass="textGray">
            </cc1:TextBoxWatermarkExtender>
            <asp:Panel ID="ucSearchPanel" DefaultButton="btnSearch" runat="server">
                <asp:TextBox ID="txtSearch" runat="server" MaxLength="100" CssClass="searchboxtext"></asp:TextBox>
            </asp:Panel>
        </div>
        <div class="searchbuttons small-6 medium-4 large-3 columns ">
            <div class="searcharea">
                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn search" OnClick="btnSearch_Click" />
            </div>
            <span class="smcartplace"></span>
           <%-- <div class=" cartsm show-for-small-only">
                <a href="cart.aspx" class="btn viewCart"
                    onclick="javascript:_gaq.push(['_trackEvent','Ordering','Submit','View Cart']);NCIAnalytics.PubsLinkTrack('View Cart', this);">Cart</a>
            </div>--%>
            <div class="searcharea show-for-xlarge-up">
                <a class="info st" href="#">Search Tips
                    <div>
                        <p>
                            <strong>Keyword Search</strong>
                        </p>
                        <p>
                            Type in the box and click Search to start.
                        </p>
                        <p>
                            Examples:
                        </p>
                        <ul>
                            <li>A word from a publication: <strong>eating</strong></li>
                            <li>Publication number: <strong>P117</strong></li>
                        </ul>
                        <p>
                            <strong>Refining a Search</strong>
                        </p>
                        <p>
                            From the search results page, click Refine Search for options to narrow your search.
                        </p>
                        <ul>
                            <li>Example: To see <strong>Treatment</strong> publications for breast cancer, click
                                Refine Search. On the Refine Search page, make sure <strong>Treatment</strong> is
                                selected and select <strong>Breast</strong> from the Type of Cancer box.</li>
                        </ul>
                    </div>
                </a>
            </div>
        </div>
    </div>
    <div id="divSearchTitle" class="titleindex show-for-large-up" runat="server">
        <!--title indices-->
        <div id="divAdvSpacer" visible="false" runat="server">
        </div>
        <span class="ti">Title Index:</span>
        <asp:Repeater ID="RepeaterLetterIndices" runat="server" OnItemDataBound="RepeaterLetterIndices_ItemDataBound">
            <ItemTemplate>
                <asp:HyperLink ID="lnkLetterIndex" runat="server" CssClass="linkPubIndex">HyperLink</asp:HyperLink>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <div id="divBlank" class="blanksearchbar" runat="server">
    </div>
</div>
