/***********************************************
* Sliding Menu Bar Script 2- © Dynamic Drive (www.dynamicdrive.com)
* This notice must stay intact for use
* Visit http://www.dynamicdrive.com/ for full source code
***********************************************/
var _isvisible = false;
var slidemenu_width = '450px'; //specify width of menu (in pixels)
var slidemenu_reveal = '-20px'; //specify amount that menu should protrude initially
var slidemenu_top = '80px';   //specify vertical offset of menu on page

var ns4 = document.layers ? 1 : 0;
var ie4 = document.all;
var ns6 = document.getElementById && !document.all ? 1 : 0;

if (ie4 || ns6)
    document.write('<div id="slidemenubar2" style="left:' + ((parseInt(slidemenu_width) - parseInt(slidemenu_reveal)) * -1) + 'px; top:' + slidemenu_top + '; width:' + slidemenu_width + '" title="">');
else if (ns4) {
    document.write('<style>\n#slidemenubar{\nwidth:' + slidemenu_width + ';}\n<\/style>\n');
    document.write('<layer id="slidemenubar" left=0 top=' + slidemenu_top + ' width=' + slidemenu_width + ' visibility=hide>');
}

var sitems = new Array();

//If you want the links to load in another frame/window, specify name of target (ie: target="_new")
var target = "";


