<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GenProjSettingE.ascx.cs"
    Inherits="PubEntAdmin.UserControl.GenProjSettingE" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="PubEntAdmin" Namespace="PubEntAdmin.CustomControl" TagPrefix="cc2" %>
<%@ Register Src="TextCtrl_SpellCk.ascx" TagName="TextCtrl_SpellCk" TagPrefix="uc1" %>
<%@ Register TagPrefix="uc1" TagName="ListMultiSelect" Src="ListMultiSelect.ascx" %>
<style type="text/css">
    div.fileinputs { position: relative; text-align: right; }
    div.fakefile { position: absolute; top: 0px; left: 0px; z-index: 1; }
    input.inputfile { position: relative; text-align: right; -moz-opacity: 0; filter: alpha(opacity: 0); opacity: 0; z-index: 2; }
</style>
<script type="text/javascript">
    function iFrame_OnUploadComplete() {
        //$find('<%=this.cpe.BehaviorID %>').togglePanel(); 
    }
    function copy() {
        document.getElementById('<%=this.fakeinput.ClientID %>').value =
        document.getElementById('<%=this.realinput.ClientID %>').value;
    }
    function Lgcopy() {
        document.getElementById('<%=this.fakeLginput.ClientID %>').value =
        document.getElementById('<%=this.realLginput.ClientID %>').value;
    }
</script>
<cc1:CollapsiblePanelExtender ID="cpe" runat="Server" TargetControlID="ContentPanel"
    BehaviorID="yourCollapsedPanel" ExpandControlID="TitlePanel" CollapseControlID="TitlePanel"
    Collapsed="True" TextLabelID="lblExpColDescription" ExpandedText="Hide Details"
    CollapsedText="Show Details" ImageControlID="imgExpCol" ExpandedImage="~/Image/collapse_blue.jpg"
    CollapsedImage="~/Image/expand_blue.jpg" SuppressPostBack="true">
</cc1:CollapsiblePanelExtender>
<asp:Panel ID="TitlePanel" runat="server" CssClass="collapsePanelHeader">
    <asp:Image ID="imgExpCol" runat="server" ImageUrl="~/Image/expand_blue.jpg" AlternateText="Show Details" />
    <asp:Label ID="lblExpColDescription" runat="server">Show Details</asp:Label>
