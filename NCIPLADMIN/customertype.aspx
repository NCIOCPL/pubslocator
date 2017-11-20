<%@ Page Title="Type of Customer" Language="C#" MasterPageFile="~/Template/Default2.Master" AutoEventWireup="true" CodeBehind="customertype.aspx.cs" Inherits="PubEntAdmin.customertype" EnableEventValidation="false" %>

<%--<asp:Content ID="contentHead" ContentPlaceHolderID="head" runat="server">
</asp:Content>--%>
<asp:Content ID="Content2" ContentPlaceHolderID="topContent" runat="server">
</asp:Content>
<asp:Content ID="contentMain" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="lookupmgmt">
        <h3>Add New Type of Customer</h3>
        <div class="lookupnew">
            <div class="lookupfld">
                <asp:Label ID="lblTypeofCustomer" runat="server" Text="Type of Customer (NCIPLcc)" AssociatedControlID="txtTypeofCustomer"></asp:Label>
                <asp:TextBox ID="txtTypeofCustomer" runat="server" MaxLength="50"></asp:TextBox>
            </div>
            <div class="lookupbtn">
                <asp:Button ID="btnAdd" runat="server" CssClass="btn" Text="Add"
                    OnClick="btnAdd_Click" />
            </div>
        </div>
        <h3>Edit Existing Type of Customer</h3>
        <!--Begin Grid-->
        <asp:GridView ID="grdVwTypeofCustomer" runat="server"
            AutoGenerateColumns="False" OnRowCommand="grdVwTypeofCustomer_RowCommand"
            OnRowDataBound="grdVwTypeofCustomer_RowDataBound"
            OnRowEditing="grdVwTypeofCustomer_RowEditing"
            OnRowUpdating="grdVwTypeofCustomer_RowUpdating"
            OnRowCancelingEdit="grdVwTypeofCustomer_RowCancelingEdit"
            GridLines="Horizontal" CssClass="valuestable gray-border" Width="100%">
            <HeaderStyle CssClass="rowHead" />
            <AlternatingRowStyle CssClass="rowOdd" />
            <RowStyle CssClass="rowEven" />
            <Columns>
                <asp:TemplateField AccessibleHeaderText="Type of Customer"
                    HeaderText="Type of Customer">
                    <ItemTemplate>
                        <asp:HiddenField ID="hidCustId" runat="server" />
                        <asp:Label ID="lblCustType" runat="server" Text="Type of Customer"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtCustType" runat="server" MaxLength="50"></asp:TextBox>
                        <asp:HiddenField ID="hidCustId" runat="server" />
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
                        <label for="<%#((GridViewRow)(Container)).FindControl("chkCC").ClientID%>" style="display: none">CC</label><asp:CheckBox ID="chkCC" runat="server" Style="text-align: center" />
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
