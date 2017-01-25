using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//added
using System.Web.SessionState;
using NCICatalog.BLL;
using System.Configuration;
using NCICatalog.DAL;

namespace NCICatalog.GlobalUtilities
{
    public class UtilityMethods
    {
        //Save all catalog records to session variable
        public static void SaveCatalogsToSession(string Category,
                                        string SubCategory,
                                        string SubSubCategory,
                                        string WYNTK,
                                        string SpanishWYNTK,
                                        string IsFactSheet,
                                        string ProductId,
                                        string LongTitle,
                                        string Url,
                                        string Abstract,
                                        int Month,
                                        int Year,
                                        int Limit,
                                        int WYNTKCode)
        {
            string MonthYear = "";
            string OrderLimit = "";
            string WYNTKSubjectCode = "";

            if (Month != Int32.MinValue && Year != Int32.MinValue)
                MonthYear = Month.ToString() + "/" + Year.ToString();
            else if (Year != Int32.MinValue)
                MonthYear = Year.ToString();
            if (Limit != Int32.MinValue)
                OrderLimit = Limit.ToString();
            if (WYNTKCode != Int32.MinValue)
                WYNTKSubjectCode = WYNTKCode.ToString();

            CatalogRecordCollection Coll;

            if (HttpContext.Current.Session["NCI_CatalogColl"] == null)
                Coll = new CatalogRecordCollection();
            else
                Coll = (CatalogRecordCollection)HttpContext.Current.Session["NCI_CatalogColl"];

            Coll.Add(new CatalogRecord(Category,
                                        SubCategory,
                                        SubSubCategory,
                                        WYNTK,
                                        SpanishWYNTK,
                                        IsFactSheet,
                                        ProductId,
                                        LongTitle,
                                        Url,
                                        Abstract,
                                        OrderLimit,
                                        MonthYear,
                                        WYNTKSubjectCode));

            HttpContext.Current.Session["NCI_CatalogColl"] = Coll;

        }

        //Save all cancer types to session variable
        public static void SaveCancerTypesToSession()
        {
            KVPairCollection Coll = new KVPairCollection();
            string activeflag = ConfigurationManager.AppSettings["ActiveCancerTypes"];
            if (string.Compare(activeflag, "1") == 0)
                Coll = SQLDataAcccess.GetCancerTypes(1);
            else
                Coll = SQLDataAcccess.GetCancerTypes(0); //No check for active flag in LU_CANCERTYPE

            HttpContext.Current.Session["NCI_CancerTypeColl"] = Coll;
            Coll = null;
        }

        public static string GetCurrentDate()
        {
            string strYear = DateTime.Now.Date.Year.ToString();
            string strMonth = DateTime.Now.Date.Month.ToString();
            if (strMonth.Length == 1)
            {
                strMonth = "0" + strMonth;
            }
            string strDate = DateTime.Now.Date.Day.ToString();
            if (strDate.Length == 1)
            {
                strDate = "0" + strDate;
            }
            string strDt = strMonth + "-" + strDate + "-" + strYear;
            return strDt;
        }

        public static string[] strArrAdjustPage()
        {
            string[] arrTemp = ConfigurationManager.AppSettings["AdjustToNextPage"].Split('+');
            return arrTemp;
        }

        public static string GetCancerTypeText(string CancerTypeId)
        {
            string CancerTypeText = "";

            if (CancerTypeId == "") //Exit Condition
                return "";

            KVPairCollection kvPairColl = (KVPairCollection)HttpContext.Current.Session["NCI_CancerTypeColl"];
            foreach (KVPair kvPairItem in kvPairColl)
            {
                if (string.Compare(kvPairItem.Key, CancerTypeId, true) == 0)
                {
                    CancerTypeText = kvPairItem.Val;
                    break;
                }
            }

            return CancerTypeText;
        }
    }
}
