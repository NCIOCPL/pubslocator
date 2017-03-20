using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using PubEntAdmin.DAL;

namespace PubEntAdmin.BLL
{
        public class Pub : MultiSelectListBoxItem
        {
            #region Fields
            private string strProdid;
            private string strLongTitle;
            private string strNIHNum1;
            private string strNIHNum2;
            private string strFSNum;
            private string strSpanishAccentLongTitle;
            private string strSpanishNoAccentLongTitle;
            private string strURL;
            private string strURL2;
            private string strPDFURL;
            private string strKINDLEURL;
            private string strEPubURL;
            private string strPRINTFILEURL;
            private int intQtyThreshold = 0;
            private bool blnNONEDITABLEFIELDSFLAG = false;
            private int intQtyAvailable = 0;
            private int intQty = 0;
            private string strStatus;
            private double intWeight = 0.0;
            private string strOwner;
            private string strSponsor;
            private string iUpdated_pub;
            private string iNew_pub;
            private DateTime dtRecDate;
            private int dtProdOrigDate_M;
            private int dtProdOrigDate_D;
            private int dtProdOrigDate_Y;
            private int dtLastPrintDate_M;
            private int dtLastPrintDate_D;
            private int dtLastPrintDate_Y;
            private int dtLastRevDate_M;
            private int dtLastRevDate_D;
            private int dtLastRevDate_Y;
            private DateTime dtArchDate;

            private string strAbstract;
            private string strKeywords;
            private string strSummary;
            private int blnCopyRight;
            private string strThumbnail;
            private string strLargeImage;
            private int intTotalNumPage = 0;
            private string strDimension;
            private string strColor;
            private string strOther;
            private string strPOSInst;

            private int intNEW;
            private int intUPDATED;
            private DateTime dtEXPDATE;

            private string strTranslation;
            private DateTime dateCreated;
            private bool blnNCIPL = false;
            private bool blnROO = false;
            private bool blnVK = false;

            //private int intRank;
            private string strRank;

            private int intOwnerID;
            private int intSponsorID;
            private bool blnOwneractive;
            private bool blnSponeractive;
            private bool blnOwnerarchvie;
            private bool blnSponerarchvie;
            #endregion

