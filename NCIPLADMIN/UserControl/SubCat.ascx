<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SubCat.ascx.cs" Inherits="PubEntAdmin.UserControl.SubCat" %>
<%@ Register Src="AddSubcat.ascx" TagName="AddSubcat" TagPrefix="uc1" %>
<%@ Register Src="EditSubcat.ascx" TagName="EditSubcat" TagPrefix="uc1" %>
<%@ Register Src="TabStrip.ascx" TagName="TabStrip" TagPrefix="uc1" %>
<input id="hidSubCatIndex" type="hidden" runat="server" />
<asp:Wizard runat="server" ID="SubCatWizard" DisplaySideBar="False" Width="100%"
    OnNextButtonClick="OnNext" OnActiveStepChanged="OnActiveStepChanged">
    <WizardSteps>
        <asp:TemplatedWizardStep runat="server" ID="CategoryStep" Title="" AllowReturn="true"
            StepType="Auto">
            <ContentTemplate>
                <asp:Label ID="Label2" runat="server" AssociatedControlID="ddlCategory">Categories</asp:Label>
                <asp:DropDownList ID="ddlCategory" AutoPostBack="true" runat="server" OnSelectedIndexChanged="CategoryStepSelectedIndexChanged">
                </asp:DropDownList><br />
                <asp:Label ID="errMsg" runat="server" Text="" CssClass="error"></asp:Label>
            </ContentTemplate>
        </asp:TemplatedWizardStep>
        <asp:TemplatedWizardStep runat="server" ID="SubCategoryAddEditStep" Title="" AllowReturn="true"
            StepType="Auto">
            <ContentTemplate>
                <asp:Label ID="lblCategoryStep" runat="server" Text=""></asp:Label>
                <uc1:TabStrip ID="TabStrip1" runat="server" OnSelectionChanged="TabStrip1_SelectionChanged" />
                <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                    <asp:View ID="Tab1" runat="server">
                        <uc1:AddSubcat ID="AddSubcat1" runat="server" OnBubbleSaveSubcatClick="AddSubcat1Save_Click"
                            OnBubbleCancelSubcatClick="AddSubcat1Cancel_Click"></uc1:AddSubcat>
                    </asp:View>
                    <asp:View ID="Tab2" runat="server">
                        <asp:DropDownList ID="ddlSubCategory" runat="server" AutoPostBack="true" OnSelectedIndexChanged="SubCategoryAddEditStepSelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:Label ID="errMsg" runat="server" Text="" CssClass="error"></asp:Label>
                    </asp:View>
                </asp:MultiView>
            </ContentTemplate>
        </asp:TemplatedWizardStep>
        <asp:TemplatedWizardStep runat="server" ID="SubCategoryEditStep" Title="" AllowReturn="true"
            StepType="Finish">
            <ContentTemplate>
                <asp:Label ID="lblCategoryStep2" runat="server" Text="Label"></asp:Label>
                <asp:Button ID="lnkSubCategories" runat="server" OnClick="lnkSubCategories_Click"
                    CssClass="btn" Text="Subcategories"></asp:Button>
                <uc1:EditSubcat ID="EditSubcat1" runat="server" OnBubbleSaveEditSubcatClick="EditSubcat1Save_Click"
                    OnBubbleCancelEditSubcatClick="EditSubcat1Cancel_Click" OnBubbleDelEditSubcatClick="EditSubcat1Del_Click"
                    OnBubbleEditSubsubcatClick="EditSubcat1EditSubsubcat_Click" OnBubbleAddSubsubcatClick="EditSubcat1AddSubsubcat_Click"></uc1:EditSubcat>
            </ContentTemplate>
        </asp:TemplatedWizardStep>
    </WizardSteps>
    <FinishNavigationTemplate>
        <asp:Button ID="FinishPreviousButton" runat="server" CssClass="btn" CausesValidation="False"
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