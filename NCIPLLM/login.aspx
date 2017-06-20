<%@ Page Title="" Language="C#" MasterPageFile="~/pubmaster.Master" AutoEventWireup="true"
    CodeBehind="login.aspx.cs" Inherits="PubEnt.login" %>

<%@ MasterType VirtualPath="~/pubmaster.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="indentwrap">
        <a id="skiptocontent" tabindex="-1"></a>
        <h2>
            <asp:Label ID="Label1" runat="server" Text="Login" CssClass=""></asp:Label></h2>
        <div id="divChangePwd" class="ctable clearFix" runat="server">
            <asp:Panel ID="Panel2" runat="server">
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
                <div class="loginhave addr">
                    <%--     <h3>
                        I have an account
                    </h3>--%>
                    <p>
                        * Required field</p>
                    <p>
                        <asp:Label ID="lblGuamMsg" runat="server" Text="" ForeColor="Red"></asp:Label></p>
                    <table>
                        <tr>
                            <td class="labelDefault">
                                <asp:Label runat="server" ID="lblUserName" AssociatedControlID="txtUserName" Text="User Name" />
                                *
                            </td>
                            <td>
                                <asp:TextBox ID="txtUserName" runat="server" class="addrfield" OnTextChanged="txtUserName_TextChanged"
                                    MaxLength="256"></asp:TextBox>
                                <br />
                                <em>(User Name is your E-mail address)</em>
                            </td>
                        </tr>
                        <tr>
                            <td class="labelDefault">
                                <asp:Label ID="lblCurrentPassword" runat="server" AssociatedControlID="txtPassword"
                                    Text="Password" />
                                *
                            </td>
                            <td>
                                <asp:TextBox ID="txtPassword" AutoCompleteType="None" class="addrfield" TextMode="Password"
                                    runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <div class="orderbtns">
                        <asp:Button ID="btnLogin" runat="server" class="btn goAction" OnClick="btnLogin_Click"
                            Text="Login" />
                        <asp:Button ID="btnForgotPassword" runat="server" class="btn forgot" OnClick="btnForgotPassword_Click"
                            Text="Forgot Password" />
                    </div>
                </div>
                <div class="logincreate addr">
                    <%-- <h3>
                        I want to create an account
                    </h3>
                    <div class="loginbtns">
                        <asp:Button ID="btnRegister" runat="server" class="btn goAction" OnClick="btnRegister_Click"
                            Text="Register" />
                        <asp:Button ID="btnCancel" runat="server" class="btn verifyOrder" OnClick="btnCancel_Click"
                            Visible="false" Text="Cancel" /></div>--%>
                </div>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