            #region Constructors
            /// <summary>
            /// Product General Data
            /// </summary>
            public Pub(int PUBID, string PRODUCTID, string SHORTTITLE, string LONGTITLE,
                string NIH_NUM1, string NIH_NUM2, string NEW_PUB, string UPDATED_PUB, string FSNUM, string SPANISH_ACCENT_LONGTITLE,
                string SPANISH_NOACCENT_LONGTITLE, string URL, string URL2, string PDFURL, string KINDLEURL, string EPUBURL, string PRINTFILEURL, bool NONEDITABLEFIELDSFLAG,
                int MAXQTY, int QUANTITY_AVAILABLE, int QUANTITY_THRESHOLD, string BookStatus,
                double WEIGHT, string OWNER_NAME, int OWNERID, bool OWNERACTIVE, bool OWNERARCHIVE, string SPONSOR_NAME, int SPONSORID, 
                bool SPONSORACTIVE, bool SPONSORARCHIVE, DateTime RECEIVED_DATE, 
                int PUBORIGDATE_M, int PUBORIGDATE_D, int PUBORIGDATE_Y,
                int LAST_PRINT_DATE_M, int LAST_PRINT_DATE_D, int LAST_PRINT_DATE_Y,
                int LAST_REVISED_DATE_M, int LAST_REVISED_DATE_D, int LAST_REVISED_DATE_Y,
                DateTime ARCHIVED_DATE)
                : base(PUBID, SHORTTITLE)
            {
                this.strProdid = PRODUCTID;
                this.strLongTitle = LONGTITLE;
                this.strNIHNum1 = NIH_NUM1;
                this.strNIHNum2 = NIH_NUM2;
                this.strFSNum = FSNUM;
                this.strSpanishAccentLongTitle = SPANISH_ACCENT_LONGTITLE;
                this.strSpanishNoAccentLongTitle = SPANISH_NOACCENT_LONGTITLE;
                this.strURL = URL;
                this.strURL2 = URL2;
                this.strPDFURL = PDFURL;
                this.strKINDLEURL = KINDLEURL;
                this.strEPubURL = EPUBURL;
                this.strPRINTFILEURL = PRINTFILEURL;
                this.QtyThreshold = QUANTITY_THRESHOLD;
                this.blnNONEDITABLEFIELDSFLAG = NONEDITABLEFIELDSFLAG;
                this.intQty = MAXQTY;
                this.intQtyAvailable = QUANTITY_AVAILABLE;
                this.strStatus = BookStatus;
                this.intWeight = WEIGHT;
                this.strOwner = OWNER_NAME;
                this.intOwnerID = OWNERID;
                this.blnOwneractive = OWNERACTIVE;
                this.blnOwnerarchvie = OWNERARCHIVE;
                this.blnSponeractive = SPONSORACTIVE;
                this.blnSponerarchvie = SPONSORARCHIVE;
                this.intSponsorID = SPONSORID;
                this.strSponsor = SPONSOR_NAME;
                this.iNew_pub = NEW_PUB;
                this.iUpdated_pub = UPDATED_PUB;
                this.dtRecDate = RECEIVED_DATE;
                this.dtProdOrigDate_M = PUBORIGDATE_M;
                this.dtProdOrigDate_D = PUBORIGDATE_D;
                this.dtProdOrigDate_Y = PUBORIGDATE_Y;
                this.dtLastPrintDate_M = LAST_PRINT_DATE_M;
                this.dtLastPrintDate_D = LAST_PRINT_DATE_D;
                this.dtLastPrintDate_Y = LAST_PRINT_DATE_Y;
                this.dtLastRevDate_M = LAST_REVISED_DATE_M;
                this.dtLastRevDate_D = LAST_REVISED_DATE_D;
                this.dtLastRevDate_Y = LAST_REVISED_DATE_Y;
                this.dtArchDate = ARCHIVED_DATE;
            }

            /// <summary>
            /// Product Common Data
            /// </summary>
            /// <param name="pubid"></param>
            /// <param name="?"></param>
            public Pub(int pubid, string SHORTTITLE, string _abstract, string keywords,
                string summary, int copyright, string thumbnail, string largeimage, int totalnumpage,
                string dimension, string color, string other, string POSInst,
                int NEW, int UPDATED, DateTime EXPDATE)
                : base(pubid, SHORTTITLE)
            {
                this.strAbstract = _abstract;
                this.strKeywords = keywords;
                this.strSummary = summary;
                this.blnCopyRight = copyright;
                this.strThumbnail = thumbnail;
                this.strLargeImage = largeimage;
                this.intTotalNumPage = totalnumpage;
                this.strDimension = dimension;
                this.strColor = color;
                this.strOther = other;
                this.strPOSInst = POSInst;

                this.intNEW = NEW;
                this.intUPDATED = UPDATED;
                this.dtEXPDATE = EXPDATE;

            }

            /// <summary>
            /// SearchResult
            /// </summary>
            public Pub(int pubid, string prodid, string name, int qtyAvailable,
                string status, DateTime dateCreated, string rank)
                : this(pubid, prodid, name, qtyAvailable,
                status, dateCreated)
            {
                this.strRank = rank;
            }

            public Pub(int pubid, string prodid, string name, int qtyAvailable,
                string status, DateTime dateCreated)
                : base(pubid, name)
            {
                this.strProdid = prodid;
                this.intQtyAvailable = qtyAvailable;
                this.strStatus = status;
                this.dateCreated = dateCreated;
                
            }

            /// <summary>
            /// Get all vk and lp
            /// </summary>
            public Pub(int pubid, string prodid, string name, bool NCIPL, bool ROO, bool VK_LP)
                : base(pubid, name)
            {
                this.strProdid = prodid;
                this.blnNCIPL = NCIPL;
                this.blnROO = ROO;
                this.blnVK = VK_LP;
            }
            /// <summary>
            /// constructor for kit pub info display
            /// </summary>
            public Pub(int pubid, string prodid, string name)
                : base(pubid, name)
            {
                this.strProdid = prodid;
            }

