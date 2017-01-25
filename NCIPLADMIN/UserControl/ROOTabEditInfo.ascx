<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ROOTabEditInfo.ascx.cs"
    Inherits="PubEntAdmin.UserControl.ROOTabEditInfo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="TextCtrl_SpellCk.ascx" TagName="TextCtrl_SpellCk" TagPrefix="uc1" %>
<%@ Register TagPrefix="uc1" TagName="ListMultiSelect" Src="ListMultiSelect.ascx" %>
<%@ Register Assembly="PubEntAdmin" Namespace="PubEntAdmin.CustomControl" TagPrefix="cc2" %>
<div class="ckboxrow">
    <div class="editbox">
        <asp:Label ID="lblDisStatus" runat="server" Text="Display Status" CssClass=""></asp:Label>
        <cc2:CkboxListDisplayStatus ID="ckboxListDisplayStatusROO" runat="server"
            RepeatDirection="Horizontal" CssClass="recordckbox ckbox3">
        </cc2:CkboxListDisplayStatus>
    </div>
</div>
<div class="editboxrow">
    <div class="editbox">
        <asp:Label ID="lblMaxQty" runat="server" Text="Max Qty./NCIPLcc" CssClass=""></asp:Label>
        <asp:TextBox ID="txtMaxQtyROO" runat="server" MaxLength="8"></asp:TextBox>
        <cc1:FilteredTextBoxExtender ID="ftxteMaxQtyROO" runat="server" TargetControlID="txtMaxQtyROO"
            FilterType="Numbers">
        </cc1:FilteredTextBoxExtender>
    </div>
    <div class="editbox">
        <asp:Label ID="lblInclEveryOrder" runat="server" Text="Include in Every Order" CssClass=""></asp:Label>
        <cc2:RdbtnListYesNo ID="rdbtnListYesNoEveryOrder" runat="server" CssClass="recordckbox rdio" RepeatDirection="Horizontal">
        </cc2:RdbtnListYesNo>
    </div>
</div>
<div class="editboxrow">
    <div class="editbox">
        <asp:Label ID="lblShowInSrchRes" runat="server" Text="Show in Search Results" CssClass=""></asp:Label>
        <cc2:RdbtnListYesNo ID="rdbtnListYesNoShowInSearchRes" runat="server" CssClass="recordckbox rdio" RepeatDirection="Horizontal">
        </cc2:RdbtnListYesNo>
    </div>
    <div class="editbox">
        <asp:Label ID="lblROOKit" runat="server" Text="NCIPLcc Kit" CssClass=""></asp:Label>
        <cc2:RdbtnListYesNo ID="rdbtnListYesNoROOKit" runat="server"  CssClass="recordckbox rdio" RepeatDirection="Horizontal">
        </cc2:RdbtnListYesNo>
    </div>
</div>
<div class="editboxrow">
    <div class="editbox">
        <asp:Label ID="lblSubj" runat="server" Text="Subject" CssClass=""></asp:Label>
        <uc1:ListMultiSelect ID="listSubject" Rows="6" TurnOffValidator="true" DisplayDefault="true"
            SelectionMode="Multiple" runat="server" CssClass="MultiSelect"></uc1:ListMultiSelect>
    </div>
    <div class="editbox">
        <asp:Label ID="lblCollections" runat="server" Text="Collections" CssClass=""></asp:Label>
        <uc1:ListMultiSelect ID="listCollections" Rows="6" TurnOffValidator="true" DisplayDefault="true"
            SelectionMode="Multiple" runat="server" CssClass="MultiSelect"></uc1:ListMultiSelect>
    </div>
</div>

