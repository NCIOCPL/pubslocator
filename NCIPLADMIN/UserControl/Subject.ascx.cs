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
using System.Collections.Generic;
using System.Text.RegularExpressions;
using AjaxControlToolkit;
using PubEntAdmin.DAL;
using PubEntAdmin.BLL;

namespace PubEntAdmin.UserControl
{
    public partial class Subject : System.Web.UI.UserControl
    {
        #region Events Handling
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                this.BindData();

            this.SecVal();
        }

        protected void Page_PreRender(object sender, System.EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                "Subject_ClientScript",
            @"
                function LkupParticipants()
                {
                    return '" + this.GetSpellCheckParticipants() + @"';
                }
            ", true);
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            bool blnSubjSave = false;
            int l_subjectid = 0;

            this.SecVal();

            if (txtSubjName.Text.Trim().Length > 0)
                //&& this.LiveIntSelSubj.SelectedAny)
            {
                Regex r = new Regex(@"\s+");
                blnSubjSave = PE_DAL.SetSubject(ref l_subjectid, r.Replace(this.txtSubjName.Text.Trim(), " "),
                    System.Convert.ToInt32(this.LiveIntSelSubj.InNCIPL),
                    System.Convert.ToInt32(this.LiveIntSelSubj.InROO),
                    System.Convert.ToInt32(this.LiveIntSelSubj.InExh),
                    System.Convert.ToInt32(this.LiveIntSelSubj.InCatalog),
                    System.Convert.ToInt32(this.chboxSubj.Checked));
            }

            if (blnSubjSave)
            {
                this.txtSubjName.Text = "";
                this.chboxSubj.Checked = false;
                this.LiveIntSelSubj.clearAll();
                this.BindData();
                this.lblMsg.Text = "";
            }
            else
            {
                if (l_subjectid == 0)
                {
                    this.lblMsg.Text = "The subject already existed.";
                }
                else if (l_subjectid == -1)
                {
                    this.lblMsg.Text = "Error occurs.";
                }
            }
        }

