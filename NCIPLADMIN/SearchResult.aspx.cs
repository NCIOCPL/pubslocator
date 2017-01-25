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
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Drawing;
using AjaxControlToolkit;
using PubEntAdmin.DAL;
using PubEntAdmin.BLL;
using GlobalUtils;

namespace PubEntAdmin
{
    public partial class SearchResult : System.Web.UI.Page
    {
        #region Controls
        private Search mySearch;
        private new Home PreviousPage=null;
        private bool blnExport = false;
        #endregion

        #region Events Handling

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Context.Handler is Home)
                {
                    this.PreviousPage = (Home)Context.Handler;
                }

                if (PreviousPage != null)
                {
                    if ((String.Compare(PreviousPage.AppRelativeVirtualPath,"~/Home.aspx",true) != 0) ||
                    !(PreviousPage is Home))
                    {
                        this.mySearch = ((Search)Session[PubEntAdminManager.strSearchCriteria]);
                        this.lblSearchCriteria.Text = this.mySearch.SearchCriteriaDisplay;
                        this.BindData(this.mySearch.PUBIDs);
                    }
                    else//from home
                    {
                        Session.Remove(PubEntAdminManager.strSearchCriteria);
                        this.lblSearchCriteria.Text = this.CaptureSearchCriteria();
                        this.BindData_(null);
                    }
                }
                else
                {
                    this.mySearch = ((Search)Session[PubEntAdminManager.strSearchCriteria]);
                    this.lblSearchCriteria.Text = this.mySearch.SearchCriteriaDisplay;
                    this.BindData(this.mySearch.PUBIDs);
                }

