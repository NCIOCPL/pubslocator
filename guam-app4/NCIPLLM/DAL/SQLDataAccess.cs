using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//Manually added
using System.Data;
using System.Data.Common;
using PubEnt.BLL;
using System.Collections;
//EntLib References
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Logging;

//using System.Web.SessionState;
using System.Configuration;

using System.Data.SqlTypes;

using System.Text;
using System.Collections.Specialized;

namespace PubEnt.DAL
{

    public class DAL 
    {

        #region CONSTANTS
        //private const string SP_GET_ALL_NCIPL_RECORDS = "PRODUCTS_getAll_NCIPL_Products";
        private const string SP_GET_ALL_NCIPL_RECORDS = "PRODUCTS_getAll_NCIPL_PRODUCTS_TEMP";
        
        #endregion

        #region SQL_HELPER_METHODS
        //Return a dataset
        private static DataSet ExecutedataSet(Database db, DbCommand dbCmd)
        {
            if (db == null) throw (new ArgumentNullException("db"));
            if (dbCmd == null) throw (new ArgumentNullException("dbCmd"));

            return (db.ExecuteDataSet(dbCmd));
        }

        //***EAC Return a datareader
        public static System.Data.IDataReader ExecuteReader(Database db, DbCommand cw)
        {
            //***EAC mandatory checks
            if (db == null) throw (new ArgumentNullException("db"));
            if (cw == null) throw (new ArgumentNullException("cw"));

            return (db.ExecuteReader(cw));
        }
        #endregion

        
        public static DataSet GetAllNCIPLRecords()
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(SP_GET_ALL_NCIPL_RECORDS);
            //db.AddInParameter(dbCommand, "CategoryID", DbType.Int32, Category);
            DataSet productDataSet = null;
            //productDataSet = db.ExecuteDataSet(dbCommand);
            productDataSet = ExecutedataSet(db, dbCommand);
            return productDataSet;
        }

