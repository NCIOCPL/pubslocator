using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//Added
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data; //Uses Microsoft.Practices.EnterpriseLibrary.Common;
using NCICatalog.BLL;

namespace NCICatalog.DAL
{
    public class SQLDataAcccess
    {

        ////Return collections that a pub belongs to
        //public static IDataReader GetPubCollections(int pubid)
        //{
        //    Database db = DatabaseFactory.CreateDatabase();
        //    DbCommand dbCommand = db.GetStoredProcCommand("sp_NCIPL_getPubCollections");

        //    db.AddInParameter(dbCommand, "pubid", DbType.Int32, pubid);
        //    return db.ExecuteReader(dbCommand);
        //}

        #region UtilityMethods
        private string CombineDateParts(string mon_dt, string day_dt, string year_dt)
        {
            string return_dt = "";
            if (year_dt.Length == 4)
            {
                //if (mon_dt.Length < 2)
                //    mon_dt = "0" + mon_dt;

                //if (day_dt.Length < 2)
                //    day_dt = "0" + day_dt;


                ////final check
                //if (string.Compare(mon_dt, "00") == 0)
                //    mon_dt = "";
                //if (string.Compare(day_dt, "00") == 0)
                //    day_dt = "";

                if (string.Compare(mon_dt, "0") == 0)
                    mon_dt = "";
                if (string.Compare(day_dt, "0") == 0)
                    day_dt = "";
                
                if (mon_dt.Length != 0 && day_dt.Length != 0 && year_dt.Length != 0)
                    return_dt = mon_dt + "/" + day_dt + "/" + year_dt;
                else if (mon_dt.Length != 0 && year_dt.Length != 0)
                    return_dt = mon_dt + "/" + year_dt;
                else
                    return_dt = year_dt;
            }
            return return_dt;
        }
        
        private static System.Data.IDataReader ExecuteReader(Database db, DbCommand dbCmd)
        {
            if (db == null) throw (new ArgumentNullException("db"));
            if (dbCmd == null) throw (new ArgumentNullException("dbCmd"));

            return (db.ExecuteReader(dbCmd));
        }
        #endregion

        public static int GetPubIdFromProductId(string ProductId)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cmd = db.GetStoredProcCommand("sp_NCIPL_GetPubIdFromProductId");
            db.AddInParameter(cmd, "productid", DbType.String, ProductId);
            db.AddOutParameter(cmd, "returnvalue", DbType.Int32, 0);
            db.ExecuteNonQuery(cmd);
            int retvalue = (int)db.GetParameterValue(cmd, "returnvalue");
            return retvalue;
        }

