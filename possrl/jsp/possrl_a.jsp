
<%@ page language="java" buffer="none" contentType="text/html; charset=utf-8" %>
<%@ page import="java.sql.*" %>
<%@ page import="java.util.StringTokenizer" %>
<%@ page buffer="none" %>
<%@ include file="ec.inc" %>

<head><title>SRL</title>
<style>
SPAN
{
	CURSOR:HAND;
}
</style>
<script src="dw_layer.js" type="text/javascript"></script>
<script language="Javascript">

	var dt = new Date;
	dt.setMonth(dt.getMonth() + 10);
	var expires = "; expires=" + dt.toGMTString();

	fldOpen = new Image();	fldOpen.src = "img/icdirectoryu.gif";
	fldClose = new Image();	fldClose.src = "img/icdirectorycloseds.gif";

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
<table border=0 width=100% >
<tr><td>
<form action=possrl_a.jsp method=post>
	<input alt='srchitem' name=srchitem type=text size=25 maxlength=100 value="<%=MaskSymbols(request.getParameter("srchitem"))%>"> 
	<input type=submit value = Search >
	<input type=button onclick="srchitem.value=''; parent.prv.dvtitle.innerHTML=''; parent.prv.ectxtpreviewvisible.value=''; parent.prv.ectxtpreview.value=''; this.form.submit();" value = Reset >	
	<div style='vertical-align:bottom; position:absolute' id=dvfound></div>
<!--	
	<input type=button onclick="parent.uberbuffer=document.body.innerHTML; alert(document.body.innerHTML);" value = 'Save'>
	<input type=button onclick="alert(parent.uberbuffer);document.body.innerHTML=parent.uberbuffer;" value = 'Back'>
-->	

<div id=dvbody name=dvbody style="display:none">
<%

	String mysql="";

	String w1="-999,";
	String w2="";
	String tok="";
	
   int folders[] = new int[10];
   
   String filler = " <img alt='spacer' src=img/empty.gif height=1 width=50 border=1> ";
   Connection conn = null;
   Statement stmt = null;
   ResultSet rs = null;

   try
   {
    //Class.forName("oracle.jdbc.driver.OracleDriver");
    Class.forName("com.microsoft.jdbc.sqlserver.SQLServerDriver");

    conn = DriverManager.getConnection(MyGHSConn, MyGHSUser, MyGHSPass);
    stmt = conn.createStatement();

	String scr="<script language=Javascript>\nSRLTEXT = new Array(\n";

	int temp;
	int cnt = 0;
	int ind = 0;
	int f = 0;
	String fold="icdirectorycloseds.gif";
	String disp="none";
	folders[0] = -1;



	//String srchitem = request.getParameter("srchitem");
	String srchitem = Validate(request.getParameter("srchitem"));
	if (srchitem=="__badcharsdetected") response.sendRedirect("errorpage.jsp");
	
	if( srchitem == null ) srchitem = "";
	srchitem = Replace(srchitem,"\'","''");
	
	if (srchitem.length() == 0  || srchitem == null){
		mysql = "select * from NCI_POSSRL where active = 1 order by pos";
	}
	else{
		fold="icdirectoryu.gif";
		disp="";
			
		StringTokenizer st = new StringTokenizer(srchitem);
			while (st.hasMoreTokens()) {
			tok = st.nextToken();
				//out.println("<br>NOEL:" + st.nextToken());
				w2 +=  " upper(title + ' ' + text) like upper('%" + tok + "%') and ";
			}
		w2 += " active = 1 ";

		mysql = "select pos,text2 from NCI_POSSRL where " + w2 ;
		//out.println(mysql);
		rs = stmt.executeQuery(mysql);
		while(rs.next())      {
			w1 += rs.getString("pos") + "," + rs.getString("text2");
		}	
		mysql = "select * from  NCI_POSSRL where pos in (" + Dedupe(w1) + ") order by pos";
		
	}

	//out.println(mysql);
	rs = stmt.executeQuery(mysql);

	while(rs.next())      {
		ind = rs.getInt("indent");
		scr = scr + "\"" + MaskSymbols(rs.getString("text")) + "\",\n" ;

		if (ind <= folders[f]){
			while(ind <= folders[f]){
				out.println("</DIV>\n");
				f--;
			}
		}

		out.println("<img alt='spacer' src=img/empty.gif height=10 width=" + 30 * ind + ">");
		if (rs.getInt("folder") == 1){
			out.println("<span  onclick='xpandfolder(document.all.Out" + rs.getInt("autoid") + ")'><img alt='spacer' src=img/" + fold + " id=Out" + rs.getInt("autoid") + " ><b>" + HTMLEncodeJSP(rs.getString("title")) + "</b><br> \n<div style='display:" + disp + "' id=Out" + rs.getInt("autoid") + "d></span>");
			folders[++f] = ind;
		}
		else {
			out.println("<span  id=span" + rs.getInt("autoid") + " onmousedown=\"this.className='over'; preview2(this," + cnt + "," + rs.getInt("autoid") + ")\" > "  +HTMLEncodeJSP(rs.getString("title")) + "</span><br>");
		}
		cnt = cnt+1;
	}
	for (temp=1; temp<=f; temp++){
		out.println("</DIV>\n");
	}
	//out.println("<script>document.all['dvfound'].innerText='Found:" + cnt + "'</script>");
	out.println("Found: " + cnt );
	if (cnt == 0) {
		out.println("No records matched!");
	}
	out.println(scr + "0)</script>");
	stmt.close();
   
   }
   

   catch(SQLException e)
   {
      out.println("SQLException: " + e.getMessage() + "<BR>");
      while((e = e.getNextException()) != null)
         out.println(e.getMessage() + "<BR>");
   }
   catch(ClassNotFoundException e)
   {
      out.println("ClassNotFoundException: " + e.getMessage() + "<BR>");
   }
   finally
   {
		//Clean up resources, close the connection.
		if(stmt != null){
		try{
		stmt.close();
		}
		catch (Exception ignored) {}
		}
		if(rs != null){
		try{
		rs.close();
		}
		catch (Exception ignored) {}
		}
		if(conn != null){
		try{
		conn.close();
		}
		catch (Exception ignored) {}
		}
   }

