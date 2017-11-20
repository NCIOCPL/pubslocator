<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SubSubCatx.ascx.cs" Inherits="PubEntAdmin.UserControl.SubSubCatx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="AddSubsubcat.ascx" TagName="AddSubsubcat" TagPrefix="uc1" %>
<%@ Register Src="EditSubsubcat.ascx" TagName="EditSubsubcat" TagPrefix="uc1" %>

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
    window.onload = SubSubCatOnload;
    function SubSubCatOnload() {
        if (document.getElementById('<%= this.hidSubSubCatIndex.UniqueID %>').value == "1") {
            document.getElementById('<%= this.hidSubSubCatIndex.UniqueID %>').value = "";
            hideSpellChk();
        }
        else {
            if (document.getElementById('<%= this.ParentSpellCkrID1 %>')) {
                showSpellChk();
            }
        }
    }
</script>
<input id="hidSubSubCatIndex" type="hidden" runat="server" />
<input id="btnAddTab" runat="server" style="display: none;" type="button" onserverclick="btnAddTab_Click" />
<input id="btnEditTab" runat="server" style="display: none;" type="button" onserverclick="btnEditTab_Click" />
<asp:Wizard runat="server" ID="SubsubcatWizard" DisplaySideBar="false"
    Width="100%">
    <WizardSteps>
        <asp:TemplatedWizardStep runat="server" ID="SubsubcategoryStep" Title="" AllowReturn="true"
            StepType="Start">
            <ContentTemplate>
                <asp:Label ID="lblCat_SubcatStep" runat="server" Text=""></asp:Label>
                <cc1:TabContainer ID="tabCont" runat="server" OnClientActiveTabChanged="clientActiveTabChanged" ActiveTabIndex="1">
                    <cc1:TabPanel ID="tabpnlAdd" runat="server" Enabled="true" HeaderText="Add New Sub subcategory"
                        Width="100%">
                        <ContentTemplate>
                            <uc1:AddSubsubcat ID="AddSubsubcat1" runat="server" OnBubbleSaveSubsubcatClick="AddSubsubcat1Save_Click"
                                OnBubbleCancelSubsubcatClick="AddNewSubsubcat1Cancel_Click"></uc1:AddSubsubcat>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="tabpnlEdit" runat="server" Enabled="true" HeaderText="Edit Existing Sub subcategory"
                        Width="100%">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlSubSubCategory" runat="server" AutoPostBack="true"
                                OnSelectedIndexChanged="SubsubcategoryStepSelectedIndexChanged">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
            </ContentTemplate>
            <CustomNavigationTemplate>
            </CustomNavigationTemplate>
        </asp:TemplatedWizardStep>
        <asp:TemplatedWizardStep runat="server" ID="SubsubcategoryEditStep" Title="" AllowReturn="true"
            StepType="Finish">
            <ContentTemplate>
                <asp:Label ID="lblcategoryStep" runat="server" Text=""></asp:Label>
                <asp:Label ID="lblsubcategoryStep" runat="server" Text=""></asp:Label>
                <asp:LinkButton ID="lnkSubCategories" runat="server" OnClick="lnkSubsubategories_Click">SubSubcategories</asp:LinkButton>
                <uc1:EditSubsubcat ID="EditSubsubcat1" runat="server"
                    OnBubbleSaveEditSubsubcatClick="EditSubsubcat1Save_Click"
                    OnBubbleCancelEditSubsubcatClick="EditSubsubcat1Cancel_Click"
                    OnBubbleDelEditSubsubcatClick="EditSubsubcat1Del_Click"></uc1:EditSubsubcat>
            </ContentTemplate>
            <CustomNavigationTemplate>
            </CustomNavigationTemplate>
        </asp:TemplatedWizardStep>
    </WizardSteps>
</asp:Wizard>
