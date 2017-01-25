<!--#include file="connect.inc" -->
<Html>
<head><title>SRL</title>

<script language="Javascript">

	var dt = new Date;
	dt.setMonth(dt.getMonth() + 10);
	var expires = "; expires=" + dt.toGMTString();

	fldOpen = new Image();	fldOpen.src = "img/folderopen.gif";
	fldClose = new Image();	fldClose.src = "img/folderclosed.gif";

	var currentobj=0;

function preview2(obj,i,j){
	try{
		var temp = document.getElementById("span"+currentobj);
		temp.className = '';
		
		var srl = String(SRLTEXT[i]);
		srl = srl.replace(/<br>/gi,"\n");
		parent.prv.ectxtpreviewvisible.value = srl;
		parent.prv.ectxtpreview.value = srl;
		parent.prv.ectxtpreview.focus();
		parent.prv.ectxtpreview.select();	
		parent.prv.dvtitle.innerHTML = (obj.innerText.length<100) ? obj.innerText : obj.innerText.substr(0,100)+'...';
	
		obj.dragDrop();

		currentobj=j;			
	}catch(e){window.status = e}
}

function preview_fancy(obj,i,j){
	var stepLeft = 300;
	var stepTop = -40
	try{
		var temp = document.getElementById("span"+currentobj);
		temp.className = '';
		
		var srl = String(SRLTEXT[i]);
		srl = srl.replace(/<br>/gi,"\n");


		var el = layerHandler.getRefs('dvprv');
		e = window.event;
		if (e) {
			var x =  (e.pageX)? e.pageX: (e.clientX)? e.clientX + getScrollX(): 0;
			var y =  (e.pageY)? e.pageY: (e.clientY)? e.clientY + getScrollY(): 0;

			x = x+stepLeft;
			y = y+stepTop;
			W = document.body.clientWidth + getScrollX();
			H = document.body.clientHeight + getScrollY();
			
			if (x+500>W){			
				x = W - 503;				
			}
			if (y+300>H){
				y = H - 303;
			}

			layerHandler.shiftTo(el, x, y);
		}		
		
		//***EAC not really needed -- layerHandler.show(el);	

		tst = document.frames['frmprv'];
		tst.dvtitle.innerHTML = (obj.innerText.length<35) ? obj.innerText.substr(0,35): obj.innerText.substr(0,35)+'...';
		tst.ectxtpreviewvisible.value = srl;
		tst.ectxtpreview.value = srl;
		tst.ectxtpreview.focus();
		tst.ectxtpreview.select();	
		
//alert(tst.ectxtpreviewvisible.style.height);
		
		
		obj.dragDrop();
		//window.status = currentobj + ' test:' + j;	
		
		currentobj=j;			
	}catch(e){window.status = e}
}
function xpandfolder(obj){
	var tg;		
		tg = document.getElementById(obj.id + "d");
		if (tg.style.display == "none") {
			tg.style.display = "";
			obj.src = fldOpen.src;
		} else {
			tg.style.display = "none";
			obj.src = fldClose.src;
		}
}



function on_2xclick(i){
	try{
		parent.parent.parent.parent.interactionAreaDynFrame.frames[1].document.all.captureTextField.value += String(SRLTEXT[i]) + "\n";
		parent.parent.parent.parent.interactionAreaDynFrame.frames[1].document.all.captureTextField.doScroll();
	}
	catch (e) {
	} 
}


function show_props(obj, objName) {

	var r = "";
	for (var i in obj) {
		r += objName + "." + i + " = " + obj[i] + " \n    ";
	}
	return (r);
	
	
}


</script>
</head>
<link rel="stylesheet" href="ec.css" type="text/css">
<body bgcolor=white onload="document.all.dvbody.style.display=''" >

<span id=span0><span>
<table border=0 width=100% ID="Table1">
<tr><td>
<form action=possrl_a.asp method=post >
	<input alt='srchitem' name=srchitem type=text size=25 maxlength=100 value="<%=MaskSymbols(request.Form("srchitem"))%>" > 
	<input type=submit value = Search >
	<input type=button onclick="srchitem.value=''; parent.prv.dvtitle.innerHTML=''; parent.prv.ectxtpreviewvisible.value=''; parent.prv.ectxtpreview.value=''; this.form.submit();" value = Reset >	
	<div style='vertical-align:bottom; position:absolute' id=dvfound></div>
