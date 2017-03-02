using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using AjaxControlToolkit;
using GlobalUtils;

using PubEntAdmin.BLL;
using PubEntAdmin.DAL;


namespace PubEntAdmin
{
    public partial class Conference : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session.IsNewSession)
            {
                Response.Redirect("Home.aspx");
            }

            this.Title = "Exhibit ";
            this.PageTitle = "Exhibit Details";

            if (1 == 2) // temp auth fix
            {
                PubEntAdminManager.UnathorizedAccess();
            }
            if (!Page.IsPostBack)
            {
                BindConfData();
                BindTimeoutData();
            }

            txtRotateTime.Style["text-align"] = "right";
            txtPageTime.Style["text-align"] = "right";
            txtSessionTime.Style["text-align"] = "right";
        }
        
        #region Methods
        protected void BindConfData()
        {
            this.gvResult.DataSource = LU_DAL.DisplayConf();
            this.gvResult.DataBind();
        }

        protected void SetConfData(string confName, int confseq, int maxOrder, DateTime sDate, DateTime eDate)
        {
            LU_DAL.SetConf(confName, confseq, maxOrder, sDate, eDate);
            Response.Redirect("Conference.aspx", false);
        }

        protected void SetTimoutData(int RotateTime, int PageTime, int SessionTime)
        {

            LU_DAL.SetTimeout(RotateTime, PageTime, SessionTime, ((CustomPrincipal)HttpContext.Current.User).UserID);
            Response.Redirect("Conference.aspx", false);
        }

        protected void UpdateConfData(int Confid,string confname,int maxOrder, DateTime sDate, DateTime eDate)
        {
            LU_DAL.UpdateConf(Confid,confname, maxOrder, sDate, eDate);
            Response.Redirect("Conference.aspx", false);
        }

        protected void BindTimeoutData()
        {
            DataSet ds = new DataSet();
            ds = LU_DAL.displayTimeouts();

            if (ds.Tables[0].Rows.Count >0)
            {
                this.pnlTimoutEdit.Visible = false;
                this.pnlTimeoutDisplay.Visible = true;

                this.lblRotationTime.Text = ds.Tables[0].Rows[0].ItemArray[0].ToString();
                this.lblPageTime.Text = ds.Tables[0].Rows[0].ItemArray[1].ToString();
                this.lblSessionTime.Text = ds.Tables[0].Rows[0].ItemArray[2].ToString();

                this.txtRotateTime.Text = ds.Tables[0].Rows[0].ItemArray[0].ToString();
                this.txtPageTime.Text = ds.Tables[0].Rows[0].ItemArray[1].ToString();
                this.txtSessionTime.Text = ds.Tables[0].Rows[0].ItemArray[2].ToString();
            }
            else
            {
                this.pnlTimoutEdit.Visible = true;
                this.pnlTimeoutDisplay.Visible = false;
                btnAddTime.Visible = true;
            }
            
        }

        #endregion

        protected void gvResult_ItemCreated(object source, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.Item.Cells[5].Controls[0] is Button)
                {
                    Button l_lnkbtnEdit = e.Item.Cells[5].Controls[0] as Button;
                    l_lnkbtnEdit.ID = "gvResult_Edit";
                }

            }
            else if ((e.Item.ItemType == ListItemType.EditItem))
            {
                if (e.Item.Cells[5].Controls[0] is Button)
                {
                    Button l_btnUpdate = ((Button)e.Item.Cells[5].Controls[0]);
                    l_btnUpdate.ID = "gvResult_Update";
                }

                if (e.Item.Cells[5].Controls[2] is Button)
                {
                    Button l_btnCancel = ((Button)e.Item.Cells[5].Controls[2]);
                    l_btnCancel.ID = "gvResult_Cancel";
                }
            }
        }

        protected void gvResult_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            string vInactive = ((System.Web.UI.WebControls.Button)(e.CommandSource)).Text;

            if (vInactive == "Delete")
                gvResult_DeleteCommand(source, e);

            if (vInactive == "Rotation Publications")
            {
                string strConfid = ((Label)e.Item.Cells[0].Controls[1]).Text;
                int confid = Convert.ToInt32(strConfid);

                string strConfname = ((Label)e.Item.Cells[1].Controls[1]).Text;

                string strDates = ((Label)e.Item.Cells[2].Controls[1]).Text + " - " + ((Label)e.Item.Cells[3].Controls[1]).Text;

                gvRotatPubs.DataSource = LU_DAL.DisplayRotationPubs(confid);
                gvRotatPubs.EmptyDataText = "Sorry, no publications have been selected for " + strConfname;
                gvRotatPubs.Caption = "<strong>Rotation Publications for " + strConfname + "</strong><br>" + strDates + "<br><br>";
                gvRotatPubs.DataBind();


                PubEntAdminManager.ExportToExcel(gvRotatPubs, this.Page);

                //PubEntAdminManager.ExportGridViewToExcel(gvRotatPubs, "RotationPublicationsNew", "<strong>Rotation Publications for " + strConfname + "</strong><br>" + strDates, this.Page.Response);


                //ExportRoutines.ExportToExcel(this.Page, "RotationPublications", "<strong>Rotation Publications for " + strConfname + "</strong><br>" + strDates, this.gvRotatPubs);            

            }
           
        }        

        protected void gvResult_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            ConfCollection dt = ((ConfCollection)this.gvResult.DataSource);
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                
                Conf l_conf = dt[e.Item.ItemIndex];
                //((TableCell)e.Item.Cells[0]).Text = l_conf.ConfID.ToString();
                ((Label)e.Item.Cells[1].Controls[1]).Text = l_conf.ConfName;
                ((Label)e.Item.Cells[2].Controls[1]).Text = l_conf.StartDate.ToShortDateString();
                ((Label)e.Item.Cells[3].Controls[1]).Text = l_conf.EndDate.ToShortDateString();
                ((Label)e.Item.Cells[4].Controls[1]).Text = l_conf.MaxOrder_INTL.ToString();

                //delete btn col
                Button l_able = e.Item.Cells[6].FindControl("lnkbtnDel") as Button;
                l_able.Text = "Delete";
                Panel l_pnl = e.Item.Cells[6].FindControl("pnlConfirmDel") as Panel;
                ((Label)l_pnl.Controls[1]).Text = "Are you sure you want to delete this Exhibit Record?";

               
            }
            else if (e.Item.ItemType == ListItemType.EditItem)
            {
                Conf l_conf = dt[e.Item.ItemIndex];

                //delete btn col
                Button l_able = e.Item.Cells[6].FindControl("lnkbtnDel") as Button;
                l_able.Text = "Delete";
                Panel l_pnl = e.Item.Cells[6].FindControl("pnlConfirmDel") as Panel;
                ((Label)l_pnl.Controls[1]).Text = "Are you sure you want to delete this Exhibit Record?";

                //confirmation for cancel
                if (e.Item.Cells[5].Controls[2] is Button)
                {
                    Button l_btnCancel = ((Button)e.Item.Cells[5].Controls[2]);

                    Panel l_panel = new Panel();
                    l_panel.ID = "l_panel";
                    l_panel.CssClass = "modalPopup";
                    l_panel.Style.Add("display", "none");
                    l_panel.Width = Unit.Pixel(233);

                    Label l_label = new Label();
                    l_label.Text = "Are you sure you want to continue?";

                    HtmlGenericControl l_div = new HtmlGenericControl();
                    Button l_ok = new Button();
                    Button l_cancel = new Button();
                    l_ok.ID = "l_ok";
                    l_ok.Text = "OK";
                    l_cancel.ID = "l_cancel";
                    l_cancel.Text = "Cancel";
                    l_div.Controls.Add(l_ok);
                    l_div.Controls.Add(new LiteralControl("&nbsp;"));
                    l_div.Controls.Add(l_cancel);
                    l_div.Attributes.Add("align", "center");

                    l_panel.Controls.Add(l_label);
                    l_panel.Controls.Add(new LiteralControl("<br>"));
                    l_panel.Controls.Add(new LiteralControl("<br>"));
                    l_panel.Controls.Add(l_div);

                    ModalPopupExtender l_mpe = new ModalPopupExtender();
                    l_mpe.ID = "l_mpe";
                    l_mpe.TargetControlID = l_btnCancel.ID;
                    l_mpe.PopupControlID = l_panel.ID;
                    l_mpe.BackgroundCssClass = "modalBackground";
                    l_mpe.DropShadow = true;
                    l_mpe.OkControlID = l_ok.ID;
                    l_mpe.CancelControlID = l_cancel.ID;

                    ConfirmButtonExtender l_cbe = new ConfirmButtonExtender();
                    l_cbe.TargetControlID = l_btnCancel.ID;
                    l_cbe.ConfirmText = "";
                    l_cbe.DisplayModalPopupID = l_mpe.ID;

                    e.Item.Cells[5].Controls.Add(l_panel);
                    e.Item.Cells[5].Controls.Add(l_mpe);
                    e.Item.Cells[5].Controls.Add(l_cbe);
             
                }

                if (this.gvResult.EditItemIndex != -1)
                {
                    ((MaskedEditValidator)e.Item.Cells[2].Controls[7]).Enabled = true;
                    ((MaskedEditValidator)e.Item.Cells[3].Controls[7]).Enabled = true;
                }
                else
                {
                    ((MaskedEditValidator)e.Item.Cells[2].Controls[7]).Enabled = false;
                    ((MaskedEditValidator)e.Item.Cells[3].Controls[7]).Enabled = false;
                }

            }
        }

        protected void gvResult_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            this.gvResult.EditItemIndex = -1;
            this.BindConfData();
         }

        protected void gvResult_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.gvResult.EditItemIndex = e.Item.ItemIndex;
            this.BindConfData();

        }

        protected void gvResult_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            string strConfid = ((Label)e.Item.Cells[0].Controls[1]).Text;
            int confid = Convert.ToInt32(strConfid); 
            string confname = ((TextBox)e.Item.Cells[1].Controls[1]).Text;
            string strsDate = ((TextBox)e.Item.Cells[2].Controls[1]).Text;
            DateTime sDate = Convert.ToDateTime(strsDate); 
            string streDate = ((TextBox)e.Item.Cells[3].Controls[1]).Text;
            DateTime eDate = Convert.ToDateTime(streDate);
            string strMaxOrder = ((TextBox)e.Item.Cells[4].Controls[1]).Text;
            int maxOrder = Convert.ToInt32(strMaxOrder);

            Boolean valid = false;
            Boolean validnum = false;
            Boolean validlen = false;

            if (confname.Length > 0)
            {
                valid = PubEntAdminManager.OtherVal(confname);
                validnum = PubEntAdminManager.SpecialVal2(confname);
                validlen = PubEntAdminManager.LenVal(confname, 10);
            }

            if ((valid == false) && (validnum == false) && (validlen == true))
            {

                this.UpdateConfData(confid, confname,maxOrder, sDate, eDate);
            }
            else
            {
                Response.Redirect("InvalidInput.aspx");
            }
       }

        protected void btnAdd_Click(object sender, EventArgs e)
        {

            this.SecVal();
            Message.Text = "";
            string confName = this.txtConfName.Text.Trim();
            string strsDate = this.txtStartDate.Text;
            string streDate = this.txtEndDate.Text;
            string strMaxOrder = this.txtMaxOrder.Text.Trim();
            int confseq = 0;


            if (confName.Length == 0 || strsDate.Length == 0 || streDate.Length == 0 || strMaxOrder.Length == 0)
            {
                Message.Text = "Please enter required values";
            }
            else if (strsDate.Length != 0 && streDate.Length != 0)
            {
                int iMaxOrder = Convert.ToInt32(this.txtMaxOrder.Text.Trim());
                DateTime sDate = Convert.ToDateTime(strsDate);
                DateTime eDate = Convert.ToDateTime(streDate);
                if (sDate > eDate)
                {
                    Message.Text = "Start Date cannot be greater than End Date";
                } 
                else if (confName.Length != 0 && strsDate.Length != 0 && streDate.Length != 0 && (Convert.ToDateTime(strsDate) < Convert.ToDateTime(streDate)))
                {
                    this.SetConfData(confName, confseq, iMaxOrder, sDate, eDate);
                }
            }
        
        }        

        protected void gvResult_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
           Label confid = ((Label)e.Item.Cells[0].FindControl("sConfid"));
           LU_DAL.DeleteConfLU(System.Convert.ToInt32(confid.Text.Trim()));
           Response.Redirect("Conference.aspx", false);
        }

        protected void btnTimeoutAdd_click(object sender, EventArgs e)
        {

            this.SecVal();
            lblTimeoutErrorMeg.Text = "";
            string RotateTime = this.txtRotateTime.Text;
            string PageTime = this.txtPageTime.Text;
            string SessionTime = this.txtSessionTime.Text;


            if (RotateTime.ToString().Length == 0 || PageTime.ToString().Length == 0 || SessionTime.ToString().Length == 0)
            {
                lblTimeoutErrorMeg.Text = "Please fill out all three timeout fields<br><br>";
            }
            else
            {
                int iRotateTime = Convert.ToInt32(RotateTime);
                int iPageTime = Convert.ToInt32(PageTime);
                int iSessionTime = Convert.ToInt32(SessionTime);

                this.SetTimoutData(iRotateTime, iPageTime, iSessionTime);
            }

        }

        protected void btnTimoutEdit_click(object sender, EventArgs e)
        {
            this.pnlTimoutEdit.Visible = true;
            this.pnlTimeoutDisplay.Visible = false;
            this.btnAddTime.Visible = false;
            this.btnUpdateTime.Visible = true;
            this.btnCancelTime.Visible = true;         
           
        }

        protected void btnTimeoutCancel_click(object sender, EventArgs e)
        {
            Response.Redirect("Conference.aspx", false);
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            return;
        }
       
        #region Properties
        public string PageTitle
        {
            set
            {
                ((Label)this.Master.FindControl("lblPageTitle")).Text = value;
            }
            get
            {
                return ((Label)this.Master.FindControl("lblPageTitle")).Text;
            }
        }
        #endregion


        #region Sec Val
        private void SecVal()
        {
            this.LenVal();
            this.TagVal();
            this.SpecialVal();
        }

        private void LenVal()
        {
            if (txtConfName.Text.Length > 0)
            {
                if (!PubEntAdminManager.LenVal(this.txtConfName.Text, 10))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            if (txtRotateTime.Text.Length > 0 || txtPageTime.Text.Length > 0 || txtSessionTime.Text.Length > 0)
            {
                if (!PubEntAdminManager.LenVal(this.txtRotateTime.Text, 5) && !PubEntAdminManager.LenVal(this.txtPageTime.Text, 5)
                    && !PubEntAdminManager.LenVal(this.txtSessionTime.Text, 5))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }
           
        }

        private void TagVal()
        {
            if (txtConfName.Text.Length > 0)
            {
                if (PubEntAdminManager.OtherVal(this.txtConfName.Text))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            if (txtRotateTime.Text.Length > 0 || txtPageTime.Text.Length > 0 || txtSessionTime.Text.Length > 0)
            {
                if (PubEntAdminManager.OtherVal(this.txtRotateTime.Text) && PubEntAdminManager.OtherVal(this.txtPageTime.Text) &&
                    PubEntAdminManager.OtherVal(this.txtSessionTime.Text))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }
            
        }

        private void SpecialVal()
        {
            if (txtConfName.Text.Length > 0)
            {
                if (PubEntAdminManager.SpecialVal2(this.txtConfName.Text.Replace(" ", "")))
                {
                    Response.Redirect("InvalidInput.aspx");
                }

            }   
         
            if(txtRotateTime.Text.Length > 0 || txtPageTime.Text.Length > 0 || txtSessionTime.Text.Length>0)
            {
                if (PubEntAdminManager.SpecialVal2(this.txtRotateTime.Text.Replace(" ", "")) && PubEntAdminManager.SpecialVal2(this.txtPageTime.Text.Replace(" ", "")) &&
                    PubEntAdminManager.SpecialVal2(this.txtSessionTime.Text.Replace(" ", "")))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }
            
        }
        #endregion
   
    }
}
