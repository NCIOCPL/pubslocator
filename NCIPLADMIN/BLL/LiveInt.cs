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
    public class LiveInt
    {
        #region Fields
        private int intNCIPL;
        private int intROO;
        private int intExhibit;
        private int intCatalog;
        #endregion

        #region Constructors
        public LiveInt(int intNCIPL, int intROO, int intExhibit, int intCatalog)
        {
            this.intNCIPL = intNCIPL;
            this.intROO = intROO;
            this.intExhibit = intExhibit;
            this.intCatalog = intCatalog;
        }

        public LiveInt()
        { }
        #endregion

        #region Methods
        public static bool SetLiveInt(int pubid, string selectedValue, char delim)
        {
            //return PE_DAL.SetLangByPubID(pubid, selectedValue, delim);
            return true;
        }
        #endregion

        #region Properties
        public int NCIPL
        {
            get { return this.intNCIPL; }
        }

        public int ROO
        {
            get { return this.intROO; }
        }

        public int Exhibit
        {
            get { return this.intExhibit; }
        }

        public int Catalog
        {
            get { return this.intCatalog; }
        }
        #endregion

    }
}
