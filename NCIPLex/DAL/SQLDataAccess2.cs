using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//Manually added
using System.Data;
using System.Data.Common;
using NCIPLex.BLL;
using System.Collections;
//EntLib References
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace NCIPLex.DAL2
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
            DbCommand cw = db.GetStoredProcCommand("sp_NCIPLex_SaveEmail");
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
            DbCommand cw = db.GetStoredProcCommand("sp_NCIPLex_SaveSearch");
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
    }
}
