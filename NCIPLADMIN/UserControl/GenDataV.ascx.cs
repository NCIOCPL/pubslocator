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

using PubEntAdmin.BLL;
using PubEntAdmin.DAL;

namespace PubEntAdmin.UserControl
{
    public partial class GenDataV : System.Web.UI.UserControl
    {
        #region Fields
        private int intPubID;
        #endregion

        #region Event Handling
        protected void Page_Load(object sender, EventArgs e)
        {
            this.BindValue();
        }
        #endregion

        #region Methods
        protected void BindValue()
        {
            Pub onePub = PE_DAL.GetProdGenData(intPubID);
            this.lblCPJNum.Text = onePub.ProdID;
            this.lblShortTitle.Text = onePub.ShortTitle;
            this.lblLongTitle.Text = onePub.LongTitle;
            this.lblNIHNum.Text = onePub.NIHNum;
            this.lblFSNum.Text = onePub.FSNum;
            this.lblSpanishAccentLongTitle.Text = onePub.SpanishAccentLongTitle;
            this.lblSpanishNoAccentLongTitle.Text = onePub.SpanishNoAccentLongTitle;
            this.hylnkURL.Text = this.hylnkURL.NavigateUrl = onePub.URL;
            this.hylnkPDFURL.Text = this.hylnkPDFURL.NavigateUrl = onePub.PDFURL;
            this.hylnkKindleURL.Text = this.hylnkKindleURL.NavigateUrl = onePub.KINDLEURL;
            this.hylnkePubURL.Text = this.hylnkePubURL.NavigateUrl = onePub.EPUBURL;
            this.hylnkPrintFileURL.Text = this.hylnkPrintFileURL.NavigateUrl = onePub.PRINTFILEURL;
            this.hylnkNerdoURL.Text = this.hylnkNerdoURL.NavigateUrl = onePub.URL2;
            if (onePub.Qty >= 0)
                this.lblMaxQtyTile.Text = onePub.Qty.ToString();
            else
                this.lblMaxQtyTile.Text = String.Empty;
            if (onePub.QtyAvailable >= 0)
                this.lblQtyAvai.Text = onePub.QtyAvailable.ToString();
            else
                this.lblQtyAvai.Text = String.Empty;
            if (onePub.QtyThreshold >= 0)
                this.lblQtyThreshold.Text = onePub.QtyThreshold.ToString();
            else
                this.lblQtyThreshold.Text = String.Empty;
            this.lblBkStatus.Text = onePub.Status;
            if (onePub.Weight >= 0)
                this.lblWeight.Text = onePub.Weight.ToString();
            else
                this.lblWeight.Text = String.Empty;
            this.lblRecPubDate.Text = onePub.RecDate.CompareTo(DateTime.MinValue)!=0?onePub.RecDate.ToShortDateString():String.Empty;

            this.lblOwnervalue.Text = onePub.Owner;
            this.lblSponsorvalue.Text = onePub.Sponsor;

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

            //this.lblOrigPubDate.Text = onePub.ProdOrigDate.CompareTo(DateTime.MinValue)!=0?onePub.ProdOrigDate.ToString():String.Empty;
            //this.lblLatestPrintDate.Text = onePub.LastPrintDate.CompareTo(DateTime.MinValue)!=0?onePub.LastPrintDate.ToString():String.Empty;
            //this.lblRevPubDate.Text = onePub.LastRevDate.CompareTo(DateTime.MinValue)!=0?onePub.LastRevDate.ToString():String.Empty;
            this.lblArchiveDate.Text = onePub.ArchDate.CompareTo(DateTime.MinValue) != 0 ? onePub.ArchDate.ToShortDateString() : String.Empty;

            //QC Report
            if (onePub.ProdID.Length > 0)
                this.QCLink.NavigateUrl = "~/reportform.aspx?pubid=" + onePub.PubID;
        }
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
        #endregion

    }
}