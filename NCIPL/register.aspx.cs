using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

using PubEnt.DAL;
using PubEnt.BLL;
using Aspensys.GlobalUsers.WebServiceClient;
using Aspensys.GlobalUsers.WebServiceClient.UserService;

namespace PubEnt
{
    public partial class register : System.Web.UI.Page
    {
        public string GuamErrorMsg = "There is an issue with GUAM System.";
        public string PubEntErrorMsg = "There is an issue with updating database.";

        Person billto
        {
            get
            {
                if (Session["NCIPL_billto"] == null)
                    return null;
                else
                    return (Person)Session["NCIPL_billto"];
            }
            set { Session["NCIPL_billto"] = value; }
        }
        Person shipto
        {
            get
            {
                if (Session["NCIPL_shipto"] == null)
                    return null;
                else
                    return (Person)Session["NCIPL_shipto"];
            }
            set { Session["NCIPL_shipto"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string sPreviousPage = "home.aspx";

            if (Session["NCIPL_REGISTERREFERRER"] != null)
            {
                if (Session["NCIPL_REGISTERREFERRER"].ToString() != "")
                {
                    sPreviousPage = Session["NCIPL_REGISTERREFERRER"].ToString();
                }
            }
            btnCancel.Attributes.Add("onclick", "window.location='" + sPreviousPage + "'; return(false);");
            
            if (!IsPostBack)
            {
                divUserRegConfirmation.Visible = false;
                lblGuamMsg.Visible = false;

                //**** Retrieve Security Questions List
                new UserServiceClient().Using(client =>
                {
                    // daquinohd 2017/02/10 - hardcoding question values for testing purposes. 
                    /// TODO: populate values from database
                    /* 
                    var application_info = client.GetApplicationInformation();
                    ddlQuestions.DataSource = application_info.Questions;
                    ddlQuestions.DataValueField = "QuestionID";
                    ddlQuestions.DataTextField = "QuestionText";
                    ddlQuestions.DataBind();
                     */
                    ddlQuestions.Items.Insert(0, new ListItem("[Select from the list]", ""));
                    ddlQuestions.Items.Insert(1, new ListItem("What was your first car?", "car"));
                    ddlQuestions.Items.Insert(2, new ListItem("What is your favorite pizza topping?", "pizza"));
                    ddlQuestions.Items.Insert(3, new ListItem("What won't you do for love?", "meatloaf"));
                });
                BindTotals("");
            }
        }


        protected void chkSameaddress_CC(object sender, EventArgs e)
        {
            if (chkSameaddress.Checked)
            {
                pnlBIllingInfo.Enabled = false;
            }
            else
            {
                pnlBIllingInfo.Enabled = true;
            }

        }

        protected void Zip_Changed(object sender, EventArgs e)
        {
            if (txtzip5.Text.Length == 5)
            {
                //***EAC HITT 7460 
                drpcity.Items.Clear();
                txtstate.Text = "";
                try
                {
                    foreach (KVPair city in Zipcode.GetCities(Convert.ToInt32(txtzip5.Text)))
                    {
                        drpcity.Items.Add(new ListItem(city.Key, city.Key));
                        txtstate.Text = city.Val; //doesn't hurt to do this n times
                    }
                    txtcity.Visible = true;     //**just in case the line below throws an error
                    drpcity.Visible = false;    //**just in case the line below throws an error
                    txtcity.Text = drpcity.Items[0].Text;
                    if (drpcity.Items.Count <= 1)
                    {
                        txtcity.Visible = true;
                        drpcity.Visible = false;
                        lbltxtcity.AssociatedControlID = "txtcity";
                    }
                    else
                    {
                        txtcity.Visible = false;
                        drpcity.Visible = true;
                        lbltxtcity.AssociatedControlID = "drpcity";
                    }
                    BindTotals(txtstate.Text);
                }
                catch { }
            }
        }

        protected void BillZip_Changed(object sender, EventArgs e)
        {
            if (txt2zip5.Text.Length == 5)
            {
                //***EAC HITT 7460 
                drpcity2.Items.Clear();
                txt2state.Text = "";
                try
                {
                    foreach (KVPair city in Zipcode.GetCities(Convert.ToInt32(txt2zip5.Text)))
                    {
                        drpcity2.Items.Add(new ListItem(city.Key, city.Key));
                        txt2state.Text = city.Val; //doesn't hurt to do this n times
                    }
                    txt2city.Visible = true;     //**just in case the line below throws an error
                    drpcity2.Visible = false;    //**just in case the line below throws an error
                    txt2city.Text = drpcity2.Items[0].Text;
                    if (drpcity2.Items.Count <= 1)
                    {
                        txt2city.Visible = true;
                        drpcity2.Visible = false;
                        lbltxt2city.AssociatedControlID = "txt2city";
                    }
                    else
                    {
                        txt2city.Visible = false;
                        drpcity2.Visible = true;
                        lbltxt2city.AssociatedControlID = "drpcity2";
                    }
                }
                catch { }
            }

            //Zipcode z = Zipcode.GetCSZ(Int32.Parse(txt2zip5.Text));
            //txt2city.Text = z.City;
            //txt2state.Text = z.State;
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            RedirectPreviousPage();
        }

        protected void RedirectPreviousPage()
        {
            if (Session["NCIPL_REGISTERREFERRER"] != null)
            {
                if (Session["NCIPL_REGISTERREFERRER"].ToString().Contains("changepwd.aspx")
                    || Session["NCIPL_REGISTERREFERRER"].ToString().Contains("forgotpwd.aspx")
                    || Session["NCIPL_REGISTERREFERRER"].ToString().Contains("login.aspx") 
                    || Session["NCIPL_REGISTERREFERRER"].ToString().Contains("edit_registerinfo.aspx"))
                {
                    Response.Redirect("home.aspx");
                }
                else
                {
                    Response.Redirect(Session["NCIPL_REGISTERREFERRER"].ToString());
                }
            }
            else
            {
                Response.Redirect("home.aspx");
            }

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            #region Toggle Billing Validators
            if (chkSameaddress.Checked)
            {
                RequiredFieldValidator9.Enabled = false;
                RequiredFieldValidator10.Enabled = false;
                RequiredFieldValidator11.Enabled = false;
                RequiredFieldValidator12.Enabled = false;
                RequiredFieldValidator14.Enabled = false;
                RequiredFieldValidator16.Enabled = false;
                RequiredFieldValidator17.Enabled = false;

                RegularExpressionValidator2.Enabled = false;
                RegularExpressionValidator4.Enabled = false;
            }
            else
            {
                RequiredFieldValidator9.Enabled = true;
                RequiredFieldValidator10.Enabled = true;
                RequiredFieldValidator11.Enabled = true;
                RequiredFieldValidator12.Enabled = true;
                RequiredFieldValidator14.Enabled = true;
                RequiredFieldValidator16.Enabled = true;
                RequiredFieldValidator17.Enabled = true;

                RegularExpressionValidator2.Enabled = true;
                RegularExpressionValidator4.Enabled = true;
            }
            #endregion

            if (Page.IsValid)
            {
                //*** EAC We passed .Net validation so now just
                //*** validate the lengths so AppScan doesn't get angry
                if (txtzip5.Text.Length + txtzip4.Text.Length + txtcity.Text.Length + txtstate.Text.Length +
                    txtname.Text.Length + txtorg.Text.Length + txtaddr1.Text.Length + txtaddr2.Text.Length + txtphone.Text.Length + txtemail.Text.Length > 220)
                    throw (new ArgumentOutOfRangeException("Shipping object is too big"));

                if (txt2zip5.Text.Length + txt2zip4.Text.Length + txt2city.Text.Length + txt2state.Text.Length +
                    txt2name.Text.Length + txt2org.Text.Length + txt2addr1.Text.Length + txt2addr2.Text.Length + txt2phone.Text.Length + txt2email.Text.Length > 220)
                    throw (new ArgumentOutOfRangeException("Billing object is too big"));

                //***EAC At this point, data looks clean
                this.shipto = new Person(1, txtname.Text, txtorg.Text, txtemail.Text, txtaddr1.Text, txtaddr2.Text, (txtcity.Visible == true ? txtcity.Text : drpcity.SelectedValue), txtstate.Text, txtzip5.Text, txtzip4.Text, txtphone.Text);
                //this.cc = new CreditCard(drpcard.SelectedValue, txtccnum.Text, drpmonth.SelectedValue, drpyr.SelectedValue, txtcvv2.Text, this.cc.Cost, drpcard.SelectedValue );
                if (chkSameaddress.Checked)
                    this.billto = this.shipto;
                else
                    this.billto = new Person(1, txt2name.Text, txt2org.Text, txt2email.Text, txt2addr1.Text, txt2addr2.Text, (txt2city.Visible == true ? txt2city.Text : drpcity2.SelectedValue), txt2state.Text, txt2zip5.Text, txt2zip4.Text, txt2phone.Text);

                if (!UserExists(txtUserName.Text.ToString()))
                {
                    //*** Update PubEnt Registration table
                    bool retSaveRegistration = DAL.DAL.SaveRegistration(txtUserName.Text, shipto, billto, -1);  //***EAC typeofcustomerid is now ignored

                    if (retSaveRegistration)
                    {

                        //*** Create user in GUAM
                        try
                        {
                            new UserServiceClient().Using(client =>
                            {
                                ReturnObject ro;

                                //*** Add a new user 
                                ro = client.AddUser(txtUserName.Text.ToString());
                                if (ro.ReturnCode != 0)
                                {
                                    RemoveUser();
                                    lblGuamMsg.Text = ro.DefaultErrorMessage;
                                    lblGuamMsg.Visible = true;
                                }
                                else
                                {
                                    lblGuamMsg.Text = "";
                                    lblGuamMsg.Visible = false;
                                    //*** Set User Email
                                    ro = client.SetUserMetaData(txtUserName.Text.ToString(), "Email", txtUserName.Text.ToString());

                                    //*** Generate Password
                                    ro = client.GeneratePassword(txtUserName.Text.ToString());
                                    string newpwd = ro.ReturnValue.ToString();

                                    //*** Assign Password
                                    ro = client.AssignPassword(txtUserName.Text.ToString(), newpwd);

                                    //*** Set questions and answer
                                    UserQuestion[] questions_answer = new UserQuestion[1];
                                    questions_answer[0] = new UserQuestion();
                                    questions_answer[0].QuestionText = ddlQuestions.SelectedItem.Text;
                                    questions_answer[0].Answer = txtAnswer.Text;
                                    ro = client.SetUserQuestionsAndAnswers(txtUserName.Text.ToString(), questions_answer);

                                    //*** Add User to Role
                                    ro = client.AddUserToRole(txtUserName.Text.ToString(), "NCIPL_PUBLIC");

                                    //*** Registration Complete
                                    divUserReg.Visible = false;
                                    divUserRegConfirmation.Visible = true;
                                    lblRegUserName.Text = txtUserName.Text;
                                    lblRegPassword.Text = newpwd;

                                    if (Zipcode.isXPO(txtstate.Text))
                                        lblXPO.Text = "Please note: We will provide free shipping via U.S. Postal Service for orders up to " + PubEnt.GlobalUtils.Const.XPOMaxQuantity.ToString() + " items to your location. We are sorry we cannot send orders of more than " + PubEnt.GlobalUtils.Const.XPOMaxQuantity.ToString() + " items or send items via FedEx or UPS to your shipping address.";
                                    Session["NCIPL_User"] = txtUserName.Text;
                                }
                            });
                        }
                        catch
                        {
                            RemoveUser();
                            divUserRegConfirmation.Visible = false;
                            lblGuamMsg.Text = GuamErrorMsg;
                            lblGuamMsg.Visible = true;
                        }
                    }
                    else
                    {
                        divUserRegConfirmation.Visible = false;
                        lblGuamMsg.Text = PubEntErrorMsg;
                        lblGuamMsg.Visible = true;
                    }
                }
                else
                {
                    divUserRegConfirmation.Visible = false;
                    lblGuamMsg.Text = "You already have an account with us. Please login with your username and password or select the option to reset your password.";
                    lblGuamMsg.Visible = true;
                }
            }
        }

        protected bool UserExists(string sUserName)
        {
            //*** Check if user name exists
            bool existsFlag = false;
            try
            {
                new UserServiceClient().Using(client =>
                {
                    ReturnObject ro;

                    // User exist?
                    ro = client.ExistsUsername(sUserName);
                    if (ro.ReturnValue.ToString().ToLower() == "true")
                    {
                        existsFlag = true;
                    }
                });
            }
            catch
            {
                
            }

            return existsFlag;
        }

        protected void RemoveUser()
        {
            bool retDeleteRegistration = DAL.DAL.DeleteRegistration(txtUserName.Text);

        }

        protected void txtUserName_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtAnswer_TextChanged(object sender, EventArgs e)
        {

        }

        protected void TextBox4_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtorg_TextChanged(object sender, EventArgs e)
        {

        }

        protected void TextBox7_TextChanged(object sender, EventArgs e)
        {

        }

        protected void TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        protected void TextBox8_TextChanged(object sender, EventArgs e)
        {

        }
        protected void ShowBillingItems()
        {
            pnlBIllingInfo.Visible = true;
            chkSameaddress.Visible = true;
            lblBill.Visible = true;
        }
        protected void HideBillingItems()
        {
            pnlBIllingInfo.Visible = false;
            chkSameaddress.Visible = false;
            lblBill.Visible = false;
        }
        //***EAC update count, shipcosts etc
        protected void BindTotals(string state)
        {
            if (Zipcode.isXPO(state))
                HideBillingItems();
            else
                ShowBillingItems();

            //Display the master page tabs 
            GlobalUtils.Utils UtilMethod = new GlobalUtils.Utils();
            if (Session["NCIPL_Pubs"] != null)
                Master.LiteralText = UtilMethod.GetTabHtmlMarkUp(Session["NCIPL_Qtys"].ToString(), "");
            else
                Master.LiteralText = UtilMethod.GetTabHtmlMarkUp("", "");
            UtilMethod = null;
        }
    }
}
