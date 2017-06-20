var tempDT;

var ecMonthMax=new Array(31,28,31,30,31,30,31,31,30,31,30,31);
var ecMonths = new Array("January","February","March","April","May","June","July","August","September","October","November","December");
var ecDay; var ecCurDt, ecTempDt;
var ecCalString ;
var ecMon,ecYear,ecTarget;
var ecTID;

ecTempDt = new Date();
ecCurDt = new Date();
ecTempDt.setDate(1);

ecMon = ecCurDt.getMonth();
ecYear = ecCurDt.getYear();
ecDay = ecCurDt.getDay();

function ecCalendarChangeEvent(obj){
	// DO NOT MODIFY CODE //
	checkDate(obj);
	return(true);
}

function hideCalendar() {
	document.all['ecCalendar'].style.visibility = "hidden"
}

function selectDate(mm,dd,yyyy){
	ecTarget.value = mm+1 + '/' + dd + '/' + yyyy;
	//alert(ecTarget.value);
	document.all['ecCalendar'].style.visibility = "hidden"
	showSelects(document.forms[0]);
	ecCalendarChangeEvent(ecTarget);	// fire the event manually...
}

function isLeap(year) {
	if ((year%400==0)||((year%4==0)&&(year%100!=0))) {return true;}
	else {return false;}
}

function getCalendarFor(temp){

   hideSelects(temp.form);
   setCalendar(ecCurDt.getMonth(),ecCurDt.getYear());
   var obj = document.all['ecCalendar'];
   obj.style.left = document.body.scrollLeft+event.clientX;
   obj.style.top  = document.body.scrollTop+event.clientY ;
   obj.style.visibility = "visible";
   ecTarget = temp;
   
}

function hideCalendar(){
	document.all['ecCalendar'].style.visibility = "hidden"
	showSelects(document.forms[0]);
}
function setCalendarFromString(dt){
	var tmp = dt.split(",");
	setCalendar(tmp[0],tmp[1]);
}

function setCalendar(mm,yy) {
	//next 2 lines obsolete, we assume year+month are valid!!!!
	if (yy  == null) {yy = ecCurrDt.getYear();}
	if (mm == null) {mm = ecCurrDt.getMonth();}
	if (mm == 1) {ecMonthMax[1]  = (isLeap(yy)) ? 29 : 28;}
	ecTempDt.setYear(yy);
	ecTempDt.setMonth(mm);
	ecTempDt.setDate(1);
	document.all['monthDays'].innerHTML = drawCal();	
	document.all['ecMonthSelector'].innerHTML = drawSelect(mm + ',' + yy);	

}

function drawCell(bg,dd,mm,yy){
	var ecCell;
	var td1 = "<td width=20 bgcolor='" + bg + "' onMouseup=selectDate(" + ecTempDt.getMonth() + "," +  dd + "," + ecTempDt.getYear() + ")  onMouseOver=this.style.backgroundColor='FF0000';this.style.cursor='hand' onMouseOut=this.style.backgroundColor='" + bg + "'><font face='MS Sans Serif, sans-serif' size='1'>";
	var td2 = " </font></td>";
	//ecCell = td1 + '<a href=javascript:alert(' + dd + ')>' + dd + '</a>' +  td2;
	ecCell = td1 + dd +  td2;
	return(ecCell);
}


function drawCal() {
	var ecCellStart = "<TD width=20><font face='MS Sans Serif, sans-serif' size='1'>"
	var ecCellEnd = "</font></td>"
	var year  = ecTempDt.getYear();
	var month = ecTempDt.getMonth();
	
	var date  = 1;
	var day   = ecTempDt.getDay();
	var max   = ecMonthMax[month];
	var bgr,cnt,tmp = "";
	var j,i = 0;

	ecCalString = "<table width=\"200\" cellspacing=\"1\" cellpadding=\"2\" border=\"1\" bordercolorlight=\"#000000\" bordercolordark=\"#000000\">\n";
	ecDay = ecTempDt.getDay();
	for (i = 0; i < ecDay; ++i) {
		ecCalString +=  ecCellStart + "&nbsp" + ecCellEnd;
		
	}

	for (j = 1; j <= max ; j++) {
			//ecCalString += ecCellStart + drawCell('FFFFFF',j) + ecCellEnd;
			ecCalString +=  drawCell('FFFFFF',j,ecTempDt.getMonth(),ecTempDt.getYear()) ;						
			if ((j+ecDay)%7==0) ecCalString = ecCalString + "</TR>";
	}
	ecCalString += "</TABLE>"
	
	return(ecCalString);
} 
 




