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
    public partial class Owner : System.Web.UI.UserControl
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
              OwnerCollection dt = ((OwnerCollection)this.gvResult.DataSource);

              PubEntAdmin.BLL.Owner l_owner= dt[e.Item.DataSetIndex];
              int OwnerID = l_owner.OwnerID;              

              string lname = ((TextBox)e.Item.Cells[1].Controls[1]).Text.Trim();
              string fname = ((TextBox)e.Item.Cells[2].Controls[1]).Text.Trim();
              string minitial = ((TextBox)e.Item.Cells[3].Controls[1]).Text;


              bool valid = false;
              bool validlen = false;

              if (lname.Length != 0)
              {
                  valid = PubEntAdminManager.OtherVal(lname);
                  validlen = PubEntAdminManager.LenVal(lname, 20);
              }


              if (fname.Length != 0)
              {
                  valid = PubEntAdminManager.OtherVal(fname);
                  validlen = PubEntAdminManager.LenVal(fname, 20);
              }

              if (minitial.Length != 0)
              {
                  valid = PubEntAdminManager.OtherVal(minitial);
                  validlen = PubEntAdminManager.LenVal(minitial, 1);
                  minitial = minitial.Trim();
              }

              if ((valid == false) && (validlen == true))
              {

                  OwnerCollection coll = LU_DAL.GetOwnerByFullname(fname,lname,minitial);
                  if (coll.Count > 0)
                  {
                      string confirm = "The Owner already existed.";
                      ((Label)e.Item.Cells[8].Controls[1]).Text = confirm;
                  }
                  else
                  {
                      LU_DAL.UpdateOwnerLU(OwnerID,fname,lname,minitial);
                      this.gvResult.EditItemIndex = -1;
                      //this.BindDataAferUpdate();
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
              OwnerCollection dt = ((OwnerCollection)this.gvResult.DataSource);

              PubEntAdmin.BLL.Owner l_owner = dt[e.Item.DataSetIndex];
              int Ownerid = l_owner.OwnerID;
              string Fname = l_owner.FirstName;
              string Lname = l_owner.LastName;
              string Minitial = l_owner.MiddleInitial;

              string Activestatus = ((Button)e.Item.Cells[7].Controls[7]).Text;

              if (Activestatus == "Inactive")
              {
                  Boolean ownerExist = LU_DAL.OwnerExist(Ownerid);

                  if (ownerExist == false)
                  {
                      LU_DAL.DeleteOwnerLU(Ownerid);
                      this.BindData();
                  }
                  else if (ownerExist == true)
                  {
                      string confirm = "Unable to Inactivate, value associated with Publication.";
                      ((Label)e.Item.Cells[8].Controls[1]).Text = confirm;

                  }
              }
              if (Activestatus == "Active")
              {                 
                  LU_DAL.UpdateOwnerLU(Ownerid,Fname,Lname,Minitial);
                  this.BindData();
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

          protected void gvResult_ItemDataBound(object sender, DataGridItemEventArgs e)
          {
              OwnerCollection dt = ((OwnerCollection)this.gvResult.DataSource);

              if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
              {
                  PubEntAdmin.BLL.Owner l_owner = dt[e.Item.DataSetIndex];

                  //delete btn col
                  Button l_able = e.Item.Cells[7].FindControl("lnkbtnDel") as Button;

                  if (l_owner.Checked)
                  {
                      ((Label)e.Item.Cells[5].Controls[1]).Text = "Active";
                      l_able.Text = "Inactivate";
                      Panel l_pnl = e.Item.Cells[7].FindControl("pnlConfirmDel") as Panel;
                      ((Label)l_pnl.Controls[1]).Text = "Are you sure you want to inactivate this Lookup Value [" + Server.HtmlEncode(l_owner.LastName+','+l_owner.FirstName) + "]?";

                  }
                  else
                  {
                      ((Label)e.Item.Cells[5].Controls[1]).Text = "Inactive";
                      l_able.Text = "Activate";
                      Panel l_pnl = e.Item.Cells[7].FindControl("pnlConfirmDel") as Panel;
                      ((Label)l_pnl.Controls[1]).Text = "Are you sure you want to activate this Lookup Value [" + Server.HtmlEncode(l_owner.LastName + ',' + l_owner.FirstName) + "]?";

                      ((Button)e.Item.Cells[6].Controls[0]).Enabled = false;

                  }


              }
              else if (e.Item.ItemType == ListItemType.EditItem)
              {
                  PubEntAdmin.BLL.Owner l_owner = dt[e.Item.ItemIndex];
                  String status = "";
                  if (l_owner.Checked == true)
                  { status = "Active"; }
                  else
                  { status = "Inactive"; }
                  ((Label)e.Item.Cells[5].Controls[1]).Text = status;

                  if (e.Item.Cells[6].Controls[2] is Button)
                  {
                      Button l_btnCancel = ((Button)e.Item.Cells[6].Controls[2]);
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

                      e.Item.Cells[6].Controls.Add(l_panel);
                      e.Item.Cells[6].Controls.Add(l_mpe);
                      e.Item.Cells[6].Controls.Add(l_cbe);
                  }

                  //delete btn col
                  Button l_able = e.Item.Cells[7].FindControl("lnkbtnDel") as Button;

                  if (l_owner.Checked)
                  {
                      ((Label)e.Item.Cells[5].Controls[1]).Text = "Active";
                      l_able.Text = "Inactivate";
                      Panel l_pnl = e.Item.Cells[6].FindControl("pnlConfirmDel") as Panel;
                      ((Label)l_pnl.Controls[1]).Text = "Are you sure you want to inactivate this Lookup Value [" + Server.HtmlEncode(l_owner.LastName + ',' + l_owner.FirstName) + "]?";
                  }
                  else
                  {
                      ((Label)e.Item.Cells[5].Controls[1]).Text = "Inactive";
                      l_able.Text = "Activate";
                      Panel l_pnl = e.Item.Cells[7].FindControl("pnlConfirmDel") as Panel;
                      ((Label)l_pnl.Controls[1]).Text = "Are you sure you want to activate this Lookup Value [" + Server.HtmlEncode(l_owner.LastName + ',' + l_owner.FirstName) + "]?";
                  }
              }
          }

          protected void gvResult_PageindexChange(object sender, DataGridPageChangedEventArgs e)
          {
              gvResult.CurrentPageIndex = e.NewPageIndex;
              this.BindData();
          }

          protected void btn_Find_Click(object sender, EventArgs e)
          {
              this.lblSearchKeywordOwerids.Text = "";
              if (this.txtFind.Text.Trim() != "")
              {
                  this.lblSearchKeywordOwerids.Text = LU_DAL.GetOwnerIDsbyKeyword(this.txtFind.Text);
                  if (this.lblSearchKeywordOwerids.Text == "")
                  {
                      this.pnlResult.Visible = false;
                      this.lblMessage.Text = PubEntAdminManager.strNoSearchResults;
                  }
                  else
                  {

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
              this.lblErFname.Text = "";
              this.lblErLname.Text = "";

              if (this.txtFName.Text == string.Empty)
              {
                  string confirm = "Please enter required value.";
                  this.lblErFname.Text = confirm;
                  blnIsempty = true;
              }

              if (this.txtLName.Text == string.Empty)
              {
                  string confirm = "Please enter required value.";
                  this.lblErLname.Text = confirm;
                  blnIsempty = true;
              }

              bool valid = false;
              //bool validnum = false;
              bool validlen = false;

              if (txtFName.Text.Length != 0)
              {
                  string fname = txtFName.Text;
                  valid = PubEntAdminManager.OtherVal(fname);
                  validlen = PubEntAdminManager.LenVal(fname, 20);
              }


              if (txtLName.Text.Length != 0)
              {
                  string lname = txtLName.Text;
                  valid = PubEntAdminManager.OtherVal(lname);
                  validlen = PubEntAdminManager.LenVal(lname, 20);
              }

              if (this.txtMInit.Text.Length != 0)
              {
                  string minitial = txtMInit.Text;
                  valid = PubEntAdminManager.OtherVal(minitial);
                  validlen = PubEntAdminManager.LenVal(minitial, 1);
              }

              if (!blnIsempty)
              {
                  if ((valid == false) && (validlen == true))
                  {
                      AddLookup();

                  }
                  else
                  {
                      Response.Redirect("InvalidInput.aspx");
                  }
              }

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

        protected void BindData()
        {
            if (lblSearchKeywordOwerids.Text == null || lblSearchKeywordOwerids.Text == "")
            {
                this.gvResult.DataSource = LU_DAL.GetAllLuOwners(false);
              
            }
            else
            {
               
               //this.gvResult.DataSource = LU_DAL.GetOwnersbyKeyword(lblSearchPubids.Text.Trim());
                this.gvResult.DataSource = LU_DAL.GetOwnersbyOwneridbyKeyword(lblSearchKeywordOwerids.Text.Trim());
              
            }
            this.gvResult.DataKeyField = "OwnerID";
            this.gvResult.DataBind();

        }

      
        protected void AddLookup()
        {

            string fname = this.txtFName.Text.Trim();
            string lname = this.txtLName.Text.Trim();
            string minitial=this.txtMInit.Text.Trim();

            this.gvResult.DataSource = LU_DAL.GetOwnerByFullname(fname,lname,minitial);
            this.gvResult.DataKeyField = "OwnerID";
            this.gvResult.DataBind();

            if (gvResult.Items.Count > 0)
            {
                pnlResult.Visible = true;
                this.lblMessage.Text = "The Owner already existed. Please check below for duplicate record(s).";
            }

            else
            {
                int iOwnerid = LU_DAL.AddOwner(fname,lname,minitial);
                if (iOwnerid > 0)
                {
                    this.lblMessage.Text = "The Owner has been saved.";
                    this.txtFName.Text = "";
                    this.txtLName.Text = "";
                    this.txtMInit.Text = "";
                    this.pnlResult.Visible = false;
                    this.txtFind.Text = "";


                }
                else
                {
                    this.lblMessage.Text = "The Owner has not been saved.";
                }
            }

        }



        public string GetSpellCheckParticipants()
        {

            string participant = this.txtFind.ClientID;

            //if (this.gvResult.EditItemIndex != -1)
            //    participant += "," + ((TextBox)this.gvResult.Items[this.gvResult.EditItemIndex].Cells[1].FindControl("txtAwdName")).ClientID;

            return participant;
            
        }

        protected void reset()
        {
            this.txtFName.Text = "";
            this.txtLName.Text = "";
            this.txtMInit.Text = "";
            this.lblMessage.Text = "";
            this.txtFind.Text = "";
            this.lblErFname.Text = "";
            this.lblErLname.Text = "";
            //this.pnlResult.Visible = false;
        }
       
        #endregion
    }
}