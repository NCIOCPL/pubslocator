<!--#include file="validation.asp"-->
<!--#include file="livehelp.inc" -->
<%
	for each item in Request.Form
		'Response.Write item & "=" & Request.Form(item) & "<br>"
	next

	dim moms(10)
	dim inds(10)
	moms(0) = 0
	inds(0) = -999
	dim m
	m=0
	text2 = ""
	Set Conn = Server.CreateObject("ADODB.Connection")
	Set rs = Server.CreateObject("ADODB.RecordSet")
	Conn.open DSN 
	conn.Execute "insert into NCI_POSHISTORY(staff,action,modify_src) values('" & Validate(mid(Request.ServerVariables("LOGON_USER"),1,50),VTYPENAME,50,true) & "','REORDER " & "','" & Request.ServerVariables("REMOTE_ADDR") & "')"

	'***EAC dirty way to reorder the SRL but it works...TODO:use update/select	
	conn.BeginTrans 
	rs.open "select * from NCI_POSSRL order by pos asc",conn,1,3
	
		while not rs.EOF
			rs("pos") = Validate(Request.Form("srl"&rs("autoid")),VTYPENUMERIC,5,false)
			rs.movenext 
		wend 
	
		rs.close

		rs.open "select * from NCI_POSSRL order by pos asc",conn,1,3

		i = 1
		while not rs.EOF
			rs("pos") = i
			
			'***EAC we need to figure out the mother of this record
			
			if m=0 then 
				rs("mom") = 0
			else
				if cint(rs("indent")) > cint(inds(m)) then
					rs("mom") = moms(m)
				else
					while (cint(rs("indent")) <= cint(inds(m)))
						m = m - 1
					wend
					rs("mom") = moms(m)
					
				end if
			end if
			
			'***EAC lets update the moms table.  "m" is the count of "open" folders
			if rs("folder") = "1" then
				m = m + 1
				moms(m) = rs("pos")
				inds(m) = rs("indent")				
			end if


			'***EAC update TEXT2 (ancestors)..note that this is using POS instead of AUTOID now
			rs("text2") = ""			
			for j=0 to m
				rs("text2") = rs("text2")  & moms(j) & ","
			next
			'Response.Write "<br>POS:" & rs("pos") & " MOM:" & rs("mom") 
			'if rs("folder") = "1" then Response.Write " FOLDER:" & rs("folder")
			rs.movenext
			i=i+1
		wend 

	

	if conn.Errors.Count <> 0 then
		conn.RollbackTrans 
		Response.Write "Error! Problem saving data..."
		Response.End 
	else			
		conn.CommitTrans  
	end if 
	rs.close




	Response.Write "Reorder Successful! Redirecting to the main SRL page now..."
	Response.Write "<script>setTimeout('redir()',2000);function redir(){	window.location.href = 'srladmin.asp';}</script>"
	
	conn.Close 
	set conn=nothing
	set rs=nothing

%>

