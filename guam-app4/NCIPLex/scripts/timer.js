function expiredprompt() {
    windowexpired = dhtmlmodal.open('expiredbox', 'div', 'modalalertdiv', '', 'width=600px,height=350px,center=1,resize=0,scrolling=0')
}
function processit(whichbutton) {
    if (window.idler2) clearTimeout(idler2);
    if (whichbutton == "yes") {
        startIdleTimer();
    }
    if (whichbutton == "no") {
        gobackAttractScreen();
    }
    windowexpired.hide()
}
function gobackAttractScreen() {
    window.location.href = "location.aspx";
}
function expireIdleTimer() {
    clearInterval(idler);
    timer2 = 0;
    idler2 = setTimeout("expireIdle2Timer()", 1000);
    expiredprompt();
}
function resetTimers(event) {
    clearInterval(idler);
    startIdleTimer();
}



