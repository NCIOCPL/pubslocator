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
using PubEntAdmin.BLL;
using PubEntAdmin.DAL;

namespace PubEntAdmin.UserControl
{
    public partial class GenDataE : System.Web.UI.UserControl
    {
        #region Fields
        private int intPubID;
        private string strMode;
        #endregion

        #region Event Handling
        protected void Page_Load(object sender, EventArgs e)
        {
            this.txtLongTitle.Focus();
            #region ClientScript
            ScriptManager.RegisterStartupScript(this,
                typeof(GenDataE), "GenDataE_ClientScript",
            @"
                function txtCPJNumVal(oSrc, args)
                {

                    var e = window.event;
                    if (e){
					    var targ;
					    if (e.target) targ = e.target;
					    else if (e.srcElement) targ = e.srcElement;
				        if ((targ.id == '" + this.GetSaveBtn + @"') ||
                            (targ.id == '" + this.GetSaveBtn2 + @"'))
				        {
                            var errormsg = '';
                            
                            if (document.getElementById('" + this.txtCPJNum.ClientID + @"').value.length>0)
                                args.IsValid = true;
                            else
                            {
                                args.IsValid = false;
                                alert('Publication ID is required.');
                                needToConfirm = true;
                            }
                        }
                        else
                        {
                            args.IsValid = true;
                        }
                    }
                } 

                function txtShortTitleVal(oSrc, args)
                {
                    var e = window.event;
                    if (e){
					    var targ
					    if (e.target) targ = e.target
					    else if (e.srcElement) targ = e.srcElement
					    if ((targ.id == '" + this.GetSaveBtn + @"') ||
                            (targ.id == '" + this.GetSaveBtn2 + @"'))
					    {
                            var errormsg = '';
                            
                            if (document.getElementById('" + this.txtShortTitle.ClientID + @"').value.length>0)
                                args.IsValid = true;
                            else
                            {
                                args.IsValid = false;
                                alert('Short Title is required.');
                                needToConfirm = true;
                            }
                        }
                        else
                        {
                            args.IsValid = true;
                        }
                    }
                } 

                function txtLongTitleVal(oSrc, args)
                {
                    var e = window.event;
                    if (e){
					    var targ
					    if (e.target) targ = e.target
					    else if (e.srcElement) targ = e.srcElement
					    if ((targ.id == '" + this.GetSaveBtn + @"') ||
                            (targ.id == '" + this.GetSaveBtn2 + @"'))
					    {
                            var errormsg = '';
                            
                            if (document.getElementById('" + this.txtLongTitle.ClientID + @"').value.length>0)
                                args.IsValid = true;
                            else
                            {
                                args.IsValid = false;
                                alert('Long Title is required.');
                                needToConfirm = true;
                            }
                        }
                        else
                        {
                            args.IsValid = true;
                        }
                    }
                }
            ", true);
            #endregion

            this.BindLists();
            if (this.PubID > 0)
                this.BindValue();
            this.SecVal();
            this.RegisterCharCountDown();
            this.RegisterMonitoredChanges();
            

        }
        #endregion

        #region Methods

        public string GetSpellCheckParticipants()
        {
            return //this.txtShortTitle.ClientID + "," +
                 this.txtLongTitle.ClientID+ "," +
                 this.txtSpanishAccentLongTitle.ClientID + "," +
                 this.txtSpanishNoAccentLongTitle.ClientID;
        }

        protected void BindValue()
        {
            Pub onePub = PE_DAL.GetProdGenData(intPubID);
            this.txtCPJNum.Text = onePub.ProdID;
            this.txtShortTitle.Text = onePub.ShortTitle;
            this.txtLongTitle.Text = onePub.LongTitle;
            this.lblNIHNum.Text = onePub.NIHNum;
            this.txtFS.Text = onePub.FSNum;
            this.txtSpanishAccentLongTitle.Text = onePub.SpanishAccentLongTitle;
            this.txtSpanishNoAccentLongTitle.Text = onePub.SpanishNoAccentLongTitle;
            this.txtURL.Text = onePub.URL;
            this.txtNerdoURL.Text = onePub.URL2;
            this.txtPDFURL.Text = onePub.PDFURL;
            this.txtKindleURL.Text = onePub.KINDLEURL;
            this.txtePubURL.Text = onePub.EPUBURL;
            this.txtPrintFileURL.Text = onePub.PRINTFILEURL;
           
            if (onePub.Qty >= 0)
                this.lblMaxQtyTile.Text = this.txtMaxQtyTile.Text = onePub.Qty.ToString();
            else
                this.lblMaxQtyTile.Text = this.txtMaxQtyTile.Text = String.Empty;
            if (onePub.QtyAvailable >= 0)
            {
                //NCIDC this.lblQtyAvai.Text = onePub.QtyAvailable.ToString();
                this.txtQtyAvai.Text = onePub.QtyAvailable.ToString();
            }
            else
            {
                //NCIDC this.lblQtyAvai.Text = String.Empty;
                this.txtQtyAvai.Text = String.Empty;
            }
            if (onePub.QtyThreshold  >= 0)
                this.txtQtyThresh.Text = onePub.QtyThreshold.ToString();
            else
                this.txtQtyThresh.Text = String.Empty;


            this.PopulateBookStatusDropDown(onePub.Status.Trim()); //NCIDC
            //NCIDC this.lblBkStatus.Text = onePub.Status;

            if (onePub.Weight >= 0)
                this.lblWeight.Text = this.txtWeight.Text = onePub.Weight.ToString();
            else
                this.lblWeight.Text = this.txtWeight.Text = String.Empty;

            //this.lblOwnervalue.Text = onePub.Owner;
            //this.lblSponsorvalue.Text = onePub.Sponsor;

            this.lblRecPubDate.Text = onePub.RecDate.CompareTo(DateTime.MinValue) != 0 ? onePub.RecDate.ToShortDateString() : String.Empty;

            if (onePub.ProdOrigDate_M == 0)
            {
                if (onePub.ProdOrigDate_Y == 0)
                    this.lblOrigPubDate.Text = "";
                else
                    this.lblOrigPubDate.Text = onePub.ProdOrigDate_Y.ToString();
            }
            else
            {

                this.lblOrigPubDate.Text = onePub.ProdOrigDate_M.ToString();

                if (onePub.ProdOrigDate_D == 0)
                {
                    if (onePub.ProdOrigDate_Y == 0)
                    {
                        this.lblOrigPubDate.Text = "";
                    }
                    else
                    {
                        this.lblOrigPubDate.Text += "/" + onePub.ProdOrigDate_Y.ToString();
                    }
                }
                else
                {
                    this.lblOrigPubDate.Text += "/" + onePub.ProdOrigDate_D.ToString();
                    if (onePub.ProdOrigDate_Y == 0)
                    {
                        this.lblOrigPubDate.Text = "";
                    }
                    else
                    {
                        this.lblOrigPubDate.Text += "/" + onePub.ProdOrigDate_Y.ToString();
                    }
                }
            }

            if (onePub.LastPrintDate_M == 0)
            {
                if (onePub.LastPrintDate_Y == 0)
                    this.lblLatestPrintDate.Text = "";
                else
                    this.lblLatestPrintDate.Text = onePub.LastPrintDate_Y.ToString();
            }
            else
            {

                this.lblLatestPrintDate.Text = onePub.LastPrintDate_M.ToString();

                if (onePub.LastPrintDate_D == 0)
                {
                    if (onePub.LastPrintDate_Y == 0)
                    {
                        this.lblLatestPrintDate.Text = "";
                    }
                    else
                    {
                        this.lblLatestPrintDate.Text += "/" + onePub.LastPrintDate_Y.ToString();
                    }
                }
                else
                {
                    this.lblLatestPrintDate.Text += "/" + onePub.LastPrintDate_D.ToString();
                    if (onePub.LastPrintDate_Y == 0)
                    {
                        this.lblLatestPrintDate.Text = "";
                    }
                    else
                    {
                        this.lblLatestPrintDate.Text += "/" + onePub.LastPrintDate_Y.ToString();
                    }
                }
            }

            if (onePub.LastRevDate_M == 0)
            {
                if (onePub.LastRevDate_Y == 0)
                    this.lblRevPubDate.Text = "";
                else
                    this.lblRevPubDate.Text = onePub.LastRevDate_Y.ToString();
            }
            else
            {

                this.lblRevPubDate.Text = onePub.LastRevDate_M.ToString();

                if (onePub.LastRevDate_D == 0)
                {
                    if (onePub.LastRevDate_Y == 0)
                    {
                        this.lblRevPubDate.Text = "";
                    }
                    else
                    {
                        this.lblRevPubDate.Text += "/" + onePub.LastRevDate_Y.ToString();
                    }
                }
                else
                {
                    this.lblRevPubDate.Text += "/" + onePub.LastRevDate_D.ToString();
                    if (onePub.LastRevDate_Y == 0)
                    {
                        this.lblRevPubDate.Text = "";
                    }
                    else
                    {
                        this.lblRevPubDate.Text += "/" + onePub.LastRevDate_Y.ToString();
                    }
                }
            }
            //this.lblOrigPubDate.Text = onePub.ProdOrigDate.CompareTo(DateTime.MinValue) != 0 ? onePub.ProdOrigDate.ToString() : String.Empty;
            //this.lblLatestPrintDate.Text = onePub.LastPrintDate.CompareTo(DateTime.MinValue) != 0 ? onePub.LastPrintDate.ToString() : String.Empty;
            //this.lblRevPubDate.Text = onePub.LastRevDate.CompareTo(DateTime.MinValue) != 0 ? onePub.LastRevDate.ToString() : String.Empty;
            this.lblArchiveDate.Text = onePub.ArchDate.CompareTo(DateTime.MinValue) != 0 ? onePub.ArchDate.ToShortDateString() : String.Empty;

            //NCIDC if ((this.Mode == PubEntAdminManager.strPubGlobalAMode) ||
                //NCIDC ((this.Mode == PubEntAdminManager.strPubGlobalEMode) && !onePub.NONEDITABLEFIELDSFLAG))
            if ((this.Mode == PubEntAdminManager.strPubGlobalAMode) ||
                (this.Mode == PubEntAdminManager.strPubGlobalEMode)) //Removed check for NONEDITABLEFIELDSFLAG flag (Verified this flag is not set from Admin Tool - possibly had to do something with CPJ at some point)

            {
                this.txtWeight.Visible = this.txtMaxQtyTile.Visible = true;
                this.lblWeight.Visible = this.lblMaxQtyTile.Visible = false;
            }
            else
            {
                this.txtWeight.Visible = this.txtMaxQtyTile.Visible = false;
                this.lblWeight.Visible = this.lblMaxQtyTile.Visible = true;
            }

            //QC Report
            if (onePub.ProdID.Length > 0)
                this.QCLink.NavigateUrl = "~/reportform.aspx?pubid=" + onePub.PubID;


            if (false && onePub.Status == PubEntAdminManager.strInactivePubs)   //***EAC Disabled per HITT 12194 (20120827)
                this.ddlOwner.Enabled = false;
            else
            {

                if (onePub.OwnerID != -1 && onePub.OwnerActive && !onePub.OwnerArcchive)
                    ddlOwner.SelectedValue = onePub.OwnerID.ToString();
                if (!onePub.OwnerActive || onePub.OwnerArcchive)
                {
                    ddlOwner.SelectedItem.Value = onePub.OwnerID.ToString();
                    ddlOwner.SelectedItem.Text = "";
                }
            }

            if (false && onePub.Status == PubEntAdminManager.strInactivePubs)   //***EAC Disabled per HITT 12194 (20120827)
                this.ddlSponsor.Enabled = false;
            else
            {
                if (onePub.SponsorID != -1 && onePub.SponsorActive && !onePub.SponsorArchvie)
                    ddlSponsor.SelectedValue = onePub.SponsorID.ToString();
                if (!onePub.SponsorActive || onePub.SponsorArchvie)
                {
                    ddlSponsor.SelectedItem.Value = onePub.SponsorID.ToString();
                    ddlSponsor.SelectedItem.Text = "";
                }
            }
            
        }

        protected void BindLists()
        {
            this.ddlOwner.DataSource = PE_DAL.GetAllOwners(true);
            this.ddlOwner.DataValueField = "id";
            this.ddlOwner.DataTextField = "name";
            this.ddlOwner.DataBind();
            this.ddlOwner.Items.Insert(0, "");

            this.ddlSponsor.DataSource = PE_DAL.GetAllSponsors(true);
            this.ddlSponsor.DataValueField = "id";
            this.ddlSponsor.DataTextField = "name";
            this.ddlSponsor.DataBind();
            this.ddlSponsor.Items.Insert(0, "");

            this.PopulateBookStatusDropDown(""); //NCIDC
        }

        private void PopulateBookStatusDropDown(string Status) //Added for NCIDC related modifications
        {   
            ddlBookStatus.Items.Clear();
            ddlBookStatus.Items.Add(new ListItem("[Select a value]", ""));

            bool IsBookStatusAssigned = false;
            BookstatusCollection bookStatusColl = PE_DAL.GetAllBookstatus(true);
            foreach (Bookstatus currbookStatus in bookStatusColl)
            {
                ListItem li = new ListItem();
                li.Value = currbookStatus.BookstatusID.ToString();
                li.Text = currbookStatus.BookstatusName;
                if (Status.Length > 0 && string.Compare(Status, li.Text.Trim(), true) == 0)
                {
                    li.Selected = true;
                    IsBookStatusAssigned = true;
                }
                ddlBookStatus.Items.Add(li);
                li = null;
            }
            if (!IsBookStatusAssigned)
                ddlBookStatus.SelectedValue = "";
        }

        public bool Save()
        {
            int retValueSetProdGenData = 0;
            bool retResult = false;
            bool retOwner = true;
            bool retSponsor = true;
            bool blnOwneractive=true;
            bool blnSponsoractive = true;
            bool blnLiveInt = false;            
            
            double l_weight = -1;
            int l_maxQty = -1;

            //Begin NCIDC
            int maxqty = 0; double weight = 0; int qtyavai = 0; int qtythresh = 0; int bookstatusid = 0;
            if (int.TryParse(txtMaxQtyTile.Text.Trim(), out maxqty))
            {
                if (maxqty > 0)
                    l_maxQty = maxqty;
            }
            if (double.TryParse(txtWeight.Text.Trim(), out weight))
            {
                if (weight > 0)
                    l_weight = weight;
            }
            if (int.TryParse(txtQtyAvai.Text.Trim(), out qtyavai))
            {
                if (qtyavai <= 0)
                    qtyavai = -1;
            }
            else
                qtyavai = -1;
            if (int.TryParse(txtQtyThresh.Text.Trim(), out qtythresh))
            {
                if (qtythresh <= 0)
                    qtythresh = -1;
            }
            else
                qtythresh = -1;
            if (string.Compare(ddlBookStatus.SelectedValue.Trim(), "") != 0)
            {
                if (int.TryParse(ddlBookStatus.SelectedValue.Trim(), out bookstatusid))
                {
                    if (bookstatusid <= 0)
                        bookstatusid = -1;
                }
                else
                    bookstatusid = -1;
            }
            else
                bookstatusid = 0; //Special case - remove the assigned bookstatus
            //End NCIDC

            this.SecVal();

            if (this.txtWeight.Visible && this.txtMaxQtyTile.Visible)
            {
                if (this.txtWeight.Text.Trim().Length > 0)
                    l_weight = System.Convert.ToDouble(this.txtWeight.Text.Trim());

                if (this.txtMaxQtyTile.Text.Trim().Length > 0)
                    l_maxQty = System.Convert.ToInt32(this.txtMaxQtyTile.Text.Trim());
            }
            else
            {
                if (this.lblWeight.Text.Trim().Length > 0)
                    l_weight = System.Convert.ToDouble(this.lblWeight.Text.Trim());

                if (this.lblMaxQtyTile.Text.Trim().Length > 0)
                    l_maxQty = System.Convert.ToInt32(this.lblMaxQtyTile.Text.Trim());
            }

            int l_pubid = 0;

            if (Session[PubEntAdminManager.strPubGlobalMode].ToString() !=
                    PubEntAdminManager.strPubGlobalAMode)
            {
                l_pubid = this.PubID;
            }            
                     
            if(ddlOwner.SelectedValue!="")
                blnOwneractive=PE_DAL.GetOwnerStatusByOwnerID(Convert.ToInt32(ddlOwner.SelectedValue));
            if(ddlSponsor.SelectedValue!="")
                blnSponsoractive = PE_DAL.GetSponsorStatusByOwnerID(Convert.ToInt32(ddlSponsor.SelectedValue));
            //NCIDC if (this.lblBkStatus.Text.Trim() != PubEntAdminManager.strInactivePubs && blnOwneractive == false
                //NCIDC || this.lblBkStatus.Text.Trim() != PubEntAdminManager.strInactivePubs && blnSponsoractive == false)
            if (this.ddlBookStatus.SelectedValue.Trim() != PubEntAdminManager.strInactivePubs && blnOwneractive == false
                || this.ddlBookStatus.SelectedValue.Trim() != PubEntAdminManager.strInactivePubs && blnSponsoractive == false)
            {
                //NCIDC if (this.lblBkStatus.Text.Trim() != PubEntAdminManager.strInactivePubs && blnOwneractive == false)
                if (this.ddlBookStatus.SelectedValue.Trim() != PubEntAdminManager.strInactivePubs && blnOwneractive == false)
                {
                    lblOwnerEr.Visible = true;
                    lblOwnerEr.Text = "An active owner name is required.";
                    retResult = false;
                }
                //NCIDC if (this.lblBkStatus.Text.Trim() != PubEntAdminManager.strInactivePubs && blnSponsoractive == false)
                if (this.ddlBookStatus.SelectedValue.Trim() != PubEntAdminManager.strInactivePubs && blnSponsoractive == false)
                {
                    lblSponsorEr.Visible = true;
                    lblSponsorEr.Text = "An active sponsor name is required.";
                    retResult = false;
                }
            }

            else
            { 
                PlaceHolder plhLiveInt=(PlaceHolder)this.Parent.FindControl("plcHldLiveInt");
                bool InNCIPL = ((LiveIntSel)plhLiveInt.Controls[0]).InNCIPL;
                bool InROO = ((LiveIntSel)plhLiveInt.Controls[0]).InROO;
                bool InExh = ((LiveIntSel)plhLiveInt.Controls[0]).InExh;
                bool InCatalog = ((LiveIntSel)plhLiveInt.Controls[0]).InCatalog;

                blnLiveInt = InNCIPL || InROO || InExh || InCatalog;
                if (blnLiveInt == true && ddlOwner.SelectedValue == "" ||
                    blnLiveInt == true && this.ddlSponsor.SelectedValue == "")
                {
                    lblOwnerEr.Text = "";
                    lblSponsorEr.Text = "";
                    
                    if (blnLiveInt == true && ddlOwner.SelectedValue == "")
                    {
                        lblOwnerEr.Visible = true;
                        lblOwnerEr.Text = "An owner name is required.";
                        retResult = false;
                    }
                    if (blnLiveInt == true && this.ddlSponsor.SelectedValue == "")
                    {
                        lblSponsorEr.Visible = true;
                        lblSponsorEr.Text = "An sponsor name is required.";
                        retResult = false;
                    }
                }
                else
                {
                    Regex r = new Regex(@"\s+");
                    retValueSetProdGenData = PE_DAL.SetProdGenData(r.Replace(this.txtCPJNum.Text.Trim(), " "),
                            this.txtShortTitle.Text.Trim(),
                            this.txtLongTitle.Text.Trim(),
                            this.txtFS.Text.Trim().Length > 0 ? this.txtFS.Text.Trim() : null,
                            this.txtSpanishAccentLongTitle.Text.Trim().Length > 0 ? this.txtSpanishAccentLongTitle.Text.Trim() : null,
                            this.txtSpanishNoAccentLongTitle.Text.Trim().Length > 0 ? this.txtSpanishNoAccentLongTitle.Text.Trim() : null,
                            this.txtURL.Text.Trim().Length > 0 ? this.txtURL.Text.Trim() : null,                           
                            this.txtNerdoURL.Text.Trim().Length > 0 ? this.txtNerdoURL.Text.Trim() : null,
                            this.txtPDFURL.Text.Trim().Length > 0 ? this.txtPDFURL.Text.Trim() : null,
                            this.txtKindleURL.Text.Trim().Length > 0 ? this.txtKindleURL.Text.Trim() : null,
                            this.txtePubURL.Text.Trim().Length > 0 ? this.txtePubURL.Text.Trim() : null,
                            this.txtPrintFileURL.Text.Trim().Length > 0 ? this.txtPrintFileURL.Text.Trim() : null,
                            l_maxQty, l_weight,
                            qtyavai, qtythresh, bookstatusid,
                            ref l_pubid);

                    if (this.ddlOwner.SelectedValue.ToString() != "")
                    {
                        retOwner = PE_DAL.SetOwnerByPubID(l_pubid, Convert.ToInt32(this.ddlOwner.SelectedValue));                        
                    }
                    else
                        PE_DAL.DeleteOwnerByPubID(l_pubid);

                    if (this.ddlSponsor.SelectedValue.ToString() != "")
                    {
                        retSponsor = PE_DAL.SetSponsorByPubID(l_pubid, Convert.ToInt32(this.ddlSponsor.SelectedValue));
                       
                    }
                    else
                        PE_DAL.DeleteSponsorByPubID(l_pubid);
                  

                    if (Session[PubEntAdminManager.strPubGlobalMode].ToString() ==
                             PubEntAdminManager.strPubGlobalAMode)
                    {
                        if (retValueSetProdGenData > 0 && retOwner && retSponsor)
                        {
                            this.PubID = l_pubid;
                            retResult = true;                      
                           
                          
                        }
                        else if (retValueSetProdGenData == 0 || retValueSetProdGenData == -1)
                        {
                            if (l_pubid == 0)
                            {
                                Session[PubEntAdminManager.strGlobalMsg] = "The publication already existed.";

                            }
                            else if (l_pubid == -1)
                            {
                                Session[PubEntAdminManager.strGlobalMsg] = "Error occurs.";
                            }
                            retResult = false;
                        }
                    }
                    else
                    {
                        if (retValueSetProdGenData > 0)
                        {
                            retResult = true;
                        }

                        else if (retValueSetProdGenData == -1)
                        {
                            retResult = false;
                        }
                    }
                }
            }
            return retResult;
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
            if ((!PubEntAdminManager.LenVal(this.txtCPJNum.Text,10))||
                (!PubEntAdminManager.LenVal(this.txtShortTitle.Text,42))||
                (!PubEntAdminManager.LenVal(this.txtLongTitle.Text,500))||
                (!PubEntAdminManager.LenVal(this.txtFS.Text,6))||
                (!PubEntAdminManager.LenVal(this.txtSpanishAccentLongTitle.Text,300))||
                (!PubEntAdminManager.LenVal(this.txtSpanishNoAccentLongTitle.Text,300))||
                (!PubEntAdminManager.LenVal(this.txtURL.Text,500))||
                (!PubEntAdminManager.LenVal(this.txtNerdoURL.Text,500)))
            {
                Response.Redirect("InvalidInput.aspx");
            }

        }

        private void TypeVal()
        {
            if (this.txtMaxQtyTile.Visible && this.txtWeight.Visible)
            {
                if (this.txtMaxQtyTile.Text.Trim().Length > 0 &&
                    this.txtWeight.Text.Trim().Length > 0)
                {
                    if ((!PubEntAdminManager.ContentNumVal(this.txtMaxQtyTile.Text.Trim())) ||
                    (!PubEntAdminManager.ContentNumVal(this.txtWeight.Text.Trim())))
                    {
                        Response.Redirect("InvalidInput.aspx");
                    }
                }
            }
        }

        private void TagVal()
        {
            if ((PubEntAdminManager.OtherVal(this.txtCPJNum.Text)) ||
                (PubEntAdminManager.OtherVal(this.txtShortTitle.Text)) ||
                (PubEntAdminManager.OtherVal(this.txtLongTitle.Text)) ||
                (PubEntAdminManager.OtherVal(this.txtFS.Text)) ||
                (PubEntAdminManager.OtherVal(this.txtSpanishAccentLongTitle.Text)) ||
                (PubEntAdminManager.OtherVal(this.txtSpanishNoAccentLongTitle.Text)) ||
                (PubEntAdminManager.OtherVal(this.txtURL.Text)) ||
                (PubEntAdminManager.OtherVal(this.txtNerdoURL.Text)))
            {
                Response.Redirect("InvalidInput.aspx");
            }

            if (this.txtMaxQtyTile.Visible && this.txtWeight.Visible)
            {
                if ((PubEntAdminManager.OtherVal(this.txtMaxQtyTile.Text.Trim())) ||
                (PubEntAdminManager.OtherVal(this.txtWeight.Text.Trim())))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }
        }

        private void SpecialVal()
        {
            if ((PubEntAdminManager.SpecialVal2(this.txtCPJNum.Text.Replace(" ",""))) ||
                (PubEntAdminManager.SpecialVal2(this.txtShortTitle.Text.Replace(" ",""))) ||
                (PubEntAdminManager.SpecialVal2(this.txtLongTitle.Text.Replace(" ",""))) ||
                (PubEntAdminManager.SpecialVal2(this.txtFS.Text.Replace(" ",""))) ||
                (PubEntAdminManager.SpecialVal2(this.txtSpanishAccentLongTitle.Text.Replace(" ",""))) ||
                (PubEntAdminManager.SpecialVal2(this.txtSpanishNoAccentLongTitle.Text.Replace(" ",""))) ||
                (PubEntAdminManager.SpecialVal2(this.txtURL.Text.Replace(" ",""))) ||
                (PubEntAdminManager.SpecialVal2(this.txtNerdoURL.Text.Replace(" ",""))))
            {
                Response.Redirect("InvalidInput.aspx");
            }

            if (this.txtMaxQtyTile.Visible && this.txtWeight.Visible)
            {
                if ((PubEntAdminManager.SpecialVal2(this.txtMaxQtyTile.Text.Replace(" ", ""))) ||
                (PubEntAdminManager.SpecialVal2(this.txtWeight.Text.Replace(" ", ""))))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }
        }
        #endregion

        #region MonitorChanges
        protected void RegisterMonitoredChanges()
        {
            PubEntAdminManager.MonitorChanges(this.Page, this.txtCPJNum);
            PubEntAdminManager.MonitorChanges(this.Page, this.txtShortTitle);
            PubEntAdminManager.MonitorChanges(this.Page, this.txtLongTitle);
            PubEntAdminManager.MonitorChanges(this.Page, this.txtFS);
            PubEntAdminManager.MonitorChanges(this.Page, this.txtSpanishAccentLongTitle);
            PubEntAdminManager.MonitorChanges(this.Page, this.txtSpanishNoAccentLongTitle);
            PubEntAdminManager.MonitorChanges(this.Page, this.txtURL);
            PubEntAdminManager.MonitorChanges(this.Page, this.txtNerdoURL);
            if (this.txtWeight.Visible && this.txtMaxQtyTile.Visible)
            {
                PubEntAdminManager.MonitorChanges(this.Page, this.txtWeight);
                PubEntAdminManager.MonitorChanges(this.Page, this.txtMaxQtyTile);
            }
        }
        #endregion

        #region CharCountDown
        protected void RegisterCharCountDown()
        {
            PubEntAdminManager.RegisterCharCountDown(this.txtShortTitle, this.txtShortTitle.MaxLength);
            PubEntAdminManager.RegisterCharCountDown(this.txtLongTitle, this.txtLongTitle.MaxLength);
            PubEntAdminManager.RegisterCharCountDown(this.txtSpanishAccentLongTitle, this.txtSpanishAccentLongTitle.MaxLength);
            PubEntAdminManager.RegisterCharCountDown(this.txtSpanishNoAccentLongTitle, this.txtSpanishNoAccentLongTitle.MaxLength);
            
        }
        #endregion

        #endregion

        #region Properties
        public int PubID
        {
            set
            {
                this.intPubID = value;
            }
            get
            {
                return this.intPubID;
            }
        }

        public string URLClientID
        {
            get { return this.txtURL.ClientID; }
        }

        public string PDFURLClientID
        {
            get { return this.txtPDFURL.ClientID; }
        }

        protected string GetSaveBtn
        {
            get { return ((PubRecord)this.Page).GetSaveBtn1; }
        }

        protected string GetSaveBtn2
        {
            get { return ((PubRecord)this.Page).GetSaveBtn2; }
        }

        public string Mode
        {
            set { this.strMode = value; }
            get { return this.strMode; }
        }
        #endregion

        protected void txtPDFURL_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtKindleURL_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtePubURL_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtPrintFileURL_TextChanged(object sender, EventArgs e)
        {

        }
        

        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtSpanishAccentLongTitle_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtSpanishNoAccentLongTitle_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtURL_TextChanged(object sender, EventArgs e)
        {

        }
    }
}