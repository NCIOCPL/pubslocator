<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SubCatx.ascx.cs" Inherits="PubEntAdmin.UserControl.SubCatx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="AddSubcat.ascx" TagName="AddSubcat" TagPrefix="uc1" %>
<%@ Register Src="EditSubcat.ascx" TagName="EditSubcat" TagPrefix="uc1" %>

<script type="text/javascript">
    var currentTabIndex = 0;
    function clientActiveTabChanged(sender, args) {
        switch (sender.get_activeTabIndex()) {
            case 0:
                currentTabIndex = 0;
                showSpellChk();
                break;
            case 1:
                currentTabIndex = 1;
                hideSpellChk();
                break;
        }
    }
    function showSpellChk() {
        document.getElementById('<%= this.ParentSpellCkrID1 %>').style.display = '';
        document.getElementById('<%= this.ParentSpellCkrID2 %>').style.display = '';
    }
    function hideSpellChk() {
        document.getElementById('<%= this.ParentSpellCkrID1 %>').style.display = 'none';
        document.getElementById('<%= this.ParentSpellCkrID2 %>').style.display = 'none';
    }
    window.onload = SubCatOnload;
    var uncheckAllowHsSubSubReminder = 0;
    function SubCatOnload() {
        uncheckAllowHsSubSubReminder++;
        if (document.getElementById('<%= this.hidSubCatIndex.UniqueID %>').value == "1") {
            document.getElementById('<%= this.hidSubCatIndex.UniqueID %>').value = "";
            hideSpellChk();
        }
        else {
            if (document.getElementById('<%= this.ParentSpellCkrID1 %>')) {
                showSpellChk();
            }
        }
    }
</script>
<input id="hidSubCatIndex" type="hidden" runat="server" />
<input id="btnAddTab" runat="server" style="display: none;" type="button" onserverclick="btnAddTab_Click" />
<input id="btnEditTab" runat="server" style="display: none;" type="button" onserverclick="btnEditTab_Click" />
<asp:Wizard runat="server" ID="SubCatWizard" DisplaySideBar="false"
    Width="100%">
    <WizardSteps>
        <asp:TemplatedWizardStep runat="server" ID="CategoryStep" Title="" AllowReturn="true"
            StepType="Start">
            <ContentTemplate>
                Categories
                <asp:DropDownList ID="ddlCategory" AutoPostBack="true" runat="server" OnSelectedIndexChanged="CategoryStepSelectedIndexChanged">
                </asp:DropDownList>
            </ContentTemplate>
            <CustomNavigationTemplate>
            </CustomNavigationTemplate>
        </asp:TemplatedWizardStep>
        <asp:TemplatedWizardStep runat="server" ID="SubCategoryAddEditStep" Title="" AllowReturn="true"
            StepType="Step">
            <ContentTemplate>
                <asp:Label ID="lblCategoryStep" runat="server" Text=""></asp:Label>
                <cc1:TabContainer ID="tabCont" runat="server" OnClientActiveTabChanged="clientActiveTabChanged" ActiveTabIndex="1">
                    <cc1:TabPanel ID="tabpnlAdd" runat="server" Enabled="true" HeaderText="Add New Subcategory"
                        Width="100%">
                        <ContentTemplate>
                            <uc1:AddSubcat ID="AddSubcat1" runat="server" OnBubbleSaveSubcatClick="AddSubcat1Save_Click"
                                OnBubbleCancelSubcatClick="AddSubcat1Cancel_Click"></uc1:AddSubcat>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="tabpnlEdit" runat="server" Enabled="true" HeaderText="Edit Existing Subcategory"
                        Width="100%">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlSubCategory" runat="server" AutoPostBack="true" OnSelectedIndexChanged="SubCategoryAddEditStepSelectedIndexChanged">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
            </ContentTemplate>
            <CustomNavigationTemplate>
            </CustomNavigationTemplate>
        </asp:TemplatedWizardStep>
        <asp:TemplatedWizardStep runat="server" ID="SubCategoryEditStep" Title="" AllowReturn="true"
            StepType="Finish">
            <ContentTemplate>
                <asp:Label ID="lblCategoryStep2" runat="server" Text="Label"></asp:Label>
                <asp:LinkButton ID="lnkSubCategories" runat="server" OnClick="lnkSubCategories_Click">Subcategories</asp:LinkButton>
                <uc1:EditSubcat ID="EditSubcat1" runat="server" OnBubbleSaveEditSubcatClick="EditSubcat1Save_Click"
                    OnBubbleCancelEditSubcatClick="EditSubcat1Cancel_Click" OnBubbleDelEditSubcatClick="EditSubcat1Del_Click"
                    OnBubbleEditSubsubcatClick="EditSubcat1EditSubsubcat_Click" OnBubbleAddSubsubcatClick="EditSubcat1AddSubsubcat_Click"></uc1:EditSubcat>
            </ContentTemplate>
            <CustomNavigationTemplate>
            </CustomNavigationTemplate>
        </asp:TemplatedWizardStep>
    </WizardSteps>
</asp:Wizard>
