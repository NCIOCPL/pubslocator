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
        
        public static int GetTimeout(int appID, int timeoutID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand("sp_KIOSK_GetTimeout");
            db.AddInParameter(cw, "appID", DbType.Int32, appID);
            db.AddInParameter(cw, "timeoutID", DbType.Int32, timeoutID);
            using (System.Data.IDataReader dr = db.ExecuteReader(cw))
            {
                if (dr.Read())
                {
                    return (dr.GetInt32(0));
                }
            }
            throw (new ArgumentOutOfRangeException("Timeout","Value is missing"));
        }

        public static string GetAttractPubs(int appID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand("sp_KIOSK_GetAttractPubs");
            db.AddInParameter(cw, "ConfID", DbType.Int32, appID);
            using (System.Data.IDataReader dr = db.ExecuteReader(cw))
            {
                if (dr.Read())
                {
                    return (dr["result"].ToString());
                }
            }
            throw (new ArgumentOutOfRangeException("Could not get attract pubs"));
        }

        public static bool SaveNewOrder(Transaction t, string ipaddr, int confid, out int returnvalue, out int retordernum)
        {
            returnvalue = -1;   //unused
            retordernum = -1;   //will return the orderseqnum
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand cw = db.GetStoredProcCommand("sp_KIOSK_SaveNewOrder");

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
                db.AddInParameter(cw, "ordersource", DbType.String, "IVPR");                //always IVPR for KIOSK
                db.AddInParameter(cw, "bopolicy", DbType.String, "N");                      //always 'N' for VPR and NCIPLEX 

                //db.AddInParameter(cw, "orderdate", DbType.String, orderdate);             //obsolete - so what if its null?
                //db.AddInParameter(cw, "ordernotes", DbType.String, OrderNotes);           //obsolete - CDSH stuff
                //db.AddInParameter(cw, "uploadseq", DbType.String, UploadSeq);             //obsolete - so what if ts blank?
                //db.AddInParameter(cw, "ponum", DbType.String, PoNum);                     //obsolete - we use orderseqnum nowadays
                //db.AddInParameter(cw, "shippingmethod", DbType.String, ShippingMethod);   //obsolete - we use field SHIPMETHOD nowadays
                //db.AddInParameter(cw, "termcode", DbType.String, TermCode);               //obsolete - G,A,M,V
                //db.AddInParameter(cw, "shiptocusttype", DbType.String, ShipToCustType);   //obsolete - NA, P1
                //db.AddInParameter(cw, "paymentcode", DbType.String, PaymentCode);         //obsolete - always seems to 'G' anyway

                db.AddInParameter(cw, "num", DbType.Int32, t.Cart.NumItemsForShipping);                   //IMPT - line items
                db.AddInParameter(cw, "numunitsordered", DbType.Int32, t.Cart.TotalQtyForShipping);    //IMPT - total copies

                //db.AddInParameter(cw, "shipvendor", DbType.String, t.Cart.ShipVendor);
                db.AddInParameter(cw, "shipmethod", DbType.String, "");                     //Always empty string for KIOSK
                db.AddInParameter(cw, "shipacctnum", DbType.String, "");                    //Always empty string for KIOSK
                db.AddInParameter(cw, "shipcost", DbType.Double, 0.0);                      //Always free (domestic or intl) for KIOSK 
                db.AddInParameter(cw, "ordercreator", DbType.String, "");                   //Always empty string for KIOSK
                db.AddInParameter(cw, "ordermedia", DbType.String, "");                     //Always empty string for KIOSK
                db.AddInParameter(cw, "customertypeid", DbType.Int32, null);                //Always null
                db.AddInParameter(cw, "ordercomment", DbType.String, "");                   //Always empty string for KIOSK
                db.AddInParameter(cw, "rntnumber", DbType.String, "");                      //Always empty string for KIOSK
                db.AddInParameter(cw, "ordercreatorrole", DbType.String, HttpContext.Current.Session["NCIPL_role"]);
                db.AddInParameter(cw, "location", DbType.String, HttpContext.Current.Session["KIOSK_ShipLocation"]);

                string shoppingcart = "";
                string emailcart = "";
                string delim = "";
                string delim2 = "";
                foreach (Product item in t.Cart)
                {
                    //***EAC Just need the PUBID, PRODUCTID and QTY of Non-ONLINE items
                    //if (item.OrderDisplayStatus == "ORDER")
                    if (item.IsPhysicalItem)    //(20130411)
                    {
                        shoppingcart += delim + item.PubId.ToString() + ",'" + item.ProductId + "'," + item.NumQtyOrdered.ToString();
                        delim = "|";
                    }
                    else //if (item.OnlineDisplayStatus == "ONLINE" ) //It has to be an email-order (20130411)
                    {
                        emailcart += delim2 + item.PubId.ToString();
                        delim2 = "|";
                    }
                }
                db.AddInParameter(cw, "cart", DbType.String, shoppingcart);
                db.AddInParameter(cw, "emailcart", DbType.String, emailcart);
                db.AddInParameter(cw, "numlinks", DbType.Int32, t.Cart.NumItemsForEmailing);
                db.AddInParameter(cw, "confid", DbType.Int32, confid); 

                db.AddOutParameter(cw, "ordernumber", DbType.Int32, 0);

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
            DbCommand cw = db.GetStoredProcCommand("sp_KIOSK_GetEbookUrl");
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
    }
}
