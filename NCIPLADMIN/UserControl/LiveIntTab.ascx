<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LiveIntTab.ascx.cs"
    Inherits="PubEntAdmin.UserControl.LiveIntTab" EnableViewState="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="TextCtrl_SpellCk.ascx" TagName="TextCtrl_SpellCk" TagPrefix="uc1" %>

<script type="text/javascript">
    var currentTabIndex = -1;
    //alert('new currentTabIndex assignment!!!');
    var nextTabIndex;
    function LiveIntTabOnload() {
        if (document.getElementById('<%= this.hidCurrTabIndex.UniqueID %>')) //HITT #12045
        {
            if (document.getElementById('<%= this.hidCurrTabIndex.UniqueID %>').value.length > 0) {
                currentTabIndex = document.getElementById('<%= this.hidCurrTabIndex.UniqueID %>').value;
                //alert(' in LiveIntTabOnload, currentTabIndex ' + document.getElementById('<%= this.hidCurrTabIndex.ClientID %>').value);
                var tab = document.getElementById('<%= this.tabContLiveInt.ClientID %>');
                tab.ActiveTabIndex = currentTabIndex;
                //document.getElementById('<%= this.hidCurrTabIndex.UniqueID %>').value = "";
            }
        }
    }
    function DisableEnableTab(boolVal, tabNo) {
        var tab = $find('<%= this.tabContLiveInt.ClientID %>');
        if (boolVal == true) {
            tab.get_tabs()[tabNo].set_enabled(false);
        }
        else {
            tab.get_tabs()[tabNo].set_enabled(true);
        }
    }
    function setHidActiveIndex(idx) {
        document.getElementById('<%= this.hidCurrTabIndex.UniqueID %>').value = idx;
    }
    function canChangeTab(currTabIndex) {
        var can = true;
        if (typeof monitorTabChangesIDs != 'undefined') {
            //alert('calling confirmTabClose');
            can = confirmTabClose();
        }
        //alert('in canChangeTab ' + can);
        return can;
    }
    function clientActiveTabChanged(sender, args) {
        needToConfirm = true;
        nextTabIndex = sender.get_activeTabIndex();
        if (currentTabIndex == -1)
            LiveIntTabOnload();
        //alert('currentTabIndex = '+ currentTabIndex+' nextTabIndex = '+nextTabIndex);
        if (currentTabIndex != sender.get_activeTabIndex())//not clicking the current tab
        {
            if (canChangeTab(currentTabIndex)) {
                switch (sender.get_activeTabIndex()) {
                    case 1:
                        currentTabIndex = 1;
                        __doPostBack('<%= this.btnNCIPL.UniqueID %>', '');
                        break;
                    case 4:
                        currentTabIndex = 4;
                        __doPostBack('<%= this.btnCatalog.UniqueID %>', '');
                        break;
                    case 0:
                        currentTabIndex = 0;
                        __doPostBack('<%= this.btnPubHist.UniqueID %>', '');
                        break;
                    case 2:
                        currentTabIndex = 2;
                        __doPostBack('<%= this.btnROO.UniqueID %>', '');
                        break;
                    case 3:
                        currentTabIndex = 3;
                        __doPostBack('<%= this.btnExh.UniqueID %>', '');
                        break;
                    case 5:
                        currentTabIndex = 5;
                        __doPostBack('<%= this.btnCmt.UniqueID %>', '');
                        break;
                    case 6:
                        currentTabIndex = 6;
                        __doPostBack('<%= this.btnAttach.UniqueID %>', '');
                        break;
                    case 7:
                        currentTabIndex = 7;
                        __doPostBack('<%= this.btnRelated.UniqueID %>', '');
                        break;
                    case 8:
                        currentTabIndex = 8;
                        __doPostBack('<%= this.btnTranslation.UniqueID %>', '');
                        break;
                }
            }
            else {
                sender.set_activeTab(sender.get_tabs()[currentTabIndex]);
            }
        }
    }
