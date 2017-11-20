<%@ Page Title="" Language="C#" MasterPageFile="~/pubmaster.Master" AutoEventWireup="true"
    CodeBehind="home.aspx.cs" Inherits="NCIPLex.home" %>

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
                    <!--Newupdated, AZ list but no featured for NCIPLCC-->
                    <asp:DataList ID="ListProductUpdates" runat="server" OnItemDataBound="ListProductUpdates_IDB">
                        <ItemTemplate>
                            <span class="exnewupd"><asp:Image ImageUrl="images/star.gif" ID="imgnewupdated" runat="server" ></asp:Image></span>
                           <span class="exnewupd"> <asp:HyperLink ID="lnkPU" runat="server" >HyperLink</asp:HyperLink></span>
                        </ItemTemplate>
                    </asp:DataList>
                    <div>
                        <span class="textazlist">See the <a href="http://www.cancer.gov/cancertopics/types/alphalist/">
                            A to Z List of Cancers</a> on Cancer.gov for information on other cancer types</span>
                    </div>
                </div>
            </div>
            <!--End of fourth column-->
        </div>
    </div>
</asp:Content>
