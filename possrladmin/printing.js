function makePageBreaks(tbl, height, startrow, continued, cls){

	var pgHeight=height;
	var ith=startrow;
	var pageTop=0;
	var elBase=0;
	var wd = tbl.width;

	var START = "<TABLE border=0 CELLSPACING=0 CELLPADDING=0 WIDTH='" + wd +"px' class='" + cls  + "'>";
	for (x=0; x<startrow; x++){
		START += tbl.rows[x].outerHTML;
	}

	newHTML=START;
	//alert(startrow);
	//alert(tbl.rows.length);
	for (x=ith; x<tbl.rows.length; x++){
	     el = tbl.rows[x];
	     elBasePrevRow = elBase;
	     elBase = el.offsetTop+el.offsetHeight;
	     if(parseInt(elBase-pageTop)>pgHeight){
	       newHTML+="</table><p><DIV style='page-break-after:always'></DIV>" + continued  + "<P>" + START;
	           pageTop=elBasePrevRow;
	     }
	     newHTML+=tbl.rows[x].outerHTML;
	     
	}
	newHTML += "</table>";
	//document.body.innerHTML=newHTML;
	document.all.dvTBLRESULTS.innerHTML=newHTML;
}

function makePageBreaks2(t, height, startrow, continued, cls, dv){
	var tbl = document.getElementById(t)
	var pgHeight=height;
	var ith=startrow;
	var pageTop=0;
	var elBase=0;
	var wd = tbl.width;

	var START = "<TABLE border=0 CELLSPACING=0 CELLPADDING=0 WIDTH='" + wd +"px' class='" + cls  + "'>";
	for (x=0; x<startrow; x++){
		START += tbl.rows[x].outerHTML;
	}

	newHTML=START;
	//alert(START);
	//alert(tbl.rows.length);
	for (x=ith; x<tbl.rows.length; x++){
	     el = tbl.rows[x];
	     elBasePrevRow = elBase;
	     elBase = el.offsetTop+el.offsetHeight;
	     if(parseInt(elBase-pageTop)>pgHeight){
	       newHTML+="</table><p><DIV style='page-break-after:always'></DIV>" + continued  + "<P>" + START;
	           pageTop=elBasePrevRow;
	     }
	     newHTML+=tbl.rows[x].outerHTML;
	     
	}
	newHTML += "</table>";
	document.getElementById(dv).innerHTML=newHTML;
}