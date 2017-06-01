
using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Globalization;
using GlobalUtils;
using System.Collections;
//using LumenWorks.Framework.IO;
//using DocumentFormat.OpenXml;

namespace PubEntAdmin
{
    public class PubEntAdminManager
    {
        #region Constants

        public const string AdminRole = "Administrator";
        public const string DWHStaffRole = "Data Warehouse Staff";
        public const string RURole = "Regular User";

        public const string RECORD_MENUITEM = "RECORD";
        public const string REPORTS_MENUITEM = "REPORTS";
        public const string LOOKUPS_MENUITEM = "LOOKUPS";
        public const string HELP_MENUITEM = "HELP";
        public const string LOGOUT_MENUITEM = "LOGOUT";


        public const string JS = "js";

        public static readonly string strUploadFileThumbnailDir = "ProdImages";
        public static readonly string strUploadFileLargeImageDir = "ProdImages\\LargeImages";
        public static readonly string strUploadFileFeaturedImageDir = "ProdImages\\NCIPLFeatured_Images";
        public static readonly string strUploadFileThumbnailPath = "uploadFileThumbnailPath";

        protected static readonly string strTamperProof = "TamperProof";
        public static readonly string strOracleConnectionInstance = "OracleConn";


        public const string SPELLCHECK2CHECKFIELDSLIST = "src";

        public const string DefAdminPrefixRptName = "PubEntAdmin_";
        public const string DefAdminSuffixRptName = "SearchRpt";
        public const string DefAdminSearchResultRptTitle = "PubEntAdmin Search Result Report";
        public const string DefAdminSponsorTitle = "Master Sponsor Code List";

        public static readonly string strPubGlobalMode = "mode";
        public static readonly string strPubGlobalAMode = "add";
        public static readonly string strPubGlobalVMode = "view";
        public static readonly string strPubGlobalEMode = "edit";

        public static readonly string strSearchAction = "action";
        public static readonly string strSearchRefine = "refine";
        public static readonly string strSearchNew = "new";

        public static readonly string strGlobalMsg = "GlobalMsg";
        public static readonly string strUnauthorizedDetail = "unathorizedDetail";

        public static readonly string strPubID = "pubid";
        public static readonly string strProdID = "prodid";
        public static readonly string strFileID = "FileId";

        public static readonly char charDelim = ',';
        public static readonly string stringDelim = "^~";
        public static readonly string pairDelim = ";";
        public static readonly string indDelim = ",";

        public static readonly string strTabContentCurrActTabIndex = "TabContentCurrActTabIndex";
        public static readonly string strTabContentPrevActTabIndex = "TabContentPrevActTabIndex";

        public static readonly string strSearchCriteria = "SearchCriteria";

        public static readonly string strDefaultoSearchSorting = "LONGTITLE";
        public static readonly string strDefaultSearchSorting = "LONGTITLE";

        public static readonly string strReloadPubHist = "reloadPubHist";
        public static readonly string strReloadRelatedPub = "reloadRelatedPub";
        public static readonly string strReloadRelatedTranslation = "reloadRelatedTranslation";

        public static readonly string strTabContentLUCurrActTabIndex = "TabContentLUCurrActTabIndex";

        public static readonly string strCurrCatName = "CurrCatName";
        public static readonly string strCurrSubcatName = "CurrSubcatName";

        public static readonly string dtSQLDefault = "01/01/1900";
        public static readonly string strDTFormat = "MM/dd/yyyy";

        public static readonly string strNewUpdated_NEW = "New";
        public static readonly string strNewUpdated_UPDATED = "Updated";

        public static readonly string strDisplayStatus_ONLINE = "Online";
        public static readonly string strDisplayStatus_ORDER = "Order";
        public static readonly string strDisplayStatus_ContentAndCover = "Content & Cover";

        public static readonly string strVK_LPType = "type";
        public static readonly string strVKType = "vk";
        public static readonly string strLPType = "lp";
        public static readonly string strInterface = "int";


        public static readonly string NCIPL_INTERFACE = "NCIPL";
        public static readonly string ROO_INTERFACE = "ROO";

