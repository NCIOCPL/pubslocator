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
using PubEntAdmin.DAL;
using PubEntAdmin.BLL;

using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Logging;
//using Microsoft.Practices.ObjectBuilder2;
using System.Text.RegularExpressions;

namespace PubEntAdmin.UserControl
{
    public partial class Award : System.Web.UI.UserControl
    {
        protected System.Web.UI.WebControls.Label lblMessage;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.BindData();
            this.SecVal();
        }

       #region Sec Val
        private void SecVal()
        {
            this.LenVal();
            this.TagVal();
            this.SpecialVal();
        }

        private void LenVal()
        {
            if ((!PubEntAdminManager.LenVal(this.txtAwdName.Text, 50)))
            {
                Response.Redirect("InvalidInput.aspx");
            }
        }

        private void TagVal()
        {
            if ((PubEntAdminManager.OtherVal(this.txtAwdName.Text)))
            {
                Response.Redirect("InvalidInput.aspx");
            }

        }

        private void SpecialVal()
        {

            if ((PubEntAdminManager.SpecialVal2(this.txtAwdName.Text.Replace(" ", ""))))
            {
                Response.Redirect("InvalidInput.aspx");
            }

        }
#endregion

        protected void Page_PreRender(object sender, System.EventArgs e)
        {
           ((LookupMgmt)this.Page).ShowHideSpellChecker(true);
             
            ScriptManager.RegisterClientScriptBlock(this,
                    this.GetType(), "Subject_ClientScript",
                @"
                    function LkupParticipants()
                    {
                        return '" + this.GetSpellCheckParticipants() + @"';
                    }
                ", true);
        }

        protected void BindData()
        {
            this.gvResult.DataSource = LU_DAL.GetAwardLU();
            this.gvResult.DataBind();

        }

        protected void gvResult_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            this.gvResult.EditItemIndex = -1;
            this.BindData();
        }

