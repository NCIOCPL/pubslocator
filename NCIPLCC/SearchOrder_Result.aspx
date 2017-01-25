<%@ Page Title="" Language="C#" MasterPageFile="~/pubmaster.Master" AutoEventWireup="true"
    CodeBehind="SearchOrder_Result.aspx.cs" Inherits="PubEnt.SearchOrder_Result" %>

<%@ MasterType VirtualPath="~/pubmaster.master" %>
<%@ Register Src="usercontrols/searchbar_search.ascx" TagName="searchbar" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <uc2:searchbar ID="searchbar" runat="server" />
    </div>
    <div id="searchorderresult" class="indentwrap">
        <a id="skiptocontent" tabindex="-1"></a>
        <h2>
            <asp:Label ID="Label1" runat="server" Text="Order Lookup Search Results"></asp:Label>
        </h2>
        <div id="divRecNavigation" class="criteria" runat="server">
            <h3>
                Search Criteria:</h3>
            <div id="divStartDate" runat="server" visible="false">
                Start Date:&nbsp;<asp:Label ID="lblStartDate" Text="" runat="server"></asp:Label></div>
            <div id="divEndDate" runat="server" visible="false">
                End Date:&nbsp;<asp:Label ID="lblEndDate" Text="" runat="server"></asp:Label></div>
            <div id="divTypeofCustomer" runat="server" visible="false">
                Type of Customer:&nbsp;<asp:Label ID="lblTypeofCustomer" Text="" runat="server"></asp:Label></div>
            <div id="divSearchPhase" runat="server" visible="false">
                Search Phrase:&nbsp;<asp:Label ID="lblSearchPhase" Text="" runat="server"></asp:Label></div>
            <div class="nav">
                <div>
                    <strong>Total Records Found:&nbsp;<asp:Label ID="lblRecordCount" runat="server" Text="Total Records Found"></asp:Label></strong>
                </div>
                <div>
                    <asp:Label ID="lblRecordShow" runat="server" Text="Records Shown:" AssociatedControlID="drpQJumpsN"></asp:Label>
                    <asp:DropDownList ID="drpQJumpsN" runat="server" AutoPostBack="true" class="select0"
                        OnSelectedIndexChanged="drpQJumpsN_Changed">
                    </asp:DropDownList>
                </div>
                <div class="pager">
                    <strong>
                        <%=strNav%></strong>
                </div>
            </div>
        </div>
        <div id="divSearchOrderResult" runat="server">
            <table class="orderresults">
                <tr>
                    <th scope="col">
                        Name
                    </th>
                    <th scope="col">
                        Organization
                    </th>
                    <th scope="col">
                        Email
                    </th>
                    <th scope="col">
                        Order #
                    </th>
                    <th scope="col">
                        Date Created
                    </th>
                </tr>
                <asp:Literal ID="ltrlSearchOrderResult" runat="server"></asp:Literal>
            </table>
        </div>
        <div id="divRecNavigationBtm" runat="server">
            <div class="pager bottom">
                <strong>
                    <%=strNav%></strong></div>
        </div>
        <div id="divNoRecord" style="display: none" runat="server">
            <p>
                No Records Found</p>
        </div>
    </div>
    <asp:HiddenField ID="hidCustID" Value="" runat="server" /> <asp:HiddenField ID="hidOrderNum"
    Value="" runat="server" /> <asp:HiddenField ID="hidPrevPage" Value="" runat="server"
    />
</asp:Content>