        public static readonly string LOOKUP_SUB = "sub";


        public static readonly string PUBENTHISTSTARTYEARNUM = "PubEntHistStartYearNum";
        public static readonly string PUBENTHISTENDYEARNUM = "PubEntHistEndYearNum";

        public static readonly string strNoSearchResults = "No records were returned by the search.";
        public static readonly string strInactivePubs = "Purged/Archived";

        #endregion

        public static ArrayList arrlst;

        #region Static Methods

        public static int PubHistStartYearNum
        {
            get
            {
                return Convert.ToInt32(ConfigurationSettings.AppSettings[PUBENTHISTSTARTYEARNUM]);
            }
        }

        public static int PubEntHistEndYearNum
        {
            get
            {
                return Convert.ToInt32(ConfigurationSettings.AppSettings[PUBENTHISTENDYEARNUM]);
            }
        }

        public static bool TamperProof
        {
            get
            {
                return Convert.ToBoolean(ConfigurationSettings.AppSettings[strTamperProof]);
            }
        }

        public static string UploadFileThumbnailPath
        {
            get
            {
                return ConfigurationSettings.AppSettings[strUploadFileThumbnailPath] + "\\" + strUploadFileThumbnailDir;
            }
        }

        public static string UploadFileLargeImagePath
        {
            get
            {
                return ConfigurationSettings.AppSettings[strUploadFileThumbnailPath] + "\\" + strUploadFileLargeImageDir;
            }
        }

        public static string UploadFileFeaturedIamgePath
        {
            get
            {
                return ConfigurationSettings.AppSettings[strUploadFileThumbnailPath] + "\\" + strUploadFileFeaturedImageDir;
            }
        }

        public static DateTime SQLDefaultDt
        {
            get
            {
                CultureInfo provider = CultureInfo.InvariantCulture;

                return DateTime.ParseExact(dtSQLDefault, strDTFormat, provider);
            }
        }

        public static string EncodedURLWithQS(string pagename, string qs)
        {
            UrlBuilder myUrlBuilder = new UrlBuilder(HttpContext.Current.Request.Url.AbsoluteUri, new Base64Encoder());
            myUrlBuilder.PageName = pagename;
            myUrlBuilder.Query = qs;
            return myUrlBuilder.ToString(true);
        }

        public static void RedirectEncodedURLWithQS(string pagename, string qs)
        {
            UrlBuilder myUrlBuilder = new UrlBuilder(HttpContext.Current.Request.Url.AbsoluteUri, new Base64Encoder());
            myUrlBuilder.PageName = pagename;
            myUrlBuilder.Query = qs;
            myUrlBuilder.Navigate_useNewQuery(true);
        }

        public static bool ContainURLQS(string qsname)
        {
            UrlBuilder myUrlBuilder = new UrlBuilder(HttpContext.Current.Request.Url.AbsoluteUri, new Base64Encoder());
            return myUrlBuilder.QueryString.ContainsKey(qsname);
        }

        public static string GetURLQS(string qsname)
        {
            UrlBuilder myUrlBuilder = new UrlBuilder(HttpContext.Current.Request.Url.AbsoluteUri, new Base64Encoder());
            if (myUrlBuilder.QueryString.ContainsKey(qsname))
            {
                return myUrlBuilder.QueryString[qsname];
            }
            else
            {
                return String.Empty;
            }
        }

        public static void UnathorizedAccess()
        {
            if (PubEntAdminManager.TamperProof)
            {
                UrlBuilder myUrlBuilder = new UrlBuilder(HttpContext.Current.Request.Url.AbsoluteUri, new Base64Encoder());
                myUrlBuilder.PageName = "UnauthorizedAccess.aspx";
                myUrlBuilder.Query = String.Empty;
                myUrlBuilder.Navigate_useNewQuery(true);
            }
            else
            {
                HttpContext.Current.Response.Redirect("UnauthorizedAccess.aspx", true);
            }
        }

