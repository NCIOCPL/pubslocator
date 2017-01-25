<%--<%@ Control Language="c#" AutoEventWireup="false" Codebehind="CheckList.ascx.cs" Inherits="PubEnt.usercontrols.CheckList" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script>
function Left2Right(o)
{
        //alert("dvCBRight_" + o);
	var noel="";
		var temp="";
		var dv = document.getElementById("dvCBRight_" + o);		
		dv.innerHTML = "";
		var chkBoxList = document.getElementById(o + "_CheckBoxListLeft");
		var chkBoxCount= chkBoxList.getElementsByTagName("input");
		
		var lbls= chkBoxList.getElementsByTagName("label");
		var txtLeft= document.getElementById(o + "_TextLeft");
		eval("var arrLeft=new Array(" + txtLeft.value + ");")

        for(var i=0;i<chkBoxCount.length;i++)
        {
			var it = document.getElementById(o + "_CheckBoxListLeft_" + i);
			if (it.checked){
				if (temp != "") 
					temp += "," + arrLeft[i];
				else
					temp = arrLeft[i];
				noel += "&nbsp;&nbsp;&nbsp;&nbsp;" + lbls[i].innerHTML + '<br>';
			}
        }	
		tv = document.getElementById(o + "_TextValues");
		tv.value = temp;
dv.innerHTML = noel;
}
</script>
<body>
<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="1" 
    style="border: 1px solid #111111;">
	<TR>
		<TD width="50%">
			<div id="dvCBLeft_<%=this.ClientID%>" style="OVERFLOW: auto; HEIGHT: 75px" 
      ><asp:checkboxlist id="CheckBoxListLeft" Width="95%" runat="server"  RepeatColumns="1"
					BorderColor="DarkGray" BorderStyle="None" RepeatLayout="Flow">
					<asp:ListItem Value="All">All</asp:ListItem>
				</asp:checkboxlist></div>
			<div style="DISPLAY:none" id="dvHidden">
			    <asp:Label runat="server" ID="lblTextLeft" AssociatedControlID="TextLeft" Text="" CssClass="hidden-label" />
			    <asp:textbox id="TextLeft" runat="server"></asp:textbox>
			    <asp:Label runat="server" ID="lblTextValues" AssociatedControlID="TextValues" Text="" CssClass="hidden-label" />
			    <asp:textbox id="TextValues" runat="server"></asp:textbox>
		    </div>
		</TD>
		<TD width="50%" valign=top>
			<div id="dvCBRight_<%=this.ClientID%>" style="OVERFLOW:auto;">&nbsp</div>
			<asp:checkboxlist id="CheckBoxListRight" Width="100%" runat="server" BorderColor="Gray" BorderStyle="Dashed"
				RepeatLayout="Flow" Visible="False">
				<asp:ListItem Value="Dummy">Dummy</asp:ListItem>
			</asp:checkboxlist></TD>
	</TR>
</TABLE>
<body>--%>