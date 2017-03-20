using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//added
using NCICatalog.BLL;
using NCICatalog.DAL;

namespace NCICatalog.BLL
{
    [Serializable]
    public class CatalogPub
    {
        private int _catalogwyntkflag = int.MinValue;
        private int _spanishwyntkflag = int.MinValue;
        private string _catalogsubjdesc;
        private int _pubid;
        private string _productid;
        private string _longtitle;
        private string _pubabstract;
        private string _url;
        private int _cpjmaxqty;
        private int _revisedmonth;
        private int _revisedday;
        private int _revisedyear;
        private int _catalogsubj1fkcode;
        private int _catalogsubj2fkcode;
        private int _catalogsubcategoryfk;
        private int _catalogsubsubcategoryfk;
        private int _catalogflag;
        private string _isfactsheet;

        //constructor
        public CatalogPub()
        {
        }

        ////For sp_CATALOG_getCatalogPubsBySubjectId
        //public CatalogPub(int catalogwyntkflag,
        //                    string catalogsubjdesc,
        //                    int spanishwyntkflag,
        //                    int pubid,
        //                    string productid,
        //                    string longtitle,
        //                    string pubabstract,
        //                    string url,
        //                    int catalogflag,
        //                    int catalogsubj1fkcode,
        //                    int catalogsubj2fkcode,
        //                    int catalogsubcategoryfk
        //    )
        //{
        //    _catalogwyntkflag = catalogwyntkflag;
        ////    _spanishwyntkflag = spanishwyntkflag;
        //    _catalogsubjdesc = catalogsubjdesc;
        //    _pubid = pubid;
        //    _productid = productid;
        //    _longtitle = longtitle;
        //    _pubabstract = pubabstract;
        //    _url = url;
        //    _catalogflag = catalogflag;
        //    _catalogsubj1fkcode = catalogsubj1fkcode;
        //    _catalogsubj2fkcode = catalogsubj2fkcode;
        //    _catalogsubcategoryfk = catalogsubcategoryfk;
        //}

        //For sp_CATALOG_getCatalogPubsWithoutSubCategoriesBySubjectId
        public CatalogPub(int catalogwyntkflag,
                            string catalogsubjdesc,
                            int spanishwyntkflag,
                            int pubid,
                            string productid,
                            string longtitle,
                            string pubabstract,
                            string url,
                            int cpjmaxqty,
                            int revisedmonth,
                            int revisedyear,
                            int catalogflag,
                            int catalogsubj1fkcode,
                            int catalogsubj2fkcode,
                            int catalogsubcategoryfk,
                            string isfactsheet
            )
        {
            _catalogwyntkflag = catalogwyntkflag;
            _spanishwyntkflag = spanishwyntkflag;
            _catalogsubjdesc = catalogsubjdesc;
            _pubid = pubid;
            _productid = productid;
            _longtitle = longtitle;
            _pubabstract = pubabstract;
            _url = url;
            _cpjmaxqty = cpjmaxqty;
            _revisedmonth = revisedmonth;
            _revisedyear = revisedyear;
            _catalogflag = catalogflag;
            _catalogsubj1fkcode = catalogsubj1fkcode;
            _catalogsubj2fkcode = catalogsubj2fkcode;
            _catalogsubcategoryfk = catalogsubcategoryfk;
            _isfactsheet = isfactsheet;
        }

        
        
        //For sp_CATALOG_getCatalogPubsBySubSubCategoryIdBySubCategoryIDBySubjectId
        //and
        //For sp_CATALOG_getCatalogPubsBySubjectId
        public CatalogPub(int catalogwyntkflag,
                            string catalogsubjdesc,
                            int spanishwyntkflag,
                            int pubid,
                            string productid,
                            string longtitle,
                            string pubabstract,
                            string url,
                            int cpjmaxqty,
                            int revisedmonth,
                            int revisedyear,
                            int catalogsubcategoryfk,
                            int catalogsubj1fkcode,
                            string isfactsheet
            )
        {
            _catalogwyntkflag = catalogwyntkflag;
            _spanishwyntkflag = spanishwyntkflag;
            _catalogsubjdesc = catalogsubjdesc;
            _pubid = pubid;
            _productid = productid;
            _longtitle = longtitle;
            _pubabstract = pubabstract;
            _url = url;
            _cpjmaxqty = cpjmaxqty;
            _revisedmonth = revisedmonth;
            _revisedyear = revisedyear;
            _catalogsubcategoryfk = catalogsubcategoryfk;
            _isfactsheet = isfactsheet;
            _catalogsubj1fkcode = catalogsubj1fkcode;
        }

        //For sp_CATALOG_getCatalogPubsWithoutSubSubCategoriesBySubjectId
        public CatalogPub(int catalogwyntkflag,
                            int pubid,
                            string productid,
                            string longtitle,
                            string pubabstract,
                            string url,
                            int cpjmaxqty,
                            int revisedmonth,
                            int revisedyear,
                            int catalogsubsubcategoryfk,
                            int catalogsubj1fkcode,
                            string isfactsheet
            )
        {
            _catalogwyntkflag = catalogwyntkflag;
            _pubid = pubid;
            _productid = productid;
            _longtitle = longtitle;
            _pubabstract = pubabstract;
            _url = url;
            _cpjmaxqty = cpjmaxqty;
            _revisedmonth = revisedmonth;
            _revisedyear = revisedyear;
            _catalogsubsubcategoryfk = catalogsubsubcategoryfk;
            _isfactsheet = isfactsheet;
            _catalogsubj1fkcode = catalogsubj1fkcode;
        }

        public string ProductId { get { return _productid; } }
        public string LongTitle
        {
            get { return _longtitle; }
            //set { _longtitle = value; }
        }
        public string Url { get { return _url; } }
        public string Abstract { get { return _pubabstract; } }
        public int Month { get { return _revisedmonth; } }
        public int Year { get { return _revisedyear; } }
        public int Limit { get { return _cpjmaxqty; } }
        public string IsFactSheet { get { return _isfactsheet; } }

        //Method returns string SpanishWYNTK Value
        private string GetSpanishWYNTKValue()
        {
            if (_spanishwyntkflag == int.MinValue)
                return "";
            else
                return _spanishwyntkflag.ToString();
        }

        //Method returns string CatalogWYNTK Value
        private string GetWYNTKValue()
        {
            if (_catalogwyntkflag == int.MinValue)
                return "";
            else
                return _catalogwyntkflag.ToString();
        }

        public string SpanishWYNTK { get { return GetSpanishWYNTKValue(); } }
        public string CatalogWYNTK { get { return GetWYNTKValue(); } }
        public int CatalogWYNTKCancerType { get { return _catalogsubj1fkcode; } }
    }
}
