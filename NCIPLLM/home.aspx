<%@ Page Language="C#" MasterPageFile="~/pubmaster.Master" AutoEventWireup="true"
    CodeBehind="home.aspx.cs" Inherits="PubEnt.home" Title="" %>

<%@ MasterType VirtualPath="~/pubmaster.master" %>
<%@ Register Src="usercontrols/searchbar_search.ascx" TagName="searchbar" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="home">
        <div>
            <uc2:searchbar ID="searchbar" runat="server" />
        </div>
        <div class="" id="homecols">
            <div class="col firstcol">
                <!--First Column-->
                <div class="browsegroup">
                    <a id="skiptocontent" tabindex="-1"></a>
                    <!--Types of Cancer-->
                    <h2>
                        <span>Find publications about</span> A Type of Cancer</h2>
                    <asp:DataList ID="ListCancerTypes" runat="server" OnItemDataBound="ListCancerTypes_ItemDataBound">
                        <ItemTemplate>
                            <ul class="listHomePad">
                                <li>
                                    <asp:HyperLink ID="lnkCanType" runat="server">HyperLink</asp:HyperLink>
                                </li>
                            </ul>
                        </ItemTemplate>
                    </asp:DataList>
                </div>
                <div class="browsegroup">
                    <!--A toZ list-->
                    <span class="textazlist">See the <a href="http://www.cancer.gov/cancertopics/types/alphalist/">
                        A to Z List of Cancers</a> on Cancer.gov for information on other cancer types</span>
                </div>
            </div>
            <div class=" col">
                <!--Second Column-->
                <div class="browsegroup">
                    <!--Subject-->
                    <h2>
                        <span>Find publications on </span>Cancer Topics</h2>
                    <asp:DataList ID="ListSubjs" runat="server" OnItemDataBound="ListSubjs_ItemDataBound">
                        <ItemTemplate>
                            <ul class="listHomePad">
                                <li>
                                    <asp:Label ID="lblSubj" runat="server" Visible="False">Label</asp:Label>
                                    <asp:HyperLink ID="lnkSubj" runat="server">HyperLink</asp:HyperLink>
                                </li>
                            </ul>
                        </ItemTemplate>
                    </asp:DataList>
                </div>
                <div class="browsegroup">
                    <!--Collections-->
                    <h2>
                        <span>See all publications in </span>Collections</h2>
                    <asp:DataList ID="ListCollections" runat="server" OnItemDataBound="ListCollections_IDB">
                        <ItemTemplate>
                            <ul class="listHomePad">
                                <li>
                                    <asp:HyperLink ID="lnkCollection" runat="server">HyperLink</asp:HyperLink>
                                </li>
                            </ul>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Literal ID="Literal1" runat="server" Text="&lt;ul class='listHomePad'&gt;&lt;li&gt;&lt;a href='outofstock.aspx'&gt;Out of Stock Publications&lt;/a&gt;&lt;/ul&gt;"></asp:Literal></FooterTemplate>
                    </asp:DataList>
                </div>
            </div>
            <div class=" col">
                <!--Third Column-->
                <div class="browsegroup">
                    <!--Audience-->
                    <h2>
                        <span>Find publications for </span>Audience</h2>
                    <asp:DataList ID="ListAudience" runat="server" OnItemDataBound="ListAudience_IDB">
                        <ItemTemplate>
                            <ul class="listHomePad">
                                <li>
                                    <asp:Label ID="lblAudience" runat="server" Visible="False">Label</asp:Label>
                                    <asp:HyperLink ID="lnkAudience" runat="server">HyperLink</asp:HyperLink>
                                </li>
                            </ul>
                        </ItemTemplate>
                    </asp:DataList>
                </div>
                <div class="browsegroup">
                    <!--Language-->
                    <h3>
                        <span>Written in</span></h3>
                    <asp:DataList ID="ListLanguages" runat="server" OnItemDataBound="ListLanguages_IDB">
                        <ItemTemplate>
                            <ul class="listHomePad">
                                <li>
                                    <asp:Label ID="lblLanguage" runat="server" Visible="False">Label</asp:Label>
                                    <asp:HyperLink ID="lnkLanguage" runat="server">HyperLink</asp:HyperLink>
                                </li>
                            </ul>
                        </ItemTemplate>
                    </asp:DataList>
                </div>
                <div class="browsegroup">
                    <!--Race/Eth-->
                    <h3>
                        <span>Written for & about</span></h3>
                    <asp:DataList ID="ListRace" runat="server" OnItemDataBound="ListRace_IDB">
                        <ItemTemplate>
                            <ul class="listHomePad">
                                <li>
                                    <asp:Label ID="lblRace" runat="server" Visible="False">Label</asp:Label>
                                    <asp:HyperLink ID="lnkRace" runat="server">HyperLink</asp:HyperLink>
                                </li>
                            </ul>
                        </ItemTemplate>
                    </asp:DataList>
                </div>
                <div class="browsegroup">
                    <!--Format-->
                    <h2>
                        <span>Find publications by</span>Format</h2>
                    <asp:DataList ID="ListProductFormat" runat="server" OnItemDataBound="ListProductFormat_IDB">
                        <ItemTemplate>
                            <ul class="listHomePad">
                                <li>
                                    <asp:Label ID="lblProductFormat" runat="server" Visible="False">Label</asp:Label>
                                    <asp:HyperLink ID="lnkProductFormat" runat="server">HyperLink</asp:HyperLink>
                                </li>
                            </ul>
                        </ItemTemplate>
                    </asp:DataList>
                </div>
            </div>
            <!--End of third column-->
            <div class=" col">
                <!--Fourth column-->
                <div class="browsegroup">
                    <!--ANNC but no featured for NCIPLCC-->
                    <h2 class="fh2">
                        Announcements</h2>
                    <div class="announcements" id="divAnnouncements" runat="server">
                        <asp:DataList ID="ListAnnouncements" runat="server" OnItemDataBound="ListAnn_IDB">
                            <ItemTemplate>
                                <asp:HyperLink ID="lnkAnn" runat="server" CssClass="linkAnnounce" Target="_blank"></asp:HyperLink>
                                <asp:Label ID="lblAnnYear" runat="server" Text="" CssClass="textDefault" Visible="false"></asp:Label>
                                <asp:Label ID="lblAnn" runat="server" CssClass="textMoreSymbol" Visible="false"> >> </asp:Label>
                            </ItemTemplate>
                        </asp:DataList>
                    </div>
                </div>
						                                    <% /*01-27-2012 NCIPL_CC - No stacks programming. Stacks cannot be displayed without related Admin changes
						                                    <asp:Literal ID="litStackStyle" runat="server">Stack CSS</asp:Literal>
						                                    <asp:DataList id="ListFeatures" runat="server" onitemdatabound="ListFeatures_IDB" OnPreRender="ListFeatures_PreRender">
							                                    <ItemTemplate>
							                                            <div class="featuredpubs">
                                                                            <div style="padding-bottom:2px;">
                                                                            <asp:Label ID="lblStackTitle" runat="server" Text="Stack Title" CssClass="labelDefault"></asp:Label>
                                                                            </div>
                                                                            <asp:Literal ID="litStackMarkup" runat="server">Stack Markup</asp:Literal>
							                                            </div>
							                                    </ItemTemplate>
						                                    </asp:DataList>
						                                    <asp:Literal ID="litStackScript" runat="server">Stack JavaScript</asp:Literal>
						                                    */ %>
                <div class="featuredpubs">
                    <a href="search.aspx?newupt=1" class="newupdlink">View all new/updated publications</a>
                </div>
            </div>
            <!--End of fourth column-->
        </div>
    </div>
</asp:Content>
