function fnAddItemToCart(sender, e) 
{
    __doPostBack(sender, e);
}
function fnPostBack(sender, e, ctrlId1, ctrlId2, ctrlId3, ctrlId4) {
    if (fnCheckValue(ctrlId1, ctrlId2, ctrlId3))
        __doPostBack(sender, e);     
}
function fnCheckValue(ctrlId1, ctrlId2, ctrlId3) {
    
    var obj1 = document.getElementById(ctrlId1);
    var obj2 = document.getElementById(ctrlId2);
    var obj3 = document.getElementById(ctrlId3);

    var flag = true;

    if (trim(obj1.value).length == 0)
        flag = false;

    //check for number
    if (flag == true)
        if (isNaN(obj1.value))
            flag = false;
    
    //check for integer
    if (flag == true)
        if (!isInteger(obj1.value))
            flag = false;
    
    //check for value
    if (flag == true)
        if (parseInt(obj1.value) == 0 || parseInt(obj1.value) > parseInt(obj2.value)) 
            flag = false;
        
    if (flag == false) {
        obj3.innerHTML = "Please enter a valid quantity.";
        obj1.focus();
        return false;
    }
    else
        obj3.innerHTML = "";

    return true;
}

//Trims right and left spaces
function trim(stringToTrim) {
	return stringToTrim.replace(/^\s+|\s+$/g,"");
}

//checks for integer
function isUnsignedInteger(s) {
    return (s.toString().search(/^[0-9]+$/) == 0);
}

function isInteger(n) {
    return (Math.floor(n) == n);
}

//reset text on cancel
function fnCancelClick(ctrlId1) {
    var obj1 = document.getElementById(ctrlId1);
    obj1.value = "1";
}
