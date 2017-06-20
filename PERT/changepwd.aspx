<%@ Page Title="" Language="C#" MasterPageFile="~/pubmaster.Master" AutoEventWireup="true" CodeBehind="changepwd.aspx.cs" Inherits="PubEnt.changepwd" %>

<%@ MasterType VirtualPath="~/pubmaster.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="indentwrap">
        <a name="skiptocontent"></a>

        <table style="width: 100%;" border="0">
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Edit Account Information and Change Password"
                        CssClass="headPagehead"></asp:Label>
                </td>
                <td>&nbsp;</td>
                <td align="right">&nbsp;</td>
            </tr>
            <!--tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr-->
        </table>

        <div id="divChangePwd" runat="server">
            <p>
                Due to new password security requirements, the password needs to be changed.
            </p>
            <p>
                <table cols="80%,2%,18%" border="0" width="100%">
                    <tr>
                        <td colspan="3" align="left">* Required field
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="left">
                            <asp:Label ID="lblGuamMsg" runat="server" Text="" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3"></td>
                    </tr>
                    <tr>
                        <td>
                            <b>
                                <asp:Label ID="Label2" runat="server" Text="Account Information" CssClass="headSub"></asp:Label>
                            </b></td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr valign="top">
                        <td valign="top">&nbsp;</td>
                        <td>&nbsp;</td>
                        <td valign="top" align="center">&nbsp;</td>
                    </tr>
                    <tr valign="top">
                        <td valign="top">
                            <asp:Panel ID="Panel1" runat="server" Width="80%">
                                <table style="width: 100%;" width="100%">
                                    <tr>
                                        <td align="right" class="labelDefault" width="28%">User Name*</td>
                                        <td width="2%">&nbsp;</td>
                                        <td nowrap="true" width="70%" valign="top">
                                            <asp:Label runat="server" ID="lblUser" Text="" /></td>
                                    </tr>
                                    <tr>

                                        <td align="right" class="labelDefault" width="28%">
                                            <asp:Label ID="lblPassword" runat="server" AssociatedControlID="txtPassword" Text="Current Password" />
                                            *</td>
                                        <td width="2%">&nbsp;</td>
                                        <td nowrap="true" valign="top" width="70%">
                                            <asp:TextBox ID="txtPassword" AutoCompleteType="None" Width="250px" TextMode="Password" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                        <td></td>
                        <td></td>
                    </tr>
                </table>
            </p>
            <p>
                <table cols="80%,2%,18%" border="0" width="100%">
                    <tr valign="top">
                        <td valign="top">&nbsp;</td>
                        <td>&nbsp;</td>
                        <td valign="top" align="center">&nbsp;</td>
                    </tr>
                    <tr valign="top">
                        <td valign="top">
                            <asp:Panel ID="Panel2" runat="server" Width="80%">
                                <table style="width: 100%;" width="100%">
                                    <tr>
                                        <td align="right" class="labelDefault" width="28%">
                                            <asp:Label ID="lblSecurityQuestion" runat="server" AssociatedControlID="ddlQuestions"
                                                Text="Security Question" />*</td>
                                        <td width="2%">&nbsp;</td>
                                        <td nowrap="true" width="70%">
                                            <asp:DropDownList ID="ddlQuestions" runat="server"></asp:DropDownList>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" class="labelDefault" width="28%">
                                            <asp:Label ID="lblAnswer" runat="server" AssociatedControlID="txtAnswer" Text="Answer" />*</td>
                                        <td width="2%">&nbsp;</td>
                                        <td nowrap="true" width="70%">
                                            <asp:TextBox ID="txtAnswer" runat="server" MaxLength="64"
                                                OnTextChanged="txtAnswer_TextChanged" Width="250px"></asp:TextBox>
                                        </td>
                                    </tr>

                                </table>
                            </asp:Panel>
                        </td>
                        <td></td>
                        <td></td>
                    </tr>
                </table>
            </p>
            <p align="left">
                <table style="width: 100%;">
                    <tr>
                        <td colspan="3" align="left">
                            <asp:Button ID="btnUpdate" runat="server" class="btn verifyOrder" OnClick="btnUpdate_Click"
                                Text="Update" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                </table>
            </p>
            <p>
                <table cols="80%,2%,18%" border="0" width="100%">
                    <tr>
                        <td>
                            <b>
                                <asp:Label ID="Label3" runat="server" Text="Change Password" CssClass="headSub"></asp:Label>
                            </b></td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>

                    <tr valign="top">
                        <td valign="top">&nbsp;</td>
                        <td>&nbsp;</td>
                        <td valign="top" align="center">&nbsp;</td>
                    </tr>
                    <tr valign="top">
                        <td valign="top">
                            <asp:Panel ID="Panel3" runat="server" Width="80%">
                                <table style="width: 100%;" width="100%">
                                    <tr>
                                        <td class="labelDefault"></td>
                                        <td colspan="2">User password must include a minimum of eight(8) characters and contain three of the following four categories: 1. Uppercase characters (A through Z) 2. Lowercase characters (a through z) 3. Numbers (0 through 9) 4. Non-alphanumeric characters (Examples: !, $, #, %)
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" class="labelDefault" width="28%">
                                            <asp:Label ID="lblNewPassword" runat="server" AssociatedControlID="txtNewPassword" Text="New Password" />
                                            *</td>
                                        <td width="2%">&nbsp;</td>
                                        <td nowrap="true" valign="top" width="70%">
                                            <asp:TextBox ID="txtNewPassword" AutoCompleteType="None" Width="250px" TextMode="Password" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" class="labelDefault" width="28%">
                                            <asp:Label ID="lblConfirmPassword" runat="server" AssociatedControlID="txtConfirmPassword" Text="Confirm New Password" />
                                            *</td>
                                        <td width="2%">&nbsp;</td>
                                        <td nowrap="true" valign="top" width="70%">
                                            <asp:TextBox ID="txtConfirmPassword" AutoCompleteType="None" Width="250px" TextMode="Password" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                        <td></td>
                        <td></td>
                    </tr>
                </table>
            </p>

            <p align="left">
                <table style="width: 100%;">
                    <tr>
                        <td colspan="3" align="left">
                            <asp:Button ID="btnChangePassword" runat="server" class="btn verifyOrder" OnClick="btnChangePassword_Click"
                                Text="Change Password" />
                            &nbsp;&nbsp;
                        <asp:Button ID="btnCancel" runat="server" class="btn verifyOrder" Text="Cancel" Visible="false" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                </table>
            </p>
            <p>&nbsp;</p>
            <p></p>
        </div>


        <div id="divChangePwdConfirmation" runat="server">

            <p>&nbsp;</p>
            <table style="width: 50%;">
                <tr>
                    <td colspan="3">
                        <asp:Label ID="lblConfirmation" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>

            <p>
                <table style="width: 50%;">
                    <tr>
                        <td colspan="3" align="left">
                            <asp:Button ID="btnReturn" runat="server" class="btn returnPreviousPage" OnClick="btnReturn_Click"
                                Text="Return to Previous Page" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                </table>
            </p>
            <p>&nbsp;</p>
            <p></p>
        </div>
    </div>
</asp:Content>
