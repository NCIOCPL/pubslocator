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
using PubEntAdmin.DAL;
using PubEntAdmin.BLL;

namespace PubEntAdmin.UserControl
{
    public partial class CatSeq : System.Web.UI.UserControl
    {
        #region Constants
        private readonly string DEFAULT_SORTBY = "SORT_SEQ";
        #endregion

        #region Events Handling
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ViewState["SortAscending"] = SortDirection.Ascending;
                this.SortExpression = DEFAULT_SORTBY;
                this.ddlValue.Visible = false;
                this.BindData(PE_DAL.GetAllCatalogSubjectSeq(this.SortExpression, true, true));

                if (System.Convert.ToBoolean(Session[PubEntAdminManager.JS]))
                {
                    this.btnUpdateCatSelection.Visible = this.btnUpdateValueSection.Visible = false;
                }
                else
                {
                    this.btnUpdateCatSelection.Visible = true;
                    this.btnUpdateValueSection.Visible = false;

                }
            }
            this.SecVal();
        }

        protected void gvResult_ItemCommand(object source, DataGridCommandEventArgs e)
        {

        }

        protected void gvResult_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            List<Seq> dt = ((List<Seq>)this.gvResult.DataSource);

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Cells[1].Text = Server.HtmlDecode(dt[e.Item.ItemIndex].SeqName);
                ((TextBox)e.Item.FindControl("txtSeq")).Text = dt[e.Item.ItemIndex].SeqValue.ToString();
            }
        }

        protected void gvResult_SortCommand(object source, DataGridSortCommandEventArgs e)
        {
            if ((e.SortExpression == this.SortExpression))
            {
                SortAscending = !SortAscending;
            }
            else
            {
                SortAscending = true;
            }
            // Set the SortExpression property to the SortExpression passed in
            this.SortExpression = e.SortExpression;

            List<Seq> a=null;

            switch (this.ddlCat.SelectedValue)
            {
                case "Category":
                    a = PE_DAL.GetAllCatalogSubjectSeq(this.SortExpression, this.SortAscending, true);
                    break;
                case "Subcategory":
                    a = PE_DAL.GetAllCatalogSubCategorySeq(System.Convert.ToInt32(this.ddlValue.SelectedValue), this.SortExpression, this.SortAscending, true);
                    break;
                case "Subsubcategory":
                    a = PE_DAL.GetAllCatalogSubSubCategorySeq(System.Convert.ToInt32(this.ddlValue.SelectedValue), this.SortExpression, this.SortAscending, true);
                    break;
            }
            this.BindData(a);
        }

        protected void ddlCatSelectedIndexChanged(object sender, EventArgs e)
        {
            this.SortExpression = DEFAULT_SORTBY;
            this.SortAscending = true;

            if (this.ddlCat.SelectedIndex != 0)
            {
                this.ddlValue.Visible = true;

                if (String.Compare(this.ddlCat.SelectedItem.Text, "Subcategory", true) == 0)
                {
                    this.BindDDLValues_Sub(PE_DAL.GetAllCatalogSubjectHvSubcat(true));
                    this.ddlValue.SelectedIndex = 0;
                    this.BindData(PE_DAL.GetAllCatalogSubCategorySeq(System.Convert.ToInt32(this.ddlValue.SelectedValue), this.SortExpression, this.SortAscending, true));
                }
                else if (String.Compare(this.ddlCat.SelectedItem.Text, "Subsubcategory", true) == 0)
                {
                    this.BindDDLValues_Subsub(PE_DAL.GetAllCatalogSubcatHvSubSubcat(true));
                    this.ddlValue.SelectedIndex = 0;
                    this.BindData(PE_DAL.GetAllCatalogSubSubCategorySeq(System.Convert.ToInt32(this.ddlValue.SelectedValue), this.SortExpression, this.SortAscending, true));
                }
            }
            else
            {
                this.ddlValue.Visible = false;

                if (String.Compare(this.ddlCat.SelectedItem.Text, "Category", true) == 0)
                {
                    this.BindData(PE_DAL.GetAllCatalogSubjectSeq(this.SortExpression, this.SortAscending, true));
                }
            }
        }

        protected void ddlValueSelectedIndexChanged(object sender, EventArgs e)
        {
            if (String.Compare(this.ddlCat.SelectedItem.Text, "Subcategory", true) == 0)
            {
                this.BindData(PE_DAL.GetAllCatalogSubCategorySeq(System.Convert.ToInt32(this.ddlValue.SelectedValue),
                    DEFAULT_SORTBY, true, true));
            }
            else if (String.Compare(this.ddlCat.SelectedItem.Text, "Subsubcategory", true) == 0)
            {
                this.BindData(PE_DAL.GetAllCatalogSubSubCategorySeq(System.Convert.ToInt32(this.ddlValue.SelectedValue),
                    DEFAULT_SORTBY, true, true));
            }
        }

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            switch (this.ddlCat.SelectedValue)
            {
                case "Category":
                    if (this.CatSave())
                        this.BindData(PE_DAL.GetAllCatalogSubjectSeq(DEFAULT_SORTBY, true, true));
                    break;
                case "Subcategory":
                    if (this.SubCatSave())
                        this.BindData(PE_DAL.GetAllCatalogSubCategorySeq(System.Convert.ToInt32(this.ddlValue.SelectedValue), DEFAULT_SORTBY, true, true));
                    break;
                case "Subsubcategory":
                    if (this.SubSubCatSave())
                        this.BindData(PE_DAL.GetAllCatalogSubSubCategorySeq(System.Convert.ToInt32(this.ddlValue.SelectedValue), DEFAULT_SORTBY, true, true));
                    break;
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            switch (this.ddlCat.SelectedValue)
            {
                case "Category":
                    this.BindData(PE_DAL.GetAllCatalogSubjectSeq(DEFAULT_SORTBY, true, true));
                    break;
                case "Subcategory":
                    this.BindData(PE_DAL.GetAllCatalogSubCategorySeq(System.Convert.ToInt32(this.ddlValue.SelectedValue), DEFAULT_SORTBY, true, true));
                    break;
                case "Subsubcategory":
                    this.BindData(PE_DAL.GetAllCatalogSubSubCategorySeq(System.Convert.ToInt32(this.ddlValue.SelectedValue), DEFAULT_SORTBY, true, true));
                    break;
            }
        }

        protected void btnUpdateCatSelection_Click(object sender, EventArgs e)
        {
            if (this.ddlCat.Enabled)
            {
                this.btnUpdateCatSelection.Text = "Enable catalog selection";
                
                ddlCatSelectedIndexChanged(this.ddlCat, new EventArgs());

                if (String.Compare(this.ddlCat.SelectedItem.Text, "Category", true) != 0)
                {
                    this.ddlValue.Enabled = true;
                    this.btnUpdateValueSection.Visible = true;
                }
                else
                {
                    this.ddlValue.Enabled = false;
                    this.btnUpdateValueSection.Visible = false;
                }

                this.ddlCat.Enabled = false;
            }
            else
            {
                this.ddlCat.Enabled = true;
                this.btnUpdateCatSelection.Text = "Get Catelog selection";
                this.ddlValue.Enabled = false;
                this.btnUpdateValueSection.Visible = false;
            }

        }

        protected void btnUpdateValueSection_Click(object sender, EventArgs e)
        {
            ddlValueSelectedIndexChanged(this.ddlValue, new EventArgs());
        }
        #endregion

        #region Methods
        private string SaveAUX()
        {
            string id_seqs = "";

            foreach (DataGridItem c in this.gvResult.Items)
            {
                if (c.ItemType == ListItemType.Item || c.ItemType == ListItemType.AlternatingItem)
                {
                    if (id_seqs.Length > 0)
                        id_seqs += PubEntAdminManager.pairDelim;

                    if (((TextBox)c.Cells[2].FindControl("txtSeq")).Text.Trim().Length > 0)
                    {
                        id_seqs += c.Cells[0].Text + PubEntAdminManager.indDelim +
                            ((TextBox)c.Cells[2].FindControl("txtSeq")).Text;
                    }
                }
            }

            return id_seqs;
        }

        protected bool CatSave()
        {
            bool blnCatSave = false;
            this.SecVal();
            string id_seqs = this.SaveAUX();

            if (id_seqs.Length > 0)
                blnCatSave = PE_DAL.SetAllCatalogSubjectSeq(id_seqs, PubEntAdminManager.pairDelim,
                    PubEntAdminManager.indDelim);

            return blnCatSave;
        }

        protected bool SubCatSave()
        {
            bool blnSubCatSave = false;
            this.SecVal();
            string id_seqs = this.SaveAUX();

            if (id_seqs.Length > 0)
                blnSubCatSave = PE_DAL.SetAllCatalogSubCatSeq(id_seqs, PubEntAdminManager.pairDelim,
                    PubEntAdminManager.indDelim);

            return blnSubCatSave;
        }

        protected bool SubSubCatSave()
        {
            bool SubSubCatSave = false;
            this.SecVal();
            string id_seqs = this.SaveAUX();

            if (id_seqs.Length > 0)
                SubSubCatSave = PE_DAL.SetAllCatalogSubSubCatSeq(id_seqs, PubEntAdminManager.pairDelim,
                    PubEntAdminManager.indDelim);

            return SubSubCatSave;
        }

        protected void BindDDLValues_Sub(List<PubEntAdmin.BLL.Subject> coll)
        {
            this.ddlValue.Items.Clear();
            foreach (PubEntAdmin.BLL.Subject o in coll)
            {
                this.ddlValue.Items.Add(new ListItem(o.SubjName, o.SubjID.ToString()));
            }
        }

        protected void BindDDLValues_Subsub(List<PubEntAdmin.BLL.SubCat> coll)
        {
            this.ddlValue.Items.Clear();
            foreach (PubEntAdmin.BLL.SubCat o in coll)
            {
                this.ddlValue.Items.Add(new ListItem(o.SubCatName, o.SubCatID.ToString()));
            }
        }

        protected void BindData(List<Seq> coll)
        {
            this.gvResult.DataSource = coll; 
            this.gvResult.DataBind();
        }

        #region Sec Val
        private void SecVal()
        {
            this.LenVal();
            this.TypeVal();
            this.TagVal();
            this.SpecialVal();
        }

        private void LenVal()
        {
            foreach (DataGridItem c in this.gvResult.Items)
            {
                if (c.ItemType == ListItemType.Item || c.ItemType == ListItemType.AlternatingItem)
                {
                    if (!PubEntAdminManager.LenVal(((TextBox)c.Cells[2].FindControl("txtSeq")).Text,8))
                    {
                        Response.Redirect("InvalidInput.aspx");
                    }
                }
            }
        }

        private void TypeVal()
        {
            foreach (DataGridItem c in this.gvResult.Items)
            {
                if (c.ItemType == ListItemType.Item || c.ItemType == ListItemType.AlternatingItem)
                {
                    if (!PubEntAdminManager.ContentNumVal(((TextBox)c.Cells[2].FindControl("txtSeq")).Text))
                    {
                        Response.Redirect("InvalidInput.aspx");
                    }
                }
            }
        }

        private void TagVal()
        {
            foreach (ListItem li in this.ddlCat.Items)
            {
                if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in ddlValue.Items)
            {
                if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (DataGridItem c in this.gvResult.Items)
            {
                if (c.ItemType == ListItemType.Item || c.ItemType == ListItemType.AlternatingItem)
                {
                    if (PubEntAdminManager.OtherVal(c.Cells[0].Text) ||
                        PubEntAdminManager.OtherVal(((TextBox)c.Cells[2].FindControl("txtSeq")).Text))
                    {
                        Response.Redirect("InvalidInput.aspx");
                    }
                }
            }
        }

        private void SpecialVal()
        {
            foreach (ListItem li in ddlCat.Items)
            {
                if (PubEntAdminManager.SpecialVal2(li.Text.Replace(" ", "")) ||
                    PubEntAdminManager.SpecialVal2(li.Value.Replace(" ", "")))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in ddlValue.Items)
            {
                if (PubEntAdminManager.SpecialVal2(li.Text.Replace(" ", "")) ||
                    PubEntAdminManager.SpecialVal2(li.Value.Replace(" ", "")))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (DataGridItem c in this.gvResult.Items)
            {
                if (c.ItemType == ListItemType.Item || c.ItemType == ListItemType.AlternatingItem)
                {
                    if (PubEntAdminManager.SpecialVal2(c.Cells[0].Text.Replace(" ", "")) ||
                        PubEntAdminManager.SpecialVal2(((TextBox)c.Cells[2].FindControl("txtSeq")).Text.Replace(" ", "")))
                    {
                        Response.Redirect("InvalidInput.aspx");
                    }
                }
            }
        }
        #endregion

        #endregion

        #region Properties
        private string SortExpression
        {
            get
            {
                object o = ViewState["SortExpression"];
                if ((o == null))
                {
                    return String.Empty;
                }
                else
                {
                    return o.ToString();
                }
            }
            set
            {
                ViewState["SortExpression"] = value;
            }
        }

        private bool SortAscending
        {
            get
            {
                object o = ViewState["SortAscending"];
                if ((o == null))
                {
                    return true;
                }
                else
                {
                    if (o is SortDirection)
                        return true;
                    else
                        return Convert.ToBoolean(o);
                }
            }
            set
            {
                ViewState["SortAscending"] = value;
            }
        }
        #endregion
        
    }
}