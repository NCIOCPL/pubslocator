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
    public class SubSubCat : MultiSelectListBoxItem
    {
        #region Fields
        private string strAssignedSubCatName;
        private int strAssignedSubCatID;
        private string strAssignedSubName;
        private int strAssignedSubID;
        #endregion

        #region Constructors

        public SubSubCat(int subsubcatId, string subsubcatName, int assignedSubCatID,
            string assignedSubCatName, int assignedSubID, string assignedSubName)
            : this(subsubcatId, subsubcatName, assignedSubCatID, assignedSubCatName)
        {
            this.strAssignedSubName = assignedSubName;
            this.strAssignedSubID = assignedSubID;
        }

        public SubSubCat(int subsubcatId, string subsubcatName, int assignedSubCatID,
            string assignedSubCatName)
            : this(subsubcatId, subsubcatName, assignedSubCatID)
        {
            this.strAssignedSubCatName = assignedSubCatName;
        }

        public SubSubCat(int subsubcatId, string subsubcatName, int assignedSubCatID)
            : base(subsubcatId, subsubcatName)
        {
            this.strAssignedSubCatID = assignedSubCatID;
        }

        #endregion

        #region Properties
        public int SubSubCatID
        {
            get { return this.ID; }
            set { this.ID = value; }
        }

        public string SubSubCatName
        {
            get { return this.Name; }
            set { this.Name = value; }
        }

        public int SubCatID
        {
            get { return this.strAssignedSubCatID; }
            set { this.strAssignedSubCatID = value; }
        }

        public string SubCatName
        {
            get { return this.strAssignedSubCatName; }
            set { this.strAssignedSubCatName = value; }
        }

        public int SubID
        {
            get { return this.strAssignedSubID; }
            set { this.strAssignedSubID = value; }
        }

        public string SubName
        {
            get { return this.strAssignedSubName; }
            set { this.strAssignedSubName = value; }
        }
        #endregion
    }
}
