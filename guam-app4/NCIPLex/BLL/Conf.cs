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
using NCIPLex.DAL;

namespace NCIPLex.BLL
{
    /// <summary>
    /// Summary description for Conf.
    /// </summary>
    public class Conf
    {
        #region Fields
        private int intConfID;
        private string strConfName;
        #endregion

        #region Constructor
        public Conf(int intConfID, string strConfName)
        {
            this.intConfID = intConfID;
            this.strConfName = strConfName;
        }
        #endregion

        #region Properties
        public int ConfID
        {
            set { this.intConfID = value; }
            get { return this.intConfID; }
        }

        public string ConfName
        {
            set { this.strConfName = value; }
            get { return this.strConfName; }
        }
        #endregion

        #region Methods
        public static Confs GetAllConf()
        {
            return SQLDataAccess.GetAllConf();
        }
        #endregion
    }
}
