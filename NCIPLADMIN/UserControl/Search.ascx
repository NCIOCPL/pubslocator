<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Search.ascx.cs" Inherits="PubEntAdmin.UserControl.Search" %>
<%@ Register TagPrefix="uc1" TagName="ListMultiSelect" Src="ListMultiSelect.ascx" %>
<%@ Register Src="LiveIntSel.ascx" TagName="LiveIntSel" TagPrefix="uc2" %>
<%@ Register Src="LiveNewUpStatus.ascx" TagName="LiveNewUpStatus" TagPrefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="PubEntAdmin" Namespace="PubEntAdmin.CustomControl" TagPrefix="cc2" %>
<asp:Button ID="btnSearch2" runat="server" Text="Search" OnClick="btnSearch_Click" CssClass="btnGo" />
<div class="">
    <div class="ckbox">
        <asp:Label ID="lblLivInt" runat="server" Text="Live Interfaces" CssClass=""></asp:Label>
        <uc2:LiveIntSel ID="LiveIntSelSearch" runat="server" RepeatDirection="Horizontal" />
    </div>
    <div class="ckbox">
        <asp:Label ID="lblLivInt0" runat="server" Text="New/Updated Status" CssClass=""></asp:Label>
        <uc3:LiveNewUpStatus ID="NewUpStatus" runat="server" RepeatDirection="Horizontal" />
    </div>
</div>
<asp:Label ID="errormsgnu" runat="server" Text="" Visible="false" CssClass="errorText"></asp:Label>
<div class="">
    <div class="fullbox">
        <label for="<%= txtKeyword.ClientID%>">Search</label>
        <asp:TextBox ID="txtKeyword" runat="server" Width="90%" MaxLength="400"
            placeholder="Enter a word (or part of a word). Search targets all free text fields.">
        </asp:TextBox>
    </div>
</div>
<div class="homesearchrow">
    <div class="homesearchbox date">
        <span class="datespacer">&nbsp;</span>
        <label for="<%= txtNIHNum1.ClientID%>">NIH Publication #</label>
        <asp:TextBox ID="txtNIHNum1" runat="server" MaxLength="2" Columns="4"></asp:TextBox>&nbsp;
        <label for="<%= txtNIHNum2.ClientID%>" class="nihnum">-</label>&nbsp;
  <asp:TextBox ID="txtNIHNum2" runat="server" MaxLength="5" Columns="8"></asp:TextBox>
        <asp:CustomValidator
            ID="cusValNIHNum" runat="server" ErrorMessage="Incorrect NIH # format."
            OnServerValidate="NIH_ServerValidate"></asp:CustomValidator>
    </div>
    <div class="homesearchbox date">
        <span class="datespacer">Date Record Created </span>
        <label for="<%= txtSrRecordStartDate.ClientID%>">From</label>
        <asp:ScriptManager ID="ScriptManager_PubRcod" EnableScriptGlobalization="false" runat="Server"
            LoadScriptsBeforeUI="true" EnableHistory="True">
        </asp:ScriptManager>
        <asp:TextBox ID="txtSrRecordStartDate" runat="server" MaxLength="10"></asp:TextBox>
        <cc1:CalendarExtender ID="CalSrRecordStartDate" runat="server" TargetControlID="txtSrRecordStartDate"
            CssClass="MyCalendar" Format="MM/dd/yyyy">
        </cc1:CalendarExtender>
        <cc1:MaskedEditExtender ID="MaskedEditExtEndDate" runat="server" TargetControlID="txtSrRecordStartDate"
            Mask="99/99/9999" MaskType="Date">
        </cc1:MaskedEditExtender>
        <cc1:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlToValidate="txtSrRecordStartDate"
            ControlExtender="MaskedEditEnDate" Display="Dynamic" TooltipMessage="" IsValidEmpty="true"
            EmptyValueMessage="A Date is Required" InvalidValueMessage="This date is invalid">
        </cc1:MaskedEditValidator>
        <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="require."
            Display="none" EnableClientScript="true">
        </asp:CustomValidator>
    </div>
    <div class="homesearchbox date">
        <span class="datespacer">&nbsp;</span>
        <label for="<%= txtEnRecordEndDate.ClientID%>">To</label>
        <asp:TextBox ID="txtEnRecordEndDate" runat="server" MaxLength="10"></asp:TextBox>
        <cc1:CalendarExtender ID="CalEnRecordEndDate" runat="server" TargetControlID="txtEnRecordEndDate"
            CssClass="MyCalendar" Format="MM/dd/yyyy">
        </cc1:CalendarExtender>
        <cc1:MaskedEditExtender ID="MaskedEditEnDate" runat="server" TargetControlID="txtEnRecordEndDate"
            Mask="99/99/9999" MaskType="Date">
        </cc1:MaskedEditExtender>
        <cc1:MaskedEditValidator ID="MaskedEditValEndDate" runat="server" ControlToValidate="txtEnRecordEndDate"
            ControlExtender="MaskedEditEnDate" Display="Dynamic" TooltipMessage="" IsValidEmpty="true"
            EmptyValueMessage="A Date is Required" InvalidValueMessage="This date is invalid"></cc1:MaskedEditValidator>
        <asp:CustomValidator
            ID="CustomValidator1" runat="server" ErrorMessage="require." Display="none" EnableClientScript="true">
        </asp:CustomValidator>
        <asp:Label ID="errormsg" runat="server" Text="" Visible="false" CssClass="errorText"></asp:Label>
    </div>
