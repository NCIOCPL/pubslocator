<HTML>
<link rel="stylesheet" href="livehelp.css" type="text/css">
<HEAD>
</HEAD>
<!--#include file="validation.asp"-->
<!--#include file="livehelp.inc" -->

<%

	Response.write "<form method=post onsubmit=return(submitit(this)) action=addsrl2.asp>"
	Response.write "<input type=hidden name=pos value=" & Request.querystring("pos") & ">"
	Response.Write "<table width=650 class=ectbl_result border=0>"
	Response.Write "<tr><td width=50>INDENT: <td><select name=indent>"
%>
		<option	value=0		>0
		<option	value=1		>1
		<option	value=2		>2
		<option	value=3	>3
		<option	value=4		>4		
	<tr><td>FOLDER: <td><select name=isfolder>
		<option value=1 >Yes
		<option value=0 selected         >No
	</select>

<%
	Response.Write "</select>"
	
	Response.Write "<tr><td width=550><LABEL for='title'>TITLE:</label> <td><input type=text size=50 maxlength=100 id=title name=title value=''>"


	Response.Write "<tr><td><LABEL for='txt'>TEXT:</label> <td><textarea onKeyUp=updCount(this) name=txt rows=10 cols=60></textarea>"

	Response.Write "<tr><td>ACTIVE: <td><select name=isactive>"
		Response.Write "<option value=1 >Yes"
		Response.Write "<option value=0         >No"
	Response.Write "</select>"
	Response.Write "</table>"
	Response.Write "<input type=submit value='Add SRL Entry'>"
	Response.Write "</form>"
	
	

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
</HTML>
