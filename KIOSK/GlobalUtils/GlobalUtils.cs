using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//Manually added
using System.Web.UI.WebControls;
using System.Drawing;
using System.Configuration;
using System.IO;

using Exhibit.BLL;

namespace PubEnt.GlobalUtils
{
    public class Utils
    {
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

        public bool IsQtyValueValid(string val, string limit)
        {
            bool boolValidVal = false;
            if (!string.IsNullOrEmpty(val))
            {
                try
                {
                    //Int32.Parse(QuantityOrdered.Text);
                    if (Int32.Parse(val) <= Int32.Parse(limit))
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
            string litText1 = "<li></li>";
            string litText2 = "<li></li>";
            string litText3 = "<li></li>";
            
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

            switch (pagename)
            {
                case "home":
                    litText1 = @"<li id=""selected""><a href=""home.aspx"">Home</a></li>";
                    litText2 = @"<li><a href=""nciplselfprint.aspx"">Self-Printing Options</a></li>";
                    litText3 = @"<li><span id=""shopcart""><a href=""cart.aspx"">" + "Shopping Cart (" + intTotalNum.ToString() + ") </a></span></li>";
                    break;

                case "selfprint":
                    litText1 = @"<li><a href=""home.aspx"">Home</a></li>";
                    litText2 = @"<li id=""selected""><a href=""nciplselfprint.aspx"">Self-Printing Options</a></li>";
                    litText3 = @"<li><span id=""shopcart""><a href=""cart.aspx"">" + "Shopping Cart (" + intTotalNum.ToString() + ") </a></span></li>";
                    break;

                case "cart":
                    litText1 = @"<li><a href=""home.aspx"">Home</a></li>";
                    litText2 = @"<li><a href=""nciplselfprint.aspx"">Self-Printing Options</a></li>";
                    litText3 = @"<li id=""selected""><span id=""shopcart""><a href=""cart.aspx"">" + "Shopping Cart (" + intTotalNum.ToString() + ") </a></span></li>";
                    break;

                default:
                    litText1 = @"<li><a href=""home.aspx"">Home</a></li>";
                    litText2 = @"<li><a href=""nciplselfprint.aspx"">Self-Printing Options</a></li>";
                    litText3 = @"<li><span id=""shopcart""><a href=""cart.aspx"">" + "Shopping Cart (" + intTotalNum.ToString() + ") </a></span></li>";
                    break;

            }

            //string tag = @"<div id=""navmenu"">";
            //tabmarkup = tag + "<ul>" + litText1 + litText2 + litText3 + "</ul>" + "</div>";
            tabmarkup = "<ul>" + litText1 + litText2 + litText3 + "</ul>";

            return tabmarkup;
        }

        //--- New section by Vu Tran ---
        public static int SearchDirectory(string FileName, string DirectoryName)
        {
            //Declare a List of Strings for Returning File Names            
            List<string> strFileNames = new List<string>();
            try
            {
                //Get the Directory Info using Directory Name                
                DirectoryInfo dirInfor = new DirectoryInfo(DirectoryName);

                // Get all files whose names starts with FileName Passed
                //FileInfo[] filesInfo = dirInfor.GetFiles(FileName + "*", SearchOption.TopDirectoryOnly);
                FileInfo[] filesInfo = dirInfor.GetFiles(FileName, SearchOption.TopDirectoryOnly);

                //Loop through all the files found and add to                 
                //List and return them                
                foreach (FileInfo fi in filesInfo)
                {
                    strFileNames.Add(fi.FullName);
                }
                return strFileNames.Count;
            }
            catch
            {
                return 0;
            }
        }

        public static String GetSearchCategory(int ConfID, String CategoryType)
        {
            String categoryjavascript = "";
            String category_selid = "";
            String category_optdesc = "";

            if (CategoryType == "CANCERTYPE")
            {
                category_selid = "optCancerType";
                category_optdesc = "Type of Cancer";
            }
            else if (CategoryType == "SUBJECT")
            {
                category_selid = "optSubject";
                category_optdesc = "Subject";
            }
            else if (CategoryType == "AUDIENCE")
            {
                category_selid = "optAudience";
                category_optdesc = "Audience";
            }
            else if (CategoryType == "PUBLICATIONFORMAT")
            {
                category_selid = "optPublicationFormat";
                category_optdesc = "Publication Format";
            }
            else if (CategoryType == "SERIES") //Collections
            {
                category_selid = "optSeries";
                category_optdesc = "Collections";
            }
            else if (CategoryType == "LANGUAGES")
            {
                category_selid = "optLanguages";
                category_optdesc = "Languages";
            }

            Confs CategoryList = new Confs();
            CategoryList = PubEnt.DAL.DAL.GetSearchCategory(ConfID, CategoryType);

            if (CategoryList.Count > 0)
            {
                categoryjavascript = "document.write('<tr>');";
                categoryjavascript = categoryjavascript + "document.write('<td class=\"floatmenudrop\" align=\"left\">');";
                categoryjavascript = categoryjavascript + "document.write('<select name=\"" + category_selid + "\" id=\"" + category_selid + "\" onchange=\"goSearch(this);\" style=\"width:400px\">');";
                categoryjavascript = categoryjavascript + "document.write('<option value=\"\">" + category_optdesc + "</option>');";
                foreach (Conf CategoryItem in CategoryList)
                {
                    categoryjavascript = categoryjavascript + "document.write('<option value=\"" + CategoryItem.ConfID.ToString() + "\">" + CategoryItem.ConfName + "</option>');";
                }
                categoryjavascript = categoryjavascript + "document.write('</select>');";
                categoryjavascript = categoryjavascript + "document.write('</td>');";
                categoryjavascript = categoryjavascript + "document.write('</tr>');";
            }

            return categoryjavascript;
        }

        //Called by Cart, Verify
        public static void ResetSessions()
        {
            HttpContext.Current.Session["KIOSK_Urls"] = "";
            HttpContext.Current.Session["KIOSK_Pubs"] = "";
            HttpContext.Current.Session["KIOSK_Qtys"] = "";
            HttpContext.Current.Session["KIOSK_TypeOfCancer"] = "";
            HttpContext.Current.Session["KIOSK_Subject"] = "";
            HttpContext.Current.Session["KIOSK_Audience"] = "";
            HttpContext.Current.Session["KIOSK_ProductFormat"] = "";
            HttpContext.Current.Session["KIOSK_Language"] = "";
            HttpContext.Current.Session["KIOSK_Series"] = ""; //Or collection
            HttpContext.Current.Session["KIOSK_Extratext"] = "";
            
            HttpContext.Current.Session["KIOSK_ShipLocation"] = "";

            
            //Shipping Information
            HttpContext.Current.Session["KIOSK_Name"] = "";
            HttpContext.Current.Session["KIOSK_Organization"] = "";
            HttpContext.Current.Session["KIOSK_Address1"] = "";
            HttpContext.Current.Session["KIOSK_Address2"] = "";

            HttpContext.Current.Session["KIOSK_ZIPCode"] = "";  //Also use for Postal if International Order
            HttpContext.Current.Session["KIOSK_ZIPPlus4"] = "";
            HttpContext.Current.Session["KIOSK_City"] = "";
            HttpContext.Current.Session["KIOSK_State"] = "";    //Also use of Province if International Order
            HttpContext.Current.Session["KIOSK_Country"] = "";  //Use only if International Order
            HttpContext.Current.Session["KIOSK_Email"] = "";
            HttpContext.Current.Session["KIOSK_Phone"] = "";
            

        }

        public static string StringPadLeft(string content, int totalLength, char charToPad)
        {
            return content.PadLeft(totalLength, charToPad);
        }
    }
}
