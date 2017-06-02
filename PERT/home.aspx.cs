using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using PubEnt.DAL;
using PubEnt.BLL;
using System.Configuration;

using Microsoft.Reporting.WebForms;
using Microsoft.ReportingServices;

using System.Web.Services;
using System.Collections;
using System.Collections.Specialized;

using System.IO;
using System.Net;
using System.Security.Principal;
using System.Data;


namespace PubEnt
{
    public partial class home : System.Web.UI.Page
    {
        string strRptPath;

        #region Delegate code for usercontrol
        protected override void OnInit(EventArgs e)
        {            
            base.OnInit(e);
        }
       
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            //Check GUAM UserId, Role for NCIPL_CC
            if (GlobalUtils.UserRoles.getLoggedInUserId().Length == 0 || GlobalUtils.UserRoles.getLoggedInUserRole() < 1)
            {
                string currASPXfilename = System.IO.Path.GetFileName(Request.Path).ToString();
                Session["NCIPL_REGISTERREFERRER"] = currASPXfilename;
                Response.Redirect("~/login.aspx?msg=invaliduser&redir=" + currASPXfilename, true);
            }

//Response.Write(Session["NCIPL_Role"]);
            if (Session["NCIPL_Role"].ToString() == "NCI")
                pnStandardRpt.Visible = false;

            if (!Page.IsPostBack)
            {
                if (Request.QueryString["js"] != null) //Test for JavaScript
                    if (string.Compare(Request.QueryString["js"].ToString(), "2") == 0)
                        Session["JSTurnedOn"] = "False";                

                    IReportServerCredentials irsc = new ReportServerCredentials1((string)ConfigurationManager.AppSettings["ReportingServerUserName"],
                                                                                    (string)ConfigurationManager.AppSettings["ReportingServerPassword"],
                                                                                      (string)ConfigurationManager.AppSettings["ReportingServerDomain"]);
                    rptReport.ShowCredentialPrompts = false;
                    rptReport.ServerReport.ReportServerCredentials = irsc;
                    rptReport.ServerReport.ReportServerUrl = new Uri((string)ConfigurationManager.AppSettings["ReportingServer"]);
                    rptReport.Visible = false;
                    rptReport.ZoomPercent = 100;

                    ddlMonth.SelectedValue = (DateTime.Now.Month - 1).ToString();
                    
                    // Dynamically add current and previous years' values to list
                    string currYear = (DateTime.Now.Year).ToString();
                    string lastYear = (DateTime.Now.Year - 1).ToString();
                    ddlYear.Items.Add(new ListItem(currYear, currYear));
                    ddlYear.Items.Add(new ListItem(lastYear, lastYear));
                    ddlYear.SelectedValue = currYear;

                    DateTime firstDayOfTheMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    txtStartDate_source.Text = (DateTime.Now.Month - 1).ToString() + "/1/" + DateTime.Now.Year.ToString();
                    txtEndDate_source.Text = (DateTime.Now.Month - 1).ToString() + "/" + firstDayOfTheMonth.AddDays(-1).Day.ToString() + "/" + DateTime.Now.Year.ToString();

                    txtStartDate_type.Text = (DateTime.Now.Month - 1).ToString() + "/1/" + DateTime.Now.Year.ToString();
                    txtEndDate_type.Text = (DateTime.Now.Month - 1).ToString() + "/" + firstDayOfTheMonth.AddDays(-1).Day.ToString() + "/" + DateTime.Now.Year.ToString();

                    txtStartDate_Intl.Text = (DateTime.Now.Month - 1).ToString() + "/1/" + DateTime.Now.Year.ToString();
                    txtEndDate_Intl.Text = (DateTime.Now.Month - 1).ToString() + "/" + firstDayOfTheMonth.AddDays(-1).Day.ToString() + "/" + DateTime.Now.Year.ToString();

                    txtStartDate_Ann.Text = "1/1/" + DateTime.Now.Year.ToString();
                    txtEndDate_Ann.Text = (DateTime.Now.Month ).ToString() + "/1/" + DateTime.Now.Year.ToString();

                    txtStartDate_CancerType.Text = "1/1/" + DateTime.Now.Year.ToString();
                    txtEndDate_CancerType.Text = (DateTime.Now.Month).ToString() + "/1/" + DateTime.Now.Year.ToString();

                    txtStartDate_AnnOrder.Text = "1/1/" + DateTime.Now.Year.ToString();
                    txtEndDate_AnnOrder.Text = (DateTime.Now.Month).ToString() + "/1/" + DateTime.Now.Year.ToString();

                    if (DateTime.Now.Month == 1)
                    {
                        ddlMonth.SelectedValue = "12";
                        ddlYear.SelectedValue = lastYear;

                        txtStartDate_source.Text = "12/1/" + lastYear;
                        txtEndDate_source.Text = "12/31/" + lastYear;

                        txtStartDate_type.Text = "12/1/" + lastYear;
                        txtEndDate_type.Text = "12/31/" + lastYear;

                        txtStartDate_Intl.Text = "12/1/" + lastYear;
                        txtEndDate_Intl.Text = "12/31/" + lastYear;
                    }
               
            }

