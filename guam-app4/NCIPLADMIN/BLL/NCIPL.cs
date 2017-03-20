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
    public class NCIPL : MultiSelectListBoxItem
    {

        private int intMAXQTY_NCIPL;
        private int intMAXINTL_NCIPL;
        private int blnEVERYORDER_NCIPL;
        private int blnISSEARCHABLE_NCIPL;
        private int blnNCIPLFeatured;
        private int intRank_NCIPL;
        private string strNCIPLFeaturedImage;
        
        //public NCIPL(int pubid, string name,
        //    int MAXQTY_NCIPL, int MAXINTL_NCIPL, bool EVERYORDER_NCIPL,
        //    bool ISSEARCHABLE_NCIPL, bool NCIPLFeatured)
        //    : base(pubid, name)
        //{
        //    this.intMAXQTY_NCIPL = MAXQTY_NCIPL;
        //    this.intMAXINTL_NCIPL = MAXINTL_NCIPL;
        //    this.blnEVERYORDER_NCIPL = EVERYORDER_NCIPL;
        //    this.blnISSEARCHABLE_NCIPL = ISSEARCHABLE_NCIPL;
        //    this.blnNCIPLFeatured = NCIPLFeatured;
        //}

        public NCIPL(int pubid, string name,
            int MAXQTY_NCIPL, int MAXINTL_NCIPL, int EVERYORDER_NCIPL,
            int ISSEARCHABLE_NCIPL, int NCIPLFeatured, int Rank_NCIPL, string NCIPLFeaturedImage)
            : base(pubid, name)
        {

            this.intMAXQTY_NCIPL = MAXQTY_NCIPL;
            this.intMAXINTL_NCIPL = MAXINTL_NCIPL;
            this.blnEVERYORDER_NCIPL = EVERYORDER_NCIPL;
            this.blnISSEARCHABLE_NCIPL = ISSEARCHABLE_NCIPL;
            this.blnNCIPLFeatured = NCIPLFeatured;
            this.intRank_NCIPL = Rank_NCIPL;
            this.strNCIPLFeaturedImage = NCIPLFeaturedImage;

            //this.blnEVERYORDER_NCIPL = EVERYORDER_NCIPL>0?true:;
            //this.blnISSEARCHABLE_NCIPL = ISSEARCHABLE_NCIPL;
            //this.blnNCIPLFeatured = NCIPLFeatured;
        }

        public NCIPL(int pubid, string name)
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

        public int MAXQTY_NCIPL
        {
            get { return this.intMAXQTY_NCIPL; }
            set { this.intMAXQTY_NCIPL = value; }
        }

        public int MAXINTL_NCIPL
        {
            get { return this.intMAXINTL_NCIPL; }
            set { this.intMAXINTL_NCIPL = value; }
        }

        public int EVERYORDER_NCIPL
        {
            get {
                return this.blnEVERYORDER_NCIPL;
            }
            set { this.blnEVERYORDER_NCIPL = value; }
        }

        public int ISSEARCHABLE_NCIPL
        {
            get { return this.blnISSEARCHABLE_NCIPL; }
            set { this.blnISSEARCHABLE_NCIPL = value; }
        }

        public int NCIPLFeatured
        {
            get { return this.blnNCIPLFeatured; }
            set { this.blnNCIPLFeatured = value; }
        }

        public int Rank_NCIPL
        {
            get { return this.intRank_NCIPL; }
            set { this.intRank_NCIPL = value; }
        }

        public string NCIPLFeaturedImage
        {
            get { return this.strNCIPLFeaturedImage; }
            set { this.strNCIPLFeaturedImage = value; }
        }

    }
}
