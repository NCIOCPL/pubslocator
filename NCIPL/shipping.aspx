<%@ Page Title="Order free National Cancer Institute publications - NCI Publications Locator"
    Language="C#" MasterPageFile="~/pubmaster.Master" MaintainScrollPositionOnPostback="True"
    AutoEventWireup="true" CodeBehind="shipping.aspx.cs" Inherits="PubEnt.shipping" %>

<%@ Register Src="usercontrols/steps.ascx" TagName="steps" TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/pubmaster.master" %>
<%@ Register Src="usercontrols/searchbar_search.ascx" TagName="searchbar" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta name="description" content="Order free educational and support publications about cancer. Find resources for patients and their families, health care providers, and the public." />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <uc2:searchbar ID="searchbar" runat="server" />
    </div>
    <div class="indentwrap" id="ship">
        <a id="skiptocontent" tabindex="-1"></a>
        <div class="ctable clearFix">
            <h2>
                <asp:Label ID="Label1" runat="server" Text="Shipping &amp; Payment" CssClass="headPagehead"></asp:Label>
            </h2>
            <div class='steps show-for-large-up'>
                <uc1:steps ID="steps1" runat="server" />
            </div>
        </div>
        <p class="show-for-medium-up">
            <span class="textDefault"><a class="linkDefault" href="nciplhelp.aspx#intl" target="_blank">Ordering to an address outside the United States?</a> </span>
        </p>
        <p>
            * Required field
        </p>
        <div class="err">
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
            <asp:Label ID="lblXPO" runat="server"></asp:Label>
        </div>
        <div class="row small-collapse">
            <div class="columns large-6 addr">
                <asp:Panel ID="Panel1" runat="server" CssClass="">
                    <table>
                        <tr>
                            <td colspan="2">
                                <asp:Literal ID="lblShip" runat="server" Text="<h3>Shipping Address</h3>" Mode="PassThrough"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="ckbox"></td>
                        </tr>
                        <tr>
                            <td class="labelDefault">
                                <asp:Label runat="server" ID="lbltxtname" AssociatedControlID="txtname" Text="Name" />
                                *                         
                            </td>
                            <td>
                                <asp:TextBox ID="txtname" runat="server" CssClass="addrfield" OnTextChanged="TextBox4_TextChanged"
                                    MaxLength="30"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtname"
                                    ErrorMessage="Missing Name"> *</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="labelDefault">
                                <asp:Label runat="server" ID="lbltxtorg" AssociatedControlID="txtorg" Text="Organization" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtorg" runat="server" CssClass="addrfield" MaxLength="30" OnTextChanged="txtorg_TextChanged"></asp:TextBox>
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
                                    ErrorMessage="ZIP Code required" Display="Dynamic"> *</asp:RequiredFieldValidator>
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
                            </td>
                            <td>
                                <asp:TextBox ID="txtemail" runat="server" MaxLength="40" CssClass="addrfield"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtemail"
                                    ErrorMessage="Check Shipping Email Address" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"> *</asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="txtline show-for-medium-up">Your information will be kept private. Read our <a href="privacypolicy.aspx" target="_blank">privacy policy</a>.
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
            <div class="addr columns large-6">
                <table>
                    <tr>
                        <td colspan="2">
                            <asp:Literal ID="lblBill" runat="server" Text="<h3>Billing Address</h3>" Mode="PassThrough"></asp:Literal>
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
                    <table>
                        <tr>
                            <td class="labelDefault">
                                <asp:Label runat="server" ID="lbltxt2name" AssociatedControlID="txt2name" Text="Name" />
                                *
                            </td>
                            <td>
                                <asp:TextBox ID="txt2name" runat="server" CssClass="addrfield" MaxLength="30"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txt2name"
                                    ErrorMessage="Missing Billing Name" EnableClientScript="False"> *</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="labelDefault">
                                <asp:Label runat="server" ID="lbltxt2org" AssociatedControlID="txt2org" Text="Organization" />
                            </td>
                            <td>
                                <asp:TextBox ID="txt2org" runat="server" CssClass="addrfield" MaxLength="30"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="labelDefault">
                                <asp:Label runat="server" ID="lbltxt2addr1" AssociatedControlID="txt2addr1" Text="Address 1" />
                                *
                            </td>
                            <td>
                                <asp:TextBox ID="txt2addr1" runat="server" CssClass="addrfield" MaxLength="30"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txt2addr1"
                                    ErrorMessage="Missing Billing Address 1" EnableClientScript="False"> *</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="labelDefault">
                                <asp:Label runat="server" ID="lbltxt2addr2" AssociatedControlID="txt2addr2" Text="Address 2" />
                            </td>
                            <td>
                                <asp:TextBox ID="txt2addr2" runat="server" CssClass="addrfield" MaxLength="30"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="labelDefault">
                                <asp:Label runat="server" ID="lbltxt2zip5" AssociatedControlID="txt2zip5" Text="ZIP Code" />
                                *
                            </td>
                            <td>
                                <asp:TextBox ID="txt2zip5" runat="server" CssClass="addrfieldz5" AutoPostBack="True"
                                    MaxLength="5" OnTextChanged="BillZip_Changed"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ControlToValidate="txt2zip5"
                                    Display="Dynamic" ErrorMessage="Check the Billing ZIP Code" ValidationExpression="\d{5}"> *</asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="txt2zip5"
                                    ErrorMessage="Billing ZIP Code required" Display="Dynamic" EnableClientScript="False"> *</asp:RequiredFieldValidator><asp:Label
                                        runat="server" ID="lbltxt2zip4" AssociatedControlID="txt2zip4" Text="-" />
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
                            <td>
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
                            <td>
                                <asp:TextBox ID="txt2state" runat="server" CssClass="addrfieldst" MaxLength="2"></asp:TextBox>
                                &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txt2state"
                                    ErrorMessage="Missing Billing State" EnableClientScript="False"> *</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="labelDefault">
                                <asp:Label runat="server" ID="lbltxt2phone" AssociatedControlID="txt2phone" Text="Phone" />
                            </td>
                            <td>
                                <asp:TextBox ID="txt2phone" runat="server" CssClass="addrfield" MaxLength="20"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txt2phone"
                                    ErrorMessage="Billing Phone Format must be in (123) 456-7890" ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}"
                                    EnableClientScript="False"> *</asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="labelDefault">
                                <asp:Label runat="server" ID="lbltxt2email" AssociatedControlID="txt2email" Text="E-mail" />
                            </td>
                            <td>
                                <asp:TextBox ID="txt2email" runat="server" CssClass="addrfield" MaxLength="40"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txt2email"
                                    ErrorMessage="Check Billing Email Address" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                    EnableClientScript="False"> *</asp:RegularExpressionValidator>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
        </div>
        <asp:Panel ID="pnlPaymentInfo" runat="server" CssClass="row">
            <div class="payinfo columns large-6 large-offset-6 small-collapse medium-uncollapse">
                <h3>Shipping Information</h3>
                <p>
                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/nciplhelp.aspx#shipping" Target="_blank">When will items arrive?</asp:HyperLink>
                </p>
                <p>
                    If you would like a shipping estimate, please 
                        <asp:HyperLink ID="ContactLink" runat="server" NavigateUrl="~/nciplhelp.aspx#contact" Target="_blank">contact GPO directly</asp:HyperLink>.
                    <br />
                </p>
                <table class="">
                    <tr>
                        <td class="labelDefault">
                            <asp:Label AssociatedControlID="drpDelivery" runat="server">Shipping Method *</asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="drpDelivery" runat="server" OnSelectedIndexChanged="DeliveryChanged" CssClass="dropdownlist">
                                <asp:ListItem Value="">[Select Method]</asp:ListItem>
                                <asp:ListItem Value="STANDARD">Standard Ground</asp:ListItem>
                                <asp:ListItem Value="OVERNIGHT">Standard Overnight</asp:ListItem>
                                <asp:ListItem Value="2DAY">2 Day</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ControlToValidate="drpDelivery"
                                ErrorMessage="Shipping Method is required"> *</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="labelDefault">
                            <asp:Label AssociatedControlID="txtAccountNumber" runat="server">Shipping Number *</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAccountNumber" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ControlToValidate="txtAccountNumber"
                                ErrorMessage="Account Number is required"> *</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="FedExExpressionValidator10" runat="server" ControlToValidate="txtAccountNumber"
                                EnableClientScript="False" Enabled="False" ErrorMessage="Shipping Number is not the correct format for FedEx
