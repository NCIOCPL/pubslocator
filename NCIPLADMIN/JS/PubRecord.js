
var prm = Sys.WebForms.PageRequestManager.getInstance();

prm.add_initializeRequest(initializeRequest);
prm.add_endRequest(endRequest);

var postbackElement;

function initializeRequest(sender, args) {
    postbackElement = args.get_postBackElement();
    
    if (postbackElement.id.indexOf("btnNCIPL") > 0) {
        ActivateAlertDiv('visible', 'NCIPLAlertDiv', 'Loading...');
    }
    else if (postbackElement.id.indexOf("btnCatalog") > 0) {
        ActivateAlertDiv('visible', 'CatalogAlertDiv', 'Loading...');
    }
    else if (postbackElement.id.indexOf("btnPubHist") > 0) {
        ActivateAlertDiv('visible', 'PubHistAlertDiv', 'Loading...');
    }
    else if (postbackElement.id.indexOf("btnROO") > 0) {
        ActivateAlertDiv('visible', 'ROOAlertDiv', 'Loading...');
    }
    else if (postbackElement.id.indexOf("btnExh") > 0) {
        ActivateAlertDiv('visible', 'ExhAlertDiv', 'Loading...');
    }
    else if (postbackElement.id.indexOf("btnCmt") > 0) {
        ActivateAlertDiv('visible', 'CmtAlertDiv', 'Loading...');
    }
    else if (postbackElement.id.indexOf("btnAttach") > 0) {
        ActivateAlertDiv('visible', 'AttachAlertDiv', 'Loading...');
    }
    else if (postbackElement.id.indexOf("btnRelated") > 0) {
        ActivateAlertDiv('visible', 'RelatedAlertDiv', 'Loading...');
    }
    else if (postbackElement.id.indexOf("btnTranslation") > 0) {
        ActivateAlertDiv('visible', 'TranslationAlertDiv', 'Loading...');
    }

    
    
    document.body.style.cursor = "wait";
    if (prm.get_isInAsyncPostBack()) {
        args.set_cancel(true);
    }

}

function endRequest(sender, args) {

    if (postbackElement.id.indexOf("btnNCIPL") > 0) {
        ActivateAlertDiv('hidden', 'NCIPLAlertDiv', '');
        //if (typeof monitorTabChangesIDs != 'undefined')
            assignInitialValuesForMonitorTabChanges();
    }
    else if (postbackElement.id.indexOf("btnCatalog") > 0) {
        ActivateAlertDiv('hidden', 'CatalogAlertDiv', '');
        assignInitialValuesForMonitorTabChanges();
        
    }
    else if (postbackElement.id.indexOf("btnPubHist") > 0) {
        ActivateAlertDiv('hidden', 'PubHistAlertDiv', '');
        //if (typeof monitorTabChangesIDs != 'undefined')
            assignInitialValuesForMonitorTabChanges();
    }
    else if (postbackElement.id.indexOf("btnROO") > 0) {
        ActivateAlertDiv('hidden', 'ROOAlertDiv', '');
        //if (typeof monitorTabChangesIDs != 'undefined')
            assignInitialValuesForMonitorTabChanges();
    }
    else if (postbackElement.id.indexOf("btnExh") > 0) {
        ActivateAlertDiv('hidden', 'ExhAlertDiv', '');
        //if (typeof monitorTabChangesIDs != 'undefined')
            assignInitialValuesForMonitorTabChanges();
    }
    else if (postbackElement.id.indexOf("btnCmt") > 0) {
        ActivateAlertDiv('hidden', 'CmtAlertDiv', '');
        //if (typeof monitorTabChangesIDs != 'undefined')
            assignInitialValuesForMonitorTabChanges();
    }
    else if (postbackElement.id.indexOf("btnAttach") > 0) {
        ActivateAlertDiv('hidden', 'AttachAlertDiv', '');
        //if (typeof monitorTabChangesIDs != 'undefined')
            assignInitialValuesForMonitorTabChanges();
    }
    else if (postbackElement.id.indexOf("btnRelated") > 0) {
        ActivateAlertDiv('hidden', 'RelatedAlertDiv', '');
        //if (typeof monitorTabChangesIDs != 'undefined')
            assignInitialValuesForMonitorTabChanges();
    }
    else if (postbackElement.id.indexOf("btnTranslation") > 0) {
        ActivateAlertDiv('hidden', 'TranslationAlertDiv', '');
        //if (typeof monitorTabChangesIDs != 'undefined')
            assignInitialValuesForMonitorTabChanges();
    }

    //LiveIntTabOnload();
    needToConfirm = true;
    document.body.style.cursor = "default";


}

function ActivateAlertDiv(visstring, elem, msg) {
    var adiv = $get(elem);
    adiv.style.visibility = visstring;
    adiv.innerHTML = msg;
}

function handleEnter(field, event) {
    var keyCode = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;
    //alert(keyCode);
    if (keyCode == 13) {
        var i;
        var pass = false;
        //alert(field.form.elements.length);
        for (i = 0; i < field.form.elements.length; i++) {
            
            if (field == field.form.elements[i]) {
                //alert(field.form.elements[i].id);
                pass = true;
                var j = (i + 1) % field.form.elements.length;
                if (field.form.elements[j].style.display == '') {
                    //alert('next is: ' + field.form.elements[j].id);
                    if (field.form.elements[j].type == "text" || field.form.elements[j].type == "select-one" ||
                        field.form.elements[j].type == "radio" || field.form.elements[j].type == "select" ||
                        field.form.elements[j].type == "textarea") {
                        //alert('1st - is type ' + field.form.elements[j].id);
                        try {
                            field.form.elements[j].focus();
                            break;
                        }
                        catch (e) { }
                    }
                    else
                        continue;
                }
                else {
                    continue;
                }
            }
            else {
                if (pass) {
                    //alert('in pass: ' + field.form.elements[i].id);
                    if (field.form.elements[i].style.display == '') {
                        if (field.form.elements[i].type == "text" || field.form.elements[i].type == "select-one" ||
                            field.form.elements[i].type == "radio" || field.form.elements[i].type == "select" ||
                            field.form.elements[i].type == "textarea") {
                            //alert('2nd - is type ' + field.form.elements[i].id);
                            try {
                                field.form.elements[i].focus();
                                break;
                            }
                            catch (e) { }
                        }
                        else
                            continue;
                    }
                    else {
                        continue;
                    }
                }
            }
        }
        return false;
    }
    else
        return true;
}
