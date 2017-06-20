////saves scroll positon of the body
//function fnSaveScrollPos(ctrlId) {
//    var obj = document.getElementById(ctrlId);
//    if (obj != null) {
//        obj.value = document.getElementById("mbody").scrollTop;
//    }
//    return true;
//}
function fnMouseOver() {
    var currpage = location.href;
    if (currpage.indexOf("home.aspx") > 0)
        return false;
    window.document.getElementById('divFloatLink').style.display = ""; //or block
    return true;
}
function fnMouseOut() {
    var currpage = location.href;
    if (currpage.indexOf("home.aspx") > 0)
        return false;
    window.document.getElementById('divFloatLink').style.display = "none";
    return true;
}
function fnMouseOverLogo() {
    var currpage = location.href;
    if (currpage.indexOf("home.aspx") > 0)
        return false;
    window.document.getElementById('logoImage').src = "images/logorollover.gif";
}
function fnMouseOutLogo() {
    var currpage = location.href;
    if (currpage.indexOf("home.aspx") > 0)
        return false;
    window.document.getElementById('logoImage').src = "images/logo.gif";
}