        public static string AdminSearchRptName()
        {
            string ret = DefAdminPrefixRptName;
            ret += DefAdminSuffixRptName + "_" +
                DateTime.Now.ToShortDateString().Replace("/", "-");
            return ret;
        }

        public static string Clean(string s)
        {
            string notallowed = ConfigurationManager.AppSettings["PubEntBadCharacters"];
            char[] arr = s.ToLower().ToCharArray();
            string temp = "";
            for (int i = 0; i < arr.Length; i++)
            {
                if (notallowed.IndexOf(s[i]) < 0)
                {
                    temp += s[i];
                }
                else
                {
                    temp += " ";
                }
            }
            return (temp);
        }

        public static bool NoiseWordVal(string s)
        {
            string noise = ConfigurationManager.AppSettings["PubEntNoiseWords"];
            if (noise.ToLower().Trim().Contains("_" + s + "_"))
            {
                return (true);
            }
            else
            {
                return (false);
            }
        }

        public static string StripOutNoise(string s)
        {
            string ret = String.Empty;

            string[] split = s.Split(new string[] {" "}, StringSplitOptions.RemoveEmptyEntries);
            foreach (string l_s in split)
            {
                if (!NoiseWordVal(l_s.ToLower()))
                {
                    if (ret.Length>0)
                        ret+=" ";
                    ret += l_s;
                }
            }

            return ret;
        }

        public static bool LenVal(string s, int len)
        {
            return (s.Length <= len) ? true : false;
        }

        public static bool ContentVal(string s, string regExp)
        {
            return System.Text.RegularExpressions.Regex.
                IsMatch(s, regExp);
        }

        public static bool ContentNumVal(string s)
        {
            if (s.Length > 0)
                return ContentVal(s, @"^\d*[0-9](\.\d*[0-9])?$");
            else
                return true;
        }

        public static bool ContentDateVal(string s)
        {
            try
            {
                DateTime t_dt = DateTime.Parse(s);
                return true;
            }
            catch (FormatException fex)
            {
                return false;
            }
        }

        public static bool OtherVal(string s)
        {
            arrlst = new ArrayList();
            arrlst.Add("<applet");
            arrlst.Add("<body");
            arrlst.Add("<embed");
            arrlst.Add("<frame");
            arrlst.Add("<script");
            arrlst.Add("<frameset");
            arrlst.Add("<html");
            arrlst.Add("<iframe");
            arrlst.Add("<img");
            arrlst.Add("<style");
            arrlst.Add("<layer");
            arrlst.Add("<link");
            arrlst.Add("<ilayer");
            arrlst.Add("<meta");
            arrlst.Add("<object");
            arrlst.Add("1=1");
            arrlst.Add("1=0");

            foreach (object o in arrlst)
            {
                int pos = s.IndexOf(o.ToString());
                if (pos >= 0)
                {
                    //if (s.IndexOf(("\"", 0, pos) < 0)
                    //{
                    //    return true;
                    //}

                    return true;
                }
            }
            return false;
        }