            public Pub(int pubid, string prodid, string name, int qty)
                : this(pubid, prodid, name)
            {
                this.intQty = qty;
            }
            /// <summary>
            /// for translation
            /// </summary>
            public Pub(int pubid, string prodid, string name, string translation)
                : base(pubid, name)
            {
                this.strProdid = prodid;
                this.strTranslation = translation;

            }

            public Pub(int pubid, string name)
                : base(pubid, name) { }

            public Pub() { }

            #endregion

            #region Properties
            
            public int PubID
            {
                get { return this.ID; }
                set { this.ID = value; }
            }

            public string ProdID
            {
                get { return this.strProdid; }
                set { this.strProdid = value; }
            }

            public string PubName
            {
                get { return this.Name; }
                set { this.Name = value; }
            }

            public string ShortTitle
            {
                get { return this.PubName; }
                set { this.PubName = value; }
            }

            public string LongTitle
            {
                get {
                    if (this.strLongTitle != null)
                        return this.strLongTitle;
                    else
                        return String.Empty;
                }
                set { this.PubName = value; }
            }

            public string NIHNum1
            {
                get
                {
                    if (this.strNIHNum1 != null)
                        return this.strNIHNum1;
                    else
                        return String.Empty;
                }
                set { this.strNIHNum1 = value; }
            }

            public string NIHNum2
            {
                get
                {
                    if (this.strNIHNum2 != null)
                        return this.strNIHNum2;
                    else
                        return String.Empty;
                }
                set { this.strNIHNum2 = value; }
            }

            public string NIHNum
            {
                get
                {
                    if (this.strNIHNum1 != null && this.strNIHNum2 != null)
                        return this.strNIHNum1 + " - " + this.strNIHNum2;
                    else
                        return String.Empty;
                }
            }

            public string FSNum
            {
                get
                {
                    if (this.strFSNum != null)
                        return this.strFSNum;
                    else
                        return String.Empty;
                }
                set { this.strFSNum = value; }
            }

            public string New_Pub
            {
                get { return this.iNew_pub; }
                set { this.iNew_pub = value; }
            }

            public string Updated_Pub
            {
                get { return this.iUpdated_pub; }
                set { this.iUpdated_pub = value; }
            }

            public string SpanishAccentLongTitle
            {
                get
                {
                    if (this.strSpanishAccentLongTitle != null)
                        return this.strSpanishAccentLongTitle;
                    else
                        return String.Empty;
                }
                set { this.strSpanishAccentLongTitle = value; }
            }

            public string SpanishNoAccentLongTitle
            {
                get
                {
                    if (this.strSpanishNoAccentLongTitle != null)
                        return this.strSpanishNoAccentLongTitle;
                    else
                        return String.Empty;
                }
                set { this.strSpanishNoAccentLongTitle = value; }
            }

            public string URL
            {
                get
                {
                    if (this.strURL != null)
                        return this.strURL;
                    else
                        return String.Empty;
                }
                set { this.strURL = value; }
            }

            public string URL2
            {
                get
                {
                    if (this.strURL2 != null)
                        return this.strURL2;
                    else
                        return String.Empty;
                }
                set { this.strURL2 = value; }
            }

            public string PDFURL
            {
                get
                {
                    if (this.strPDFURL != null)
                        return this.strPDFURL;
                    else
                        return String.Empty;
                }
                set { this.strPDFURL = value; }
            }
            public string KINDLEURL
            {
                get
                {
                    if (this.strKINDLEURL != null)
                        return this.strKINDLEURL;
                    else
                        return String.Empty;
                }
                set { this.strKINDLEURL = value; }
            }
            public string EPUBURL
            {
                get
                {
                    if (this.strEPubURL != null)
                        return this.strEPubURL;
                    else
                        return String.Empty;
                }
                set { this.strEPubURL = value; }
            }
            public string PRINTFILEURL
            {
                get
                {
                    if (this.strPRINTFILEURL != null)
                        return this.strPRINTFILEURL;
                    else
                        return String.Empty;
                }
                set { this.strPRINTFILEURL = value; }
            }
            public bool NONEDITABLEFIELDSFLAG
            {
                get
                {
                    return this.blnNONEDITABLEFIELDSFLAG;
                }
                set { this.blnNONEDITABLEFIELDSFLAG = value; }
            }

