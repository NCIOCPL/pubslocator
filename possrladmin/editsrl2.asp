<!--#include file="validation.asp"-->
<!--#include file="livehelp.inc" -->
<%
	for each item in Request.Form
		'Response.Write item & "=" & Request.Form(item) & "<br>"
	next
	
	Set Conn = Server.CreateObject("ADODB.Connection")
	Set rs = Server.CreateObject("ADODB.RecordSet")
	Conn.open DSN 
	conn.BeginTrans 
	
		rs.open "select * from NCI_POSSRL where autoid= " & Validate(Request.form("id"),VTYPENUMERIC,5,false),conn,1,3

		rs("indent") = Validate(Request.Form("indent"),VTYPENUMERIC,1,false)
		rs("title") = Validate(mid(Request.Form("title"),1,100),VTYPEGENERAL,100,false)
		rs("aCtive") = Validate(Request.Form("isactive"),VTYPENUMERIC,1,false)
		rs("update_dt") = now()
		rs("updated_by") = Validate(mid(Request.ServerVariables("LOGON_USER"),1,50),VTYPENAME,50,true)
		
		'rs("text") = mid(Request.Form("txt"),1,4000)
		rs("text") = Validate(Request.Form("txt"),VTYPEGENERAL,3000,false)
		'if rs("folder") = "1" then rs("text") = rs("title")

		rs.Update
		'***EAC need this to fix ADO bug with text > 2000 chars...
		conn.Execute("update NCI_POSSRL set text='" & replace(Validate(mid(Request.Form("txt"),1,3000),VTYPEGENERAL,3000,false),"'","''") & "' WHERE autoid=" & Request.form("id"))
	
	if conn.Errors.Count <> 0 then
		conn.RollbackTrans 
		Response.Write "Error! Problem saving data..."
		Response.End 
	else	
		conn.CommitTrans  
	end if 
	rs.close
	Response.Write "Update Successful! Redirecting to the main SRL page now..."
	Response.Write "<script>setTimeout('redir()',2000);function redir(){	window.location.href = 'srladmin.asp';}</script>"
	conn.Close 
	set conn=nothing
	set rs=nothing

%>

