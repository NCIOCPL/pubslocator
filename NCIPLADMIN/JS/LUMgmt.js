
var prm = Sys.WebForms.PageRequestManager.getInstance();

prm.add_initializeRequest(initializeRequest);
prm.add_endRequest(endRequest);

var postbackElement;

function initializeRequest(sender, args) {

    if (prm.get_isInAsyncPostBack()) {
        args.set_cancel(true);
    }
    
    postbackElement = args.get_postBackElement();
    if (postbackElement.id.indexOf("btnAddTab") > 0)
    {
        ActivateAlertDiv('visible', 'AddTabAlertDiv', 'Loading...');
    }
    else if (postbackElement.id.indexOf("btnEditTab") > 0)
    {
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
    
}

function endRequest(sender, args) 
{
    if (postbackElement.id.indexOf("btnAddTab") > 0)
    {
        ActivateAlertDiv('hidden', 'AddTabAlertDiv', '');
    }
    else if (postbackElement.id.indexOf("btnEditTab") > 0)
    {
        ActivateAlertDiv('hidden', 'EditTabAlertDiv', '');
    }
    else if (postbackElement.id.indexOf("btnAdd") > 0) {
    ActivateAlertDiv('hidden', 'AddTabAlertDiv', '');
    SubjectxOnload();
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

    //if (typeof SubjectxOnload == 'function') {
    //    SubjectxOnload();
    //}
    
}


function ActivateAlertDiv(visstring, elem, msg)
{
     var adiv = $get(elem);
     adiv.style.visibility = visstring;
     adiv.innerHTML = msg;                     
}