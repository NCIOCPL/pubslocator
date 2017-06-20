using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml;
using AjaxControlToolkit;
using System.Data.SqlClient;
using PubEntAdmin.DAL;
using GlobalUtils;
using System.Collections;
using System.Collections.Specialized;
using System.Web.Services.Protocols;
using System.Data;
using PubEntAdmin.BLL;


namespace PubEntAdmin
{

    /// <summary>
    /// Summary description for ws_category
    /// </summary>
    [WebService(Namespace = "http://lmco.bps.com")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class ws_category : System.Web.Services.WebService
    {
        public ws_category()
        {
            
        }

        [System.Web.Services.WebMethod]
        public CascadingDropDownNameValue[] GetCategory(string knownCategoryValues, string category)
        {
            List<Subject> a = PE_DAL.GetAllCatalogSubject(true);

            List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
            foreach (Subject b in a)
            {
                string cat_name = b.SubjName;
                int cat_id = b.SubjID;
                values.Add(new CascadingDropDownNameValue(cat_name, cat_id.ToString()));
            }
            return values.ToArray();
        }


        [System.Web.Services.WebMethod]
        public CascadingDropDownNameValue[] GetSubCategoryBySubID(
               string knownCategoryValues, string category, string contextKey)
        {
            int categoryID;
            StringDictionary categoryValues = AjaxControlToolkit.CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            categoryID = Convert.ToInt32(categoryValues["category"]);

            List<SubCat> a = PE_DAL.GetSubCatBySubID(categoryID);

            List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
            foreach (SubCat b in a)
            {
                string subcat_name = b.SubCatName;
                int subcat_id = b.SubCatID;
                if (subcat_id.ToString() == contextKey)
                    values.Add(new CascadingDropDownNameValue(subcat_name, subcat_id.ToString(),true));
                else
                    values.Add(new CascadingDropDownNameValue(subcat_name, subcat_id.ToString()));
            }
            return values.ToArray();
        }

        [System.Web.Services.WebMethod]
        public CascadingDropDownNameValue[] GetSubSubCategoryBySubID(
               string knownCategoryValues, string category, string contextKey)
        {
            int subcategoryID;
            StringDictionary categoryValues = AjaxControlToolkit.CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            subcategoryID = Convert.ToInt32(categoryValues["subcategory"]);

            List<SubSubCat> a = PE_DAL.GetSubSubCatBySubID(subcategoryID);

            List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();
            foreach (SubSubCat b in a)
            {
                string subsubcat_name = b.SubSubCatName;
                int subsubcat_id = b.SubSubCatID;
                if (subsubcat_id.ToString() == contextKey)
                    values.Add(new CascadingDropDownNameValue(subsubcat_name, subsubcat_id.ToString(),true));
                else
                    values.Add(new CascadingDropDownNameValue(subsubcat_name, subsubcat_id.ToString()));
            }
            return values.ToArray();
        }

        [WebMethod]
        public string[] GetProdInterfaceByProdID(string ProdID, int IsVK)
        {
            string [] ret = new string[1];
            ret[0] = "InvalidInput";
            bool pass = true;

            if ((!PubEntAdminManager.LenVal(ProdID, 10)) ||
                (!PubEntAdminManager.LenVal(IsVK.ToString(), 1)))
            {
                pass = false;
            }

            if (!PubEntAdminManager.ContentVal(IsVK.ToString(), @"^\d{1}$"))
            {
                pass = false;
            }

            if ((PubEntAdminManager.OtherVal(ProdID)))
            {
                pass = false;
            }

            if ((PubEntAdminManager.SpecialVal2(ProdID.Replace(" ", ""))))
            {
                pass = false;
            }

            if (pass)
            {
                if (ProdID.Length > 0 && (IsVK == 0 || IsVK == 1))
                {
                    return PE_DAL.GetProdInt(ProdID, IsVK).ToArray();
                }
                else
                {
                    return ret;
                }
            }
            else
                return ret;
        }
        
    }
}
