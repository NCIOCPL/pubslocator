using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//Manually added
using System.Collections;

using PubEnt.BLL;
using PubEnt.DAL;

namespace PubEnt.BLL
{
    [Serializable]
    public class Product
    {
      
        //Private Fields
        private int _pubid;
        private string _productid = "";
        private string _bookstatusall = ""; //retrive from db
        private string _bookstatus = ""; //Only one book status is allowed at this time
        private string _displaystatusall = ""; //retrive from db
        private string _displaystatusonline = ""; //compute and assign
        private string _displaystatusorder = ""; //compute and assign
        private string _displaystatusnerdo = ""; //compute and assign
        private string _longtitle = "";
        private string _shorttitle = "";
        private string _abstract = "";
        private string _summary = "";
        private DateTime _dtrecordcreatedate; 
        private string _recordupdatedate;
        private DateTime _dtrecordupdatedate; //recordupdateddate as date
        private int _revisedmonth;
        private int _revisedday;
        private int _revisedyear;
        private DateTime _dtreviseddate; 
        private int _isonline;
        private string _url = "";
        private string _urlnerdo = ""; //URL2 - Nerdo Content URL
        
        //private int _orderdisplaystatus; //Business logic needed
        //private int _onlinedisplaystatus; //Business logic needed
        //private int _nerdodisplaystatus; //Business logic needed

        private string _pubimage = "";
        private int _numqtyavailable; //Available quantity
        private int _numqtyordered; //Quantity ordered for the item
        private int _numqtylimit; //Max Quantitly allowed

        private string _pubidcover;
        private string _productidcover = "";
        private string _bookstatuscoverall = ""; //retrieve from db
        private string _bookstatuscover = ""; //Only one book status is allowed at this time
        private string _displaystatuscoverall = ""; //retrieve from db
        private string _displaystatuscoveronline = ""; //compute and assign
        private string _displaystatuscoverorder = ""; //compute and assign
        private string _displaystatuscovernerdo = ""; //compute and assign
        private string _urlcover = "";
        private int __numqtycoveravailable;
        private int _numqtycoverordered;
        private int _numqtycoverlimit;

        private string _audience = "";
        private string _awards = "";
        private string _cancertype = "";
        private string _language = "";
        private string _format = "";
        private string _series = "";
        private string _subject = "";
        private string _nihnum = "";

        private string _pubstatus = ""; //HITT 8300
        private string _datetype = ""; //HITT 8516 

        //variables to save properties that will be checked in Presentation Layer Code
        private int _canorder;
        private string _ordermsg = "";
        private int _canview;
        private int _canordercover;
        private string _covermsg = "";
        private int _numpages;
        //end variables to save properties that will be checked in Presentation Layer Code

        //For Kits
        private int _kitid;

        private string _pdfurl = ""; //CR 10-001-28
        private string _publargeimage = ""; //CR 10-001-28
        private string _pubfeaturedimage = ""; //CR 10-001-28

        private double _weight;
        public Product() //Defautl Constructor
        {
        }

        public Product(int pubid, int numqtyordered) //Defautl Constructor
        {
            _pubid = pubid;
            _numqtyordered = numqtyordered;
        }

        public Product(int kitid, int pubid, int numqtyordered)
        {
            _kitid = kitid;
            _pubid = pubid;
            _numqtyordered = numqtyordered;

        }

        public Product(int pubid, string productid, string longtitle, int numqtyordered)
        {
            _pubid = pubid;
            _productid = productid;
            _longtitle = longtitle;
            _numqtyordered = numqtyordered;
        }

