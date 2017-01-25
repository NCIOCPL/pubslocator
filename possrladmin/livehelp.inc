<!--#include file="connect.inc" -->
<%
	response.cachecontrol = "no-cache, no-Store"
	response.addheader "Pragma", "no-cache"
	response.expires = -1
	response.buffer = true
	session.Timeout = 120
	server.scriptTimeout = 6000


	LOGON_USER = trim(ucase(request.servervariables("LOGON_USER")))
	'*** EAC for the testing purposes....
	if Request.querystring("imp") <> "" then
		LOGON_USER = trim(ucase(Request.querystring("imp")))
	end if

	if LOGON_USER = "" then
		response.write "Access denied! Please logon properly to the CIS Portal (using your NT account)."
		response.end		
	else
		session("srl_login") = LOGON_USER		'***EAC im not really planning to use this sess var
	end if



	

function getUTCoffset()
dim conn,rs,hroffset
	Set Conn = Server.CreateObject("ADODB.Connection")
	Set rs = Server.CreateObject("ADODB.RecordSet")
	Conn.open DSNGEN 
	'***EAC let get the offset...code specific to ctilab2 and ctirocksql
	set rs =conn.execute("select @@version as vers")
	
	if instr(rs("vers"),"Microsoft SQL Server 7.00" ) > 0 then
		hroffset = 4	'***EAC give it anything since we are at dev
	else
		set rs =conn.execute("select datediff(hh,getdate(),getutcdate()) as offs")
		hroffset = cint(rs("offs"))
	end if
	rs.Close 
	getUTCoffset = hroffset
end function


function loadArray(A,SQL,title)
'Response.Write sql

	dim temprs
	set temprs =conn.execute(SQL)
	if not temprs.EOF then
		A =  temprs.GetRows 
		temprs = array2table(temprs,A,title,"","",650,0,0)
	end if
end function


function array2table(rs,ar,title1 ,title2,title3,wd,brow,bclick)

	numcols=ubound(ar,1)
	numrows=ubound(ar,2)	


'	Response.Write "<TABLE  bgcolor=white border=0 cellpadding=2 cellspacing=0 width=" & wd & ">"
'	Response.Write "<tr><TH colspan=" & numcols & " ><b>" & (Title1) & "</b></TH></tr>" 
'	if Title2<> "" then
'		Response.Write "<tr><TH colspan=" & numcols & " ><b>" & (Title2) & "</b></TH></tr>"
 '       End if
'	Response.Write "<tr><TH colspan=" & numcols & " ><b>" & (Title3) & "</b></TH></tr>" 
'	Response.Write "<tr><td colspan =" & numcols & "></td></tr>"  
'	Response.Write "</TABLE>" & chr(13)
 '       Response.write "<br>"

	Response.Write "<div name=dvTBLRESULTS id=dvTBLRESULTS><TABLE name=ecTable id=ecTable class='ectbl_result' border=0 cellpadding=2 cellspacing=0 width=" & wd & ">"

	
	if brow=1 then ss="<TR><TH bgcolor=CCCCCC>ROW</TH>"
	For k = 0 to numcols 
            Dim strName
            
            strName = replace(ucase(rs.fields(k).name),"_"," ") 
   
	    ss = ss & "<TH align=left bgcolor=CCCCCC>" & strName & "</TH>" & chr(13)
	next

	Response.Write ss




	FOR rowcounter= 0 TO numrows
		if rowcounter mod 2 = 0 then 
			color = "EEEEEE"
		else
			color = "FFFFFF"
		end if
	   if bclick=1 then
		   ss="<tr onclick=clicked(" & ar(0,rowcounter) & ") bgcolor=" & color & "  onmouseover=" & chr(34) & "mOvr(this,'#C6C8CA')" & chr(34) & "  onmouseout=" & chr(34) & "mOut(this,'EEEEEE')" & chr(34) & ">"		
	   else
		   ss = "<tr>"
	   end if


	   if brow=1 then ss=ss & "<td bgcolor= " & color & ">" & rowcounter & "</td>" 
	   FOR colcounter=0 to numcols
	      field=ar(colcounter,rowcounter)
	      if isnull(field) or trim(field) ="" then field= "-"


	      ss=ss& "<td  bgcolor= " & color & " align=left>" & field & "</TD>" 

'	      response.write "</td>" 
	   NEXT
	   response.write ss & "</tr>" & vbcrlf
	   
	NEXT
	
	Response.Write "</TABLE>"
	response.write rowcounter & " rows</DIV>"

end function	


function array2table2(rs,ar,title1 ,title2,title3,wd,brow,bclick,page)
	numcols=ubound(ar,1)
	numrows=ubound(ar,2)	


