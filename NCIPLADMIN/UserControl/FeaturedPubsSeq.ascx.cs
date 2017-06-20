using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using PubEntAdmin.DAL;
using PubEntAdmin.BLL;

namespace PubEntAdmin.UserControl
{
    public partial class FeaturedPubsSeq : System.Web.UI.UserControl
    {
        #region Constants
        private readonly string DEFAULT_SORTBY = "NCIPLFEATURED_SEQUENCE";
        #endregion
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ViewState["SortAscending"] = SortDirection.Ascending;
                this.SortExpression = DEFAULT_SORTBY;
                this.BindData(PE_DAL.GetAllFeaturedPubsSeq(this.SortExpression, true));              
            }
            this.SecVal();
        }

        protected void BindData(List<Seq> coll)
        {
            this.gvResult.DataSource = coll;
            this.gvResult.DataBind();
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

            List<Seq> a = null;

            a = PE_DAL.GetAllFeaturedPubsSeq(this.SortExpression, this.SortAscending);
           
            this.BindData(a);
        }

        protected void btnSave_OnClick(object sender, EventArgs e)
        {

            if (this.SeqSave())
             this.BindData(PE_DAL.GetAllFeaturedPubsSeq(DEFAULT_SORTBY, true));
                 
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
          
            this.BindData(PE_DAL.GetAllFeaturedPubsSeq(DEFAULT_SORTBY, true));
                   
        }

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

        protected bool SeqSave()
        {
            bool blnSeqSave = false;
            this.SecVal();
            string id_seqs = this.SaveAUX();

            if (id_seqs.Length > 0)
                blnSeqSave = PE_DAL.SetAllFeaturedPubsSeq(id_seqs, PubEntAdminManager.pairDelim,
                    PubEntAdminManager.indDelim);

            return blnSeqSave;
        }
        #endregion

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
                    if (!PubEntAdminManager.LenVal(((TextBox)c.Cells[2].FindControl("txtSeq")).Text, 8))
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