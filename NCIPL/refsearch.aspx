<%@ Page Title="Order free National Cancer Institute publications - NCI Publications Locator"
    Language="C#" MasterPageFile="~/pubmaster.Master" AutoEventWireup="true" CodeBehind="refsearch.aspx.cs"
    Inherits="PubEnt.refsearch" %>

<%@ MasterType VirtualPath="~/pubmaster.master" %>
<%@ Register Src="usercontrols/searchbar_search.ascx" TagName="searchbar" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta name="description" content="Order free educational and support publications about cancer. Find resources for patients and their families, health care providers, and the public." />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="defaultPanel" runat="server" DefaultButton="btnSearch">
        <uc2:searchbar ID="searchbar" runat="server" />
        <asp:PlaceHolder ID="MessagePlaceHolder" runat="server"></asp:PlaceHolder>
        <div class="indentwrap" id="refine">
            <a id="skiptocontent" tabindex="-1"></a>
            <h2>Refine Search</h2>
            <asp:Panel ID="MsgPanel" runat="server" Visible="false" CssClass="err">
                <asp:Label ID="lblMessage" Text="" runat="server"></asp:Label>
            </asp:Panel>
            <p>
                You can narrow your search by selecting one or more options below:
            </p>
            <div class="row">
                <div class="medium-3 large-2 columns">
                    <asp:Label ID="labelKeywords" runat="server" Text="Keywords" AssociatedControlID="txtSearch"></asp:Label>
                </div>
                <div class="medium-6 large-5 columns left">
                    <asp:TextBox ID="txtSearch" runat="server" MaxLength="100" CssClass="reffield"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="medium-3 large-2 columns">
                    <asp:Label ID="labelTOC" runat="server" Text="Type of Cancer" AssociatedControlID="ddlTOC"></asp:Label>
                </div>
                <div class="medium-6 large-5 columns left">
                    <asp:DropDownList ID="ddlTOC" runat="server" CssClass="sm_toc">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="medium-3 large-2 columns">
                    <asp:Label ID="labelCT" runat="server" Text="Cancer Topics" AssociatedControlID="ddlCT"></asp:Label>
                </div>
                <div class="medium-6 large-5 columns left">
                    <asp:DropDownList ID="ddlCT" runat="server" CssClass="sm_ct">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="medium-3 large-2 columns">
                    <asp:Label ID="labelAud" runat="server" Text="Audience" AssociatedControlID="ddlAud"></asp:Label>
                </div>
                <div class="medium-6 large-5 columns left">
                    <asp:DropDownList ID="ddlAud" runat="server" CssClass="sm_aud">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="medium-3 large-2 columns">
                    <asp:Label ID="labelLan" runat="server" Text="Language" AssociatedControlID="ddlLan"></asp:Label>
                </div>
                <div class="medium-6 large-5 columns left">
                    <asp:DropDownList ID="ddlLan" runat="server" CssClass="sm_lang">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="medium-3 large-2 columns">
                    <asp:Label ID="labelForm" runat="server" Text="Format" AssociatedControlID="ddlForm"></asp:Label>
                </div>
                <div class="medium-6 large-5 columns left">
                    <asp:DropDownList ID="ddlForm" runat="server" CssClass="sm_form">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="medium-3 large-2 columns">
                    <asp:Label ID="labelColl" runat="server" Text="Collections" AssociatedControlID="ddlColl"></asp:Label>
                </div>
                <div class="medium-6 large-5 columns left">
                    <asp:DropDownList ID="ddlColl" runat="server" CssClass="sm_coll">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="medium-3 large-2 columns">
                    &nbsp;
                </div>
                <div class="medium-6 large-5 columns left">
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="btn search" OnClick="btnReset_Click" />
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn goAction" OnClick="btnSearch_Click" />
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
