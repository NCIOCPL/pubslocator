using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//Manually added
using System.Data;
using System.Data.Common;
using PubEnt.BLL;
using System.Collections;
using System.Configuration;

//EntLib References
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace PubEnt.DAL2
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
        #endregion


        public static string GetNerdoURLByChild(string productid)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand("sp_NCIPL_GetNerdoURLByChild");
            db.AddInParameter(cw, "productid", DbType.String, productid);
            using (System.Data.IDataReader dr = db.ExecuteReader(cw))
            {

                if (dr.Read())
                {

                    return (dr["URL2"].ToString());

                }
                
            }
            return ("");
        }


        public static Zipcode GetCSZ(int z)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand("sp_NCIPL_GetCSZ");
            db.AddInParameter(cw, "zip", DbType.String, z.ToString());
            using (System.Data.IDataReader dr = db.ExecuteReader(cw))
            {
                Zipcode  k = new Zipcode();
                if (dr.Read())
                {
                    //k.zip5=z;
                    k.City = dr["City"].ToString();
                    k.State  = dr["State"].ToString();
                    k.Zip4 = dr["zip4"].ToString();
                }
                return (k); //May or may not have real values at this point...thats ok.                
            }
        }
        public static KVPairCollection GetCities(int z)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("sp_NCIPL_GetCities");

            db.AddInParameter(dbCommand, "zip", DbType.String,  z.ToString().PadLeft(5,'0'));//***EAC HOTFIX for hitt8245...better to change here than in the calling procedures.


            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                // Processing code
                KVPairCollection c = new KVPairCollection();
                while (dr.Read())
                {

                    KVPair k = new KVPair(
                                            dr["cityname"].ToString(),
                                            dr["state"].ToString()                                            
                                            );
                    c.Add(k);
                }
                return c;
            }
        }
        public static ProductCollection GetVirtualKits(int pubid)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("sp_NCIPL_GetVirtualKits");
            
            db.AddInParameter(dbCommand, "pubid", DbType.Int32, pubid);


            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                // Processing code
                ProductCollection collProducts = new ProductCollection();
                while (dr.Read())
                {

                    Product k = new Product(
                                            dr.GetInt32(dr.GetOrdinal("pubid")),
                                            dr["productid"].ToString(),
                                            dr["longtitle"].ToString(),
                                            dr.GetInt32(dr.GetOrdinal("ncipl_qty"))
                                            );
                    collProducts.Add(k);
                }
                return collProducts;
            }
        }
        public static bool SaveDemographics(string contacted, string howfind, string howfindother, int age, string sex,
            string ethnicity, string racial, string education, string zip5, string zip4, string bestdesc, string place,
            string healthcare, string income,string place2, string mons12, string whichhealth, int howincome)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand("sp_NCIPL_SaveDemographics");
            db.AddInParameter(cw, "contacted", DbType.String, contacted);
            db.AddInParameter(cw, "howfind", DbType.String, howfind);
            db.AddInParameter(cw, "howfindother", DbType.String, howfindother);
            db.AddInParameter(cw, "age", DbType.Int32, age);
            db.AddInParameter(cw, "sex", DbType.String, sex);

            db.AddInParameter(cw, "ethnicity", DbType.String, ethnicity);
            db.AddInParameter(cw, "racial", DbType.String, racial);
            db.AddInParameter(cw, "education", DbType.String, education);
            db.AddInParameter(cw, "zip5", DbType.String, zip5);
            db.AddInParameter(cw, "zip4", DbType.String, zip4);

            db.AddInParameter(cw, "bestdesc", DbType.String, bestdesc);
            db.AddInParameter(cw, "place", DbType.String, place);
            db.AddInParameter(cw, "healthcare", DbType.String, healthcare);
            db.AddInParameter(cw, "income", DbType.String, income);

            db.AddInParameter(cw, "place2", DbType.String, place2);
            db.AddInParameter(cw, "mons12", DbType.String, mons12);
            db.AddInParameter(cw, "whichhealth", DbType.String, whichhealth);
            db.AddInParameter(cw, "howincome", DbType.Int32, howincome);

            db.ExecuteNonQuery(cw);
            return (true);
        }
        public static bool SaveEmail(string emailto, string emailbody)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand("sp_NCIPL_SaveEmail");
            db.AddInParameter(cw, "emailto", DbType.String, emailto);
            db.AddInParameter(cw, "emailbody", DbType.String, emailbody);
            db.ExecuteNonQuery(cw);
            return (true);
        }
        //public static bool SaveSearch(string terms, int hits) //Not used anymore due to CR-31 HITT 7074
        //{
        //    Database db = DatabaseFactory.CreateDatabase();
        //    DbCommand cw = db.GetStoredProcCommand("sp_NCIPL_SaveSearch");
        //    db.AddInParameter(cw, "terms", DbType.String, terms);
        //    db.AddInParameter(cw, "hits", DbType.Int32, hits);
        //    db.ExecuteNonQuery(cw);
        //    return (true);
        //}
        //Saves all search criteria to TBL_SEARCHKEYWORDS table - CR-31 HITT 7074
        public static bool SaveSearchCriteriaText(  string terms, int hits,
                                        string cancertypecriteria,
                                        string subjectcriteria,
                                        string audiencecriteria,
                                        string languagecriteria,
                                        string productformatcriteria,
                                        string startswithcriteria,
                                        string seriescriteria,
                                        string neworupdatedcriteria,
                                        string racecriteria,
                                        string interfacename  )
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand("sp_NCIPL_SaveSearch");
            db.AddInParameter(cw, "terms", DbType.String, terms);
            db.AddInParameter(cw, "hits", DbType.Int32, hits);
            db.AddInParameter(cw, "cancertype", DbType.String, cancertypecriteria);
            db.AddInParameter(cw, "subject", DbType.String, subjectcriteria);
            db.AddInParameter(cw, "audience", DbType.String, audiencecriteria);
            db.AddInParameter(cw, "race", DbType.String, racecriteria);
            db.AddInParameter(cw, "productformat", DbType.String, productformatcriteria);
            db.AddInParameter(cw, "language", DbType.String, languagecriteria);
            db.AddInParameter(cw, "series", DbType.String, seriescriteria);
            db.AddInParameter(cw, "titleindex", DbType.String, startswithcriteria);
            db.AddInParameter(cw, "neworupdated", DbType.String, neworupdatedcriteria);
            db.AddInParameter(cw, "interface", DbType.String, interfacename);
            db.ExecuteNonQuery(cw);
            return (true);
        }
        public static Person GetShippingInfo(string username)
        {
            //***EAC Return shipping information from TBL_REGISTRATION
            //***EAC Otherwise returns a blank Person object if username is not found
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("sp_NCIPL_getShippingInfoByUsername");
            db.AddInParameter(dbCommand, "username", DbType.String, username);
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                Person p = new Person();
                if (dr.Read())
                {
                    p = new Person(dr.GetInt32(dr.GetOrdinal("custid")),
                                  dr["shiptoName"].ToString(),
                                  dr["shiptoOrgname"].ToString(),
                                  dr["shiptoEmail"].ToString(),
                                  dr["shiptoaddr1"].ToString(),
                                  dr["shiptoaddr2"].ToString(),
                                  dr["shiptocity"].ToString(),
                                  dr["shiptostate"].ToString(),
                                  dr["shiptozip"].ToString(),
                                  dr["shiptozip4"].ToString(),
                                  dr["shiptophone"].ToString()
                                  );
                }
                return p;
            }
        }
        public static KVPairCollection GetAllShippingMethods()
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand("sp_NCIPL_GetAllShippingMethods");
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
        public static string GetShippingMethodValuebyID(string methodid)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand("sp_NCIPL_GetShippingMethodValuebyID");
            db.AddInParameter(cw, "methodid", DbType.String, methodid);
            using (System.Data.IDataReader dr = db.ExecuteReader(cw))
            {

                if (dr.Read())
                {

                    return (dr["METHODVALUE"].ToString());

                }

            }
            return ("");
        }

        //public static bool SaveOrder(Transaction t, string s, string pubids, string pubqtys, out int returnvalue, out int returnedordernum)
        //{
        //    string OrderUploadPath = ConfigurationSettings.AppSettings["OrderUploadPath"];
        //    string EnvVariable = ConfigurationSettings.AppSettings["OrderUploadEnvironment"];
        //    string ccCostRecovery = "";
        //    if (t.Cart.ShipCost > 0)
        //        ccCostRecovery = "1";
        //    else
        //        t.CC.CCType = "";

        //    #region Log the Order Info just before calling order upload
        //    //Log the Order Info just before calling order upload
        //    try
        //    {
        //        //Write to log
        //        LogEntry logEnt = new LogEntry();
        //        string logmessage = "\r\n";
        //        logmessage += "Org Name                 :" + "############" + "\r\n";
        //        logmessage += "Cont Name                :" + "############" + "\r\n";
        //        logmessage += "Addr1                    :" + "############" + "\r\n";
        //        logmessage += "Addr2                    :" + "############" + "\r\n";
        //        logmessage += "City                     :" + "############" + "\r\n";
        //        logmessage += "State                    :" + "############" + "\r\n";
        //        logmessage += "Zip                      :" + t.ShipTo.Zip5 + "-" + t.BillTo.Zip4 + "\r\n";
        //        logmessage += "Pubs                     :" + pubids + "\r\n";
        //        logmessage += "Qtys                     :" + pubqtys + "\r\n";
        //        logmessage += "CostRecovery             :" + ccCostRecovery + "\r\n";
        //        logmessage += "TransId                  :" + t.CC.TransID + "\r\n";
        //        logEnt.Message = logmessage;
        //        Logger.Write(logEnt, "Logs");
        //    }
        //    catch (Exception Ex)
        //    {
        //        //Do nothing for now
        //    }
        //    //End of Logging Code
        //    #endregion

        //    //isabella
        //    OrderUpload ordUpload = new OrderUpload(
        //        EnvVariable,
        //        "ROO", //"NCIPL",
        //        "", //TO DO: later - probably from demographics
        //        t.BillTo.Organization,
        //        t.BillTo.Fullname,
        //        t.BillTo.Addr1,
        //        t.BillTo.Addr2,
        //        t.BillTo.City,
        //        t.BillTo.State,
        //        t.BillTo.Zip5,
        //        t.BillTo.Zip4,
        //        t.BillTo.Phone,
        //        "",
        //        t.BillTo.Email,
        //        t.ShipTo.Organization,
        //        t.ShipTo.Fullname,
        //        t.ShipTo.Addr1,
        //        t.ShipTo.Addr2,
        //        t.ShipTo.City,
        //        t.ShipTo.State,
        //        t.ShipTo.Zip5,
        //        t.ShipTo.Zip4,
        //        t.ShipTo.Phone,
        //        "",
        //        t.ShipTo.Email,
        //        pubids,
        //        pubqtys,
        //        ccCostRecovery,
        //        t.CC.ApprovalCode,
        //        t.CC.Cost.ToString(),
        //        t.CC.CCType,
        //        t.CC.CCnum,
        //        t.CC.ExpYr,
        //        t.CC.ExpMon,
        //        t.CC.TransID,
        //        t.CC.ApprovalCode,
        //        t.CC.CVV2,
        //        t.Cart.SplitOrder, //"", //(t.ShipTo.NeedCover == false) ? "" : "1",
        //        t.Cart.CustomerTypeId, //NCIPL_CC newly added Feb 20, 12
        //        OrderUploadPath,
        //        "",
        //        "",
        //        "",
        //        "",
        //        "",
        //        s,
        //        t.Cart.ShipAcctNum,
        //        t.Cart.ShipCost,
        //        t.Cart.ShipMethod,
        //        t.Cart.ShipVendor,
        //        t.Cart.OrderCreator,
        //        //JPJ 03-26-2012 t.Cart.OrderMedia,
        //        t.Cart.OrderMedia,
        //        t.Cart.OrderComment,
        //        t.Cart.RNTNumber,
        //        out returnvalue,
        //        out returnedordernum);

        //    #region Some Error happened, log the shipping org details and return code
        //    if (returnvalue != 1) //Some Error happened, log the shipping org details and return code
        //    {
        //        try
        //        {
        //            //Write to log
        //            LogEntry logEnt = new LogEntry();
        //            string logmessage = "\r\n";
        //            logmessage += "WARNING: Order upload component prematurely exited." + "\r\n";
        //            logmessage += "Return Value: " + returnvalue.ToString() + "\r\n";
        //            logmessage += "Org Name                 :" + "############" + "\r\n";
        //            logmessage += "Cont Name                :" + "############" + "\r\n";
        //            logmessage += "Zip                      :" + t.ShipTo.Zip5 + "-" + t.BillTo.Zip4 + "\r\n";
        //            logmessage += "CostRecovery             :" + ccCostRecovery + "\r\n";
        //            logmessage += "TransId                  :" + t.CC.TransID + "\r\n";
        //            logEnt.Message = logmessage;
        //            Logger.Write(logEnt, "Logs");
        //        }
        //        catch (Exception Ex)
        //        {
        //            //Do not do anything
        //        }
        //    }
        //    //End - Log the output value
        //    #endregion

        //    return (true);
        //}

        public static DateTime MostRecentOrder(string addr1, string zip5)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand("sp_NCIPL_GetMostRecentOrder");
            db.AddInParameter(cw, "addr1", DbType.String, addr1);
            db.AddInParameter(cw, "zip5", DbType.String, zip5);
            using (IDataReader dr = db.ExecuteReader(cw))
            {
                DateTime t = DateTime.Now;
                if (dr.Read())
                {
                    string temp = dr["orderdate"].ToString();
                    t = DateTime.Parse(dr["orderdate"].ToString());
                }
                return t;
            }
        }

        public static int getNoShipFlag(string addr1, string zip5)
        {
            int NoShip = 0;
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand("sp_NCIPLCC_getNoShipFlag");
            db.AddInParameter(cw, "addr1", DbType.String, addr1);
            db.AddInParameter(cw, "zip5", DbType.String, zip5);

            object result = db.ExecuteScalar(cw);

            ////(dr["s_date"] == DBNull.Value) ? DateTime.MinValue : (DateTime)dr["s_date"],
            //NoShip = (db.ExecuteScalar(cw) == DBNull.Value) ? 0 : 1;

            ////cw.Dispose();

            return (result == null) ? 0 : (int)result;
            
        }

        public static Person GetBillingInfo(string username)
        {
            //***EAC Return shipping information from TBL_REGISTRATION
            //***EAC Otherwise returns a blank Person object if username is not found
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("sp_NCIPL_getBillingInfoByUsername");
            db.AddInParameter(dbCommand, "username", DbType.String, username);
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                Person p = new Person();
                if (dr.Read())
                {
                    p = new Person(dr.GetInt32(dr.GetOrdinal("custid")),
                                  dr["billtoName"].ToString(),
                                  dr["billtoOrgname"].ToString(),
                                  dr["billtoEmail"].ToString(),
                                  dr["billtoaddr1"].ToString(),
                                  dr["billtoaddr2"].ToString(),
                                  dr["billtocity"].ToString(),
                                  dr["billtostate"].ToString(),
                                  dr["billtozip"].ToString(),
                                  dr["billtozip4"].ToString(),
                                  dr["billtophone"].ToString()
                                  );
                }
                return p;
            }
        }
        public static KVPairCollection GetCountries()
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand("SP_NCIPLLM_GetCountry");
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

        public static bool SaveNewOrder(Transaction t, string ipaddr, string pubids, string pubqtys, out int returnvalue, out int retordernum)
        {
            returnvalue = -1;   //unused
            retordernum = -1;   //will return the orderseqnum
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand cw = db.GetStoredProcCommand("sp_NCIPLCC_SaveNewOrder");

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

                db.AddInParameter(cw, "ipaddr", DbType.String, ipaddr);
                db.AddInParameter(cw, "ordersource", DbType.String, "ICIS");                //always 'ICIS' for NCIPLCC/NCIPLLM
                db.AddInParameter(cw, "bopolicy", DbType.String, "O");                      //always 'O' for NCIPLCC/NCIPLLM                

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
                db.AddInParameter(cw, "shipmethod", DbType.String, t.Cart.ShipMethod);
                db.AddInParameter(cw, "shipacctnum", DbType.String, t.Cart.ShipAcctNum);
                db.AddInParameter(cw, "shipcost", DbType.Double, t.Cart.ShipCost);

                db.AddInParameter(cw, "ordercreator", DbType.String, t.Cart.OrderCreator);
                db.AddInParameter(cw, "ordermedia", DbType.String, t.Cart.OrderMedia);

                db.AddInParameter(cw, "customertypeid", DbType.Int32, t.Cart.CustomerTypeId);

                //if (ordercomment.Length > 0)
                db.AddInParameter(cw, "ordercomment", DbType.String, t.Cart.OrderComment);

                //if (orderrntnumber.Length > 0)
                db.AddInParameter(cw, "rntnumber", DbType.String, t.Cart.RNTNumber); 

                db.AddInParameter(cw, "ordercreatorrole", DbType.String, HttpContext.Current.Session["NCIPL_role"]);

                db.AddOutParameter(cw, "ordernumber", DbType.Int32, 0);

                string shoppingcart = "";
                string delim = "";
                foreach (Product item in t.Cart)
                {
                    //***EAC Just need the PUBID, PRODUCTID and QTY
                    shoppingcart += delim + item.PubId.ToString() + ",'" + item.ProductId + "'," + item.NumQtyOrdered.ToString();
                    delim = "|";
                }
                db.AddInParameter(cw, "cart", DbType.String, shoppingcart);


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
            }
            return (true);
        }
        public static bool LMSaveNewOrder(Transaction t, string ipaddr, string pubids, string pubqtys, out int returnvalue, out int retordernum)
        {
            returnvalue = -1;   //unused
            retordernum = -1;   //will return the orderseqnum
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand cw = db.GetStoredProcCommand("sp_NCIPLLM_SaveNewOrder");

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

                db.AddInParameter(cw, "ipaddr", DbType.String, ipaddr);
                db.AddInParameter(cw, "ordersource", DbType.String, "ICIS");                //always 'ICIS' for NCIPLCC/NCIPLLM
                db.AddInParameter(cw, "bopolicy", DbType.String, "O");                      //always 'O' for NCIPLCC/NCIPLLM                

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
                db.AddInParameter(cw, "shipmethod", DbType.String, t.Cart.ShipMethod);
                db.AddInParameter(cw, "shipacctnum", DbType.String, t.Cart.ShipAcctNum);
                db.AddInParameter(cw, "shipcost", DbType.Double, t.Cart.ShipCost);

                db.AddInParameter(cw, "ordercreator", DbType.String, t.Cart.OrderCreator);
                db.AddInParameter(cw, "ordermedia", DbType.String, t.Cart.OrderMedia);

                db.AddInParameter(cw, "customertypeid", DbType.Int32, t.Cart.CustomerTypeId);

                //if (ordercomment.Length > 0)
                db.AddInParameter(cw, "ordercomment", DbType.String, t.Cart.OrderComment);

                //if (orderrntnumber.Length > 0)
                db.AddInParameter(cw, "rntnumber", DbType.String, t.Cart.RNTNumber);

                db.AddInParameter(cw, "ordercreatorrole", DbType.String, HttpContext.Current.Session["NCIPL_role"]);

                db.AddOutParameter(cw, "ordernumber", DbType.Int32, 0);

                string shoppingcart = "";
                string delim = "";
                foreach (Product item in t.Cart)
                {
                    //***EAC Just need the PUBID, PRODUCTID and QTY
                    shoppingcart += delim + item.PubId.ToString() + ",'" + item.ProductId + "'," + item.NumQtyOrdered.ToString();
                    delim = "|";
                }
                db.AddInParameter(cw, "cart", DbType.String, shoppingcart);


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
            }
            return (true);
        }
        public static bool UpdateOrder(Transaction t, string CurrCustID, string ipaddr, string pubids, string pubqtys, out int returnvalue, out int retordernum)
        {
            returnvalue = -1;   //unused
            retordernum = -1;   //will return the orderseqnum
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand cw = db.GetStoredProcCommand("[sp_NCIPLCC_SaveUpdateNewOrder]");
                db.AddInParameter(cw, "CurrCustID", DbType.Int32, Convert.ToInt32(CurrCustID));
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

                db.AddInParameter(cw, "ipaddr", DbType.String, ipaddr);
                db.AddInParameter(cw, "ordersource", DbType.String, "ICIS");                //always 'ICIS' for NCIPLCC/NCIPLLM
                db.AddInParameter(cw, "bopolicy", DbType.String, "O");                      //always 'O' for NCIPLCC/NCIPLLM                

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
                db.AddInParameter(cw, "shipmethod", DbType.String, t.Cart.ShipMethod);
                db.AddInParameter(cw, "shipacctnum", DbType.String, t.Cart.ShipAcctNum);
                db.AddInParameter(cw, "shipcost", DbType.Double, t.Cart.ShipCost);

                db.AddInParameter(cw, "ordercreator", DbType.String, t.Cart.OrderCreator);
                db.AddInParameter(cw, "ordermedia", DbType.String, t.Cart.OrderMedia);

                db.AddInParameter(cw, "customertypeid", DbType.Int32, t.Cart.CustomerTypeId);

                //if (ordercomment.Length > 0)
                db.AddInParameter(cw, "ordercomment", DbType.String, t.Cart.OrderComment);

                //if (orderrntnumber.Length > 0)
                db.AddInParameter(cw, "rntnumber", DbType.String, t.Cart.RNTNumber);

                db.AddInParameter(cw, "ordercreatorrole", DbType.String, HttpContext.Current.Session["NCIPL_role"]);

                db.AddOutParameter(cw, "ordernumber", DbType.Int32, 0);

                string shoppingcart = "";
                string delim = "";
                foreach (Product item in t.Cart)
                {
                    //***EAC Just need the PUBID, PRODUCTID and QTY
                    shoppingcart += delim + item.PubId.ToString() + ",'" + item.ProductId + "'," + item.NumQtyOrdered.ToString();
                    delim = "|";
                }
                db.AddInParameter(cw, "cart", DbType.String, shoppingcart);


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
            }
            return (true);
        }
        public static string EbookUrl(int pubid, string form)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand("sp_NCIPL_GetEbookUrl");
            db.AddInParameter(cw, "pubid", DbType.Int32, pubid);
            db.AddInParameter(cw, "form", DbType.String, form);
            using (IDataReader dr = db.ExecuteReader(cw))
            {
                if (dr.Read())
                    return dr["ebookurl"].ToString();
                else
                    return "";
            }
        }
        public static KVPairCollection GetTypeOfCustomer(string role)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand("sp_NCIPLCC_getTypeOfCustomer");
            db.AddInParameter(cw, "role", DbType.String, role);
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
        public static KVPairCollection GetOrderMedia(string role)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand("sp_NCIPLCC_getOrderMedia");
            db.AddInParameter(cw, "role", DbType.String, role);
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
    }
}
