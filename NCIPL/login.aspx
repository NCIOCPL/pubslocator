<%@ Page Title="Order free National Cancer Institute publications - NCI Publications Locator"
    Language="C#" MasterPageFile="~/pubmaster.Master" AutoEventWireup="true" CodeBehind="login.aspx.cs"
    Inherits="PubEnt.login" %>

<%@ MasterType VirtualPath="~/pubmaster.master" %>
<%@ Register Src="usercontrols/searchbar_search.ascx" TagName="searchbar" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <uc2:searchbar ID="searchbar" runat="server" />
    </div>
    <div class="indentwrap">
        <a id="skiptocontent" tabindex="-1"></a>
        <h2>
            <asp:Label ID="Label1" runat="server" Text="Login" CssClass="lhead"></asp:Label></h2>
        <div id="divChangePwd" class="row" runat="server">
            <asp:Panel ID="Panel2" runat="server">
                <div class="loginhave columns medium-6">
                    <h3>I have an account
                    </h3>
                    <p>
                        * Required field
                    </p>
                    <p>
                        <asp:Label ID="lblGuamMsg" runat="server" Text="" ForeColor="Red"></asp:Label>
                    </p>
                    <span>
                        <asp:Label runat="server" ID="lblUserName" AssociatedControlID="txtUserName" Text="User Name" />
                        * <em>(your e-mail address)</em></span>
                    <asp:TextBox ID="txtUserName" runat="server" class="addrfield" OnTextChanged="txtUserName_TextChanged"
                        MaxLength="256"></asp:TextBox>
                    <span>
                        <asp:Label ID="lblCurrentPassword" runat="server" AssociatedControlID="txtPassword"
                            Text="Password" />
                        *
                    </span>
                    <asp:TextBox ID="txtPassword" AutoCompleteType="None" class="addrfield" TextMode="Password"
                        runat="server"></asp:TextBox>
                    <div class="orderbtns">
                        <asp:Button ID="btnLogin" runat="server" class="btn goAction" OnClick="btnLogin_Click"
                            Text="Login" />
                        <asp:Button ID="btnForgotPassword" runat="server" class="btn forgot" OnClick="btnForgotPassword_Click"
                            Text="Forgot Password" />
                    </div>
                </div>
                <div class="logincreate columns medium-5 medium-offset-1">
                    <h3>I want to create an account
                    </h3>
                    <div class="loginbtns">
                        <asp:Button ID="btnRegister" runat="server" class="btn goAction" OnClick="btnRegister_Click"
                            Text="Register" />
                        <asp:Button ID="btnCancel" runat="server" class="btn verifyOrder" OnClick="btnCancel_Click"
                            Visible="false" Text="Cancel" />
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
