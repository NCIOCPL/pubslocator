<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminMenu.ascx.cs" Inherits="PubEntAdmin.UserControl.AdminMenu" %>
<asp:Menu ID="AdminMenu1" runat="server" DataSourceID="XmlDataSrcMenu" MaximumDynamicDisplayLevels="4"
    StaticDisplayLevels="1" DynamicHorizontalOffset="1" DynamicVerticalOffset="1"
    Orientation="Horizontal" OnMenuItemDataBound="Menu_OnMenuItemDataBound" 
    CssClass="linkNavAdmin, adminmenu-border, adminmenu-top" SkipLinkText=''>
    <StaticMenuItemStyle CssClass="linkNavAdmin" />
    <DynamicHoverStyle CssClass="linkNavAdmin:hover" BackColor="#6699FF" />
    <StaticHoverStyle CssClass="linkNavAdmin:hover" BackColor="#6699FF" />
    <DynamicMenuStyle BackColor="#D7E6EF" CssClass="adminmenu-border 
        DynamicItemZIndex" />
    <StaticSelectedStyle CssClass="linkNavAdmin:active" />
    <DynamicSelectedStyle CssClass="linkNavAdmin:link" />
    <DynamicMenuItemStyle CssClass="linkNavAdmin" />
    <DataBindings>
        <asp:MenuItemBinding DataMember="Menu" TextField="text" ValueField="text" NavigateUrlField="url" />
        <asp:MenuItemBinding DataMember="SubMenu" NavigateUrlField="url" TextField="text"
            ValueField="text" />
    </DataBindings>
</asp:Menu>
<asp:XmlDataSource ID="XmlDataSrcMenu" runat="server" XPath="/Home/Menu"></asp:XmlDataSource>
<div class="container" id="xjsRecordMenu" runat="server">
    Record<br />
    <a class="inside" href="PubRecord.aspx?mode=add">Add New Publication</a> <a class="inside"
        href="Announcement.aspx">Announcement</a> <a class="inside" href="CatSeq.aspx">Catalog
            Seq.</a> <a class="inside" href="Conference.aspx">Exhibit</a>
    <a class="inside" href="featuredpubs.aspx">Featured Pubs Setup</a> <a class="inside"
        href="DisplayKitPub.aspx?type=lp">Linked Pubs</a> <a class="inside" href="DisplayKitPub.aspx?type=vk">Virtual Kit</a> <a class="inside" href="customertype.aspx">Type of Customer</a><!--NCIPL_CC-->
    <a class="inside" href="ordermedia.aspx">Order Source</a><!--NCIPL_CC-->
</div>
<div class="container" id="xjsSearchMenu" runat="server">
    <a class="inside" href="AdminHome.htm">Search</a>
</div>
<div class="container" id="xjsReportsMenu" runat="server">
    Reports<br />
    <a class="inside" href="FeaturedStacksAccessReport.aspx">Featured Stacks Access</a>
    <a class="inside" href="featuredpubshistreport.aspx">Featured Stacks History</a>
    <a class="inside" href="KeywordReport.aspx">Keyword Report</a> <a class="inside"
        href="SearchKeywords.aspx">Search Keywords</a>
</div>
<div class="container" id="xjsLookupsMenu" runat="server">
    Lookups<br />
    <a class="inside" href="LookupMgmt.aspx?sub=audience">Audience</a> <a class="inside"
        href="LookupMgmt.aspx?sub=award">Awards</a> <a class="inside" href="LookupMgmt.aspx?sub=cancertype">Cancer Type</a> <a class="inside" href="LookupMgmt.aspx?sub=language">Language</a>
    <a class="inside" href="LookupMgmt.aspx?sub=owner">Owner</a> <a class="inside" href="LookupMgmt.aspx?sub=prodformat">Publication Format</a> <a class="inside" href="LookupMgmt.aspx?sub=readinglevel">Reading
            Level</a> <a class="inside" href="LookupMgmt.aspx?sub=sponsor">Sponsor</a>
    <a class="inside" href="LookupMgmt.aspx?sub=race">Race</a> <a class="inside" href="LookupMgmt.aspx?sub=serie">Collections</a> <a class="inside" href="LookupMgmt.aspx?sub=subj">Subject</a>
    <a class="inside" href="LookupMgmt.aspx?sub=subcat">Subcategory</a> <a class="inside"
        href="LookupMgmt.aspx?sub=subsubcat">Sub Subcategory</a>
</div>
<div class="container" id="xjsCannedMenu" runat="server">
    <a class="inside" href="cssetup.aspx">Canned Search</a>
</div>
<div class="container" id="xjsHelpMenu" runat="server">
    <a class="inside" href="Help/index.html" target="_blank">Help</a>
</div>
<div class="container" id="xjsLogoutMenu" runat="server">
    <a class="inside" href="Logout.aspx">Logout</a>
</div>
