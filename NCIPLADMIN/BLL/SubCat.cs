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
    public class SubCat : MultiSelectListBoxItem
    {
        #region Fields
        private string strAssignedCatName;
        private bool blnAllowHaveSubSubcat;
        private int intAssignedCatID;
        private int intSubSubcatID;
        private int intHvHowManySubSub;
        #endregion

        #region Constructors
        public SubCat(int subcatId, string subcatName, int assignedCatID,
            string assignedCatName, bool allowHaveSubSubcat, int subSubcatID)
            : this(subcatId, subcatName, assignedCatID, allowHaveSubSubcat)
        {
            this.strAssignedCatName = assignedCatName;
            this.intSubSubcatID = subSubcatID;
        }

        public SubCat(int subcatId, string subcatName, int assignedCatID,
            bool allowHaveSubSubcat, int HvHowManySubSub)
            :this(subcatId, subcatName, assignedCatID, allowHaveSubSubcat)
        {
            this.intHvHowManySubSub = HvHowManySubSub;
        }

        public SubCat(int subcatId, string subcatName, int assignedCatID,
            bool allowHaveSubSubcat)
            : base(subcatId, subcatName)
        {
            this.blnAllowHaveSubSubcat = allowHaveSubSubcat;
            this.intAssignedCatID = assignedCatID;
        }

        public SubCat(int subcatId, string subcatName, int assignedCatID,
            string subjName)
            : base(subcatId, subcatName)
        {
            this.strAssignedCatName = subjName;
            this.intAssignedCatID = assignedCatID;
        }
        #endregion

        #region Properties
        public int SubCatID
        {
            get { return this.ID; }
            set { this.ID = value; }
        }

        public string SubCatName
        {
            get { return this.Name; }
            set { this.Name = value; }
        }

        public string AssignedCatName
        {
            get { return this.strAssignedCatName; }
            set { this.strAssignedCatName = value; }
        }

        public bool AllowHaveSubSubcat
        {
            get { return this.blnAllowHaveSubSubcat; }
            set { this.blnAllowHaveSubSubcat = value; }
        }

        public int AssignedCatID
        {
            get { return this.intAssignedCatID; }
            set { this.intAssignedCatID = value; }
        }

        public int SubSubcatID
        {
            get { return this.intSubSubcatID; }
            set { this.intSubSubcatID = value; }
        }

        public int HvHowManySubSub
        {
            get { return this.intHvHowManySubSub; }
            set { this.intHvHowManySubSub = value; }
        }
        
        #endregion

    }
}
