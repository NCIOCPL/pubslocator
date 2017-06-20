function taLimit(maxLength) {
    
	var taObj=event.srcElement;
	//alert(taObj.value.length+" "+maxLength);
	if ((taObj.value.length==maxLength*1)) 
	{
		alert('The maximum allowed character limit for this control has been exceeded!!!');
		return false;
	}
	
}

function taCount(maxLength) { 
	var taObj=event.srcElement;
	
	if (taObj.value.length>maxLength*1)
		taObj.value=taObj.value.substring(0,maxLength*1);
	//if (visCnt) 
		//visCnt.innerText
    //alert(maxLength-taObj.value.length);
    window.status = maxLength - taObj.value.length + ' character(s) remaining';
}

function taGone() {
    window.status = 'Done';
}
