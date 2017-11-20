<%@ Page Title="Order free National Cancer Institute publications - NCI Publications Locator"
    Language="C#" MasterPageFile="~/pubmaster.Master" AutoEventWireup="true" CodeBehind="changepwd.aspx.cs"
    Inherits="PubEnt.changepwd" %>

<%@ MasterType VirtualPath="~/pubmaster.master" %>
<%@ Register Src="usercontrols/searchbar_search.ascx" TagName="searchbar" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta name="description" content="Order free educational and support publications about cancer. Find resources for patients and their families, health care providers, and the public." />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <uc2:searchbar ID="searchbar" runat="server" />
    </div>
    <div class="indentwrap" id="changepw">
        <a id="skiptocontent" tabindex="-1"></a>
        <h2>
            <asp:Label ID="Label1" runat="server" Text="Change Password" CssClass="headPagehead"></asp:Label></h2>
        <div id="divChangePwd" class="ctable clearFix" runat="server">
            <p>
                Due to new password security requirements, the password needs to be changed.

            </p>
            <p>
                * Required field
            </p>
            <p>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
                <asp:Label ID="lblGuamMsg" runat="server" Text="" ForeColor="Red"></asp:Label>
            </p>
            <asp:Panel ID="Panel2" runat="server" CssClass="row" ><div class="columns large-10 addr payinfo">
                <table role="presentation">
                    <tr class="uname">
                        <td class="labelDefault">User Name *
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblUser" Text="" />
                        </td>
                    </tr>
                    <tr>
                        <td class="labelDefault">
                            <asp:Label ID="lblCurrentPassword" runat="server" AssociatedControlID="txtCurrentPassword"
                                Text="Current Password" />
                            *
                        </td>
                        <td>
                            <asp:TextBox ID="txtCurrentPassword" AutoCompleteType="None" class="addrfield" TextMode="Password"
                                runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtCurrentPassword"
                                ErrorMessage="Missing Current Password">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="labelDefault"></td>
                        <td>User password must include a minimum of eight(8) characters and contain three of the following four categories: 1. Uppercase characters (A through Z) 2. Lowercase characters (a through z) 3. Numbers (0 through 9) 4. Non-alphanumeric characters (Examples: !, $, #, %)
                        </td>
                    </tr>
                    <tr>
                        <td class="labelDefault">
                            <asp:Label ID="lblNewPassword" runat="server" AssociatedControlID="txtNewPassword"
                                Text="New Password" />
                            *
                        </td>
                        <td>
                            <asp:TextBox ID="txtNewPassword" AutoCompleteType="None" class="addrfield" TextMode="Password"
                                runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtNewPassword"
                                ErrorMessage="Missing New Password">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="labelDefault">
                            <asp:Label ID="lblConfirmPassword" runat="server" AssociatedControlID="txtConfirmPassword"
                                Text="Confirm New Password" />
                            *
                        </td>
                        <td>
                            <asp:TextBox ID="txtConfirmPassword" AutoCompleteType="None" class="addrfield" TextMode="Password"
                                runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtConfirmPassword"
                                ErrorMessage="Missing Confirm New Password">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table></div>
            </asp:Panel>
            <div class="loginbtns">
                <asp:Button ID="btnChangePassword" runat="server" class="btn goAction" OnClick="btnChangePassword_Click"
                    Text="Change Password" />
                <asp:Button ID="btnForgotPassword" runat="server" class="btn verifyOrder" Text="Forgot Password" />
                <asp:Button ID="btnCancel" runat="server" class="btn verifyOrder" Text="Cancel" Visible="false" />
            </div>
        </div>
        <div id="divChangePwdConfirmation" runat="server">
            <p>
                Your account password has been changed successfully.
            </p>
            <asp:Button ID="btnReturn" runat="server" class="btn returnPreviousPage" OnClick="btnReturn_Click"
                Text="Return to Previous Page" />
        </div>
    </div>
</asp:Content>