        //*********************************************************************
        //
        // Generate Collection  Methods
        //
        // The following methods are used to generate collections of objects
        //[]
        //*********************************************************************
        public static bool SaveCCResponse(string s)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand("sp_NCIPL_SaveCCResponse");
            db.AddInParameter(cw, "resp", DbType.String, s);
            db.ExecuteNonQuery(cw);
            return (true);
        }

        public static KVPairCollection GetKVPair(string s)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand(s);
            using (System.Data.IDataReader dr = db.ExecuteReader(cw))
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

        public static AnnouncementCollection GetAnnouncements()
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand("sp_NCIPLCC_getAnnouncements");  //***EAC 20130214 ok to use nciplcc version
            using (System.Data.IDataReader dr = db.ExecuteReader(cw))
            {
                AnnouncementCollection coll = new AnnouncementCollection();
                while (dr.Read())
                {
                    Announcement k = new Announcement(
                        dr.GetInt32(dr.GetOrdinal("announcementid")),
                                            dr["announcement_desc"].ToString(),
                                            dr["announcement_url"].ToString(),
                                            (dr["s_date"] == DBNull.Value) ? DateTime.MinValue : (DateTime)dr["s_date"],
                                            (dr["e_date"] == DBNull.Value) ? DateTime.MinValue : (DateTime)dr["e_date"]
                                            );
                    coll.Add(k);
                }
                return (coll);
            }
        }
        
        public static Product GetProductbyProductID(string productid)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand("sp_NCIPLLM_getPubbyProductID");
            db.AddInParameter(cw, "productid", DbType.String, productid);
            db.AddInParameter(cw, "guamrole", DbType.String, HttpContext.Current.Session["NCIPL_ROLE"]);

            using (System.Data.IDataReader dr = db.ExecuteReader(cw))
            {
                if (dr.Read())
                {
                    Product k = new Product(
                                            dr.GetInt32(dr.GetOrdinal("pubid")),
                                            dr["productid"].ToString(),
                                            dr["BOOKSTATUS"].ToString(),
                                            dr["DISPLAYSTATUS"].ToString(),
                                            dr["longtitle"].ToString(),
                                            dr["shorttitle"].ToString(),
                                            dr["abstract"].ToString(),
                                            dr["summary"].ToString(),

                                            (dr["RECORDUPDATEDATE"] == DBNull.Value) ? "" : dr["RECORDUPDATEDATE"].ToString(),

                                            (dr["RECORDUPDATEDATE"] == DBNull.Value) ? DateTime.MinValue : (DateTime)dr["RECORDUPDATEDATE"],

                                            dr.GetInt32(dr.GetOrdinal("ISONLINE")),
                                            dr["URL"].ToString(),
                                            dr["URL2"].ToString(),
                                            dr["thumbnailfile"].ToString(),
                                            dr.GetInt32(dr.GetOrdinal("QUANTITY_AVAILABLE")),
                                            dr.GetInt32(dr.GetOrdinal("maxqty_roo")),

                                            "",//nerdo field: obsolete
                                            "",//nerdo field: obsolete
                                            "",//nerdo field: obsolete
                                            "",//nerdo field: obsolete
                                            "",//nerdo field: obsolete
                                            0,//nerdo field: obsolete
                                            0,//nerdo field: obsolete

                                            dr["AUDIENCE"].ToString(),
                                            dr["AWARDS"].ToString(),
                                            dr["CANCERTYPE"].ToString(),
                                            dr["LANGUAGE"].ToString(),
                                            dr["FORMAT"].ToString(),
                                            dr["SERIES"].ToString(),
                                            dr["SUBJECT"].ToString(),
                                            dr["NIHNUM"].ToString(),
                                            (dr["recordcreatedate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)dr["recordcreatedate"],
                                            (dr["REVISED_MONTH"] == DBNull.Value) ? 0 : dr.GetInt32(dr.GetOrdinal("REVISED_MONTH")),
                                            (dr["REVISED_DAY"] == DBNull.Value) ? 0 : dr.GetInt32(dr.GetOrdinal("REVISED_DAY")),
                                            (dr["REVISED_YEAR"] == DBNull.Value) ? 0 : dr.GetInt32(dr.GetOrdinal("REVISED_YEAR")),
                                            (dr["REVISED_DATE"] == DBNull.Value) ? DateTime.MinValue : (DateTime)dr["REVISED_DATE"],
                                            (dr["REVISED_DATE_TYPE"] == DBNull.Value) ? "" : dr["REVISED_DATE_TYPE"].ToString(),
                                            dr.GetInt32(dr.GetOrdinal("TOTAL_NUM_PAGE")),
                                            dr["PUB_STATUS"].ToString(),
                                            dr["PDFURL"].ToString(),
                                            dr["LARGEIMAGEFILE"].ToString(),
                                            (dr["NCIPLFEATURED_IMAGEFILE"] == DBNull.Value) ? "" : dr["NCIPLFEATURED_IMAGEFILE"].ToString()
                                            );
                    return (k);
                }
                else throw new ArgumentException("Publication ID not found", "value");
                
            }
        }
        public static Product GetProductbyPubID(int pubid)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand("sp_NCIPLLM_getPubbyPubID");
            db.AddInParameter(cw, "pubid", DbType.Int32, pubid);
            db.AddInParameter(cw, "guamrole", DbType.String, HttpContext.Current.Session["NCIPL_ROLE"]);
            using (System.Data.IDataReader dr = db.ExecuteReader(cw))
            {
                if (dr.Read())
                {
                    Product k = new Product(
                                            dr.GetInt32(dr.GetOrdinal("pubid")),
                                            dr["productid"].ToString(),
                                            dr["BOOKSTATUS"].ToString(),
                                            dr["DISPLAYSTATUS"].ToString(),
                                            dr["longtitle"].ToString(),
                                            dr["shorttitle"].ToString(),
                                            dr["abstract"].ToString(),
                                            dr["summary"].ToString(),

                                            (dr["RECORDUPDATEDATE"] == DBNull.Value) ? "" : dr["RECORDUPDATEDATE"].ToString(),

                                            (dr["RECORDUPDATEDATE"] == DBNull.Value) ? DateTime.MinValue : (DateTime)dr["RECORDUPDATEDATE"],

                                            dr.GetInt32(dr.GetOrdinal("ISONLINE")),
                                            dr["URL"].ToString(),
                                            dr["URL2"].ToString(),
                                            dr["thumbnailfile"].ToString(),
                                            dr.GetInt32(dr.GetOrdinal("QUANTITY_AVAILABLE")),
                                            dr.GetInt32(dr.GetOrdinal("maxqty_roo")),

                                            "",//nerdo field: obsolete
                                            "",//nerdo field: obsolete
                                            "",//nerdo field: obsolete
                                            "",//nerdo field: obsolete
                                            "",//nerdo field: obsolete
                                            0,//nerdo field: obsolete
                                            0,//nerdo field: obsolete

                                            dr["AUDIENCE"].ToString(),
                                            dr["AWARDS"].ToString(),
                                            dr["CANCERTYPE"].ToString(),
                                            dr["LANGUAGE"].ToString(),
                                            dr["FORMAT"].ToString(),
                                            dr["SERIES"].ToString(),
                                            dr["SUBJECT"].ToString(),
                                            dr["NIHNUM"].ToString(),
                                            (dr["recordcreatedate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)dr["recordcreatedate"],
                                            (dr["REVISED_MONTH"] == DBNull.Value) ? 0 : dr.GetInt32(dr.GetOrdinal("REVISED_MONTH")),
                                            (dr["REVISED_DAY"] == DBNull.Value) ? 0 : dr.GetInt32(dr.GetOrdinal("REVISED_DAY")),
                                            (dr["REVISED_YEAR"] == DBNull.Value) ? 0 : dr.GetInt32(dr.GetOrdinal("REVISED_YEAR")),
                                            (dr["REVISED_DATE"] == DBNull.Value) ? DateTime.MinValue : (DateTime)dr["REVISED_DATE"],
                                            (dr["REVISED_DATE_TYPE"] == DBNull.Value) ? "" : dr["REVISED_DATE_TYPE"].ToString(),
                                            dr.GetInt32(dr.GetOrdinal("TOTAL_NUM_PAGE")),
                                            dr["PUB_STATUS"].ToString(),
                                            dr["PDFURL"].ToString(),
                                            dr["LARGEIMAGEFILE"].ToString(),
                                            (dr["NCIPLFEATURED_IMAGEFILE"] == DBNull.Value) ? "" : dr["NCIPLFEATURED_IMAGEFILE"].ToString()
                                            );
                    return (k);
                }
                else throw new ArgumentException("Publication ID not found", "value");

            }

        }

        //***EAC Exclusively written for the shopping cart
        public static Product GetCartPubByPubID(int pubid)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand("sp_NCIPLLM_getPubbyPubID");
            db.AddInParameter(cw, "pubid", DbType.Int32, pubid);
            db.AddInParameter(cw, "guamrole", DbType.String, HttpContext.Current.Session["NCIPL_ROLE"]);
            using (System.Data.IDataReader dr = db.ExecuteReader(cw))
            {
                if (dr.Read())
                {
                    Product k = new Product(dr.GetInt32(dr.GetOrdinal("pubid")),
                                            dr["productid"].ToString(),
                                            dr["longtitle"].ToString(),
                                            dr.GetInt32(dr.GetOrdinal("maxqty_roo")),
                                            dr.GetDouble(dr.GetOrdinal("weight")),
                                            dr["bookstatus"].ToString() //***EAC 20120516 we are assuming only 1 bookstatus per pub
                                            );
                    return (k);
                }
                else throw new ArgumentException("Publication ID not found", "value");

            }

        }

        public static ProductCollection GetProductsByParams(string terms, string cantype, string subj, string aud, string form, string lang, string startswith, string series, string neworupdated, string race)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand("sp_NCIPLLM_SearchContains");

            cw.CommandTimeout = 120; //HITT 10249 JPJ - TO FIX TIME OUT ON DEV -- Observed that default is 30 seconds, set to 60 or more to make it work

            db.AddInParameter(cw, "terms", DbType.String, terms);
            db.AddInParameter(cw, "cantype", DbType.String, cantype);
            db.AddInParameter(cw, "subj", DbType.String, subj);
            db.AddInParameter(cw, "aud", DbType.String, aud);
            db.AddInParameter(cw, "form", DbType.String, form);
            db.AddInParameter(cw, "lang", DbType.String, lang);
            db.AddInParameter(cw, "startswith", DbType.String, startswith);
            db.AddInParameter(cw, "series", DbType.String, series);
            if (string.Compare(neworupdated, "1", true) == 0)
                neworupdated = "1";
            else
                neworupdated = "";
            db.AddInParameter(cw, "neworupdated", DbType.String, neworupdated);
            db.AddInParameter(cw, "race", DbType.String, race);

            using (System.Data.IDataReader dr = db.ExecuteReader(cw))            
            {
                ProductCollection coll = new ProductCollection();
                while (dr.Read())
                {                   
                    Product k = new Product(
                                            dr.GetInt32(dr.GetOrdinal ("pubid")),
                                            dr["productid"].ToString(),
                                            dr["BOOKSTATUS"].ToString(),
                                            dr["DISPLAYSTATUS"].ToString(),
                                            dr["longtitle"].ToString(),
                                            dr["shorttitle"].ToString(),
                                            dr["abstract"].ToString(),
                                            dr["summary"].ToString(),

                                            (dr["RECORDUPDATEDATE"] == DBNull.Value) ? "" : dr["RECORDUPDATEDATE"].ToString(),
                                            
                                            (dr["RECORDUPDATEDATE"] == DBNull.Value) ? DateTime.MinValue : (DateTime)dr["RECORDUPDATEDATE"],
                                            
                                            dr.GetInt32(dr.GetOrdinal("ISONLINE")),
                                            dr["URL"].ToString(),
                                            dr["URL2"].ToString(),
                                            dr["thumbnailfile"].ToString(),
                                            
                                            dr.GetInt32(dr.GetOrdinal("QUANTITY_AVAILABLE")),
                                            
                                            dr.GetInt32(dr.GetOrdinal("maxqty_roo")),

                                            "",//nerdo field: obsolete
                                            "",//nerdo field: obsolete
                                            "",//nerdo field: obsolete
                                            "",//nerdo field: obsolete
                                            "",//nerdo field: obsolete
                                            0,//nerdo field: obsolete
                                            0,//nerdo field: obsolete

                                            dr["AUDIENCE"].ToString(),
                                            dr["AWARDS"].ToString(),
                                            dr["CANCERTYPE"].ToString(),
                                            dr["LANGUAGE"].ToString(),
                                            dr["FORMAT"].ToString(),
                                            dr["SERIES"].ToString(),
                                            dr["SUBJECT"].ToString(),
                                            dr["NIHNUM"].ToString(),
                                            (dr["recordcreatedate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)dr["recordcreatedate"],
                                            (dr["REVISED_MONTH"] == DBNull.Value) ? 0 : dr.GetInt32(dr.GetOrdinal("REVISED_MONTH")),
                                            (dr["REVISED_DAY"] == DBNull.Value) ? 0 : dr.GetInt32(dr.GetOrdinal("REVISED_DAY")),
                                            (dr["REVISED_YEAR"] == DBNull.Value) ? 0 : dr.GetInt32(dr.GetOrdinal("REVISED_YEAR")),
                                            (dr["REVISED_DATE"] == DBNull.Value) ? DateTime.MinValue : (DateTime)dr["REVISED_DATE"],
                                            (dr["REVISED_DATE_TYPE"] == DBNull.Value) ? "" : dr["REVISED_DATE_TYPE"].ToString(),
                                            dr.GetInt32(dr.GetOrdinal("TOTAL_NUM_PAGE")),
                                            dr["PUB_STATUS"].ToString(),
                                            dr["PDFURL"].ToString(),
                                            dr["LARGEIMAGEFILE"].ToString(),
                                            (dr["NCIPLFEATURED_IMAGEFILE"] == DBNull.Value) ? "" : dr["NCIPLFEATURED_IMAGEFILE"].ToString()
                                            );
                    
                    
                    //Product k = new Product(1,"111","111111 1 11 1","111 ","11111","","","","",DateTime.Now,1,1,1,"fake1.jpg",1,1);
                    //coll.Add(k);
                    coll.AddItemsToCollWithRules(k);
                }
                return (coll);
            }
        }

        public static ProductCollection GetProductbyProductIDWithRules(string productid)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand("sp_ncipllm_getPubbyProductID");
            db.AddInParameter(cw, "productid", DbType.String, productid);
            db.AddInParameter(cw, "guamrole", DbType.String, HttpContext.Current.Session["NCIPL_ROLE"]);

            using (IDataReader dataReader = db.ExecuteReader(cw))
            {
                // Processing code
                ProductCollection collProducts = new ProductCollection();
                while (dataReader.Read())
                {
                    Product newProduct = new Product(
                                                    dataReader.GetInt32(dataReader.GetOrdinal("pubid")),
                                                    dataReader["productid"].ToString(),
                                                    dataReader["BOOKSTATUS"].ToString(),
                                                    dataReader["DISPLAYSTATUS"].ToString(),
                                                    dataReader["longtitle"].ToString(),
                                                    dataReader["shorttitle"].ToString(),
                                                    dataReader["abstract"].ToString(),
                                                    dataReader["summary"].ToString(),

                                                    (dataReader["RECORDUPDATEDATE"] == DBNull.Value) ? "" : dataReader["RECORDUPDATEDATE"].ToString(),

                                                    (dataReader["RECORDUPDATEDATE"] == DBNull.Value) ? DateTime.MinValue : (DateTime)dataReader["RECORDUPDATEDATE"],

                                                    dataReader.GetInt32(dataReader.GetOrdinal("ISONLINE")),
                                                    dataReader["URL"].ToString(),
                                                    dataReader["URL2"].ToString(),
                                                    dataReader["thumbnailfile"].ToString(),

                                                    dataReader.GetInt32(dataReader.GetOrdinal("QUANTITY_AVAILABLE")),
                                                    dataReader.GetInt32(dataReader.GetOrdinal("maxqty_roo")),

                                                    "",//nerdo field: obsolete
                                                    "",//nerdo field: obsolete
                                                    "",//nerdo field: obsolete
                                                    "",//nerdo field: obsolete
                                                    "",//nerdo field: obsolete
                                                    0,//nerdo field: obsolete
                                                    0,//nerdo field: obsolete

                                                    dataReader["AUDIENCE"].ToString(),
                                                    dataReader["AWARDS"].ToString(),
                                                    dataReader["CANCERTYPE"].ToString(),
                                                    dataReader["LANGUAGE"].ToString(),
                                                    dataReader["FORMAT"].ToString(),
                                                    dataReader["SERIES"].ToString(),
                                                    dataReader["SUBJECT"].ToString(),
                                                    dataReader["NIHNUM"].ToString(),
                                                    (dataReader["recordcreatedate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)dataReader["recordcreatedate"],
                                                    (dataReader["REVISED_MONTH"] == DBNull.Value) ? 0 : dataReader.GetInt32(dataReader.GetOrdinal("REVISED_MONTH")),
                                                    (dataReader["REVISED_DAY"] == DBNull.Value) ? 0 : dataReader.GetInt32(dataReader.GetOrdinal("REVISED_DAY")),
                                                    (dataReader["REVISED_YEAR"] == DBNull.Value) ? 0 : dataReader.GetInt32(dataReader.GetOrdinal("REVISED_YEAR")),
                                                    (dataReader["REVISED_DATE"] == DBNull.Value) ? DateTime.MinValue : (DateTime)dataReader["REVISED_DATE"],
                                                    (dataReader["REVISED_DATE_TYPE"] == DBNull.Value) ? "" : dataReader["REVISED_DATE_TYPE"].ToString(),
                                                    dataReader.GetInt32(dataReader.GetOrdinal("TOTAL_NUM_PAGE")),
                                                    dataReader["PUB_STATUS"].ToString(),
                                                    dataReader["PDFURL"].ToString(),
                                                    dataReader["LARGEIMAGEFILE"].ToString(),
                                                    (dataReader["NCIPLFEATURED_IMAGEFILE"] == DBNull.Value) ? "" : dataReader["NCIPLFEATURED_IMAGEFILE"].ToString()
                                                    );
                    //collProducts.Add(newProduct);
                    collProducts.AddItemsToCollWithRules(newProduct);
                }
                return collProducts;
            }
        }

        public static ProductCollection GetRelatedProducts(int pubid)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("sp_NCIPLLM_getRelatedProducts");

            db.AddInParameter(dbCommand, "pubid", DbType.Int32, pubid);

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                ProductCollection collProducts = new ProductCollection();
                while (dr.Read())
                {
                    Product RelatedProduct = new Product(
                                                            dr.GetInt32(dr.GetOrdinal("pubid")),
                                                            dr["productid"].ToString(),
                                                            dr["BOOKSTATUS"].ToString(),
                                                            dr["DISPLAYSTATUS"].ToString(),
                                                            dr["longtitle"].ToString(),
                                                            dr["shorttitle"].ToString(),
                                                            dr["URL"].ToString(),
                                                            dr["thumbnailfile"].ToString()
                                                        );
                    collProducts.AddItemsToCollWithRules(RelatedProduct);

                }
                return collProducts;
            }

        }
        public static int GetPubIdFromProductId(string ProductId)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cmd = db.GetStoredProcCommand("sp_NCIPLLM_GetPubIdFromProductId");
            db.AddInParameter(cmd, "productid", DbType.String, ProductId);
            db.AddOutParameter(cmd, "returnvalue", DbType.Int32, 0);
            db.ExecuteNonQuery(cmd);
            int retvalue = (int)db.GetParameterValue(cmd, "returnvalue");
            return retvalue;
        }

        public static IDataReader GetPubPhysicalDesc(int pubid)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("sp_NCIPLCC_getPubPhysicalDescription");  //***EAC 20130214 ok to use nciplcc version

            db.AddInParameter(dbCommand, "pubid", DbType.Int32, pubid);
            return db.ExecuteReader(dbCommand);
        }

        //Return collections that a pub belongs to
        public static IDataReader GetPubCollections(int pubid)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("sp_NCIPLCC_getPubCollections");    //***EAC 20130214 ok to use nciplcc version

            db.AddInParameter(dbCommand, "pubid", DbType.Int32, pubid);
            return db.ExecuteReader(dbCommand);
        }

        //Get New and Updated Publications
        //CR-36 public static ProductCollection GetNewUpdatedProducts()
        public static ProductCollection GetNewUpdatedProducts(int type)
        {
            string procname = ""; //CR-36
            Database db = DatabaseFactory.CreateDatabase();
            //Begin CR 11-001-36
            //DbCommand cw = db.GetStoredProcCommand("sp_NCIPL_GetNewUpdatedPubs");
            switch (type)
            {
                case 1:
                    procname = "sp_NCIPLLM_GetNewUpdatedPubs";
                    break;
                //case 2:
                //    procname = "sp_NCIPL_GetNewPubs_ForStacks";
                //    break;
                //case 3:
                //    procname = "sp_NCIPL_GetUpdatedPubs_ForStacks";
                //    break;
            }
            DbCommand cw = db.GetStoredProcCommand(procname);
            //End CR-36

            using (System.Data.IDataReader dr = db.ExecuteReader(cw))            
            {
                ProductCollection coll = new ProductCollection();
                while (dr.Read())
                {
                    
                    Product k = new Product(
                                            dr.GetInt32(dr.GetOrdinal ("pubid")),
                                            dr["productid"].ToString(),
                                            dr["BOOKSTATUS"].ToString(),
                                            dr["DISPLAYSTATUS"].ToString(),
                                            dr["longtitle"].ToString(),
                                            dr["shorttitle"].ToString(),
                                            dr["abstract"].ToString(),
                                            dr["summary"].ToString(),

                                            (dr["RECORDUPDATEDATE"] == DBNull.Value) ? "" : dr["RECORDUPDATEDATE"].ToString(),
                                            
                                            (dr["RECORDUPDATEDATE"] == DBNull.Value) ? DateTime.MinValue : (DateTime)dr["RECORDUPDATEDATE"],
                                            
                                            dr.GetInt32(dr.GetOrdinal("ISONLINE")),
                                            dr["URL"].ToString(),
                                            dr["URL2"].ToString(),
                                            dr["thumbnailfile"].ToString(),
                                            
                                            dr.GetInt32(dr.GetOrdinal("QUANTITY_AVAILABLE")),
                                            
                                            dr.GetInt32(dr.GetOrdinal("maxqty_roo")),
                                            null, //dr["PUBID_COVER"].ToString(),
                                            null, //dr["PRODUCTID_COVER"].ToString(),
                                            null, //dr["BOOKSTATUS_COVER"].ToString(),
                                            null, //dr["DISPLAYSTATUS_COVER"].ToString(),
                                            null, //dr["URL_COVER"].ToString(),

                                            0, //dr.GetInt32(dr.GetOrdinal("QTY_COVER")),

                                            0, //dr.GetInt32(dr.GetOrdinal("MAXQTY_ROO_COVER")),
                                            dr["AUDIENCE"].ToString(),
                                            dr["AWARDS"].ToString(),
                                            dr["CANCERTYPE"].ToString(),
                                            dr["LANGUAGE"].ToString(),
                                            dr["FORMAT"].ToString(),
                                            dr["SERIES"].ToString(),
                                            dr["SUBJECT"].ToString(),
                                            dr["NIHNUM"].ToString(),
                                            (dr["recordcreatedate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)dr["recordcreatedate"],
                                            (dr["REVISED_MONTH"] == DBNull.Value) ? 0 : dr.GetInt32(dr.GetOrdinal("REVISED_MONTH")),
                                            (dr["REVISED_DAY"] == DBNull.Value) ? 0 : dr.GetInt32(dr.GetOrdinal("REVISED_DAY")),
                                            (dr["REVISED_YEAR"] == DBNull.Value) ? 0 : dr.GetInt32(dr.GetOrdinal("REVISED_YEAR")),
                                            (dr["REVISED_DATE"] == DBNull.Value) ? DateTime.MinValue : (DateTime)dr["REVISED_DATE"],
                                            (dr["REVISED_DATE_TYPE"] == DBNull.Value) ? "" : dr["REVISED_DATE_TYPE"].ToString(),
                                            dr.GetInt32(dr.GetOrdinal("TOTAL_NUM_PAGE")),
                                            dr["PUB_STATUS"].ToString(),
                                            dr["PDFURL"].ToString(),
                                            dr["LARGEIMAGEFILE"].ToString(),
                                            (dr["NCIPLFEATURED_IMAGEFILE"] == DBNull.Value) ? "" : dr["NCIPLFEATURED_IMAGEFILE"].ToString()
                                            );
                    
                    
                    coll.AddItemsToCollWithRules(k);
                }
                return (coll);
            }
        }

        //BEGIN HITT 8329 - CR 21 #############################################

        public static KVPairCollection GetKVPair2(string procname, string param)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand(procname);
            db.AddInParameter(cw, "csv", DbType.String, param);
            using (System.Data.IDataReader dr = db.ExecuteReader(cw))
            {
                KVPairCollection coll = new KVPairCollection();
                while (dr.Read())
                {
                    KVPair k = new KVPair(
                                             dr.GetInt32(0).ToString(),
                                            (dr[1] == DBNull.Value) ? "" : dr[1].ToString()
                                         );
                    coll.Add(k);
                }
                return (coll);
            }
        }

        //Get the Product Ids for the provided PubIds
        private static ArrayList GetProductIds(ArrayList arrlstPubId)
        {
            ArrayList arrlstProductIds = new ArrayList();

            //Get the Pubs Arraylist into a comma separated string value
            //arrlstPubId.ToString();
            //string PubIdList = string.Join(",", (string[])arrlstPubId.ToArray(Type.GetType("System.Int32")));

            string PubIdList = "";
            int Counter = 0;
            int MaxCount = arrlstPubId.Count;
            for (Counter = 0; Counter < MaxCount; Counter++)
            {
                if (Counter == MaxCount - 1)
                    PubIdList += arrlstPubId[Counter].ToString();
                else
                    PubIdList += arrlstPubId[Counter].ToString() + ",";
            }

            //string PubIdList = string.Join(",", (string[])arrlstPubId.ToArray());
            KVPairCollection kvPairColl = GetKVPair2("sp_getAllProductIds", PubIdList);

            for (Counter = 0; Counter < MaxCount; Counter++)
            {
                foreach (KVPair kvPair in kvPairColl)
                {
                    if (string.Compare(arrlstPubId[Counter].ToString(), kvPair.Key) == 0)
                    {
                        arrlstProductIds.Add(kvPair.Val);
                        break;
                    }
                }
            }

            return arrlstProductIds;
        }

        /******TO DO STARTING HERE and DOWNWARDS***************/

        public static bool SaveRawOrder(
            string SOrganization,
            string SFullname,
            string SAddr1,
            string SAddr2,
            string SCity,
            string SState,
            string SZip5,
            string SZip4,
            string SPhone,
            string SFax,
            string SEmail,
            
            string SProvince,
            string SCountry,
            string CostRecoveryInd,

            string pubids,
            string qtys,
            string ipaddress,
            string Interfacename,
            string International
            )
        {

            try
            {

                Database db = DatabaseFactory.CreateDatabase();
                DbCommand cw = db.GetStoredProcCommand("sp_NCIPL_SaveRawOrder");
                
                db.AddInParameter(cw, "@shporg", DbType.String, SOrganization);
                db.AddInParameter(cw, "shpname", DbType.String, SFullname);
                db.AddInParameter(cw, "shpaddr1", DbType.String, SAddr1);
                db.AddInParameter(cw, "shpaddr2", DbType.String, SAddr2);
                db.AddInParameter(cw, "shpcity", DbType.String, SCity);
                db.AddInParameter(cw, "shpstate", DbType.String, SState);
                db.AddInParameter(cw, "shpzip", DbType.String, SZip5);
                db.AddInParameter(cw, "shpzip4", DbType.String, SZip4);
                db.AddInParameter(cw, "shpphone", DbType.String, SPhone);
                db.AddInParameter(cw, "shpfax", DbType.String, SFax);
                db.AddInParameter(cw, "shpemail", DbType.String, SEmail);

                db.AddInParameter(cw, "ipaddr", DbType.String, ipaddress);
                db.AddInParameter(cw, "shpprovince", DbType.String, SProvince);
                db.AddInParameter(cw, "shpcountry", DbType.String, SCountry);
                db.AddInParameter(cw, "costrecoverind", DbType.String, CostRecoveryInd);
                db.AddInParameter(cw, "interface", DbType.String, Interfacename);
                db.AddInParameter(cw, "international", DbType.String, International);

                //Get the PubId Array
                ArrayList arrlstPubIds = new ArrayList();
                string[] tempPubArray = pubids.Split(',');
                foreach (string p in tempPubArray)
                {
                    arrlstPubIds.Add(p);
                }
                
                //Get the ProductId Array
                ArrayList arrlstProductIds = GetProductIds(arrlstPubIds);
                
                //Get the Qty Array
                ArrayList arrlstQtys = new ArrayList();
                string[] tempQtyArray = qtys.Split(',');
                foreach (string q in tempQtyArray)
                {
                    arrlstQtys.Add(q);
                }

                string shoppingcart = "";
                string delim = "";

                //Build the cart parameter for the stored procedure
                for (int i = 0; i < arrlstPubIds.Count; i++)
                {
                    //***EAC Just need the PUBID, PRODUCTID and QTY
                    shoppingcart += delim + arrlstPubIds[i].ToString() + ",'" + arrlstProductIds[i].ToString() + "'," + arrlstQtys[i].ToString();
                    delim = "|";
                }
                db.AddInParameter(cw, "cart", DbType.String, shoppingcart);

                db.ExecuteNonQuery(cw);
            }
            catch (Exception Ex)
            {
                //Write to log
                LogEntry logEnt = new LogEntry();
                logEnt.Message = "\r\n" + "SaveRawOrder Error." + "\r\n" + "Source: " + Ex.Source + "\r\n" + "Description: " + Ex.Message + "\r\n" + "Stack Trace: " + Ex.StackTrace;
                Logger.Write(logEnt, "Logs");
            }

            return (true);
        }
        //END HITT 8329 #############################################

        //BEGIN CR - 23 -- Canned Search
        public static KVPairCollection GetCannedSearchIdText(string procname, string param)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand(procname);
            db.AddInParameter(cw, "recordid", DbType.String, param);
            using (System.Data.IDataReader dr = db.ExecuteReader(cw))
            {
                KVPairCollection coll = new KVPairCollection();
                while (dr.Read())
                {
                    KVPair k = new KVPair(
                                             dr.GetInt32(0).ToString(),
                                            (dr[1] == DBNull.Value) ? "" : dr[1].ToString()
                                         );
                    coll.Add(k);
                }
                return (coll);
            }
        }

        public static ProductCollection GetCannedSearchProducts(int CannId)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand("sp_NCIPL_CannedSearch");

            db.AddInParameter(cw, "cannid", DbType.Int32, CannId);
            
            using (System.Data.IDataReader dr = db.ExecuteReader(cw))
            {
                ProductCollection coll = new ProductCollection();
                while (dr.Read())
                {   
                    Product k = new Product(
                                            dr.GetInt32(dr.GetOrdinal("pubid")),
                                            dr["productid"].ToString(),
                                            dr["BOOKSTATUS"].ToString(),
                                            dr["DISPLAYSTATUS"].ToString(),
                                            dr["longtitle"].ToString(),
                                            dr["shorttitle"].ToString(),
                                            dr["abstract"].ToString(),
                                            dr["summary"].ToString(),

                                            (dr["RECORDUPDATEDATE"] == DBNull.Value) ? "" : dr["RECORDUPDATEDATE"].ToString(),

                                            (dr["RECORDUPDATEDATE"] == DBNull.Value) ? DateTime.MinValue : (DateTime)dr["RECORDUPDATEDATE"],

                                            dr.GetInt32(dr.GetOrdinal("ISONLINE")),
                                            dr["URL"].ToString(),
                                            dr["URL2"].ToString(),
                                            dr["thumbnailfile"].ToString(),

                                            dr.GetInt32(dr.GetOrdinal("QUANTITY_AVAILABLE")),

                                            dr.GetInt32(dr.GetOrdinal("maxqty_ncipl")),
                                            dr["PUBID_COVER"].ToString(),
                                            dr["PRODUCTID_COVER"].ToString(),
                                            dr["BOOKSTATUS_COVER"].ToString(),
                                            dr["DISPLAYSTATUS_COVER"].ToString(),
                                            dr["URL_COVER"].ToString(),

                                            dr.GetInt32(dr.GetOrdinal("QTY_COVER")),

                                            dr.GetInt32(dr.GetOrdinal("MAXQTY_NCIPL_COVER")),
                                            dr["AUDIENCE"].ToString(),
                                            dr["AWARDS"].ToString(),
                                            dr["CANCERTYPE"].ToString(),
                                            dr["LANGUAGE"].ToString(),
                                            dr["FORMAT"].ToString(),
                                            dr["SERIES"].ToString(),
                                            dr["SUBJECT"].ToString(),
                                            dr["NIHNUM"].ToString(),
                                            (dr["recordcreatedate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)dr["recordcreatedate"],
                                            (dr["REVISED_MONTH"] == DBNull.Value) ? 0 : dr.GetInt32(dr.GetOrdinal("REVISED_MONTH")),
                                            (dr["REVISED_DAY"] == DBNull.Value) ? 0 : dr.GetInt32(dr.GetOrdinal("REVISED_DAY")),
                                            (dr["REVISED_YEAR"] == DBNull.Value) ? 0 : dr.GetInt32(dr.GetOrdinal("REVISED_YEAR")),
                                            (dr["REVISED_DATE"] == DBNull.Value) ? DateTime.MinValue : (DateTime)dr["REVISED_DATE"],
                                            (dr["REVISED_DATE_TYPE"] == DBNull.Value) ? "" : dr["REVISED_DATE_TYPE"].ToString(),
                                            dr.GetInt32(dr.GetOrdinal("TOTAL_NUM_PAGE")),
                                            dr["PUB_STATUS"].ToString(),
                                            dr["PDFURL"].ToString(),
                                            dr["LARGEIMAGEFILE"].ToString(),
                                            (dr["NCIPLFEATURED_IMAGEFILE"] == DBNull.Value) ? "" : dr["NCIPLFEATURED_IMAGEFILE"].ToString()
                                            );

                    //Product k = new Product(1,"111","111111 1 11 1","111 ","11111","","","","",DateTime.Now,1,1,1,"fake1.jpg",1,1);
                    //coll.Add(k);
                    coll.AddItemsToCollWithRules(k);
                }
                return (coll);
            }
        }
        //END CR - 23

        ///Begin CR - 30 : HITT 8719
        public static ProductCollection GetProductTranslations(int pubid)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("sp_NCIPL_getProductTranslations"); //JPJ Jan 27, 12 Using NCIPL Proc, but okay since accepts ROO interface as parameter

            db.AddInParameter(dbCommand, "pubid", DbType.Int32, pubid);
            db.AddInParameter(dbCommand, "interface", DbType.String, "ROO");

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                ProductCollection collProducts = new ProductCollection();
                while (dr.Read())
                {
                    Product ProductTranslation = new Product(
                                                            dr.GetInt32(dr.GetOrdinal("pubid")),
                                                            dr["productid"].ToString(),
                                                            dr["BOOKSTATUS"].ToString(),
                                                            dr["DISPLAYSTATUS"].ToString(),
                                                            dr["LANGUAGE"].ToString()
                                                        );
                    collProducts.AddItemsToCollWithRules(ProductTranslation);

                }
                return collProducts;
            }
        }
        ///End CR - 30
        
        ///Begin CR-28 July 9, 2010
        public static IDataReader GetIndexLetters()
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("sp_NCIPLCC_getLetterIndex"); //***EAC 20130214 ok to use nciplcc version
            return db.ExecuteReader(dbCommand);
        }
        ///End CR-28


        //NCIPLCC Get POS_INSTRUCTION for the pub id
        public static IDataReader GetPubPosInstruction(int pubid)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("sp_NCIPLCC_getPOSInstructionByPubId");   //***EAC 20130214 ok to use nciplcc version

            db.AddInParameter(dbCommand, "pubid", DbType.Int32, pubid);
            return db.ExecuteReader(dbCommand);
        }

        public static CustomerCollection GetSearchOrderResult(int Search_CustomerType, string Search_KeyWord, string SearchSort, string Start_Date, string End_Date, int FromRow, int ToRow)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbc;
            dbc = db.GetStoredProcCommand("sp_NCIPLCC_SearchOrder");    //***EAC 20130214 ok to use nciplcc version
            db.AddInParameter(dbc, "Search_CustomerType", DbType.Int32, Search_CustomerType);
            db.AddInParameter(dbc, "Search_KeyWord", DbType.String, Search_KeyWord);
            db.AddInParameter(dbc, "SearchSort", DbType.String, SearchSort);
            db.AddInParameter(dbc, "Start_Date", DbType.String, Start_Date);
            db.AddInParameter(dbc, "End_Date", DbType.String, End_Date);
            db.AddInParameter(dbc, "FromRow", DbType.Int32, FromRow);
            db.AddInParameter(dbc, "ToRow", DbType.Int32, ToRow);
            
            using (System.Data.IDataReader dr = ExecuteReader(db, dbc))
            {
                CustomerCollection coll = new CustomerCollection();
                while (dr.Read())
                {
                    Customer o = new Customer((int)dr["custid"],
                                                dr["shiptoname"].ToString(),
                                                dr["shiptoorgname"].ToString(),
                                                dr["shiptoaddr1"].ToString(),
                                                dr["shiptoaddr2"].ToString(),
                                                dr["shiptocity"].ToString(),
                                                dr["shiptostate"].ToString(),
                                                dr["shiptozip"].ToString(),
                                                dr["shiptozip4"].ToString(),
                                                dr["shiptophone"].ToString(),
                                                dr["shiptofax"].ToString(),
                                                dr["shiptoemail"].ToString(),
                                                dr["shiptoprovince"].ToString(),
                                                dr["shiptocountrycode"].ToString());
                    coll.Add(o);
                }
                return (coll);
            }
        }

        public static int GetSearchOrderCount(int Search_CustomerType, string Search_KeyWord, string Start_Date, string End_Date)
        {
            int searchorder_count = 0;
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbc;
            dbc = db.GetStoredProcCommand("sp_NCIPLCC_SearchOrderCount");   //***EAC 20130214 ok to use nciplcc version
            db.AddInParameter(dbc, "Search_CustomerType", DbType.Int32, Search_CustomerType);
            db.AddInParameter(dbc, "Search_KeyWord", DbType.String, Search_KeyWord);
            db.AddInParameter(dbc, "Start_Date", DbType.String, Start_Date);
            db.AddInParameter(dbc, "End_Date", DbType.String, End_Date);

            using (System.Data.IDataReader dr = ExecuteReader(db, dbc))
            {
                if (dr.Read())
                {
                    searchorder_count = (int)dr["SEARCHORDER_COUNT"];
                }
                return (searchorder_count);
            }
        
        }

        public static StringCollection GetOrderByCustomer(int CustomerID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbc;
            dbc = db.GetStoredProcCommand("sp_NCIPLCC_GetOrderByCustomer");     //***EAC 20130214 ok to use nciplcc version
            db.AddInParameter(dbc, "CustomerID", DbType.Int32, CustomerID);

            using (System.Data.IDataReader dr = ExecuteReader(db, dbc))
            {
                StringCollection coll = new StringCollection();
                while (dr.Read())
                {
                    coll.Add(dr["ORDERSEQNUM"].ToString());
                }
                return (coll);
            }
        }

        public static KVPairCollection GetOrder_Date_ByCustomer(int CustomerID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbc;
            dbc = db.GetStoredProcCommand("sp_NCIPLCC_GetOrder_Date_ByCustomer");   //***EAC 20130214 ok to use nciplcc version
            db.AddInParameter(dbc, "CustomerID", DbType.Int32, CustomerID);

            using (System.Data.IDataReader dr = ExecuteReader(db, dbc))
            {
                KVPairCollection coll = new KVPairCollection();
                while (dr.Read())
                {
                    KVPair k = new KVPair(dr["ORDERSEQNUM"].ToString(), dr["CREATEDATE"].ToString());
                    coll.Add(k);
                }
                return (coll);
            }
        }

        public static Customer GetOrderCustInfobyOrderNum(string OrderNum)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbc;
            dbc = db.GetStoredProcCommand("sp_NCIPLCC_GetCustInfobyOrderNum");  //***EAC 20130214 ok to use nciplcc version
            db.AddInParameter(dbc, "OrderNum", DbType.String, OrderNum);

            using (System.Data.IDataReader dr = ExecuteReader(db, dbc))
            {
                Customer Cust = new Customer();
                if (dr.Read())
                {
                    Cust.CustID = (int)dr["custid"];
                    Cust.ShipToName = dr["shiptoname"].ToString();
                    Cust.ShipToOrg = dr["shiptoorgname"].ToString();
                    Cust.ShipToAddr1 = dr["shiptoaddr1"].ToString();
                    Cust.ShipToAddr2 = dr["shiptoaddr2"].ToString();
                    Cust.ShipToCity = dr["shiptocity"].ToString();
                    Cust.ShipToState = dr["shiptostate"].ToString();
                    Cust.ShipToZip5 = dr["shiptozip"].ToString();
                    Cust.ShipToZip4 = dr["shiptozip4"].ToString();
                    Cust.ShipToPhone = dr["shiptophone"].ToString();
                    Cust.ShipToFax = dr["shiptofax"].ToString();
                    Cust.ShipToEmail = dr["shiptoemail"].ToString();
                    Cust.ShipToProvince = dr["shiptoprovince"].ToString();
                    Cust.ShipToCountry = dr["shiptocountrycode"].ToString();
                    Cust.BillToName = dr["billtoname"].ToString();
                    Cust.BillToOrg = dr["billtoorgname"].ToString();
                    Cust.BillToAddr1 = dr["billtoaddr1"].ToString();
                    Cust.BillToAddr2 = dr["billtoaddr2"].ToString();
                    Cust.BillToCity = dr["billtocity"].ToString();
                    Cust.BillToState = dr["billtostate"].ToString();
                    Cust.BillToZip5 = dr["billtozip"].ToString();
                    Cust.BillToZip4 = dr["billtozip4"].ToString();
                    Cust.BillToPhone = dr["billtophone"].ToString();
                    Cust.BillToFax = dr["billtofax"].ToString();
                    Cust.BillToEmail = dr["billtoemail"].ToString();
                    Cust.BillToProvince = dr["billtoprovince"].ToString();
                    Cust.BillToCountry = dr["billtocountrycode"].ToString();
                }
                return (Cust);
            }

        }

        public static Customer GetOrderCustInfobyCustID(string CustID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbc;
            dbc = db.GetStoredProcCommand("sp_NCIPLCC_GetCustInfobyCustID");    //***EAC 20130214 ok to use nciplcc version
            db.AddInParameter(dbc, "CustID", DbType.String, CustID);

            using (System.Data.IDataReader dr = ExecuteReader(db, dbc))
            {
                Customer Cust = new Customer();
                if (dr.Read())
                {
                    Cust.CustID = (int)dr["custid"];
                    Cust.ShipToName = dr["shiptoname"].ToString();
                    Cust.ShipToOrg = dr["shiptoorgname"].ToString();
                    Cust.ShipToAddr1 = dr["shiptoaddr1"].ToString();
                    Cust.ShipToAddr2 = dr["shiptoaddr2"].ToString();
                    Cust.ShipToCity = dr["shiptocity"].ToString();
                    Cust.ShipToState = dr["shiptostate"].ToString();
                    Cust.ShipToZip5 = dr["shiptozip"].ToString();
                    Cust.ShipToZip4 = dr["shiptozip4"].ToString();
                    Cust.ShipToPhone = dr["shiptophone"].ToString();
                    Cust.ShipToFax = dr["shiptofax"].ToString();
                    Cust.ShipToEmail = dr["shiptoemail"].ToString();
                    Cust.ShipToProvince = dr["shiptoprovince"].ToString();
                    Cust.ShipToCountry = dr["shiptocountrycode"].ToString();
                    Cust.BillToName = dr["billtoname"].ToString();
                    Cust.BillToOrg = dr["billtoorgname"].ToString();
                    Cust.BillToAddr1 = dr["billtoaddr1"].ToString();
                    Cust.BillToAddr2 = dr["billtoaddr2"].ToString();
                    Cust.BillToCity = dr["billtocity"].ToString();
                    Cust.BillToState = dr["billtostate"].ToString();
                    Cust.BillToZip5 = dr["billtozip"].ToString();
                    Cust.BillToZip4 = dr["billtozip4"].ToString();
                    Cust.BillToPhone = dr["billtophone"].ToString();
                    Cust.BillToFax = dr["billtofax"].ToString();
                    Cust.BillToEmail = dr["billtoemail"].ToString();
                    Cust.BillToProvince = dr["billtoprovince"].ToString();
                    Cust.BillToCountry = dr["billtocountrycode"].ToString();
                }
                return (Cust);
            }

        }

        public static ProductCollection GetHistOrderDetail(string OrderNum)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("sp_NCIPLLM_GetHistOrderDetail");
            db.AddInParameter(dbCommand, "OrderNum", DbType.String, OrderNum);
                        
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                ProductCollection collProducts = new ProductCollection();
                while (dr.Read())
                {
                    Product ProductTranslation = new Product(
                                                            dr.GetInt32(dr.GetOrdinal("pubid")),
                                                            dr["cpjid"].ToString(),
                                                            dr["longtitle"].ToString(),
                                                            dr.GetInt32(dr.GetOrdinal("quantity"))
                                                        );
                    collProducts.Add(ProductTranslation);

                }
                return collProducts;
            }
        }

        public static ProductCollection GetOrigOrderDetail(string OrderNum)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("sp_NCIPLLM_GetOrigOrderDetail");
            db.AddInParameter(dbCommand, "OrderNum", DbType.String, OrderNum);

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                ProductCollection collProducts = new ProductCollection();
                while (dr.Read())
                {
                    Product ProductTranslation = new Product(
                                                            dr.GetInt32(dr.GetOrdinal("pubid")),
                                                            dr["cpjid"].ToString(),
                                                            dr["longtitle"].ToString(),
                                                            dr.GetInt32(dr.GetOrdinal("quantity"))
                                                        );
                    collProducts.Add(ProductTranslation);

                }
                return collProducts;
            }
        }

        public static bool SaveCustomerInfo(string CustID, Person shipto, Person billto)
        {

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand("sp_NCIPLCC_SaveCustomerInfo");  //***EAC 20130214 ok to use nciplcc version
            db.AddInParameter(cw, "CustID", DbType.String, CustID);
            db.AddInParameter(cw, "shpname", DbType.String, shipto.Fullname);
            db.AddInParameter(cw, "shporg", DbType.String, shipto.Organization);
            db.AddInParameter(cw, "shpaddr1", DbType.String, shipto.Addr1);
            db.AddInParameter(cw, "shpaddr2", DbType.String, shipto.Addr2);
            db.AddInParameter(cw, "shpcity", DbType.String, shipto.City);
            db.AddInParameter(cw, "shpstate", DbType.String, shipto.State);
            db.AddInParameter(cw, "shpzip5", DbType.String, shipto.Zip5);
            db.AddInParameter(cw, "shpzip4", DbType.String, shipto.Zip4);
            db.AddInParameter(cw, "shpphone", DbType.String, shipto.Phone);
            db.AddInParameter(cw, "shpemail", DbType.String, shipto.Email);


            db.AddInParameter(cw, "bilname", DbType.String, billto.Fullname);
            db.AddInParameter(cw, "bilorg", DbType.String, billto.Organization);
            db.AddInParameter(cw, "biladdr1", DbType.String, billto.Addr1);
            db.AddInParameter(cw, "biladdr2", DbType.String, billto.Addr2);
            db.AddInParameter(cw, "bilcity", DbType.String, billto.City);
            db.AddInParameter(cw, "bilstate", DbType.String, billto.State);
            db.AddInParameter(cw, "bilzip5", DbType.String, billto.Zip5);
            db.AddInParameter(cw, "bilzip4", DbType.String, billto.Zip4);
            db.AddInParameter(cw, "bilphone", DbType.String, billto.Phone);
            db.AddInParameter(cw, "bilemail", DbType.String, billto.Email);
            
            try
            {
                db.ExecuteNonQuery(cw);
                return (true);
            }
            catch (Exception e)
            {
                return (false); //***EAC dont care what problem was...
            }
        }


        public static IDataReader GetOrderStatus(string OrderNum)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("sp_NCIPLCC_GetOrderStatus");     //***EAC 20130214 ok to use nciplcc version

            db.AddInParameter(dbCommand, "OrderNum", DbType.String, OrderNum);
            return db.ExecuteReader(dbCommand);
        }

        public static ProductCollection GetOutofStockPubs()
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand("sp_NCIPLLM_GetOutofStockPubs");

            using (System.Data.IDataReader dr = db.ExecuteReader(cw))
            {
                ProductCollection coll = new ProductCollection();
                while (dr.Read())
                {

                    Product k = new Product(
                                            dr.GetInt32(dr.GetOrdinal("pubid")),
                                            dr["productid"].ToString(),
                                            dr["BOOKSTATUS"].ToString(),
                                            dr["DISPLAYSTATUS"].ToString(),
                                            dr["longtitle"].ToString(),
                                            dr["shorttitle"].ToString(),
                                            dr["abstract"].ToString(),
                                            dr["summary"].ToString(),

                                            (dr["RECORDUPDATEDATE"] == DBNull.Value) ? "" : dr["RECORDUPDATEDATE"].ToString(),

                                            (dr["RECORDUPDATEDATE"] == DBNull.Value) ? DateTime.MinValue : (DateTime)dr["RECORDUPDATEDATE"],

                                            dr.GetInt32(dr.GetOrdinal("ISONLINE")),
                                            dr["URL"].ToString(),
                                            dr["URL2"].ToString(),
                                            dr["thumbnailfile"].ToString(),
                                            dr.GetInt32(dr.GetOrdinal("QUANTITY_AVAILABLE")),
                                            dr.GetInt32(dr.GetOrdinal("maxqty_roo")),

                                            "",//nerdo field: obsolete
                                            "",//nerdo field: obsolete
                                            "",//nerdo field: obsolete
                                            "",//nerdo field: obsolete
                                            "",//nerdo field: obsolete
                                            0,//nerdo field: obsolete
                                            0,//nerdo field: obsolete

                                            dr["AUDIENCE"].ToString(),
                                            dr["AWARDS"].ToString(),
                                            dr["CANCERTYPE"].ToString(),
                                            dr["LANGUAGE"].ToString(),
                                            dr["FORMAT"].ToString(),
                                            dr["SERIES"].ToString(),
                                            dr["SUBJECT"].ToString(),
                                            dr["NIHNUM"].ToString(),
                                            (dr["recordcreatedate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)dr["recordcreatedate"],
                                            (dr["REVISED_MONTH"] == DBNull.Value) ? 0 : dr.GetInt32(dr.GetOrdinal("REVISED_MONTH")),
                                            (dr["REVISED_DAY"] == DBNull.Value) ? 0 : dr.GetInt32(dr.GetOrdinal("REVISED_DAY")),
                                            (dr["REVISED_YEAR"] == DBNull.Value) ? 0 : dr.GetInt32(dr.GetOrdinal("REVISED_YEAR")),
                                            (dr["REVISED_DATE"] == DBNull.Value) ? DateTime.MinValue : (DateTime)dr["REVISED_DATE"],
                                            (dr["REVISED_DATE_TYPE"] == DBNull.Value) ? "" : dr["REVISED_DATE_TYPE"].ToString(),
                                            dr.GetInt32(dr.GetOrdinal("TOTAL_NUM_PAGE")),
                                            dr["PUB_STATUS"].ToString(),
                                            dr["PDFURL"].ToString(),
                                            dr["LARGEIMAGEFILE"].ToString(),
                                            (dr["NCIPLFEATURED_IMAGEFILE"] == DBNull.Value) ? "" : dr["NCIPLFEATURED_IMAGEFILE"].ToString()
                                            );


                    coll.AddItemsToCollWithRules(k);
                }
                return (coll);
            }
        }
        public static int IsPromotionPub(int PubID)
        {
            int iPromotionPub = 0;
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbc;
            dbc = db.GetStoredProcCommand("sp_NCIPLCC_IsPromotionPub");     //***EAC ok to use NCIPLCC version
            db.AddInParameter(dbc, "PubID", DbType.Int32, PubID);

            using (System.Data.IDataReader dr = ExecuteReader(db, dbc))
            {
                if (dr.Read())
                {
                    iPromotionPub = (int)dr["PROMOTION_FLAG"];
                }
                return (iPromotionPub);
            }

        }
    }
}