</asp:Panel>
<asp:Panel ID="ContentPanel" runat="server" CssClass="collapsePanel" Width="100%">
    <div class="editboxrow">
        <div class="editbox">
            <label id="lblDescription" for="<%= TextCtrl_SpellCkDescription.TextCtrl_SpellCheckClentID%>" class="">Description</label>
            <uc1:TextCtrl_SpellCk ID="TextCtrl_SpellCkDescription" runat="server" CssClass="content"
                Width="400" TurnOffValidator="true" TextMode="MultiLine" MaxLength="2000" onkeypress="return handleEnter(this, event);"
                TabIndex="10" />
        </div>
        <div class="editbox">
            <label for="<%= TextCtrl_SpellCk_Keyword.TextCtrl_SpellCheckClentID%>" class="">Keywords</label>
            <uc1:TextCtrl_SpellCk ID="TextCtrl_SpellCk_Keyword" runat="server" CssClass="content"
                Width="400" TurnOffValidator="true" TextMode="MultiLine" MaxLength="1000" onkeypress="return handleEnter(this, event);"
                TabIndex="12" />
        </div>
        <div class="editbox">
            <asp:Label ID="lblContCopyMat" runat="server" Text="Copyrighted Data Included"
                CssClass=""></asp:Label><div class="recordckbox rdio">
                    <cc2:RdbtnListYesNo ID="rdbtnListCopyRightMaterial" runat="server" RepeatDirection="Horizontal"
                        onkeypress="return handleEnter(this, event);" TabIndex="11">
                    </cc2:RdbtnListYesNo>
                </div>
        </div>
    </div>
    <div class="editboxrow">
        <div class="editbox">
            <asp:Label ID="lblThumbnail" runat="server" Text="Thumbnail File Name" AssociatedControlID="realinput" CssClass=""></asp:Label>
            <div class="fileinputs">
                <input id="realinput" type="file" class="inputfile" onchange="copy()" runat="server"
                    style="width: 5px" />
                <div class="fakefile">
                    <label for="<%=fakeinput.ClientID %>" style="display: none">Fake File</label>
                    <input id="fakeinput" runat="server" style="width: 225px" readonly="true" />
                    <img id="imgUpload" runat="server" src="~/Image/Browse.gif" alt="Browse" />
                </div>
            </div>
        </div>
        <div class="editbox">
            <asp:Label ID="Label2" runat="server" Text="Large Image File Name" AssociatedControlID="realLginput" CssClass=""></asp:Label>
            <div class="fileinputs">
                <input id="realLginput" type="file" class="inputfile" onchange="Lgcopy()" runat="server"
                    style="width: 5px" />
                <div class="fakefile">
                    <label for="<%=fakeLginput.ClientID %>" style="display: none">Fake Login Input</label>
                    <input id="fakeLginput" runat="server" style="width: 225px" readonly="true" />
                    <img id="img1" runat="server" src="~/Image/Browse.gif" alt="Browse" />
                </div>
            </div>
        </div>
        <div style="display: none">
            <label for="<%= TextCtrl_SpellCk_Summary.TextCtrl_SpellCheckClentID%>" class="">Summary</label>
            <uc1:TextCtrl_SpellCk ID="TextCtrl_SpellCk_Summary" runat="server" CssClass="content"
                Width="400" TurnOffValidator="true" TextMode="MultiLine" MaxLength="300" onkeypress="return handleEnter(this, event);"
                TabIndex="13" />
        </div>
        <div class="editbox">
            <asp:Label ID="lblNumTotalPages" runat="server" Text="Number of Pages" AssociatedControlID="txtTotalPage" CssClass=""></asp:Label>
            <asp:TextBox ID="txtTotalPage" runat="server" MaxLength="8" onkeypress="return handleEnter(this, event);"
                TabIndex="14"></asp:TextBox>
            <cc1:FilteredTextBoxExtender ID="ftbeTotalPage" runat="server" TargetControlID="txtTotalPage"
                ValidChars="0123456789">
            </cc1:FilteredTextBoxExtender>
            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtenderTotalPage" runat="server"
                TargetControlID="txtTotalPage" WatermarkCssClass="" WatermarkText="N/A">
            </cc1:TextBoxWatermarkExtender>
        </div>
    </div>
    <div class="editboxrow">
        <div class="editbox">
            <label for="<%= TextCtrl_SpellCk_Dimension.TextCtrl_SpellCheckClentID%>" class="">Size</label>
            <uc1:TextCtrl_SpellCk ID="TextCtrl_SpellCk_Dimension" runat="server" CssClass="content"
                Width="400" TurnOffValidator="true" TextMode="MultiLine" MaxLength="25" onkeypress="return handleEnter(this, event);"
                TabIndex="15" />
        </div>
        <div class="editbox">
            <label for="<%= TextCtrl_SpellCk_Color.TextCtrl_SpellCheckClentID%>" class="">Color</label>
            <uc1:TextCtrl_SpellCk ID="TextCtrl_SpellCk_Color" runat="server" CssClass="content"
                Width="400" TurnOffValidator="true" TextMode="MultiLine" MaxLength="20" onkeypress="return handleEnter(this, event);"
                TabIndex="16" />

        </div>
        <div style="display: none">
            <label for="<%= TextCtrl_SpellCk_Other.TextCtrl_SpellCheckClentID%>" class="">Other</label>
            <uc1:TextCtrl_SpellCk ID="TextCtrl_SpellCk_Other" runat="server" CssClass="content"
                Width="400" TurnOffValidator="true" TextMode="MultiLine" MaxLength="50" onkeypress="return handleEnter(this, event);"
                TabIndex="17" />
        </div>
        <div class="editbox">
            <label for="<%= TextCtrl_SpellCk_POSInst.TextCtrl_SpellCheckClentID%>" class="">POS Instructions</label>
            <uc1:TextCtrl_SpellCk ID="TextCtrl_SpellCk_POSInst" runat="server" CssClass="content"
                Width="400" TurnOffValidator="true" TextMode="MultiLine" MaxLength="300" onkeypress="return handleEnter(this, event);"
                TabIndex="18" />
        </div>
    </div>
    <div class="editboxrow">
        <div class="editbox">
            <asp:Label ID="lblCancerType" runat="server" Text="Cancer Type" AssociatedControlID="listCancerType" CssClass=""></asp:Label>
            <uc1:ListMultiSelect ID="listCancerType" Rows="6" TurnOffValidator="true" DisplayDefault="false"
                SelectionMode="Multiple" runat="server" CssClass="MultiSelect" onkeypress="return handleEnter(this, event);"
                TabIndex="19"></uc1:ListMultiSelect>
        </div>
        <div class="editbox">
            <asp:Label ID="lblAud" runat="server" Text="Audience" AssociatedControlID="listAudience" CssClass=""></asp:Label>
            <uc1:ListMultiSelect ID="listAudience" Rows="6" TurnOffValidator="true" DisplayDefault="false"
                SelectionMode="Multiple" runat="server" CssClass="MultiSelect" onkeypress="return handleEnter(this, event);"
                TabIndex="20"></uc1:ListMultiSelect>
        </div>
        <div class="editbox">
            <asp:Label ID="lblLang" runat="server" Text="Language" AssociatedControlID="listLang" CssClass=""></asp:Label>
            <uc1:ListMultiSelect ID="listLang" Rows="6" TurnOffValidator="true" DisplayDefault="false"
                SelectionMode="Multiple" runat="server" CssClass="MultiSelect" onkeypress="return handleEnter(this, event);"
                TabIndex="21"></uc1:ListMultiSelect>
        </div>
    </div>
    <div class="editboxrow">
        <div class="editbox">
            <asp:Label ID="lblPubFormat" runat="server" Text="Product Format" AssociatedControlID="listProdFormat" CssClass=""></asp:Label>
            <uc1:ListMultiSelect ID="listProdFormat" Rows="6" TurnOffValidator="true" DisplayDefault="false"
                SelectionMode="Multiple" runat="server" CssClass="MultiSelect" onkeypress="return handleEnter(this, event);"
                TabIndex="22"></uc1:ListMultiSelect>
        </div>
        <div class="editbox">
            <asp:Label ID="lblRace" runat="server" Text="Race/Ethnic Group" AssociatedControlID="listRace" CssClass=""></asp:Label>
            <uc1:ListMultiSelect ID="listRace" Rows="6" TurnOffValidator="true" DisplayDefault="false"
                SelectionMode="Multiple" runat="server" CssClass="MultiSelect" onkeypress="return handleEnter(this, event);"
                TabIndex="24"></uc1:ListMultiSelect>
        </div>
        <div class="editbox">
            <asp:Label ID="lblReadLevel" runat="server" Text="Reading Level" AssociatedControlID="listReadingLevel" CssClass=""></asp:Label>
            <uc1:ListMultiSelect ID="listReadingLevel" Rows="6" TurnOffValidator="true" DisplayDefault="false"
                SelectionMode="Single" runat="server" CssClass="MultiSelect" onkeypress="return handleEnter(this, event);"
                TabIndex="25"></uc1:ListMultiSelect>
        </div>
    </div>
    <div class="editboxrow">
        <div class="editbox">
            <asp:Label ID="lblAwards" runat="server" Text="Awards" AssociatedControlID="listAward" CssClass=""></asp:Label>
            <uc1:ListMultiSelect ID="listAward" Rows="6" TurnOffValidator="true" DisplayDefault="false"
                SelectionMode="Multiple" runat="server" CssClass="MultiSelect" onkeypress="return handleEnter(this, event);"
                TabIndex="26"></uc1:ListMultiSelect>
            <%--<asp:Label ID="Label5" runat="server" Text="" Height="5px"></asp:Label><br />--%>
        </div>
        <div class="editbox">
            <asp:Label ID="lblNewUpdated" runat="server" Text="New & Updated List" CssClass=""></asp:Label>
            <div class="recordckbox">
                <cc2:CkboxListNewUpdated ID="CkboxListNewUpdated" runat="server" RepeatDirection="Horizontal">
                </cc2:CkboxListNewUpdated>
            </div>
        </div>
        <div class="editbox">
            <asp:Label ID="lblExpiration" runat="server" Text="Expiration Date" AssociatedControlID="txtExpDate" CssClass=""></asp:Label>
            <asp:TextBox ID="txtExpDate" runat="server"></asp:TextBox>
            <cc1:CalendarExtender ID="CalExtExpDate" runat="server" TargetControlID="txtExpDate"
                CssClass="MyCalendar" Format="MM/dd/yyyy">
            </cc1:CalendarExtender>
            <cc1:MaskedEditExtender ID="MaskedEditExtExpDate" runat="server" TargetControlID="txtExpDate"
                Mask="99/99/9999" MaskType="Date">
            </cc1:MaskedEditExtender>
            <asp:CustomValidator ID="CustValExpDate" runat="server" ControlToValidate="txtExpDate"
                Display="None" ClientValidationFunction="MyOwnExpDateVal"></asp:CustomValidator>
            <asp:CustomValidator ID="CustValNewUpdatedList" runat="server"
                Display="None" ClientValidationFunction="MyOwnNewUpdatedListVal"></asp:CustomValidator>
        </div>
    </div>
</asp:Panel>
