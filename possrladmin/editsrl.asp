<HTML>
<link rel="stylesheet" href="livehelp.css" type="text/css">
<HEAD>
</HEAD>
<!--#include file="livehelp.inc" -->
<%
	Set Conn = Server.CreateObject("ADODB.Connection")
	Set rs = Server.CreateObject("ADODB.RecordSet")
	Conn.open DSN 

	set rs =conn.execute("select * from NCI_POSSRL where autoid= " & Request.QueryString("id"))
	
	'response.Write "<pre>" & rs("text") & "</pre>"
'	if not rs.EOF then
'		A =  rs.GetRows 
'		temprs = array2table(rs,A,title,"Printed : " & date(),"",650,0,0)
'	end if

	Response.write "<form method=post onsubmit=return(submitit(this)) action=editsrl2.asp>"
	Response.write "<input type=hidden name=id value=" & rs("autoid") & ">"
	Response.Write "<table width=650 class=ectbl_result border=0>"
	'Response.Write "<tr><td width=50>INDENT: <td><input type=text size=4 maxlength=6 name=indent value='" & rs("indent") & "'>"
	Response.Write "<tr><td width=50>INDENT: <td><select name=indent>"
%>
		<option	value=0	<%if rs("indent") = "0" then Response.Write "selected"%>	>0
		<option	value=1	<%if rs("indent") = "1" then Response.Write "selected"%>	>1
		<option	value=2	<%if rs("indent") = "2" then Response.Write "selected"%>	>2
		<option	value=3	<%if rs("indent") = "3" then Response.Write "selected"%>	>3
		<option	value=4	<%if rs("indent") = "4" then Response.Write "selected"%>	>4		
<%
	Response.Write "</select>"

	Response.Write "<tr><td width=550><label for='title'>TITLE:</label> <td><input type=text size=50 maxlength=100 name=title value=""" & server.HTMLEncode (""&rs("title")) & """>"

if rs("folder") = "0" then
	Response.Write "<tr><td>TEXT: <td><textarea onKeyUp=updCount(this) name=txt rows=10 cols=60>" & server.htmlencode(""&rs("text")) 	& "</textarea>"
end if
	Response.Write "<tr><td>ACTIVE: <td><select name=isactive>"
		if rs("active") = "1" then
			Response.Write "<option value=1 selected>Yes"
			Response.Write "<option value=0         >No"
		else
			Response.Write "<option value=1         >Yes"
			Response.Write "<option value=0 selected>No"
		end if
		
		
	Response.Write "</select>"
	Response.Write "</table>"
	Response.Write "<input type=submit >"
	Response.Write "</form>"
	
	

	




	conn.Close 
	set conn=nothing
	set rs=nothing

%>
<p>
<a href="javascript:history.go(-1)" onMouseOut="MM_swapImgRestore()" onMouseOver="MM_swapImage('back','','img/mainPageActive.gif',1)">
<img alt='main' name="back"  src="img/mainPage.gif" width="30" height="30" border="0"></a>




<script>
	function submitit(frm){
		if (frm.title.value=='') {
			alert('No title');
			return false;
		}
	}
</script>