</div>
<div class="homesearchrow">
    <div class="homesearchbox">
        <label for="<%= listCancerType.ClientID%>">Cancer Type</label>
        <uc1:ListMultiSelect ID="listCancerType" Rows="6" TurnOffValidator="true" DisplayDefault="true"
            SelectionMode="Multiple" runat="server" CssClass="MultiSelect"></uc1:ListMultiSelect>
    </div>
    <div class="homesearchbox">
        <label for="<%= listSubject.ClientID%>">Subject</label>
        <uc1:ListMultiSelect ID="listSubject" Rows="6" TurnOffValidator="true" DisplayDefault="true"
            SelectionMode="Multiple" runat="server" CssClass="MultiSelect"></uc1:ListMultiSelect>
    </div>
    <div class="homesearchbox">
        <label for="<%= listAudience.ClientID%>">Audience</label>
        <uc1:ListMultiSelect ID="listAudience" Rows="6" TurnOffValidator="true" DisplayDefault="true"
            SelectionMode="Multiple" runat="server" CssClass="MultiSelect"></uc1:ListMultiSelect>
    </div>
</div>
<div class="homesearchrow">
    <div class="homesearchbox">
        <label for="<%= listLang.ClientID%>">Language</label>
        <uc1:ListMultiSelect ID="listLang" Rows="6" TurnOffValidator="true" DisplayDefault="true"
            SelectionMode="Multiple" runat="server" CssClass="MultiSelect"></uc1:ListMultiSelect>
    </div>
    <div class="homesearchbox">
        <label for="<%= listProdFormat.ClientID%>">Product Format</label>
        <uc1:ListMultiSelect ID="listProdFormat" Rows="6" TurnOffValidator="true" DisplayDefault="true"
            SelectionMode="Multiple" runat="server" CssClass="MultiSelect"></uc1:ListMultiSelect>
    </div>
    <div class="homesearchbox">
        <label for="<%= listSeries.ClientID%>">Series</label>
        <uc1:ListMultiSelect ID="listSeries" Rows="6" TurnOffValidator="true" DisplayDefault="true"
            SelectionMode="Multiple" runat="server" CssClass="MultiSelect"></uc1:ListMultiSelect>
    </div>
</div>
<div class="homesearchrow">
    <div class="homesearchbox">
        <label for="<%= listRace.ClientID%>">Race</label>
        <uc1:ListMultiSelect ID="listRace" Rows="6" TurnOffValidator="true" DisplayDefault="true"
            SelectionMode="Multiple" runat="server" CssClass="MultiSelect"></uc1:ListMultiSelect>
    </div>
    <div class="homesearchbox">
        <label for="<%= listBookStatus.ClientID%>">Book Status</label>
        <uc1:ListMultiSelect ID="listBookStatus" Rows="6" TurnOffValidator="true" DisplayDefault="true"
            SelectionMode="Multiple" runat="server" CssClass="MultiSelect"></uc1:ListMultiSelect>
    </div>
    <div class="homesearchbox">
        <label for="<%= listReadingLevel.ClientID%>">Reading Level</label>
        <uc1:ListMultiSelect ID="listReadingLevel" Rows="6" TurnOffValidator="true" DisplayDefault="true"
            SelectionMode="Single" runat="server" CssClass="MultiSelect"></uc1:ListMultiSelect>
    </div>
</div>
<div class="homesearchrow">
    <div class="homesearchbox">
        <label for="<%= listROOSubject.ClientID%>">NCIPLcc Subject</label>
        <uc1:ListMultiSelect ID="listROOSubject" Rows="6" TurnOffValidator="true" DisplayDefault="true"
            SelectionMode="Multiple" runat="server" CssClass="MultiSelect"></uc1:ListMultiSelect>
    </div>
    <div class="homesearchbox">
        <label for="<%= chboxMostCommonList.ClientID%>" >NCIPLcc Most Common List</label><asp:CheckBox ID="chboxMostCommonList" runat="server" Text="" TextAlign="Left" />
    </div>
    <div class="homesearchbox">
        <label for="<%= listOwner.ClientID%>">Owners</label>
        <uc1:ListMultiSelect ID="listOwner" Rows="6" TurnOffValidator="true" DisplayDefault="true"
            SelectionMode="Multiple" runat="server" CssClass="MultiSelect" />
    </div>
</div>
<div class="">
    <div class="homesearchbox bigbox">
        <label for="<%= listSponsor.ClientID%>">Sponsors</label>
        <uc1:ListMultiSelect ID="listSponsor" Rows="6" TurnOffValidator="true" DisplayDefault="true"
            SelectionMode="Multiple" runat="server" CssClass="MultiSelect" />
    </div>
    <div class="homesearchbox bigbox">
        <label for="<%= listAward.ClientID%>">Awards</label><uc1:ListMultiSelect ID="listAward" Rows="6" TurnOffValidator="true" DisplayDefault="true"
            SelectionMode="Multiple" runat="server" CssClass="MultiSelect"></uc1:ListMultiSelect>
    </div>
</div>
<asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" CssClass="btnGo" />
