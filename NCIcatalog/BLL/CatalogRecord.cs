using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NCICatalog.BLL
{
    [Serializable]
    public class CatalogRecord
    {
        private string _category;
        private string _subcategory;
        private string _subsubcategory;
        private string _wyntk;
        private string _spanihswyntk;
        private string _isfactsheet;
        private string _productid;
        private string _longtitle;
        private string _url;
        private string _abstract;
        private string _limit;
        private string _monthyear;
        private string _catalogsubj1fkcode;         //the code
        //private string _catalogsubj1fkdescription;  //the subject

        public CatalogRecord(   string Category,
                                string SubCategory,
                                string SubSubCategory,
                                string WYNTK,
                                string SpanishWYNTK,
                                string IsFactSheet,
                                string ProductId,
                                string LongTitle,
                                string URL,
                                string Abstract,
                                string Limit,
                                string MonthYear,
                                string WYNTKSubjectCode)
        {
            _category = Category;
            _subcategory = SubCategory;
            _subsubcategory = SubSubCategory;
            _wyntk = WYNTK;
            _spanihswyntk = SpanishWYNTK;
            _isfactsheet = IsFactSheet;
            _productid = ProductId;
            _longtitle = LongTitle;
            _url = URL;
            _abstract = Abstract;
            _limit = Limit;
            _monthyear = MonthYear;
            _catalogsubj1fkcode = WYNTKSubjectCode;
        }

        public string Category { get { return _category; } }
        public string SubCategory { get { return _subcategory; } }
        public string SubSubCategory { get { return _subsubcategory; } }
        public string CatalogWYNTK { get { return _wyntk; } }
        public string SpanishWYNTK { get { return _spanihswyntk; } }
        public string IsFactSheet { get { return _isfactsheet; } }
        public string ProductId { get { return _productid; } }
        public string LongTitle { get { return _longtitle; } }
        public string Abstract { get { return _abstract; } }
        public string URL { get { return _url; } }
        public string Limit { get { return _limit; } }
        public string MonthYear { get { return _monthyear; } }
        public string CatalogWYNTKCancerType { get { return _catalogsubj1fkcode; } }
        public string CatalogWYNTKCancerTypeDesc { get { return GlobalUtilities.UtilityMethods.GetCancerTypeText(_catalogsubj1fkcode); } }
    }
}
