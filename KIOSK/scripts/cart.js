//Displays a confirmation message to the user, before removing an item from shopping cart.
function fnAskBeforeRemoval(ctrlId) 
{
    /*var obj = document.getElementById(ctrlId);
    if (obj != null) 
    {
        if (confirm("Do you wish to remove this item?"))
            return true;
        else
            return false;
    }*/
    return true;
}
//Displays a confirmation message to the user, before cancelling an order.
function fnAskBeforeCancelling(ctrlId) {
    /*var obj = document.getElementById(ctrlId);
    if (obj != null) {
        if (confirm("Do you wish to cancel the order?"))
            return true;
        else
            return false;
    }*/
    return true;
}
//Modify quantity ordered
function fnUpdateQty(
                    strPubId,
                    ctrlQty,
                    ctrlHdnQty,
                    strLimit,
                    strTotalLimit,
                    strAction,
                    ctrlTotal,
                    ctrlRemaining,
                    ctrlMessage
                    ) 
{
    var objQty = document.getElementById(ctrlQty);
    var objHdnQty = document.getElementById(ctrlHdnQty);
    var objTotal = document.getElementById(ctrlTotal);
    var objRemaining = document.getElementById(ctrlRemaining);
    var objMessage = document.getElementById(ctrlMessage);

    var pubid = strPubId;
    var qty = objQty.innerHTML;
    var hdnqty;
    var limit = strLimit;
    var totallimit = strTotalLimit;
    var action = strAction;
    var total = objTotal.innerHTML;
    var remaining = objRemaining.innerHTML;
    var message = objMessage.innerHTML;

    if (objQty != null && objTotal != null
        && objRemaining != null && objMessage != null
        && pubid.length > 0 && limit.length > 0
        && totallimit.length > 0 && action.length > 0) {

        message = "";
        var tempqty = qty;
        var temptotal = total;
        var tempremaining = remaining;
        
        var flag = true;
        

        if (action == "plus"){
            tempqty++;
            temptotal++;
            tempremaining--;
            }
        if (action == "minus"){
            tempqty--;
            temptotal--;
            tempremaining++;
            }

            if (tempqty < 0) {
                flag = false;
            }

            if (tempqty > limit) {
                //message = "Sorry, you cannot order more than <font color=red>" + limit + "</font> copies of " + pubid + ".";
                //message = "Sorry, you cannot order more than " + limit + " copies of " + pubid + ".";
                if (limit == "1") {
                    //message = "Sorry, you cannot order more than " + limit + " copy of this item.";
                    message = "You have reached the limit of " + limit + " copy of this item.";
                }
                else {
                    //message = "Sorry, you cannot order more than " + limit + " copies of this item.";
                    message = "You have reached the limit of " + limit + " copies of this item.";
                }
                objMessage.innerHTML = message;
                window.document.getElementById('divErrMsg').style.display = "";

                //objQty.style.fontWeight = 'bold';
                //objQty.style.color = 'red';
                //objQty.style.backgroundColor = '#FFEC5C';
                objQty.className = 'cartnumwarning';
                
                flag = false;
            }
            else {
                //objQty.style.color = '';
                objQty.className = '';
                //objQty.style.backgroundColor = '';
            }

        if (flag == true && temptotal > totallimit) {
            //message = "Sorry, you cannot order more than <font color=red>" + totallimit + "</font> items.";
            //message = "Sorry, you cannot order more than " + totallimit + " items.";
            message = "You have reached the limit of " + totallimit + " total items per order.";
            
            objMessage.innerHTML = message;
            window.document.getElementById('divErrMsg').style.display = "";

            //objQty.style.color = 'red';
            //objQty.style.backgroundColor = '#FFEC5C';
            objQty.className = 'cartnumwarning';
            flag = false;
        }
        else {
            if (flag == true) {
                //objQty.style.fontWeight = 'normal';
                
                //objQty.style.color = '';
                //objQty.style.backgroundColor = '';
                objQty.className = '';
            }
        }

        if (flag == true) {
            qty = tempqty;
            total = temptotal;
            remaining = tempremaining;

            hdnqty = qty;

            //Assign back to controls
            objQty.innerHTML = qty;
            objHdnQty.value = hdnqty;
            objTotal.innerHTML = total;
            objRemaining.innerHTML = remaining;
            objMessage.innerHTML = "";
            window.document.getElementById('divErrMsg').style.display = "none";
        }
    }
    
    return false; //Always false to stop post back
    
    
//    if (obj != null) {
//        if (confirm("Do you wish to remove this item?"))
//            return true;
//        else
//            return false;
//    }
    
    //return true;
}
////Set Visibility
//function setvisibility(ctrlId, state) {
//    var obj = document.getElementById(ctrlId);
//    obj.style.display = state;
//    return true;
//}
////Call visibility
////setvisibility('dvSeparator', 'none'); //Hide
////setvisibility('dvSeparator', ''); //Show
