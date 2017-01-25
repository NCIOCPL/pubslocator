<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NCIPLTabEditInfo.ascx.cs"
    Inherits="PubEntAdmin.UserControl.NCIPLTabEditInfo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="TextCtrl_SpellCk.ascx" TagName="TextCtrl_SpellCk" TagPrefix="uc1" %>
<%@ Register TagPrefix="uc1" TagName="ListMultiSelect" Src="ListMultiSelect.ascx" %>
<%@ Register Assembly="PubEntAdmin" Namespace="PubEntAdmin.CustomControl" TagPrefix="cc2" %>
<style type="text/css">
    div.fileinputs { position: relative; text-align: right; }
    div.fakefile { position: absolute; top: 0px; left: 0px; z-index: 1; }
    input.inputfile { position: relative; text-align: right; -moz-opacity: 0; filter: alpha(opacity: 0); opacity: 0; z-index: 2; }
</style>
<div class="ckboxrow">
    <div class="editbox">
        <asp:Label ID="lblDisStatus" runat="server" Text="Display Status" CssClass=""></asp:Label>
        <cc2:CkboxListDisplayStatus ID="ckboxListDisplayStatusNCIPL" runat="server" RepeatDirection="Horizontal"
            CssClass="recordckbox ckbox3">
        </cc2:CkboxListDisplayStatus>
        <asp:CustomValidator ID="CustValckboxListDisplayStatusNCIPL" runat="server" ErrorMessage=""
            ClientValidationFunction="NCIPLDisplayStatusVal" Display="None"></asp:CustomValidator>
    </div>
</div>
<div class="editboxrow">
    <div class="editbox">
        <asp:Label ID="Label1" runat="server" Text="Rank" CssClass=""
            AssociatedControlID="txtRank"></asp:Label>
        <asp:TextBox ID="txtRank" runat="server" MaxLength="4"></asp:TextBox>
        <cc1:FilteredTextBoxExtender ID="filttxtRank" runat="server" TargetControlID="txtRank"
            FilterType="Numbers">
        </cc1:FilteredTextBoxExtender>
        <asp:Label ID="lblErRank" runat="server" CssClass="errorText" Font-Size="X-Small"></asp:Label>
    </div>
</div>
<div class="editboxrow">
    <div class="editbox">
        <asp:Label ID="lblMaxQty" runat="server" Text="Max Qty./NCIPL" CssClass=""
            AssociatedControlID="txtMaxQtyNCIPL"></asp:Label>
        <asp:TextBox ID="txtMaxQtyNCIPL" runat="server" MaxLength="8"></asp:TextBox>
        <cc1:FilteredTextBoxExtender ID="ftxteMaxQtyNCIPL" runat="server" TargetControlID="txtMaxQtyNCIPL"
            FilterType="Numbers">
        </cc1:FilteredTextBoxExtender>
    </div>
    <div class="editbox">
        <asp:Label ID="lblMaxQtyIntl" runat="server" Text="Max Qty./International" CssClass=""
            AssociatedControlID="txtMaxQtyIntl"></asp:Label>
        <asp:TextBox ID="txtMaxQtyIntl" runat="server" MaxLength="8"></asp:TextBox>
        <cc1:FilteredTextBoxExtender ID="ftbeMaxQtyIntl" runat="server" TargetControlID="txtMaxQtyIntl"
            FilterType="Numbers">
        </cc1:FilteredTextBoxExtender>
    </div>
</div>
<div class="editboxrow">
    <div class="editbox">
        <asp:Label ID="lblInclEveryOrder" runat="server" Text="Include in Every Order" CssClass=""></asp:Label>
        <cc2:RdbtnListYesNo ID="rdbtnListYesNoEveryOrder" runat="server" RepeatDirection="Horizontal" CssClass="recordckbox rdio">
        </cc2:RdbtnListYesNo>
        <span class="Hint">[Only if orderable and free]</span>
    </div>
    <div class="editbox">
        <asp:Label ID="lblShowInSrchRes" runat="server" Text="Show in Search Results" CssClass=""></asp:Label>
        <cc2:RdbtnListYesNo ID="rdbtnListYesNoShowInSearchRes" runat="server" RepeatDirection="Horizontal" CssClass="recordckbox rdio">
        </cc2:RdbtnListYesNo>
        <span class="Hint">[Applies to all Display Status]</span>
    </div>
</div>
<div class="editboxrow">
    <div class="editbox">
        <asp:Label ID="lblHomeFeature" runat="server" Text="NCIPL Homepage Featured" CssClass=""></asp:Label>
        <cc2:RdbtnListYesNo ID="rdbtnListYesNoFeatured" runat="server" RepeatDirection="Horizontal" CssClass="recordckbox rdio">
        </cc2:RdbtnListYesNo>
    </div>
    <div class="editbox">
        <asp:Label ID="Label2" runat="server" Text="NCIPL Featured Image" CssClass=""></asp:Label>
        <div class="fileinputs">
            <label for="<%=realFeaturedinput.ClientID %>" style="display: none">
                Real Featured Input
            </label>
            <input id="realFeaturedinput" type="file" class="inputfile" onchange="copyfeaturedimage()"
                runat="server" style="width: 5px" />
            <div class="fakefile">
                <label for="<%=fakeFeaturedinput.ClientID %>" style="display: none">
                    Fake Featured Input
                </label>
                <input id="fakeFeaturedinput" runat="server" style="width: 225px; position: relative; top: -7px;" readonly="readonly" />
                <img id="imgUpload" runat="server" src="~/Image/Browse.gif" alt="Browse" />
            </div>
        </div>
    </div>
</div>
<div class="editboxrow">
    <div class="editbox">
        <asp:Label ID="lblStacks" runat="server" Text="Image Stack" CssClass="" AssociatedControlID="listStacks"></asp:Label>
        <asp:Label ID="lblErrStack" runat="server" CssClass="errorText" Font-Size="X-Small"></asp:Label>
        <uc1:ListMultiSelect ID="listStacks" runat="server" Rows="4" TurnOffValidator="true"
            DisplayDefault="true" SelectionMode="Multiple" CssClass="MultiSelect"></uc1:ListMultiSelect>
    </div>
</div>
<div class="editboxrow">
    <div class="editbox">
        <asp:Label ID="lblSubj" runat="server" Text="Subject" CssClass="" AssociatedControlID="listSubject"></asp:Label>
        <uc1:ListMultiSelect ID="listSubject" Rows="6" TurnOffValidator="true" DisplayDefault="true"
            SelectionMode="Multiple" runat="server" CssClass="MultiSelect"></uc1:ListMultiSelect>
    </div>
    <div class="editbox">
        <asp:Label ID="lblCollections" runat="server" Text="Collections" CssClass="" AssociatedControlID="listCollections"></asp:Label>
        <uc1:ListMultiSelect ID="listCollections" Rows="6" TurnOffValidator="true" DisplayDefault="true"
            SelectionMode="Multiple" runat="server" CssClass="MultiSelect"></uc1:ListMultiSelect>
    </div>
</div>
