/////////////////////////////////////////////////////////

if (ie4 || ns4 || ns6) {
    for (i = 0; i < sitems.length; i++) {
        if (sitems[i][1])
            document.write('<a href="' + sitems[i][1] + '" target="' + target + '">');
        document.write(sitems[i][0]);
        if (sitems[i][1])
            document.write('</a>');
        document.write('<br>\n');
    }
}

function regenerate() {
    window.location.reload();
}

function regenerate2() {
    if (ns4) {
        document.slidemenubar.left = ((parseInt(slidemenu_width) - parseInt(slidemenu_reveal)) * -1);
        document.slidemenubar.visibility = "show";
        setTimeout("window.onresize=regenerate", 400);
    }
}
window.onload = regenerate2;

rightboundary = 0;
leftboundary = (parseInt(slidemenu_width) - parseInt(slidemenu_reveal)) * -1;

if (ie4 || ns6) {
    document.write('</div>');
    themenu = (ns6) ? document.getElementById("slidemenubar2").style : document.all.slidemenubar2.style;
}
else if (ns4) {
    document.write('</layer>');
    themenu = document.layers.slidemenubar;
}

function pull() {
    _isvisible = true;
    if (window.drawit)
        clearInterval(drawit);
    pullit = setInterval("pullengine()", 10);
}

function draw() {
    _isvisible = false;
    clearInterval(pullit);
    drawit = setInterval("drawengine()", 10);
}

function pullengine() {
    if ((ie4 || ns6) && parseInt(themenu.left) < rightboundary)
        themenu.left = parseInt(themenu.left) + 10 + "px";
    else if (ns4 && themenu.left < rightboundary)
        themenu.left += 10;
    else if (window.pullit) {
        themenu.left = 0;
        clearInterval(pullit);
    }
}

function drawengine() {
    if ((ie4 || ns6) && parseInt(themenu.left) > leftboundary)
        themenu.left = parseInt(themenu.left) - 10 + "px";
    else if (ns4 && themenu.left > leftboundary)
        themenu.left -= 10;
    else if (window.drawit) {
        themenu.left = leftboundary;
        clearInterval(drawit);
    }
}

if (ns4)
    document.captureEvents(Event.KEYPRESS);

/*function menuengine(e){
if (ns4||ns6){
if (e.which==120)
pull();
if (e.which==122)
draw();
}
else if (ie4){
if (event.keyCode==120)
pull();
if (event.keyCode==122)
draw();
}
}*/

function clickme() {
    if (ns4 || ns6) {
        if (_isvisible)
            draw();
        else
            pull();
    }
    else if (ie4) {
        if (_isvisible)
            draw();
        else
            pull();
    }
}

function goSearch(arg) {
    var strSearch = "";

    if (arg.name == "optCancerType") {
        //strSearch = strSearch + "CancerType=" + arg.options[arg.selectedIndex].value + "&st=Cancer Type : " + escape(arg.options[arg.selectedIndex].text);
        strSearch = strSearch + "CancerType=" + arg.options[arg.selectedIndex].value + "&st=" + escape(arg.options[arg.selectedIndex].text);
    }
    else {
        strSearch = strSearch + "CancerType="
    }

    if (arg.name == "optSubject") {
        //strSearch = strSearch + "&Subject=" + arg.options[arg.selectedIndex].value + "&st=Subject : " + escape(arg.options[arg.selectedIndex].text);
        strSearch = strSearch + "&Subject=" + arg.options[arg.selectedIndex].value + "&st=" + escape(arg.options[arg.selectedIndex].text);
    }
    else {
        strSearch = strSearch + "&Subject="
    }

    if (arg.name == "optAudience") {
        //strSearch = strSearch + "&Audience=" + arg.options[arg.selectedIndex].value + "&st=Audience : " + escape(arg.options[arg.selectedIndex].text);
        strSearch = strSearch + "&Audience=" + arg.options[arg.selectedIndex].value + "&st=" + escape(arg.options[arg.selectedIndex].text);
    }
    else {
        strSearch = strSearch + "&Audience="
    }

    if (arg.name == "optPublicationFormat") {
        //strSearch = strSearch + "&PublicationFormat=" + arg.options[arg.selectedIndex].value + "&st=Format : " + escape(arg.options[arg.selectedIndex].text);
        strSearch = strSearch + "&ProductFormat=" + arg.options[arg.selectedIndex].value + "&st=" + escape(arg.options[arg.selectedIndex].text);
    }
    else {
        strSearch = strSearch + "&ProductFormat="
    }

    if (arg.name == "optSeries") {
        //strSearch = strSearch + "&Series=" + arg.options[arg.selectedIndex].value + "&st=Collection : " + escape(arg.options[arg.selectedIndex].text);
        strSearch = strSearch + "&Series=" + arg.options[arg.selectedIndex].value + "&st=" + escape(arg.options[arg.selectedIndex].text);
    }
    else {
        strSearch = strSearch + "&Series="
    }

    if (arg.name == "optLanguages") {
        //strSearch = strSearch + "&Languages=" + arg.options[arg.selectedIndex].value + "&st=Language : " + escape(arg.options[arg.selectedIndex].text);
        strSearch = strSearch + "&Languages=" + arg.options[arg.selectedIndex].value + "&st=" + encodeURI(arg.options[arg.selectedIndex].text);
    }
    else {
        strSearch = strSearch + "&Languages="
    }

    searchit(strSearch);
}

//document.onkeypress=menuengine;