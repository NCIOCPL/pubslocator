<%@ Page Language="C#" MasterPageFile="~/pubmaster.Master" AutoEventWireup="true"
    CodeBehind="home.aspx.cs" Inherits="PubEnt.home" Title="Order free National Cancer Institute publications - NCI Publications Locator" %>

<%@ MasterType VirtualPath="~/pubmaster.master" %>
<%@ Register Src="usercontrols/searchbar_search.ascx" TagName="searchbar" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta name="description" content="Order free educational and support publications about cancer. Find resources for patients and their families, health care providers, and the public." />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="home">
        <div>
            <uc2:searchbar ID="searchbar" runat="server" />
        </div>
        <div class="show-for-large-up">
            <a href="annc.aspx" class="anncbanner">
                <div>
                    <span class="arrow-link-white">Due to limited resources, some
                publications are no longer available in hard copy, and others are available in reduced
                quantities. Read about your options</span>
                </div>
            </a>
        </div>
        <a id="skiptocontent" tabindex="-1"></a>
        <p class="homeintrohead">Find publications by:</p>
        <ul class="small-block-grid-1 medium-block-grid-3 large-block-grid-4" id="hpcols">
            <li>
                <ul class="hpnavcol " data-accordion="myAccordionGroup" role="tablist">
                    <li class="hpnav ">
                        <a href="#panel1c" role="tab" id="panel1c-heading" aria-controls="panel1c">
                            <h2>
                                <%--<span>Find publications about </span>--%>A Type of Cancer</h2>
                        </a>
                        <div id="panel1c" class="content" role="tabpanel" aria-labelledby="panel1c-heading">
                            <asp:DataList ID="ListCancerTypes" runat="server" OnItemDataBound="ListCancerTypes_ItemDataBound">
                                <ItemTemplate>
                                    <ul class="listHomePad">
                                        <li>
                                            <asp:HyperLink ID="lnkCanType" runat="server">HyperLink</asp:HyperLink>
                                        </li>
                                    </ul>
                                </ItemTemplate>
                            </asp:DataList>
                            <span class="textazlist">See the <a href="http://www.cancer.gov/types">A to Z List of Cancers</a> on Cancer.gov for information on other cancer types</span>
                        </div>
                    </li>
                </ul>
            </li>
            <li>
                <ul class="hpnavcol " data-accordion="myAccordionGroup" role="tablist">
                    <li class="hpnav">
                        <a href="#panel4c" role="tab" id="panel4c-heading" aria-controls="panel4c">
                            <h2>
                                <%--<span>Find publications on </span>--%>Cancer Topics</h2>
                        </a>
                        <div id="panel4c" class="content" role="tabpanel" aria-labelledby="panel4c-heading">
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
                    </li>
                    <li class="hpnav ">
                        <a href="#panel5c" role="tab" id="panel5c-heading" aria-controls="panel5c">
                            <h2>
                                <%--<span>See all publications in </span>--%>Collections</h2>
                        </a>
                        <div id="panel5c" class="content" role="tabpanel" aria-labelledby="panel5c-heading">
                            <asp:DataList ID="ListCollections" runat="server" OnItemDataBound="ListCollections_IDB">
                                <ItemTemplate>
                                    <ul class="listHomePad">
                                        <li>
                                            <asp:HyperLink ID="lnkCollection" runat="server">HyperLink</asp:HyperLink>
                                        </li>
                                    </ul>
                                </ItemTemplate>
                            </asp:DataList>
                        </div>
                    </li>
                </ul>
            </li>
            <li>
                <ul class="hpnavcol " data-accordion="myAccordionGroup" role="tablist">
                    <li class="hpnav ">
                        <a href="#panel7c" role="tab" id="panel7c-heading" aria-controls="panel7c">
                            <h2>
                                <%--<span>Find publications for </span>--%>Audience</h2>
                        </a>
                        <div id="panel7c" class="content" role="tabpanel" aria-labelledby="panel7c-heading">
                            <asp:DataList ID="ListAudience" runat="server" OnItemDataBound="ListAudience_IDB">
                                <ItemTemplate>
                                    <ul class="listHomePad">
                                        <li>
                                            <asp:Label ID="lblAudience" runat="server" Visible="False">Label</asp:Label>
                                            <asp:HyperLink ID="lnkAudience" runat="server">HyperLink</asp:HyperLink>
                                        </li>
                                    </ul>
                                </ItemTemplate>
                            </asp:DataList><h3>
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
                            </asp:DataList><h3>
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
                    </li>
                    <li class="hpnav">
                        <a href="#panel8c" role="tab" id="panel8c-heading" aria-controls="panel8c">
                            <h2>
                                <%--<span>Find publications by </span>--%>Format</h2>
                        </a>
                        <div id="panel8c" class="content" role="tabpanel" aria-labelledby="panel8c-heading">
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
                    </li>
                </ul>
            </li>
            <li class="show-for-large-up">
                <ul class="hpnavcol">
                    <li class="hpnav ">
                        <asp:Literal ID="litStackStyle" runat="server">Stack CSS</asp:Literal>
                        <h2>Featured Publications</h2>
                        <asp:DataList ID="ListFeatures" runat="server" OnItemDataBound="ListFeatures_IDB"
                            OnPreRender="ListFeatures_PreRender">
                            <ItemTemplate>
                                <div class="featuredpubs">
                                    <asp:Label ID="lblStackTitle" runat="server" Text="Stack Title" CssClass=""></asp:Label>
                                    <asp:Literal ID="litStackMarkup" runat="server">Stack Markup</asp:Literal>
                                </div>
                            </ItemTemplate>
                        </asp:DataList>
                        <asp:Literal ID="litStackScript" runat="server">Stack JavaScript</asp:Literal>
                        <div class="featuredpubs">
                            <a href="search.aspx?newupt=1" class="newupdlink">View all new/updated publications</a>
                        </div>
                    </li>
                </ul>
            </li>
        </ul>
    </div>
</asp:Content>
