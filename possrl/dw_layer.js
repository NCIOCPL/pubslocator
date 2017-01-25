/*************************************************************************
  This code is from Dynamic Web Coding at http://www.dyn-web.com/
  Copyright 2003 by Sharon Paine 
  See Terms of Use at http://www.dyn-web.com/bus/terms.html
  regarding conditions under which you may use this code.
  This notice must be retained in the code as is!
*************************************************************************/

/*  
    dw_layer.js
    a few commonly-used functions for handling positioned divs
    used by Writing to Layers examples at www.dyn-web.com  
*/

layerHandler = {
  getRefs: function (id) {
    var el = (document.getElementById)? document.getElementById(id): (document.all)? document.all[id]: (document.layers)? document.layers[id]: null;
    if (el) el.css = el.style? el.style: el;
    return el;
  },
  
  writeLayer: function (el, cntnt) {
    if (typeof el.innerHTML!="undefined") {
        el.innerHTML = cntnt;
    } else if (document.layers) {
  			el.document.write(cntnt);
  			el.document.close();
    }
  },
  
  shiftTo: function (el,x,y) {
    var px = (document.layers || window.opera)? 0: "px";
    if (x != null) el.css.left = x + px;
    if (y != null) el.css.top = y + px;
  },

  show: function (el) { el.css.visibility = "visible"; },
  hide: function (el) { el.css.visibility = "hidden"; }
}

var imageHandler = {
  imgs: [], path: "",
  preload: function() {
    for (var i=0; arguments[i]; i++) {
      var img = new Image(); img.src = this.path + arguments[i];
      this.imgs[this.imgs.length] = img;
    }
  }
}

// returns amount of vertical scroll
function getScrollY() {
	var sy = 0;
	if (document.documentElement && document.documentElement.scrollTop)
		sy = document.documentElement.scrollTop;
	else if (document.body && document.body.scrollTop) 
		sy = document.body.scrollTop; 
	else if (window.pageYOffset)
		sy = window.pageYOffset;
	else if (window.scrollY)
		sy = window.scrollY;
	return sy;
}

// returns amount of horizontal scroll
function getScrollX() {
	var sx = 0;
	if (document.documentElement && document.documentElement.scrollLeft)
		sx = document.documentElement.scrollLeft;
	else if (document.body && document.body.scrollLeft) 
		sx = document.body.scrollLeft; 
	else if (window.pageXOffset)
		sx = window.pageXOffset;
	else if (window.scrollX)
		sx = window.scrollX;
	return sx;
}