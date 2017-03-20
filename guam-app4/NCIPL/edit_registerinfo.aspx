<%@ Page Title="Order free National Cancer Institute publications - NCI Publications Locator"
    Language="C#" MasterPageFile="~/pubmaster.Master" AutoEventWireup="true" CodeBehind="edit_registerinfo.aspx.cs"
    Inherits="PubEnt.edit_registerinfo" %>

<%@ MasterType VirtualPath="~/pubmaster.master" %>
<%@ Register Src="usercontrols/searchbar_search.ascx" TagName="searchbar" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta name="description" content="Order free educational and support publications about cancer. Find resources for patients and their families, health care providers, and the public." />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <uc2:searchbar ID="searchbar" runat="server" />
    </div>
    <div class="indentwrap">
        <a id="skiptocontent" tabindex="-1"></a>
        <div id="divUserReg" class="" runat="server">
            <h2>
                <asp:Label ID="Label1" runat="server" Text="Edit Shipping and Billing Addresses"
                    CssClass="headPagehead"></asp:Label></h2>
            <p>
                * Required field</p>
            <div class="err">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
                <p>
                    <asp:Label ID="lblGuamMsg" runat="server" Text=""></asp:Label></p>
            </div>
            <div class="row small-collapse">
                <asp:Panel ID="Panel1" runat="server" CssClass="addr columns large-6">
                    <table role="presentation">
                        <tr>
                            <td colspan="2">
                                <h3>
                                    <asp:Label ID="lblShip" runat="server" Text="Shipping Address" CssClass=""></asp:Label>
                                </h3>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="ckbox">
                            </td>
                        </tr>
                        <tr>
                            <td class="labelDefault">
                                <asp:Label runat="server" ID="lbltxtname" AssociatedControlID="txtname" Text="Name" />
                                *
                            </td>
                            <td >
                                <asp:TextBox ID="txtname" runat="server" CssClass="addrfield" OnTextChanged="TextBox4_TextChanged"
                                    MaxLength="30"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtname"
                                    ErrorMessage="Missing Name"> *</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="labelDefault">
                                <asp:Label runat="server" ID="lbltxtorg" AssociatedControlID="txtorg" Text="Organization" />
                                *
                            </td>
                            <td>
                                <asp:TextBox ID="txtorg" runat="server" CssClass="addrfield" MaxLength="30" OnTextChanged="txtorg_TextChanged"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtorg"
                                    ErrorMessage="Missing Organization"> *</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="labelDefault">
                                <asp:Label runat="server" ID="lbltxtaddr1" AssociatedControlID="txtaddr1" Text="Address 1" />
                                *
                            </td>
                            <td>
                                <asp:TextBox ID="txtaddr1" runat="server" CssClass="addrfield" MaxLength="30"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtaddr1"
                                    ErrorMessage="Missing Address 1"> *</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="labelDefault">
                                <asp:Label runat="server" ID="lbltxtaddr2" AssociatedControlID="txtaddr2" Text="Address 2" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtaddr2" runat="server" CssClass="addrfield" MaxLength="30" OnTextChanged="TextBox7_TextChanged"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="labelDefault">
                                <asp:Label runat="server" ID="lbltxtzip5" AssociatedControlID="txtzip5" Text="ZIP Code" />
                                *
                            </td>
                            <td>
                                <asp:TextBox ID="txtzip5" runat="server" OnTextChanged="Zip_Changed" CssClass="addrfieldz5"
                                    AutoPostBack="True" MaxLength="5"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="txtzip5"
                                    Display="Dynamic" ErrorMessage="Check ZIP Code" ValidationExpression="\d{5}"> *</asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txtzip5"
                                    ErrorMessage="ZIP Code required" Display="Dynamic"> *
                                </asp:RequiredFieldValidator>
                                <asp:Label ID="lbltxtzip4" runat="server" AssociatedControlID="txtzip4" Text="-" />
                                <asp:TextBox ID="txtzip4" runat="server" MaxLength="4" CssClass="addrfieldz4"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="txtzip4"
                                    ErrorMessage="Check ZIP4 " ValidationExpression="\d{4}"> *</asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="labelDefault">
                                <asp:Label runat="server" ID="lbltxtcity" AssociatedControlID="txtcity" Text="City" />
                                *
                            </td>
                            <td>
                                <asp:TextBox ID="txtcity" runat="server" CssClass="addrfield" OnTextChanged="TextBox2_TextChanged"></asp:TextBox>
                                <asp:DropDownList ID="drpcity" runat="server" Visible="False" CssClass="addrfield">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtcity"
                                    ErrorMessage="Missing City"> *</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="labelDefault">
                                <asp:Label runat="server" ID="lbltxtstate" AssociatedControlID="txtstate" Text="State" />
                                *
                            </td>
                            <td>
                                <asp:TextBox ID="txtstate" runat="server" CssClass="addrfieldst" MaxLength="2"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtstate"
                                    ErrorMessage="Missing State"> *</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="labelDefault">
                                <asp:Label runat="server" ID="lbltxtphone" AssociatedControlID="txtphone" Text="Phone" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtphone" runat="server" CssClass="addrfield" MaxLength="20" OnTextChanged="TextBox8_TextChanged"
                                    ToolTip="Phone Format must be in (123) 456-7890"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtphone"
                                    ErrorMessage="Phone Format must be in (123) 456-7890" ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}"> *</asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="labelDefault">
                                <asp:Label ID="lbltxtemail" runat="server" AssociatedControlID="txtemail" Text="E-mail" />
                                *
                            </td>
                            <td>
                                <asp:TextBox ID="txtemail" runat="server" MaxLength="40" CssClass="addrfield"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="txtemail"
                                    ErrorMessage="Missing E-mail"> *</asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtemail"
                                    ErrorMessage="Check Shipping Email Address" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"> *</asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="txtline">
                                Your information will be kept private. Read our <a href="privacypolicy.aspx" target="_blank">
                                    privacy policy</a>.
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <div class="addr columns large-6">
                    <table role="presentation">
                        <tr>
                            <td colspan="2">
                                <h3>
                                    <asp:Label ID="lblBill" runat="server" Text="Billing Address"></asp:Label>
                                </h3>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="ckbox">
                                <asp:CheckBox ID="chkSameaddress" runat="server" Text="Use my shipping address" AutoPostBack="True"
                                    OnCheckedChanged="chkSameaddress_CC" />
                            </td>
                        </tr>
                    </table>
                    <asp:Panel ID="pnlBIllingInfo" runat="server">
                        <table role="presentation">
                            <tr>
                                <td class="labelDefault">
                                    <asp:Label runat="server" ID="lbltxt2name" AssociatedControlID="txt2name" Text="Name" />
                                    *
                                </td>
                                <td >
                                    <asp:TextBox ID="txt2name" runat="server" CssClass="addrfield" MaxLength="30"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txt2name"
                                        ErrorMessage="Missing Billing Name" EnableClientScript="False"> *</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="labelDefault">
                                    <asp:Label runat="server" ID="lbltxt2org" AssociatedControlID="txt2org" Text="Organization" />
                                    *
                                </td>
                                <td >
                                    <asp:TextBox ID="txt2org" runat="server" CssClass="addrfield" MaxLength="30"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="txt2org"
                                        ErrorMessage="Missing Billing Organization" EnableClientScript="False"> *</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="labelDefault">
                                    <asp:Label runat="server" ID="lbltxt2addr1" AssociatedControlID="txt2addr1" Text="Address 1" />
                                    *
                                </td>
                                <td >
                                    <asp:TextBox ID="txt2addr1" runat="server" CssClass="addrfield" MaxLength="30"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txt2addr1"
                                        ErrorMessage="Missing Billing Address 1" EnableClientScript="False"> *</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="labelDefault">
                                    <asp:Label runat="server" ID="lbltxt2addr2" AssociatedControlID="txt2addr2" Text="Address 2" />
                                </td>
                                <td >
                                    <asp:TextBox ID="txt2addr2" runat="server" CssClass="addrfield" MaxLength="30"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="labelDefault">
                                    <asp:Label runat="server" ID="lbltxt2zip5" AssociatedControlID="txt2zip5" Text="ZIP Code" />
                                    *
                                </td>
                                <td >
                                    <asp:TextBox ID="txt2zip5" runat="server" CssClass="addrfieldz5" AutoPostBack="True" MaxLength="5"
                                        OnTextChanged="BillZip_Changed"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ControlToValidate="txt2zip5" Display="Dynamic"
                                        ErrorMessage="Check the Billing ZIP Code" ValidationExpression="\d{5}"> *</asp:RegularExpressionValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="txt2zip5" Display="Dynamic"
                                        ErrorMessage="Billing ZIP Code required" EnableClientScript="False"> *</asp:RequiredFieldValidator>
                                    <asp:Label runat="server" ID="lbltxt2zip4" AssociatedControlID="txt2zip4" Text="-" />
                                    <asp:TextBox ID="txt2zip4" runat="server" CssClass="addrfieldz4" MaxLength="4"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator9" runat="server" ControlToValidate="txt2zip4"
                                        ErrorMessage="Check Billing ZIP4" ValidationExpression="\d{4}"> *</asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="labelDefault">
                                    <asp:Label runat="server" ID="lbltxt2city" AssociatedControlID="txt2city" Text="City" />
                                    *
                                </td>
                                <td >
                                    <asp:TextBox ID="txt2city" runat="server" CssClass="addrfield" MaxLength="30" OnTextChanged="BillZip_Changed"></asp:TextBox>
                                    <asp:DropDownList ID="drpcity2" runat="server" CssClass="addrfield" Visible="False">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txt2city"
                                        ErrorMessage="Missing Billing City" EnableClientScript="False"> *</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="labelDefault">
                                    <asp:Label runat="server" ID="lbltxt2state" AssociatedControlID="txt2state" Text="State" />
                                    *
                                </td>
                                <td >
                                    <asp:TextBox ID="txt2state" runat="server" CssClass="addrfieldst" MaxLength="2"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txt2state"
                                        ErrorMessage="Missing Billing State" EnableClientScript="False"> *</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="labelDefault">
                                    <asp:Label runat="server" ID="lbltxt2phone" AssociatedControlID="txt2phone" Text="Phone" />
                                </td>
                                <td >
                                    <asp:TextBox ID="txt2phone" runat="server" CssClass="addrfield" MaxLength="20"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txt2phone"
                                        ErrorMessage="Billing Phone Format must be in (123) 456-7890" ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}"
                                        EnableClientScript="False"> *</asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="labelDefault">
                                    <asp:Label runat="server" ID="lbltxt2email" AssociatedControlID="txt2email" Text="E-mail" />
                                    *
                                </td>
                                <td >
                                    <asp:TextBox ID="txt2email" runat="server" CssClass="addrfield" MaxLength="40"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ControlToValidate="txt2email"
                                        ErrorMessage="Missing Billing E-mail" EnableClientScript="False"> *</asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txt2email"
                                        ErrorMessage="Check Billing Email Address" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                        EnableClientScript="False"> *</asp:RegularExpressionValidator>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
            </div>
            <div class="clearFix">
                <asp:Button ID="btnSubmit" runat="server" CssClass="btn goAction" OnClick="btnSubmit_Click"
                    Text="Submit" />
                <asp:Button ID="btnChangePassword" runat="server" CssClass="btn verifyOrder" OnClick="btnChangePassword_Click"
                    Text="Change Password" />
                <asp:Button ID="btnCancel" runat="server" CssClass="btn verifyOrder" Text="Cancel"
                    OnClick="btnCancel_Click" Visible="false" /></div>
        </div>
        <div id="divUserRegConfirmation" class='regconf' runat="server">
            <h2>
                <asp:Label ID="Label3" runat="server" Text="Edit Shipping and Billing Addresses"
                    CssClass="headPagehead"></asp:Label></h2>
            <p>
                Your shipping and billing addresses have been updated successfully.
            </p>
            <p>
                <asp:Label ID="lblXPO" runat="server" ForeColor="Red"></asp:Label></p>
            <asp:Button ID="btnReturn" runat="server" CssClass="btn returnPreviousPage" OnClick="btnReturn_Click"
                Text="Return to Previous Page" />
        </div>
    </div>
</asp:Content>