            public int QtyAvailable
            {
                get 
                {
                    return this.intQtyAvailable;
                }
                set { this.intQtyAvailable = value; }
            }
            public int QtyThreshold
            {
                get
                {
                    return this.intQtyThreshold ;
                }
                set { this.intQtyThreshold = value; }
            }
            public int Qty
            {
                get {
                        return this.intQty;
                }
                set { this.intQty = value; }
            }

            public string Status
            {
                get 
                {
                    if (this.strStatus != null)
                        return this.strStatus;
                    else
                        return String.Empty;
                }
                set { this.strStatus = value; }
            }

            public double Weight
            {
                get 
                {
                    return this.intWeight;
                }
                set { this.intWeight = value; }
            }

            public string Sponsor
            {
                get
                {
                    if (this.strSponsor != null)
                        return this.strSponsor;
                    else
                        return String.Empty;
                }
                set { this.strSponsor = value; }
            }

            public int SponsorID
            {
                get
                {
                    return this.intSponsorID;
                }
                set { this.intSponsorID = value; }
            }

            public bool SponsorActive
            {
                get { return this.blnSponeractive; }
                set { this.blnOwneractive = value; }
            }

            public bool SponsorArchvie
            {
                get { return this.blnSponerarchvie; }
                set { this.blnSponerarchvie = value; }
            }

            public string Owner
            {
                get
                {
                    if (this.strOwner != null)
                        return this.strOwner;
                    else
                        return String.Empty;
                }
                set { this.strOwner = value; }
            }

            public int OwnerID
            {
                get
                {
                    return this.intOwnerID;
                }
                set { this.intOwnerID = value; }
            }

            public bool OwnerActive
            {
                get { return this.blnOwneractive; }
                set { this.blnOwneractive = value; }
            }

            public bool OwnerArcchive
            {
                get { return this.blnOwnerarchvie; }
                set { this.blnOwnerarchvie = value; }
            }
            

            public string PubLang
            {
                get 
                {
                    if (this.strTranslation != null)
                        return this.strTranslation;
                    else
                        return String.Empty;
                }
                set { this.strTranslation = value; }
            }

            public DateTime CreateDate
            {
                get 
                {
                    if (this.dateCreated != null)
                        return this.dateCreated;
                    else
                        return DateTime.MinValue;
                }
                set { this.dateCreated = value; }
            }

            public DateTime RecDate
            {
                get 
                {
                    if (this.dtRecDate.ToShortDateString().CompareTo("1/1/1900") != 0)
                        return this.dtRecDate;
                    else
                        return DateTime.MinValue;
                }
                set { this.dtRecDate = value; }
            }

            public int ProdOrigDate_M
            {
                get
                {
                   return this.dtProdOrigDate_M;
                }
                set { this.dtProdOrigDate_M = value; }
            }

            public int ProdOrigDate_D
            {
                get
                {
                    return this.dtProdOrigDate_D;
                }
                set { this.dtProdOrigDate_D = value; }
            }

            public int ProdOrigDate_Y
            {
                get
                {
                    return this.dtProdOrigDate_Y;
                }
                set { this.dtProdOrigDate_Y = value; }
            }

            public int LastPrintDate_M
            {
                get
                {
                    return this.dtLastPrintDate_M;
                }
                set { this.dtLastPrintDate_M = value; }
            }

            public int LastPrintDate_D
            {
                get
                {
                    return this.dtLastPrintDate_D;
                }
                set { this.dtLastPrintDate_D = value; }
            }

            public int LastPrintDate_Y
            {
                get
                {
                    return this.dtLastPrintDate_Y;
                }
                set { this.dtLastPrintDate_Y = value; }
            }