            //Begin - Code for Appropriate Tabs
            GlobalUtils.Utils UtilMethod = new GlobalUtils.Utils();
            if (Session["NCIPL_Pubs"] != null)
                Master.LiteralText = UtilMethod.GetTabHtmlMarkUp(Session["NCIPL_Qtys"].ToString(), "home");
            else
                Master.LiteralText = UtilMethod.GetTabHtmlMarkUp("", "home");
            UtilMethod = null;
            //End Code for Tab

        }

        protected void btnGenMonthDis_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {                
                rptReport.Visible = true;
                strRptPath = "ReportFile1";
                rptReport.ServerReport.ReportPath = (string)ConfigurationManager.AppSettings["ReportPath"] + (string)ConfigurationManager.AppSettings[strRptPath];
                ReportParameter[] p = new ReportParameter[1];                
                p.SetValue(new ReportParameter("ReportMonth", ddlMonth.SelectedItem.ToString() + " " + ddlYear.SelectedValue), 0);
                rptReport.ServerReport.SetParameters(p);
                rptReport.ServerReport.Refresh();
            }
            else
                rptReport.Visible = false;
        }

        protected void btnGenOrderBySource_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                rptReport.Visible = true;
                strRptPath = "ReportFile2";
                rptReport.ServerReport.ReportPath = (string)ConfigurationManager.AppSettings["ReportPath"] + (string)ConfigurationManager.AppSettings[strRptPath];
                ReportParameter[] p = new ReportParameter[2];
                p.SetValue(new ReportParameter("Start_Date", txtStartDate_source.Text), 0);
                p.SetValue(new ReportParameter("End_Date", txtEndDate_source.Text), 1);
                rptReport.ServerReport.SetParameters(p);
                rptReport.ServerReport.Refresh();
            }
            else
                rptReport.Visible = false;
        }

        protected void btnGenOrderByType_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                rptReport.Visible = true;
                strRptPath = "ReportFile3";
                rptReport.ServerReport.ReportPath = (string)ConfigurationManager.AppSettings["ReportPath"] + (string)ConfigurationManager.AppSettings[strRptPath];
                ReportParameter[] p = new ReportParameter[2];
                p.SetValue(new ReportParameter("start_date", txtStartDate_type.Text), 0);
                p.SetValue(new ReportParameter("end_date", txtEndDate_type.Text), 1);
                rptReport.ServerReport.SetParameters(p);
                rptReport.ServerReport.Refresh();
            }
            else
                rptReport.Visible = false;
        }

        protected void btnGenOrderIntl_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                rptReport.Visible = true;
                strRptPath = "ReportFile4";
                rptReport.ServerReport.ReportPath = (string)ConfigurationManager.AppSettings["ReportPath"] + (string)ConfigurationManager.AppSettings[strRptPath];
                ReportParameter[] p = new ReportParameter[2];
                p.SetValue(new ReportParameter("Start_Date", txtStartDate_Intl.Text), 0);
                p.SetValue(new ReportParameter("End_Date", txtEndDate_Intl.Text), 1);
                rptReport.ServerReport.SetParameters(p);
                rptReport.ServerReport.Refresh();
            }
            else
                rptReport.Visible = false;
        }

        protected void btnGenOrderAnn_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                rptReport.Visible = true;
                strRptPath = "ReportFile5";
                rptReport.ServerReport.ReportPath = (string)ConfigurationManager.AppSettings["ReportPath"] + (string)ConfigurationManager.AppSettings[strRptPath];
                ReportParameter[] p = new ReportParameter[2];
                p.SetValue(new ReportParameter("Start_date", txtStartDate_Ann.Text), 0);
                p.SetValue(new ReportParameter("End_date", txtEndDate_Ann.Text), 1);
                rptReport.ServerReport.SetParameters(p);
                rptReport.ServerReport.Refresh();
            }
            else
                rptReport.Visible = false;
        }

        protected void btnGenOrderCancerType_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                rptReport.Visible = true;
                strRptPath = "ReportFile6";
                rptReport.ServerReport.ReportPath = (string)ConfigurationManager.AppSettings["ReportPath"] + (string)ConfigurationManager.AppSettings[strRptPath];
                ReportParameter[] p = new ReportParameter[2];
                p.SetValue(new ReportParameter("Start_Date", txtStartDate_CancerType.Text), 0);
                p.SetValue(new ReportParameter("End_Date", txtEndDate_CancerType.Text), 1);
                rptReport.ServerReport.SetParameters(p);
                rptReport.ServerReport.Refresh();
            }
            else
                rptReport.Visible = false;
        }

        protected void btnGenOrderAnnOrder_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                rptReport.Visible = true;
                strRptPath = "ReportFile7";
                rptReport.ServerReport.ReportPath = (string)ConfigurationManager.AppSettings["ReportPath"] + (string)ConfigurationManager.AppSettings[strRptPath];
                ReportParameter[] p = new ReportParameter[2];
                p.SetValue(new ReportParameter("Start_date", txtStartDate_AnnOrder.Text), 0);
                p.SetValue(new ReportParameter("End_date", txtEndDate_AnnOrder.Text), 1);
                rptReport.ServerReport.SetParameters(p);
                rptReport.ServerReport.Refresh();
            }
            else
                rptReport.Visible = false;
        }
        
        
        

        protected void validateCustom_source(object source, ServerValidateEventArgs args)
        {           
            args.IsValid = true;
            if (txtStartDate_source.Text == "" | txtEndDate_source.Text == "") return;
            if (Convert.ToDateTime(txtStartDate_source.Text) > Convert.ToDateTime(txtEndDate_source.Text))
            {
                args.IsValid = false;
                valDateSource.ErrorMessage = "Start date should be less than End date.";
            }
            else if (Convert.ToDateTime(txtEndDate_source.Text)>DateTime.Now)
            {
                args.IsValid = false;
                valDateSource.ErrorMessage = "End date should be less than current date.";
            }
            else if (Convert.ToDateTime(txtStartDate_source.Text) < Convert.ToDateTime("1/1/2012") | Convert.ToDateTime(txtEndDate_source.Text) < Convert.ToDateTime("1/1/2012"))
            {
                args.IsValid = false;
                valDateSource.ErrorMessage = "Date should be less than 1/1/2012.";
            }
        }
        protected void validateCustom_type(object source, ServerValidateEventArgs args)
        {           
            args.IsValid = true;
            if (txtStartDate_type.Text == "" | txtEndDate_type.Text == "") return;
            if (Convert.ToDateTime(txtStartDate_type.Text) > Convert.ToDateTime(txtEndDate_type.Text))
            {
                args.IsValid = false;
                valDateType.ErrorMessage = "Start date should be less than End date.";
            }
            else if (Convert.ToDateTime(txtEndDate_type.Text) > DateTime.Now)
            {
                args.IsValid = false;
                valDateType.ErrorMessage = "End date should be less than current date.";
            }
            else if (Convert.ToDateTime(txtStartDate_type.Text) < Convert.ToDateTime("1/1/2012") | Convert.ToDateTime(txtEndDate_type.Text) < Convert.ToDateTime("1/1/2012"))
            {
                args.IsValid = false;
                valDateType.ErrorMessage = "Date should be less than 1/1/2012.";
            }
        }

        protected void validateCustom_Intl(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
            if (txtStartDate_Intl.Text == "" | txtEndDate_Intl.Text == "") return;
            if (Convert.ToDateTime(txtStartDate_Intl.Text) > Convert.ToDateTime(txtEndDate_Intl.Text))
            {
                args.IsValid = false;
                valDateIntl.ErrorMessage = "Start date should be less than End date.";
            }
            else if (Convert.ToDateTime(txtEndDate_Intl.Text) > DateTime.Now)
            {
                args.IsValid = false;
                valDateIntl.ErrorMessage = "End date should be less than current date.";
            }
            else if (Convert.ToDateTime(txtStartDate_Intl.Text) < Convert.ToDateTime("1/1/2012") | Convert.ToDateTime(txtEndDate_Intl.Text) < Convert.ToDateTime("1/1/2012"))
            {
                args.IsValid = false;
                valDateIntl.ErrorMessage = "Date should be less than 1/1/2012.";
            }
        }

        protected void validateCustom_Ann(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
            if (txtStartDate_Ann.Text == "" | txtEndDate_Ann.Text == "") return;
            if (Convert.ToDateTime(txtStartDate_Ann.Text) > Convert.ToDateTime(txtEndDate_Ann.Text))
            {
                args.IsValid = false;
                valDateAnn.ErrorMessage = "Start date should be less than End date.";
            }
            else if (Convert.ToDateTime(txtEndDate_Ann.Text) > DateTime.Now)
            {
                args.IsValid = false;
                valDateAnn.ErrorMessage = "End date should be less than current date.";
            }
            else if (Convert.ToDateTime(txtStartDate_Ann.Text) < Convert.ToDateTime("1/1/2012") | Convert.ToDateTime(txtEndDate_Ann.Text) < Convert.ToDateTime("1/1/2012"))
            {
                args.IsValid = false;
                valDateAnn.ErrorMessage = "Date should be less than 1/1/2012.";
            }
        }

        protected void validateCustom_CancerType(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
            if (txtStartDate_CancerType.Text == "" | txtEndDate_CancerType.Text == "") return;
            if (Convert.ToDateTime(txtStartDate_CancerType.Text) > Convert.ToDateTime(txtEndDate_CancerType.Text))
            {
                args.IsValid = false;
                valDateCancerType.ErrorMessage = "Start date should be less than End date.";
            }
            else if (Convert.ToDateTime(txtEndDate_CancerType.Text) > DateTime.Now)
            {
                args.IsValid = false;
                valDateCancerType.ErrorMessage = "End date should be less than current date.";
            }
            else if (Convert.ToDateTime(txtStartDate_CancerType.Text) < Convert.ToDateTime("1/1/2012") | Convert.ToDateTime(txtEndDate_CancerType.Text) < Convert.ToDateTime("1/1/2012"))
            {
                args.IsValid = false;
                valDateCancerType.ErrorMessage = "Date should be less than 1/1/2012.";
            }
        }

        protected void validateCustom_AnnOrder(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
            if (txtStartDate_AnnOrder.Text == "" | txtEndDate_CancerType.Text == "") return;
            if (Convert.ToDateTime(txtStartDate_CancerType.Text) > Convert.ToDateTime(txtEndDate_CancerType.Text))
            {
                args.IsValid = false;
                valDateCancerType.ErrorMessage = "Start date should be less than End date.";
            }
            else if (Convert.ToDateTime(txtEndDate_AnnOrder.Text) > DateTime.Now)
            {
                args.IsValid = false;
                valDateAnnOrder.ErrorMessage = "End date should be less than current date.";
            }
            else if (Convert.ToDateTime(txtStartDate_AnnOrder.Text) < Convert.ToDateTime("1/1/2012") | Convert.ToDateTime(txtEndDate_AnnOrder.Text) < Convert.ToDateTime("1/1/2012"))
            {
                args.IsValid = false;
                valDateAnnOrder.ErrorMessage = "Date should be less than 1/1/2012.";
            }
        }

        

        // credentials for the report
        [Serializable]
        public class ReportServerCredentials1 : Microsoft.Reporting.WebForms.IReportServerCredentials
        {
            // local variable for network credential.
            private string _UserName;
            private string _PassWord;
            private string _DomainName;

            public ReportServerCredentials1(string UserName, string PassWord, string DomainName)
            {
                _UserName = UserName;
                _PassWord = PassWord;
                _DomainName = DomainName;
            }

            public WindowsIdentity ImpersonationUser
            {
                get
                {
                    return null;  // not use ImpersonationUser
                }
            }

            public ICredentials NetworkCredentials
            {
                get
                {
                    // use NetworkCredentials
                    return new NetworkCredential(_UserName, _PassWord, _DomainName);
                }
            }

            public bool GetFormsCredentials(out Cookie authCookie, out string user, out string password, out string authority)
            {
                authCookie = new Cookie();
                user = string.Empty;
                password = string.Empty;
                authority = string.Empty;
                return false;
            }
        }

  
    }
}