<!--	
	<input type=button onclick="parent.uberbuffer=document.body.innerHTML; alert(document.body.innerHTML);" value = 'Save'>
	<input type=button onclick="alert(parent.uberbuffer);document.body.innerHTML=parent.uberbuffer;" value = 'Back'>
-->	

<div id="dvbody" name=dvbody style="display:_none">
<%
	mysql=""
	w1="-999,"
	w2=""
	tok=""

   dim folders(100)
   
   filler = " <img alt='spacer' src=img/empty.gif height=1 width=50 border=1> "


	for each item in Request.Form
		'Response.Write item & "=" & Request.Form(item) & "<br>"
	next

	Set conn = Server.CreateObject("ADODB.Connection")
	Set rs = Server.CreateObject("ADODB.RecordSet")
	conn.open DSN
	
	scr="<script language=Javascript>" & vbcrlf & "SRLTEXT = new Array(" & vbcrlf
	
	temp = ""
	cnt = 0
	ind = 0
	f = 1
	fold="folderclosed.gif"
	disp="none"

	folders(1) = -1

	srchitem = Validate(request.Form("srchitem"))

	if srchitem = null or len(srchitem) = 0 then
		mysql = "select * from NCI_POSSRL where active = 1 order by pos"
	else
		fold="folderopen.gif"
		disp=""
		'srchs = split(srchitem," ")
		for each tok in split(srchitem," ")
			w2 = w2 & " upper(title + ' ' + text) like upper('%" & tok & "%') and "
		next 
		w2 = w2 & " active=1"
		mysql = "select pos,text2 from NCI_POSSRL where  " & w2

		'response.Write " 22222222 " & mysql 
	
		rs.open mysql,conn,1,3
		while not rs.EOF
			w1 = w1 & rs("pos") & "," & rs("text2")
			rs.MoveNext
		wend
		w1 = w1 & "-1"

		mysql = "select * from  NCI_POSSRL where pos in (" + Dedupe(w1) + ") order by pos"
		rs.close
	end if
	
	'response.Write " 111111111 " & mysql 
	rs.open mysql,conn,1,3

	while not rs.EOF
		ind = rs("indent")
		text = MaskSymbols(rs("text"))
		
		scr = scr & """" & text & """," & vbcrlf
		
		'TODO: write /DIVs here
		if ind <= folders(f) then
			while ind <= folders(f)
				response.Write "</DIV>" & vbcrlf
				f = f - 1
			wend	
		end if
		
		response.Write "<img alt='spacer' src=img/empty.gif height=10 width=" & 30 * ind & ">"
		if rs("folder") = 1 then
			response.Write "<span  onclick='xpandfolder(document.all.Out" & rs("autoid") & ")'><img alt='spacer' src=img/" & fold & " id=Out" & rs("autoid") & " ><b>" & rs("title") & "</b><br> <div style='display:" & disp & "' id=Out" & rs("autoid") & "d></span>"
			f = f + 1
			folders(f) = ind
		else
			response.Write "<span  id=span" & rs("autoid") & " onmousedown=""this.className='over'; preview2(this," & cnt & "," & rs("autoid") & ")"" > "  & rs("title") & "</span><br>"
		end if
		cnt = cnt + 1
		rs.MoveNext	
	wend 
	
	response.write "</div>Found: " & cnt
	if cnt = 0 then
		response.write " No records matched!"
	end if
	response.write SCR & "0)</script>"

	function Dedupe (s)
		Dedupe = s
	end function 
	
	
	function MaskSymbols (s)
		temp = ""
		if s <> "" then
			for i = 1 to len(s)
				ch = mid(s,i,1)
				if ch = chr(13) then				
					'do nothing: #10 will take care of this
				elseif ch = chr(10) then
 					temp = temp + "\n"
				elseif ch = "<" then
 					' do nothing: tags are dangerous 
				elseif ch = ">" then
 					' do nothing: tags are dangerous
				elseif ch = "%" then
 					' do nothing: percent sign dangerous
				elseif ch = "=" then
 					' do nothing: equal sign dangerous
				elseif ch = """" then
 					temp = temp + "\"""					
				else
					temp = temp & ch
				end if
				
			next
		end if
		MaskSymbols = temp
	end function
	
	function Validate (s)
		Validate = trim(s)
	end function	
%>

</table>
</form>

</body>
