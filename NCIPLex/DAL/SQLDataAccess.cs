using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//Manually added
using System.Data;
using System.Data.Common;
using System.Configuration;
//using PubEnt.BLL;
using System.Collections;
//EntLib References
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using NCIPLex.BLL;
using NCIPLex.GlobalUtils;
namespace NCIPLex.DAL
{
    public class SQLDataAccess
    {
        #region CONSTANTS
        private static readonly string strSpGetAllConfName = "sp_NCIPLex_GetAllConfName";
        #endregion

        public static int GetTimeout(int appID, int timeoutID)
        {
            if (string.Compare(ConfigurationManager.AppSettings["TimeOutFromFile"], "1", true) == 0)
            {
                int timeoutvalue = 0;
                if (timeoutID == 2) ///Page timeout
                    timeoutvalue = Int32.Parse(ConfigurationManager.AppSettings["PageTimeOut"]);
                if (timeoutID == 3) ///Session (or counter) timeout
                    timeoutvalue = Int32.Parse(ConfigurationManager.AppSettings["SessionTimeOut"]);
                return timeoutvalue; ///Return Here
            }

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand("sp_NCIPLex_GetTimeout");
            db.AddInParameter(cw, "appID", DbType.Int32, appID);
            db.AddInParameter(cw, "timeoutID", DbType.Int32, timeoutID);
            using (System.Data.IDataReader dr = db.ExecuteReader(cw))
            {
                if (dr.Read())
                {
                    return (dr.GetInt32(0));
                }
            }
            throw (new ArgumentOutOfRangeException("Timeout", "Value is missing"));
        }

        private static System.Data.IDataReader ExecuteReader(Database db, DbCommand dbCmd)
        {
            if (db == null) throw (new ArgumentNullException("db"));
            if (dbCmd == null) throw (new ArgumentNullException("dbCmd"));

            return (db.ExecuteReader(dbCmd));
        }

        public static Confs GetAllConf()
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSpGetAllConfName);
            using (System.Data.IDataReader dr = ExecuteReader(db, dbCommand))
            {
                Confs coll = new Confs();
                Conf newConf;

                while (dr.Read())
                {
                    newConf = new Conf(Convert.ToInt32(dr["CONFERENCEID"]),
                        (string)dr["CONFERENCENAME"]);
                    coll.Add(newConf);
                }
                return (coll);
            }
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

