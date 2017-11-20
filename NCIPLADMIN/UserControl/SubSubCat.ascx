<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SubSubCat.ascx.cs" Inherits="PubEntAdmin.UserControl.SubSubCat" %>
<%@ Register Src="AddSubsubcat.ascx" TagName="AddSubsubcat" TagPrefix="uc1" %>
<%@ Register Src="EditSubsubcat.ascx" TagName="EditSubsubcat" TagPrefix="uc1" %>
<%@ Register Src="TabStrip.ascx" TagName="TabStrip" TagPrefix="uc1" %>
<input id="hidSubSubCatIndex" type="hidden" runat="server" />
<asp:Wizard runat="server" ID="SubsubcatWizard" DisplaySideBar="False"
    Width="100%" OnNextButtonClick="OnNext" OnActiveStepChanged="OnActiveStepChanged">
    <WizardSteps>
        <asp:TemplatedWizardStep runat="server" ID="SubsubcategoryStep" Title="" AllowReturn="true"
            StepType="Auto">
            <ContentTemplate>
                <asp:Label ID="lblCat_SubcatStep" runat="server" Text=""></asp:Label>
                <uc1:TabStrip ID="TabStrip1" runat="server" OnSelectionChanged="TabStrip1_SelectionChanged" />
                <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                    <asp:View ID="Tab1" runat="server">
                        <uc1:AddSubsubcat ID="AddSubsubcat1" runat="server" OnBubbleSaveSubsubcatClick="AddSubsubcat1Save_Click"
                            OnBubbleCancelSubsubcatClick="AddNewSubsubcat1Cancel_Click"></uc1:AddSubsubcat>
                    </asp:View>
                    <asp:View ID="Tab2" runat="server">
                        <asp:DropDownList ID="ddlSubSubCategory" runat="server" AutoPostBack="true" OnSelectedIndexChanged="SubsubcategoryStepSelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:Label ID="errMsg" runat="server" Text="" CssClass="error"></asp:Label>
                    </asp:View>
                </asp:MultiView>
            </ContentTemplate>
        </asp:TemplatedWizardStep>
        <asp:TemplatedWizardStep runat="server" ID="SubsubcategoryEditStep" Title="" AllowReturn="true"
            StepType="Finish">
            <ContentTemplate>
                <asp:Label ID="lblcategoryStep" runat="server" Text=""></asp:Label>
                <asp:Label ID="lblsubcategoryStep" runat="server" Text=""></asp:Label>
                <asp:Button ID="lnkSubCategories" runat="server" CssClass="btn"
                    OnClick="lnkSubsubategories_Click" Text="SubSubcategories"></asp:Button>
                <uc1:EditSubsubcat ID="EditSubsubcat1" runat="server" OnBubbleSaveEditSubsubcatClick="EditSubsubcat1Save_Click"
                    OnBubbleCancelEditSubsubcatClick="EditSubsubcat1Cancel_Click" OnBubbleDelEditSubsubcatClick="EditSubsubcat1Del_Click"></uc1:EditSubsubcat>
            </ContentTemplate>
        </asp:TemplatedWizardStep>
    </WizardSteps>
    <FinishNavigationTemplate>
        <asp:Button ID="FinishPreviousButton" runat="server" CausesValidation="False" CssClass="btn"
            CommandName="MovePrevious" Text="Previous" />
        <asp:Button ID="FinishButton" runat="server" CommandName="MoveComplete" CssClass="btn"
            Text="Finish" />
    </FinishNavigationTemplate>
    <StepNavigationTemplate>
        <asp:Button ID="StepPreviousButton" runat="server" CausesValidation="False" CssClass="btn"
            CommandName="MovePrevious" Text="Previous" />
        <asp:Button ID="StepNextButton" runat="server" CommandName="MoveNext" CssClass="btn"
            Text="Next" />
    </StepNavigationTemplate>
</asp:Wizard>
