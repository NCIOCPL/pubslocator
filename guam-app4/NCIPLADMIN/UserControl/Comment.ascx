<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Comment.ascx.cs" Inherits="PubEntAdmin.UserControl.Comment" %>
<%@ Register Src="TextCtrl_SpellCk.ascx" TagName="TextCtrl_SpellCk" TagPrefix="uc1" %>
<script type="text/javascript" src="../SubmissionScript.js"></script>
<noscript>
    This site requires Javascript. Please enable Javascript in your browser or switch
    to a browser that supports Javascript in order to view this site.
</noscript>
<asp:Panel ID="pnlTop" runat="server"  CssClass="editboxrow commenttab">  
    <div class="editbox">     
                <asp:Label ID="lblComments" runat="server" Text="Comments:" ></asp:Label>
                <uc1:TextCtrl_SpellCk ID="txtComment" runat="server"  
        TurnOffValidator="true" TextMode="MultiLine" MaxLength="300"  />
    <asp:Button ID="btnAdd" Text="Add Comment" CssClass="standardText"
        runat="Server" OnClick="Comment_Click" Visible="false"></asp:Button><asp:Label ID="lblErrmsg" runat="server" Text="" CssClass="error"></asp:Label>
</div> </asp:Panel>
    <asp:DataGrid ID="grdComments" AutoGenerateColumns="False" Width="100%" 
          runat="Server"  OnItemDataBound="grdComments_ItemDataBound" UseAccessibleHeader="true" CssClass="gray-border valuestable commenttab">
        <ItemStyle VerticalAlign="Top"></ItemStyle>
        <Columns>
            <asp:TemplateColumn>
                <ItemTemplate>
                    <p>
                        <asp:Label ID="lblCreatorDisplayName" runat="Server" ></asp:Label> said on
                        <asp:Label ID="lblDateCreated" runat="Server"></asp:Label>:
                                               <asp:Panel ID="Panel1" runat="server">
                            <asp:Literal ID="ltlComment" runat="Server"></asp:Literal>
                        </asp:Panel>
                    </p>
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
        <PagerStyle HorizontalAlign="Center" Mode="NumericPages"></PagerStyle>
    </asp:DataGrid>