</script>
<input id="hidCurrTabIndex" type="hidden" runat="server" value="" />
<input id="btnNCIPL" runat="server" style="display: none;" type="button" onserverclick="btnNCIPL_Click" />
<input id="btnCatalog" runat="server" style="display: none;" type="button" onserverclick="btnCatalog_Click" />
<input id="btnPubHist" runat="server" style="display: none;" type="button" onserverclick="btnPubHist_Click" />
<input id="btnROO" runat="server" style="display: none;" type="button" onserverclick="btnROO_Click" />
<input id="btnExh" runat="server" style="display: none;" type="button" onserverclick="btnExh_Click" />
<input id="btnCmt" runat="server" style="display: none;" type="button" onserverclick="btnCmt_Click" />
<input id="btnAttach" runat="server" style="display: none;" type="button" onserverclick="btnAttach_Click" />
<input id="btnRelated" runat="server" style="display: none;" type="button" onserverclick="btnRelated_Click" />
<input id="btnTranslation" runat="server" style="display: none;" type="button" onserverclick="btnTranslation_Click" />
<cc1:TabContainer ID="tabContLiveInt" runat="server" OnClientActiveTabChanged="clientActiveTabChanged">
    <cc1:TabPanel runat="server" ID="tabpnlPubHist" Enabled="true" HeaderText="Prod History" 
        Width="100%">
        <ContentTemplate>
            <div id="PubHistAlertDiv" class="AlertStyle">
            </div>
            <asp:UpdatePanel ID="updpnlPubHist" runat="server" UpdateMode="Conditional"  >
                <ContentTemplate>
                    <input id="hiddenRevdDate" type="hidden" runat="server" />
                    <asp:PlaceHolder ID="plHldPubHistTabAddInfo" runat="server"></asp:PlaceHolder>
                    <asp:PlaceHolder ID="plHldPubHistTabViewInfo" runat="server"></asp:PlaceHolder>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnPubHist" />
                </Triggers>
            </asp:UpdatePanel>
        </ContentTemplate>
    </cc1:TabPanel>
    <cc1:TabPanel runat="server" ID="tabpnlNCIPL" Enabled="true" HeaderText="NCIPL" Width="100%">
        <ContentTemplate>
            <div id="NCIPLAlertDiv" class="AlertStyle">
            </div>
            <asp:UpdatePanel ID="updpnlNCIPL" runat="server" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnNCIPL" />
                </Triggers>
                <ContentTemplate>
                    <asp:PlaceHolder ID="plHldNCIPLTabInfo" runat="server"></asp:PlaceHolder>
                    <asp:Button ID="btnNCIPLTabInfo" runat="server" Text="Edit" OnClick="btnNCIPLTabInfo_Click"
                        Visible="false" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </cc1:TabPanel>
    <cc1:TabPanel runat="server" ID="tabpnlROO" Enabled="true" HeaderText="NCIPLcc" Width="100%">
        <ContentTemplate>
            <div id="ROOAlertDiv" class="AlertStyle">
            </div>
            <asp:UpdatePanel ID="updpnlROO" runat="server" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnROO" />
                    <asp:AsyncPostBackTrigger ControlID="btnROOTabInfo" />
                </Triggers>
                <ContentTemplate>
                    <asp:PlaceHolder ID="plHldROOTabInfo" runat="server"></asp:PlaceHolder>
                    <asp:Button ID="btnROOTabInfo" runat="server" Text="Edit" OnClick="btnROOTabInfo_Click"
                        Visible="false" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </cc1:TabPanel>
    <cc1:TabPanel runat="server" ID="tabpnlExh" Enabled="true" HeaderText="Exhibits"
        Width="100%">
        <ContentTemplate>
            <div id="ExhAlertDiv" class="AlertStyle">
            </div>
            <asp:UpdatePanel ID="updpnlExh" runat="server" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnExh" />
                </Triggers>
                <ContentTemplate>
                    <asp:PlaceHolder ID="plHldExhTabInfo" runat="server"></asp:PlaceHolder>
                    <asp:Button ID="btnExhTabInfo" runat="server" Text="Edit" OnClick="btnExhTabInfo_Click"
                        Visible="false" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </cc1:TabPanel>
    <cc1:TabPanel runat="server" ID="tabpnlCatalog" Enabled="true" HeaderText="Catalog"
        Width="100%">
        <ContentTemplate>
            <div id="CatalogAlertDiv" class="AlertStyle">
            </div>
            <asp:UpdatePanel ID="updpnlCatelog" runat="server" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnCatalog" />
                </Triggers>
                <ContentTemplate>
                    <asp:PlaceHolder ID="plHldCatalogTabInfo" runat="server"></asp:PlaceHolder>
                    <asp:Button ID="btnCatalogTabInfo" runat="server" Text="Edit" OnClick="btnCatalogTabInfo_Click"
                        Visible="false" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </cc1:TabPanel>
    <cc1:TabPanel runat="server" ID="tabpnlCmt" Enabled="true" HeaderText="Comment" Width="100%">
        <ContentTemplate>
            <div id="CmtAlertDiv" class="AlertStyle">
            </div>
            <asp:UpdatePanel ID="updpnlCmt" runat="server" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnCmt" />
                </Triggers>
                <ContentTemplate>
                    <asp:PlaceHolder ID="plHldCmtTabInfo" runat="server"></asp:PlaceHolder>
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </cc1:TabPanel>
    <cc1:TabPanel runat="server" ID="tabpnlAttach" Enabled="true" HeaderText="Attachment"
        Width="100%">
        <ContentTemplate>
            <div id="AttachAlertDiv" class="AlertStyle">
            </div>
            <asp:UpdatePanel ID="updpnlAttach" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:PlaceHolder ID="plHldAttachTabInfo" runat="server"></asp:PlaceHolder>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnAttach" />
                </Triggers>
            </asp:UpdatePanel>
        </ContentTemplate>
    </cc1:TabPanel>
    <cc1:TabPanel runat="server" ID="tabpnlRelated" Enabled="true" HeaderText="Related Products"
        Width="100%">
        <ContentTemplate>
            <div id="RelatedAlertDiv" class="AlertStyle">
            </div>
            <asp:UpdatePanel ID="updpnlRelated" runat="server" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnRelated" />
                </Triggers>
                <ContentTemplate>
                    <asp:PlaceHolder ID="plHldRelatedTabInfo" runat="server"></asp:PlaceHolder>
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </cc1:TabPanel>
    <cc1:TabPanel runat="server" ID="tabpnlTranslation" Enabled="true" HeaderText="Translations"
        Width="100%">
        <ContentTemplate>
            <div id="TranslationAlertDiv" class="AlertStyle">
            </div>
            <asp:UpdatePanel ID="updpnlTranslation" runat="server" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnTranslation" />
                </Triggers>
                <ContentTemplate>
                    <asp:PlaceHolder ID="plHldTranslationTabInfo" runat="server"></asp:PlaceHolder>
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </cc1:TabPanel>
</cc1:TabContainer>