        //***EAC Specifically for shopping cart pubs
        public Product(int pubid, string productid, string longtitle, int numqtylimit, double weight, string bookstatus)
        {
            _pubid = pubid;
            _productid = productid;
            _longtitle = longtitle;
            _numqtylimit = numqtylimit; //Max Quantitly allowed
            _weight = weight;
            _bookstatus = bookstatus; //***EAC 20120509 we are assuming only 1 bookstatus per pub
        }
        public Product(int pubid, string productid, string bookstatusall,
                        string displaystatusall, string longtitle, string shorttitle, string url, string pubimage)
        {
            _pubid = pubid;
            _productid = productid;
            _bookstatusall = bookstatusall; //retrive from db
            //Only one book status is allowed at this time
           
                if (GetAllStatus(bookstatusall) != null) //yma
                {
                    if (GetAllStatus(bookstatusall).Length > 0)
                    {
                        _bookstatus = GetAllStatus(bookstatusall)[0].ToString();
                    }
                }
           
            _displaystatusall = displaystatusall; //retrive from db
            foreach (string currStatus in GetAllStatus(displaystatusall))
            {
                if (string.Compare(currStatus, "ONLINE", true) == 0)
                    _displaystatusonline = "ONLINE";

                if (string.Compare(currStatus, "ORDER", true) == 0)
                    _displaystatusorder = "ORDER";

                if (string.Compare(currStatus, "NERDO", true) == 0)
                    _displaystatusnerdo = "NERDO";
            }
            _longtitle = longtitle;
            _shorttitle = shorttitle;
            _url = url;
            _pubimage = pubimage;
        }
        
        //For ProductTranslation CR 30 : HITT 8719
        public Product(int pubid, string productid, string bookstatusall,
                        string displaystatusall, string language)
        {
            _pubid = pubid;
            _productid = productid;
            _bookstatusall = bookstatusall; //retrive from db
            //Only one book status is allowed at this time
            if (GetAllStatus(bookstatusall) != null) //yma
            {
                if (GetAllStatus(bookstatusall).Length > 0)
                {
                    _bookstatus = GetAllStatus(bookstatusall)[0].ToString();
                }
            }
            _displaystatusall = displaystatusall; //retrive from db
            foreach (string currStatus in GetAllStatus(displaystatusall))
            {
                if (string.Compare(currStatus, "ONLINE", true) == 0)
                    _displaystatusonline = "ONLINE";

                if (string.Compare(currStatus, "ORDER", true) == 0)
                    _displaystatusorder = "ORDER";

                if (string.Compare(currStatus, "NERDO", true) == 0)
                    _displaystatusnerdo = "NERDO";
            }
            _language = language;
        }

