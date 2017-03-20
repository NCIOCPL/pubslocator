<%@ Page Title="" Language="C#" MasterPageFile="~/pubmaster.Master" AutoEventWireup="true"
    CodeBehind="shipping.aspx.cs" Inherits="NCIPLex.shipping" %>

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
        <p>
            * Required field
        </p>
        <div class="err">
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
        </div>
        <div class="clearFix">
            <asp:Panel ID="Panel1" runat="server" CssClass="addr shipaddr">
                <table>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblShip" runat="server" Text="<h3>Shipping Address</h3>"></asp:Label>
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
                            <asp:Label runat="server" ID="lbltxtzip5" AssociatedControlID="txtzip5" Text="ZIP Code *" />
                            <asp:Label runat="server" ID="lblIntltxtzip" AssociatedControlID="txtIntlZip" Text="Postal *"
                                Visible="false" /><!--Intl control-->
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
                            <!--Begin Intl zip controls-->
                            <asp:TextBox ID="txtIntlZip" runat="server" MaxLength="50" Visible="false" CssClass="addrfield"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldIntlZip" runat="server" ErrorMessage="Postal Code required"
                                ControlToValidate="txtIntlZip" Display="Dynamic" Visible="false"> *</asp:RequiredFieldValidator>
                            <!--End Intl zip controls-->
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
                            <asp:Label runat="server" ID="lbltxtstate" AssociatedControlID="txtstate" Text="State *" />
                            <!--Begin Intl controls-->
                            <asp:Label runat="server" ID="lbltxtIntlState" AssociatedControlID="txtIntlState"
                                Text="Province" Visible="false" />
                            <!--end Intl controls-->
                        </td>
                        <td>
                            <asp:TextBox ID="txtstate" runat="server" CssClass="addrfieldst" MaxLength="2"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtstate"
                                ErrorMessage="Missing State">*</asp:RequiredFieldValidator>
                            <!--Begin Intl controls-->
                            <asp:TextBox ID="txtIntlState" runat="server" MaxLength="50" Visible="false" CssClass="addrfield"></asp:TextBox>
                            <!-- End Intl controls-->
                        </td>
                    </tr>
                    <!-- Intl country row -->
                    <tr runat="server" id="rowIntlCountry">
                        <td class="labelDefault">
                            <div id="divlblCountry" runat="server" visible="false">
                                <asp:Label runat="server" ID="lbltxtIntlCountry" AssociatedControlID="ddlIntlCountry"
                                    Text="Country *" Visible="false" />
                            </div>
                        </td>
                        <td>
                            <div id="divtxtCountry" runat="server" visible="false">
                                <asp:DropDownList ID="ddlIntlCountry" runat="server" Visible="false">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldIntlCountry" runat="server" ControlToValidate="ddlIntlCountry"
                                    ErrorMessage="Missing Country" Visible="false"> *</asp:RequiredFieldValidator>
                            </div>
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
                            Your e-mail address will only be used for shipping confirmation.
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlBIllingInfo" runat="server">
                <table>
                    <tr>
                        <td colspan="3">
                            <asp:Label ID="lblBill" runat="server" Text="Billing Address" CssClass="headSub"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" class="ckbox">
                            <asp:CheckBox ID="chkSameaddress" runat="server" Text="Use my shipping address" AutoPostBack="True"
                                OnCheckedChanged="chkSameaddress_CC" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="labelDefault">
                            <asp:Label runat="server" ID="lbltxt2name" AssociatedControlID="txt2name" Text="Name" />
                            *
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="LEFT" nowrap="true">
                            <asp:TextBox ID="txt2name" runat="server" Width="250px" MaxLength="30"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txt2name"
                                ErrorMessage="Missing Billing Name" EnableClientScript="False">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="labelDefault">
                            <asp:Label runat="server" ID="lbltxt2org" AssociatedControlID="txt2org" Text="Organization" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="LEFT" nowrap="true">
                            <asp:TextBox ID="txt2org" runat="server" Width="250px" MaxLength="30"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="labelDefault">
                            <asp:Label runat="server" ID="lbltxt2addr1" AssociatedControlID="txt2addr1" Text="Address 1" />
                            *
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="LEFT" nowrap="true">
                            <asp:TextBox ID="txt2addr1" runat="server" Width="250px" MaxLength="30"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txt2addr1"
                                ErrorMessage="Missing Billing Address 1" EnableClientScript="False">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="labelDefault">
                            <asp:Label runat="server" ID="lbltxt2addr2" AssociatedControlID="txt2addr2" Text="Address 2" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="LEFT" nowrap="true">
                            <asp:TextBox ID="txt2addr2" runat="server" Width="250px" MaxLength="30"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="labelDefault">
                            <asp:Label runat="server" ID="lbltxt2zip5" AssociatedControlID="txt2zip5" Text="ZIP Code" />
                            *
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="LEFT" nowrap="true">
                            <asp:TextBox ID="txt2zip5" runat="server" Width="75px" AutoPostBack="True" MaxLength="5"
                                OnTextChanged="BillZip_Changed"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ControlToValidate="txt2zip5"
                                ErrorMessage="Check the Billing ZIP Code" ValidationExpression="\d{5}">*</asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="txt2zip5"
                                ErrorMessage="Billing ZIP Code required" EnableClientScript="False">*</asp:RequiredFieldValidator>
                            <asp:Label runat="server" ID="lbltxt2zip4" AssociatedControlID="txt2zip4" Text="-" />&nbsp;
                            <asp:TextBox ID="txt2zip4" runat="server" Width="50px" MaxLength="4"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator9" runat="server" ControlToValidate="txt2zip4"
                                ErrorMessage="Check Billing ZIP4" ValidationExpression="\d{4}">*</asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="labelDefault">
                            <asp:Label runat="server" ID="lbltxt2city" AssociatedControlID="txt2city" Text="City" />
                            *
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="LEFT" nowrap="true">
                            <asp:TextBox ID="txt2city" runat="server" Width="250px" MaxLength="30" OnTextChanged="BillZip_Changed"></asp:TextBox>
                            <asp:DropDownList ID="drpcity2" runat="server" Width="250px" Visible="False">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txt2city"
                                ErrorMessage="Missing Billing City" EnableClientScript="False">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="labelDefault">
                            <asp:Label runat="server" ID="lbltxt2state" AssociatedControlID="txt2state" Text="State" />
                            *
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="LEFT" nowrap="true">
                            <asp:TextBox ID="txt2state" runat="server" Width="50px" MaxLength="2"></asp:TextBox>
                            &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txt2state"
                                ErrorMessage="Missing Billing State" EnableClientScript="False">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="labelDefault">
                            <asp:Label runat="server" ID="lbltxt2phone" AssociatedControlID="txt2phone" Text="Phone" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="LEFT" nowrap="true">
                            <asp:TextBox ID="txt2phone" runat="server" Width="250px" MaxLength="20"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txt2phone"
                                ErrorMessage="Billing Phone Format must be in (123) 456-7890" ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}"
                                EnableClientScript="False">*</asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="labelDefault">
                            <asp:Label runat="server" ID="lbltxt2email" AssociatedControlID="txt2email" Text="E-mail" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="LEFT" nowrap="true">
                            <asp:TextBox ID="txt2email" runat="server" Width="250px" MaxLength="40"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txt2email"
                                ErrorMessage="Check Billing Email Address" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                EnableClientScript="False">*</asp:RegularExpressionValidator>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Label ID="AVMessage" runat="server"></asp:Label>
            <asp:Panel ID="pnlPaymentInfo" runat="server">
                <table style="width: 100%;">
                    <tr>
                        <td class="headSub">
                            Payment Information
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="left">
                            <img src="images\cclogo.gif" alt="Credit Card Logo" />
                    </tr>
                    <tr>
                        <td class="labelDefault">
                            Credit Card *<br />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:DropDownList ID="drpcard" runat="server">
                                <asp:ListItem Value="M">Mastercard</asp:ListItem>
                                <asp:ListItem Value="V">Visa</asp:ListItem>
                                <asp:ListItem Value="A">American Express</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="labelDefault">
                            <asp:Label runat="server" ID="lbltxtccnum" AssociatedControlID="txtccnum" Text="Credit Card Number" />
                            *
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtccnum" runat="server" MaxLength="16"></asp:TextBox>
                            &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtccnum"
                                ErrorMessage="Missing Credit Card Number">*</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" ControlToValidate="txtccnum"
                                ErrorMessage="Check the Credit Card Number" ValidationExpression="\d{15,16}">*</asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="labelDefault">
                            <asp:Label runat="server" ID="lbltxtcvv2" AssociatedControlID="txtcvv2" Text="Validation Code" />
                            *<br />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtcvv2" runat="server" Width="50px"></asp:TextBox>
                            &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="txtcvv2"
                                ErrorMessage="Missing Validation Code">*</asp:RequiredFieldValidator>
                            &nbsp;<a class="info cvv" href="#">What&#39;s This? <span><strong>Validation Code</strong>
                                <p>
                                    The validation code is <strong>a 3-digit or 4-digit security code</strong> printed
                                    on your credit card or on the signature strip on the back.</p>
                                <ul>
                                    <li>The validation code is not part of your card number.<br>
                                        <br>
                                    </li>
                                    <li>Entering the validation code verifies that you have the physical card when you make
                                        a purchase.</li>
                                </ul>
                            </span></a>
                        </td>
                    </tr>
                    <tr>
                        <td class="labelDefault">
                            Expiration Date *<br />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:DropDownList ID="drpmonth" runat="server">
                                <asp:ListItem Value="01">January</asp:ListItem>
                                <asp:ListItem Value="02">February</asp:ListItem>
                                <asp:ListItem Value="03">March</asp:ListItem>
                                <asp:ListItem Value="04">April</asp:ListItem>
                                <asp:ListItem Value="05">May</asp:ListItem>
                                <asp:ListItem Value="06">June</asp:ListItem>
                                <asp:ListItem Value="07">July</asp:ListItem>
                                <asp:ListItem Value="08">August</asp:ListItem>
                                <asp:ListItem Value="09">September</asp:ListItem>
                                <asp:ListItem Value="10">October</asp:ListItem>
                                <asp:ListItem Value="11">November</asp:ListItem>
                                <asp:ListItem Value="12">December</asp:ListItem>
                            </asp:DropDownList>
                            &nbsp;&nbsp;
                            <asp:DropDownList ID="drpyr" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="labelDefault">
                            Cost
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lblCost" runat="server" Style="font-weight: 700"></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
        <table class='ctable'>
            <tr id="rowsource" runat="server">
                <td>
                    <div class='ctable clearFix'>
                        <asp:Button ID="Button1" runat="server" CssClass="btn goAction" OnClick="Button1_Click"
                            Text="Verify Order &gt;&gt;" />
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CssClass="linkCheckout"
                            PostBackUrl="cart.aspx">Back to Shopping Cart</asp:LinkButton>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