            public int LastRevDate_M
            {
                get
                {
                    return this.dtLastRevDate_M;
                }
                set { this.dtLastRevDate_M = value; }
            }

            public int LastRevDate_D
            {
                get
                {
                    return this.dtLastRevDate_D;
                }
                set { this.dtLastRevDate_D = value; }
            }

            public int LastRevDate_Y
            {
                get
                {
                    return this.dtLastRevDate_Y;
                }
                set { this.dtLastRevDate_Y = value; }
            }

            public DateTime ArchDate
            {
                get
                {
                    if (this.dtArchDate.ToShortDateString().CompareTo("1/1/1900") != 0)
                        return this.dtArchDate;
                    else
                        return DateTime.MinValue;
                }
                set { this.dtArchDate = value; }
            }

            public string Description
            {
                get
                {
                    if (this.strAbstract != null)
                        return this.strAbstract;
                    else
                        return String.Empty;
                }
                set { this.strAbstract = value; }
            }

            public string Keywords
            {
                get
                {
                    if (this.strKeywords != null)
                        return this.strKeywords;
                    else
                        return String.Empty;
                }
                set { this.strKeywords = value; }
            }

            public string Summary
            {
                get
                {
                    if (this.strSummary != null)
                        return this.strSummary;
                    else
                        return String.Empty;
                }
                set { this.strSummary = value; }
            }
            
            public int IsCopyRight
            {
                get { return this.blnCopyRight; }
                set { this.blnCopyRight = value; }
            }

            public string Thumbnail
            {
                get
                {
                    if (this.strThumbnail != null)
                        return this.strThumbnail;
                    else
                        return String.Empty;
                }
                set { this.strThumbnail = value; }
            }

            public string LargeImage
            {
                get
                {
                    if (this.strLargeImage != null)
                        return this.strLargeImage;
                    else
                        return String.Empty;
                }
                set { this.strLargeImage = value; }
            }
            
            public int TotalNumPage
            {
                get 
                {
                    return this.intTotalNumPage;
                }
                set { this.intTotalNumPage = value; }
            }

            public string Dimension
            {
                get
                {
                    if (this.strDimension != null)
                        return this.strDimension;
                    else
                        return String.Empty;
                }
                set { this.strThumbnail = value; }
            }

            public string Color
            {
                get
                {
                    if (this.strColor != null)
                        return this.strColor;
                    else
                        return String.Empty;
                }
                set { this.strColor = value; }
            }

            public string Other
            {
                get
                {
                    if (this.strOther != null)
                        return this.strOther;
                    else
                        return String.Empty;
                }
                set { this.strOther = value; }
            }

            public string POSInst
            {
                get
                {
                    if (this.strPOSInst != null)
                        return this.strPOSInst;
                    else
                        return String.Empty;
                }
                set { this.strPOSInst = value; }
            }

            public bool IsNCIPL
            {
                get { return this.blnNCIPL; }
                set { this.blnNCIPL = value; }
            }

            public bool IsROO
            {
                get { return this.blnROO; }
                set { this.blnROO = value; }
            }

            public bool IsVK
            {
                get { return this.blnVK; }
                set { this.blnVK = value; }
            }

            public bool IsLP
            {
                get
                {
                    return !this.blnVK;
                }
                set { this.blnVK = !(value); }
            }

            public string Rank
            {
                get { return this.strRank; }
                set { this.strRank = value; }
            }

            public int NEW
            {
                get { return this.intNEW; }
                set { this.intNEW = value; }
            }

            public int UPDATED
            {
                get { return this.intUPDATED; }
                set { this.intUPDATED = value; }
            }

            public DateTime EXPDATE
            {
                get
                {
                    if (this.dtEXPDATE.ToShortDateString().CompareTo("1/1/1900") != 0)
                        return this.dtEXPDATE;
                    else
                        return DateTime.MinValue;
                }
                set { this.dtEXPDATE = value; }
            }
            #endregion

            #region Methods
            //public static PubCollection GetSearchResult()
            //{
            //    return (PE_DAL.GetSearchResult());
            //}
            #endregion
        }
}