"
                                ValidationExpression="\s*([0-9]{9})\s*"> *</asp:RegularExpressionValidator>
                            <asp:RegularExpressionValidator ID="UPSExpressionValidator10" runat="server" ControlToValidate="txtAccountNumber"
                                EnableClientScript="False" Enabled="False" ErrorMessage="Shipping Number is not the correct format for UPS
"
                                ValidationExpression="(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{6})$"> *</asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="labelDefault snconf">
                            <asp:Label AssociatedControlID="txtAccountNumber2" runat="server">Confirm Shipping Number *</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAccountNumber2" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ControlToValidate="txtAccountNumber2"
                                ErrorMessage="Confirmation of Account Number is required"> *</asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtAccountNumber"
                                ControlToValidate="txtAccountNumber2" ErrorMessage="Account numbers do not match"> *</asp:CompareValidator>
                        </td>
                    </tr>
                    <tr class="hide">
                        <td class="labelDefault">
                            <asp:Label ID="lblShipping" runat="server" Text="Estimated Cost"></asp:Label>

                        </td>
                        <td class="cost">
                            <asp:Label ID="lblCost" runat="server"></asp:Label>
                            <asp:Button ID="Button3" runat="server" CausesValidation="False" CssClass="btn goAction"
                                OnClick="Button3_Click" Text="Recalculate" />
                        </td>
                    </tr>
                </table>
            </div>
        </asp:Panel>
        <table class='ctable'>
            <tr id="rowsource" runat="server">
                <td>
                    <div class='ctable clearFix'>
                        <asp:Button ID="Button1" runat="server" CssClass="btn goAction" OnClick="Button1_Click"
                            Text="Verify Order &gt;&gt;" OnClientClick="javascript:_gaq.push(['_trackEvent','Ordering','Submit','Verify Order']);NCIAnalytics.PubsLinkTrack('Verify Order', this);" />
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CssClass="linkCheckout"
                            OnClick="Click_BacktoCart">Back to Shopping Cart</asp:LinkButton>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
