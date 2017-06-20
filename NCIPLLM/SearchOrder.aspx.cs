using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PubEnt.DAL;
using PubEnt.BLL;

namespace PubEnt
{
    public partial class SearchOrder : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Check GUAM UserId, Role for NCIPL_CC
            if (GlobalUtils.UserRoles.getLoggedInUserId().Length == 0 || GlobalUtils.UserRoles.getLoggedInUserRole() < 1)
            {
                string currASPXfilename = System.IO.Path.GetFileName(Request.Path).ToString();
                Session["NCIPL_REGISTERREFERRER"] = currASPXfilename;
                Response.Redirect("~/login.aspx?msg=invaliduser&redir=" + currASPXfilename);
            }

            //Hide label error message
            lblErrorMsg.Visible = false;

            //Reset Session SEARCHORDER_CURRPAGE
            Session["SEARCHORDER_CURRPAGE"] = null;     //destroy

            if (!Page.IsPostBack)
            {
                //Display the master page tabs 
                GlobalUtils.Utils UtilMethod = new GlobalUtils.Utils();
                if (Session["NCIPL_Pubs"] != null)
                    Master.LiteralText = UtilMethod.GetTabHtmlMarkUp(Session["NCIPL_Qtys"].ToString(), "");
                else
                    Master.LiteralText = UtilMethod.GetTabHtmlMarkUp("", "");
                UtilMethod = null;

                #region Bind the form here!
                //NCIPL_CC - Populate Customer Type Dropdown
                KVPairCollection kvpairColl = KVPair.GetKVPair("sp_NCIPLCC_getAllTypeOfCustomers"); //***EAC probably better to make one specific to NCIPLLM
                foreach (KVPair kvpair in kvpairColl)
                {
                    ListItem li = new ListItem();
                    li.Value = kvpair.Key;
                    li.Text = kvpair.Val;
                    drpCustomerType.Items.Add(li);
                }
                kvpairColl = null;

                #endregion

                //Reset Sessions for Search Order Look up
                Session["SEARCHORDER_CUSTOMERTYPE"] = null;         //destroy
                Session["SEARCHORDER_CUSTOMERTYPEDESC"] = null;     //destroy
                Session["SEARCHORDER_KEYWORD"] = null;              //destroy

                Session["SEARCHORDER_SDATE"] = null;                //destroy
                Session["SEARCHORDER_EDATE"] = null;                //destroy
                
                Session["VIEW_CUSTID"] = null;
                Session["VIEW_ORDERNUM"] = null;
                Session["VIEW_PREVPAGE"] = null;
            }

        }               
               
        protected void clickSearchOrder(object sender, EventArgs e)
        {
            string strCustomerType = this.drpCustomerType.SelectedValue.ToString();
            string strKeyWords = txtKeywords.Text.Trim();

            string strSDate = txtStartDate.Text.Trim();
            string strEDate = txtEndDate.Text.Trim();

            if (strCustomerType.Length > 0)
            {
                Session["SEARCHORDER_CUSTOMERTYPE"] = strCustomerType;
                Session["SEARCHORDER_CUSTOMERTYPEDESC"] = this.drpCustomerType.SelectedItem.ToString();

            }
            else
            {
                Session["SEARCHORDER_CUSTOMERTYPE"] = "";
                Session["SEARCHORDER_CUSTOMERTYPEDESC"] = "";
            }

            if (strKeyWords.Length > 0)
            {
                Session["SEARCHORDER_KEYWORD"] = strKeyWords;
            }
            else
            {
                Session["SEARCHORDER_KEYWORD"] = "";
            }

            if (strSDate.Length > 0)
            {
                Session["SEARCHORDER_SDATE"] = strSDate;
            }
            else
            {
                Session["SEARCHORDER_SDATE"] = "";
            }

            if (strEDate.Length > 0)
            {
                Session["SEARCHORDER_EDATE"] = strEDate;
            }
            else
            {
                Session["SEARCHORDER_EDATE"] = "";
            }

            if (strCustomerType.Length > 0 || strKeyWords.Length > 0)
            {
                lblErrorMsg.Visible = false;
                GoSearch_Order();
            }
            else
            {
                lblErrorMsg.Text = "Please enter a search phrase or select a customer type";
                lblErrorMsg.Visible = true;
            }
        }

        protected void GoSearch_Order()
        {
            string temp = "SearchOrder_Result.aspx";
            Response.Redirect(temp);
        }

        protected void txtKeywords_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