        protected void gvResult_ItemCreated(object source, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.Item.Cells[6].Controls[0] is Button)
                {
                    Button l_lnkbtnEdit = e.Item.Cells[6].Controls[0] as Button;
                    l_lnkbtnEdit.ID = "gvResult_Edit";
                }

            }
            else if ((e.Item.ItemType == ListItemType.EditItem))
            {
                if (e.Item.Cells[4].Controls[1] is CheckBox)
                {
                    CheckBox l_chkboxRemove = ((CheckBox)e.Item.FindControl("ckboxHasSubCat"));
                    l_chkboxRemove.Attributes.Add("onclick", @"(!this.checked)?alert('NOTE:\nALL the SubCategories and SubSubCategories assigned\nto this Category will be deleted!'):''");
                }

                if (e.Item.Cells[6].Controls[0] is Button)
                {
                    Button l_btnUpdate = ((Button)e.Item.Cells[6].Controls[0]);
                    l_btnUpdate.ID = "gvResult_Update";
                }

                if (e.Item.Cells[6].Controls[2] is Button)
                {
                    Button l_btnCancel = ((Button)e.Item.Cells[6].Controls[2]);
                    l_btnCancel.ID = "gvResult_Cancel";
                }
            }
        }

        protected void gvResult_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            this.gvResult.EditItemIndex = -1;
            Session.Remove("SubjectEditItemIndex");
            this.BindData();
        }

        protected void gvResult_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.gvResult.EditItemIndex = e.Item.ItemIndex;
            Session["SubjectEditItemIndex"] = e.Item.ItemIndex;
            this.BindData();
        }

        protected void gvResult_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandSource is Button)
            {
                bool blnSubjSave = false;

                Button l_lnkbtn_del = e.CommandSource as Button;
                if (l_lnkbtn_del.Text.ToLower() == "inactive")
                {
                    if (e.Item.Cells[0].Text == "True")
                    {
                        CustomValidator lcv = ((CustomValidator)e.Item.Cells[7].FindControl("CustValDisable"));
                        lcv.IsValid = false;
                    }
                    else
                    {
                        blnSubjSave = PE_DAL.DeleteSubject((int)gvResult.DataKeys[e.Item.ItemIndex]);
                        this.gvResult.EditItemIndex = -1;
                        Session.Remove("SubjectEditItemIndex");
                    }
                }
                else if (l_lnkbtn_del.Text.ToLower() == "active")
                {
                    blnSubjSave = PE_DAL.EnableSubject((int)gvResult.DataKeys[e.Item.ItemIndex]);
                    this.gvResult.EditItemIndex = -1;
                    Session.Remove("SubjectEditItemIndex");
                }

                if (blnSubjSave)
                    this.BindData();
            }
        }

        protected void gvResult_SortCommand(object source, DataGridSortCommandEventArgs e)
        {

        }

        protected void gvResult_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            int Id = (int)gvResult.DataKeys[e.Item.ItemIndex];

            LiveIntSel l_LiveIntSel = ((LiveIntSel)((System.Web.UI.WebControls.TableRow)(e.Item)).
                Cells[3].FindControl("LiveIntSel2"));

            ///-----------------------------------------------
            if (!PubEntAdminManager.LenVal(((TextBox)((System.Web.UI.WebControls.TableRow)(e.Item)).
                Cells[2].FindControl("txtSubjName")).Text.Trim(), 50))
            {
                Response.Redirect("InvalidInput.aspx");
            }

            if (PubEntAdminManager.OtherVal(((TextBox)((System.Web.UI.WebControls.TableRow)(e.Item)).
                Cells[2].FindControl("txtSubjName")).Text.Trim()))
            {
                Response.Redirect("InvalidInput.aspx");
            }

            foreach (ListItem li in l_LiveIntSel.Items)
            {
                if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            if (PubEntAdminManager.SpecialVal2(((TextBox)((System.Web.UI.WebControls.TableRow)(e.Item)).
                Cells[2].FindControl("txtSubjName")).Text.Replace(" ", "")))
            {
                Response.Redirect("InvalidInput.aspx");
            }

            foreach (ListItem li in l_LiveIntSel.Items)
            {
                if (PubEntAdminManager.SpecialVal2(li.Text.Replace(" ", "")) ||
                    PubEntAdminManager.SpecialVal2(li.Value.Replace(" ", "")))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }
            ///-----------------------------------------------
            
            bool blnSubjSave =
                PE_DAL.SetSubject(ref Id,
                    ((TextBox)((System.Web.UI.WebControls.TableRow)(e.Item)).Cells[2].FindControl("txtSubjName")).Text.Trim(),
                System.Convert.ToInt32(l_LiveIntSel.InNCIPL),
                System.Convert.ToInt32(l_LiveIntSel.InROO),
                System.Convert.ToInt32(l_LiveIntSel.InExh),
                System.Convert.ToInt32(l_LiveIntSel.InCatalog),
                System.Convert.ToInt32(((CheckBox)((System.Web.UI.WebControls.TableRow)(e.Item)).Cells[4].FindControl("ckboxHasSubCat")).Checked));

            if (blnSubjSave)
            {
                this.gvResult.EditItemIndex = -1;
                Session.Remove("SubjectEditItemIndex");
                this.BindData();
            }
        }

        protected void gvResult_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            SubjectCollection dt = ((SubjectCollection)this.gvResult.DataSource);
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                PubEntAdmin.BLL.Subject l_conf = dt[e.Item.ItemIndex];

                e.Item.Cells[0].Text = l_conf.CannotRem.ToString();
                
                ((Label)e.Item.Cells[2].Controls[1]).Text = Server.HtmlEncode(l_conf.SubjName);

                //----------------------------------------
                string a = "";

                if (l_conf.InNCIPL)
                    a = "NCIPL";
                if (l_conf.InROO)
                {
                    if (a.Length != 0)
                        a += "<br />";
                    a+= "NCIPLcc";
                }
                if (l_conf.InExh)
                {
                    if (a.Length != 0)
                        a += "<br />";
                    a+= "Exhibit";
                }
                if (l_conf.InCatalog)
                {
                    if (a.Length != 0)
                        a += "<br />";
                    a+= "Catalog";
                }

                if (a.Length != 0)
                    ((Label)e.Item.Cells[3].Controls[1]).Text = a;
                else
                    ((Label)e.Item.Cells[3].Controls[1]).Text = "N/A";

                //----------------------------------------
                ((Label)e.Item.Cells[4].Controls[1]).Text = l_conf.HasSubCat.ToString();
                //----------------------------------------
                ((Label)e.Item.FindControl("lblStatus1")).Text = l_conf.Active ? "Active" : "Inactive";
                //----------------------------------------
                //delete btn col
                Button l_able = e.Item.Cells[7].FindControl("lnkbtnDel") as Button;

                if (l_conf.Active)
                {
                    l_able.Text = "Inactivate";
                    Panel l_pnl = e.Item.Cells[7].FindControl("pnlConfirmDel") as Panel;
                    ((Label)l_pnl.Controls[1]).Text = "Are you sure you want to inactivate this Subject [" + Server.HtmlEncode(l_conf.SubjName) + "]?<br />NOTE: If the Category has SubCategories they will also be inactivated!";
                }
                else
                {
                    l_able.Text = "Activate";
                    Panel l_pnl = e.Item.Cells[7].FindControl("pnlConfirmDel") as Panel;
                    ((Label)l_pnl.Controls[1]).Text = "Are you sure you want to activate this Subject [" + Server.HtmlEncode(l_conf.SubjName) + "]?";
                }
            }
            else if (e.Item.ItemType == ListItemType.EditItem)
            {
                PubEntAdmin.BLL.Subject l_conf = dt[e.Item.ItemIndex];

                e.Item.Cells[0].Text = l_conf.CannotRem.ToString();

                ((TextBox)e.Item.Cells[2].Controls[1]).Text = Server.HtmlEncode(l_conf.SubjName);
                //----------------------------------------

                if (l_conf.InNCIPL)
                {
                    ((LiveIntSel)e.Item.Cells[3].Controls[1]).InNCIPL = true;
                }
                if (l_conf.InROO)
                {
                    ((LiveIntSel)e.Item.Cells[3].Controls[1]).InROO = true;
                }
                if (l_conf.InExh)
                {
                    ((LiveIntSel)e.Item.Cells[3].Controls[1]).InExh = true;
                }
                if (l_conf.InCatalog)
                {
                    ((LiveIntSel)e.Item.Cells[3].Controls[1]).InCatalog = true;
                }

                //----------------------------------------
                if (e.Item.Cells[4].Controls[1] is CheckBox)
                {
                    CheckBox l_chkboxRemove = ((CheckBox)e.Item.FindControl("ckboxHasSubCat"));
                    
                    l_chkboxRemove.Attributes.Add("onclick", @"(!this.checked)?alert('NOTE:\nALL the SubCategories and SubSubCategories assigned\nto this Category will be disabled!'):''");
                    ((CheckBox)e.Item.Cells[4].Controls[1]).Checked = l_conf.HasSubCat;
                }
                //----------------------------------------
                ((Label)e.Item.FindControl("lblStatus2")).Text = l_conf.Active ? "Active" : "Inactive";
                //----------------------------------------

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
                    l_ok.CssClass = "btn";
                    l_cancel.ID = "l_cancel";
                    l_cancel.Text = "Cancel";
                    l_cancel.CssClass = "btn";
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
                //----------------------------------------
                //delete btn col
                Button l_able = e.Item.Cells[7].FindControl("lnkbtnDel") as Button;

                if (l_conf.Active)
                {
                    l_able.Text = "Inactivate";
                    Panel l_pnl = e.Item.Cells[7].FindControl("pnlConfirmDel") as Panel;
                    ((Label)l_pnl.Controls[1]).Text = "Are you sure you want to inactivate this Subject [" + Server.HtmlEncode(l_conf.SubjName) + "]?<br />NOTE: If the Category has SubCategories they will also be inactivated!";
                }
                else
                {
                    l_able.Text = "Activate";
                    Panel l_pnl = e.Item.Cells[7].FindControl("pnlConfirmDel") as Panel;
                    ((Label)l_pnl.Controls[1]).Text = "Are you sure you want to activate this Subject [" + Server.HtmlEncode(l_conf.SubjName) + "]?";


                }
            }
        }
        
        #endregion

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
            this.gvResult.DataSource = PE_DAL.GetAllSubject(false);
            this.gvResult.DataKeyField = "SubjID";
            if (Session["SubjectEditItemIndex"] != null)
            {
                this.gvResult.EditItemIndex = System.Convert.ToInt32(Session["SubjectEditItemIndex"].ToString());
            }
            this.gvResult.DataBind();
        }

        public string GetSpellCheckParticipants()
        {
            //if (System.Convert.ToInt32(Session[PubEntAdminManager.strTabContentLUCurrActTabIndex].ToString()) == 0)
            //    return this.txtSubjName.ClientID;
            //else
            //{
            string participant = this.txtSubjName.ClientID;

                if (this.gvResult.EditItemIndex != -1)
                    participant += ","+((TextBox)this.gvResult.Items[this.gvResult.EditItemIndex].Cells[1].FindControl("txtSubjName")).ClientID;
                
            //}
                return participant;
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
            if ((!PubEntAdminManager.LenVal(this.txtSubjName.Text, 50)))
            {
                Response.Redirect("InvalidInput.aspx");
            }
        }

        private void TagVal()
        {
            if ((PubEntAdminManager.OtherVal(this.txtSubjName.Text)))
            {
                Response.Redirect("InvalidInput.aspx");
            }

            foreach (ListItem li in this.LiveIntSelSubj.Items)
            {
                if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

        }

        private void SpecialVal()
        {

            if ((PubEntAdminManager.SpecialVal2(this.txtSubjName.Text.Replace(" ", ""))) )
            {
                Response.Redirect("InvalidInput.aspx");
            }

            foreach (ListItem li in LiveIntSelSubj.Items)
            {
                if (PubEntAdminManager.SpecialVal2(li.Text.Replace(" ", "")) ||
                    PubEntAdminManager.SpecialVal2(li.Value.Replace(" ", "")))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

        }
        #endregion

        #endregion
    }
}