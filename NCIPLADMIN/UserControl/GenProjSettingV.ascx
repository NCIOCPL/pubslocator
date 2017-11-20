<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GenProjSettingV.ascx.cs"
    Inherits="PubEntAdmin.UserControl.GenProjSettingV" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="PubEntAdmin" Namespace="PubEntAdmin.CustomControl" TagPrefix="cc2" %>
<%@ Register Src="TextCtrl_SpellCk.ascx" TagName="TextCtrl_SpellCk" TagPrefix="uc1" %>
<%@ Register TagPrefix="uc1" TagName="ListMultiSelect" Src="ListMultiSelect.ascx" %>
<cc1:CollapsiblePanelExtender ID="cpe" runat="Server" TargetControlID="ContentPanel"
    ExpandControlID="TitlePanel" CollapseControlID="TitlePanel" Collapsed="True"
    TextLabelID="lblExpColDescription" ExpandedText="Hide Details" CollapsedText="Show Details"
    ImageControlID="imgExpCol" ExpandedImage="~/Image/collapse_blue.jpg" CollapsedImage="~/Image/expand_blue.jpg"
    SuppressPostBack="true">
</cc1:CollapsiblePanelExtender>
<asp:Panel ID="TitlePanel" runat="server" CssClass="collapsePanelHeader">
    <asp:Image ID="imgExpCol" runat="server" ImageUrl="~/Image/expand_blue.jpg" AlternateText="Show Details" />
    <asp:Label ID="lblExpColDescription" runat="server">Show Details</asp:Label>