        public Product(int pubid,
                        string productid,
                        string bookstatusall,
                        string displaystatusall,
                        string longtitle,
                        string shorttitle,
                        string productabstract,
                        string summary,
                        string recordupdatedate,
                        DateTime dtrecordupdatedate,
                        int isonline,
                        string url,
                        string urlnerdo,
                        string pubimage,
                        int numqtyavailable,
                        int numqtylimit,
                        string pubidcover,
                        string productidcover,
                        string bookstatuscoverall,
                        string displaystatuscoverall,
                        string urlcover,
                        int numqtycoveravailable,
                        int numqtycoverlimit,
                        string audience,
                        string awards,
                        string cancertype,
                        string language,
                        string format,
                        string series,
                        string subject,
                        string nihnum,
                        DateTime dtrecordcreatedate,
                        int revisedmonth,
                        int revisedday,
                        int revisedyear,
                        DateTime dtreviseddate,
                        string datetype,
                        int numpages,
                        string pubstatus,
                        string pdfurl,
                        string publargeimage,
                        string pubfeaturedimage
            )
        {

            _pubid = pubid;
            _productid = productid;
            _bookstatusall = bookstatusall; //retrive from db
            //Only one book status is allowed at this time
            if (GetAllStatus(bookstatusall).Length > 0)
            {
                _bookstatus = GetAllStatus(bookstatusall)[0].ToString();
            }

            _displaystatusall = displaystatusall; //retrive from db

            if (_displaystatusall != ""){  //yma fix this
            foreach (string currStatus in GetAllStatus(displaystatusall)) 
            {
                if (string.Compare(currStatus, "ONLINE", true) == 0)
                    _displaystatusonline = "ONLINE";

                if (string.Compare(currStatus, "ORDER", true) == 0)
                    _displaystatusorder = "ORDER";

                if (string.Compare(currStatus, "NERDO", true) == 0)
                    _displaystatusnerdo = "NERDO";
            }
            }
            
             //compute and assign
            
            _longtitle = longtitle;
            _shorttitle = shorttitle;
            _abstract = productabstract;
            _summary = summary;
            _recordupdatedate = recordupdatedate;
            _dtrecordupdatedate = dtrecordupdatedate; //recordupdateddate as date
            _isonline = isonline;
            _url = url;
            _urlnerdo = urlnerdo; //URL2 - Nerdo Content URL

            //private int _orderdisplaystatus; //Business logic needed
            //private int _onlinedisplaystatus; //Business logic needed
            //private int _nerdodisplaystatus; //Business logic needed

            _pubimage = pubimage;
            _numqtyavailable = numqtyavailable; //Available quantity
            //_numqtyordered = 0; //Quantity ordered for the item
            _numqtylimit = numqtylimit; //Max Quantitly allowed

            _pubidcover = pubidcover;
            _productidcover = productidcover;
            _bookstatuscoverall = bookstatuscoverall; //retrieve from db
            if (GetAllStatus(bookstatuscoverall) != null) //yma
            {
                if (GetAllStatus(bookstatuscoverall).Length > 0)
                {
                    _bookstatuscover = GetAllStatus(bookstatuscoverall)[0].ToString();
                }
            }
            //_bookstatuscover; //Only one book status is allowed at this time
            _displaystatuscoverall = displaystatuscoverall; //retrieve from db
            if (GetAllStatus(displaystatuscoverall) != null) //yma
            {
                foreach (string currStatus in GetAllStatus(displaystatuscoverall))
                {
                    if (string.Compare(currStatus, "ONLINE", true) == 0)
                        _displaystatuscoveronline = "ONLINE";

                    if (string.Compare(currStatus, "ORDER", true) == 0)
                        _displaystatuscoverorder = "ORDER";

                    if (string.Compare(currStatus, "NERDO", true) == 0)
                        _displaystatuscovernerdo = "NERDO";
                }
            }
            //_displaystatuscoveronline; //compute and assign
            //_displaystatuscoverorder; //compute and assign
            //_displaystatuscovernerdo; //compute and assign
            _urlcover = urlcover;
            __numqtycoveravailable = numqtycoveravailable;
            //_numqtycoverordered;
            _numqtycoverlimit = numqtycoverlimit;

            _audience = audience.Replace("^~", ", ");
            _awards = awards.Replace("^~", ", ");
            _cancertype = cancertype.Replace("^~", ", ");
            _language = language.Replace("^~", ", ");
            _format = format.Replace("^~", ", ");
            _series = series.Replace("^~", ", ");
            _subject = subject.Replace("^~", ", ");
            //_nihnum = nihnum.Replace("^~", ", ");
            /***********/
            //Some checks specific for nih number
            //if (_nihnum.Contains("-,"))
            //    _nihnum = "";
            if (nihnum.Length > 0)
            {
                string tmpdelimStr = "^~"; //Fixed delimitter
                char[] tmpdelimiter = tmpdelimStr.ToCharArray();
                string[] tmpallstatus = nihnum.Split(tmpdelimiter);
                for (int counter = 0; counter < tmpallstatus.Length; counter++)
                {
                    if (tmpallstatus[counter].Length > 1)
                    {
                        if (_nihnum.Contains(tmpallstatus[counter]) == false) //Add if it is not already there
                        {
                            if (_nihnum.Length == 0)
                                _nihnum = tmpallstatus[counter];
                            else
                                _nihnum = _nihnum + ", " + tmpallstatus[counter];
                        }
                        
                    }
                }
            }
            /***********/

            _dtrecordcreatedate = dtrecordcreatedate;
            _revisedmonth = revisedmonth;
            _revisedday = revisedday;
            _revisedyear = revisedyear;
            _dtreviseddate = dtreviseddate;

            _numpages = numpages;
            _pubstatus = pubstatus;
            _datetype = datetype;

            _pdfurl = pdfurl; //CR 10-001-28
            _publargeimage = publargeimage; //CR 10-001-28
            _pubfeaturedimage = pubfeaturedimage; //CR 10-001-28
        }

        //Properties
        public int PubId
        {
            get { return _pubid; }
        }

        public string ProductId
        {
            get { return _productid; }
        }

        public string BookStatus
        {
            get { return _bookstatus; }
        }

        public string OnlineDisplayStatus
        {
            get { return _displaystatusonline; }
        }

        public string OrderDisplayStatus
        {
            get { return _displaystatusorder; }
        }