        public static KVPairCollection GetKVPairStr(string s)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand(s);
            using (System.Data.IDataReader dr = db.ExecuteReader(cw))
            {
                KVPairCollection coll = new KVPairCollection();
                while (dr.Read())
                {
                    KVPair k = new KVPair((string)dr.GetString(0), (string)dr.GetString(1));
                    coll.Add(k);
                }
                return (coll);
            }
        }


        public static Product GetProductbyProductID(string productid)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand("sp_NCIPLex_getPubByProductID");
            db.AddInParameter(cw, "productid", DbType.String, productid);
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
                    return (k);
                }
                else throw new ArgumentException("Publication ID not found", "value");

            }
        }
        public static Product GetProductbyPubID(int pubid)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand("sp_NCIPLex_getPubByPubID");
            db.AddInParameter(cw, "pubid", DbType.Int32, pubid);
            using (System.Data.IDataReader dr = db.ExecuteReader(cw))
            {
                if (dr.Read())
                {
                    //Product k = new Product((int)dr.GetInt32(1),
                    //                    dr["PRODUCTID"].ToString(),
                    //                    dr["LONGTITLE"].ToString(),
                    //                    "todo:shorttitle",
                    //                    "todo:abstract",
                    //                    "todo:summary",
                    //                    dr["URL"].ToString(),
                    //                    dr["URL2"].ToString(),  //EAC I am guessing URL2 is NERDO URL
                    //                    dr["RECORDUPDATEDATE"].ToString(),
                    //                    DateTime.Now, //EAC not needed since we already have the string vers (DateTime)dr["RECORDUPDATEDATE"]
                    //                    (int)dr.GetInt32(7),    //orderdisplaystatus 
                    //                    (int)dr.GetInt32(5),    //onlinedisplaystatus
                    //                    (int)dr.GetInt32(8),    //nerdodisplaystatus
                    //                    dr["THUMBNAILFILE"].ToString(),
                    //                    1,                      //default value of 1 for qty_ordered
                    //                    (int)dr.GetInt32(11)
                    //                    );
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
                    return (k);
                }
                else throw new ArgumentException("Publication ID not found", "value");

            }

        }
        public static ProductCollection GetProductsByParams(string terms, string cantype, string subj, string aud, string form, string lang, string startswith, string series, string neworupdated, string race)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand("sp_NCIPLex_SearchContains");

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
                    //Product k = new Product((int)dr.GetInt32(1),
                    //                    dr["PRODUCTID"].ToString(),
                    //                    dr["LONGTITLE"].ToString(),
                    //                    "todo:shorttitle",
                    //                    "todo:abstract",
                    //                    "todo:summary",
                    //                    dr["URL"].ToString(),
                    //                    dr["URL2"].ToString(),  //EAC I am guessing URL2 is NERDO URL
                    //                    dr["RECORDUPDATEDATE"].ToString(),
                    //                    DateTime.Now, //EAC not needed since we already have the string vers (DateTime)dr["RECORDUPDATEDATE"]
                    //                    (int)dr.GetInt32(7),    //orderdisplaystatus
                    //                    (int)dr.GetInt32(5),    //onlinedisplaystatus
                    //                    (int)dr.GetInt32(8),    //nerdodisplaystatus
                    //                    dr["THUMBNAILFILE"].ToString(),
                    //                    1,                      //default value of 1 for qty_ordered
                    //                    (int)dr.GetInt32(11)
                    //                    );


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

        //public static ProductCollection GetAllProducts(string SortField)
        public static ProductCollection GetProductbyProductIDWithRules(string productid)
        {
            Database db = DatabaseFactory.CreateDatabase();
            //DbCommand dbCommand = db.GetStoredProcCommand(SP_GET_ALL_NCIPL_RECORDS);
            ////WITH PARAMS db.AddInParameter(dbCommand, "CategoryID", DbType.Int32, Category);
            //db.AddInParameter(dbCommand, "OrderByField", DbType.String, SortField);

            DbCommand cw = db.GetStoredProcCommand("sp_NCIPLex_getPubByProductID");
            db.AddInParameter(cw, "productid", DbType.String, productid);

            //A.PUBID, PRODUCTID, LONGTITLE, URL, RECORDUPDATEDATE, CANORDER, B.ISONLINE, ISNERDO, PUBIMAGE
            using (IDataReader dataReader = db.ExecuteReader(cw))
            {
                // Processing code
                ProductCollection collProducts = new ProductCollection();
                while (dataReader.Read())
                {
                    //Product newProduct = new Product(
                    //                     Convert.ToInt32(dataReader["PUBID"].ToString(), 10),
                    //                    (dataReader["PRODUCTID"] == DBNull.Value) ? "" : (string)dataReader["PRODUCTID"],
                    //                    (dataReader["LONGTITLE"] == DBNull.Value) ? "" : (string)dataReader["LONGTITLE"],
                    //                    "",
                    //                    "",
                    //                    "",
                    //                    (dataReader["URL"] == DBNull.Value) ? "" : (string)dataReader["URL"],
                    //                    "",
                    //                    (dataReader["RECORDUPDATEDATE"] == DBNull.Value) ? "" : dataReader["RECORDUPDATEDATE"].ToString(),
                    //                    (dataReader["RECORDUPDATEDATE"] == DBNull.Value) ? DateTime.MinValue : (DateTime)dataReader["RECORDUPDATEDATE"],
                    //                    (dataReader["CANORDER"] == DBNull.Value) ? 0 : (int)dataReader.GetInt32(5),
                    //                    (dataReader["ISONLINE"] == DBNull.Value) ? 0 : (int)dataReader.GetInt32(6),
                    //                    (dataReader["ISNERDO"] == DBNull.Value) ? 0 : (int)dataReader.GetInt32(7),
                    //                    (dataReader["PUBIMAGE"] == DBNull.Value) ? "" : (string)dataReader["PUBIMAGE"],
                    //                    1,                      //default value of 1 for qty_ordered
                    //                    0
                    //                    );
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

                                                    dataReader.GetInt32(dataReader.GetOrdinal("maxqty_ncipl")),
                                                    dataReader["PUBID_COVER"].ToString(),
                                                    dataReader["PRODUCTID_COVER"].ToString(),
                                                    dataReader["BOOKSTATUS_COVER"].ToString(),
                                                    dataReader["DISPLAYSTATUS_COVER"].ToString(),
                                                    dataReader["URL_COVER"].ToString(),

                                                    dataReader.GetInt32(dataReader.GetOrdinal("QTY_COVER")),

                                                    dataReader.GetInt32(dataReader.GetOrdinal("MAXQTY_NCIPL_COVER")),
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
        public static ProductCollection GetProductsByCSV(string csv)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("sp_NCIPLex_getPubsByCSV");

            db.AddInParameter(dbCommand, "csv", DbType.String, csv);


            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                // Processing code
                ProductCollection collProducts = new ProductCollection();
                while (dr.Read())
                {
                    //Product k = new Product((int)dr.GetInt32(1), //pubid
                    //                    dr["PRODUCTID"].ToString(),
                    //                    dr["LONGTITLE"].ToString(),
                    //                    "todo:shorttitle",
                    //                    "todo:abstract",
                    //                    "todo:summary",
                    //                    dr["URL"].ToString(),
                    //                    dr["URL2"].ToString(),  //EAC I am guessing URL2 is NERDO URL
                    //                    dr["RECORDUPDATEDATE"].ToString(),
                    //                    DateTime.Now, //EAC not needed since we already have the string vers (DateTime)dr["RECORDUPDATEDATE"]
                    //                    (int)dr.GetInt32(7),    //orderdisplaystatus
                    //                    (int)dr.GetInt32(5),    //onlinedisplaystatus
                    //                    (int)dr.GetInt32(8),    //nerdodisplaystatus
                    //                    dr["THUMBNAILFILE"].ToString(),
                    //                    1,                      //default value of 1 for qty_ordered
                    //                    (int)dr.GetInt32(11)
                    //                    );

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

                    //collProducts.Add(k);
                    collProducts.AddItemsToCollWithRules(k);
                }
                return collProducts;
            }
        }

        public static IDataReader GetIndexLetters()
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("sp_NCIPLex_getLetterIndex");
            return db.ExecuteReader(dbCommand);
        }

        public static ProductCollection GetProductTranslations(int pubid)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("sp_NCIPLex_getProductTranslations");

            db.AddInParameter(dbCommand, "pubid", DbType.Int32, pubid);
            db.AddInParameter(dbCommand, "interface", DbType.String, "NCIPL");

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

        public static IDataReader GetPubPhysicalDesc(int pubid)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("sp_NCIPLex_getPubPhysicalDescription");

            db.AddInParameter(dbCommand, "pubid", DbType.Int32, pubid);
            return db.ExecuteReader(dbCommand);
        }

        //Return collections that a pub belongs to
        public static IDataReader GetPubCollections(int pubid)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("sp_NCIPLex_getPubCollections");

            db.AddInParameter(dbCommand, "pubid", DbType.Int32, pubid);
            return db.ExecuteReader(dbCommand);
        }

        public static ProductCollection GetRelatedProducts(int pubid)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("sp_NCIPLex_getRelatedProducts");

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
            DbCommand cmd = db.GetStoredProcCommand("sp_NCIPLex_GetPubIdFromProductId");
            db.AddInParameter(cmd, "productid", DbType.String, ProductId);
            db.AddOutParameter(cmd, "returnvalue", DbType.Int32, 0);
            db.ExecuteNonQuery(cmd);
            int retvalue = (int)db.GetParameterValue(cmd, "returnvalue");
            return retvalue;
        }

        //public static bool SaveTransaction(Transaction t, string s, string pubids, string pubqtys, int confid, string shiplocation, out int returnvalue)
        //{

        //    int returnordernumber = 0;
        //    string OrderUploadPath = ConfigurationSettings.AppSettings["OrderUploadPath"];
        //    string EnvVariable = ConfigurationSettings.AppSettings["OrderUploadEnvironment"];
        //    string strIntlFile = ConfigurationSettings.AppSettings["IntlFileName"]; //Used only for International Order
        //    string strOrderSeparator = ConfigurationSettings.AppSettings["IntlOrderSeparator"]; //Used only for International Order

        //    string strOrderType = "";
        //    string strOrderCountry = "";

        //    string strOrderPO = GetConferenceName(confid, 15);


        //    //Log the Order Info just before calling order upload
        //    try
        //    {
        //        //Write to log
        //        LogEntry logEnt = new LogEntry();
        //        string logmessage = "\r\n";
        //        logmessage += "Conference PO            :" + strOrderPO + "\r\n";
        //        logmessage += "Org Name                 :" + "############" + "\r\n";
        //        logmessage += "Cont Name                :" + "############" + "\r\n";
        //        logmessage += "Addr1                    :" + "############" + "\r\n";
        //        logmessage += "Addr2                    :" + "############" + "\r\n";
        //        logmessage += "City                     :" + "############" + "\r\n";
        //        logmessage += "State                    :" + "############" + "\r\n";
        //        logmessage += "Zip                      :" + t.ShipTo.Zip5 + "-" + t.ShipTo.Zip4 + "\r\n";
        //        logmessage += "Pubs                     :" + pubids + "\r\n";
        //        logmessage += "Qtys                     :" + pubqtys + "\r\n";
        //        //logmessage += "CostRecovery             :" + ccCostRecovery + "\r\n";
        //        //logmessage += "TransId                  :" + t.CC.TransID + "\r\n";
        //        logEnt.Message = logmessage;
        //        Logger.Write(logEnt, "Logs");
        //    }
        //    catch (Exception Ex)
        //    {
        //        //Do nothing for now
        //    }
        //    //End of Logging Code

        //    if (string.Compare(shiplocation, "International", true) == 0)
        //    {
        //        strOrderType = "1";
        //        strOrderCountry = t.ShipTo.Country;
        //    }
        //    else
        //    {
        //        strOrderType = "";
        //        strOrderSeparator = "";
        //        strOrderCountry = "";
        //    }

        //    //Finally call order upload
        //    NCIPLex.OrderUpload ordUpload = new NCIPLex.OrderUpload(EnvVariable,        //Environment
        //                    ConfigurationManager.AppSettings["InterfaceName"],                      //Specify Application
        //                    "",                             //User Type - See explanation above
        //                    t.BillTo.Organization,          //BillToOrgzName
        //                    t.BillTo.Fullname,              //Bill ToContact Name
        //                    t.BillTo.Addr1,                 //BillToCustAddr
        //                    t.BillTo.Addr2,                 //BillToCustAddr2
        //                    t.BillTo.City,                  //BillToCustCity
        //                    t.BillTo.State,                 //BillToState
        //                    t.BillTo.Zip5,                  //BillToZip
        //                    t.BillTo.Zip4,                  //BillToZip4
        //                    t.BillTo.Phone,                 //BillToPhone
        //                    "",                             //BillToCountryCode - Presently uses only "US"
        //                    t.BillTo.Email,                 //BillToEmail

        //                    t.ShipTo.Organization,          //ShipToOrgName
        //                    t.ShipTo.Fullname,              //ShipTo Contact Name
        //                    t.ShipTo.Addr1,                 //ShipToCustAddr
        //                    t.ShipTo.Addr2,                 //ShipToCustAddr2
        //                    t.ShipTo.City,                  //ShipToCustCity
        //                    t.ShipTo.State,                 //ShipToState
        //                    t.ShipTo.Zip5,                  //ShipToZip
        //                    t.ShipTo.Zip4,                  //ShipToZip4
        //                    t.ShipTo.Phone,                 //ShipToPhone
        //                    "",                             //ShipToCountryCode - Presently uses only "US"
        //                    t.ShipTo.Email,                 //ShipToEmail

        //                    pubids,                         //Pub Ids - See explanation above
        //                    pubqtys,                        //Quantities - See explanation above
        //                    "",                             //IsCostRecoveryOn - Pass "1" for ON or "" for no cost recovery
        //        //"1", //IsccSwipeDone - Pass "1" or ""
        //                    "",                             //ccApprovalCode - The cc aproval code
        //                    "0.00",                         //ccAmount - pass the amount
        //                    "",                             //Type of Credit Card - Pass "M", "V" or "A" or pass "" if nothing
        //                    "",                             //ccNumber
        //                    "",                             //ccExpYYYY
        //                    "",                             //ccExpMM
        //                    "",                             //Credit Card Transaction Id
        //                    "",                             //ccSwipeAppCode - Credit Card Approval Code
        //                    "",                             //ccCVV2 - Credit Card CVV2
        //                    "",                             //ROO Cover Indicator - Pass "1" or ""
        //                    -1,                              //Customer ID
        //                    OrderUploadPath,                //Order Upload Path - Only pass the path to the OrderUpload directory, do not pass any file name.

        //                    strOrderPO,                     //EXHIBIT CONFERENCE NAME
        //                    strOrderType,                   //IS THIS EXHIBIT INTERNATIONAL ORDER ? - Pass "1" else Pass ""
        //                    strOrderSeparator,              //INTL ORDER SEPARATOR FOR EXHIBIT - pass the order separator "," if it is intel order, else pass ""
        //                    strIntlFile,                    //EXHIBIT International Order Upload file name.
        //                    strOrderCountry,                //EXHIBIT Country (For Intl Order)
        //                    s,                              //IP Address

        //                    "",                             //shipacctnum
        //                    0.00,                           //shipcost
        //                    "",                             //shipmethod
        //                    "",                             //shipvendor
        //                    "",                             //ordercreator
        //                    "",                             //ordermedia
        //                    out returnvalue,                //Success Indicator - See explanation above
        //                    out returnordernumber           //retordernumber
        //                    );

        //    //Log the output value
        //    if (returnvalue != 1) //Some Error happened, log the shipping org details and return code
        //    {
        //        try
        //        {
        //            //Write to log
        //            LogEntry logEnt = new LogEntry();
        //            string logmessage = "\r\n";
        //            logmessage += "WARNING: Order upload component prematurely exited." + "\r\n";
        //            logmessage += "Return Value: " + returnvalue.ToString() + "\r\n";
        //            logmessage += "Conference PO            :" + strOrderPO + "\r\n";
        //            logmessage += "Org Name                 :" + t.ShipTo.Organization + "\r\n";
        //            logmessage += "Cont Name                :" + t.ShipTo.Fullname + "\r\n";
        //            logmessage += "Zip                      :" + t.ShipTo.Zip5 + "-" + t.ShipTo.Zip4 + "\r\n";
        //            //logmessage += "CostRecovery             :" + "" + "\r\n";
        //            //logmessage += "TransId                  :" + "" + "\r\n";
        //            logEnt.Message = logmessage;
        //            Logger.Write(logEnt, "Logs");
        //        }
        //        catch (Exception Ex)
        //        {
        //            //throw (new ApplicationException("Cannot save order."));
        //            //Do not do anything
        //        }
        //    }

        //    return (true);
        //}

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

        private static string GetConferenceName(int ConfId, int ConfFieldLen)
        {
            string confName = "";
            int padLen = 0;
            int confSeq = 0;

            confName = SQLDataAccess.GetConfName(ConfId);
            confSeq = SQLDataAccess.GetConferenceSeq(confName.Trim());

            padLen = ConfFieldLen - confName.Length - confSeq.ToString().Length;
            return confName + Utils.StringPadLeft(confSeq.ToString(), (ConfFieldLen - confName.Length), '0');
        }

        public static string GetConfName(int ConfId)
        {
            string returnvalue = "";
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cmd = db.GetStoredProcCommand("sp_NCIPLex_GetConfName");
            db.AddInParameter(cmd, "confid", DbType.Int32, ConfId);
            db.AddOutParameter(cmd, "returnvalue", DbType.String, 100);
            db.ExecuteNonQuery(cmd);
            returnvalue = db.GetParameterValue(cmd, "returnvalue").ToString();
            return returnvalue;
        }

        public static int GetConferenceSeq(string confName)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("sp_NCIPLex_GetConferenceSeq");

            db.AddInParameter(dbCommand, "i_ConferenceName", DbType.AnsiString, confName);
            db.AddOutParameter(dbCommand, "o_ConferenceSeq", DbType.Int32, int.MaxValue);
            db.ExecuteNonQuery(dbCommand);
            return (int)db.GetParameterValue(dbCommand, "o_ConferenceSeq");
        }

        public static int GetIntl_MaxOrder(int ConfId)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("SP_EXHIBIT_INTL_MAXORDER");

            db.AddInParameter(dbCommand, "confid", DbType.Int32, ConfId);
            db.AddOutParameter(dbCommand, "intl_maxorder", DbType.Int32, int.MaxValue);
            db.ExecuteNonQuery(dbCommand);
            return (int)db.GetParameterValue(dbCommand, "intl_maxorder");
        }

        //Get New and Updated Publications
        public static ProductCollection GetNewUpdatedProducts()
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand("sp_NCIPL_GetNewUpdatedPubs");
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


                    coll.AddItemsToCollWithRules(k);
                }
                return (coll);
            }
        }

        public static bool SaveNewOrder(Transaction t, int confid, string ipaddr, string pubids, string pubqtys, out int returnvalue, out int retordernum)
        {
            returnvalue = -1;   //unused
            retordernum = -1;   //will return the orderseqnum
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand cw = db.GetStoredProcCommand("sp_NCIPLEX_SaveNewOrder");

                

                db.AddInParameter(cw, "shpname", DbType.String, t.ShipTo.Fullname);
                db.AddInParameter(cw, "shporg", DbType.String, t.ShipTo.Organization);
                db.AddInParameter(cw, "shpaddr1", DbType.String, t.ShipTo.Addr1);
                db.AddInParameter(cw, "shpaddr2", DbType.String, t.ShipTo.Addr2);
                db.AddInParameter(cw, "shpcity", DbType.String, t.ShipTo.City);
                db.AddInParameter(cw, "shpstate", DbType.String, t.ShipTo.State);
                db.AddInParameter(cw, "shpzip5", DbType.String, t.ShipTo.Zip5);
                db.AddInParameter(cw, "shpzip4", DbType.String, t.ShipTo.Zip4);
                db.AddInParameter(cw, "shpphone", DbType.String, t.ShipTo.Phone);
                db.AddInParameter(cw, "shpemail", DbType.String, t.ShipTo.Email);
                db.AddInParameter(cw, "shpcountry", DbType.String, t.ShipTo.Country);

                db.AddInParameter(cw, "bilname", DbType.String, t.BillTo.Fullname);
                db.AddInParameter(cw, "bilorg", DbType.String, t.BillTo.Organization);
                db.AddInParameter(cw, "biladdr1", DbType.String, t.BillTo.Addr1);
                db.AddInParameter(cw, "biladdr2", DbType.String, t.BillTo.Addr2);
                db.AddInParameter(cw, "bilcity", DbType.String, t.BillTo.City);
                db.AddInParameter(cw, "bilstate", DbType.String, t.BillTo.State);
                db.AddInParameter(cw, "bilzip5", DbType.String, t.BillTo.Zip5);
                db.AddInParameter(cw, "bilzip4", DbType.String, t.BillTo.Zip4);
                db.AddInParameter(cw, "bilphone", DbType.String, t.BillTo.Phone);
                db.AddInParameter(cw, "bilemail", DbType.String, t.BillTo.Email);
                db.AddInParameter(cw, "bilcountry", DbType.String, t.BillTo.Country);

                //***EAC All creditcard info blanked
                //db.AddInParameter(cw, "cctransid", DbType.String, "");
                //db.AddInParameter(cw, "ccamount", DbType.Double, 0.0);
                //db.AddInParameter(cw, "ccexpyy", DbType.String, "");
                //db.AddInParameter(cw, "ccexpmm", DbType.String, "");
                //db.AddInParameter(cw, "cccvv2", DbType.String, "");

                string shoppingcart = "";
                string delim = "";
                foreach (Product item in t.Cart)
                {
                    //***EAC Just need the PUBID, PRODUCTID and QTY
                    shoppingcart += delim + item.PubId.ToString() + ",'" + item.ProductId + "'," + item.NumQtyOrdered.ToString();
                    delim = "|";
                }
                db.AddInParameter(cw, "cart", DbType.String, shoppingcart);

                db.AddInParameter(cw, "ipaddr", DbType.String, ipaddr);
                db.AddInParameter(cw, "ordersource", DbType.String, "IEOT");                //always IEOT for PLEX
                db.AddInParameter(cw, "bopolicy", DbType.String, "N");                      //always 'O' for NCIPL               

                //db.AddInParameter(cw, "orderdate", DbType.String, orderdate);             //obsolete - so what if its null?
                //db.AddInParameter(cw, "ordernotes", DbType.String, OrderNotes);           //obsolete - CDSH stuff
                //db.AddInParameter(cw, "uploadseq", DbType.String, UploadSeq);             //obsolete - so what if ts blank?
                //db.AddInParameter(cw, "ponum", DbType.String, PoNum);                     //obsolete - we use orderseqnum nowadays
                //db.AddInParameter(cw, "shippingmethod", DbType.String, ShippingMethod);   //obsolete - we use field SHIPMETHOD nowadays
                //db.AddInParameter(cw, "termcode", DbType.String, TermCode);               //obsolete - G,A,M,V
                //db.AddInParameter(cw, "shiptocusttype", DbType.String, ShipToCustType);   //obsolete - NA, P1
                //db.AddInParameter(cw, "paymentcode", DbType.String, PaymentCode);         //obsolete - always seems to 'G' anyway

                db.AddInParameter(cw, "num", DbType.Int32, t.Cart.Count);                   //IMPT - line items
                db.AddInParameter(cw, "numunitsordered", DbType.Int32, t.Cart.TotalQty);    //IMPT - total copies

                //db.AddInParameter(cw, "shipvendor", DbType.String, t.Cart.ShipVendor);
                db.AddInParameter(cw, "shipmethod", DbType.String, "");                     //always blank for PLEX
                db.AddInParameter(cw, "shipacctnum", DbType.String, "");                    //always blank for PLEX
                db.AddInParameter(cw, "shipcost", DbType.Double, 0);                        //always 0.0

                db.AddInParameter(cw, "ordercreator", DbType.String, "");                   //always blank for PLEX
                db.AddInParameter(cw, "ordermedia", DbType.String, "");                     //always blank for PLEX

                db.AddInParameter(cw, "customertypeid", DbType.Int32, null);

                //if (ordercomment.Length > 0)
                db.AddInParameter(cw, "ordercomment", DbType.String, "");

                //if (orderrntnumber.Length > 0)
                db.AddInParameter(cw, "rntnumber", DbType.String, "");

                db.AddInParameter(cw, "ordercreatorrole", DbType.String, HttpContext.Current.Session["NCIPL_role"]);
                db.AddInParameter(cw, "confid", DbType.Int32, confid);
                //db.AddOutParameter(cw, "ordernumber", DbType.Int32, 0);


                db.AddOutParameter(cw, "ordernumber", DbType.Int32, 4);

               

                db.ExecuteNonQuery(cw);
                retordernum = (int)db.GetParameterValue(cw, "@ordernumber"); //Get Output variable value from SQL Server

            }
            catch (Exception Ex)
            {
                retordernum = -1; //default value
                //Write to log
                LogEntry logEnt = new LogEntry();
                logEnt.Message = "\r\n" + "Save Order Error." + "\r\n" + "Source: " + Ex.Source + "\r\n" + "Description: " + Ex.Message + "\r\n" + "Stack Trace: " + Ex.StackTrace;
                Logger.Write(logEnt, "Logs");
                throw Ex; //yma add this

            }
            return (true);
        }
    }
}