%>
</table>
</form>

</body>


<%!
public String Dedupe (String strIn)
{
	String result="-999,";
	
	String temp[];
	temp = strIn.split(",");
	for (int i=0; i<temp.length; i++){
		if (result.indexOf("," + temp[i] + ",")<0){
			result += temp[i] + ",";
		}
	}
	result += "-999";
	return (result);
}


public String MaskSymbols (String strIn)
{
	String strOut = "";
	int i;
	int iLen;
	char ch;
	if (strIn != null)
	{
		iLen = strIn.length();
		for (i=0; i < iLen; i++)
		{
			ch = strIn.charAt(i);
			if (ch == '\r')
				strOut += "\\r";
			else if (ch == '\n')
				strOut += "\\n";
			else if (ch == '\t')
				strOut += "\\t";
			else if (ch == '"')
				strOut += "\\\"";
			else if (ch == '\\')
				strOut += "\\\\";
			else if (ch == '<')
			{
				if (i != iLen-1)
					strOut += "\" + \"<\" + \"";
				else
					strOut += "\" + \"<";
			}
			else if (ch == '>')
			{
				if (i != iLen-1)
					strOut += "\" + \">\" + \"";
				else
					strOut += "\" + \">";
			}
			else
				strOut += ch;
		}
	}
	return strOut;

};

public static String Replace(String source, String target, String replacement)
      {
          //***EAC dirty but it works...
          StringBuffer result = new StringBuffer(source.length());
          int i = 0, j = 0;
          int len = source.length();
          while (i < len) {
              j = source.indexOf(target, i);
              if (j == -1) {
                  result.append( source.substring(i,len) );
                  break;
              }
              else {
                  result.append( source.substring(i,j) );
                  result.append( replacement );
                  i = j + target.length();
              }
          }
          return result.toString();
      }

%>
