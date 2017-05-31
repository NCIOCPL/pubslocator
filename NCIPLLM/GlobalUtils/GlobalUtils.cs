using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//Manually added
using System.Web.UI.WebControls;
using System.Drawing;
using System.Configuration;
using System.Text;
using PubEnt.BLL;
using System.Collections;

namespace PubEnt.GlobalUtils
{
    public class Const
    {
        public static int XPOMaxQuantity = 30;
        public static string[] XPOs = { "AE", "AA", "AP", "AS", "FM", "GU", "MH", "MP", "PW", "PR", "VI" };
    }
    public class Utils
    {
        public static bool ValidatePassword(string passWord)
        {
            char[] special = { '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '_', '-', '+', '=' }; // or whatever

            if (passWord.Length < 8) return false;

            int validConditions = 0;

            //check containing lower case letter
            foreach (char c in passWord)
            {
                if (c >= 'a' && c <= 'z')
                {
                    validConditions++;
                    break;
                }
            }

            //check containing upper case letter
            foreach (char c in passWord)
            {
                if (c >= 'A' && c <= 'Z')
                {
                    validConditions++;
                    break;
                }
            }

            //check containing number
            foreach (char c in passWord)
            {
                if (c >= '0' && c <= '9')
                {
                    validConditions++;
                    break;
                }
            }

            //check containing special letter
            if (passWord.IndexOfAny(special) != -1)
                validConditions++;

            if (validConditions >= 3)
                return true;
            else
                return false;
        } 

        public static string UserIPAddrress()
        {
            string temp = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (temp == null || temp == "" || temp.ToLower() == "unknown")
                temp = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            return (temp);
        }
        public static bool isLoggedin()
        {
            //***EAC we may get a request to use authentication cookie later, for now use sessionsvars
            if (HttpContext.Current.Session["NCIPL_User"] != null &&
                HttpContext.Current.Session["NCIPL_User"].ToString().Length > 0)
                return (true);
            else
                return (false);
        }
        public static string LoggedinUser()
        {
            //***EAC we may get a request to use authentication cookie later, for now use sessionsvars
            return HttpContext.Current.Session["NCIPL_User"].ToString();
        }
        public static string LoggedinRole()
        {
            //***EAC we may get a request to use authentication cookie later, for now use sessionsvars
            return HttpContext.Current.Session["NCIPL_Role"].ToString();
        }
        public static void LogoutCurrentUser()
        {
            //***EAC we may get a request to use authentication cookie later, for now use sessionsvars
            ClearAllSessions();
        }
        public static void ClearAllSessions()
        {
            HttpContext.Current.Session["NCIPL_REGISTERREFERRER"] = "~/home.aspx"; //Default value
            ClearAuthenticationSessions();
            ClearCustomerSessions();
            ClearSearchSessions();
        }
        public static void ClearAuthenticationSessions()
        {
            HttpContext.Current.Session["NCIPL_User"] = "";
            HttpContext.Current.Session["NCIPL_Role"] = "";
        }
        public static void ClearCustomerSessions()
        {
            HttpContext.Current.Session["NCIPL_Pubs"] = "";
            HttpContext.Current.Session["NCIPL_Qtys"] = "";
            HttpContext.Current.Session["NCIPL_cart"] = null;       //destroy
            HttpContext.Current.Session["NCIPL_shipto"] = null;     //destroy
            HttpContext.Current.Session["NCIPL_billto"] = null;     //destroy
            HttpContext.Current.Session["NCIPL_cc"] = null;         //destroy
            HttpContext.Current.Session["NCIPL_DONE"] = "";         //neither done nor finished
        }
        public static void ClearSearchSessions()
        {
            HttpContext.Current.Session["PUBENT_SearchKeyword"] = "";
            HttpContext.Current.Session["PUBENT_TypeOfCancer"] = "";
            HttpContext.Current.Session["PUBENT_Subject"] = "";
            HttpContext.Current.Session["PUBENT_Audience"] = "";
            HttpContext.Current.Session["PUBENT_ProductFormat"] = "";
            HttpContext.Current.Session["PUBENT_Language"] = "";
            HttpContext.Current.Session["PUBENT_StartsWith"] = "";
            HttpContext.Current.Session["PUBENT_Series"] = ""; //Or collection
            HttpContext.Current.Session["PUBENT_NewOrUpdated"] = "";
            HttpContext.Current.Session["PUBENT_Race"] = "";
        }
        //Populates the Placehoder on the caller page with the message corresponding to the Message Id passed.
        public static void DisplayMessage(ref PlaceHolder ctrlPlaceHolder, int MessageId)
        {   
            string MessageText = "";
            switch (MessageId)
            {
                case 1:
                    MessageText = "The shopping cart is empty.";
                    break;
                case 2:
                    MessageText = "Please enter a valid quantity.";
                    break;
                default:
                    MessageText = "";
                    break;

            }

            if (MessageText.Length > 0 && MessageId == 2)
            {

                //Label MessageLabel = new Label();
                //MessageLabel.ID = "msglabelid";
                //MessageLabel.Text = MessageText;
                //MessageLabel.ForeColor = Color.Red;

                //Panel phPanelParent = new Panel();
                //phPanelParent.Style.Add("padding-top", "5px");

                //Panel phPanel = new Panel();
                //phPanel.Style.Add("padding-top", "5px");
                //phPanel.Style.Add("padding-bottom", "5px");
                ////phPanel.Style.Add("background-color", "silver");
                //phPanel.Style.Add("text-align", "center");
                //phPanel.Style.Add("text-color", "red");
                //phPanel.Style.Add("width", "600px");

                //phPanel.Controls.Add(MessageLabel);
                //phPanelParent.Controls.Add(phPanel);
                //ctrlPlaceHolder.Controls.Add(phPanelParent);

                Label MessageLabel = new Label();
                MessageLabel.ID = "msglabelid";
                MessageLabel.Text = MessageText;
                MessageLabel.ForeColor = Color.Red;
                ctrlPlaceHolder.Controls.Add(MessageLabel);
            }
        }
        
        public string Clean(string s)
        {
            //***EAC probably smart to put this in .config later
            //const string notallowed = "~!@#$%^&*()_+`=:\";<>,.?/{}|[]\\";
            string notallowed = ConfigurationManager.AppSettings["PubEntBadCharacters"];
            char[] arr = s.ToLower().ToCharArray();
            string temp = "";
            for (int i = 0; i < arr.Length; i++)
            {
                if (notallowed.IndexOf(s[i]) < 0)
                {
                    temp += s[i];
                }
                else temp += " ";
            }
            return (temp);
        }
        
        public Boolean NoiseWord(string s)
        {
            //const string noise = "_about_1_after_2_all_also_3_an_4_and_5_another_6_any_7_are_8_as_9_at_0_be_$_because_been_before_being_between_both_but_by_came_can_come_could_did_do_does_each_else_for_from_get_got_has_had_he_have_her_here_him_himself_his_how_if_in_into_is_it_its_just_like_make_many_me_might_more_most_much_must_my_never_no_now_of_on_only_or_other_our_out_over_re_said_same_see_should_since_so_some_still_such_take_than_that_the_their_them_then_there_these_they_this_those_through_to_too_under_up_use_very_want_was_way_we_well_were_what_when_where_which_while_who_will_with_would_you_your_a_b_c_d_e_f_g_h_i_j_k_l_m_n_o_p_q_r_s_t_u_v_w_x_y_z_";
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

        /*NCIPL_CC - Apparently used only from cart.aspx/cs*/
        public bool IsQtyValueValid(string val, string limit)
        {
            bool boolValidVal = false;
            if (!string.IsNullOrEmpty(val))
            {
                try
                {
                    //Int32.Parse(QuantityOrdered.Text);
                    if (Int32.Parse(val) <= Int32.Parse(limit) && Int32.Parse(val) > 0)
                    {   if (Int32.Parse(val) == 0)
                            boolValidVal = false;
                        else
                            boolValidVal = true;
                    }
                    else
                        boolValidVal = false;
                }
                catch (FormatException)
                {
                    boolValidVal = false;
                }
            }
            else
                boolValidVal = false;

            return boolValidVal;
        }

        public string GetTabHtmlMarkUp(string pubsincart, string pagename)
        {
            string tabmarkup = "";
            string litText1 = ""; //CR-28 "<li></li>";
            string litText2 = ""; //CR-28  "<li></li>";
            string litText3 = ""; //CR-28  "<li></li>";
            string litText4 = ""; //NCIPL_CC
            
            int intTotalNum = 0;
            
            if (pubsincart.Length > 0)
            {
                string[] pubs = pubsincart.Split(new Char[] { ',' });
                for (int i = 0; i < pubs.Length; i++)
                {
                    if (pubs[i].Length > 0)
                        intTotalNum += Int32.Parse (pubs[i]);
                }
                //if (pubs != null)
                //    intTotalNum = pubs.Length;
            }

            /*Commenting for CR-28, no tabs anymore*/
            //switch (pagename)
            //{
            //    case "home":
            //        litText1 = @"<li id=""selected""><a href=""home.aspx"">Home</a></li>";
            //        litText2 = @"<li><a href=""nciplselfprint.aspx"">Self-Printing Options</a></li>";
            //        litText3 = @"<li><span id=""shopcart""><a href=""cart.aspx"">" + "Shopping Cart (" + intTotalNum.ToString() + ") </a></span></li>";
            //        break;

            //    case "selfprint":
            //        litText1 = @"<li><a href=""home.aspx"">Home</a></li>";
            //        litText2 = @"<li id=""selected""><a href=""nciplselfprint.aspx"">Self-Printing Options</a></li>";
            //        litText3 = @"<li><span id=""shopcart""><a href=""cart.aspx"">" + "Shopping Cart (" + intTotalNum.ToString() + ") </a></span></li>";
            //        break;

            //    case "cart":
            //        litText1 = @"<li><a href=""home.aspx"">Home</a></li>";
            //        litText2 = @"<li><a href=""nciplselfprint.aspx"">Self-Printing Options</a></li>";
            //        litText3 = @"<li id=""selected""><span id=""shopcart""><a href=""cart.aspx"">" + "Shopping Cart (" + intTotalNum.ToString() + ") </a></span></li>";
            //        break;

            //    default:
            //        litText1 = @"<li><a href=""home.aspx"">Home</a></li>";
            //        litText2 = @"<li><a href=""nciplselfprint.aspx"">Self-Printing Options</a></li>";
            //        litText3 = @"<li><span id=""shopcart""><a href=""cart.aspx"">" + "Shopping Cart (" + intTotalNum.ToString() + ") </a></span></li>";
            //        break;

            //}

            //string tag = @"<div id=""navmenu"">";
            //tabmarkup = tag + "<ul>" + litText1 + litText2 + litText3 + "</ul>" + "</div>";
            //CR-28 tabmarkup = "<ul>" + litText1 + litText2 + litText3 + "</ul>";

            string carttext = "";
            if (intTotalNum == 1)
                carttext = "1 Item in your cart ";
            else
                carttext = intTotalNum.ToString() + " Items in your cart ";
            //carttext = "10000 Items in your cart ";
            string separator = @"<div style=""height:2px; border-top:solid 1px #bbb""></div>";

            litText1 = @"<div>";
            litText1+=  @"<img alt=""Cart"" hspace=""8"" src=""images/cart.gif"" />";
            litText1 += @"<span>" + carttext + "</span>";
            //litText1 += @"<a href=""cart.aspx"" class=""btn viewCart"">View Cart</a>";
            litText1 += @"<a href=""cart.aspx"">";
            litText1 += @"<img alt=""View Cart"" hspace=""5"" src=""images/viewcart.gif"" /></a>";
            litText1 += @"</div>";

            litText1 += separator;

            //NCIPL_CC litText2 = @"<span>Help with ordering: 1-800-4-CANCER (1-800-422-6237)</span>";
            //NCIPL_CC litText2 += separator;

            //NCIPL_CC litText3 = @"<a href=""nciplselfprint.aspx"">Self-Printing Options</a>";
            //litText3 += separator;

            string orderhistorypagename = "";
            if (GlobalUtils.UserRoles.getLoggedInUserId().Length == 0 || GlobalUtils.UserRoles.getLoggedInUserRole() < 2 || GlobalUtils.UserRoles.getLoggedInUserRole() > 3)
            {
                //Do nothing
            }
            else
            {
                orderhistorypagename = "searchorder.aspx";
                litText4 = @"<div style=""width:100%;text-align:right""><a href=" + "\"" + orderhistorypagename + "\"" + ">Look up an order</a></div>";
                litText4 += separator;
            }

            tabmarkup = litText1 + litText2 + litText3 + litText4;
            
            return tabmarkup;
        }

        /*Begin CR-31 - HITT 9815 */

        //Method sets the Search Session variables according to the URL Parameters
        public void SetSearchCriteriaFromURL2(string Params)
        {
         
            Params = "&" + Params; //Parameters received from the URL
            
            //Initialize a string array with all possible parameter keys
            string[] arrStrParams = {   "&skw=",
                                        "&toc=",
                                        "&sub=",
                                        "&aud=",
                                        "&lan=",
                                        "&pft=",
                                        "&ind=",
                                        "&ser=",
                                        "&rac=",
                                        "&pag="
                                    };

            ///Note: "pag" is not used as a criteria, but is used as the last array item

            //Use the parameter string received and convert it to a char array 
            //to extract each parameter parameter key and it's value. 
            //Then save the key and value as one string array item 
            char[] arrCharParams = Params.ToCharArray();
            string tempParams = "";
            int arrLength = arrCharParams.Length;
            for (int i = 0; i < arrLength; i++)
            {
                tempParams += arrCharParams[i];
                if (    tempParams == "&skw=" ||
                        tempParams == "&toc=" ||
                        tempParams == "&sub=" ||
                        tempParams == "&aud=" ||
                        tempParams == "&lan=" ||
                        tempParams == "&pft=" ||
                        tempParams == "&ind=" ||
                        tempParams == "&ser=" ||
                        tempParams == "&rac=" ||
                        tempParams == "&pag="   )
                {   
                    string tempVal = "";
                    for (int k = i+1; k < arrLength; k++)
                    {
                        tempVal += arrCharParams[k];

                        if (k == arrLength - 1) //if this is the last parameter
                        {
                            string Val = tempVal;
                            string currVal = tempParams + Val;

                            for (int c = 0; c < arrStrParams.Length; c++)
                            {
                                if (string.Compare(arrStrParams[c], tempParams, true) == 0)
                                {
                                    arrStrParams[c] = currVal;
                                    break;
                                }
                            }
                            tempParams = "";
                            break;
                        }
                        
                        if (        tempVal.Contains("&skw=") ||
                                    tempVal.Contains("&toc=") ||
                                    tempVal.Contains("&sub=") ||
                                    tempVal.Contains("&aud=") ||
                                    tempVal.Contains("&lan=") ||
                                    tempVal.Contains("&pft=") ||
                                    tempVal.Contains("&ind=") ||
                                    tempVal.Contains("&ser=") ||
                                    tempVal.Contains("&rac=") ||
                                    tempVal.Contains("&pag=")     )
                        {
                            string Val = tempVal.Remove(tempVal.Length - 5, 5);
                            string currVal = tempParams + Val;

                            for (int c = 0; c < arrStrParams.Length; c++)
                            {
                                if (string.Compare(arrStrParams[c], tempParams, true) == 0)
                                {
                                    arrStrParams[c] = currVal;
                                    break;
                                }
                            }
                            
                            tempParams = "";
                            i = k - 5;
                            break;
                        }
                    }
                }

            } 
            //At this point query string parameter values have been saved into the string array

            ArrayList arrlstParams = new ArrayList(); //Intialize the ArrayList
            
            //Default parameter values
            //"skw", "toc", "sub", "aud", "lan", "pft", "ind", "ser", "rac", "pag"
            
            //Add values to the ArrayList. If any parameter does not have a value, use the default value.
            for (int i = 0; i < arrStrParams.Length; i++)
            {   
                if (arrStrParams[i].Length == 5) //Parameters without any value
                    arrlstParams.Add(arrStrParams[i] + arrStrParams[i].Substring(1, 3));
                else
                    arrlstParams.Add(arrStrParams[i]);
            }
            
            //Initialize search session variables
            HttpContext.Current.Session["PUBENT_SearchKeyword"] = "";
            HttpContext.Current.Session["PUBENT_TypeOfCancer"] = "";
            HttpContext.Current.Session["PUBENT_Subject"] = "";
            HttpContext.Current.Session["PUBENT_Audience"] = "";
            HttpContext.Current.Session["PUBENT_Language"] = "";
            HttpContext.Current.Session["PUBENT_ProductFormat"] = "";
            HttpContext.Current.Session["PUBENT_StartsWith"] = "";
            HttpContext.Current.Session["PUBENT_Series"] = "";
            HttpContext.Current.Session["PUBENT_Race"] = "";
            
            //Assign values to the search session variables
            string listItemName = "";
            string lstItemValue = "";
            string strItem = "";
            for (int i=0; i<arrlstParams.Count; i++)
            {  
                strItem = arrlstParams[i].ToString();
                listItemName = strItem.Substring(0,5);
                lstItemValue = strItem.Substring(5,strItem.Length-5).Trim();
                
                switch (listItemName)
                {
                    case "&skw=":
                        if (string.Compare(lstItemValue, "skw", true) != 0)
                            HttpContext.Current.Session["PUBENT_SearchKeyword"] = lstItemValue;
                        break;
                    case "&toc=":
                        if (string.Compare(lstItemValue, "toc", true) != 0)
                            HttpContext.Current.Session["PUBENT_TypeOfCancer"] = lstItemValue;
                        break;
                    case "&sub=":
                        if (string.Compare(lstItemValue, "sub", true) != 0)
                            HttpContext.Current.Session["PUBENT_Subject"] = lstItemValue;
                        break;
                    case "&aud=":
                        if (string.Compare(lstItemValue, "aud", true) != 0)
                            HttpContext.Current.Session["PUBENT_Audience"] = lstItemValue;
                        break;
                    case "&lan=":
                        if (string.Compare(lstItemValue, "lan", true) != 0)
                            HttpContext.Current.Session["PUBENT_Language"] = lstItemValue;
                        break;
                    case "&pft=":
                        if (string.Compare(lstItemValue, "pft", true) != 0)
                            HttpContext.Current.Session["PUBENT_ProductFormat"] = lstItemValue;
                        break;
                    case "&ind=":
                        if (string.Compare(lstItemValue, "ind", true) != 0)
                            HttpContext.Current.Session["PUBENT_StartsWith"] = lstItemValue;
                        break;
                    case "&ser=":
                        if (string.Compare(lstItemValue, "ser", true) != 0)
                            HttpContext.Current.Session["PUBENT_Series"] = lstItemValue;
                        break;
                    case "&rac=":
                        if (string.Compare(lstItemValue, "rac", true) != 0)
                            HttpContext.Current.Session["PUBENT_Race"] = lstItemValue;
                        break;
                    case "&pag=":
                        //if (string.Compare(lstItemValue, "", true) != 0)
                        //Not used
                        break;
                }

            }
            arrlstParams = null; //Free it up

            //Set Search Results Criteria Text
            #region DisplaySearchCriteria
            string SearchCriteria = "";
            string[] arrTemp; //A temporary string array used for criteria text
            HttpContext.Current.Session["PUBENT_Criteria"] = "";
            InitializeCriteriaTextSessionVariables();
            if (HttpContext.Current.Session["PUBENT_SearchKeyword"].ToString().Length > 0)
            {
                if (SearchCriteria.Length == 0)
                    SearchCriteria = HttpContext.Current.Session["PUBENT_SearchKeyword"].ToString();
                else
                    SearchCriteria = SearchCriteria + ", " + HttpContext.Current.Session["PUBENT_SearchKeyword"].ToString();
            }
            if (HttpContext.Current.Session["PUBENT_TypeOfCancer"].ToString().Length > 0)
            {
                arrTemp = HttpContext.Current.Session["PUBENT_TypeOfCancer"].ToString().Split(',');
                //NCIPLCC KVPairCollection collCancerTypes = KVPair.GetKVPair("sp_NCIPL_getCancerTypes");
                KVPairCollection collCancerTypes = KVPair.GetKVPair("sp_NCIPLCC_getCancerTypes");
                foreach (KVPair kvItem in collCancerTypes)
                {
                    foreach (string strVal in arrTemp)
                    {
                        if (string.Compare(kvItem.Key, strVal, true) == 0)
                            if (SearchCriteria.Length == 0)
                            {
                                SearchCriteria = kvItem.Val;
                                SetCriteriaText("TypeOfCancer", kvItem.Val); //HITT 7074 CR-31
                            }
                            else
                            {
                                SearchCriteria = SearchCriteria + ", " + kvItem.Val;
                                SetCriteriaText("TypeOfCancer", kvItem.Val); //HITT 7074 CR-31
                            }
                    }
                }
            }
            if (HttpContext.Current.Session["PUBENT_Subject"].ToString().Length > 0)
            {
                arrTemp = HttpContext.Current.Session["PUBENT_Subject"].ToString().Split(',');
                //NCIPLCC KVPairCollection collSubjects = KVPair.GetKVPair("sp_NCIPL_getSubjects");
                KVPairCollection collSubjects = KVPair.GetKVPair("sp_NCIPLCC_getSubjects");
                foreach (KVPair kvItem in collSubjects)
                {
                    foreach (string strVal in arrTemp)
                    {
                        if (string.Compare(kvItem.Key, strVal, true) == 0)
                            if (SearchCriteria.Length == 0)
                            {
                                SearchCriteria = kvItem.Val;
                                SetCriteriaText("Subject", kvItem.Val); //HITT 7074 CR-31
                            }
                            else
                            {
                                SearchCriteria = SearchCriteria + ", " + kvItem.Val;
                                SetCriteriaText("Subject", kvItem.Val); //HITT 7074 CR-31
                            }
                    }
                }
            }
            if (HttpContext.Current.Session["PUBENT_Audience"].ToString().Length > 0)
            {
                arrTemp = HttpContext.Current.Session["PUBENT_Audience"].ToString().Split(',');
                //NCIPLCC KVPairCollection collAudience = KVPair.GetKVPair("sp_NCIPL_getAudience");
                KVPairCollection collAudience = KVPair.GetKVPair("sp_NCIPLCC_getAudience");
                foreach (KVPair kvItem in collAudience)
                {
                    foreach (string strVal in arrTemp)
                    {
                        if (string.Compare(kvItem.Key, strVal, true) == 0)
                            if (SearchCriteria.Length == 0)
                            {
                                SearchCriteria = kvItem.Val;
                                SetCriteriaText("Audience", kvItem.Val); //HITT 7074 CR-31
                            }
                            else
                            {
                                SearchCriteria = SearchCriteria + ", " + kvItem.Val;
                                SetCriteriaText("Audience", kvItem.Val); //HITT 7074 CR-31
                            }
                    }
                }
            }
            if (HttpContext.Current.Session["PUBENT_ProductFormat"].ToString().Length > 0)
            {
                arrTemp = HttpContext.Current.Session["PUBENT_ProductFormat"].ToString().Split(',');
                //NCIPLCC KVPairCollection collProdFormats = KVPair.GetKVPair("sp_NCIPL_getProductFormats");
                KVPairCollection collProdFormats = KVPair.GetKVPair("sp_NCIPLCC_getProductFormats");
                foreach (KVPair kvItem in collProdFormats)
                {
                    foreach (string strVal in arrTemp)
                    {
                        if (string.Compare(kvItem.Key, strVal, true) == 0)
                            if (SearchCriteria.Length == 0)
                            {
                                SearchCriteria = kvItem.Val;
                                SetCriteriaText("ProductFormat", kvItem.Val); //HITT 7074 CR-31
                            }
                            else
                            {
                                SearchCriteria = SearchCriteria + ", " + kvItem.Val;
                                SetCriteriaText("ProductFormat", kvItem.Val); //HITT 7074 CR-31
                            }
                    }
                }
            }
            if (HttpContext.Current.Session["PUBENT_Language"].ToString().Length > 0)
            {
                arrTemp = HttpContext.Current.Session["PUBENT_Language"].ToString().Split(',');
                //NCIPLCC KVPairCollection collLanguages = KVPair.GetKVPair("sp_NCIPL_getLanguages");
                KVPairCollection collLanguages = KVPair.GetKVPair("sp_NCIPLCC_getLanguages");
                foreach (KVPair kvItem in collLanguages)
                {
                    foreach (string strVal in arrTemp)
                    {
                        if (string.Compare(kvItem.Key, strVal, true) == 0)
                            if (SearchCriteria.Length == 0)
                            {
                                SearchCriteria = kvItem.Val;
                                SetCriteriaText("Language", kvItem.Val); //HITT 7074 CR-31
                            }
                            else
                            {
                                SearchCriteria = SearchCriteria + ", " + kvItem.Val;
                                SetCriteriaText("Language", kvItem.Val); //HITT 7074 CR-31
                            }
                    }
                }
            }
            if (HttpContext.Current.Session["PUBENT_Series"].ToString().Length > 0)
            {
                arrTemp = HttpContext.Current.Session["PUBENT_Series"].ToString().Split(',');
                //NCIPLCC KVPairCollection collSeries = KVPair.GetKVPair("sp_NCIPL_getCollections");
                KVPairCollection collSeries = KVPair.GetKVPair("sp_NCIPLCC_getCollections");
                foreach (KVPair kvItem in collSeries)
                {
                    foreach (string strVal in arrTemp)
                    {
                        if (string.Compare(kvItem.Key, strVal, true) == 0)
                            if (SearchCriteria.Length == 0)
                            {
                                SearchCriteria = kvItem.Val;
                                SetCriteriaText("Series", kvItem.Val); //HITT 7074 CR-31
                            }
                            else
                            {
                                SearchCriteria = SearchCriteria + ", " + kvItem.Val;
                                SetCriteriaText("Series", kvItem.Val); //HITT 7074 CR-31
                            }
                    }
                }
            }
            if (HttpContext.Current.Session["PUBENT_Race"].ToString().Length > 0)
            {
                arrTemp = HttpContext.Current.Session["PUBENT_Race"].ToString().Split(',');
                //NCIPLCC KVPairCollection collRace = KVPair.GetKVPair("sp_NCIPL_getRace");
                KVPairCollection collRace = KVPair.GetKVPair("sp_NCIPLCC_getRace");
                foreach (KVPair kvItem in collRace)
                {
                    foreach (string strVal in arrTemp)
                    {
                        if (string.Compare(kvItem.Key, strVal, true) == 0)
                            if (SearchCriteria.Length == 0)
                            {
                                SearchCriteria = kvItem.Val;
                                SetCriteriaText("Race", kvItem.Val); //HITT 7074 CR-31
                            }
                            else
                            {
                                SearchCriteria = SearchCriteria + ", " + kvItem.Val;
                                SetCriteriaText("Race", kvItem.Val); //HITT 7074 CR-31
                            }
                    }
                }
            }
            if (HttpContext.Current.Session["PUBENT_NewOrUpdated"].ToString().Length > 0)
            {   
                //NCIPLCC KVPairCollection collProdUpdates = KVPair.GetKVPair("sp_NCIPL_getProductUpdates");
                KVPairCollection collProdUpdates = KVPair.GetKVPair("sp_NCIPLCC_getProductUpdates");
                foreach (KVPair kvItem in collProdUpdates)
                {
                    if (string.Compare(kvItem.Key, HttpContext.Current.Session["PUBENT_NewOrUpdated"].ToString(), true) == 0)
                        if (SearchCriteria.Length == 0)
                        {
                            SearchCriteria = kvItem.Val;
                            SetCriteriaText("NewOrUpdated", kvItem.Val); //HITT 7074 CR-31
                        }
                        else
                        {
                            SearchCriteria = SearchCriteria + ", " + kvItem.Val;
                            SetCriteriaText("NewOrUpdated", kvItem.Val); //HITT 7074 CR-31
                        }
                }
            }
            if (HttpContext.Current.Session["PUBENT_StartsWith"].ToString().Length > 0)
            {
                if (SearchCriteria.Length == 0)
                {
                    SearchCriteria = HttpContext.Current.Session["PUBENT_StartsWith"].ToString();
                    SetCriteriaText("StartsWith", HttpContext.Current.Session["PUBENT_StartsWith"].ToString()); //HITT 7074 CR-31
                }
                else
                {
                    SearchCriteria = SearchCriteria + ", " + HttpContext.Current.Session["PUBENT_StartsWith"].ToString();
                    SetCriteriaText("StartsWith", HttpContext.Current.Session["PUBENT_StartsWith"].ToString()); //HITT 7074 CR-31
                }
            }
            HttpContext.Current.Session["PUBENT_Criteria"] = SearchCriteria;
            #endregion
        }

        //Method returns the the Search Results URL Parameters
        private string GetSearchURLParams(string pagenum)
        {
            string Params = "";
            StringBuilder sbParams = new StringBuilder();
            
            string paramSearchKeyword = "skw";
            string valSearchKeyword = "skw";
            string paramTypeOfCancer = "toc";
            string valTypeOfCancer = "toc";
            string paramSubject = "sub";
            string valSubject = "sub";
            string paramAudience = "aud";
            string valAudience = "aud";
            string paramLanguage ="lan";
            string valLanguage = "lan";
            string paramProductFormat = "pft";
            string valProductFormat = "pft";
            string paramStartsWith = "ind";
            string valStartsWith = "ind";
            string paramSeries = "ser";
            string valSeries = "ser";
            string paramRace = "rac";
            string valRace = "rac";
            string paramPage = "pag";
            string valPage = "pag";

            ///Note: "pag" is not used as a criteria, but is used as the last array item

            if (HttpContext.Current.Session["PUBENT_SearchKeyword"] != null)
            {
                if (HttpContext.Current.Session["PUBENT_SearchKeyword"].ToString().Length > 0)
                    valSearchKeyword = HttpContext.Current.Session["PUBENT_SearchKeyword"].ToString();
            }
            if (HttpContext.Current.Session["PUBENT_TypeOfCancer"] != null)
            {
                if (HttpContext.Current.Session["PUBENT_TypeOfCancer"].ToString().Length > 0)
                valTypeOfCancer = HttpContext.Current.Session["PUBENT_TypeOfCancer"].ToString();
            }
            if (HttpContext.Current.Session["PUBENT_Subject"] != null)
            {
                if (HttpContext.Current.Session["PUBENT_Subject"].ToString().Length > 0)
                    valSubject = HttpContext.Current.Session["PUBENT_Subject"].ToString();
            }
            if (HttpContext.Current.Session["PUBENT_Audience"] != null)
            {
                if (HttpContext.Current.Session["PUBENT_Audience"].ToString().Length > 0)
                    valAudience = HttpContext.Current.Session["PUBENT_Audience"].ToString();
            }
            if (HttpContext.Current.Session["PUBENT_Language"] != null)
            {
                if (HttpContext.Current.Session["PUBENT_Language"].ToString().Length > 0)
                    valLanguage = HttpContext.Current.Session["PUBENT_Language"].ToString();
            }
            if (HttpContext.Current.Session["PUBENT_ProductFormat"] != null)
            {
                if (HttpContext.Current.Session["PUBENT_ProductFormat"].ToString().Length > 0)
                    valProductFormat = HttpContext.Current.Session["PUBENT_ProductFormat"].ToString();
            }
            if (HttpContext.Current.Session["PUBENT_StartsWith"] != null)
            {
                if (HttpContext.Current.Session["PUBENT_StartsWith"].ToString().Length > 0)
                    valStartsWith = HttpContext.Current.Session["PUBENT_StartsWith"].ToString();
            }
            if (HttpContext.Current.Session["PUBENT_Series"] != null)
            {
                if (HttpContext.Current.Session["PUBENT_Series"].ToString().Length > 0)
                    valSeries = HttpContext.Current.Session["PUBENT_Series"].ToString();
            }
            if (HttpContext.Current.Session["PUBENT_Race"] != null)
            {
                if (HttpContext.Current.Session["PUBENT_Race"].ToString().Length > 0)
                    valRace = HttpContext.Current.Session["PUBENT_Race"].ToString();
            }

            //FinalParams =
            //        paramSearchKeyword + "=" + valSearchKeyword + "&" +
            //        paramTypeOfCancer + "=" + valTypeOfCancer + "&" +
            //        paramSubject + "=" + valSubject + "&" +
            //        paramAudience + "=" + valAudience + "&" +
            //        paramLanguage + "=" + valLanguage + "&" +
            //        paramProductFormat + "=" + valProductFormat + "&" +
            //        paramStartsWith + "=" + valStartsWith + "&" +
            //        paramSeries + "=" + valSeries + "&" +
            //        paramRace + "=" + valRace;

            if (valSearchKeyword != "skw")
                sbParams.Append(paramSearchKeyword + "=" + valSearchKeyword + "&");
            if (valTypeOfCancer != "toc")
                sbParams.Append(paramTypeOfCancer + "=" + valTypeOfCancer + "&");
            if (valSubject != "sub")
                sbParams.Append(paramSubject + "=" + valSubject + "&");
            if (valAudience != "aud")
                sbParams.Append(paramAudience + "=" + valAudience + "&");
            if (valLanguage  != "lan")
                sbParams.Append(paramLanguage + "=" + valLanguage + "&");
            if (valProductFormat != "pft")
                sbParams.Append(paramProductFormat + "=" + valProductFormat + "&");
            if (valStartsWith != "ind")
                sbParams.Append(paramStartsWith + "=" + valStartsWith + "&");
            if (valSeries != "ser")
                sbParams.Append(paramSeries + "=" + valSeries + "&");
            if (valRace != "rac")
                sbParams.Append(paramRace + "=" + valRace + "&");
            if (pagenum != "")
                sbParams.Append(paramPage + "=" + pagenum + "&");

            Params = sbParams.ToString();
            if (Params.Length > 1)
                Params = Params.Remove(Params.Length - 1, 1);
            return Params;

        }

        //Encrypt or Decrypt
        public bool IsEncryptOn()
        {
            if (string.Compare(ConfigurationManager.AppSettings["PubEntEncryptFlag"], "1", true) == 0)
                return true;
            else
                return false;
        }

        //Method returns encrypted querystring parameters if encryption flag is on
        //Otherwise returns regular querystring parameters
        public string GetQueryStringParams()
        {
            string Params = "";
            string EncryptedParams = "";
            Params = this.GetSearchURLParams("");

            //Encrypt the params
            if (this.IsEncryptOn())
            {
                EncryptedParams = Cryptography.Encrypt(Params);
                EncryptedParams = HttpContext.Current.Server.UrlEncode(EncryptedParams);
            }
            else
                EncryptedParams = HttpContext.Current.Server.UrlEncode(Params);
            
            return EncryptedParams;
        }

        /*End CR-31 - HITT 9815 */

        //Begin CR-31 HITT 7074
        //For saving to tbl_searchkeywords table for admin report
        /* 1-Type of Cancer
             * 2-Subject
             * 3-Audience
             * 4-Language
             * 5-ProductFormat
             * 6-LetterIndex
             * 7-Series or Collecitons
             * 8-New or Updated
             * 9-Race */
        public static void SetCriteriaText(string Type, string Text)
        {
            switch (Type)
            {
                case "TypeOfCancer":
                    if (HttpContext.Current.Session["PUBENT_TypeOfCancerCriteria"] != null)
                    {
                        if (HttpContext.Current.Session["PUBENT_TypeOfCancerCriteria"].ToString().Length > 0)
                            HttpContext.Current.Session["PUBENT_TypeOfCancerCriteria"] += ", " + Text;
                        else
                            HttpContext.Current.Session["PUBENT_TypeOfCancerCriteria"] = Text;
                    }
                    break;
                case "Subject":
                    if (HttpContext.Current.Session["PUBENT_SubjectCriteria"] != null)
                    {
                        if (HttpContext.Current.Session["PUBENT_SubjectCriteria"].ToString().Length > 0)
                            HttpContext.Current.Session["PUBENT_SubjectCriteria"] += ", " + Text;
                        else
                            HttpContext.Current.Session["PUBENT_SubjectCriteria"] = Text;
                    }
                    break;
                case "Audience":
                    if (HttpContext.Current.Session["PUBENT_AudienceCriteria"] != null)
                    {
                        if (HttpContext.Current.Session["PUBENT_AudienceCriteria"].ToString().Length > 0)
                            HttpContext.Current.Session["PUBENT_AudienceCriteria"] += ", " + Text;
                        else
                            HttpContext.Current.Session["PUBENT_AudienceCriteria"] = Text;
                    }
                    break;
                case "Language":
                    if (HttpContext.Current.Session["PUBENT_LanguageCriteria"] != null)
                    {
                        if (HttpContext.Current.Session["PUBENT_LanguageCriteria"].ToString().Length > 0)
                            HttpContext.Current.Session["PUBENT_LanguageCriteria"] += ", " + Text;
                        else
                            HttpContext.Current.Session["PUBENT_LanguageCriteria"] = Text;
                    }
                    break;
                case "ProductFormat":
                    if (HttpContext.Current.Session["PUBENT_ProductFormatCriteria"] != null)
                    {
                        if (HttpContext.Current.Session["PUBENT_ProductFormatCriteria"].ToString().Length > 0)
                            HttpContext.Current.Session["PUBENT_ProductFormatCriteria"] += ", " + Text;
                        else
                            HttpContext.Current.Session["PUBENT_ProductFormatCriteria"] = Text;
                    }
                    break;
                case "StartsWith":
                    if (HttpContext.Current.Session["PUBENT_StartsWithCriteria"] != null)
                    {
                        if (HttpContext.Current.Session["PUBENT_StartsWithCriteria"].ToString().Length > 0)
                            HttpContext.Current.Session["PUBENT_StartsWithCriteria"] += ", " + Text;
                        else
                            HttpContext.Current.Session["PUBENT_StartsWithCriteria"] = Text;
                    }
                    break;
                case "Series":
                    if (HttpContext.Current.Session["PUBENT_SeriesCriteria"] != null)
                    {
                        if (HttpContext.Current.Session["PUBENT_SeriesCriteria"].ToString().Length > 0)
                            HttpContext.Current.Session["PUBENT_SeriesCriteria"] += ", " + Text;
                        else
                            HttpContext.Current.Session["PUBENT_SeriesCriteria"] = Text;
                    }
                    break;
                case "NewOrUpdated":
                    if (HttpContext.Current.Session["PUBENT_NewOrUpdatedCriteria"] != null)
                    {
                        if (HttpContext.Current.Session["PUBENT_NewOrUpdatedCriteria"].ToString().Length > 0)
                            HttpContext.Current.Session["PUBENT_NewOrUpdatedCriteria"] += ", " + Text;
                        else
                            HttpContext.Current.Session["PUBENT_NewOrUpdatedCriteria"] = Text;
                    }
                    break;
                case "Race":
                    if (HttpContext.Current.Session["PUBENT_RaceCriteria"] != null)
                    {
                        if (HttpContext.Current.Session["PUBENT_RaceCriteria"].ToString().Length > 0)
                            HttpContext.Current.Session["PUBENT_RaceCriteria"] += ", " + Text;
                        else
                            HttpContext.Current.Session["PUBENT_RaceCriteria"] = Text;
                    }
                    break;
            }
        }
        
        public static void InitializeCriteriaTextSessionVariables()
        {
            HttpContext.Current.Session["PUBENT_TypeOfCancerCriteria"] = "";
            HttpContext.Current.Session["PUBENT_SubjectCriteria"] = "";
            HttpContext.Current.Session["PUBENT_AudienceCriteria"] = "";
            HttpContext.Current.Session["PUBENT_LanguageCriteria"] = "";
            HttpContext.Current.Session["PUBENT_ProductFormatCriteria"] = "";
            HttpContext.Current.Session["PUBENT_StartsWithCriteria"] = "";
            HttpContext.Current.Session["PUBENT_SeriesCriteria"] = "";
            HttpContext.Current.Session["PUBENT_NewOrUpdatedCriteria"] = "";
            HttpContext.Current.Session["PUBENT_RaceCriteria"] = "";
        }

        //End HITT 7074


        //Method added for CR-31 HITT 7074 - Save all search criteria values to table
        public static void SaveSearch(int Hits)
        {   
            bool boolSaveSearch = false;
            if (HttpContext.Current.Session["PUBENT_SearchKeyword_OLD"] == null ||
                HttpContext.Current.Session["PUBENT_TypeOfCancerCriteria_OLD"] == null ||
                HttpContext.Current.Session["PUBENT_SubjectCriteria_OLD"] == null ||
                HttpContext.Current.Session["PUBENT_AudienceCriteria_OLD"] == null ||
                HttpContext.Current.Session["PUBENT_LanguageCriteria_OLD"] == null ||
                HttpContext.Current.Session["PUBENT_ProductFormatCriteria_OLD"] == null ||
                HttpContext.Current.Session["PUBENT_StartsWithCriteria_OLD"] == null ||
                HttpContext.Current.Session["PUBENT_SeriesCriteria_OLD"] == null ||
                HttpContext.Current.Session["PUBENT_NewOrUpdatedCriteria_OLD"] == null ||
                HttpContext.Current.Session["PUBENT_RaceCriteria_OLD"] == null)
                boolSaveSearch = true;

            if (boolSaveSearch == false)
            {
                if (string.Compare(HttpContext.Current.Session["PUBENT_SearchKeyword"].ToString(), HttpContext.Current.Session["PUBENT_SearchKeyword_OLD"].ToString(), true) == 0 &&
                    string.Compare(HttpContext.Current.Session["PUBENT_TypeOfCancerCriteria"].ToString(), HttpContext.Current.Session["PUBENT_TypeOfCancerCriteria_OLD"].ToString(), true) == 0 &&
                    string.Compare(HttpContext.Current.Session["PUBENT_SubjectCriteria"].ToString(), HttpContext.Current.Session["PUBENT_SubjectCriteria_OLD"].ToString(), true) == 0 &&
                    string.Compare(HttpContext.Current.Session["PUBENT_AudienceCriteria"].ToString(), HttpContext.Current.Session["PUBENT_AudienceCriteria_OLD"].ToString(), true) == 0 &&
                    string.Compare(HttpContext.Current.Session["PUBENT_LanguageCriteria"].ToString(), HttpContext.Current.Session["PUBENT_LanguageCriteria_OLD"].ToString(), true) == 0 &&
                    string.Compare(HttpContext.Current.Session["PUBENT_ProductFormatCriteria"].ToString(), HttpContext.Current.Session["PUBENT_ProductFormatCriteria_OLD"].ToString(), true) == 0 &&
                    string.Compare(HttpContext.Current.Session["PUBENT_StartsWithCriteria"].ToString(), HttpContext.Current.Session["PUBENT_StartsWithCriteria_OLD"].ToString(), true) == 0 &&
                    string.Compare(HttpContext.Current.Session["PUBENT_SeriesCriteria"].ToString(), HttpContext.Current.Session["PUBENT_SeriesCriteria_OLD"].ToString(), true) == 0 &&
                    string.Compare(HttpContext.Current.Session["PUBENT_NewOrUpdatedCriteria"].ToString(), HttpContext.Current.Session["PUBENT_NewOrUpdatedCriteria_OLD"].ToString(), true) == 0 &&
                    string.Compare(HttpContext.Current.Session["PUBENT_RaceCriteria"].ToString(), HttpContext.Current.Session["PUBENT_RaceCriteria_OLD"].ToString(), true) == 0)
                {
                    boolSaveSearch = false;
                    HttpContext.Current.Session["PUBENT_AdvSearch"] = ""; //Reset
                }
                else
                    boolSaveSearch = true;
            }

            if (boolSaveSearch)
            {
                /* This condition check needs to be removed, whenever it */
                /* is decided to save the advanced search criteria text  */
                bool IsAdvSearch = false;
                if (HttpContext.Current.Session["PUBENT_AdvSearch"] != null)
                {
                    if (HttpContext.Current.Session["PUBENT_AdvSearch"].ToString() == "True")
                        IsAdvSearch = true;
                    else
                        IsAdvSearch = false;
                    HttpContext.Current.Session["PUBENT_AdvSearch"] = ""; //Reset
                }

                if (!IsAdvSearch)
                {
                    PubEnt.DAL2.DAL.SaveSearchCriteriaText(HttpContext.Current.Session["PUBENT_SearchKeyword"].ToString(), Hits,
                                                            HttpContext.Current.Session["PUBENT_TypeOfCancerCriteria"].ToString(), HttpContext.Current.Session["PUBENT_SubjectCriteria"].ToString(),
                                                            HttpContext.Current.Session["PUBENT_AudienceCriteria"].ToString(), HttpContext.Current.Session["PUBENT_LanguageCriteria"].ToString(),
                                                            HttpContext.Current.Session["PUBENT_ProductFormatCriteria"].ToString(), HttpContext.Current.Session["PUBENT_StartsWithCriteria"].ToString(),
                                                            HttpContext.Current.Session["PUBENT_SeriesCriteria"].ToString(), HttpContext.Current.Session["PUBENT_NewOrUpdatedCriteria"].ToString(),
                                                            HttpContext.Current.Session["PUBENT_RaceCriteria"].ToString(), "ROO"); //"NCIPL"

                    HttpContext.Current.Session["PUBENT_SearchKeyword_OLD"] = HttpContext.Current.Session["PUBENT_SearchKeyword"];
                    HttpContext.Current.Session["PUBENT_TypeOfCancerCriteria_OLD"] = HttpContext.Current.Session["PUBENT_TypeOfCancerCriteria"];
                    HttpContext.Current.Session["PUBENT_SubjectCriteria_OLD"] = HttpContext.Current.Session["PUBENT_SubjectCriteria"];
                    HttpContext.Current.Session["PUBENT_AudienceCriteria_OLD"] = HttpContext.Current.Session["PUBENT_AudienceCriteria"];
                    HttpContext.Current.Session["PUBENT_LanguageCriteria_OLD"] = HttpContext.Current.Session["PUBENT_LanguageCriteria"];
                    HttpContext.Current.Session["PUBENT_ProductFormatCriteria_OLD"] = HttpContext.Current.Session["PUBENT_ProductFormatCriteria"];
                    HttpContext.Current.Session["PUBENT_StartsWithCriteria_OLD"] = HttpContext.Current.Session["PUBENT_StartsWithCriteria"];
                    HttpContext.Current.Session["PUBENT_SeriesCriteria_OLD"] = HttpContext.Current.Session["PUBENT_SeriesCriteria"];
                    HttpContext.Current.Session["PUBENT_NewOrUpdatedCriteria_OLD"] = HttpContext.Current.Session["PUBENT_NewOrUpdatedCriteria"];
                    HttpContext.Current.Session["PUBENT_RaceCriteria_OLD"] = HttpContext.Current.Session["PUBENT_RaceCriteria"];
                
                }
                else //Added else condition as per Cindy's email dated 4/20/2010
                {
                    //JPJ 4/21/10 Save only the search term for an advanced search. 
                    //The value of Hits may not be accurate, since we are not saving the whole criteria. But it
                    //is fine, since the only use of the column is to keep compatibility with sp_ROO_SearchContains. (It's value is not important.)
                    PubEnt.DAL2.DAL.SaveSearchCriteriaText(HttpContext.Current.Session["PUBENT_SearchKeyword"].ToString(), Hits,
                                                            "", "",
                                                            "", "",
                                                            "", "",
                                                            "", "",
                                                            "", "ROO"); //"NCIPL"
                

                    HttpContext.Current.Session["PUBENT_SearchKeyword_OLD"] = HttpContext.Current.Session["PUBENT_SearchKeyword"];
                    HttpContext.Current.Session["PUBENT_TypeOfCancerCriteria_OLD"] = "";
                    HttpContext.Current.Session["PUBENT_SubjectCriteria_OLD"] = "";
                    HttpContext.Current.Session["PUBENT_AudienceCriteria_OLD"] = "";
                    HttpContext.Current.Session["PUBENT_LanguageCriteria_OLD"] = "";
                    HttpContext.Current.Session["PUBENT_ProductFormatCriteria_OLD"] = "";
                    HttpContext.Current.Session["PUBENT_StartsWithCriteria_OLD"] = "";
                    HttpContext.Current.Session["PUBENT_SeriesCriteria_OLD"] = "";
                    HttpContext.Current.Session["PUBENT_NewOrUpdatedCriteria_OLD"] = "";
                    HttpContext.Current.Session["PUBENT_RaceCriteria_OLD"] = "";

                }
            }

        }
       
        /// <summary>
        /// NCIPL_CC - Add check for total order quantity for NCIPL CC role
        /// Passing values in the currPub and currQty parameters will return the result based 
        /// on the those values and also the quantities already in the shopping cart
        /// Passing empty strings for currPub and currQty returns the result
        /// based on the qunatities of the publications already in the shopping cart
        /// </summary>
        /// <param name="currPub">String</param>
        /// <param name="currQty">String</param>
        /// <param name="strMsg">Out String</param>
        /// <returns>Boolean</returns>
        public bool IsTotalQtyValidForRole(string currPub, string currQty, out string strMsg)
        {  
            strMsg = "";
            bool IsValid = true;

            if (UserRoles.getLoggedInUserRole() == 1) //Check only for the CC user
            {

                bool onlyForPubsInCart = (currPub.Length == 0 && currQty.Length == 0) ? true : false;

                //Make up a temporary shopping cart
                ProductCollection TempCart = new ProductCollection();
                string[] pubs = HttpContext.Current.Session["NCIPL_Pubs"].ToString().Split(new Char[] { ',' });
                string[] qtys = HttpContext.Current.Session["NCIPL_Qtys"].ToString().Split(new Char[] { ',' });
                for (var i = 0; i < pubs.Length; i++)
                {
                    if (pubs[i].Trim() != "")
                    {
                        int pubid = Int32.Parse(pubs[i]);
                        Product p = Product.GetCartPubByPubID(pubid);
                        p.NumQtyOrdered = Int32.Parse(qtys[i]);
                        TempCart.Add(p);   //BLL will know what to do w/ dupes and 0 quantities
                    }
                }

                //Now add the current item also to the tempCart
                if (!onlyForPubsInCart)
                {
                    Product currP = Product.GetCartPubByPubID(Int32.Parse(currPub));
                    currP.NumQtyOrdered = Int32.Parse(currQty);
                    TempCart.Add(currP);
                }

                string TotalOrderLimit = ConfigurationManager.AppSettings["CCRoleOrderLimit"];

                if (onlyForPubsInCart && TempCart.NonFreeTotalQty >= Int32.Parse(TotalOrderLimit))
                {
                    IsValid = false;
                    //Do not care about strMsg here. This is for displaying the LimitReached image
                }
                //else if (!onlyForPubsInCart && TempCart.NonFreeTotalQty > Int32.Parse(TotalOrderLimit))
                else if (!onlyForPubsInCart && TempCart.TotalQty > Int32.Parse(TotalOrderLimit))
                {
                    IsValid = false;
                    //strMsg = "Total number of items cannot be more than " + TotalOrderLimit + " per order.";
                    strMsg = "Please enter a valid quantity. The limit is " + TotalOrderLimit + " total items per order.";
                }

                TempCart = null;
            }

            return IsValid;
        }

        public bool IsTotalQtyValidForRoleForCart(string pubids, string qtysforpubs, out string strMsg)
        {
            strMsg = "";
            bool IsValid = true;

            if (UserRoles.getLoggedInUserRole() == 1) //Check only for the CC user
            {

                //Make up a temporary shopping cart
                ProductCollection TempCart = new ProductCollection();
                string[] pubs = pubids.Split(new Char[] { ',' });
                string[] qtys = qtysforpubs.Split(new Char[] { ',' });
                for (var i = 0; i < pubs.Length; i++)
                {
                    if (pubs[i].Trim() != "")
                    {
                        int pubid = Int32.Parse(pubs[i]);
                        Product p = Product.GetCartPubByPubID(pubid);
                        p.NumQtyOrdered = Int32.Parse(qtys[i]);
                        TempCart.Add(p);   //BLL will know what to do w/ dupes and 0 quantities
                    }
                }

                string TotalOrderLimit = ConfigurationManager.AppSettings["CCRoleOrderLimit"];
                //if (TempCart.NonFreeTotalQty > Int32.Parse(TotalOrderLimit))
                if (TempCart.TotalQty > Int32.Parse(TotalOrderLimit))
                {
                    IsValid = false;
                    strMsg = "Please adjust one or more of the quantities. The limit is " + TotalOrderLimit + " total items per order.";
                }

                TempCart = null;
            }

            return IsValid;
        }

    }
}
