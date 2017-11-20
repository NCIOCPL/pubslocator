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
using PubEntAdmin.BLL;
using PubEntAdmin.DAL;
using AjaxControlToolkit;

namespace PubEntAdmin.UserControl
{
    public partial class Announcement : System.Web.UI.UserControl
    {
        #region Events Handling
        protected void Page_Load(object sender, EventArgs e)
        {
            List<TabItem> l = new List<TabItem>();
            TabItem t1 = new TabItem(0, "Add New Announcement", "Announcement.aspx");
            TabItem t2 = new TabItem(1, "Edit Existing Announcement", "Announcement.aspx");

            l.Add(t1);
            l.Add(t2);

            this.TabStrip1.TabSource = l;
            this.BindData();
            this.SecVal();

            if (!Page.IsPostBack)
            {
                this.MultiView1.ActiveViewIndex = 1;
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.TabStrip1.SelectTab(1);
            }
        }

        protected void gvResult_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            this.gvResult.EditItemIndex = -1;
            Session.Remove("AnnouncementEditItemIndex");
            this.BindData();
        }

        protected void gvResult_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.gvResult.EditItemIndex = e.Item.ItemIndex;
            Session["AnnouncementEditItemIndex"] = e.Item.ItemIndex;
            this.BindData();
        }

        protected void gvResult_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandSource is Button)
            {
                if (((Button)e.CommandSource).Text == "Delete")
                {
                    int announcmentid = System.Convert.ToInt32(e.Item.Cells[0].Text);
                    string Name, URL, Start, End;

                    try
                    {
                        Name = ((Label)e.Item.Cells[1].FindControl("lblAnnouncementName_gd")).Text;
                    }
                    catch
                    {
                        Name = ((TextBox)e.Item.Cells[1].FindControl("txtAnnouncementName_gd")).Text;
                    }

                    try
                    {
                        URL = ((HyperLink)e.Item.Cells[2].FindControl("hylnkAnnouncementURL_gd")).Text;
                    }
                    catch
                    {
                        URL = ((TextBox)e.Item.Cells[2].FindControl("txtAnnouncementURL_gd")).Text;
                    }

                    try
                    {
                        Start = ((Label)e.Item.Cells[3].FindControl("lblStartDate_gd")).Text;
                    }
                    catch
                    {
                        Start = ((TextBox)e.Item.Cells[3].FindControl("txtStartDate_gd")).Text;
                    }

                    try
                    {
                        End = ((Label)e.Item.Cells[4].FindControl("lblEndDate_gd")).Text;
                    }
                    catch
                    {
                        End = ((TextBox)e.Item.Cells[4].FindControl("txtEndDate_gd")).Text;
                    }

                    bool ret = PE_DAL.SetAnnouncement(ref announcmentid, Name, URL, System.Convert.ToDateTime(Start),
                        System.Convert.ToDateTime(End), 0);

                    if (ret)
                    {
                        this.gvResult.EditItemIndex = -1;
                        this.BindData();
                    }
                }
            }
        }

        protected void gvResult_SortCommand(object source, DataGridSortCommandEventArgs e)
        {

        }

        protected void gvResult_ItemCreated(object source, DataGridItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.EditItem))
            {
                CompareValidator comValDates_gd = new CompareValidator();
                comValDates_gd.ErrorMessage = "<br/>Start Date cannot be greater than End Date.";
                comValDates_gd.ControlToCompare = "txtStartDate_gd";
                comValDates_gd.ControlToValidate = "txtEndDate_gd";
                comValDates_gd.ValidationGroup = "EditVal";
                comValDates_gd.Display = ValidatorDisplay.Dynamic;
                comValDates_gd.Type = ValidationDataType.Date;
                comValDates_gd.Operator = ValidationCompareOperator.GreaterThan;

                if (e.Item.Cells[5].Controls[0] is Button)
                {
                    Button l_btnUpdate = ((Button)e.Item.Cells[5].Controls[0]);
                    l_btnUpdate.ID = "l_btnUpdate";
                    l_btnUpdate.ValidationGroup = "EditVal";
                }
                e.Item.Cells[5].Controls.Add(comValDates_gd);

                if (e.Item.Cells[5].Controls[2] is Button)
                {
                    Button l_btnCancel = ((Button)e.Item.Cells[5].Controls[2]);
                    l_btnCancel.ID = "l_btnCancel";

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
                    e.Item.Cells[5].Controls.Add(l_cbe);
                    e.Item.Cells[5].Controls.Add(l_mpe);

                }
            }
        }

        protected void gvResult_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            
            Page.Validate();
            if (Page.IsValid)
            {
                Boolean validName = false;
                Boolean validnumName = false;
                Boolean validlenName = false;

                Boolean validIRL = false;
                Boolean validnumURL = false;
                Boolean validlenURL = false;
                
                int announcmentid = System.Convert.ToInt32(e.Item.Cells[0].Text);
                string Name = ((TextBox)e.Item.Cells[1].FindControl("txtAnnouncementName_gd")).Text;
                string URL = ((TextBox)e.Item.Cells[2].FindControl("txtAnnouncementURL_gd")).Text;

                string Start = ((TextBox)e.Item.Cells[3].FindControl("txtStartDate_gd")).Text;
                string End = ((TextBox)e.Item.Cells[4].FindControl("txtEndDate_gd")).Text;

                if (Name.Length != 0)
                {
                    validlenName = PubEntAdminManager.LenVal(Name, 5);
                    validName = PubEntAdminManager.OtherVal(Name);
                    validnumName = PubEntAdminManager.SpecialVal2(Name.Replace(" ", ""));
                }

                if (URL.Length != 0)
                {
                    validlenURL = PubEntAdminManager.LenVal(URL, 500);
                    validIRL = PubEntAdminManager.OtherVal(URL);
                    validnumURL = PubEntAdminManager.SpecialVal2(URL.Replace(" ", ""));
                }

                if ((validName == false) && (validnumName == false) && (validlenName == true) &&
                    (validlenURL == false) && (validIRL == false) && (validnumURL == true))
                {

                    bool ret = PE_DAL.SetAnnouncement(ref announcmentid, Name, URL, System.Convert.ToDateTime(Start),
                        System.Convert.ToDateTime(End), 1);

                    if (ret)
                    {
                        this.gvResult.EditItemIndex = -1;
                        Session.Remove("AnnouncementEditItemIndex");
                        this.BindData();
                    }
                }
                else
                    Response.Redirect("InvalidInput.aspx");

            }
        }

        protected void gvResult_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            List<PubEntAdmin.BLL.Announcement> dt = ((List<PubEntAdmin.BLL.Announcement>)this.gvResult.DataSource);
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                PubEntAdmin.BLL.Announcement l_conf = dt[e.Item.ItemIndex];
                ((TableCell)e.Item.Cells[0]).Text = l_conf.AnnouncementID.ToString();
                ((Label)e.Item.Cells[1].Controls[1]).Text = Server.HtmlEncode(l_conf.AnnouncementName);
                ((HyperLink)e.Item.Cells[2].Controls[1]).Text = ((HyperLink)e.Item.Cells[2].Controls[1]).NavigateUrl = Server.HtmlEncode(l_conf.AnnouncementURL);
                ((Label)e.Item.Cells[3].Controls[1]).Text = l_conf.StartDate.ToShortDateString();
                ((Label)e.Item.Cells[4].Controls[1]).Text = l_conf.EndDate.ToShortDateString();

                ////delete btn col
                //Panel l_pnl = e.Item.Cells[6].Controls[1] as Panel;
                //((Label)l_pnl.Controls[1]).Text = "Are you sure you want to delete this Announcement [" + l_conf.AnnouncementName + "]?";
            }
            //else if (e.Item.ItemType == ListItemType.EditItem)
            //{
                //PubEntAdmin.BLL.Announcement l_conf = dt[e.Item.ItemIndex];

                //if (this.gvResult.EditItemIndex != -1)
                //{
                //    ((MaskedEditValidator)e.Item.Cells[3].Controls[7]).Enabled = true;
                //    ((MaskedEditValidator)e.Item.Cells[4].Controls[7]).Enabled = true;
                //}
                //else
                //{
                //    ((MaskedEditValidator)e.Item.Cells[3].Controls[7]).Enabled = false;
                //    ((MaskedEditValidator)e.Item.Cells[4].Controls[7]).Enabled = false;
                //}

                //////delete btn col
                //Panel l_pnl = e.Item.Cells[6].Controls[1] as Panel;
                //((Label)l_pnl.Controls[1]).Text = "Are you sure you want to delete this Announcement [" + l_conf.AnnouncementName + "]?";
            //}
        }

        protected void TabStrip1_SelectionChanged(object sender, TabStrip.SelectionChangedEventArgs e)
        {
            if (e.TabNameSelected == "Add New Announcement")
            {
                this.MultiView1.ActiveViewIndex = 0;
            }
            else
            {
                this.MultiView1.ActiveViewIndex = 1;
                this.BindData();
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {          

            Page.Validate();
            if (Page.IsValid)
            {
                this.SecVal();

                int l_AnnouncementID = 0;
                bool ret = PE_DAL.SetAnnouncement(ref l_AnnouncementID, this.txtAnnouncementName.Text.Trim(),
                    this.txtAnouncementURL.Text.Trim(), System.Convert.ToDateTime(this.txtStartDate.Text.Trim()),
                    System.Convert.ToDateTime(this.txtEndDate.Text.Trim()), 1);

                if (ret)
                {
                    this.txtAnnouncementName.Text = "";
                    this.txtAnouncementURL.Text = "";
                    this.txtStartDate.Text = "";
                    this.txtEndDate.Text = "";
                    this.lblMsg.Text = String.Empty;
                    this.MultiView1.ActiveViewIndex = 1;
                    this.TabStrip1.SelectTab(1);
                    this.BindData();
                }
                else
                {
                    if (l_AnnouncementID == 0)
                    {
                        this.lblMsg.Text = "The announcement already existed.";
                    }

                }

            }
        }

        protected void DateOnServerValidate(object source, ServerValidateEventArgs args)
        {
            Regex r = new Regex(@"^([\d]|1[0,1,2])/([0-9]|[0,1,2][0-9]|3[0,1])/\d{4}$");
            if (args.Value.Trim().Length > 0)
            {
                if (r.IsMatch(args.Value.Trim()))
                {
                    try
                    {
                        DateTime dt = System.Convert.ToDateTime(args.Value.Trim());
                        args.IsValid = true;
                    }
                    catch (FormatException fex)
                    {
                        args.IsValid = false;
                    }
                }
                else
                {
                    args.IsValid = false;
                }
            }
            else
            {
                args.IsValid = false;
            }

        }

        #endregion

        #region Methods

        protected void BindData()
        {
            this.gvResult.DataSource = PE_DAL.GetAnnouncement(true);
            if (Session["AnnouncementEditItemIndex"] != null)
            {
                this.gvResult.EditItemIndex = System.Convert.ToInt32(Session["AnnouncementEditItemIndex"].ToString());
            }
            this.gvResult.DataBind();
            
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
            if ((!PubEntAdminManager.LenVal(this.txtAnnouncementName.Text, 500)) ||
                (!PubEntAdminManager.LenVal(this.txtAnouncementURL.Text, 500)))
            {
                Response.Redirect("InvalidInput.aspx");
            }
        }

        private void TagVal()
        {
            if ((PubEntAdminManager.OtherVal(this.txtAnnouncementName.Text)) ||
                (PubEntAdminManager.OtherVal(this.txtAnouncementURL.Text)))
            {
                Response.Redirect("InvalidInput.aspx");
            }
        }

        private void SpecialVal()
        {
            if ((PubEntAdminManager.SpecialVal2(this.txtAnnouncementName.Text.Replace(" ", ""))) ||
                (PubEntAdminManager.SpecialVal2(this.txtAnouncementURL.Text.Replace(" ", ""))))
            {
                Response.Redirect("InvalidInput.aspx");
            }
        }
        #endregion

        #endregion
    }
}