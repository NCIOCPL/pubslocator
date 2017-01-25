<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cssetup.aspx.cs" Inherits="PubEntAdmin.cssetup" %>

<%@ Register Src="~/UserControl/cslistbox.ascx" TagName="LookUpList" TagPrefix="UC1" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>Canned Search Set Up</title>
    <link href="CSS/Common.css" rel="stylesheet" type="text/css" />
</head>
<body class="Admin">
    <div class="skip"><a href="#mainBody">Skip to content</a></div>
    <div id="container">
        <h1>Publications Enterprise Admin Tool</h1>
        <h2>
            <asp:Label ID="lblPageTitle" runat="server" Text=""></asp:Label></h2>
        <form id="cssetupform" runat="server">
            <div id="header">
                <asp:PlaceHolder ID="plcHldMenu" runat="server"></asp:PlaceHolder>
            </div>
            <div id="mainBody">
                <div id="dvSearch">
                    <h3>Search for a Record or Add a New Record</h3>
                    <div class="searchbox">
                        <asp:Label ID="Label1" runat="server" Text="Record ID"
                            AssociatedControlID="txtFind"></asp:Label>
                        <asp:TextBox ID="txtFind" runat="server"
                            MaxLength="69" ValidationGroup="ValidationSummary1"
                            EnableViewState="False" Width="90%"
                            placeholder="Leave blank to find all. Use space to separate multiple record IDs."></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                            ErrorMessage="Invalid input. Please use space to separate each Record ID, if you are searching for multiple Record IDs."
                            ControlToValidate="txtFind" Display="Static"
                            ValidationExpression="^[a-zA-Z0-9 ]{1,69}$" EnableClientScript="False"> * </asp:RegularExpressionValidator>
                    </div>
                    <div class="btncontainer">
                        <div>
                            &nbsp;
                        </div>
                        <asp:Button ID="btnFind" CssClass="btnGo"
                            runat="server" Text="Search" OnClick="btnFind_Click" /><span> or </span>
                        <asp:Button ID="btnNew" CssClass="btn"
                            runat="server" Text="New" OnClick="btnNew_Click" />
                    </div>
                </div>
                <div id="dvAdd" runat="server" visible="false">
                    <h3>Add a New Record</h3>
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btnGo" OnClick="btnSave_Click" />
                    <div class="editbox">
                        <asp:Label ID="Label4" runat="server" Text="Header Text" CssClass="" AssociatedControlID="txtHeaderText"></asp:Label>
                        <asp:TextBox ID="txtHeaderText" runat="server"
                            ValidationGroup="ValidationSummary1" MaxLength="500" Width="300px"
                            EnableViewState="False"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                            ErrorMessage="Please provide a value for the Header Text."
                            ControlToValidate="txtHeaderText" Visible="True" Display="Static"
                            EnableClientScript="False"> * </asp:RequiredFieldValidator>
                    </div>
                    <div class="editboxrow">
                        <div class="editbox">
                            <asp:Label ID="Label5" runat="server" Text="Type of Cancer" AssociatedControlID="ucCancerTypeAdd$LookUpList"></asp:Label>
                            <UC1:LookUpList ID="ucCancerTypeAdd" runat="server"
                                EnableViewState="True" />
                        </div>
                        <div class="editbox">
                            <asp:Label ID="Label6" runat="server" Text="Subject" AssociatedControlID="ucSubjectAdd$LookUpList"></asp:Label>
                            <UC1:LookUpList ID="ucSubjectAdd" runat="server"
                                EnableViewState="True" />
                        </div>
                        <div class="editbox">
                            <asp:Label ID="Label7" runat="server" Text="Publication Format" AssociatedControlID="ucPubFormatAdd$LookUpList"></asp:Label>
                            <UC1:LookUpList ID="ucPubFormatAdd" runat="server" />
                        </div>
                    </div>
                    <div class="editboxrow">
                        <div class="editbox">
                            <asp:Label ID="Label8" runat="server" Text="Race" AssociatedControlID="ucRaceAdd$LookUpList"></asp:Label>
                            <UC1:LookUpList ID="ucRaceAdd" runat="server" />
                        </div>
                        <div class="editbox">
                            <asp:Label ID="Label9" runat="server" Text="Audience" AssociatedControlID="ucAudienceAdd$LookUpList"></asp:Label>
                            <UC1:LookUpList ID="ucAudienceAdd" runat="server" />
                        </div>
                        <div class="editbox">
                            <asp:Label ID="Label10" runat="server" Text="Language" AssociatedControlID="ucLanguageAdd$LookUpList"></asp:Label>
                            <UC1:LookUpList ID="ucLanguageAdd" runat="server" />
                        </div>
                    </div>
                    <div class="editboxrow">
                        <div class="editbox">
                            <asp:Label ID="Label3" runat="server" Text="Collections" AssociatedControlID="ucCancerTypeAdd$LookUpList"></asp:Label>
                            <UC1:LookUpList ID="ucCollectionsAdd" runat="server" />
                        </div>
                    </div>
                    <asp:CustomValidator ID="CustomValidator1" runat="server"
                        ErrorMessage="Please select at least one value from the lists."
                        OnServerValidate="CustomValidator1_ServerValidate"
                        ValidateEmptyText="True" Visible="True" Display="None"></asp:CustomValidator>
                </div>
                <div id="dvEdit" runat="server" visible="false">
                    <h3>Canned Search Records</h3>
                    <asp:ListView ID="lstviewCannedRecords" runat="server"
                        OnItemDataBound="lstviewCannedRecords_ItemDataBound"
                        OnItemEditing="lstviewCannedRecords_ItemEditing"
                        OnItemCanceling="lstviewCannedRecords_ItemCanceling"
                        OnItemUpdating="lstviewCannedRecords_ItemUpdating"
                        OnPagePropertiesChanging="lstviewCannedRecords_PagePropertiesChanging">
                        <LayoutTemplate>
                            <span id="itemPlaceholder" runat="server" />
                        </LayoutTemplate>
                        <ItemTemplate>
                            <asp:HiddenField ID="CannId" runat="server" />
                            <asp:Button ID="btnEdit" CssClass="btn" runat="server" Text="Edit" CommandName="Edit" />
                            <table>
                                <tr>
                                    <td>
                                        <span class="">Record ID</span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblRecordId" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td><span class="">Header Text</span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblHeaderText" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td><span class="">Url</span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblUrl" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td><span class="">Type of Cancer</span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCancerType" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td><span class="">Subject</span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblSubject" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td><span class="">Publication Format</span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPubFormat" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td><span class="">Race</span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblRace" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td><span class="">Audience</span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblAudience" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td><span class="">Language</span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblLanguage" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td><span class="">Collections</span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCollections" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td><span class="">Active</span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblActive" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:HiddenField ID="CannIdEdit" runat="server" />
                            <div class="editbox">
                                <asp:Label ID="Label2" runat="server" Text="Record ID" CssClass=""></asp:Label>
                                <asp:Label ID="lblRecordIdEdit" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="editbox">
                                <asp:Label ID="Label4" runat="server" Text="Header Text" CssClass="" AssociatedControlID="txtHeaderTextEdit"></asp:Label>
                                <asp:TextBox ID="txtHeaderTextEdit" runat="server" MaxLength="500" Width="300px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                    ErrorMessage="Please provide a value for the Header Text."
                                    ControlToValidate="txtHeaderTextEdit" Visible="True" Display="Static"
                                    EnableClientScript="False"> * </asp:RequiredFieldValidator>
                            </div>
                            <div class="editbox editbtns">
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn" CommandName="Cancel" />
                                <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btnGo" CommandName="Update" />
                            </div>
                            <div class="editboxrow">
                                <div class="editbox">
                                    <asp:Label runat="server" Text="Type of Cancer" AssociatedControlID="ucCancerTypeEdit$LookUpList"></asp:Label>
                                    <UC1:LookUpList ID="ucCancerTypeEdit" runat="server" />
                                </div>
                                <div class="editbox">
                                    <asp:Label runat="server" Text="Subject" AssociatedControlID="ucSubjectEdit$LookUpList"></asp:Label>
                                    <UC1:LookUpList ID="ucSubjectEdit" runat="server" />
                                </div>
                                <div class="editbox">
                                    <asp:Label runat="server" Text="Publication Format" AssociatedControlID="ucPubFormatEdit$LookUpList"></asp:Label>
                                    <UC1:LookUpList ID="ucPubFormatEdit" runat="server" />
                                </div>
                            </div>
                            <div class="editboxrow">
                                <div class="editbox">
                                    <asp:Label runat="server" Text="Race" AssociatedControlID="ucRaceEdit$LookUpList"></asp:Label>
                                    <UC1:LookUpList ID="ucRaceEdit" runat="server" />
                                </div>
                                <div class="editbox">
                                    <asp:Label runat="server" Text="Audience" AssociatedControlID="ucAudienceEdit$LookUpList"></asp:Label>
                                    <UC1:LookUpList ID="ucAudienceEdit" runat="server" />
                                </div>
                                <div class="editbox">
                                    <asp:Label runat="server" Text="Language" AssociatedControlID="ucLanguageEdit$LookUpList"></asp:Label>
                                    <UC1:LookUpList ID="ucLanguageEdit" runat="server" />
                                </div>
                            </div>
                            <div class="editboxrow">
                                <div class="editbox">
                                    <asp:Label runat="server" Text="Collections" AssociatedControlID="ucCollectionsEdit$LookUpList"></asp:Label>
                                    <UC1:LookUpList ID="ucCollectionsEdit" runat="server" />
                                </div>
                                <div class="editbox">
                                    <asp:Label runat="server" Text="Active" AssociatedControlID="ddlActiveEdit"></asp:Label>
                                    <asp:DropDownList ID="ddlActiveEdit" runat="server">
                                        <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                                        <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </EditItemTemplate>
                        <ItemSeparatorTemplate><span class="divider"></span></ItemSeparatorTemplate>
                        <EmptyDataTemplate>
                            <span>No records were returned by the search.</span>
                        </EmptyDataTemplate>
                    </asp:ListView>
                    <!--End ListView-->
                </div>
                <div id="dvPager">
                    <asp:DataPager ID="DataPager" runat="server"
                        PagedControlID="lstviewCannedRecords" PageSize="2">
                        <Fields>
                            <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True" ShowLastPageButton="True" />
                        </Fields>
                    </asp:DataPager>
                </div>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
            </div>
        </form>
    </div>
</body>
</html>
