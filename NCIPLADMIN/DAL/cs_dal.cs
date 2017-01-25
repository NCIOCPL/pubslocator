using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using PubEntAdmin.BLL;
//EntLib References
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Data;
using System.Data.Common;
/// <summary>
/// Summary description for CS_DAL
/// </summary>
namespace PubEntAdmin.DAL
{
    public class cs_dal
    {
        public cs_dal()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        //Get Key, Value, Selected information for the lists
        public static KVPairCollection GetKVPair(string procname, int mode, int cannid)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand(procname);
            db.AddInParameter(cw, "mode", DbType.Int32, mode);
            db.AddInParameter(cw, "cannid", DbType.Int32, cannid);
            using (System.Data.IDataReader dr = db.ExecuteReader(cw))
            {
                KVPairCollection coll = new KVPairCollection();
                while (dr.Read())
                {
                    KVPair k = new KVPair(
                                            dr.GetInt32(0).ToString(),
                                            (dr[1] == DBNull.Value) ? "" : dr[1].ToString(),
                                            dr.GetInt32(2).ToString()
                                         );
                    coll.Add(k);
                }
                return (coll);
            }
        }

        //Save a record
        public static void Save(
                string headertext,
                string cancertype,
                string subject,
                string prodformat,
                string race,
                string audience,
                string language,
                string collection,
                int cannid,
                int updateflag,
                int active,
                ref string recordidout
            )
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand("sp_CANNED_SaveRecord");
            
            db.AddInParameter(cw, "headertext", DbType.String, headertext);
            db.AddInParameter(cw, "cancertype", DbType.String, cancertype);
            db.AddInParameter(cw, "subject", DbType.String, subject);
            db.AddInParameter(cw, "prodformat", DbType.String, prodformat);
            db.AddInParameter(cw, "race", DbType.String, race);
            db.AddInParameter(cw, "audience", DbType.String, audience);
            db.AddInParameter(cw, "language", DbType.String, language);
            db.AddInParameter(cw, "collection", DbType.String, collection);
            db.AddInParameter(cw, "cannid", DbType.Int32, cannid);
            db.AddInParameter(cw, "updateflag", DbType.Int32, updateflag);
            db.AddInParameter(cw, "active", DbType.Int32, active);
            db.AddOutParameter(cw, "recordidout", DbType.String, 6);

            db.ExecuteNonQuery(cw);
            //int retvalue = (int)db.GetParameterValue(cmd, "returnvalue");
            recordidout = (string)db.GetParameterValue(cw, "recordidout");
            return;
        }

        //Get Records
        public static cs_recordcollection GetRecords()
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand("sp_CANNED_getRecords");

            using (System.Data.IDataReader dr = db.ExecuteReader(cw))
            {
                cs_recordcollection coll = new cs_recordcollection();
                while (dr.Read())
                {
                    cs_record k = new cs_record(
                                            dr.GetInt32(dr.GetOrdinal("cannid")),
                                            dr["recordid"].ToString(),
                                            dr["headertext"].ToString(),
                                            (dr["audience"] == DBNull.Value) ? "" : dr["audience"].ToString(),
                                            (dr["cancertype"] == DBNull.Value) ? "" : dr["cancertype"].ToString(),
                                            (dr["language"] == DBNull.Value) ? "" : dr["language"].ToString(),
                                            (dr["prodformat"] == DBNull.Value) ? "" : dr["prodformat"].ToString(),
                                            (dr["race"] == DBNull.Value) ? "" : dr["race"].ToString(),
                                            (dr["series"] == DBNull.Value) ? "" : dr["series"].ToString(),
                                            (dr["subject"] == DBNull.Value) ? "" : dr["subject"].ToString(),
                                            dr.GetInt32(dr.GetOrdinal("active"))
                                            );
                    coll.Add(k);
                }
                return (coll);
            }
        }

        //Get Records By CSV
        public static cs_recordcollection GetRecordsByCSV(string csv)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand("sp_CANNED_getRecordsByCSV");
            db.AddInParameter(cw, "csv", DbType.String, csv);

            using (System.Data.IDataReader dr = db.ExecuteReader(cw))
            {
                cs_recordcollection coll = new cs_recordcollection();
                while (dr.Read())
                {
                    cs_record k = new cs_record(
                                            dr.GetInt32(dr.GetOrdinal("cannid")),
                                            dr["recordid"].ToString(),
                                            dr["headertext"].ToString(),
                                            (dr["audience"] == DBNull.Value) ? "" : dr["audience"].ToString(),
                                            (dr["cancertype"] == DBNull.Value) ? "" : dr["cancertype"].ToString(),
                                            (dr["language"] == DBNull.Value) ? "" : dr["language"].ToString(),
                                            (dr["prodformat"] == DBNull.Value) ? "" : dr["prodformat"].ToString(),
                                            (dr["race"] == DBNull.Value) ? "" : dr["race"].ToString(),
                                            (dr["series"] == DBNull.Value) ? "" : dr["series"].ToString(),
                                            (dr["subject"] == DBNull.Value) ? "" : dr["subject"].ToString(),
                                            dr.GetInt32(dr.GetOrdinal("active"))
                                            );
                    coll.Add(k);
                }
                return (coll);
            }
        }
    }
}