        public string NerdoDisplayStatus
        {
            get { return _displaystatusnerdo; }
        }


        public string LongTitle
        {
            get { return _longtitle; }
        }

        public string ShortTitle
        {
            get { return _shorttitle; }
        }

        public string Abstract
        {
            get { return _abstract; }
        }

        public string Summary
        {
            get { return _summary; }
        }

        public string RecordUpdateDate
        {
            get { return _recordupdatedate; }
        }

        public DateTime dtRecordUpdateDate
        {
            get { return _dtrecordupdatedate; }
        }

        public int IsOnline
        {
            get { return _isonline; }
        }

        public string Url
        {
            get { return _url; }
        }

        public string UrlNerdo
        {
            get { return _urlnerdo; }
        }

        public string PubImage
        {
            get { return _pubimage; }
        }

        public int NumQtyAvailable
        {
            get { return _numqtyavailable; }
        }

        public int NumQtyOrdered
        {
            get { return _numqtyordered; }
            set { _numqtyordered = value; }
        }

        public int NumQtyLimit
        {
            get { return _numqtylimit; }
        }

        public string PubIdCover
        {
            get { return _pubidcover; }
        }

        public string ProductIdCover
        {
            get { return _productidcover; }
        }

        public string BookStatusCover
        {
            get { return _bookstatuscover; }
        }

        public string OnlineDisplayStatusCover
        {
            get { return _displaystatuscoveronline; }
        }

        public string OrderDisplayStatusCover
        {
            get { return _displaystatuscoverorder; }
        }

        public string NerdoDisplayStatusCover
        {
            get { return _displaystatuscovernerdo; }
        }

        public string UrlCover
        {
            get { return _urlcover; }
        }

        public int NumQtyAvailableCover
        {
            get { return __numqtycoveravailable; }
        }

        public int NumQtyOrderedCover
        {
            get { return _numqtycoverordered; }
            set { _numqtycoverordered = value; }
        }

        public int NumQtyLimitCover
        {
            get { return _numqtycoverlimit; }
        }

        public string Audience
        {
            get { return _audience; }
        }

        public string Awards
        {
            get { return _awards; }
        }

        public string CancerType
        {
            get { return _cancertype; }
        }

        public string Language
        {
            get { return _language; }
        }

        public string Format
        {
            get { return _format; }
        }

        public string Series
        {
            get { return _series; }
        }

        public string Subject
        {
            get { return _subject; }
        }
        public string NIHNum
        {
            get { return _nihnum; }
        }
        public string NumPages
        {
            get {
                    string strnumpages = "";
                    if (_numpages > 0)
                        strnumpages = _numpages.ToString();
                    return strnumpages;
                }
        }

        //Properties used in the PE code
        public int CanOrder
        {
            get {
                if (_numqtyavailable > 0)
                    return 1;
                else
                    return 0;
            }
            set { _canorder = value; }
        }

        public string OrderMsg
        {
            get { return _ordermsg; }
            set { _ordermsg = value; }
        }

        [Obsolete]
        public int CanView  //***EAC This is deprecated (20130503)
        {
            get { return _canview; }
            set { _canview = value; }
        }

        public int CanOrderCover
        {
            get { return _canordercover; }
            set { _canordercover = value; }
        }

        public string CoverMsg
        {
            get { return _covermsg; }
            set { _covermsg = value; }
        }

        public DateTime dtRecordCreateDate
        {
            get { return _dtrecordcreatedate; }
        }

        public string RevisedMonth
        {
            get {
                string revMon = "";
                if (_revisedmonth == 0)
                    revMon = "";
                else
                {
                    switch (_revisedmonth)
                    {
                        case 1:
                            revMon = "January";
                            break;
                        case 2:
                            revMon = "February";
                            break;
                        case 3:
                            revMon = "March";
                            break;
                        case 4:
                            revMon = "April";
                            break;
                        case 5:
                            revMon = "May";
                            break;
                        case 6:
                            revMon = "June";
                            break;
                        case 7:
                            revMon = "July";
                            break;
                        case 8:
                            revMon = "August";
                            break;
                        case 9:
                            revMon = "September";
                            break;
                        case 10:
                            revMon = "October";
                            break;
                        case 11:
                            revMon = "November";
                            break;
                        case 12:
                            revMon = "December";
                            break;
                    }
                }
                return revMon; 
            }
        }