'	Response.Write "<TABLE  bgcolor=white border=0 cellpadding=2 cellspacing=0 width=" & wd & ">"
'	Response.Write "<tr><TH colspan=" & numcols & " ><b>" & (Title1) & "</b></TH></tr>" 
'	if Title2<> "" then
'		Response.Write "<tr><TH colspan=" & numcols & " ><b>" & (Title2) & "</b></TH></tr>"
 '       End if
'	Response.Write "<tr><TH colspan=" & numcols & " ><b>" & (Title3) & "</b></TH></tr>" 
'	Response.Write "<tr><td colspan =" & numcols & "></td></tr>"  
'	Response.Write "</TABLE>" & chr(13)
 '       Response.write "<br>"

	Response.Write "<div name=dvTBLRESULTS id=dvTBLRESULTS><TABLE name=ecTable id=ecTable class='ectbl_result' border=0 cellpadding=2 cellspacing=0 width=" & wd & ">"

	
	if brow=1 then ss="<TR><TH bgcolor=CCCCCC>ID</TH>"
	For k = 0 to numcols 
            Dim strName
            
            strName = replace(ucase(rs.fields(k).name),"_"," ") 
   
	    ss = ss & "<TH _onclick='TableSort(ecTable," & k & ")' align=left bgcolor=CCCCCC>" & strName & "</TH>" & chr(13)
	next

	Response.Write ss




	FOR rowcounter= 0 TO numrows
		if rowcounter mod 2 = 0 then 
			color = "EEEEEE"
		else
			color = "FFFFFF"
		end if
	   if bclick=1 then
		   ss="<tr onclick=rowclicked('" & page & ar(0,rowcounter) & "') bgcolor=" & color & "  onmouseover=" & chr(34) & "mOvr(this,'#C6C8CA')" & chr(34) & "  onmouseout=" & chr(34) & "mOut(this,'EEEEEE')" & chr(34) & ">"		
	   else
		   ss = "<tr>"
	   end if


	   if brow=1 then ss=ss & "<td bgcolor= " & color & ">" & rowcounter & "</td>" 
	   FOR colcounter=0 to numcols
	      field=ar(colcounter,rowcounter)
	      if isnull(field) or trim(field) ="" then field= "-"


	      ss=ss& "<td  bgcolor= " & color & " align=left>" & field & "</TD>" 

'	      response.write "</td>" 
	   NEXT
	   response.write ss & "</tr>" & vbcrlf
	   
	NEXT
	
	Response.Write "</TABLE>" & "</DIV>"
	response.write "Rows: " & rowcounter 
end function	

function getCell(ar,n,hr,txt)
	dim i
	getcell = "0"
	for i = 0 to n
		if ar(0,i) = hr and ar(1,i) = txt then getcell = ar(2,i)
	next
end function
function getMax(ar, n, txt)
	dim i
	getMax = 1
	for i = 0 to n
		if ar(1,i) = txt and ar(2,i)>getMax then getmax = ar(2,i)
	next
end function

function array2_byhr_table(rs,ar,title1 ,title2,title3,wd,brow,bclick,page)
	dim rowcounter, hr
	numcols=ubound(ar,1)
	numrows=ubound(ar,2)	

	maxAnsw = getmax(ar,numrows, "Answered by agent")
'	maxAban = getmax(ar,numrows, "Canceled")		--------->unused!

	Response.Write "<div name=dvTBLRESULTS id=dvTBLRESULTS><TABLE name=ecTable id=ecTable class='ectbl_result' border=0 cellpadding=2 cellspacing=0 width=" & wd & ">"
	Response.Write "<TH width=150>HOUR OF THE DAY<TH width=100>ANSWERED<TH width=100>ABANDONED<TH width=200>Answered<TH width=200>Abandoned"

	for rowcounter = 9 to 22
		if rowcounter mod 2 = 0 then 
			color = "EEEEEE"
		else
			color = "FFFFFF"
		end if
		hr = rowcounter mod 12
		if rowcounter = 12 then hr = 12
		answ = getCell(ar,numrows, rowcounter, "Answered by agent")
		aban = getCell(ar,numrows, rowcounter, "Canceled")
		response.write "<tr bgcolor=" & color & "><td>" & hr & "<td>" & answ & "<td>" & aban & "<td><img src=img\blue.gif height=10 width=" & cint(answ)/maxansw*200 & ">" & "<td><a  href=javascript:showcancels(document.forms[0]," & rowcounter & ")><img border=0 src=img\red.gif height=10 width=" & cint(aban)/maxansw*200 & "></a><td>"
	next	
	response.write "</table></div>"
end function	

%>
<script>
	document.title = '<%=session("gcnIDno") & " " & session("gcnID")  & " - " & session("gcnregionno")   & " " & session("lhroleid")%>';


