<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchResult.aspx.cs" Inherits="PubEntAdmin.SearchResult"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Assembly="PubEntAdmin" Namespace="PubEntAdmin.CustomControl" TagPrefix="cc1" %>
<%@ Reference Page="Home.aspx" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <link href="CSS/Common.css" rel="stylesheet" type="text/css" />
    <title>Search Result</title>
    <script type="text/javascript">
        function SelectAllCheckboxes(spanChk) {
            // Added as ASPX uses SPAN for checkbox
            var oItem = spanChk.children;
            var theBox = (spanChk.type == "checkbox") ? spanChk : spanChk.children.item[0];
            xState = theBox.checked;
            elm = theBox.form.elements;
            for (i = 0; i < elm.length; i++) {
                if (elm[i].type == "checkbox" && elm[i].id != theBox.id) {
                    //elm[i].click();
                    if (elm[i].checked != xState)
                        elm[i].click();
                    //elm[i].checked=xState;
                }
            }
        }
    </script>
    <noscript>
        This site requires Javascript. Please enable Javascript in your browser or switch to a browser 
			that supports Javascript in order to view this site.
    </noscript>
</head>
<body class="Admin">
    <div class="skip"><a href="#mainBody">Skip to content</a></div>
    <div id="container">
        <h1>Publications Enterprise Admin Tool</h1>
        <h2> Search Results</h2>
        <div id="mainBody">
            <form id="formSearchResult" runat="server">
                <asp:ScriptManager ID="ScriptManager_SrchRslt" EnableScriptGlobalization="false"
                    runat="Server">
                </asp:ScriptManager>
                <asp:PlaceHolder ID="plcHldMenu" runat="server"></asp:PlaceHolder>
                <div id="serp">
                    <div class="serpcritregion">
                        Showing
                    <asp:Label ID="lblTotalResCnt" runat="server" Text=""></asp:Label>
                        Results for
                    <br />
                        <div class="serpcrit">
                            <asp:Label ID="lblSearchCriteria" runat="server" Text=""></asp:Label>
                        </div>
                    </div>
                    <div class="reflinks">
                        <asp:HyperLink ID="hyplnkRefSrch1" runat="server" Text="Refine Search"></asp:HyperLink>
                        <a href="Home.aspx" id="hyplnkSrch1" runat="server">New Search</a>
                    </div>
                    <div runat="server" id="trTopCtrlPnl" class="serpcontrols">
                        <asp:ImageButton ID="ImgBtnExportSchRsltToExcel2" runat="server" AlternateText="Export Search Result to Excel"
                            ImageUrl="Image/excelicon.gif" ImageAlign="Bottom" OnClick="ImgBtnExportSchRsltToExcel_OnClick" />
                        <asp:Button ID="btnDropSel2" runat="server" Text="Drop Selection(s)" OnClick="btnDropSel_Click" />
                        <asp:Button ID="btnGetRrd2" runat="server" Text="Goto Record(s)" OnClick="btnGetRcd_Click" Visible="false" />
                        <asp:Button ID="btnKeepSel2" runat="server" Text="Keep Selection(s)" OnClick="btnKeepSel_Click" />
                    </div>
                    <asp:DataGrid ID="gvResult" runat="server" AutoGenerateColumns="False" CssClass="gray-border valuestable"
                        Width="100%" AllowSorting="True" HorizontalAlign="Center"
                        PageSize="4" OnItemDataBound="gvResult_ItemDataBound" ShowFooter="True"
                        OnSortCommand="gvResult_SortCommand" UseAccessibleHeader="True">
                        <HeaderStyle CssClass="rowHeadDefault" />
                        <AlternatingItemStyle CssClass="rowOdd" />
                        <ItemStyle CssClass="rowEven" />
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="pubid"></asp:BoundColumn>
                            <asp:BoundColumn Visible="True" DataField="rank" SortExpression="Rank" HeaderText="Rank"></asp:BoundColumn>
                            <asp:BoundColumn Visible="True" DataField="prodid" HeaderText="Publication ID" SortExpression="PRODUCTID"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Publication Title" SortExpression="LONGTITLE" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:TemplateColumn>
                            <asp:BoundColumn Visible="True" DataField="qtyAvailable" HeaderText="Qty. Available"
                                SortExpression="QUANTITY_AVAILABLE"></asp:BoundColumn>
                            <asp:BoundColumn Visible="True" DataField="status" HeaderText="Book Status" SortExpression="STATUS"></asp:BoundColumn>
                            <asp:BoundColumn Visible="True" DataField="CreateDate" HeaderText="Date Created"
                                SortExpression="RECORDCREATEDATE" DataFormatString="{0:MM/dd/yyyy}"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="QC Report">
                                <ItemTemplate>
                                    <asp:HyperLink runat="server" NavigateUrl="" Target="_blank" Text="" ID="QCLink">
                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/Image/wordicon.gif" AlternateText="QC Report" />
                                    </asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="fdsfsd">
                                <HeaderTemplate>
                                    <label for="chkAll">Select All</label><br />
                                    <input id="chkAll" name="chkAll" onclick="javascript: SelectAllCheckboxes(this);"
                                        type="checkbox" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <label for="<%#((DataGridItem)(Container)).FindControl("chkSelect").ClientID%>" style="display: none">Select</label><asp:CheckBox ID="chkSelect" runat="server" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <label for="chkAll2">Select All</label><br />
                                    <input id="chkAll2" name="chkAll2" onclick="javascript: SelectAllCheckboxes(this);"
                                        type="checkbox" />
                                </FooterTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                    <div runat="server" id="trBtmCtrlPnl" class="serpcontrols">
                        <asp:Button ID="btnGetRcd" runat="server" Text="Goto Record(s)" OnClick="btnGetRcd_Click" Visible="false" />
                        <asp:ImageButton ID="ImgBtnExportSchRsltToExcel1" runat="server" AlternateText="Export Search Result to Excel"
                            ImageUrl="Image/excelicon.gif" ImageAlign="Bottom" OnClick="ImgBtnExportSchRsltToExcel_OnClick" />
                        <asp:Button ID="btnDropSel" runat="server" Text="Drop Selection(s)" OnClick="btnDropSel_Click" />
                        <asp:Button ID="btnKeepSel" runat="server" Text="Keep Selection(s)" OnClick="btnKeepSel_Click" />
                    </div>
                    <div class="reflinks reflinksbot">
                        <asp:HyperLink ID="hyplnkRefSrch2" runat="server" Text="Refine Search"></asp:HyperLink><a
                            href="Home.aspx" id="hyplnkSrch2" runat="server">New Search</a>
                    </div>
                </div>
            </form>
        </div>
    </div>
</body>
</html>
