<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CatalogTabEditInfo.ascx.cs"
    Inherits="PubEntAdmin.UserControl.CatalogTabEditInfo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="wtf" Assembly="PubEntAdmin" Namespace="PubEntAdmin.CustomControl" %>
<%@ Register TagPrefix="uc1" TagName="ListMultiSelect" Src="ListMultiSelect.ascx" %>

<div class="editboxrow">
    <div class="editbox">
        <asp:Label ID="lblCat" runat="server" AssociatedControlID="listCategory" Text="Category:" CssClass=""></asp:Label>
        <asp:DropDownList ID="listCategory" runat="server" />
        <input id="hidden_listCategory" type="hidden" runat="server" />
        <cc1:CascadingDropDown ID="CascadingDropDown1" runat="server" Category="category" BehaviorID="CascadingDropDown1"
            TargetControlID="listCategory" PromptText="[Select Category]" LoadingText="Loading categories..."
            ServicePath="~/WS/ws_category.asmx" ServiceMethod="GetCategory">
        </cc1:CascadingDropDown>
    </div>
</div>
<div class="editboxrow">
    <div class="editbox">
        <asp:Label ID="lblSubCat" runat="server" AssociatedControlID="listSubCategory" Text="Subcategory:" CssClass=""></asp:Label>
        <asp:DropDownList ID="listSubCategory" runat="server" />
        <input id="hidden_listSubCategory" type="hidden" runat="server" />
        <cc1:CascadingDropDown ID="CascadingDropDown2" runat="server" Category="subcategory"
            TargetControlID="listSubCategory" ParentControlID="listCategory" PromptText="[Select SubCategory]"
            LoadingText="Loading subcategories..." ServicePath="~/WS/ws_category.asmx" ServiceMethod="GetSubCategoryBySubID">
        </cc1:CascadingDropDown>
    </div>
</div>
<div class="editboxrow">
    <div class="editbox">
        <asp:Label ID="lblSubSubCat" runat="server" AssociatedControlID="listSubSubCategory" Text="Subsubcategory:" CssClass=""></asp:Label>
        <asp:DropDownList ID="listSubSubCategory" runat="server" />
        <input id="hidden_listSubSubCategory" type="hidden" runat="server" />
        <cc1:CascadingDropDown ID="CascadingDropDown3" runat="server" Category="subsubcategory"
            TargetControlID="listSubSubCategory" ParentControlID="listSubCategory" PromptText="[Select SubSubCategory]"
            LoadingText="Loading subsubcategories..." ServicePath="~/WS/ws_category.asmx"
            ServiceMethod="GetSubSubCategoryBySubID">
        </cc1:CascadingDropDown>
    </div>
</div>
<div class="editboxrow">
    <div class="recordckbox editbox">
        <asp:CheckBox ID="chkboxWYNTK" runat="server" Text="WYNTK Collections" CssClass="" />
    </div>
    <div class="recordckbox editbox">
        <asp:CheckBox ID="chkboxWYNTKSpanish" runat="server" Text="WYNTK Spanish Collections" CssClass="" />
    </div>
</div>
<div class="editboxrow">
    <div class="editbox">
        <asp:Label ID="lblSubj" runat="server" AssociatedControlID="ListSubject" Text="Subject:" CssClass=""></asp:Label>
        <asp:DropDownList ID="ListSubject" runat="server" />
        <input id="hidden_ListSubject" type="hidden" runat="server" />
    </div>
</div>
