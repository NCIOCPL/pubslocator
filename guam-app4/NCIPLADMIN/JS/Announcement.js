
var prm = Sys.WebForms.PageRequestManager.getInstance();

prm.add_initializeRequest(initializeRequest);
prm.add_endRequest(endRequest);

var postbackElement;
/*
window.onload = function LUMgmtOnload() {
    alert('LUMgmtOnload');
    if (typeof AnnouncementxOnload == 'function') {
        AnnouncementxOnload();
    }
}
*/
function initializeRequest(sender, args) {

    postbackElement = args.get_postBackElement();
    if (postbackElement.id.indexOf("btnAddTab") > 0) {
        ActivateAlertDiv('visible', 'AddTabAlertDiv', 'Loading...');
    }
    else if (postbackElement.id.indexOf("btnEditTab") > 0) {
        ActivateAlertDiv('visible', 'EditTabAlertDiv', 'Loading...');
    }
    else if (postbackElement.id.indexOf("btnAdd") > 0) {
        ActivateAlertDiv('visible', 'AddTabAlertDiv', 'Loading...');
    }
    else if (postbackElement.id.indexOf("gvResult_Edit") > 0) {
        ActivateAlertDiv('visible', 'EditTabAlertDiv', 'Loading...');
    }
    else if (postbackElement.id.indexOf("gvResult_Update") > 0) {
        ActivateAlertDiv('visible', 'EditTabAlertDiv', 'Loading...');
    }
    else if (postbackElement.id.indexOf("gvResult_Cancel") > 0) {
        ActivateAlertDiv('visible', 'EditTabAlertDiv', 'Loading...');
    }
    else if (postbackElement.id.indexOf("lnkbtnDel") > 0) {
        ActivateAlertDiv('visible', 'EditTabAlertDiv', 'Loading...');
    }

    document.body.style.cursor = "wait";

    if (prm.get_isInAsyncPostBack()) {
        args.set_cancel(true);
    }
}

function endRequest(sender, args) {
    if (postbackElement.id.indexOf("btnAddTab") > 0) {
        ActivateAlertDiv('hidden', 'AddTabAlertDiv', '');
    }
    else if (postbackElement.id.indexOf("btnEditTab") > 0) {
        ActivateAlertDiv('hidden', 'EditTabAlertDiv', '');
    }
    else if (postbackElement.id.indexOf("btnAdd") > 0) {
        ActivateAlertDiv('hidden', 'AddTabAlertDiv', '');
        AnnouncementxOnload();
    }
    else if (postbackElement.id.indexOf("gvResult_Edit") > 0) {
        ActivateAlertDiv('hidden', 'EditTabAlertDiv', '');
    }
    else if (postbackElement.id.indexOf("gvResult_Update") > 0) {
        ActivateAlertDiv('hidden', 'EditTabAlertDiv', '');
    }
    else if (postbackElement.id.indexOf("gvResult_Cancel") > 0) {
        ActivateAlertDiv('hidden', 'EditTabAlertDiv', '');
    }
    else if (postbackElement.id.indexOf("lnkbtnDel") > 0) {
        ActivateAlertDiv('hidden', 'EditTabAlertDiv', '');
    }

    document.body.style.cursor = "default";

    if (typeof AnnouncementxOnload == 'function') {
        AnnouncementxOnload();
    }

}


function ActivateAlertDiv(visstring, elem, msg) {
    var adiv = $get(elem);
    adiv.style.visibility = visstring;
    adiv.innerHTML = msg;
}