<%@ Page Title="" Language="C#" MasterPageFile="~/pubmaster.Master" AutoEventWireup="true" CodeBehind="forgotpwd.aspx.cs" Inherits="PubEnt.forgotpwd" %>
<%@ MasterType VirtualPath="~/pubmaster.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="indentwrap" id="forgotpw">
        <a id="skiptocontent" tabindex="-1"></a>
        <h2>
            <asp:Label ID="Label1" runat="server" Text="Reset Password" CssClass="headPagehead"></asp:Label>
        </h2>
        <div id="divChangePwd" class="ctable clearFix" runat="server">
            <p>
                * Required field</p>
            <p>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
                <asp:Label ID="lblGuamMsg" runat="server" Text="" ForeColor="Red"></asp:Label>
            </p>
            <div id="divUserName" runat="server">
                <asp:Panel ID="Panel2" runat="server">
                    <table role="presentation">
                        <tr>
                            <td class="labelDefault">
                                <asp:Label runat="server" ID="lblUserName" AssociatedControlID="txtUserName" Text="User Name" /> *
                            </td>
                            <td>
                                <asp:TextBox ID="txtUserName" runat="server" CssClass="addrfield" OnTextChanged="txtUserName_TextChanged"
                                    MaxLength="256"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtUserName"
                                    ErrorMessage="Missing User Name"> *</asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" ControlToValidate="txtUserName"
                                    ErrorMessage="Check E-mail Address" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">*</asp:RegularExpressionValidator>
                                <br />
                                <em>(User Name is your E-mail address)</em>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
            <div id="divSecurityQuestion" runat="server">
                <asp:Panel ID="Panel1" runat="server">
                    <table role="presentation">
                        <tr>
                            <td class="labelDefault">
                                User Name *
                            </td>
                            <td >
                                <asp:Label runat="server" ID="lblUser" Text="" />
                            </td>
                        </tr>
                        <tr>
                            <td class="labelDefault">
                                Security Question *
                            </td>
                            <td >
                                <asp:Label runat="server" ID="lblSecurityQuestion" Text="" />
                            </td>
                        </tr>
                        <tr>
                            <td class="labelDefault">
                                <asp:Label ID="lblAnswer" runat="server" AssociatedControlID="txtAnswer" Text="Answer" /> *
                            </td>
                            <td >
                                <asp:TextBox ID="txtAnswer" runat="server" MaxLength="64" OnTextChanged="txtAnswer_TextChanged"
                                    CssClass="addrfield"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtAnswer"
                                    ErrorMessage="Missing Answer to Security Question"> *</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
            <div>
                <asp:Button ID="btnResetPassword" runat="server" CssClass="btn goAction"  OnClick="btnResetPassword_Click"
                    Text="Reset Password" />
                <asp:Button ID="btnCancel" runat="server" CssClass="btn verifyOrder" Text="Cancel" Visible="false" />
            </div>
        </div>
        <div id="divConfirmation" runat="server">
            <p>
                Your password has been reset. The new password will be sent to your E-mail address.
            </p>
            <asp:Button ID="btnReturn" runat="server" CssClass="btn returnPreviousPage" OnClick="btnReturn_Click"
                Text="Return to Previous Page" />
        </div>
    </div>
    <asp:HiddenField ID="HidSecurityQuestionID" runat="server" Value="" />
</asp:Content>
