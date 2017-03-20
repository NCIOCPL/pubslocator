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
    public class LiveNewUpd
    {
        #region Fields
        private int intNew;
        private int intUpdated;
        #endregion

        #region Constructors
        public LiveNewUpd(int intNew, int intUpdated)
        {
            this.intNew = intNew;
            this.intUpdated = intUpdated;
        }

        public LiveNewUpd()
        { }
        #endregion

        #region Methods
        public static bool SetLiveNewUpd(int pubid, string selectedValue, char delim)
        {
            //return PE_DAL.SetLangByPubID(pubid, selectedValue, delim);
            return true;
        }
        #endregion

        #region Properties
        public int New
        {
            get { return this.intNew; }
        }

        public int Updated
        {
            get { return this.intUpdated; }
        }

       
        #endregion

    }
}