        public static bool OtherVal2(string s)
        {
            arrlst = new ArrayList();
            arrlst.Add("<applet");
            arrlst.Add("<body");
            arrlst.Add("<embed");
            arrlst.Add("<frame");
            arrlst.Add("<script");
            arrlst.Add("<frameset");
            arrlst.Add("<html");
            arrlst.Add("<iframe");
            arrlst.Add("<img");
            arrlst.Add("<style");
            arrlst.Add("<layer");
            arrlst.Add("<link");
            arrlst.Add("<ilayer");
            arrlst.Add("<meta");
            arrlst.Add("<object");           
            arrlst.Add("1=");


            foreach (object o in arrlst)
            {
                int pos = s.IndexOf(o.ToString());
                if (pos >= 0)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool SpecialVal2(string val)
        {
            if (System.Text.RegularExpressions.Regex.
                IsMatch(val, @"('(or|and).+?'='.+?)|('(or|and)[0-9]+=[0-9]+)"))
            {
                return true;
            }

            if (System.Text.RegularExpressions.Regex.
                IsMatch(val, @"('.+?(or|and).+?'='.+?)|('.+?(or|and)[0-9]+=[0-9]+)"))
            {
                return true;
            } 
            return false;

        }

        #endregion

        #region MonitorChanges
        public static void MonitorChanges(Page page, Control wc)
        {
            if (wc == null) return;

            if (wc is CheckBoxList || wc is RadioButtonList || wc is PubEntAdmin.UserControl.ListMultiSelect)
            {
                if (wc is CheckBoxList || wc is RadioButtonList)
                    for (int i = 0; i < ((ListControl)wc).Items.Count; i++)
                    {
                        page.ClientScript.RegisterArrayDeclaration("monitorChangesIDs", "\"" + string.Concat(wc.ClientID, "_", i.ToString()) + "\"");
                        page.ClientScript.RegisterArrayDeclaration("monitorChangesValues", "null");
                    }
                else if (wc is PubEntAdmin.UserControl.ListMultiSelect)
                {
                    for (int i = 0; i < ((PubEntAdmin.UserControl.ListMultiSelect)wc).InnerList.Items.Count; i++)
                    {
                        page.ClientScript.RegisterArrayDeclaration("monitorChangesIDs", "\"" + string.Concat(wc.ClientID, ":", i.ToString()) + "\"");
                        page.ClientScript.RegisterArrayDeclaration("monitorChangesValues", "null");
                    }
                }

            }
            else
            {
                page.ClientScript.RegisterArrayDeclaration("monitorChangesIDs", string.Concat("\"", wc.ClientID, "\""));
                page.ClientScript.RegisterArrayDeclaration("monitorChangesValues", "null");
            }

            AssignMonitorChangeValuesOnPageLoad(page);
        }

        public static void MonitorChanges2(Page page, Control p, Control wc)
        {
            if (wc == null) return;

            if (wc is CheckBoxList || wc is RadioButtonList || wc is PubEntAdmin.UserControl.ListMultiSelect)
            {
                if (wc is CheckBoxList || wc is RadioButtonList)
                {

                    for (int i = 0; i < ((ListControl)wc).Items.Count; i++)
                    {
                        ScriptManager.RegisterArrayDeclaration(p, "monitorTabChangesIDs", "\"" + string.Concat(wc.ClientID, "_", i.ToString()) + "\"");
                        ScriptManager.RegisterArrayDeclaration(p, "monitorTabChangesValues", "null");
                    }
                }
                else if (wc is PubEntAdmin.UserControl.ListMultiSelect)
                {
                    for (int i = 0; i < ((PubEntAdmin.UserControl.ListMultiSelect)wc).InnerList.Items.Count; i++)
                    {
                        ScriptManager.RegisterArrayDeclaration(p, "monitorTabChangesIDs", "\"" + string.Concat(wc.ClientID, ":", i.ToString()) + "\"");
                        ScriptManager.RegisterArrayDeclaration(p, "monitorTabChangesValues", "null");
                    }
                }

            }
            else
            {
                ScriptManager.RegisterArrayDeclaration(p, "monitorTabChangesIDs", string.Concat("\"", wc.ClientID, "\""));
                ScriptManager.RegisterArrayDeclaration(p, "monitorTabChangesValues", "null");
            }

            ScriptManager.GetCurrent(page).RegisterDispose(p, "monitorTabChangesIDs = undefined;");
            ScriptManager.GetCurrent(page).RegisterDispose(p, "monitorTabChangesValues = undefined;");

            AssignMonitorTabChangeValuesOnPageLoad(p);
        }

        public static void AssignMonitorTabChangeValuesOnPageLoadInReadMode(Control p)
        {
            ScriptManager.RegisterClientScriptBlock(p,
                p.GetType(), p.GetType().ToString()+"_ClientScript_monitorChangesAssignmentFunction",
                @"
					function assignInitialValuesForMonitorTabChanges() {}", true);
        }

        public static void AssignMonitorTabChangeValuesOnPageLoad(Control p)
        {
            ScriptManager.RegisterClientScriptBlock(p,
                p.GetType(), p.GetType().ToString()+"_ClientScript_monitorChangesAssignmentFunction",
                @"
					function assignInitialValuesForMonitorTabChanges() {
                            /*
                            alert('in assignInitialValuesForMonitorTabChanges '+monitorTabChangesIDs.length);
                            
                            var tabcontname='';
                            for (var z = 0; z < monitorTabChangesIDs.length; z++){
                                tabcontname += monitorTabChangesIDs[z] +'\n';
                            }
                            alert(tabcontname);
                            */
						    for (var i = 0; i < monitorTabChangesIDs.length; i++){
                                var elem;
                                var opt;
                                var elemOptIdx;

                                  if (monitorTabChangesIDs[i].indexOf('lstMultiSelect')>=0)
                                  {
                                    elemOptIdx = monitorTabChangesIDs[i].indexOf(':');
                                    elem = document.getElementById(monitorTabChangesIDs[i].
                                            substr(0,elemOptIdx));
                                    opt = monitorTabChangesIDs[i].substr(elemOptIdx+1,
                                            monitorTabChangesIDs[i].length-(elemOptIdx+1));
                                  }
                                  else
                                  {
                                    elem = document.getElementById(monitorTabChangesIDs[i]);
                                  }
                                  
						          if (elem) 
                                  {
                                    if (elem.type == 'checkbox' || elem.type == 'radio') 
						                monitorTabChangesValues[i] = elem.checked;
                                    else if (elem.type == 'select')
                                    {
                                        if (monitorTabChangesIDs[i].indexOf('lstMultiSelect')>=0)
                                            monitorTabChangesValues[i] = elem.options[opt].selected;
                                        else
                                        {
                                            if (elem.selectedIndex >= 0)
                                                monitorTabChangesValues[i] = elem.options[elem.selectedIndex].value;
                                            else
                                                monitorTabChangesValues[i] = '';
                                        }
                                    }
                                    else if (elem.type == 'select-one')
                                    {
                                        
                                        if (elem.selectedIndex > 0)
                                        {
                                            //alert(i+' '+elem.selectedIndex+' '+elem.options[elem.selectedIndex].text+' '+elem.options[elem.selectedIndex].value);
                                            monitorTabChangesValues[i] = elem.options[elem.selectedIndex].value;
                                        }
                                        else
                                            monitorTabChangesValues[i] = 0;
                                        
                                    }
                                    else 
                                    {
                                        
                                        monitorTabChangesValues[i] = elem.value;
                                    }
                                  }
						    }
						 }
				  ", true);
        }

        public static void AssignMonitorChangeValuesOnPageLoad(Page page)
        {
            if (!page.ClientScript.IsStartupScriptRegistered("monitorChangesAssignment"))
            {
                page.ClientScript.RegisterStartupScript(page.GetType(),"monitorChangesAssignment",
                    @"<script language=""JavaScript"">
						assignInitialValuesForMonitorChanges();
					  </script>");

                page.ClientScript.RegisterClientScriptBlock(page.GetType(),"monitorChangesAssignmentFunction",
                    @"<script language=""JavaScript"">
						function assignInitialValuesForMonitorChanges() {
						    for (var i = 0; i < monitorChangesIDs.length; i++){
                                var elem;
                                var opt;
                                var elemOptIdx;

                                  if (monitorChangesIDs[i].indexOf('lstMultiSelect')>=0)
                                  {
                                    elemOptIdx = monitorChangesIDs[i].indexOf(':');
                                    elem = document.getElementById(monitorChangesIDs[i].
                                            substr(0,elemOptIdx));
                                    opt = monitorChangesIDs[i].substr(elemOptIdx+1,
                                            monitorChangesIDs[i].length-(elemOptIdx+1));
                                  }
                                  else
                                  {
                                    elem = document.getElementById(monitorChangesIDs[i]);
                                  }
                                  
						          if (elem) 
                                  {
                                    if (elem.type == 'checkbox' || elem.type == 'radio') 
						                monitorChangesValues[i] = elem.checked;
                                    else if (elem.type == 'select')
                                        monitorChangesValues[i] = elem.options[opt].selected;
                                    else monitorChangesValues[i] = elem.value;
                                  }
						    }
						 }
               
						var needToConfirm = true;
						window.onbeforeunload = confirmClose;
						
						function confirmClose() {
                            //alert('needToConfirm: '+needToConfirm);
							if (!needToConfirm) return;
							if ((typeof monitorChangesValues != 'undefined') ||
                                ((typeof monitorChangesValues != 'undefined') && (typeof monitorTabChangesValues != 'undefined')))
                            {
                                var combineMonitorChangesValues = monitorChangesValues;
                                var combineMonitorChangesIDs = monitorChangesIDs;

                                if (typeof monitorTabChangesValues != 'undefined')
                                {
                                    combineMonitorChangesValues = monitorChangesValues.concat(monitorTabChangesValues);
                                    combineMonitorChangesIDs = monitorChangesIDs.concat(monitorTabChangesIDs);
                                }

							    for (var i = 0; i < combineMonitorChangesValues.length; i++) {
								    var elem;
                                    var opt;
                                    var elemOptIdx;
                                
                                      if (combineMonitorChangesIDs[i].indexOf('lstMultiSelect')>=0)
                                      {
                                        elemOptIdx = combineMonitorChangesIDs[i].indexOf(':');
                                        elem = document.getElementById(combineMonitorChangesIDs[i].
                                                substr(0,elemOptIdx));
                                        opt = combineMonitorChangesIDs[i].substr(elemOptIdx+1,
                                                combineMonitorChangesIDs[i].length-(elemOptIdx+1));
                                      }
                                      else
                                      {
                                        elem = document.getElementById(combineMonitorChangesIDs[i]);
                                      }
                                  
								    if (elem) 
									    if (((elem.type == 'checkbox' || elem.type == 'radio' ) && elem.checked != combineMonitorChangesValues[i]))
									    { 
                                            needToConfirm = false; 
										    setTimeout('resetFlag()', 750); 
										    return ""You have modified the data entry fields since last saving. If you leave this page, any changes will be lost."";
									    }
                                        else if (elem.type == 'select' && elem.options[opt].selected != combineMonitorChangesValues[i])
                                        {
                                            needToConfirm = false; 
										    setTimeout('resetFlag()', 750); 
										    return ""You have modified the data entry fields since last saving. If you leave this page, any changes will be lost."";
                                        }
                                        else if (elem.type == 'select-one')
                                        {
                                            if (elem.selectedIndex > 0 && (elem.options[elem.selectedIndex].value != combineMonitorChangesValues[i]))
                                            {
                                                needToConfirm = false; 
										        setTimeout('resetFlag()', 750); 
										        return ""You have modified the data entry fields since last saving. If you leave this page, any changes will be lost."";
                                            }    
                                        }
                                        else if (elem.type != 'checkbox' && elem.type != 'radio' && elem.value != combineMonitorChangesValues[i])
                                        {
                                            if (combineMonitorChangesIDs[i].indexOf('txtTotalPage')>=0)
                                            {
                                                if (elem.value.trim() != 'N/A' && elem.value != combineMonitorChangesValues[i])
                                                {
                                                    //alert('N/A' +' '+elem.value +' '+combineMonitorChangesValues[i]);
                                                    needToConfirm = false; 
                                                    setTimeout('resetFlag()', 750); 
    									            return ""You have modified the data entry fields since last saving. If you leave this page, any changes will be lost."";
                                                }
                                            }
                                            else
                                            {
                                                //alert(eval(elem.value != 'N/A') + ' '+eval(elem.value != combineMonitorChangesValues[i]));
                                                //alert (elem.type+' '+elem.value+' '+combineMonitorChangesValues[i]+' '+eval(combineMonitorChangesValues[i]==null));
                                                needToConfirm = false; 
                                                setTimeout('resetFlag()', 750); 
    									        return ""You have modified the data entry fields since last saving. If you leave this page, any changes will be lost."";
                                            }
                                            
                                        }
							    }
                            }
						}
               
                        function confirmTabClose() {
                            //alert('in confirmTabClose '+needToConfirm);
                            /*
                            if (typeof monitorTabChangesValues != 'undefined')
                                alert('in confirmTabClose '+monitorTabChangesValues.length);
                            else
                                alert('in confirmTabClose '+typeof monitorTabChangesValues);
                            */

                            if (!needToConfirm) return true;
							else
                            {
                                //alert('in confirmTabClose checking'+needToConfirm);
							    if (typeof monitorTabChangesValues != 'undefined')
                                {
							        for (var i = 0; i < monitorTabChangesValues.length; i++) {
								        var elem;
                                        var opt;
                                        var elemOptIdx;
                                    
                                          if (monitorTabChangesIDs[i].indexOf('lstMultiSelect')>=0)
                                          {
                                            elemOptIdx = monitorTabChangesIDs[i].indexOf(':');
                                            elem = document.getElementById(monitorTabChangesIDs[i].
                                                    substr(0,elemOptIdx));
                                            opt = monitorTabChangesIDs[i].substr(elemOptIdx+1,
                                                    monitorTabChangesIDs[i].length-(elemOptIdx+1));
                                          }
                                          else
                                          {
                                            elem = document.getElementById(monitorTabChangesIDs[i]);
                                          }
                                      
								        if (elem)
                                        { 
                                            /*
                                            if (elem.type == 'checkbox' || elem.type == 'radio' )
                                                alert ('in elem :'+monitorTabChangesIDs[i]+'\n'+elem.type+' '+elem.checked+' '+monitorTabChangesValues[i]);
                                            else
                                                alert ('in elem :'+monitorTabChangesIDs[i]+'\n'+elem.type+' '+elem.value+' '+monitorTabChangesValues[i]);
                                            */

                                            //alert('confirmTabClose - '+elem.type);
									        if (((elem.type == 'checkbox' || elem.type == 'radio' ) && elem.checked != monitorTabChangesValues[i]))
									        { 
                                                //alert('confirmTabClose -> checkbox|radio');
                                                needToConfirm = false; 
										        setTimeout('resetFlag()', 750); 
										        return confirm( navAwyTabLang());
									        }
                                            else if (elem.type == 'select' && elem.options[opt].selected != monitorTabChangesValues[i])
                                            {
                                                needToConfirm = false; 
										        setTimeout('resetFlag()', 750); 
										        return confirm( navAwyTabLang());
                                            }
                                            else if (elem.type == 'select-one')
                                            {
                                                if (elem.selectedIndex >= 0 && (elem.options[elem.selectedIndex].value != monitorTabChangesValues[i]))
                                                {
                                                    //alert(i+' '+elem.selectedIndex+' * '+elem.options[elem.selectedIndex].text+' * '+monitorTabChangesValues[i]);
                                                    needToConfirm = false; 
									                setTimeout('resetFlag()', 750); 
									                return confirm( navAwyTabLang());
                                                }  
                                            }
                                            else if (elem.type != 'checkbox' && elem.type != 'radio' && elem.value != monitorTabChangesValues[i])
                                            {
                                                //alert('confirmTabClose X checkbox|radio'+elem.type);
                                                //alert('elem.value: '+elem.value+' monitorTabChangesValues[i]: '+monitorTabChangesValues[i]);
                                                needToConfirm = false; 
                                                setTimeout('resetFlag()', 750); 
									            return confirm( navAwyTabLang());
                                            }
                                            
                                        }                                            
							        }
                                    return true;
                                }
                            }
                                
						}

						function resetFlag() { needToConfirm = true; }

                        function navAwyTabLang()
                        {  
                            return 'Are you sure you want to navigate away from this tab?\n\n'+
                                    'You have modified the data entry fields since last saving. If you leave this tab, any changes on this tab will be lost.\n\n'+
                                    'Press OK to continue, or Cancel to stay on the current tab.';
                        }
                       
					</script>");
            }
        }

        public static void BypassModifiedMethod(WebControl wc, bool ClientSideValidationConcern)
        {
            //if (ClientSideValidationConcern)
            //    wc.Attributes["onclick"] = string.Concat("javascript:", GetBypassModifiedMethodScriptWithClientSideValidationConcern());
            //else
                wc.Attributes["onclick"] = string.Concat("javascript:", GetBypassModifiedMethodScript());
        }

        public static void BypassModifiedMethod(HtmlControl wc, bool ClientSideValidationConcern)
        {
            //if (ClientSideValidationConcern)
            //    wc.Attributes["onclick"] = string.Concat("javascript:", GetBypassModifiedMethodScriptWithClientSideValidationConcern());
            //else
                wc.Attributes["onclick"] = string.Concat("javascript:", GetBypassModifiedMethodScript());
        }

        public static string GetBypassModifiedMethodScript()
		{
			return "needToConfirm = false;";
		}
        #endregion

        #region CharCountDown
        public static void RegisterCharCountDown(WebControl ctrl, int MaxLength)
        {
            //ctrl.Attributes.Add("onkeypress", "return taLimit(" + MaxLength + ")");
            ctrl.Attributes["onkeypress"] += "return taLimit(" + MaxLength + ");";
            ctrl.Attributes.Add("onpaste", "return taCount(" + MaxLength + ");");
            ctrl.Attributes.Add("onkeyup", "return taCount(" + MaxLength + ");");
            ctrl.Attributes.Add("onkeydown", "return taCount(" + MaxLength + ");");
            ctrl.Attributes.Add("onblur", "return taGone();");
        }
        #endregion

        #region Export to excel

        //Export to Excel from a gridview
        public static void ExportToExcel(GridView gv, Page pag)
        {
            char c = (char)34;
            char bk = '\n';

            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.Buffer = true;
            System.Web.HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=RotationPublications.xls");
            System.Web.HttpContext.Current.Response.Write("<!DOCTYPE html>" + bk +
                           "<html>" + bk +
                           "<head>" + bk + "<meta http-equiv='Content-Type' content='text/html; charset=utf-8'>" + bk + "</head><body>");

            pag.EnableViewState = false;

            System.IO.StringWriter sw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(sw);
            HtmlForm frm = new HtmlForm();
            pag.Controls.Add(frm);
            frm.Controls.Add(gv);
            frm.RenderControl(htw);

            System.Web.HttpContext.Current.Response.Write(sw.ToString());
            System.Web.HttpContext.Current.Response.End();

        }



        public static bool ExportGridViewToExcel(GridView gv, string strFileName, string strTitle, System.Web.HttpResponse Response)
        {
            //string templateFile = "temp_Generic.xlsx";
            //string exportFile = strFileName;
            //int headerOffset = 1;
            //int columnOffset = 0;
            //System.Data.DataTable dt = gv.DataSource as DataTable;


            //string tempFile = AppDomain.CurrentDomain.BaseDirectory + "ExcelTemplates\\" + templateFile;

            //var excelWorkbook = new ClosedXML.Excel.XLWorkbook(tempFile);
            //var excelWorksheet = excelWorkbook.Worksheet(1);

            ////write data
            //int offsetRow = headerOffset;  //header rows in template
            //int offsetColumn = columnOffset; //column data to skip 
            //int rowIndex = 1;
            //foreach (DataRow dr in dt.Rows)
            //{
            //    ++rowIndex;
            //    for (int i = 1; i < dt.Columns.Count + 1 - offsetColumn; i++)
            //    {
            //        excelWorksheet.Cell(rowIndex + offsetRow - 1, i).Value = dr[i - 1 + offsetColumn].ToString();
            //    }
            //}

            //// export to Excel
            //try
            //{
            //    System.IO.MemoryStream stream = new System.IO.MemoryStream();
            //    excelWorkbook.SaveAs(stream);
            //    stream.Position = 0;
            //    Response.ClearContent();
            //    Response.Clear();
            //    Response.Buffer = true;
            //    Response.Charset = "";
            //    Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            //    Response.AddHeader("content-disposition", "attachment; filename=" + exportFile);
            //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //    Response.BinaryWrite(stream.ToArray());
            //    Response.Flush();
            //    Response.End();
            //    return true;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}           

            return true;
        }    

        #endregion

    }
}
