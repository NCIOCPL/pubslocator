using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
using System.Collections;
using PubEnt.DAL;

namespace PubEnt.BLL
{
    public class Announcement
    {
        private int _annid;
        private string _anndesc;
        private string _annurl;
        private DateTime _annsdate;
        private DateTime _annedate;

        public Announcement()
        {
        }
        public Announcement(int annid, string anndesc, string annurl, DateTime annsdate, DateTime annedate)
        {
            _annid = annid;
            _anndesc = anndesc;
            _annurl = annurl;
            _annsdate = annsdate;
            _annedate = annedate;

        }

        public int AnnID
        {
            get { return _annid; }
            //set { _annid = value; }
        }
        public string AnnDesc
        {
            get { return _anndesc; }
        }
        public string AnnUrl
        {
            get { return _annurl; }
        }
        public DateTime AnnSDate
        {
            get { return _annsdate; }
        }
        public DateTime AnnEDate
        {
            get { return _annedate; }
        }

        public string AnnMonth
        {
            get
            {
                string strMon = "";
                switch (_annsdate.Month)
                    {
                    case 1:
                            strMon = "January";
                            break;
                    case 2:
                            strMon = "February";
                            break;
                    case 3:
                            strMon = "March";
                            break;
                    case 4:
                            strMon = "April";
                            break;
                    case 5:
                            strMon = "May";
                            break;
                    case 6:
                            strMon = "June";
                            break;
                    case 7:
                            strMon = "July";
                            break;
                    case 8:
                            strMon = "August";
                            break;
                    case 9:
                            strMon = "September";
                            break;
                    case 10:
                            strMon = "October";
                            break;
                    case 11:
                            strMon = "November";
                            break;
                    case 12:
                            strMon = "December";
                            break;
                    }
                return strMon;
            }
        }

        public string AnnYear
        {
            get { return _annsdate.Year.ToString(); }
        }
        
        
        public static AnnouncementCollection GetAnnouncements()
        {
            return (PubEnt.DAL.DAL.GetAnnouncements());
        }
    }
}
