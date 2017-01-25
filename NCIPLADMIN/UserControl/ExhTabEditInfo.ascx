<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExhTabEditInfo.ascx.cs" Inherits="PubEntAdmin.UserControl.ExhTabEditInfo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="TextCtrl_SpellCk.ascx" TagName="TextCtrl_SpellCk" TagPrefix="uc1" %>
<%@ Register TagPrefix="uc1" TagName="ListMultiSelect" Src="ListMultiSelect.ascx" %>
<%@ Register Assembly="PubEntAdmin" Namespace="PubEntAdmin.CustomControl" TagPrefix="cc2" %>

<script type="text/javascript">
    // move items among three listboxes    
    function MoveItem(seq) {
        switch (seq) {
            case '1':
                var Source = document.getElementById('<%=this.listConf.ClientID %>');
                var Target = document.getElementById('<%=this.listSeledConf.ClientID %>');
                break;
            case '2':
                var Source = document.getElementById('<%=this.listSeledConf.ClientID %>');
                var Target = document.getElementById('<%=this.listConf.ClientID %>');
                var Source1 = document.getElementById('<%=this.listRotate.ClientID %>');
                break;
            case '3':
                var Source = document.getElementById('<%=this.listSeledConf.ClientID %>');
                var Target = document.getElementById('<%=this.listRotate.ClientID %>');
                break;
            case '4':
                var Source = document.getElementById('<%=this.listRotate.ClientID %>');
                var Target = document.getElementById('<%=this.listSeledConf.ClientID %>');
                break;
        }
        if ((Source != null) && (Target != null)) {
            for (var i = 0; i < Source.options.length; i++) {
                if (Source.options[i].selected) {
                    if (seq != 4) {
                        if (seq == 3) { // avoid duplicate records
                            if (Target.options.length > 0) {
                                var a = 0;
                                for (var j = 0; j < Target.options.length; j++) {
                                    if (Target.options[j].value == Source.options[i].value) {
                                        a++;
                                    }
                                }
                                if (a == 0)
                                    Target.options.add(new Option(Source.options[i].text, Source.options[i].value))
                            }
                            else
                                Target.options.add(new Option(Source.options[i].text, Source.options[i].value))
                            // reset textbox value
                            document.getElementById('<%=this.txtRotate.ClientID %>').value = "";
                            for (var l = 0; l < Target.options.length; l++) {
                                document.getElementById('<%=this.txtRotate.ClientID %>').value += Target.options[l].value + ',';
                            }
                        } //seq==3
                        else
                            Target.options.add(new Option(Source.options[i].text, Source.options[i].value))
                    } //seq !=4
                    // reset textbox value
                    if (seq == 1) {
                        document.getElementById('<%=this.txtSeledConf.ClientID %>').value = "";
                        for (var l = 0; l < Target.options.length; l++) {
                            document.getElementById('<%=this.txtSeledConf.ClientID %>').value += Target.options[l].value + ',';
                        }
                    }
                    if (seq != 3) {
                        if (seq == 2) {
                            if (Source1.options.length > 0) {
                                var a = 0;
                                for (var j = 0; j < Source1.options.length; j++) {
                                    if (Source1.options[j].value == Source.options[i].value) {
                                        Source1.remove(j--);
                                        Source.remove(i--);
                                        a++;
                                    }
                                }
                                if (a == 0)
                                    Source.remove(i--);
                            }
                            else
                                Source.remove(i--);
                        } // seq==2
                        else
                            Source.remove(i--);
                        // reset textbox value
                        if (seq == 2) {
                            document.getElementById('<%=this.txtSeledConf.ClientID %>').value = "";
                            for (var l = 0; l < Source.options.length; l++) {
                                document.getElementById('<%=this.txtSeledConf.ClientID %>').value += Source.options[l].value + ',';
                            }
                            // reset textbox value
                            document.getElementById('<%=this.txtRotate.ClientID %>').value = "";
                            for (var l = 0; l < Source1.options.length; l++) {
                                document.getElementById('<%=this.txtRotate.ClientID %>').value += Source1.options[l].value + ',';
                            }
                        }
                        // reset textbox value
                        if (seq == 4) {
                            document.getElementById('<%=this.txtRotate.ClientID %>').value = "";
                            for (var l = 0; l < Source.options.length; l++) {
                                document.getElementById('<%=this.txtRotate.ClientID %>').value += Source.options[l].value + ',';
                            }
                        }
                    } //seq !=3
                }
            }  // end loop 
            if (Source != null) {
                Source.options.selectedIndex = -1;
            }
            if (Target != null) {
                Target.options.selectedIndex = -1;
            }
            if (Source1 != null) {
                Source1.options.selectedIndex = -1;
            }
        }  // source !=null       
    }