                if (PubEntAdminManager.TamperProof)
                {
                    this.hyplnkRefSrch1.NavigateUrl = this.hyplnkRefSrch2.NavigateUrl = 
                        PubEntAdminManager.EncodedURLWithQS("Home.aspx", "action=refine");
                }
                else{
                    this.hyplnkRefSrch1.NavigateUrl = this.hyplnkRefSrch2.NavigateUrl =
                        "Home.aspx?action=refine";
                }
                
            }
                       

            if (!IsPostBack && !IsCallback)
            {
                //This makes performing paging and sorting uses AJAX.
                //gvResult.enEnableSortingAndPagingCallbacks = true;
                //gvResult.PageSize = 3;
                ViewState["SortAscending"] = SortDirection.Ascending;
                //this.gvResult.ArrowDownImageUrl = "~/Image/RedDown.png";
                //this.gvResult.ArrowUpImageUrl = "~/Image/BlackWhiteUp.png";
                //Response.Write("haahahhahahhah "+ this.PreviousPage.Keyword);
                this.SortExpression = PubEntAdminManager.strDefaultoSearchSorting;
                //this.BindData();
            }
            
            System.Web.UI.UserControl userControl =
                (System.Web.UI.UserControl)this.LoadControl("UserControl/AdminMenu.ascx");
            this.plcHldMenu.Controls.Add(userControl);
                
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            return;
        }
        
        protected void gvResult_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            List<Pub> dt = ((List<Pub>)this.gvResult.DataSource);
            HyperLink l_hl = new HyperLink();

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Pub l_pub = dt[e.Item.ItemIndex];
                
                if (!this.Export)
                    this.mySearch.AddPubId(l_pub.PubID);
                //---------------------------------------------------
                l_hl.Text = l_pub.Name;

                if (PubEntAdminManager.TamperProof)
                {
                    l_hl.NavigateUrl = PubEntAdminManager.EncodedURLWithQS("PubRecord.aspx", "mode=view&pubid=" + l_pub.PubID);
                }
                else
                {
                    l_hl.NavigateUrl = "~/PubRecord.aspx?mode=view&pubid=" + l_pub.PubID;
                }

                e.Item.Cells[3].Controls.Add(l_hl);
                //---------------------------------------------------
                if (l_pub.QtyAvailable == -1)
                {
                    e.Item.Cells[4].Text = "N/A";
                }
                //---------------------------------------------------
                if (l_pub.Status.Length > 0)
                {
                    if (l_pub.Status.Contains(PubEntAdminManager.stringDelim))
                    {
                        string[] l_bookstatus = l_pub.Status.Split(new string[] { PubEntAdminManager.stringDelim },
                            StringSplitOptions.RemoveEmptyEntries);
                        Table l_tb = new Table();
                        foreach (string s in l_bookstatus)
                        {
                            TableRow l_tr = new TableRow();
                            TableCell l_tc = new TableCell();
                            l_tc.Text = s;

                            l_tr.Controls.Add(l_tc);
                            l_tb.Rows.Add(l_tr);
                        }
                        e.Item.Cells[5].Controls.Add(l_tb);
                    }
                }
                else
                {
                    Label l_lbl = new Label();
                    l_lbl.Text = "-";
                    e.Item.Cells[5].Controls.Add(l_lbl);
                }
                //---------------------------------------------------
                if (l_pub.CreateDate.CompareTo(new DateTime(1900, 1, 1)) == 0)
                {
                    e.Item.Cells[6].Text = "-";
                }
                //---------------------------------------------------
                CheckBox l_chkbox = (CheckBox)e.Item.FindControl("chkSelect");
                if (Request.Browser.MSDomVersion.Major == 0) // Non IE Browser?)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(),
                    "myScript",
                    " function HighlightSelected(colorcheckboxselected, RowState) { if (colorcheckboxselected.checked) colorcheckboxselected.parentNode.parentNode.style.backgroundColor='#FFAA63'; else { if (RowState=='Item') colorcheckboxselected.parentNode.parentNode.style.backgroundColor='white'; else colorcheckboxselected.parentNode.parentNode.style.backgroundColor='#D6E3F7'; } }"
                    , true);
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(),
                    "myScript",
                    " function HighlightSelected(colorcheckboxselected, RowState) { if (colorcheckboxselected.checked) colorcheckboxselected.parentElement.parentElement.style.backgroundColor='#FFAA63'; else { if (RowState=='Item') colorcheckboxselected.parentElement.parentElement.style.backgroundColor='#f5f5f3'; else colorcheckboxselected.parentElement.parentElement.style.backgroundColor='#ffffff'; } }"
                    , true);
                }
                l_chkbox.Attributes.Add("onclick", "HighlightSelected(this,'" + Convert.ToString(e.Item.ItemType) + "' );");

                //QC Report
                HyperLink QCLink = (HyperLink)e.Item.FindControl("QCLink");
                QCLink.NavigateUrl = "~/reportform.aspx?pubid=" + l_pub.PubID;
            }
        }
        

        #region Gridview Event Handler
        protected void gvResult_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PubCollection dt = ((PubCollection)this.gvResult.DataSource);
			HyperLink l_hl = new HyperLink();

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox l_chkbox = (CheckBox)e.Row.FindControl("chkSelect");
                if (Request.Browser.MSDomVersion.Major == 0) // Non IE Browser?)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(),
                    "myScript",
                    " function HighlightSelected(colorcheckboxselected, RowState) { if (colorcheckboxselected.checked) colorcheckboxselected.parentNode.parentNode.style.backgroundColor='#FFAA63'; else { if (RowState=='Normal') colorcheckboxselected.parentNode.parentNode.style.backgroundColor='white'; else colorcheckboxselected.parentNode.parentNode.style.backgroundColor='#D6E3F7'; } }"
                    , true);

                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(),
                    "myScript",
                    " function HighlightSelected(colorcheckboxselected, RowState) { if (colorcheckboxselected.checked) colorcheckboxselected.parentElement.parentElement.style.backgroundColor='#FFAA63'; else { if (RowState=='Normal') colorcheckboxselected.parentElement.parentElement.style.backgroundColor='white'; else colorcheckboxselected.parentElement.parentElement.style.backgroundColor='#D6E3F7'; } }"
                    , true);
                }
                l_chkbox.Attributes.Add("onclick", "HighlightSelected(this,'" + Convert.ToString(e.Row.RowState) + "' );"); 
                
                Pub l_pub = dt[e.Row.DataItemIndex];

                /*if (e.Row.RowIndex % 2 == 0)
                {
                    e.Row.Style["color"] = "#0000ff";
                }
                else
                {
                    e.Row.Style["color"] = "#00bb00";
                }*/

                l_hl.Text = l_pub.Name;
                l_hl.NavigateUrl = "~/PubRecord.aspx?mode=edit&pubid=" + l_pub.PubID;

                e.Row.Cells[3].Controls.Add(l_hl);
            }
        }

        protected void gvResult_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //try
            //{
            //    gvResult.PageIndex = e.NewPageIndex;
            //    gvResult.DataSource = PE_DAL.GetSearchResult();
            //    gvResult.DataBind();
            //}
            //catch (Exception ex)
            //{

            //}

        }

        #endregion

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

            this.BindData(((PubEntAdmin.BLL.Search)Session[PubEntAdminManager.strSearchCriteria]).PUBIDs);
            // rebind the DataGrid data
        }

        protected void btnKeepSel_Click(object sender, EventArgs e)
        {
            string ret = "";

            if (this.gvResult.Visible)
            {
                foreach (DataGridItem c in this.gvResult.Items)
                {
                    if (c.ItemType == ListItemType.Item || c.ItemType == ListItemType.AlternatingItem)
                    {
                        CheckBox cb = ((CheckBox)c.Cells[7].FindControl("chkSelect"));
                        //CheckBox cb = c.Cells[6].Controls[1] as CheckBox;
                        if (cb.Checked)
                        {
                            if (ret.Length > 0)
                                ret += ",";

                            ret += c.Cells[0].Text;
                        }
                    }
                }

                if (ret.Length > 0)
                {
                    if (this.SortExpression == PubEntAdminManager.strDefaultoSearchSorting)
                    {
                        this.BindData_(ret);
                    }
                    else
                    {
                        this.BindData(ret);
                    }
                }
                else
                {
                    this.mySearch = ((Search)Session[PubEntAdminManager.strSearchCriteria]);
                    if (this.SortExpression == PubEntAdminManager.strDefaultoSearchSorting)
                    {
                        this.BindData_(this.mySearch.PUBIDs);
                    }
                    else
                    {
                        this.BindData(this.mySearch.PUBIDs);
                    }
                }
            }
        }

        protected void btnDropSel_Click(object sender, EventArgs e)
        {
            string ret = "";

            if (this.gvResult.Visible)
            {
                foreach (DataGridItem c in this.gvResult.Items)
                {
                    if (c.ItemType == ListItemType.Item || c.ItemType == ListItemType.AlternatingItem)
                    {
                        CheckBox cb = ((CheckBox)c.Cells[7].FindControl("chkSelect"));
                        if (!cb.Checked)
                        {
                            if (ret.Length > 0)
                                ret += ",";

                            ret += c.Cells[0].Text;
                        }
                    }
                }

                if (ret.Length > 0)
                {
                    if (this.SortExpression == PubEntAdminManager.strDefaultoSearchSorting)
                    {
                        this.BindData_(ret);
                    }
                    else
                    {
                        this.BindData(ret);
                    }
                }
                else
                {
                    if (this.SortExpression == PubEntAdminManager.strDefaultoSearchSorting)
                    {
                        this.BindData_(String.Empty);
                    }
                    else
                    {
                        this.BindData(String.Empty);
                    }
                }
            }
        }

        protected void ImgBtnExportSchRsltToExcel_OnClick(object sender, ImageClickEventArgs e)
        {
            this.gvResult.AllowSorting = false;
            this.Export = true;
            string ret = "";
            foreach (DataGridItem c in this.gvResult.Items)
            {
                if (c.ItemType == ListItemType.Item || c.ItemType == ListItemType.AlternatingItem)
                {
                    CheckBox cb = ((CheckBox)c.Cells[7].FindControl("chkSelect"));
                    if (cb.Checked)
                    {
                        if (ret.Length > 0)
                            ret += ",";

                        ret += c.Cells[0].Text;
                    }
                }
            }

            if (this.SortExpression == PubEntAdminManager.strDefaultoSearchSorting)
            {
                if (ret.Length == 0)
                    this.BindData_(((PubEntAdmin.BLL.Search)Session[PubEntAdminManager.strSearchCriteria]).PUBIDs);
                else
                    this.BindData_(ret);

            }
            else
            {
                if (ret.Length ==0)
                    this.BindData(((PubEntAdmin.BLL.Search)Session[PubEntAdminManager.strSearchCriteria]).PUBIDs);
                else
                    this.BindData(ret);
            }

            this.gvResult.Columns[7].Visible = false;
            this.Export = false;
            ExportRoutines.ExportToExcel(this.Page, PubEntAdminManager.AdminSearchRptName(), PubEntAdminManager.DefAdminSearchResultRptTitle, this.gvResult);
        }

        protected void btnGetRcd_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.gvResult.Items.Count; i++)
            {
                DataGridItem c = this.gvResult.Items[i];

                if (c.ItemType == ListItemType.Item || c.ItemType == ListItemType.AlternatingItem)
                {
                    CheckBox cb = ((CheckBox)c.Cells[6].FindControl("chkSelect"));
                    if (cb.Checked)
                    {
                        if (PubEntAdminManager.TamperProof)
                        {
                            PubEntAdminManager.RedirectEncodedURLWithQS("PubRecord.aspx", "mode=view&pubid=" + c.Cells[0].Text);
                        }
                        else
                        {
                            Response.Redirect("~/PubRecord.aspx?mode=view&pubid=" + c.Cells[0].Text,true);
                        }
                    }
                }
            }

        }

        #endregion

        #region Properties
        
        private bool Export
        {
            get
            {
                return this.blnExport;
            }
            set
            {
                this.blnExport = value;
            }
        }

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
                    return Convert.ToBoolean(o);
                }
            }
            set
            {
                ViewState["SortAscending"] = value;
            }
        }

        #endregion

        #region Methods
        protected string CaptureSearchCriteria()
        {
            this.mySearch = new Search();

            string l_lbl = "";
            if (this.PreviousPage.NCIPL)
            {
                l_lbl += "<strong>NCIPL:</strong> " + this.PreviousPage.NCIPL.ToString();
                this.mySearch.NCIPL = true;
            }

            if (this.PreviousPage.ROO)
            {
                if (l_lbl.Length > 0)
                    l_lbl += "</br>";
                l_lbl += "<strong>ROO:</strong> " + this.PreviousPage.ROO.ToString();
                this.mySearch.ROO = true;
            }

            if (this.PreviousPage.Exh)
            {
                if (l_lbl.Length > 0)
                    l_lbl += "</br>";
                l_lbl += "<strong>Exhibit:</strong> " + this.PreviousPage.Exh.ToString();
                this.mySearch.EXH = true;
            }

            if (this.PreviousPage.Catalog)
            {
                if (l_lbl.Length > 0)
                    l_lbl += "</br>";
                l_lbl += "<strong>Catalog:</strong> " + this.PreviousPage.Catalog.ToString();
                this.mySearch.CATALOG = true;

            }

            if (this.PreviousPage.Keyword.Trim().Length > 0)
            {
                if (l_lbl.Length > 0)
                    l_lbl += "</br>";

                Regex r = new Regex(@"\s+");
                string l_key = r.Replace(this.PreviousPage.Keyword.Trim(), " ");
                l_lbl += "<strong>Keyword:</strong> " + l_key;
                this.mySearch.Key = l_key;
            }

            if (this.PreviousPage.NIHNum1.Trim().Length > 0 && this.PreviousPage.NIHNum2.Trim().Length > 0)
            {
                if (l_lbl.Length > 0)
                    l_lbl += "</br>";
                l_lbl += "<strong>NIH Num:</strong> " + this.PreviousPage.NIHNum1.Trim() + " - " +
                    this.PreviousPage.NIHNum2.Trim();
                this.mySearch.NIH1 = this.PreviousPage.NIHNum1.Trim();
                this.mySearch.NIH2 = this.PreviousPage.NIHNum2.Trim();
            }
            else if (this.PreviousPage.NIHNum1.Trim().Length > 0)
            {
                if (l_lbl.Length > 0)
                    l_lbl += "</br>";
                l_lbl += "<strong>NIH Num:</strong> " + this.PreviousPage.NIHNum1.Trim();
                this.mySearch.NIH1 = this.PreviousPage.NIHNum1.Trim();
            }
            else if (this.PreviousPage.NIHNum2.Trim().Length > 0)
            {
                if (l_lbl.Length > 0)
                    l_lbl += "</br>";
                l_lbl += "<strong>NIH Num:</strong> " + this.PreviousPage.NIHNum2.Trim();
                this.mySearch.NIH2 = this.PreviousPage.NIHNum2.Trim();
            }

            if (this.PreviousPage.CreatedFrom.Length > 0)
            {
                if (l_lbl.Length > 0)
                    l_lbl += "</br>";
                l_lbl += "<strong>Date Record Created From:</strong> " + this.PreviousPage.CreatedFrom;
                this.mySearch.CreateFrom = this.PreviousPage.CreatedFrom;
            }

            if (this.PreviousPage.Newpub)
            {
                string status = String.Empty;
                if (this.PreviousPage.Newpub)
                    status = "Yes";
                if (l_lbl.Length > 0)
                    l_lbl += "</br>";
                l_lbl += "<strong>New Pub:</strong> " + status;
                this.mySearch.ISNEW = true;
            }

            if (this.PreviousPage.Updatedpub)
            {
                string status = String.Empty;
                if (this.PreviousPage.Updatedpub)
                    status = "Yes";
                if (l_lbl.Length > 0)
                    l_lbl += "</br>";
                l_lbl += "<strong>Updated Pub:</strong> " + status;
                this.mySearch.ISUPDATED = true;
            }

            if (this.PreviousPage.CreatedTo.Length > 0)
            {
                if (l_lbl.Length > 0)
                    l_lbl += "</br>";
                l_lbl += "<strong>Date Record Created To:</strong> " + this.PreviousPage.CreatedTo;
                this.mySearch.CreateTo = this.PreviousPage.CreatedTo;
            }

            if (this.PreviousPage.SelectedCancerTypeItem.Length > 0)
            {
                if (l_lbl.Length > 0)
                    l_lbl += "</br>";
                l_lbl += "<strong>Cancer Type:</strong> " + this.PreviousPage.SelectedCancerTypeItem;
                this.mySearch.Cancer = this.PreviousPage.SelectedCancerType;
            }


            if (this.PreviousPage.SelectedSubjectItem.Length > 0)
            {
                if (l_lbl.Length > 0)
                    l_lbl += "</br>";
                l_lbl += "<strong>Subject:</strong> " + this.PreviousPage.SelectedSubjectItem;
                this.mySearch.Subj = this.PreviousPage.SelectedSubject;
            }

            if (this.PreviousPage.SelectedAudienceItem.Length > 0)
            {
                if (l_lbl.Length > 0)
                    l_lbl += "</br>";
                l_lbl += "<strong>Audience:</strong> " + this.PreviousPage.SelectedAudienceItem;
                this.mySearch.Aud = this.PreviousPage.SelectedAudience;
            }

            if (this.PreviousPage.SelectedLangItem.Length > 0)
            {
                if (l_lbl.Length > 0)
                    l_lbl += "</br>";
                l_lbl += "<strong>Language:</strong> " + this.PreviousPage.SelectedLangItem;
                this.mySearch.Lang = this.PreviousPage.SelectedLang;
            }

            if (this.PreviousPage.SelectedProdFormatItem.Length > 0)
            {
                if (l_lbl.Length > 0)
                    l_lbl += "</br>";
                l_lbl += "<strong>Publication Format:</strong> " + this.PreviousPage.SelectedProdFormatItem;
                this.mySearch.Format = this.PreviousPage.SelectedProdFormat;
            }

            if (this.PreviousPage.SelectedSeriesItem.Length > 0)
            {
                if (l_lbl.Length > 0)
                    l_lbl += "</br>";
                l_lbl += "<strong>Collections:</strong> " + this.PreviousPage.SelectedSeriesItem;
                this.mySearch.Serie = this.PreviousPage.SelectedSeries;
            }

            if (this.PreviousPage.SelectedRaceItem.Length > 0)
            {
                if (l_lbl.Length > 0)
                    l_lbl += "</br>";
                l_lbl += "<strong>Race:</strong> " + this.PreviousPage.SelectedRaceItem;
                this.mySearch.Race = this.PreviousPage.SelectedRace;
            }

            if (this.PreviousPage.SelectedBookStatusItem.Length > 0)
            {
                if (l_lbl.Length > 0)
                    l_lbl += "</br>";
                l_lbl += "<strong>Book Status:</strong> " + this.PreviousPage.SelectedBookStatusItem;
                this.mySearch.Status = this.PreviousPage.SelectedBookStatus;
            }

            if (this.PreviousPage.SelectedReadingLevelItem.Length > 0)
            {
                if (l_lbl.Length > 0)
                    l_lbl += "</br>";
                l_lbl += "<strong>Reading Level:</strong> " + this.PreviousPage.SelectedReadingLevelItem;
                this.mySearch.Level = this.PreviousPage.SelectedReadingLevel;
            }

            if (this.PreviousPage.ROOMostCom)
            {
                if (l_lbl.Length > 0)
                    l_lbl += "</br>";
                l_lbl += "<strong>ROO Most Common List:</strong> " + this.PreviousPage.ROOMostCom.ToString();
                this.mySearch.ROOCOM = this.PreviousPage.ROOMostCom;
            }

            if (this.PreviousPage.SelectedROOSubjectItem.Length > 0)
            {
                if (l_lbl.Length > 0)
                    l_lbl += "</br>";
                l_lbl += "<strong>ROO Most Common Subject:</strong> " + this.PreviousPage.SelectedROOSubjectItem;
                this.mySearch.ROOCOMSubj = this.PreviousPage.SelectedROOSubject;
            }

            if (this.PreviousPage.SelectedAwardItem.Length > 0)
            {
                if (l_lbl.Length > 0)
                    l_lbl += "</br>";
                l_lbl += "<strong>Award:</strong> " + this.PreviousPage.SelectedAwardItem;
                this.mySearch.Award = this.PreviousPage.SelectedAward;
            }

            if (this.PreviousPage.SelectedOwner.Length > 0)
            {
                if (l_lbl.Length > 0)
                    l_lbl += "</br>";
                l_lbl += "<strong>Owner:</strong> " + this.PreviousPage.SelectedOwnerItem;
                this.mySearch.Owner = this.PreviousPage.SelectedOwner;
            }
            if (this.PreviousPage.SelectedSponsor.Length > 0)
            {
                if (l_lbl.Length > 0)
                    l_lbl += "</br>";
                l_lbl += "<strong>Sponsor:</strong> " + this.PreviousPage.SelectedSponsorItem;
                this.mySearch.Sponsor = this.PreviousPage.SelectedSponsor;
            }

            this.mySearch.SearchCriteriaDisplay = l_lbl;
            return l_lbl;
        }

        protected string CleanSearchTerms(string s)
        {
            Regex r = new Regex(@"\s+");
            string l_key = r.Replace(s, " ");
            l_key = PubEntAdminManager.Clean(l_key);
            l_key = PubEntAdminManager.StripOutNoise(l_key);

            return l_key;
        }

        protected void BindData(string PubIDs)
        {
            this.mySearch = ((Search)Session[PubEntAdminManager.strSearchCriteria]);
            //this.mySearch.ClearPubId();

            List<Pub> l_listPub = PE_DAL.AdminDoSearchByPubID(PubIDs, 
                this.SortExpression == String.Empty ? PubEntAdminManager.strDefaultSearchSorting : this.SortExpression, System.Convert.ToInt32(this.SortAscending));
            //this.lblTotalResCnt.Text = l_listPub.Count.ToString();
            //this.gvResult.DataSource = l_listPub;
            //this.gvResult.DataBind();

            if (l_listPub.Count == 1)
            {
                if (this.Export)
                {
                    this.RemColumnHeadersSortedImg();
                    this.gvResult.DataSource = l_listPub;
                    this.gvResult.DataBind();
                }
                else
                {
                    this.mySearch.ClearPubId();
                    this.mySearch.AddPubId(l_listPub[0].PubID);
                    Session[PubEntAdminManager.strSearchCriteria] = this.mySearch;

                    //go directly to pubrecord page
                    if (PubEntAdminManager.TamperProof)
                    {
                        PubEntAdminManager.RedirectEncodedURLWithQS("PubRecord.aspx", "mode=view&pubid=" + l_listPub[0].PubID.ToString());
                    }
                    else
                    {
                        Response.Redirect("PubRecord.aspx?mode=view&pubid=" + l_listPub[0].PubID.ToString(), true);
                    }
                }
            }
            else
            {
                this.lblTotalResCnt.Text = l_listPub.Count.ToString();

                if (!this.Export)
                {
                    this.mySearch.ClearPubId();
                    //Update the column headers
                    UpdateColumnHeaders();
                }
                else
                {
                    this.RemColumnHeadersSortedImg();
                }

                this.gvResult.DataSource = l_listPub;
                this.gvResult.DataBind();

                if (l_listPub.Count == 0)
                {
                    this.trTopCtrlPnl.Visible = this.trBtmCtrlPnl.Visible = false;
                    this.gvResult.Visible = false;
                    this.hyplnkRefSrch2.Visible = this.hyplnkSrch2.Visible = false;
                }
            }

            Session[PubEntAdminManager.strSearchCriteria] = this.mySearch;
        }

        protected void BindData_(string PubIDs)
        {
            List<Pub> l_listPub = null;

            string l_key = String.Empty;

            if (PubIDs == null)
            {
                l_key = this.CleanSearchTerms(this.PreviousPage.Keyword.Trim());

                l_listPub = 
                    PE_DAL.AdminDoSearch(
                    this.PreviousPage.NCIPL ? 1 : 0,
                    this.PreviousPage.ROO ? 1 : 0,
                    this.PreviousPage.Exh ? 1 : 0,
                    this.PreviousPage.Catalog ? 1 : 0,
                    l_key.Length > 0 ? l_key : null,
                    this.PreviousPage.NIHNum1.Trim().Length > 0 ? this.PreviousPage.NIHNum1.Trim() : null,
                    this.PreviousPage.NIHNum2.Trim().Length > 0 ? this.PreviousPage.NIHNum2.Trim() : null,
                    Convert.ToBoolean(this.PreviousPage.Newpub) ? 1 : 0,
                    Convert.ToBoolean(this.PreviousPage.Updatedpub) ? 1 : 0,
                    this.PreviousPage.CreatedFrom.Length > 0 ? Convert.ToDateTime(this.PreviousPage.CreatedFrom) : DateTime.MinValue ,
                    this.PreviousPage.CreatedTo.Length > 0 ? Convert.ToDateTime(this.PreviousPage.CreatedTo) : DateTime.MinValue ,
                    (this.PreviousPage.SelectedCancerType.Length >0 && this.PreviousPage.SelectedCancerType != "0") ? this.PreviousPage.SelectedCancerType : null,
                    (this.PreviousPage.SelectedSubject.Length > 0 && this.PreviousPage.SelectedSubject != "0") ? this.PreviousPage.SelectedSubject : null,
                    (this.PreviousPage.SelectedAudience.Length > 0 && this.PreviousPage.SelectedAudience != "0") ? this.PreviousPage.SelectedAudience : null,
                    (this.PreviousPage.SelectedLang.Length > 0 && this.PreviousPage.SelectedLang != "0") ? this.PreviousPage.SelectedLang : null,
                    (this.PreviousPage.SelectedProdFormat.Length > 0 && this.PreviousPage.SelectedProdFormat != "0") ? this.PreviousPage.SelectedProdFormat : null,
                    (this.PreviousPage.SelectedSeries.Length > 0 && this.PreviousPage.SelectedSeries != "0") ? this.PreviousPage.SelectedSeries : null,
                    (this.PreviousPage.SelectedRace.Length > 0 && this.PreviousPage.SelectedRace != "0") ? this.PreviousPage.SelectedRace : null,
                    (this.PreviousPage.SelectedBookStatus.Length > 0 && this.PreviousPage.SelectedBookStatus != "0") ? this.PreviousPage.SelectedBookStatus : null,
                    (this.PreviousPage.SelectedReadingLevel.Length > 0 && this.PreviousPage.SelectedReadingLevel != "0") ? this.PreviousPage.SelectedReadingLevel : null,
                    this.PreviousPage.ROOMostCom ? 1 : 0,
                    (this.PreviousPage.SelectedROOSubject.Length > 0 && this.PreviousPage.SelectedROOSubject != "0") ? this.PreviousPage.SelectedROOSubject : null,
                    (this.PreviousPage.SelectedAward.Length > 0 && this.PreviousPage.SelectedAward != "0") ? this.PreviousPage.SelectedAward : null,
                    (this.PreviousPage.SelectedOwner.Length > 0 && this.PreviousPage.SelectedOwner != "0") ? this.PreviousPage.SelectedOwner : null,
                    (this.PreviousPage.SelectedSponsor.Length > 0 && this.PreviousPage.SelectedSponsor != "0") ? this.PreviousPage.SelectedSponsor : null,null              
                    
                    );

            }
            else
            {
                this.mySearch = ((Search)Session[PubEntAdminManager.strSearchCriteria]);

                l_key = this.CleanSearchTerms(this.mySearch.Key.Trim());

                l_listPub = PE_DAL.AdminDoSearch(
                    this.mySearch.NCIPL ? 1 : 0,
                    this.mySearch.ROO ? 1 : 0,
                    this.mySearch.EXH ? 1 : 0,
                    this.mySearch.CATALOG ? 1 : 0,
                    l_key.Length > 0 ? l_key : null,
                    this.mySearch.NIH1.Trim().Length > 0 ? this.mySearch.NIH1.Trim() : null,
                    this.mySearch.NIH2.Trim().Length > 0 ? this.mySearch.NIH2.Trim() : null,
                    Convert.ToBoolean(this.mySearch.ISNEW) ? 1 : 0,
                    Convert.ToBoolean(this.mySearch.ISUPDATED) ? 1 : 0,
                    this.mySearch.CreateFrom.Length > 0 ? Convert.ToDateTime(this.mySearch.CreateFrom) : DateTime.MinValue ,
                    this.mySearch.CreateTo.Length > 0 ? Convert.ToDateTime(this.mySearch.CreateTo) : DateTime.MinValue ,
                    (this.mySearch.Cancer.Length > 0 && this.mySearch.Cancer != "0") ? this.mySearch.Cancer : null,
                    (this.mySearch.Subj.Length > 0 && this.mySearch.Subj != "0") ? this.mySearch.Subj : null,
                    (this.mySearch.Aud.Length > 0 && this.mySearch.Aud != "0") ? this.mySearch.Aud : null,
                    (this.mySearch.Lang.Length > 0 && this.mySearch.Lang != "0") ? this.mySearch.Lang : null,
                    (this.mySearch.Format.Length > 0 && this.mySearch.Format != "0") ? this.mySearch.Format : null,
                    (this.mySearch.Serie.Length > 0 && this.mySearch.Serie != "0") ? this.mySearch.Serie : null,
                    (this.mySearch.Race.Length > 0 && this.mySearch.Race != "0") ? this.mySearch.Race : null,
                    (this.mySearch.Status.Length > 0 && this.mySearch.Status != "0") ? this.mySearch.Status : null,
                    (this.mySearch.Level.Length > 0 && this.mySearch.Level != "0") ? this.mySearch.Level : null,
                    this.mySearch.ROOCOM ? 1 : 0,
                    (this.mySearch.ROOCOMSubj.Length > 0 && this.mySearch.ROOCOMSubj != "0") ? this.mySearch.ROOCOMSubj : null,
                    (this.mySearch.Award.Length > 0 && this.mySearch.Award != "0")? this.mySearch.Award : null,
                     (this.mySearch.Owner.Length > 0 && this.mySearch.Owner != "0") ? this.mySearch.Owner : null,
                      (this.mySearch.Sponsor.Length > 0 && this.mySearch.Sponsor != "0") ? this.mySearch.Sponsor : null,
                    PubIDs);
            }

            if (l_listPub.Count == 1)
            {
                if (this.Export)
                {
                    this.RemColumnHeadersSortedImg();
                    this.gvResult.DataSource = l_listPub;
                    this.gvResult.DataBind();
                }
                else
                {
                    this.mySearch.ClearPubId();
                    this.mySearch.AddPubId(l_listPub[0].PubID);
                    Session[PubEntAdminManager.strSearchCriteria] = this.mySearch;

                    //go directly to pubrecord page
                    if (PubEntAdminManager.TamperProof)
                    {
                        PubEntAdminManager.RedirectEncodedURLWithQS("PubRecord.aspx", "mode=view&pubid=" + l_listPub[0].PubID.ToString());
                    }
                    else
                    {
                        Response.Redirect("PubRecord.aspx?mode=view&pubid=" + l_listPub[0].PubID.ToString(), true);
                    }
                }
            }
            else
            {
                this.lblTotalResCnt.Text = l_listPub.Count.ToString();

                if (!this.Export)
                {
                    this.mySearch.ClearPubId();
                    //Update the column headers
                    UpdateColumnHeaders();
                }
                else
                {
                    this.RemColumnHeadersSortedImg();
                }

                this.gvResult.DataSource = l_listPub;
                this.gvResult.DataBind();

                if (l_listPub.Count == 0)
                {
                    this.trTopCtrlPnl.Visible = this.trBtmCtrlPnl.Visible = false;
                    this.gvResult.Visible = false;
                    this.hyplnkRefSrch2.Visible = this.hyplnkSrch2.Visible = false;
                }
            }
            Session[PubEntAdminManager.strSearchCriteria] = this.mySearch;
            
        }

        private void UpdateColumnHeaders()
        {
            foreach (DataGridColumn c in this.gvResult.Columns)
            {
                //Below line for QC Report [No need to display sort images]
                if (string.Compare(c.HeaderText, "QC Report") == 0)
                    continue;
                
                c.HeaderText = Regex.Replace(c.HeaderText,@"\s<.*>", String.Empty);
                // Clear any <img> tags that might be present
                if ((c.SortExpression == SortExpression)) {
                    if (SortAscending) {
                        c.HeaderText += " <img id='sortedImg' src='Image/ArrowUp.gif' border='0'>";
                    }
                    else {
                        c.HeaderText += " <img id='sortedImg' src='Image/ArrowDown.gif' border='0'>";
                    }
                }
            }
        }

        private void RemColumnHeadersSortedImg()
        {
            foreach (DataGridColumn c in this.gvResult.Columns)
            {
                c.HeaderText = Regex.Replace(c.HeaderText, @"\s<.*>", String.Empty);
            }
        }
        #endregion

    }
}