function showcancels(frm, hr){
	frm.action = 'cancelled.asp?timestart=' + hr + '&timeend=' + hr;

	frm.submit();
}

function rowclicked(pg){
	window.location.href=pg;
}

function mOvr(src,clrOver){ 
               src.style.cursor = 'hand'; 
               src.bgColor = clrOver; 

} 
function mOut(src,clrIn){ 
                src.style.cursor = 'default'; 
                src.bgColor = clrIn; 
}
</script>

<script>
//***EAC image-embelishing routines...copied verbatim 
function MM_swapImgRestore() { //v3.0
  var i,x,a=document.MM_sr; for(i=0;a&&i<a.length&&(x=a[i])&&x.oSrc;i++) x.src=x.oSrc;
}
function MM_preloadImages() { //v3.0
  var d=document; if(d.images){ if(!d.MM_p) d.MM_p=new Array();
    var i,j=d.MM_p.length,a=MM_preloadImages.arguments; for(i=0; i<a.length; i++)
    if (a[i].indexOf("#")!=0){ d.MM_p[j]=new Image; d.MM_p[j++].src=a[i];}}
}
function MM_findObj(n, d) { //v4.01
  var p,i,x;  if(!d) d=document; if((p=n.indexOf("?"))>0&&parent.frames.length) {
    d=parent.frames[n.substring(p+1)].document; n=n.substring(0,p);}
  if(!(x=d[n])&&d.all) x=d.all[n]; for (i=0;!x&&i<d.forms.length;i++) x=d.forms[i][n];
  for(i=0;!x&&d.layers&&i<d.layers.length;i++) x=MM_findObj(n,d.layers[i].document);
  if(!x && d.getElementById) x=d.getElementById(n); return x;
}
function MM_swapImage() { //v3.0
  var i,j=0,x,a=MM_swapImage.arguments; document.MM_sr=new Array; for(i=0;i<(a.length-2);i+=3)
   if ((x=MM_findObj(a[i]))!=null){document.MM_sr[j++]=x; if(!x.oSrc) x.oSrc=x.src; x.src=a[i+2];}
}


	function updCount(obj){
		if (obj.value.length>3000)
			obj.value = obj.value.substring(0,3000);
		window.status   =  (3000-obj.value.length)  + ' chars. free)';			
	}
</script>

<script language="javascript" defer="true">
<!--
function TableSort( hTable, byColumn ) {
 th = event.srcElement ;
 with ( hTable ) {
  if ( rows.length > 1 ) {
   if ( byColumn != getAttribute ( "sortcol" ) )
    setAttribute ( "sortdir", "" ) ;

   var sortdir = ( getAttribute ( "sortdir" ) == "ascending" ) ?"descending" : "ascending" ;
   setAttribute ( "sortcol", byColumn ) ;
   setAttribute ( "sortdir", sortdir ) ;
   var tabArray = new Array ( rows.length-1 ) ;
   for ( i = 1; i < rows.length ; i++ ) {
	    tabArray[i-1] = [ rows[i], rows[i].cells[byColumn].innerText ] ;
   }
   tabArray.sort ( sortdir == "ascending" ? ascending : descending ) ;
   for ( i = 1; i < rows.length ; i++ ) {
    rows[i].swapNode ( tabArray[i-1][0] ) ;
   }
  }
 }
 return  ;
}
function ascending ( a, b ) {
 return ( ( a[1] > b[1] ) ? 1 : ( a[1] < b[1] ) ? -1 : 0 ) ;
}
function descending ( a, b ) {
 return ( ( a[1] < b[1] ) ? 1 : ( a[1] > b[1] ) ? -1 : 0 ) ;
}
-->
</script>

<script language='javascript'>               
<!--
                
/*yma add this timeput code for login 4/13/2016*/
var hdlTimeoutAlert;
                SetTimeout(); 

                function SetTimeout(){                    
                    ClearTimeout();                 
                    hdlTimeoutAlert = self.setTimeout('timeoutAlert()', 1 * 100000);
                }

                function ClearTimeout(){              
                    clearTimeout(hdlTimeoutAlert);
                }
              
                function timeoutAlert(){
                    if(confirm("To maintain the security of your personal information, we have cleared your current session.\r\n Please click 'Ok' to start a new session.")) {} 
                    document.execCommand('ClearAuthenticationCache');    
                    window.location.href = window.location.href;
                }         

-->
</script>



<NOSCRIPT>
<!--This page requires Javascript to run properly-->
</NOSCRIPT>
<link rel="stylesheet" href="livehelp.css" type="text/css">
<style>

BODY
{
background-attachment:fixed;
background-image : 'url(img/ecback.gif)';

}
</style>