        protected void gvResult_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.gvResult.EditItemIndex = e.Item.ItemIndex;
            this.BindData();
        }

        protected void gvResult_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            string vInactive = ((System.Web.UI.WebControls.Button)(e.CommandSource)).Text;

            if ((vInactive == "Inactive") || (vInactive == "Active"))
                gvResult_DeleteCommand(source, e);
        }

        protected void gvResult_SortCommand(object source, DataGridSortCommandEventArgs e)
        {

        }

        protected void gvResult_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            AwardCollection dt = ((AwardCollection)this.gvResult.DataSource);
          
              PubEntAdmin.BLL.Award l_conf = dt[e.Item.ItemIndex];
              int Awdid = l_conf.AwardID;
              string Awdname = "";
              string Awdyear = "";
              string Awdcategory = "";
             
             
              Awdname = ((TextBox)e.Item.Cells[1].Controls[1]).Text;
              Awdcategory = ((TextBox)e.Item.Cells[1].Controls[3]).Text;
              Awdyear = ((TextBox)e.Item.Cells[1].Controls[5]).Text;

              if (Awdyear == "")
              {                  
                  string confirm = "Award Year cannot be empty.";
                  ((Label)e.Item.Cells[5].Controls[1]).Text = confirm;
              }
              else
              {

                  int Active = 0;
                  string sActive = ((Label)e.Item.Cells[2].Controls[1]).Text;
                  if (sActive == "Active")
                  { Active = 1; }
                  else
                  { Active = 0; }
                  ((TextBox)e.Item.Cells[1].Controls[1]).Visible = false;

                  Boolean valid = false;
                  Boolean validnum = false;
                  Boolean validlen = true;

                  if (Awdname != null && Awdname.Length != 0)
                  {
                      valid = PubEntAdminManager.OtherVal(Awdname);
                      validnum = PubEntAdminManager.SpecialVal2(Awdname);
                      validlen = PubEntAdminManager.LenVal(Awdname, 50);
                  }

                  if (Awdcategory != null && Awdcategory.Length != 0)
                  {
                      valid = PubEntAdminManager.OtherVal(Awdcategory);
                      validnum = PubEntAdminManager.SpecialVal2(Awdcategory);
                      validlen = PubEntAdminManager.LenVal(Awdcategory, 100);
                  }
                  if (Awdyear != null && Awdyear.Length != 0)
                  {
                      valid = PubEntAdminManager.OtherVal(Awdyear);
                      validnum = PubEntAdminManager.SpecialVal2(Awdyear);
                      validlen = PubEntAdminManager.LenVal(Awdyear, 10);
                  }
                  if ((valid == false) && (validnum == false) && (validlen == true))
                  {
                      DataSet ds = new DataSet();
                      ds = LU_DAL.displayAward();                     

                      string dbAwdname = "";
                      string dbAwdCate = "";
                      string dbAwdYear = "";
                      Boolean iExist=false;

                      for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                      {
                          DataRow dataRow = ds.Tables[0].Rows[i];
                          dbAwdname = (string)dataRow["AWARD_NAME"].ToString().Trim();
                          dbAwdCate = (string)dataRow["AWARD_Category"].ToString().Trim();
                          dbAwdYear = (string)dataRow["AWARD_YEAR"].ToString().Trim();

                          if (Awdname.Trim() == dbAwdname && Awdcategory.Trim() == dbAwdCate && Awdyear.Trim() == dbAwdYear)
                          {
                              iExist = true;
                          }
                      }

                      if (iExist)
                      {
                          e.Item.Cells[1].Controls[1].Visible = true;
                          string confirm = " The Lookup value already exists.";
                          ((Label)e.Item.Cells[5].Controls[1]).Text = confirm;
                      }
                      else
                      {
                          LU_DAL.UpdateAwardLU(Awdid, Awdname, Awdyear, Awdcategory, Active);
                          Response.Redirect("~/LookupMgmt.aspx?sub=award");
                      } 
                  }
                  else
                  {
                      Response.Redirect("InvalidInput.aspx");
                  }
              }

          
       }
       
        protected void gvResult_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            string sYear = "";
            AwardCollection dt = ((AwardCollection)this.gvResult.DataSource);
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                PubEntAdmin.BLL.Award l_conf = dt[e.Item.ItemIndex];
                string Awardid = Convert.ToString(l_conf.AwardID);

                sYear = LU_DAL.GetAwardLUbyAwardid(Awardid);

                ((Label)e.Item.Cells[1].Controls[1]).Text = Server.HtmlEncode(l_conf.AwardDescription);

                //delete btn col
                Button l_able = e.Item.Cells[5].FindControl("lnkbtnDel") as Button;

                if (l_conf.Checked)
                {
                    ((Label)e.Item.Cells[2].Controls[1]).Text = "Active";
                    l_able.Text = "Inactivate";
                    Panel l_pnl = e.Item.Cells[5].FindControl("pnlConfirmDel") as Panel;
                    ((Label)l_pnl.Controls[1]).Text = "Are you sure you want to inactivate this Lookup Value [" + Server.HtmlEncode(l_conf.AwdName) + "]?";
                }
                else
                {
                    ((Label)e.Item.Cells[2].Controls[1]).Text = "Inactive";
                    l_able.Text = "Activate";
                    Panel l_pnl = e.Item.Cells[5].FindControl("pnlConfirmDel") as Panel;
                    ((Label)l_pnl.Controls[1]).Text = "Are you sure you want to activate this Lookup Value [" + Server.HtmlEncode(l_conf.AwdName) + "]?";
                }
           }
            else if (e.Item.ItemType == ListItemType.EditItem)
            {
                PubEntAdmin.BLL.Award l_conf = dt[e.Item.ItemIndex];
                string Awardid = Convert.ToString(l_conf.AwardID);
                sYear = LU_DAL.GetAwardLUbyAwardid(Awardid);
                //((TextBox)e.Item.Cells[1].Controls[1]).Text = Server.HtmlEncode(l_conf.AwdName);
                String status = "";
                if (l_conf.Checked == true)
                { status = "Active"; }
                else
                { status = "Inactive"; }
                ((Label)e.Item.Cells[2].Controls[1]).Text = status;

                if (e.Item.Cells[3].Controls[2] is Button)
                {
                    Button l_btnCancel = ((Button)e.Item.Cells[3].Controls[2]);
                    l_btnCancel.ID = "gvResult_Cancel";

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

                    e.Item.Cells[3].Controls.Add(l_panel);
                    e.Item.Cells[3].Controls.Add(l_mpe);
                    e.Item.Cells[3].Controls.Add(l_cbe);
                }

                //delete btn col
                Button l_able = e.Item.Cells[5].FindControl("lnkbtnDel") as Button;

                if (l_conf.Checked)
                {
                    ((Label)e.Item.Cells[2].Controls[1]).Text = "Active";
                    l_able.Text = "Inactivate";
                    Panel l_pnl = e.Item.Cells[5].FindControl("pnlConfirmDel") as Panel;
                    ((Label)l_pnl.Controls[1]).Text = "Are you sure you want to inactivate this Lookup Value [" + Server.HtmlEncode(l_conf.AwdName) + "]?";
                }
                else
                {
                    ((Label)e.Item.Cells[2].Controls[1]).Text = "Inactive";
                    l_able.Text = "Activate";
                    Panel l_pnl = e.Item.Cells[5].FindControl("pnlConfirmDel") as Panel;
                    ((Label)l_pnl.Controls[1]).Text = "Are you sure you want to activate this Lookup Value [" + Server.HtmlEncode(l_conf.AwdName) + "]?";
                }

            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Message.Visible = false;

            if ((this.txtAwdYear.Text == string.Empty) || (this.txtAwdYear.Text == null))
            {
                string confirm = "Award Year cannot be empty.";
                Message.Text = confirm;
                Message.Visible = true;
            }
            else if (txtAwdName.Text == string.Empty || this.txtAwdName.Text == null)
            {
                string confirm = "Cannot add empty Lookup Value.";
                Message.Text = confirm;
                Message.Visible = true;
            }

            else
            {
                string awddesc = "";
                if (this.txtAwdCategory.Text == string.Empty && this.txtAwdName.Text == string.Empty)
                    awddesc = this.txtAwdYear.Text.Trim();

                else if (this.txtAwdCategory.Text == string.Empty)
                    awddesc = this.txtAwdName.Text.Trim() + ", " + this.txtAwdYear.Text.Trim();

                else if (this.txtAwdName.Text == string.Empty)
                {
                    awddesc = this.txtAwdCategory.Text.Trim() + " , " + this.txtAwdYear.Text.Trim();
                }
                else
                    awddesc = this.txtAwdName.Text.Trim() + ", " + this.txtAwdCategory.Text.Trim() + " , " + this.txtAwdYear.Text.Trim();

                Boolean valid = false;
                Boolean validnum = false;
                Boolean validlen = false;

                if (awddesc.Length != 0)
                {
                    valid = PubEntAdminManager.OtherVal(awddesc);
                    validnum = PubEntAdminManager.SpecialVal2(awddesc);
                    validlen = PubEntAdminManager.LenVal(awddesc, 150);
                }

                if ((valid == false) && (validnum == false) && (validlen == true))
                {
                    AddLookup(awddesc, txtAwdName.Text, txtAwdCategory.Text, txtAwdYear.Text);
                }
                else
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }
        }

        protected void AddLookup(string awddesc, string awdname, string awdcategory, string awdyear)
        {
            
            string dbAwdname;
            string dbAwdCate;
            string dbAwdYear;
           

            DataSet ds = new DataSet();
            ds = LU_DAL.displayAward();

           
            if (ds.Tables[0].Rows.Count == 0)
            {
                LU_DAL.AddAward(awdname, awdyear, awdcategory);
                Response.Redirect("~/LookupMgmt.aspx?sub=award");
            }
            else
            {
                 Boolean iExist=false;

                 for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                 {
                     DataRow dataRow = ds.Tables[0].Rows[i];
                     dbAwdname = (string)dataRow["AWARD_NAME"].ToString().Trim();
                     dbAwdCate = (string)dataRow["AWARD_Category"].ToString().Trim();
                     dbAwdYear = (string)dataRow["AWARD_YEAR"].ToString().Trim();

                     if (awdname.Trim() == dbAwdname && awdcategory.Trim() == dbAwdCate && awdyear.Trim() == dbAwdYear)
                     {
                         iExist = true;
                     }
                 }

                 if (iExist)
                 {
                     string confirm = "The Lookup value already exists.";
                     Message.Text = confirm;
                     Message.Visible = true;
                 }
                 else
                 {
                     LU_DAL.AddAward(awdname.Trim(), awdyear.Trim(), awdcategory.Trim());
                     Response.Redirect("~/LookupMgmt.aspx?sub=award");
                 }

            }
       }
     
           
        
        protected void gvResult_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            AwardCollection dt = ((AwardCollection)this.gvResult.DataSource);

            PubEntAdmin.BLL.Award l_conf = dt[e.Item.ItemIndex];

            int Awdid = l_conf.AwardID;
           

            string Awdname = "";
            //string AwdCategoryYear = "";
            string Awdyear = "";
            string Awdcategory = "";

            

            Awdname = l_conf.AwdName;
            Awdcategory = l_conf.AwdCategory;
            Awdyear = l_conf.AwdYear;

            
            string Activestatus = ((Button)e.Item.Cells[4].Controls[7]).Text;

            if (Activestatus == "Inactive")
            {
                Boolean awdExist = LU_DAL.AwdExist(Awdid);

                if (awdExist == false)
                {
                    LU_DAL.DeleteAwardLU(Awdid);
                    Response.Redirect("~/LookupMgmt.aspx?sub=award");
                }
                else if (awdExist == true)
                {
                    string confirm = "Unable to Inactivate, value associated with Publication.";
                    ((Label)e.Item.Cells[5].Controls[1]).Text = confirm;
                }
            }
            if (Activestatus == "Active")
            {
                int Active = 1;
                LU_DAL.UpdateAwardLU(Awdid, Awdname.Trim(), Awdyear.Trim(), Awdcategory.Trim(), Active);
                Response.Redirect("~/LookupMgmt.aspx?sub=award");
            }
        }

        #region Properties

        protected string ParentSpellCkrID1
        {
            get
            {
                return ((LookupMgmt)this.Page).ParentSpellCkr1;
            }
        }

        protected string ParentSpellCkrID2
        {
            get
            {
                return ((LookupMgmt)this.Page).ParentSpellCkr2;
            }
        }

        #endregion

        #region Methods

        public string GetSpellCheckParticipants()
        {

            string participant = this.txtAwdName.ClientID;

            if (this.gvResult.EditItemIndex != -1)
                participant += "," + ((TextBox)this.gvResult.Items[this.gvResult.EditItemIndex].Cells[1].FindControl("txtAwdName")).ClientID;

            return participant;
   
        }
        #endregion

        }
    }
