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
    public class Catalog : MultiSelectListBoxItem
    {

        private int intCategory;
        private int intSubCategory;
        private int intSubSubCategory;
        private int blnWYNTK;
        private int blnSPANISH_WYNTK;
        private int intCatalogSubject;

        public Catalog(int pubid, string name,
            int intCategory, int intSubCategory, int intSubSubCategory,
            int blnWYNTK, int blnSPANISH_WYNTK, int intCatalogSubject)
            : base(pubid, name)
        {
            this.intCategory = intCategory;
            this.intSubCategory = intSubCategory;
            this.intSubSubCategory = intSubSubCategory;
            this.blnWYNTK = blnWYNTK;
            this.blnSPANISH_WYNTK = blnSPANISH_WYNTK;
            this.intCatalogSubject = intCatalogSubject;
        }

        public Catalog(int pubid, string name)
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

        public int CATEGORY
        {
            get { return this.intCategory; }
            set { this.intCategory = value; }
        }

        public int SUBCATEGORY
        {
            get { return this.intSubCategory; }
            set { this.intSubCategory = value; }
        }

        public int SUBSUBCATEGORY
        {
            get { return this.intSubSubCategory; }
            set { this.intSubSubCategory = value; }
        }

        public int WYNTK
        {
            get { return this.blnWYNTK; }
            set { this.blnWYNTK = value; }
        }

        public int SPANISH_WYNTK
        {
            get { return this.blnSPANISH_WYNTK; }
            set { this.blnSPANISH_WYNTK = value; }
        }

        public int CATALOG_SUBJECT
        {
            get { return this.intCatalogSubject; }
            set { this.intCatalogSubject = value; }
        }

    }
}
