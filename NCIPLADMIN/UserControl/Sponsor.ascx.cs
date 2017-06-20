using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using PubEntAdmin.DAL;
using PubEntAdmin.BLL;
using System.Web.UI.HtmlControls;
using GlobalUtils;

using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace PubEntAdmin.UserControl
{
    public partial class Sponsor : System.Web.UI.UserControl
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {         
           this.BindData(); 
        }

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

            if (e.CommandSource.GetType() == typeof(Button))
            {
                string vInactive = ((System.Web.UI.WebControls.Button)(e.CommandSource)).Text;
                if ((vInactive == "Inactive") || (vInactive == "Active"))
                    gvResult_DeleteCommand(source, e);              
            }            
            
        }

        protected void gvResult_UpdateCommand(object source, DataGridCommandEventArgs e)
        {           
            SponsorCollection dt = ((SponsorCollection)this.gvResult.DataSource);

            PubEntAdmin.BLL.Sponsor l_spon = dt[e.Item.DataSetIndex];
            int Sponid = l_spon.SponsorID;
           
            //string SpDesc12 = ((TextBox)e.Item.Cells[1].Controls[1]).Text;
           
            string SpCode = ((TextBox)e.Item.Cells[1].Controls[1]).Text;
            string SpLongDesc = ((TextBox)e.Item.Cells[2].Controls[1]).Text;

          
            bool valid = false;            
            bool validlen = false;

            if (SpLongDesc.Length != 0)
            {
                string longdesc = txtSpLongDesc.Text;
                valid = PubEntAdminManager.OtherVal(longdesc);
                validlen = PubEntAdminManager.LenVal(longdesc, 101);
            }    
            
          
            if (SpCode.Length != 0)
            {
                string code = txtSpCode.Text;
                valid = PubEntAdminManager.OtherVal(code);
                validlen = PubEntAdminManager.LenVal(code, 7);
            }

            if ((valid == false) && (validlen == true))
            {

                SponsorCollection coll = LU_DAL.GetSponsorsbyDesc(SpLongDesc);
                if (coll.Count > 0)
                {
                    string confirm = "The Sponsor already existed.";
                    ((Label)e.Item.Cells[7].Controls[1]).Text = confirm;
                }
                else
                {
                    LU_DAL.UpdateSponsorLU(Sponid, SpCode, SpLongDesc, 1);
                    this.gvResult.EditItemIndex = -1;
                    this.BindData();
                }
            }
            else
            {
                Response.Redirect("InvalidInput.aspx");
            }           
            
            
        }

        protected void gvResult_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            SponsorCollection dt = ((SponsorCollection)this.gvResult.DataSource);

            PubEntAdmin.BLL.Sponsor l_spon = dt[e.Item.DataSetIndex];
            int Sponid = l_spon.SponsorID;
            string SpLongDesc = l_spon.LongDescription;
            //string SpDesc = l_spon.Description;            
            string SpCode = l_spon.SponsorCode;
            
            string Activestatus = ((Button)e.Item.Cells[6].Controls[7]).Text;

            if (Activestatus == "Inactive")
            {
                Boolean sponExist = LU_DAL.SponsorExist(Sponid);

                if (sponExist == false)
                {
                    LU_DAL.DeleteSponsorLU(Sponid);
                    this.BindData(); 
                }
                else if (sponExist == true)
                {
                    string confirm = "Unable to Inactivate, value associated with Publication.";
                    ((Label)e.Item.Cells[7].Controls[1]).Text = confirm;
                   
                }
            }
            if (Activestatus == "Active")
            {
                int Active = 1;
                LU_DAL.UpdateSponsorLU(Sponid, SpCode,SpLongDesc, Active);
                this.BindData();
            } 
            
        }

        protected void gvResult_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            SponsorCollection dt = ((SponsorCollection)this.gvResult.DataSource);            
            
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                PubEntAdmin.BLL.Sponsor l_spon = dt[e.Item.DataSetIndex]; 

                //delete btn col
                Button l_able = e.Item.Cells[6].FindControl("lnkbtnDel") as Button;               

                if (l_spon.Checked)
                {
                   ((Label)e.Item.Cells[4].Controls[1]).Text = "Active";
                    l_able.Text = "Inactivate";
                    Panel l_pnl = e.Item.Cells[6].FindControl("pnlConfirmDel") as Panel;
                    ((Label)l_pnl.Controls[1]).Text = "Are you sure you want to inactivate this Lookup Value [" +  Server.HtmlEncode(l_spon.SponsorCode) + "]?";
                    
                }
                else
                {
                    ((Label)e.Item.Cells[4].Controls[1]).Text = "Inactive";
                    l_able.Text = "Activate";
                    Panel l_pnl = e.Item.Cells[6].FindControl("pnlConfirmDel") as Panel;
                    ((Label)l_pnl.Controls[1]).Text = "Are you sure you want to activate this Lookup Value [" + Server.HtmlEncode(l_spon.SponsorCode) + "]?";
                   
                    ((Button)e.Item.Cells[5].Controls[0]).Enabled = false;

                }

               
            }
            else if (e.Item.ItemType == ListItemType.EditItem)
            {
                PubEntAdmin.BLL.Sponsor l_spon = dt[e.Item.ItemIndex];
                String status = "";
                if (l_spon.Checked == true)
                { status = "Active"; }
                else
                { status = "Inactive"; }
                ((Label)e.Item.Cells[4].Controls[1]).Text = status;

                if (e.Item.Cells[5].Controls[2] is Button)
                {
                    Button l_btnCancel = ((Button)e.Item.Cells[5].Controls[2]);
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

                    e.Item.Cells[5].Controls.Add(l_panel);
                    e.Item.Cells[5].Controls.Add(l_mpe);
                    e.Item.Cells[5].Controls.Add(l_cbe);
                }

                //delete btn col
                Button l_able = e.Item.Cells[6].FindControl("lnkbtnDel") as Button;

                if (l_spon.Checked)
                {
                   ((Label)e.Item.Cells[4].Controls[1]).Text = "Active";
                    l_able.Text = "Inactivate";
                    Panel l_pnl = e.Item.Cells[5].FindControl("pnlConfirmDel") as Panel;
                    ((Label)l_pnl.Controls[1]).Text = "Are you sure you want to inactivate this Lookup Value [" + Server.HtmlEncode(l_spon.SponsorCode) + "]?";
                }
                else
                {
                    ((Label)e.Item.Cells[4].Controls[1]).Text = "Inactive";
                    l_able.Text = "Activate";
                    Panel l_pnl = e.Item.Cells[6].FindControl("pnlConfirmDel") as Panel;
                    ((Label)l_pnl.Controls[1]).Text = "Are you sure you want to activate this Lookup Value [" + Server.HtmlEncode(l_spon.SponsorCode) + "]?";
                }
            }
        }

        protected void gvResult_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Pager)
            {
                if (e.Item.Cells.Count > 0)
                {
                    int columnSpan = e.Item.Cells[0].ColumnSpan;
                    e.Item.Cells[0].Attributes.Add("colspan", columnSpan.ToString());
                }
            }
        }



        //protected void gvResult_ItemCreate(object sender, DataGridItemEventArgs e)
        //{
        //    if (e.Item.ItemType == ListItemType.Pager)
        //    {
        //        int iTotalcount = e.Item.Cells[0].Controls.Count;
        //        if (iTotalcount > 1)
        //        {
        //            LinkButton btnFirst = new LinkButton();
        //            LinkButton btnLast = new LinkButton();

        //            btnFirst.Text = "First ";
        //            btnLast.Text = " Last";

        //            if (e.Item.Cells[0].Controls[iTotalcount-1].GetType() != typeof(System.Web.UI.WebControls.Label))
        //            {                        
        //                btnLast.CommandArgument = iTotalcount.ToString();
        //                btnLast.CommandName = "Page";
        //                btnLast.Enabled = true;
        //            }
        //            else
        //            {
        //                btnLast.Enabled = false;
        //            }

        //            if (e.Item.Cells[0].Controls[0].GetType() != typeof(System.Web.UI.WebControls.Label))
        //            {
        //                btnFirst.CommandArgument = "1";
        //                btnLast.CommandName = "Page";
        //                btnFirst.Enabled = true;
        //            }
        //            else
        //            {
        //                btnFirst.Enabled = false;
        //            }

        //            e.Item.Cells[0].Controls.AddAt(0, btnFirst);
        //            e.Item.Cells[0].Controls.AddAt(e.Item.Cells[0].Controls.Count, btnLast);
        //        }
               
        //    }
         
        //}

        protected void gvResult_PageindexChange(object sender, DataGridPageChangedEventArgs e)
        {            
            gvResult.CurrentPageIndex = e.NewPageIndex;
            this.BindData();
        }

        protected void btn_Find_Click(object sender, EventArgs e)
        {
            this.lblSearchSpids.Text = "";
            this.lblSearchASpids.Text = "";

            if (this.txtFind.Text.Trim() != "")
            {
                this.lblSearchSpids.Text = LU_DAL.GetSponsorIDsbyKeyword(this.txtFind.Text,false);
                if (this.lblSearchSpids.Text == "")
                {
                    this.pnlResult.Visible = false;
                    this.lblMessage.Text = PubEntAdminManager.strNoSearchResults;
                }
                else
                {
                    this.lblSearchASpids.Text = LU_DAL.GetSponsorIDsbyKeyword(this.txtFind.Text, true);
                    gvResult.CurrentPageIndex = 0;
                    this.BindData();
                    this.pnlResult.Visible = true;
                    this.gvResult.EditItemIndex = -1;
                    reset();
                }
            }


            if (this.txtFind.Text.Trim() == "")
            {
                gvResult.CurrentPageIndex = 0;
                this.BindData();
                this.pnlResult.Visible = true;
                this.gvResult.EditItemIndex = -1;
                reset();
            }

            
            
            
            
            //lblSearchPubids.Text = this.txtFind.Text;
            //gvResult.CurrentPageIndex = 0;            
            //this.BindData();

            //if (gvResult.Items.Count == 0)
            //{
            //    this.pnlResult.Visible = false;
            //    this.lblMessage.Text = PubEntAdminManager.strNoSearchResults;
            //}
            //else
            //{
            //    this.pnlResult.Visible = true;
            //    this.gvResult.EditItemIndex = -1;
            //    reset();
            //}
            
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            bool blnIsempty = false;
            lblErLongDesc.Text = "";
            lblErSpCode.Text = "";
           if (this.txtSpLongDesc.Text == string.Empty )
            {
                string confirm = "Please enter required value.";
                this.lblErLongDesc.Text = confirm;
                blnIsempty = true;
            }

            if (this.txtSpCode.Text == string.Empty)
            {
                string confirm = "Please enter required value.";
                this.lblErSpCode.Text = confirm;
                blnIsempty = true;
            }

            bool valid = false;
            //bool validnum = false;
            bool validlen = false;

            if (txtSpLongDesc.Text.Length != 0)
            {
                string longdesc = txtSpLongDesc.Text;
                valid = PubEntAdminManager.OtherVal(longdesc);
                validlen = PubEntAdminManager.LenVal(longdesc, 101);
            }

            
            if (txtSpCode.Text.Length != 0)
            {
                string code = txtSpCode.Text;
                valid = PubEntAdminManager.OtherVal(code);               
                validlen = PubEntAdminManager.LenVal(code, 7);
            }

            if (!blnIsempty)
            {
                if ((valid == false) && (validlen == true))
                {                                      
                    AddLookup(sender);                   

                }
                else
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }           

        }

        protected void ImgBtnExportSchRsltToExcel_OnClick(object sender, ImageClickEventArgs e)
        {           
            gvExcelRpt.Visible = true; 
            ExportRoutines.ExportToExcel(this.Page, "Master_Sponsor_Code_List", "<strong>" + PubEntAdminManager.DefAdminSponsorTitle + "</strong>" + " - " + DateTime.Now.ToShortDateString(), this.gvExcelRpt);            
            gvExcelRpt.Visible = false;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            reset();
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

        protected void AddLookup(object sender)
        {

            string longdesc = this.txtSpLongDesc.Text;           
            string code = this.txtSpCode.Text;

            this.gvResult.DataSource = LU_DAL.GetSponsorsbyDesc(longdesc);
            this.gvResult.DataKeyField = "SponsorID";
            this.gvResult.DataBind();

            if (gvResult.Items.Count > 0)
            {
                pnlResult.Visible = true;
                this.lblMessage.Text = "The Sponsor already existed. Please check below for duplicate record(s).";
            }

            else
            {
                int iSponsorid = LU_DAL.AddSponsor(longdesc, code);
                if (iSponsorid > 0)
                {
                    this.lblMessage.Text = "The Sponsor has been saved.";
                    this.txtSpCode.Text = "";
                    this.txtSpLongDesc.Text = "";
                    this.pnlResult.Visible = false;
                    this.txtFind.Text = "";                  
                    
                                     
                }
                else
                {
                    this.lblMessage.Text = "The Sponsor has not been saved.";
                }
            }

        }


        protected void BindData()
        {
            if (lblSearchSpids.Text == null || lblSearchSpids.Text == "")
            {
                this.gvResult.DataSource = LU_DAL.GetAllLuSponsors(false);
                gvExcelRpt.DataSource = LU_DAL.GetAllLuSponsors(true);
            }
            else
            {
                this.gvResult.DataSource = LU_DAL.GetSponsorsbySponsoridbyKeyword(lblSearchSpids.Text.Trim(), false);
                gvExcelRpt.DataSource = LU_DAL.GetSponsorsbySponsoridbyKeyword(lblSearchASpids.Text.Trim(), true);
            }
            this.gvResult.DataKeyField = "SponsorID";
            this.gvResult.DataBind();
            gvExcelRpt.DataBind();  
           
        }       

        public string GetSpellCheckParticipants()
        {

            string participant = this.txtFind.ClientID;

            return participant;

        }

        protected void reset()
        {
            this.txtSpCode.Text = "";
            this.txtSpLongDesc.Text = "";                    
            this.lblMessage.Text = "";
            this.lblErLongDesc.Text = "";
            this.lblErSpCode.Text = "";
            //this.pnlResult.Visible = false;
        }
       
        #endregion
        
    }
}