        public string RevisedDay
        {
            get
            {
                if (_revisedday == 0)
                    return "";
                else
                    return _revisedday.ToString();
            }
        }

        public string RevisedYear
        {
            get
            {
                if (_revisedyear == 0)
                    return "";
                else
                    return _revisedyear.ToString();
            }
        }

        public DateTime dtRevisedDate
        {
            get { return _dtreviseddate; }
        }

        public String RevisedDateType
        {
            get
            {
                string typeofdate = "";
                switch (_datetype)
                {
                    case "O":
                        typeofdate = "Date:&nbsp;";
                        break;
                    case "P":
                        typeofdate = "Last Printed:&nbsp;";
                        break;
                    case "R":
                        typeofdate = "Last Revised:&nbsp;";
                        break;
                }
                return typeofdate;
            }
        }

        public string NewOrUpdated
        {
            get { return _pubstatus; }
            //HITT 8300 get { return GetPubNewOrUpdatedStatus(_dtrecordcreatedate, _dtrecordupdatedate); }
            //HITT 8217 get { return GetPubNewOrUpdatedStatus(_dtrecordcreatedate, _dtreviseddate); }
        }

        public int KitId //Used for Kits
        {
            get { return _kitid; }
        }

        public string PDFUrl
        {
            get { return _pdfurl; }
        }

        public string PubLargeImage
        {
            get { return _publargeimage; }
        }
        
        public string PubFeaturedImage
        {
            get { return _pubfeaturedimage; }
        }

        public double Weight
        {
            get { return _weight; }
            set { _weight = value; }
        }
        //End Properties used in the PE code

        //STATIC METHODS
        
        //public static ProductCollection GetAllProducts(string SortField)
        //{
        //    //DataAccess DbLayer = DataAccessBaseClassHelper.GetDataAccesLayer();
        //    return (PubEnt.DAL.DAL.GetAllProducts(SortField));
        //    //return DAL.SQLDataAccess.GetAllProducts();
        //}
        public static Product GetPubByProductID(string productid)
        {
            return (PubEnt.DAL.DAL.GetProductbyProductID(productid));
        }
        public static Product GetPubByPubID(int pubid)
        {
            return (PubEnt.DAL.DAL.GetProductbyPubID(pubid));
        }
        public static Product GetCartPubByPubID(int pubid)
        {
            return (PubEnt.DAL.DAL.GetCartPubByPubID(pubid));
        }


        private string[] GetAllStatus(string combinedStr)
        {
            //yma fix this oject reference not set to an instance of and object 
            if (combinedStr != null)
            {
                string delimStr = "^~"; //Fixed delimitter
                char[] delimiter = delimStr.ToCharArray();
                string[] allstatus = combinedStr.Split(delimiter);
                return allstatus;
            }
            else
                return null;
        }
        
        //Returns "NEW" or "UPDATED" value for the Pub
        private string GetPubNewOrUpdatedStatus(DateTime dtrecCreate, DateTime dtrecUpdate)
        {
            string returnvalue = "";
            
            if (dtrecCreate == DateTime.MinValue && dtrecUpdate == DateTime.MinValue) //Unlikely
            {
                returnvalue = "";
                return returnvalue;
            }
            
            DateTime currdt = DateTime.Now;

            TimeSpan tsDiffCreate = currdt.Subtract(dtrecCreate);
            TimeSpan tsDiffUpdate = currdt.Subtract(dtrecUpdate);

            if (tsDiffCreate.Days <= 180) //HITT 8217 Check "NEW" first then "UPDATED"
            {
                returnvalue = "NEW";
                if (tsDiffUpdate.Days <= 180)
                {
                    int result = DateTime.Compare(dtrecCreate, dtrecUpdate);
                    if (result >= 0)
                        returnvalue = "NEW";
                    else
                        returnvalue = "UPDATED";
                }
            }
            else if (tsDiffUpdate.Days <= 180)
            {
                returnvalue = "UPDATED";
            }
            else
                returnvalue = "";

            return returnvalue;
        }
    }
}
