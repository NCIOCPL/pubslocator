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

using Exhibit.BLL;
using PubEnt.GlobalUtils;

namespace PubEnt.DAL
{

    public class DAL 
    {

        #region CONSTANTS
        //private const string SP_GET_ALL_NCIPL_RECORDS = "PRODUCTS_getAll_NCIPL_Products";
        private const string SP_GET_ALL_NCIPL_RECORDS = "PRODUCTS_getAll_NCIPL_PRODUCTS_TEMP";

        private static readonly string strSpGetAllConfName = "sp_EXHIBIT_GetAllConfName";
        private static readonly string strSpGetConfAttract = "sp_getTestRecords";

        #endregion

        #region SQL_HELPER_METHODS
        //Return a dataset
        private static DataSet ExecutedataSet(Database db, DbCommand dbCmd)
        {
            if (db == null) throw (new ArgumentNullException("db"));
            if (dbCmd == null) throw (new ArgumentNullException("dbCmd"));

            return (db.ExecuteDataSet(dbCmd));
        }
        #endregion

        //*********************************************************************
        //
        // Generate Collection  Methods
        //
        // The following methods are used to generate collections of objects
        //[]
        //*********************************************************************


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

        public static Product GetPubByProductID(string productid)
        {
            Database db = DatabaseFactory.CreateDatabase();
            //DbCommand cw = db.GetStoredProcCommand("sp_ncipl_getPubbyProductID");
            DbCommand cw = db.GetStoredProcCommand("sp_KIOSK_getPubbyProductID");
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
                                            dr.GetInt32(dr.GetOrdinal("maxqty_exhibit")),

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
                                            dr["PUB_STATUS"].ToString()
                                            );
                    return (k);
                }
                else throw new ArgumentException("Publication ID not found", "value");
                
            }
        }
        public static Product GetPubByPubID(int pubid)
        {
            Database db = DatabaseFactory.CreateDatabase();
            //DbCommand cw = db.GetStoredProcCommand("sp_ncipl_getPubbyPubID");
            DbCommand cw = db.GetStoredProcCommand("sp_KIOSK_getPubByPubID");
            db.AddInParameter(cw, "pubid", DbType.Int32, pubid);
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
                                            dr.GetInt32(dr.GetOrdinal("maxqty_exhibit")),

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
                                            dr["PUB_STATUS"].ToString()
                                            );
                    return (k);
                }
                else throw new ArgumentException("Publication ID not found", "value");

            }

        }

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
        
        public static IDataReader GetPubPhysicalDesc(int pubid)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("sp_NCIPL_getPubPhysicalDescription");

            db.AddInParameter(dbCommand, "pubid", DbType.Int32, pubid);
            return db.ExecuteReader(dbCommand);
        }

        //Return collections that a pub belongs to
        public static IDataReader GetPubCollections(int pubid)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("sp_NCIPL_getPubCollections");

            db.AddInParameter(dbCommand, "pubid", DbType.Int32, pubid);
            return db.ExecuteReader(dbCommand);
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


        //END CR - 23

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

        public static Confs GetSearchCategory(int ConfID, String CategoryType)
        {
            String procedureName = "";

            if (CategoryType == "CANCERTYPE")
            {
                procedureName = "sp_KIOSK_getCancerTypesByConfID";
            }
            else if (CategoryType == "SUBJECT")
            {
                procedureName = "sp_KIOSK_getSubjectByConfID";
            }
            else if (CategoryType == "AUDIENCE")
            {
                procedureName = "sp_KIOSK_getAudienceByConfID";
            }
            else if (CategoryType == "PUBLICATIONFORMAT")
            {
                procedureName = "sp_KIOSK_getFormatByConfID";
            }
            else if (CategoryType == "SERIES") //Collections
            {
                procedureName = "sp_KIOSK_getSeriesByConfID";
            }
            else if (CategoryType == "LANGUAGES")
            {
                procedureName = "sp_KIOSK_getLanguageByConfID";
            }

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(procedureName);
            db.AddInParameter(dbCommand, "ConfID", DbType.Int32, ConfID);
            using (System.Data.IDataReader dr = ExecuteReader(db, dbCommand))
            {
                Confs coll = new Confs();
                Conf newConf;

                while (dr.Read())
                {
                    newConf = new Conf(Convert.ToInt32(dr["OPT_ID"]),
                        (string)dr["OPT_DESC"]);
                    coll.Add(newConf);
                }
                return (coll);
            }
        }

        public static Confs GetSearchPubs(int ConfID, string cancertype, string subject, string audience, string publicationformat, string series, string languages)
        {
            String procedureName = "sp_KIOSK_SearchPubs";

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(procedureName);
            db.AddInParameter(dbCommand, "ConfID", DbType.Int32, ConfID);
            db.AddInParameter(dbCommand, "cancertype", DbType.String, cancertype);
            db.AddInParameter(dbCommand, "subject", DbType.String, subject);
            db.AddInParameter(dbCommand, "audience", DbType.String, audience);
            db.AddInParameter(dbCommand, "publicationformat", DbType.String, publicationformat);
            db.AddInParameter(dbCommand, "series", DbType.String, series);
            db.AddInParameter(dbCommand, "languages", DbType.String, languages);

            using (System.Data.IDataReader dr = ExecuteReader(db, dbCommand))
            {
                Confs coll = new Confs();
                Conf newConf;

                while (dr.Read())
                {
                    newConf = new Conf(Convert.ToInt32(dr["pubid"]),
                        (string)dr["productid"]);
                    coll.Add(newConf);
                }
                return (coll);
            }
        }

        public static DataSet GetConfAttract()
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSpGetConfAttract);
            DataSet productDataSet = null;
            productDataSet = ExecutedataSet(db, dbCommand);
            return productDataSet;
        }

        public static string GetConfName(int ConfId)
        {
            string returnvalue = "";
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cmd = db.GetStoredProcCommand("sp_KIOSK_GetConfName");
            db.AddInParameter(cmd, "confid", DbType.Int32, ConfId);
            db.AddOutParameter(cmd, "returnvalue", DbType.String, 100);
            db.ExecuteNonQuery(cmd);
            returnvalue = db.GetParameterValue(cmd, "returnvalue").ToString();
            return returnvalue;
        }

        public static int GetConferenceSeq(string confName)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("sp_KIOSK_GetConferenceSeq");

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

    }
}
