<%@ Page Title="" Language="C#" MasterPageFile="~/pubmaster.Master" AutoEventWireup="true"
    CodeBehind="shipping.aspx.cs" Inherits="PubEnt.shipping" MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="usercontrols/steps.ascx" TagName="steps" TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/pubmaster.master" %>
<%@ Register Src="usercontrols/searchbar_search.ascx" TagName="searchbar" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
            <div class='steps'>
                <uc1:steps ID="steps1" runat="server" />
            </div>
        </div>
        <!--NCIPL_CC-->
        <asp:Panel ID="pnlSource" runat="server" CssClass="clearFix">
            <asp:CheckBox ID="chkBxSplitOrder" runat="server" Text="Caller will receive materials from CIS office under separate cover"
                TextAlign="Right" CssClass="shiplabel" />
             <div class="shipsection large">
                <asp:Label ID="lblCustomerType" runat="server" Text="Type of Customer *" AssociatedControlID="drpCustomerType" CssClass="shiplabel"></asp:Label>
                <asp:DropDownList ID="drpCustomerType" runat="server">
                    <asp:ListItem Value="" Text="[Please Select]"></asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="drpCustomerType"
                    ErrorMessage="Please select a value for Type of Customer" Display="Dynamic" Text="*"
                    InitialValue=""></asp:RequiredFieldValidator>
            </div>
            <div class="shipsection">
                <asp:Label ID="lblOrderSource" runat="server" Text="Order Source *" AssociatedControlID="drpOrderMedia" CssClass="shiplabel"></asp:Label>
                <asp:DropDownList ID="drpOrderMedia" runat="server">
                    <asp:ListItem Value="" Text="[Please Select]"></asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="drpOrderMedia"
                    ErrorMessage="Please select a value for Order Source" Display="Dynamic" Text="*"
                    InitialValue=""></asp:RequiredFieldValidator>
            </div>
            <div class="shipsection">
                <asp:Label ID="lblOrderComment" runat="server" Text="Comment" AssociatedControlID="txtOrderComment" CssClass="shiplabel"></asp:Label>
                <asp:TextBox ID="txtOrderComment" runat="server" MaxLength="500" Rows="3" 
                    TextMode="MultiLine" CssClass="shiptextfield"></asp:TextBox>
            </div>
        </asp:Panel>
        <p>
            * Required field
        </p>
        <div class="err">
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
            <asp:Label ID="lblXPO" runat="server"></asp:Label>
        </div>
        <div class="clearFix">
            <asp:Panel ID="Panel1" runat="server" CssClass="addr shipaddr">
                <table>
                    <tr>
                        <td colspan="2">
                            <asp:Literal ID="lblShip" runat="server" Text="<h3>Shipping Address</h3>" Mode="PassThrough"></asp:Literal>
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
                        <td colspan="2" class="txtline">
                            <asp:Label ID="lblRNT" runat="server" Text="RNT" AssociatedControlID="txtRNT"></asp:Label>&nbsp;&nbsp;&nbsp;
                            <asp:TextBox ID="txtRNT" runat="server" MaxLength="20" Width="190px"></asp:TextBox>
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
            <div class="addr billaddr">
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
                                *
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
                                *
                            </td>
                            <td>
                                <asp:TextBox ID="txt2email" runat="server" CssClass="addrfield" MaxLength="40"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txt2email"
                                    ErrorMessage="Check Billing Email Address" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                    EnableClientScript="False"> *</asp:RegularExpressionValidator>
                                <!--Validate Billig Phone Number or Email-->
                                <asp:CustomValidator ID="custValidatorPhoneEmail" runat="server" ErrorMessage="Please collect Billing phone or e-mail."
                                    EnableClientScript="false" Display="None" OnServerValidate="ValBillPhoneorEmail"
                                    Text="">
                                </asp:CustomValidator>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
        </div>
        <asp:Panel ID="pnlPaymentInfo" runat="server" CssClass="ctable clearFix">
            <div class="payinfo">
                <table class="">
                    <tr>
                        <th class="headSub">
                            Shipping Information
                        </th>
                        <th>
                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/nciplhelp.aspx#shipping" Target="_blank">When will items arrive?</asp:HyperLink>
                        </th>
                    </tr>
                    <tr>
                        <td class="">
                        </td>
                        <td>
                            If you would like a shipping estimate, please 
                            <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/nciplhelp.aspx#contact" Target="_blank">contact GPO directly</asp:HyperLink>.
                        </td>
                    </tr>
                    <tr>
                        <td class="labelDefault">
                            <asp:Label AssociatedControlID="drpDelivery" runat="server">Shipping Method *</asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="drpDelivery" runat="server" OnSelectedIndexChanged="DeliveryChanged">
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
" ValidationExpression="\s*([0-9]{9})\s*"> *</asp:RegularExpressionValidator>
                            <asp:RegularExpressionValidator ID="UPSExpressionValidator10" runat="server" ControlToValidate="txtAccountNumber"
                                EnableClientScript="False" Enabled="False" ErrorMessage="Shipping Number is not the correct format for UPS
" ValidationExpression="(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{6})$"> *</asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="labelDefault">
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
                        <td class="labelDefault costtxt">
                            <div class="clearFix">
                                <asp:Label ID="lblShipping" runat="server" Text="Estimated Cost"></asp:Label></div>
                        </td>
                        <td class="cost">
                            <div class="clearFix">
                                <asp:Label ID="lblCost" runat="server"></asp:Label>
                                <asp:Button ID="Button3" runat="server" CausesValidation="False" CssClass="btn goAction"
                                    OnClick="Button3_Click" Text="Recalculate" /></div>
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
                            Text="Verify Order &gt;&gt;" />
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CssClass="linkCheckout"
                            OnClick="Click_BacktoCart">Back to Shopping Cart</asp:LinkButton>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
