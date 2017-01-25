<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Default2.Master" AutoEventWireup="true" CodeBehind="ordermedia.aspx.cs" Inherits="PubEntAdmin.ordermedia" %>

<%--<asp:Content ID="contentHead" ContentPlaceHolderID="head" runat="server">
</asp:Content>--%>
<asp:Content ID="Content2" ContentPlaceHolderID="topContent" runat="server">
</asp:Content>
<asp:Content ID="contentMain" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="lookupmgmt">
        <h3>Add New Order Source</h3>
        <div class="lookupnew">
            <div class="lookupfld">
                <asp:Label ID="lblOrderMedia" runat="server" Text="Order Source (NCIPLcc)" AssociatedControlID="txtOrderMedia"></asp:Label>
                <asp:TextBox ID="txtOrderMedia" runat="server" MaxLength="50"></asp:TextBox>
            </div>
            <div class="lookupbtn">
                <asp:Button ID="btnAdd" runat="server" CssClass="btn" Text="Add"
                    OnClick="btnAdd_Click" />
            </div>
        </div>
        <!--Begin Grid-->
        <h3>Edit Existing Order Source</h3>
        <asp:GridView ID="grdVwOrderMedia" runat="server"
            AutoGenerateColumns="False" OnRowCommand="grdVwOrderMedia_RowCommand"
            OnRowDataBound="grdVwOrderMedia_RowDataBound"
            OnRowEditing="grdVwOrderMedia_RowEditing"
            OnRowUpdating="grdVwOrderMedia_RowUpdating"
            OnRowCancelingEdit="grdVwOrderMedia_RowCancelingEdit"
            GridLines="Horizontal" CssClass="gray-border valuestable"
            Width="100%">
            <HeaderStyle CssClass="rowHead" />
            <AlternatingRowStyle CssClass="rowOdd" />
            <RowStyle CssClass="rowEven" />
            <Columns>
                <asp:TemplateField AccessibleHeaderText="Order Source"
                    HeaderText="Order Source" HeaderStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:HiddenField ID="hidOrderMediaId" runat="server" />
                        <asp:Label ID="lblOrderMedia" runat="server" Text="Order Source"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtOrderMedia" runat="server" MaxLength="50"></asp:TextBox>
                        <asp:HiddenField ID="hidOrderMediaId" runat="server" />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField AccessibleHeaderText="Status" HeaderText="Status">
                    <ItemTemplate>
                        <asp:Label ID="lblStatus" runat="server" Text="Label"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:CommandField ButtonType="Button" ItemStyle-CssClass="btnparent" ShowEditButton="True" />
                <asp:ButtonField ButtonType="Button" ItemStyle-CssClass="btnparent" Text="Activate" />
                <asp:TemplateField HeaderText="CC">
                    <ItemTemplate>
                        <label for="<%#((GridViewRow)(Container)).FindControl("chkCC").ClientID%>" style="display: none">CC</label><asp:CheckBox ID="chkCC" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="POS">
                    <ItemTemplate>
                        <label for="<%#((GridViewRow)(Container)).FindControl("chkPOS").ClientID%>" style="display: none">POS</label><asp:CheckBox ID="chkPOS" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="LM">
                    <ItemTemplate>
                        <label for="<%#((GridViewRow)(Container)).FindControl("chkLM").ClientID%>" style="display: none">LM</label><asp:CheckBox ID="chkLM" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <!--CommandName for buttonfield specified at runtime-->
    </div>
</asp:Content>
