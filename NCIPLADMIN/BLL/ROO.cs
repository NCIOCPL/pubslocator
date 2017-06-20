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
    public class ROO : MultiSelectListBoxItem
    {
        private int intMAXQTY_ROO;
        private int blnEVERYORDER_ROO;
        private int blnISSEARCHABLE_ROO;
        //private int blnISNEW;
        //private int blnISUPDATED;
        private int blnISROO_KIT;
        private int blnROOCOMMONLIST;
        private int intFK_COMMONLISTSUBJ;

        //public ROO(int pubid, string name,
        //    int MAXQTY_ROO, bool EVERYORDER_ROO,
        //    bool ISSEARCHABLE_ROO, bool ROOCOMMONLIST, int FK_COMMONLISTSUBJ)
        //    : base(pubid, name)
        //{
        //    this.intMAXQTY_ROO = MAXQTY_ROO;
        //    this.blnEVERYORDER_ROO = EVERYORDER_ROO;
        //    this.blnISSEARCHABLE_ROO = ISSEARCHABLE_ROO;
        //    this.blnROOCOMMONLIST = ROOCOMMONLIST;
        //    this.intFK_COMMONLISTSUBJ = FK_COMMONLISTSUBJ;
        //}

        public ROO(int pubid, string name,
            int MAXQTY_ROO, int EVERYORDER_ROO,
            int ISSEARCHABLE_ROO, int ISROO_KIT, 
            //int ISNEW, int ISUPDATED, 
            int ROOCOMMONLIST, int FK_COMMONLISTSUBJ)
            : base(pubid, name)
        {
            this.intMAXQTY_ROO = MAXQTY_ROO;
            this.blnEVERYORDER_ROO = EVERYORDER_ROO;
            this.blnISSEARCHABLE_ROO = ISSEARCHABLE_ROO;
            //this.blnISNEW = ISNEW;
            //this.blnISUPDATED = ISUPDATED;
            this.blnISROO_KIT = ISROO_KIT;
            this.blnROOCOMMONLIST = ROOCOMMONLIST;
            this.intFK_COMMONLISTSUBJ = FK_COMMONLISTSUBJ;
        }

        public int PubID
        {
            get { return this.ID; }
            set { this.ID = value; }
        }

        public string PubName
        {
            get { return this.Name; }
            set { this.Name = value; }
        }

        public int MAXQTY_ROO
        {
            get { return this.intMAXQTY_ROO; }
            set { this.intMAXQTY_ROO = value; }
        }

        public int EVERYORDER_ROO
        {
            get { return this.blnEVERYORDER_ROO; }
            set { this.blnEVERYORDER_ROO = value; }
        }

        public int ISSEARCHABLE_ROO
        {
            get { return this.blnISSEARCHABLE_ROO; }
            set { this.blnISSEARCHABLE_ROO = value; }
        }

        //public int ISNEW
        //{
        //    get { return this.blnISNEW; }
        //    set { this.blnISNEW = value; }
        //}

        //public int ISUPDATED
        //{
        //    get { return this.blnISUPDATED; }
        //    set { this.blnISUPDATED = value; }
        //}

        public int ISROO_KIT
        {
            get { return this.blnISROO_KIT; }
            set { this.blnISROO_KIT = value; }
        }

        public int ROOCOMMONLIST
        {
            get { return this.blnROOCOMMONLIST; }
            set { this.blnROOCOMMONLIST = value; }
        }

        public int FK_COMMONLISTSUBJ
        {
            get { return this.intFK_COMMONLISTSUBJ; }
            set { this.intFK_COMMONLISTSUBJ = value; }
        }
    }
}