</asp:Panel>
<asp:Panel ID="ContentPanel" runat="server" CssClass="collapsePanel">
    <div class="editboxrow">
        <div class="editbox">
            <label id="lblDescription" for="<%= TextCtrl_SpellCkDescription.TextCtrl_SpellCheckClentID%>">Description</label>
            <uc1:TextCtrl_SpellCk ID="TextCtrl_SpellCkDescription" runat="server" CssClass="content"
                Width="400" TurnOffValidator="true" TextMode="MultiLine" MaxLength="500" ReadOnly="true" />
        </div>
        <div class="editbox">
            <label for="<%= TextCtrl_SpellCk_Keyword.TextCtrl_SpellCheckClentID%>">Keywords</label>
            <uc1:TextCtrl_SpellCk ID="TextCtrl_SpellCk_Keyword" runat="server" CssClass="content"
                Width="400" TurnOffValidator="true" TextMode="MultiLine" MaxLength="500" ReadOnly="true" />
        </div>
        <div class="editbox">
            <asp:Label ID="lblContCopyMat" runat="server" Text="Copyrighted Data Included"></asp:Label>
            <asp:Label ID="lblCopyRight" runat="server" Text=""></asp:Label>
        </div>
    </div>
    <div class="editboxrow">
        <div class="editbox">
            <asp:Label ID="lblThumbnail" runat="server" Text="Thumbnail File Name" AssociatedControlID="txtThumbnail"></asp:Label>
            <asp:TextBox ID="txtThumbnail" runat="server" ReadOnly="true"></asp:TextBox>
        </div>
        <div class="editbox">
            <asp:Label ID="lblLgImage" runat="server" Text="Large Image File Name" AssociatedControlID="txtLgImage"></asp:Label>
            <asp:TextBox ID="txtLgImage" runat="server" ReadOnly="true"></asp:TextBox>
        </div>
        <div class="editbox">
            <asp:Label ID="lblNumTotalPages" runat="server" Text="Number of Pages" AssociatedControlID="txtTotalPage"></asp:Label>
            <asp:TextBox ID="txtTotalPage" runat="server" ReadOnly="true"></asp:TextBox>
        </div>
    </div>
    <div class="editboxrow">
        <div style="display: none">
            <label for="<%= TextCtrl_SpellCk_Summary.TextCtrl_SpellCheckClentID%>" class="">Summary</label>
            <uc1:TextCtrl_SpellCk ID="TextCtrl_SpellCk_Summary" runat="server" CssClass="content"
                Width="400" TurnOffValidator="true" TextMode="MultiLine" MaxLength="500" ReadOnly="true" />
        </div>
        <div class="editbox">
            <label for="<%= TextCtrl_SpellCk_Dimension.TextCtrl_SpellCheckClentID%>" class="">Size</label>
            <uc1:TextCtrl_SpellCk ID="TextCtrl_SpellCk_Dimension" runat="server" CssClass="content"
                Width="400" TurnOffValidator="true" TextMode="MultiLine" MaxLength="25" ReadOnly="true" />
        </div>
        <div class="editbox">
            <label for="<%= TextCtrl_SpellCk_Color.TextCtrl_SpellCheckClentID%>" class="">Color</label>
            <uc1:TextCtrl_SpellCk ID="TextCtrl_SpellCk_Color" runat="server" CssClass="content"
                Width="400" TurnOffValidator="true" TextMode="MultiLine" MaxLength="15" ReadOnly="true" />
        </div>
        <div style="display: none">
            <label for="<%= TextCtrl_SpellCk_Other.TextCtrl_SpellCheckClentID%>" class="">Other</label>
            <uc1:TextCtrl_SpellCk ID="TextCtrl_SpellCk_Other" runat="server" CssClass="content"
                Width="400" TurnOffValidator="true" TextMode="MultiLine" MaxLength="50" ReadOnly="true" />
        </div>
        <div class="editbox">
            <label for="<%= TextCtrl_SpellCk_POSInst.TextCtrl_SpellCheckClentID%>" class="">POS Instructions</label>
            <uc1:TextCtrl_SpellCk ID="TextCtrl_SpellCk_POSInst" runat="server" CssClass="content"
                Width="400" TurnOffValidator="true" TextMode="MultiLine" MaxLength="300" ReadOnly="true" />
        </div>
    </div>
    <div class="editboxrow">
        <div class="editbox">
            <asp:Label ID="lblCancerType" runat="server" Text="Cancer Type" AssociatedControlID="listCancerType"></asp:Label>
            <uc1:ListMultiSelect ID="listCancerType" Rows="6" TurnOffValidator="true" DisplayDefault="false"
                SelectionMode="Multiple" runat="server" CssClass="MultiSelect" ReadOnly="true"></uc1:ListMultiSelect>
        </div>
        <div class="editbox">
            <asp:Label ID="lblAud" runat="server" Text="Audience" AssociatedControlID="listAudience"></asp:Label>
            <uc1:ListMultiSelect ID="listAudience" Rows="6" TurnOffValidator="true" DisplayDefault="false"
                SelectionMode="Multiple" runat="server" CssClass="MultiSelect" ReadOnly="true"></uc1:ListMultiSelect>
        </div>
        <div class="editbox">
            <asp:Label ID="lblLang" runat="server" Text="Language" AssociatedControlID="listLang"></asp:Label>
            <uc1:ListMultiSelect ID="listLang" Rows="6" TurnOffValidator="true" DisplayDefault="false"
                SelectionMode="Multiple" runat="server" CssClass="MultiSelect" ReadOnly="true"></uc1:ListMultiSelect>
        </div>
    </div>
    <div class="editboxrow">
        <div class="editbox">
            <asp:Label ID="lblPubFormat" runat="server" Text="Product Format" AssociatedControlID="listProdFormat"></asp:Label>
            <uc1:ListMultiSelect ID="listProdFormat" Rows="6" TurnOffValidator="true" DisplayDefault="false"
                SelectionMode="Multiple" runat="server" CssClass="MultiSelect" ReadOnly="true"></uc1:ListMultiSelect>
        </div>
        <div class="editbox">
            <asp:Label ID="lblRace" runat="server" Text="Race/Ethnic Group" AssociatedControlID="listRace"></asp:Label>
            <uc1:ListMultiSelect ID="listRace" Rows="6" TurnOffValidator="true" DisplayDefault="false"
                SelectionMode="Multiple" runat="server" CssClass="MultiSelect" ReadOnly="true"></uc1:ListMultiSelect>
        </div>
        <div class="editbox">
            <asp:Label ID="lblReadLevel" runat="server" Text="Reading Level" AssociatedControlID="listReadingLevel"></asp:Label>
            <uc1:ListMultiSelect ID="listReadingLevel" Rows="6" TurnOffValidator="true" DisplayDefault="false"
                SelectionMode="Single" runat="server" CssClass="MultiSelect" ReadOnly="true"></uc1:ListMultiSelect>
        </div>
    </div>
    <div class="editboxrow">
        <div class="editbox">
            <asp:Label ID="lblAwards" runat="server" Text="Awards" AssociatedControlID="listAward"></asp:Label>
            <uc1:ListMultiSelect ID="listAward" Rows="6" TurnOffValidator="true" DisplayDefault="false"
                SelectionMode="Multiple" runat="server" CssClass="MultiSelect" ReadOnly="true"></uc1:ListMultiSelect>
        </div>
        <div class="editbox">
            <asp:Label ID="lblNewUpdated" runat="server" Text="New & Updated List"></asp:Label>
            <div class="recordckbox">
                <cc2:CkboxListNewUpdated ID="CkboxListNewUpdated" runat="server" RepeatDirection="Horizontal" Enabled="false">
                </cc2:CkboxListNewUpdated>
            </div>
        </div>
        <div class="editbox">
            <asp:Label ID="lblExpiration" runat="server" Text="Expiration Date" AssociatedControlID="txtExpDate"></asp:Label>
            <asp:TextBox ID="txtExpDate" runat="server" ReadOnly="true"></asp:TextBox>
            <input id="hiddenLangSelected" type="hidden" runat="server" />
            <input id="hiddenCancerSelected" type="hidden" runat="server" />
            <input id="hiddenAudienceSelected" type="hidden" runat="server" />
            <input id="hiddenProdFormatSelected" type="hidden" runat="server" />
            <input id="hiddenSeriesSelected" type="hidden" runat="server" />
            <input id="hiddenRaceSelected" type="hidden" runat="server" />
            <input id="hiddenReadLevelSelected" type="hidden" runat="server" />
            <input id="hiddenAwardSelected" type="hidden" runat="server" />
        </div>
    </div>
</asp:Panel>

