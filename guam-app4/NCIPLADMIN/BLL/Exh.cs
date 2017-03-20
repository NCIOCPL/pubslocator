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
    public class Exh : MultiSelectListBoxItem
    {

        private int intMAXQTY_EXHIBIT;
        private int intMAXINTL_EXHIBIT;
        private int blnEVERYORDER_EXHIBIT;
        private int blnISSEARCHABLE_EXHIBIT;

        //public Exh(int pubid, string name,
        //    int MAXQTY_EXHIBIT, int MAXINTL_EXHIBIT, bool EVERYORDER_EXHIBIT,
        //    bool ISSEARCHABLE_EXHIBIT)
        //    : base(pubid, name)
        //{
        //    this.intMAXQTY_EXHIBIT = MAXQTY_EXHIBIT;
        //    this.intMAXINTL_EXHIBIT = MAXINTL_EXHIBIT;
        //    this.blnEVERYORDER_EXHIBIT = EVERYORDER_EXHIBIT;
        //    this.blnISSEARCHABLE_EXHIBIT = ISSEARCHABLE_EXHIBIT;
        //}

        public Exh(int pubid, string name,
            int MAXQTY_EXHIBIT, int MAXINTL_EXHIBIT, int EVERYORDER_EXHIBIT,
            int ISSEARCHABLE_EXHIBIT)
            : base(pubid, name)
        {
            this.intMAXQTY_EXHIBIT = MAXQTY_EXHIBIT;
            this.intMAXINTL_EXHIBIT = MAXINTL_EXHIBIT;
            this.blnEVERYORDER_EXHIBIT = EVERYORDER_EXHIBIT;
            this.blnISSEARCHABLE_EXHIBIT = ISSEARCHABLE_EXHIBIT;
        }

        public Exh(int pubid, string name)
            : base(pubid, name) { }

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

        public int MAXQTY_EXHIBIT
        {
            get { return this.intMAXQTY_EXHIBIT; }
            set { this.intMAXQTY_EXHIBIT = value; }
        }

        public int MAXINTL_EXHIBIT
        {
            get { return this.intMAXINTL_EXHIBIT; }
            set { this.intMAXINTL_EXHIBIT = value; }
        }

        public int EVERYORDER_EXHIBIT
        {
            get { return this.blnEVERYORDER_EXHIBIT; }
            set { this.blnEVERYORDER_EXHIBIT = value; }
        }

        public int ISSEARCHABLE_EXHIBIT
        {
            get { return this.blnISSEARCHABLE_EXHIBIT; }
            set { this.blnISSEARCHABLE_EXHIBIT = value; }
        }


    }
}
