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

namespace PubEntAdmin.BLL
{
    public class Announcement : MultiSelectListBoxItem
    {
        #region Fields
        private int intAnnouncementId;
        private string strAnnouncementName;
        private string strAnnouncementURL;
        private DateTime dateStarted;
        private DateTime dateEnded;
        #endregion

        #region Constructors
        public Announcement(int announcementid, string announcementname,
            string announcementurl, DateTime dateStart, DateTime dateEnd)
            : base(announcementid, announcementname)
        {
            this.strAnnouncementURL = announcementurl;
            this.dateStarted = dateStart;
            this.dateEnded = dateEnd;
        }
        #endregion

        #region Properties
        public int AnnouncementID
        {
            get { return this.ID; }
            set { this.ID = value; }
        }

        public string AnnouncementName
        {
            get { return this.Name; }
            set { this.Name = value; }
        }

        public string AnnouncementURL
        {
            get { return this.strAnnouncementURL; }
            set { this.strAnnouncementURL = value; }
        }

        public DateTime StartDate
        {
            get { return this.dateStarted; }
            set { this.dateStarted = value; }
        }

        public DateTime EndDate
        {
            get { return this.dateEnded; }
            set { this.dateEnded = value; }
        }
        #endregion
    }
}