function drawSelect(mmyy){

var sHDR = "<select name=sItem  onChange=setCalendarFromString(this.options[this.selectedIndex].value) style=font-family: 'MS Sans Serif', sans-serif; font-size: 9pt>"
var i,m,y;
var tmp = mmyy.split(",");


	m = (tmp[0]-6 < 0) ? 14-tmp[0] : tmp[0]-6;
	y = (tmp[0]-6 < 0) ? tmp[1]-1 : tmp[1];
	
	
	for (i=0; i<12; i++) {
		if (m > 11) {m=0; y++}
		sHDR += '<option value=\'' + m + ',' + y + '\''
		if (m==tmp[0]) sHDR += ' selected'
		sHDR += '>' +  ecMonths[m++] + ' ' + y + '</option>\n' 
	}
	sHDR += "</select></font><a href=javascript:moveForward() ><font face=Arial, Helvetica, sans-serif size=2 color=#000000><b>&nbsp;&gt;&gt</b></font></a></td></tr></table>";
	
	return("<table border=1 cellspacing=1 cellpadding=1 width=200 bordercolorlight=000000 bordercolordark=000000 vspace=0 hspace=0><tr><td align=center bgcolor=CCCCCC><a href=javascript:moveBack()><font face=Arial, Helvetica, sans-serif size=2 color=#000000><b>&lt&lt;&nbsp;</b></font></a><font face=MS Sans Serif, sans-serif size=1>" + sHDR + "<table border=1 cellspacing=1 cellpadding=2 bordercolorlight=#000000 bordercolordark=#000000 width=200 vspace=0 hspace=0><tr align=center bgcolor=#CCCCCC><td width=20 bgcolor=#FFFFCC><b><font face=MS Sans Serif, sans-serif size=1>Su</font></b></td><td width=20><b><font face=MS Sans Serif, sans-serif size=1>Mo</font></b></td><td width=20><b><font face=MS Sans Serif, sans-serif size=1>Tu</font></b></td><td width=20><b><font face=MS Sans Serif, sans-serif size=1>We</font></b></td><td width=20><b><font face=MS Sans Serif, sans-serif size=1>Th</font></b></td><td width=20><b><font face=MS Sans Serif, sans-serif size=1>Fr</font></b></td><td width=20 bgcolor=#FFFFCC><b><font face=MS Sans Serif, sans-serif size=1>Sa</font></b></td></tr></table>"); 
}


function moveBack(){
var m,y;
	y = (ecTempDt.getMonth() == 0) ? ecTempDt.getYear()-1 : ecTempDt.getYear();
	m = (ecTempDt.getMonth() == 0) ? 11 : ecTempDt.getMonth()-1;
	setCalendar(m,y);		
}

function moveForward(){
var m,y;
	y = (ecTempDt.getMonth() == 11) ? ecTempDt.getYear()+1 : ecTempDt.getYear();
	m = (ecTempDt.getMonth() == 11) ? 0 : ecTempDt.getMonth()+1;
	setCalendar(m,y);		
}




function y2k(number) { return (number < 1000) ? number + 1900 : number; }


function checkDate(obj){

	if (obj.value == '') return(true);

	var temp = new Date(obj.value);
	var yy = '';
	if (isNaN(temp)) {
		obj.focus();	
		alert('That is not a valid date!'); 
		obj.value = '';
		return(false);
	}
	if (temp.getYear() < 1000) yy = '2';
	if (temp.getYear() < 100) yy = '20';
	if (temp.getYear() < 10) yy = '200';
	obj.value = (1+temp.getMonth()) + "/" + temp.getDate() + "/" + yy + temp.getYear();
}


function isDate2(day,month,year) {
    var today = new Date();
    year = ((!year) ? y2k(today.getYear()):year);
    month = ((!month) ? today.getMonth():month-1);
    if (!day) return false
    var test = new Date(year,month,day);
    if ( (y2k(test.getYear()) == year) &&
         (month == test.getMonth()) &&
         (day == test.getDate()) )
        return true;
    else
        return false
}

function hideSelects(frm){
    var n = (frm.elements) ? frm.elements.length:0;
    for (var i=0;i<n;i++) {
		if (frm.elements[i].type == 'select-one')		frm.elements[i].style.visibility = 'hidden';
		if (frm.elements[i].type == 'select-multiple')		frm.elements[i].style.visibility = 'hidden';		
    }

}


function showSelects(frm){
    var n = (frm.elements) ? frm.elements.length:0;
    for (var i=0;i<n;i++) {
		if ((frm.elements[i].type == 'select-one') && (frm.elements[i].name != 'sItem')){
			frm.elements[i].style.visibility = 'visible';
		}
		if (frm.elements[i].type == 'select-multiple' ){
			frm.elements[i].style.visibility = 'visible';
		}

    }
}


function addDays(dt,n){	// skip non-business days!!!
	window.status = '';
	var i;
	var temp=dt;
	i=1;
	while (i<=n) {
		temp = new Date(temp.getYear(),temp.getMonth(),temp.getDate() + 1)
		i++;
		if ((temp.getDay() == 0) || (temp.getDay()==6) || isHoliday(temp)){
			i--;
		}
	}


	return(temp);
}

function isHoliday(dt){

	for (i=0; i<holidays.length; i++){
		t = new Date(holidays[i])
		if ( (dt.getDate() == t.getDate()) &&  (dt.getMonth() == t.getMonth()) &&  (dt.getYear() == t.getYear())){
			window.status = window.status + t + ' is a Holiday!';
//			alert(t + ' is a Holiday!');
			return(true);
		}
	}
	return(false);
}


//***EAC HOLIDAYS ARRAY is based on http://www.opm.gov/fedhol/2004.htm
holidays= new Array(
"1/1/2002",
"1/21/2002",
"2/18/2002",
"5/27/2002",
"7/4/2002",	
"9/2/2002",
"10/14/2002",
"11/11/2002",
"11/28/2002",
"12/25/2002",

"1/1/2003",
"1/20/2003",
"2/17/2003",
"5/26/2003",
"7/4/2003",	
"9/1/2003",
"10/13/2003",
"11/11/2003",
"11/27/2003",
"12/25/2003",

"1/1/2004",
"1/19/2004",
"2/16/2004",
"5/31/2004",
"7/5/2004",	
"9/6/2004",
"10/11/2004",
"11/11/2004",
"11/25/2004",
"12/24/2004",

"12/31/2004",
"1/17/2005",
"2/21/2005",
"5/30/2005",
"7/4/2005",	
"9/5/2005",
"10/10/2005",
"11/11/2005",
"11/24/2005",
"12/26/2005",

"1/2/2006",
"1/16/2006",
"2/20/2006",
"5/29/2006",
"7/4/2006",	
"9/4/2006",
"10/9/2006",
"11/10/2006",
"11/23/2006",
"12/25/2006"
)

