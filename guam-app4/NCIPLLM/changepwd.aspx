<%@ Page Title="" Language="C#" MasterPageFile="~/pubmaster.Master" AutoEventWireup="true"
    CodeBehind="changepwd.aspx.cs" Inherits="PubEnt.changepwd" %>

<%@ MasterType VirtualPath="~/pubmaster.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="indentwrap" id="changepw">
        <a id="skiptocontent" tabindex="-1"></a>
        <h2>
            <asp:Label ID="Label1" runat="server" Text="Edit Account Information and Change Password"
                CssClass="headPagehead"></asp:Label></h2>
        <div id="divChangePwd" class="ctable clearFix" runat="server">
            <p>
                Due to new password security requirements, the password needs to be changed.
            </p>
            <p>
                * Required field</p>
            <p>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
                <asp:Label ID="lblGuamMsg" runat="server" Text="" ForeColor="Red"></asp:Label></p>
            <h3>
                <asp:Label ID="Label2" runat="server" Text="Account Information"></asp:Label></h3>
            <asp:Panel ID="Panel1" runat="server" CssClass="acctsection">
                <table role="presentation">
                    <tr>
                        <td class="labelDefault">
                            User Name *
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblUser" Text="" />
                        </td>
                    </tr>
                    <tr>
                        <td class="labelDefault">
                            <asp:Label ID="lblPassword" runat="server" AssociatedControlID="txtPassword" Text="Current Password" />
                            *
                        </td>
                        <td>
                            <asp:TextBox ID="txtPassword" AutoCompleteType="None" class="addrfield" TextMode="Password"
                                runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="Panel2" runat="server" CssClass="acctsection">
                <table role="presentation">
                    <tr>
                        <td class="labelDefault">
                            <asp:Label ID="lblSecurityQuestion" runat="server" AssociatedControlID="ddlQuestions"
                                Text="Security Question" /> *
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlQuestions" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="labelDefault">
                            <asp:Label ID="lblAnswer" runat="server" AssociatedControlID="txtAnswer" Text="Answer" /> *
                        </td>
                        <td>
                            <asp:TextBox ID="txtAnswer" runat="server" MaxLength="64" OnTextChanged="txtAnswer_TextChanged"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            
             <div class="clearFix">   <asp:Button ID="btnUpdate" runat="server" class="btn goAction" OnClick="btnUpdate_Click"
                    Text="Update" /></div>
            <h3>
                <asp:Label ID="Label3" runat="server" Text="Change Password"></asp:Label></h3>
            <asp:Panel ID="Panel3" runat="server" CssClass="acctsection">
                <table role="presentation">
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
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <div class="clearFix">
                <asp:Button ID="btnChangePassword" runat="server" class="btn goAction" OnClick="btnChangePassword_Click"
                    Text="Change Password" />
                <asp:Button ID="btnCancel" runat="server" class="btn" Text="Cancel" Visible="false" /></div>
            
        </div>
        <div id="divChangePwdConfirmation" runat="server">
            <p>
                <asp:Label ID="lblConfirmation" runat="server" Text=""></asp:Label></p>
            <asp:Button ID="btnReturn" runat="server" class="btn returnPreviousPage" OnClick="btnReturn_Click"
                Text="Return to Previous Page" />
        </div>
    </div>
</asp:Content>