        //KVPair is used to get LU_CANCERTYPE values for WYNTK Catalogs
        public static KVPairCollection GetCancerTypes(int active)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cmd = db.GetStoredProcCommand("sp_CATALOG_getCancerTypes");
            db.AddInParameter(cmd, "@active", DbType.Int32, active);
            using (System.Data.IDataReader dr = db.ExecuteReader(cmd))
            {
                KVPairCollection coll = new KVPairCollection();
                while (dr.Read())
                {
                    KVPair k = new KVPair(dr.GetInt32(0).ToString(), (string)dr.GetString(1));
                    coll.Add(k);
                }
                return (coll);
            }
        }


        #region Catalog_DataAccess_Methods
        //Get Categories
        public static CategoryCollection GetCategories()
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbcmd = db.GetStoredProcCommand("sp_CATALOG_getCategories");
            using (IDataReader dr = db.ExecuteReader(dbcmd))
            {
                CategoryCollection collCategory = new CategoryCollection();
                while (dr.Read())
                {
                    Category itemCategory = new Category(
                        dr.GetInt32(dr.GetOrdinal("SUBJECTID")),
                        (dr["SUBJECTDESC"] == DBNull.Value) ? "" : dr["SUBJECTDESC"].ToString(),
                        (dr["CATALOG_SUBCATEGORY_TYPE"] == DBNull.Value) ? int.MinValue : dr.GetInt32(dr.GetOrdinal("CATALOG_SUBCATEGORY_TYPE")),
                        (dr["HASSUBCATEGORIES_FLAG"] == DBNull.Value) ? int.MinValue : dr.GetInt32(dr.GetOrdinal("HASSUBCATEGORIES_FLAG"))
                        );
                    collCategory.Add(itemCategory);
                }
                return (collCategory);
            }
        }
        //Get number of pubs in each category
        public static int GetNumPubsPerCategory(int SubjectId)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cmd = db.GetStoredProcCommand("sp_CATALOG_GetNumPubsPerCategory");
            db.AddInParameter(cmd, "subjectid", DbType.Int32, SubjectId);
            db.AddOutParameter(cmd, "returnvalue", DbType.Int32, 0);
            db.ExecuteNonQuery(cmd);
            int retvalue = (int)db.GetParameterValue(cmd, "returnvalue");
            return retvalue;
        }
        //Get pubs that do not have a sub category
        public static CatalogPubsCollection GetCatalogPubsWithoutSubCategories(int SubjectId)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbcmd = db.GetStoredProcCommand("sp_CATALOG_getCatalogPubsWithoutSubCategoriesBySubjectId");
            db.AddInParameter(dbcmd, "subjectid", DbType.Int32, SubjectId);
            using (IDataReader dr = db.ExecuteReader(dbcmd))
            {
                CatalogPubsCollection coll = new CatalogPubsCollection();
                while (dr.Read())
                {
                    CatalogPub item = new CatalogPub(
                        (dr["CATALOG_WYNTK_FLAG"] == DBNull.Value) ? int.MinValue : dr.GetInt32(dr.GetOrdinal("CATALOG_WYNTK_FLAG")),
                        (dr["SubjectDesc"] == DBNull.Value) ? "" : dr["SubjectDesc"].ToString(),
                        (dr["SPANISH_WYNTK_FLAG"] == DBNull.Value) ? int.MinValue : dr.GetInt32(dr.GetOrdinal("SPANISH_WYNTK_FLAG")),
                        dr.GetInt32(dr.GetOrdinal("PUBID")),
                        dr["PRODUCTID"].ToString(),
                        (dr["LONGTITLE"] == DBNull.Value) ? "" : dr["LONGTITLE"].ToString(),
                        (dr["ABSTRACT"] == DBNull.Value) ? "" : dr["ABSTRACT"].ToString(),
                        (dr["URL"] == DBNull.Value) ? "" : dr["URL"].ToString(),
                        (dr["MAXQTY"] == DBNull.Value) ? int.MinValue : dr.GetInt32(dr.GetOrdinal("MAXQTY")),
                        (dr["REVISED_MONTH"] == DBNull.Value) ? int.MinValue : dr.GetInt32(dr.GetOrdinal("REVISED_MONTH")),
                        (dr["REVISED_YEAR"] == DBNull.Value) ? int.MinValue : dr.GetInt32(dr.GetOrdinal("REVISED_YEAR")),
                        (dr["CATALOG"] == DBNull.Value) ? int.MinValue : dr.GetInt32(dr.GetOrdinal("CATALOG")),
                        (dr["CATALOG_SUBJ1_FK_CODE"] == DBNull.Value) ? int.MinValue : dr.GetInt32(dr.GetOrdinal("CATALOG_SUBJ1_FK_CODE")),
                        (dr["CATALOG_SUBJ2_FK_CODE"] == DBNull.Value) ? int.MinValue : dr.GetInt32(dr.GetOrdinal("CATALOG_SUBJ2_FK_CODE")),
                        (dr["CATALOG_SUBCATEGORY_FK"] == DBNull.Value) ? int.MinValue : dr.GetInt32(dr.GetOrdinal("CATALOG_SUBCATEGORY_FK")),
                        dr["IS_FACTSHEET"].ToString()
                        );
                    coll.Add(item);
                }
                return (coll);
            }
        }
        //Get SubCategories
        public static CategoryCollection GetSubCategories(int SubjectId)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbcmd = db.GetStoredProcCommand("sp_CATALOG_getSubCategoriesBySubjectId");
            db.AddInParameter(dbcmd, "subjectid", DbType.Int32, SubjectId);
            using (IDataReader dr = db.ExecuteReader(dbcmd))
            {
                CategoryCollection collCategory = new CategoryCollection();
                while (dr.Read())
                {
                    Category itemCategory = new Category(
                        dr.GetInt32(dr.GetOrdinal("SubCategoryID")),
                        (dr["SubCategoryDescription"] == DBNull.Value) ? "" : dr["SubCategoryDescription"].ToString(),
                        dr.GetInt32(dr.GetOrdinal("FK_SubjectCode")),
                        (dr["HasSubCategories_Flag"] == DBNull.Value) ? int.MinValue : dr.GetInt32(dr.GetOrdinal("HasSubCategories_Flag")),
                        (dr["HasSubSubCategories_Flag"] == DBNull.Value) ? int.MinValue : dr.GetInt32(dr.GetOrdinal("HasSubSubCategories_Flag"))
                        );
                    collCategory.Add(itemCategory);
                }
                return (collCategory);
            }
        }
        //Get pubs with Sub Categories but not Sub Sub Categories
        public static CatalogPubsCollection GetCatalogPubsWithoutSubSubCategories(int SubjectId, int SubCategoryId)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbcmd = db.GetStoredProcCommand("sp_CATALOG_getCatalogPubsWithoutSubSubCategoriesBySubjectId");
            db.AddInParameter(dbcmd, "subjectid", DbType.Int32, SubjectId);
            db.AddInParameter(dbcmd, "subcategoryid", DbType.Int32, SubCategoryId);
            using (IDataReader dr = db.ExecuteReader(dbcmd))
            {
                CatalogPubsCollection coll = new CatalogPubsCollection();
                while (dr.Read())
                {
                    CatalogPub item = new CatalogPub(
                        (dr["CATALOG_WYNTK_FLAG"] == DBNull.Value) ? int.MinValue : dr.GetInt32(dr.GetOrdinal("CATALOG_WYNTK_FLAG")),
                        dr.GetInt32(dr.GetOrdinal("PUBID")),
                        dr["PRODUCTID"].ToString(),
                        (dr["LONGTITLE"] == DBNull.Value) ? "" : dr["LONGTITLE"].ToString(),
                        (dr["ABSTRACT"] == DBNull.Value) ? "" : dr["ABSTRACT"].ToString(),
                        (dr["URL"] == DBNull.Value) ? "" : dr["URL"].ToString(),
                        (dr["MAXQTY"] == DBNull.Value) ? int.MinValue : dr.GetInt32(dr.GetOrdinal("MAXQTY")),
                        (dr["REVISED_MONTH"] == DBNull.Value) ? int.MinValue : dr.GetInt32(dr.GetOrdinal("REVISED_MONTH")),
                        (dr["REVISED_YEAR"] == DBNull.Value) ? int.MinValue : dr.GetInt32(dr.GetOrdinal("REVISED_YEAR")),
                        (dr["CATALOG_SUBSUBCATEGORY_FK"] == DBNull.Value) ? int.MinValue : dr.GetInt32(dr.GetOrdinal("CATALOG_SUBSUBCATEGORY_FK")),
                        (dr["CATALOG_SUBJ1_FK_CODE"] == DBNull.Value) ? int.MinValue : dr.GetInt32(dr.GetOrdinal("CATALOG_SUBJ1_FK_CODE")),
                        dr["IS_FACTSHEET"].ToString()
                        );
                    coll.Add(item);
                }
                return (coll);
            }
        }
        //Get SubSubCategories
        public static CategoryCollection GetSubSubCategories(int SubCategoryId)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbcmd = db.GetStoredProcCommand("sp_CATALOG_getSubSubCategoriesBySubCategoryId");
            db.AddInParameter(dbcmd, "subcategoryid", DbType.Int32, SubCategoryId);
            using (IDataReader dr = db.ExecuteReader(dbcmd))
            {
                CategoryCollection collCategory = new CategoryCollection();
                while (dr.Read())
                {
                    Category itemCategory = new Category(
                        dr.GetInt32(dr.GetOrdinal("FK_SubCategoryID")),
                        dr.GetInt32(dr.GetOrdinal("SubSubCategoryID")),
                        (dr["SubSubCategoryDescription"] == DBNull.Value) ? "" : dr["SubSubCategoryDescription"].ToString()
                        );
                    collCategory.Add(itemCategory);
                }
                return (collCategory);
            }
        }
        //Get Pubs that are under SubCategories and SubSubCategories
        public static CatalogPubsCollection GetCatalogPubsWithSubSubCategories(int SubjectId, int SubCategoryId, int SubSubCategoryId)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbcmd = db.GetStoredProcCommand("sp_CATALOG_getCatalogPubsBySubSubCategoryIdBySubCategoryIDBySubjectId");
            db.AddInParameter(dbcmd, "subjectid", DbType.Int32, SubjectId);
            db.AddInParameter(dbcmd, "subcategoryid", DbType.Int32, SubCategoryId);
            db.AddInParameter(dbcmd, "subsubcategoryid", DbType.Int32, SubSubCategoryId);
            using (IDataReader dr = db.ExecuteReader(dbcmd))
            {
                CatalogPubsCollection coll = new CatalogPubsCollection();
                while (dr.Read())
                {
                    CatalogPub item = new CatalogPub(
                        (dr["CATALOG_WYNTK_FLAG"] == DBNull.Value) ? int.MinValue : dr.GetInt32(dr.GetOrdinal("CATALOG_WYNTK_FLAG")),
                        (dr["SubjectDesc"] == DBNull.Value) ? "" : dr["SubjectDesc"].ToString(),
                        (dr["SPANISH_WYNTK_FLAG"] == DBNull.Value) ? int.MinValue : dr.GetInt32(dr.GetOrdinal("SPANISH_WYNTK_FLAG")),
                        dr.GetInt32(dr.GetOrdinal("PUBID")),
                        dr["PRODUCTID"].ToString(),
                        (dr["LONGTITLE"] == DBNull.Value) ? "" : dr["LONGTITLE"].ToString(),
                        (dr["ABSTRACT"] == DBNull.Value) ? "" : dr["ABSTRACT"].ToString(),
                        (dr["URL"] == DBNull.Value) ? "" : dr["URL"].ToString(),
                        (dr["MAXQTY"] == DBNull.Value) ? int.MinValue : dr.GetInt32(dr.GetOrdinal("MAXQTY")),
                        (dr["REVISED_MONTH"] == DBNull.Value) ? int.MinValue : dr.GetInt32(dr.GetOrdinal("REVISED_MONTH")),
                        (dr["REVISED_YEAR"] == DBNull.Value) ? int.MinValue : dr.GetInt32(dr.GetOrdinal("REVISED_YEAR")),
                        dr.GetInt32(dr.GetOrdinal("CATALOG_SUBSUBCATEGORY_FK")),
                        (dr["CATALOG_SUBJ1_FK_CODE"] == DBNull.Value) ? int.MinValue : dr.GetInt32(dr.GetOrdinal("CATALOG_SUBJ1_FK_CODE")),
                        dr["IS_FACTSHEET"].ToString()
                        );
                    coll.Add(item);
                }
                return (coll);
            }
        }
        #endregion

    }
}