</script>
<div class="ckboxrow">
    <div class="editbox">
        <asp:Label ID="lblDisStatus" runat="server" Text="Display Status" CssClass=""></asp:Label>
        <cc2:CkboxListDisplayStatus ID="ckboxListDisplayStatusExh" runat="server" CssClass="recordckbox ckbox3" RepeatDirection="Horizontal"></cc2:CkboxListDisplayStatus>
    </div>
</div>
<div class="editboxrow">
    <div class="editbox">
        <asp:Label ID="lblMaxQty" runat="server" Text="Max Qty./Exhibit" AssociatedControlID="txtMaxQtyExh" CssClass=""></asp:Label>
        <asp:TextBox ID="txtMaxQtyExh" runat="server" MaxLength="8"></asp:TextBox>
        <cc1:FilteredTextBoxExtender ID="ftxteMaxQtyExh" runat="server" TargetControlID="txtMaxQtyExh"
            FilterType="Numbers">
        </cc1:FilteredTextBoxExtender>
    </div>
    <div class="editbox">
        <asp:Label ID="lblMaxQtyIntl" runat="server" Text="Max Qty./International" AssociatedControlID="txtMaxQtyIntl" CssClass=""></asp:Label>
        <asp:TextBox ID="txtMaxQtyIntl" runat="server" MaxLength="8"></asp:TextBox>
        <cc1:FilteredTextBoxExtender ID="ftbeMaxQtyIntl" runat="server" TargetControlID="txtMaxQtyIntl"
            FilterType="Numbers">
        </cc1:FilteredTextBoxExtender>
    </div>
</div>
<div class="editboxrow">
    <div class="editbox">
        <asp:Label ID="lblInclEveryOrder" runat="server" Text="Include in Every Order" CssClass=""></asp:Label>
        <cc2:RdbtnListYesNo ID="rdbtnListYesNoEveryOrder" CssClass="recordckbox rdio" runat="server" RepeatDirection="Horizontal"></cc2:RdbtnListYesNo>
    </div>
    <div class="editbox">
        <asp:Label ID="lblShowInSrchRes" runat="server" Text="Show in Search Results" CssClass=""></asp:Label>
        <cc2:RdbtnListYesNo ID="rdbtnListYesNoShowInSearchRes" CssClass="recordckbox rdio" runat="server" RepeatDirection="Horizontal"></cc2:RdbtnListYesNo>
    </div>
</div>
<asp:Panel ID="pnlkiosk" runat="server" CssClass="editboxrow" Visible="false">
    <div class="editbox">
        <asp:Label ID="lblConf" AssociatedControlID="listConf" runat="server" Text="Conferences" CssClass=""></asp:Label>
        <uc1:ListMultiSelect ID="listConf" Rows="6" TurnOffValidator="true" DisplayDefault="false"
            SelectionMode="Multiple" runat="server" CssClass="MultiSelect"></uc1:ListMultiSelect>
        <div class="exhbtns">
            <input id="btnR1" onclick="Javascript: MoveItem('1');" type="button" runat="server" value="->" />
            <input id="btnL1" onclick="Javascript: MoveItem('2');" runat="server" type="button" value="<-" />
        </div>
    </div>
    <div class="editbox">
        <asp:Label ID="lblSeledConf" AssociatedControlID="listSeledConf" runat="server" Text="Selected Conferences" CssClass=""></asp:Label>
        <uc1:ListMultiSelect ID="listSeledConf" Rows="6" TurnOffValidator="true" DisplayDefault="false"
            SelectionMode="Multiple" runat="server" CssClass="MultiSelect"></uc1:ListMultiSelect>
        <div class="exhbtns">
            <input onclick="Javascript: MoveItem('3');" type="button" id="btnR2" value="->" />
            <input onclick="Javascript: MoveItem('4');" type="button" id="btnL2" value="<-" />
        </div>
    </div>
    <div class="editbox">
        <asp:Label ID="lblRotate" runat="server" Text="Rotate" CssClass="" AssociatedControlID="listRotate"></asp:Label>
        <uc1:ListMultiSelect ID="listRotate" Rows="6" TurnOffValidator="true" DisplayDefault="false"
            SelectionMode="Multiple" runat="server" CssClass="MultiSelect"></uc1:ListMultiSelect>
        <asp:Label ID="lblErrorRotate" runat="server" CssClass="errorText" Font-Size="Smaller"></asp:Label>
        <asp:TextBox ID="txtSeledConf" runat="server" CssClass="Invisible"></asp:TextBox>
        <asp:TextBox ID="txtRotate" runat="server" CssClass="Invisible"></asp:TextBox>
    </div>
</asp:Panel>

