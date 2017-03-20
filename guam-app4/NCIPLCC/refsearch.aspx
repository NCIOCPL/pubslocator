<%@ Page Title="" Language="C#" MasterPageFile="~/pubmaster.Master" AutoEventWireup="true" CodeBehind="refsearch.aspx.cs" Inherits="PubEnt.refsearch" %>
<%@ MasterType  virtualPath="~/pubmaster.master"%> 
<%@ Register src="usercontrols/searchbar_search.ascx" tagname="searchbar" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="defaultPanel" runat="server" DefaultButton="btnSearch">
        <uc2:searchbar ID="searchbar" runat="server" />
        <asp:PlaceHolder ID="MessagePlaceHolder" runat="server"></asp:PlaceHolder>
        <div class="indentwrap" id="refine">
            <a id="skiptocontent" tabindex="-1"></a>
            <h2>
                Refine Search</h2>
            <asp:Panel ID="MsgPanel" runat="server" Visible="false" CssClass="err">
                <asp:Label ID="lblMessage" Text="" runat="server"></asp:Label>
            </asp:Panel>
            <p>
                You can narrow your search by selecting one or more options below:
            </p>
            <table>
                <tr>
                    <td class="labelDefault">
                        <asp:Label ID="labelKeywords" runat="server" Text="Keywords" AssociatedControlID="txtSearch"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtSearch" runat="server" MaxLength="100" CssClass="reffield"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="labelDefault">
                        <asp:Label ID="labelTOC" runat="server" Text="Type of Cancer" AssociatedControlID="ddlTOC"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlTOC" runat="server" CssClass="dropdownlist">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="labelDefault">
                        <asp:Label ID="labelCT" runat="server" Text="Cancer Topics" AssociatedControlID="ddlCT"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCT" runat="server" CssClass="dropdownlist">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="labelDefault">
                        <asp:Label ID="labelAud" runat="server" Text="Audience" AssociatedControlID="ddlAud"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlAud" runat="server" CssClass="dropdownlist">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="labelDefault">
                        <asp:Label ID="labelLan" runat="server" Text="Language" AssociatedControlID="ddlLan"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlLan" runat="server" CssClass="dropdownlist">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="labelDefault">
                        <asp:Label ID="labelForm" runat="server" Text="Format" AssociatedControlID="ddlForm"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlForm" runat="server" CssClass="dropdownlist">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="labelDefault">
                        <asp:Label ID="labelColl" runat="server" Text="Collections" AssociatedControlID="ddlColl"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlColl" runat="server" CssClass="dropdownlist">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="btn search" OnClick="btnReset_Click" />
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn goAction" OnClick="btnSearch_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
</asp:Content>
