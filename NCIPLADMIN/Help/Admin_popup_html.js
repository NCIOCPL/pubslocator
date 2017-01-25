/* --- Script © 2005-2007 EC Software --- */
var ua = navigator.userAgent;
var dom = (document.getElementById) ? true : false;
var ie4 = (document.all && !dom) ? true : false;
var ie5_5 = ((ua.indexOf("MSIE 5.5")>=0 || ua.indexOf("MSIE 6")>=0) && ua.indexOf("Opera")<0) ? true : false;
var ns4 = (document.layers && !dom) ? true : false;
var offsxy = 6;
function hmshowPopup(e, txt, stick) {
  var tip = '<table  border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFC8" style="{border-width:0px; border-color:#FF0000; border-collapse:collapse;}"><tr valign=top><td>'+ txt + '<\/td><\/tr><\/table>';
  var tooltip = atooltip();
  e = e?e:window.event;
  var mx = ns4 ? e.PageX : e.clientX;
  var my = ns4 ? e.PageY : e.clientY;
  var obj   = (window.document.compatMode && window.document.compatMode == "CSS1Compat") ? window.document.documentElement : window.document.body;
  var bodyl = (window.pageXOffset) ? window.pageXOffset : obj.scrollLeft;
  var bodyt = (window.pageYOffset) ? window.pageYOffset : obj.scrollTop;
  var bodyw = (window.innerWidth)  ? window.innerWidth  : obj.offsetWidth;
  if (ns4) {
    tooltip.document.write(tip);
    tooltip.document.close();
    if ((mx + offsxy + bodyl + tooltip.width) > bodyw) { mx = bodyw - offsxy - bodyl - tooltip.width; if (mx < 0) mx = 0; }
    tooltip.left = mx + offsxy + bodyl;
    tooltip.top = my + offsxy + bodyt;
  }
  else {
    tooltip.innerHTML = tip;
    if (tooltip.offsetWidth) if ((mx + offsxy + bodyl + tooltip.offsetWidth) > bodyw) { mx = bodyw - offsxy - bodyl - tooltip.offsetWidth; if (mx < 0) mx = 0; }
    tooltip.style.left = (mx + offsxy + bodyl)+"px";
    tooltip.style.top  = (my + offsxy + bodyt)+"px";
  }
  with(tooltip) { ns4 ? visibility="show" : style.visibility="visible" }
  if (stick) document.onmouseup = hmhidePopup;
}
function hmhidePopup() {
  var tooltip = atooltip();
  ns4 ? tooltip.visibility="hide" : tooltip.style.visibility="hidden";
}
function atooltip(){
 return ns4 ? document.hmpopupDiv : ie4 ? document.all.hmpopupDiv : document.getElementById('hmpopupDiv')
}
popid_1570347453X="<p class=\"p_Heading3DefTerm\"><span class=\"f_Heading3DefTerm\">Glossaryterm1<\/span><\/p>\n\r<p>Definition goes here<\/p>\n\r<p class=\"p_IndentList3Callout\"><span class=\"f_IndentList3Callout\"><a href=\"javascript:void(0);\" onclick=\"return hmshowPopup(event, popid_2069508284X, true);\" class=\"popuplink\">Link to details<\/a><\/span><\/p>\n\r"
popid_2069508284X="<div style=\"text-align: left; text-indent: 0px; padding: 0px 0px 0px 0px; margin: 0px 0px 0px 0px;\"><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"line-height: normal;\"><tr style=\"vertical-align:baseline\" valign=\"baseline\"><td width=\"13\"><span style=\"font-size: 9pt; font-family: \'Arial Unicode MS\', \'Lucida Sans Unicode\', \'Arial\'; color: #000000;\">&#8226;<\/span><\/td><td><span class=\"f_PopupBox\">You can details details.<\/span><\/td><\/tr><\/table><\/div><div style=\"text-align: left; text-indent: 0px; padding: 0px 0px 0px 0px; margin: 0px 0px 0px 0px;\"><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"line-height: normal;\"><tr style=\"vertical-align:baseline\" valign=\"baseline\"><td width=\"13\"><span style=\"font-size: 9pt; font-family: \'Arial Unicode MS\', \'Lucida Sans Unicode\', \'Arial\'; color: #000000;\">&#8226;<\/span><\/td><td><span class=\"f_PopupBox\">When details details, details details.<\/span><\/td><\/tr><\/table><\/div>"
popid_1570347452X="<p class=\"p_Heading3DefTerm\"><span class=\"f_Heading3DefTerm\">Glossaryterm2<\/span><\/p>\n\r<p>Definition goes here<\/p>\n\r<p class=\"p_IndentList3Callout\"><span class=\"f_IndentList3Callout\"><a href=\"javascript:void(0);\" onclick=\"return hmshowPopup(event, popid_475243099X, true);\" class=\"popuplink\">Link to details<\/a><\/span><\/p>\n\r<p class=\"p_IndentList3Callout\"><span class=\"f_IndentList3Callout\"><a href=\"javascript:void(0);\" onclick=\"return hmshowPopup(event, popid_1911089531, true);\" class=\"popuplink\">Link to extra details<\/a><\/span><\/p>\n\r"
popid_475243099X="<p class=\"p_PopupBox\"><span class=\"f_PopupBox\">To details details:<\/span><\/p>\n\r<div style=\"text-align: left; text-indent: 0px; padding: 0px 0px 0px 0px; margin: 0px 0px 0px 0px;\"><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"line-height: normal;\"><tr style=\"vertical-align:baseline\" valign=\"baseline\"><td width=\"13\"><span style=\"font-size: 9pt; font-family: \'Arial\'; color: #000000;\">1.<\/span><\/td><td><span class=\"f_PopupBox\">Click <\/span><span class=\"f_PopupBox\" style=\"font-weight: bold;\">Details<\/span><span class=\"f_PopupBox\"> to details.<\/span><\/td><\/tr><\/table><\/div><div style=\"text-align: left; text-indent: 0px; padding: 0px 0px 0px 0px; margin: 0px 0px 0px 0px;\"><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"line-height: normal;\"><tr style=\"vertical-align:baseline\" valign=\"baseline\"><td width=\"13\"><span style=\"font-size: 9pt; font-family: \'Arial\'; color: #000000;\">2.<\/span><\/td><td><span class=\"f_PopupBox\">Click <\/span><span class=\"f_PopupBox\" style=\"font-weight: bold;\">Details Details<\/span><span class=\"f_PopupBox\"> to details.<\/span><\/td><\/tr><\/table><\/div>"
popid_1911089531="<p class=\"p_PopupBox\"><span class=\"f_PopupBox\">The details details is details.<\/span><\/p>\n\r"
