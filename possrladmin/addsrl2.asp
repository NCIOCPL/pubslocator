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

	Set Conn = Server.CreateObject("ADODB.Connection")
	Set rs = Server.CreateObject("ADODB.RecordSet")
	Conn.open DSN 
	conn.BeginTrans 
		rs.open "select * from NCI_POSSRL ",conn,1,3

		rs.AddNew 
		rs("pos") = Validate(Request.Form("pos"),VTYPENUMERIC,5,false)
		rs("indent") = Validate(Request.Form("indent"),VTYPENUMERIC,1,false)
		rs("folder") = Validate(Request.Form("isfolder"),VTYPENUMERIC,1,false)
		rs("title") = Validate(mid(Request.Form("title"),1,100),VTYPEGENERAL,100,false)
		rs("aCtive") = Validate(Request.Form("isactive"),VTYPENUMERIC,1,false)
		rs("created_by") = Validate(mid(Request.ServerVariables("LOGON_USER"),1,50),VTYPENAME,50,true)
		rs("text") = Validate(mid(Request.Form("txt"),1,3000),VTYPEGENERAL,3000,false)
		'if rs("folder") = "1" then rs("text") = rs("title")
		rs.Update
		newid = rs("autoid")
		'***EAC need this to fix ADO bug with text > 2000 chars...
		conn.Execute("update NCI_POSSRL set text='" & replace(Validate(mid(Request.Form("txt"),1,3000),VTYPEGENERAL,3000,false),"'","''") & "' WHERE autoid=" & newid)

		rs.Close 
		rs.open "select * from NCI_POSSRL order by pos,title ",conn,1,3

		
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

	Response.Write "Update Successful! Redirecting to the main SRL page now..."
	Response.Write "<script>setTimeout('redir()',2000);function redir(){	window.location.href = 'srladmin.asp';}</script>"
	conn.Close 
	set conn=nothing
	set rs=nothing

%>

