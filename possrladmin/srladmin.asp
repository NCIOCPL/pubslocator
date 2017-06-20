<HTML>
<link rel="stylesheet" href="livehelp.css" type="text/css">
<HEAD>
</HEAD>
<!--#include file="livehelp.inc" -->

<style>
A{
	text-decoration : none; 
}
</style>
<body>
<table width="100%" border="0" cellspacing="0" cellpadding="0">
<tr bgcolor="#88949f">
<td align=left valign="top" ><font face="Verdana, Arial, Helvetica, sans-serif" color="#ffffff" size="+1">
	SRL Admin Page</font></td>
<td align=right>
	<!--a href="<%=urlGCN%>/custom/admin.jsp"><font face="Verdana, Arial, Helvetica, sans-serif" color="#ffffff" >Home</font></a-->
</td>
</table>
<%
	Set Conn = Server.CreateObject("ADODB.Connection")
	Set rs = Server.CreateObject("ADODB.RecordSet")
	Conn.open DSN 
	set rs =conn.execute("select * from NCI_POSSRL order by pos ")

	rs.MoveFirst 
	Response.Write "<form method=post action=reorder.asp onsubmit=return(submitit(this.form)) id=form1 name=form1><input type=button value='Update Order' id=submit2 name=submit2 onclick='this.disabled=true;  this.form.submit()'>"
	Response.Write "<table cellpadding=0 cellspacing=0 width=100% class=ectbl_result border=0>"
	Response.Write "<th>Pos<th>Title<th>Add Entry"
	n = 1
	while not rs.EOF 
		Response.Write "<tr height=20 valign=top><td><input alt='pos" & n & "' onchange='return verifyit(this)' style='height:100%; width:100%; font-size:10' type=text size=2 name=srl" & rs("autoid") & " value=" & n & "></td><td>"
		for i= 1 to cint(rs("indent"))			 
			Response.Write "<img alt='spacer' src=img/empty.gif border=0 height=5 width=" & 32  & ">"
		next
		
		's = "<a onmouseout=""this.className=''"" onmouseover=""this.className='over'"" href=editsrl.asp?id=" & rs("autoid") & ">" & server.HTMLEncode(""&rs("title")) & "</a>"
		s = "<a href=editsrl.asp?id=" & rs("autoid") & ">" & server.HTMLEncode(""&rs("title")) & "</a>"

		if rs("folder") = "1" then
			s = "<b><img alt='folder' src=img/icdirectoryu.gif>" & s & "</b>"
		end if
		if rs("active") <> "1" then
			s = " <span style='font-style:italic;position:absolute;float:left;filter:alpha(opacity=25);-moz-opacity:.25;opacity:.25;'  title='.25 opacity' >" & s & "</span>"
		end if		
		Response.Write s & vbcrlf 
		
		Response.Write "<br><img alt='redline' border=0 src=img/redline.gif style='visibility:hidden' height=3  width=100% name=line" & n & ">"
		
		Response.Write "</td><td onmouseout=hideline(" & n & ")  onmouseover=showline(" & n & ") ><a  border=0 href=addsrl.asp?pos=" & n & ".5><img alt='here' border=0  src=img/inserthere.gif></a></td></tr>" 
		Response.Write "</tr>" & vbcrlf
		rs.MoveNext 
		n = n + 1
	wend
	Response.Write "</table>Total : " & n-1 & "<br>"
	conn.Close 
	set conn=nothing
	set rs=nothing

%>
<input type=button value='Update Order' id=submit1 name=submit1 onclick='this.disabled=true;  this.form.submit()'>
</form>
<script>
function submitit(frm){
	return(true);
}
function verifyit(obj){
	if (isNaN(parseFloat(obj.value))){
		alert('Please use a number');
		return (false);
	}
	obj.value = parseFloat(obj.value);
}
function showline(n){
	eval("document.line" + n +".style.visibility = 'visible';");
}
function hideline(n){
	eval("document.line" + n +".style.visibility = 'hidden';");
}

</script>
<p>
<!--a href="<%=urlGCN%>/custom/admin.jsp" onMouseOut="MM_swapImgRestore()" onMouseOver="MM_swapImage('back','','img/mainPageActive.gif',1)">
<img alt='main' name="back"  src="img/mainPage.gif" width="30" height="30" border="0"></a-->
</body>