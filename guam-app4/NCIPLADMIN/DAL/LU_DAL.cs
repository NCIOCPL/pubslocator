using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections;
using System.Data.SqlClient;

using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.SqlCe;
using PubEntAdmin.BLL;

namespace PubEntAdmin.DAL
{
    public class LU_DAL
    {
        #region Static variable
        private static readonly string SP_ADMIN_GetAllConference = "SP_ADMIN_GetAllConference";
        private static readonly string SP_ADMIN_GetMAXConferenceID = "SP_ADMIN_GetMAXConferenceID";
        private static readonly string SP_ADMIN_SetConferenceByConfid = "SP_ADMIN_SetConferenceByConfid";
        private static readonly string SP_ADMIN_SetUpdateConferenceByConfID = "SP_ADMIN_SetUpdateConferenceByConfID";
        private static readonly string SP_ADMIN_DeleteConferenceByCnfid ="SP_ADMIN_DeleteConferenceByCnfid";

        private static readonly string SP_ADMIN_GetAllLUAudience = "SP_ADMIN_GetAllLUAudience";
        private static readonly string SP_ADMIN_SetAudienceByAudid = "SP_ADMIN_SetAudienceByAudid";
        private static readonly string SP_ADMIN_DeleteAudienceByAudid = "SP_ADMIN_DeleteAudienceByAudid";
        private static readonly string SP_ADMIN_UpdateAudienceByAudid = "SP_ADMIN_UpdateAudienceByAudid";
        private static readonly string SP_ADMIN_CheckExistAudienceByAudid = "SP_ADMIN_CheckExistAudienceByAudid";

        private static readonly string SP_ADMIN_GetAllLUAwards = "SP_ADMIN_GetAllLUAwards";
        private static readonly string SP_ADMIN_SetAwardByAwdid = "SP_ADMIN_SetAwardByAwdid";
        private static readonly string SP_ADMIN_DeleteAwardByAwdid = "SP_ADMIN_DeleteAwardByAwdid";
        private static readonly string SP_ADMIN_UpdateAwardByAwdid ="SP_ADMIN_UpdateAwardByAwdid";
        private static readonly string SP_ADMIN_CheckExistAwardByAwdid = "SP_ADMIN_CheckExistAwardByAwdid";

        private static readonly string SP_ADMIN_GetAllLUCancerType = "SP_ADMIN_GetAllLUCancerType";
        private static readonly string SP_ADMIN_SetCancerTypeByCtpyid = "SP_ADMIN_SetCancerTypeByCtpyid";
        private static readonly string SP_ADMIN_DeleteCancerTypeByCtpyid = "SP_ADMIN_DeleteCancerTypeByCtpyid";
        private static readonly string SP_ADMIN_UpdateCancerTypeByCtypid = "SP_ADMIN_UpdateCancerTypeByCtypid";
        private static readonly string SP_ADMIN_CheckExistCancerTypeByCtpyid = "SP_ADMIN_CheckExistCancerTypeByCtpyid";

        private static readonly string SP_ADMIN_GetAllLULang = "SP_ADMIN_GetAllLULang";
        private static readonly string SP_ADMIN_SetLangByLngid = "SP_ADMIN_SetLangByLngid";
        private static readonly string SP_ADMIN_DeleteLanguageByLngid = "SP_ADMIN_DeleteLanguageByLngid";
        private static readonly string SP_ADMIN_UpdateLanguageByLngid = "SP_ADMIN_UpdateLanguageByLngid";
        private static readonly string SP_ADMIN_CheckExistLanguageByCtpyid = "SP_ADMIN_CheckExistLanguageByCtpyid";

        private static readonly string SP_ADMIN_GetAllLUProdFormat = "SP_ADMIN_GetAllLUProdFormat";
        private static readonly string SP_ADMIN_SetProductFormatByPftid = "SP_ADMIN_SetProductFormatByPftid";
        private static readonly string SP_ADMIN_DeleteProductFormatByPftid = "SP_ADMIN_DeleteProductFormatByPftid";
        private static readonly string SP_ADMIN_UpdateProductFormatByPftid = "SP_ADMIN_UpdateProductFormatByPftid";
        private static readonly string SP_ADMIN_CheckExistProdFormatByPftid = "SP_ADMIN_CheckExistProdFormatByPftid";

        private static readonly string SP_ADMIN_GetAllLUReadinglevel = "SP_ADMIN_GetAllLUReadinglevel";
        private static readonly string SP_ADMIN_SetReadingLevelByRdlid = "SP_ADMIN_SetReadingLevelByRdlid";
        private static readonly string SP_ADMIN_DeleteReadinglevelByRdlid = "SP_ADMIN_DeleteReadinglevelByRdlid";
        private static readonly string SP_ADMIN_UpdateReadinglevelByRdlid = "SP_ADMIN_UpdateReadinglevelByRdlid";
        private static readonly string SP_ADMIN_CheckExistReadinglevelByRdlid = "SP_ADMIN_CheckExistReadinglevelByRdlid";

        private static readonly string SP_ADMIN_GetAllLURace = "SP_ADMIN_GetAllLURace";
        private static readonly string SP_ADMIN_SetRaceByRceid = "SP_ADMIN_SetRaceByRceid";
        private static readonly string SP_ADMIN_DeleteRaceByRceid = "SP_ADMIN_DeleteRaceByRceid";
        private static readonly string SP_ADMIN_UpdateRaceByRceid = "SP_ADMIN_UpdateRaceByRceid";
        private static readonly string SP_ADMIN_CheckExistRaceByRceid = "SP_ADMIN_CheckExistRaceByRceid";

        //NCIPL_CC private static readonly string SP_ADMIN_GetAllLUSeries = "SP_ADMIN_GetAllLUSeries";
        private static readonly string SP_ADMIN_GetAllLUSeries = "SP_ADMIN_GetAllLUSeries_2";
        //NCIPL_CC private static readonly string SP_ADMIN_SetSeriesBySreid = "SP_ADMIN_SetSeriesBySreid";
        private static readonly string SP_ADMIN_SetSeriesBySreid = "SP_ADMIN_SetSeriesBySreid_2";
        private static readonly string SP_ADMIN_DeleteSeriesBySreid = "SP_ADMIN_DeleteSeriesBySreid";
        private static readonly string SP_ADMIN_UpdateSeriesBySreid = "SP_ADMIN_UpdateSeriesBySreid";
        private static readonly string SP_ADMIN_UpdateSeriesBySreid_2 = "SP_ADMIN_UpdateSeriesBySreid_2"; //Added for NCIPL_CC
        private static readonly string SP_ADMIN_CheckSeriesByInterfaceBySeriesId = "SP_ADMIN_CheckSeries_ByInterfaceBySeriesId"; //Added for NCIPL_CC
        private static readonly string SP_ADMIN_CheckExistSeriesBySreid = "SP_ADMIN_CheckExistSeriesBySreid";

        private static readonly string SP_ADMIN_SetTimeoutByTimeid = "SP_ADMIN_SetTimeoutByTimeoutid";
        private static readonly string SP_ADMIN_GetTimeout = "SP_ADMIN_GetTimeout";
        private static readonly string SP_ADMIN_GetRotationPubsByConfid = "SP_ADMIN_GetRotationPubsByConfid";
        private static readonly string SP_ADMIN_GetKioskPub = "SP_ADMIN_GetKioskPub";

        #region sponsor
        private static readonly string strSP_ADMIN_GetAllLUSponsors = "SP_ADMIN_GetAllLUSponsors";
        private static readonly string strSP_ADMIN_GetSponsorByKeyword = "SP_ADMIN_GetSponsorsByKeyword";
        private static readonly string strSP_ADMIN_GetSponsorsByDesc = "SP_ADMIN_GetSponsorsByDesc";
        private static readonly string strSP_ADMIN_SetSponsorBySpid = "SP_ADMIN_SetSponsorBySpid";
        private static readonly string strSP_ADMIN_GetSponsorBySpid = "SP_ADMIN_GetSponsorBySpid";
        private static readonly string strSP_ADMIN_CheckExistSponsorBySpid = "SP_ADMIN_CheckExistSponsorBySpid";
        private static readonly string strSP_ADMIN_DeleteSponsorBySponid = "SP_ADMIN_DeleteSponsorBySponid";
        private static readonly string strSP_ADMIN_UpdateSponsorBySponid = "SP_ADMIN_UpdateSponsorBySponid";
        private static readonly string strSP_ADMIN_GetSponsorsBySponsoridsIdByKeyword = "SP_ADMIN_GetSponsorsBySponsoridsIdByKeyword";
        #endregion

        #region Owner
        private static readonly string strSP_ADMIN_GetAllLUOwners = "SP_ADMIN_GetAllLUOwners";
        private static readonly string strSP_ADMIN_GetOwnerByFullname = "SP_ADMIN_GetOwnerByFullname";
        private static readonly string strSP_ADMIN_SetOwnerByOwnerid = "SP_ADMIN_SetOwnerByOwnerid";
        private static readonly string strSP_ADMIN_GetOwnersByKeyword = "SP_ADMIN_GetOwnersByKeyword";
        private static readonly string strSP_ADMIN_GetOwnersByOwnerIdByKeyword = "SP_ADMIN_GetOwnersByOwnerIdByKeyword";
        private static readonly string strSP_ADMIN_UpdateOwnerByOwnerid = "SP_ADMIN_UpdateOwnerByOwnerid";
        private static readonly string strSP_ADMIN_CheckExistOwnerByOwnerid = "SP_ADMIN_CheckExistOwnerByOwnerid";
        private static readonly string strSP_ADMIN_DeleteOwnerByOwnerid = "SP_ADMIN_DeleteOwnerByOwnerid";
        #endregion

        #endregion

        #region Lookups

        //* Start Code******************************************************************************/
        //* Sapna Linus
        //******************************************************************************************/

        #region Conference
        //*DisplayConf - To Get the conference Dates to display in Grid.

        public static ConfCollection DisplayConf()
        {
            ConfCollection coll = new ConfCollection();
            DataSet ds = new DataSet();
            ds = displayConfDates();

            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Conf newMultiSelectItem = new Conf(System.Convert.ToInt32(dr.ItemArray[0]),
                        System.Convert.ToString(dr.ItemArray[1]),
                        System.Convert.ToInt32(dr.ItemArray[5]),
                        System.Convert.ToDateTime(dr.ItemArray[2]),
                        System.Convert.ToDateTime(dr.ItemArray[3])
                        );
                    coll.Add(newMultiSelectItem);
                }
            }
            return (coll);
        }

        // Query to display the Conference Dates

        public static DataSet displayConfDates()
        {
            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_GetAllConference);
            DataSet arPubid = new DataSet();

            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                DataTable schemaTable = dataReader.GetSchemaTable();
                DataTable dataTable = new DataTable();

                if (schemaTable != null)
                {
                    for (int i = 0; i < schemaTable.Rows.Count; i++)
                    {
                        DataRow dataRow = schemaTable.Rows[i];
                        string columnName = (string)dataRow["ColumnName"];
                        DataColumn column = new DataColumn(columnName, (Type)dataRow["DataType"]);
                        dataTable.Columns.Add(column);
                    }
                    arPubid.Tables.Add(dataTable);
                    while (dataReader.Read())
                    {
                        DataRow dataRow = dataTable.NewRow();

                        for (int i = 0; i < dataReader.FieldCount; i++)
                            dataRow[i] = dataReader.GetValue(i);

                        dataTable.Rows.Add(dataRow);
                    }
                }
                return arPubid;
            }
        }
        
        //Query to display Timeouts data

        public static DataSet displayTimeouts()
        {
            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_GetTimeout);
            DataSet arPubid = new DataSet();

            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                DataTable schemaTable = dataReader.GetSchemaTable();
                DataTable dataTable = new DataTable();

                if (schemaTable != null)
                {
                    for (int i = 0; i < schemaTable.Rows.Count; i++)
                    {
                        DataRow dataRow = schemaTable.Rows[i];
                        string columnName = (string)dataRow["ColumnName"];
                        DataColumn column = new DataColumn(columnName, (Type)dataRow["DataType"]);
                        dataTable.Columns.Add(column);
                    }
                    arPubid.Tables.Add(dataTable);
                    while (dataReader.Read())
                    {
                        DataRow dataRow = dataTable.NewRow();

                        for (int i = 0; i < dataReader.FieldCount; i++)
                            dataRow[i] = dataReader.GetValue(i);

                        dataTable.Rows.Add(dataRow);
                    }
                }
                return arPubid;
            }
        }

        //*Display Rotation Publicatons - To Get the ratation pub Dates to display in excel.

        public static RotationPubsCollection DisplayRotationPubs(int Confid)
        {
            RotationPubsCollection coll = new RotationPubsCollection();
            DataSet ds = new DataSet();
            ds = displayRotationPubsData(Confid);

            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    RotationPubs newMultiSelectItem = new RotationPubs(System.Convert.ToInt32(dr.ItemArray[0]),
                        System.Convert.ToString(dr.ItemArray[1]),
                        System.Convert.ToDateTime(dr.ItemArray[2]).ToShortDateString()+"-"+System.Convert.ToDateTime(dr.ItemArray[3]).ToShortDateString(),                        
                        System.Convert.ToString(dr.ItemArray[4]), 
                        System.Convert.ToString(dr.ItemArray[5])
                        );
                    coll.Add(newMultiSelectItem);
                }
            }
            return (coll);
        }

        // Query to display Rotation Publications

        public static DataSet displayRotationPubsData(int Confid)
        {
            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_GetRotationPubsByConfid);
            db.AddInParameter(dbCommand, "@ConfID", DbType.Int32, Confid);           
            DataSet arPubid = new DataSet();

            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                DataTable schemaTable = dataReader.GetSchemaTable();
                DataTable dataTable = new DataTable();

                if (schemaTable != null)
                {
                    for (int i = 0; i < schemaTable.Rows.Count; i++)
                    {
                        DataRow dataRow = schemaTable.Rows[i];
                        string columnName = (string)dataRow["ColumnName"];
                        DataColumn column = new DataColumn(columnName, (Type)dataRow["DataType"]);
                        dataTable.Columns.Add(column);
                    }
                    arPubid.Tables.Add(dataTable);
                    while (dataReader.Read())
                    {
                        DataRow dataRow = dataTable.NewRow();

                        for (int i = 0; i < dataReader.FieldCount; i++)
                            dataRow[i] = dataReader.GetValue(i);

                        dataTable.Rows.Add(dataRow);
                    }
                }
                return arPubid;
            }
        }

        //GetTimeout - to save the 3 timeout (Rotation time, Page expiration time, Session expiration time)
        public static void SetTimeout(int RotateTime, int PageTime, int SessionTime, int Userid)
        {
            DataSet ds = new DataSet();
            ds = AddTimeout(RotateTime, PageTime, SessionTime, Userid);
        }

        // GetConf - To Save the conference Dates to database.

        public static void SetConf(string confName, int confseq, int maxOrder, DateTime sDate, DateTime eDate)
        {
            DataSet ds = new DataSet();
            ds = AddConf(confName, confseq, maxOrder, sDate, eDate);
        }


        public static DataSet AddTimeout(int RotateTime, int PageTime, int SessionTime, int Userid)
        {
            Database db = DatabaseFactory.CreateDatabase();

            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_SetTimeoutByTimeid);
            db.AddInParameter(dbCommand, "@rotationtime", DbType.Int32, RotateTime);
            db.AddInParameter(dbCommand, "@pagetime", DbType.Int32, PageTime);
            db.AddInParameter(dbCommand, "@sessiontime", DbType.Int32, SessionTime);
            db.AddInParameter(dbCommand, "@userid", DbType.Int32, Userid);

            DataSet ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }

        // Query to Add the Conference Dates

        public static DataSet AddConf(string confname, int confseq, int maxOrder, DateTime sDate, DateTime eDate)
        {
            Database db = DatabaseFactory.CreateDatabase();
            int active = 1;

            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand("SP_ADMIN_CreateNewConference");
            db.AddInParameter(dbCommand, "@confname", DbType.String, confname);
            db.AddInParameter(dbCommand, "@confseq", DbType.String, confseq);
            db.AddInParameter(dbCommand, "@maxOrder", DbType.Int32, maxOrder);
            db.AddInParameter(dbCommand, "@sDate", DbType.DateTime, sDate);
            db.AddInParameter(dbCommand, "@eDate", DbType.DateTime, eDate);
            db.AddInParameter(dbCommand, "@active", DbType.String, active);

            DataSet ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }

        public static DataSet getconfid(Database db)
        {
            System.Collections.ArrayList arcount = new ArrayList();
            DataSet dsConfid = new DataSet();

            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_GetMAXConferenceID);

            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                DataTable schemaTable = dataReader.GetSchemaTable();
                DataTable dataTable = new DataTable();

                if (schemaTable != null)
                {
                    for (int i = 0; i < schemaTable.Rows.Count; i++)
                    {
                        DataRow dataRow = schemaTable.Rows[i];
                        string columnName = (string)dataRow["ColumnName"];
                        DataColumn column = new DataColumn(columnName, (Type)dataRow["DataType"]);
                        dataTable.Columns.Add(column);
                    }
                    dsConfid.Tables.Add(dataTable);
                    while (dataReader.Read())
                    {
                        DataRow dataRow = dataTable.NewRow();

                        for (int i = 0; i < dataReader.FieldCount; i++)
                            dataRow[i] = dataReader.GetValue(i);

                        dataTable.Rows.Add(dataRow);
                    }
                }
                return dsConfid;
            }
        }

        public static void UpdateConf(int Confid,string confname, int maxOrder,DateTime sDate, DateTime eDate)
        {
            ConfCollection coll = new ConfCollection();
            DataSet ds = new DataSet();
            ds = UpdateConfData(Confid, confname, maxOrder, sDate, eDate);
        }

        public static DataSet UpdateConfData(int Confid, string confname, int maxOrder,DateTime sDate, DateTime eDate)
        {
            Database db = DatabaseFactory.CreateDatabase();

            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_SetUpdateConferenceByConfID);
            db.AddInParameter(dbCommand, "@Confid", DbType.Int32, Confid);
            db.AddInParameter(dbCommand, "@confname", DbType.String, confname);
            db.AddInParameter(dbCommand, "@maxOrder", DbType.Int32, maxOrder);
            db.AddInParameter(dbCommand, "@sDate", DbType.DateTime, sDate);
            db.AddInParameter(dbCommand, "@eDate", DbType.DateTime, eDate);

            DataSet ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }


        public static DataSet GetKioskPub(int Pubid)
        {
            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_GetKioskPub);
            db.AddInParameter(dbCommand, "@Pubid", DbType.Int32, Pubid);           
            DataSet arPubid = new DataSet();

            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                DataTable schemaTable = dataReader.GetSchemaTable();
                DataTable dataTable = new DataTable();

                if (schemaTable != null)
                {
                    for (int i = 0; i < schemaTable.Rows.Count; i++)
                    {
                        DataRow dataRow = schemaTable.Rows[i];
                        string columnName = (string)dataRow["ColumnName"];
                        DataColumn column = new DataColumn(columnName, (Type)dataRow["DataType"]);
                        dataTable.Columns.Add(column);
                    }
                    arPubid.Tables.Add(dataTable);
                    while (dataReader.Read())
                    {
                        DataRow dataRow = dataTable.NewRow();

                        for (int i = 0; i < dataReader.FieldCount; i++)
                            dataRow[i] = dataReader.GetValue(i);

                        dataTable.Rows.Add(dataRow);
                    }
                }
                return arPubid;
            }
        }

        // Query to Delete the Conference
        public static DataSet DeleteConfLU(int Cnfid)
        {
            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_DeleteConferenceByCnfid);
            db.AddInParameter(dbCommand, "@Cnfid", DbType.Int32, Cnfid);
            DataSet ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }

        #endregion Conference

        //******************************************************************************************/
        //* Lookup - Audience  - Sapna Linus
        //******************************************************************************************/
       
        #region Audience
        public static AudienceCollection GetAudienceLU()
        {
            AudienceCollection coll = new AudienceCollection();
            DataSet ds = new DataSet();
            ds = displayAudience();

            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Audience newMultiSelectItem = new Audience(System.Convert.ToInt32(dr.ItemArray[0]),
                        System.Convert.ToString(dr.ItemArray[1]), System.Convert.ToBoolean(dr.ItemArray[2]));
                    coll.Add(newMultiSelectItem);
                }
            }
            return (coll);
        }

        public static DataSet displayAudience()
        {
            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_GetAllLUAudience);
            DataSet arAudid = new DataSet();

            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                DataTable schemaTable = dataReader.GetSchemaTable();
                DataTable dataTable = new DataTable();

                if (schemaTable != null)
                {
                    for (int i = 0; i < schemaTable.Rows.Count; i++)
                    {
                        DataRow dataRow = schemaTable.Rows[i];
                        string columnName = (string)dataRow["ColumnName"];
                        DataColumn column = new DataColumn(columnName, (Type)dataRow["DataType"]);
                        dataTable.Columns.Add(column);
                    }
                    arAudid.Tables.Add(dataTable);
                    while (dataReader.Read())
                    {
                        DataRow dataRow = dataTable.NewRow();

                        for (int i = 0; i < dataReader.FieldCount; i++)
                            dataRow[i] = dataReader.GetValue(i);

                        dataTable.Rows.Add(dataRow);
                    }
                }
                return arAudid;
            }
        }

        // Query to Add the Audience

        public static DataSet AddAudience(string auddesc, string audcode)
        {
            Database db = DatabaseFactory.CreateDatabase();

            string auddesc7 = null;
            string auddesc12 = null;
            int active = 1;

            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_SetAudienceByAudid);
            db.AddInParameter(dbCommand, "@audcode", DbType.String, audcode);
            db.AddInParameter(dbCommand, "@auddesc", DbType.String, auddesc);
            db.AddInParameter(dbCommand, "@auddesc7", DbType.String, auddesc7);
            db.AddInParameter(dbCommand, "@auddesc12", DbType.String, auddesc12);
            db.AddInParameter(dbCommand, "@active", DbType.String, active);

            DataSet ds = db.ExecuteDataSet(dbCommand);

            return ds;
           }

        // Query to Delete the Audience
        public static DataSet DeleteAudienceLU(int Audid)
        {
            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_DeleteAudienceByAudid);
            db.AddInParameter(dbCommand, "@Audid", DbType.String, Audid);
            DataSet ds = db.ExecuteDataSet(dbCommand);
            
            return ds;
        }

        // Query to Update the Audience
        public static DataSet UpdateAudienceLU(int Audid, string Audname, int Active)
        {
            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_UpdateAudienceByAudid);
            db.AddInParameter(dbCommand, "@Audid", DbType.String, Audid);
            db.AddInParameter(dbCommand, "@Active", DbType.Int32, Active);
            db.AddInParameter(dbCommand, "@Audname", DbType.String, Audname);
            DataSet ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }

        //Query to Check if the Audience exisits in Associate Tables
         public static Boolean AudExist(int Audid)
         {
             Boolean audExist = false;
             Database db = DatabaseFactory.CreateDatabase();
             System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_CheckExistAudienceByAudid);
             db.AddInParameter(dbCommand, "@Audid", DbType.String, Audid);
             DataSet ds = db.ExecuteDataSet(dbCommand);
            if (ds.Tables.Count > 0)
            {
             foreach (DataRow dr in ds.Tables[0].Rows)
             {
                 int aEx = System.Convert.ToInt32(dr.ItemArray[0]);
                 audExist = Convert.ToBoolean(aEx);
             }
             }
             return audExist;
         }
        #endregion

        //******************************************************************************************/
        //* Lookup - Award  - Sapna Linus
        //******************************************************************************************/
       
        #region Award
        public static AwardCollection GetAwardLU()
        {
            AwardCollection coll = new AwardCollection();
            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_GetAllLUAwards);
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    Award newMultiSelectItem = new Award(
                        System.Convert.ToInt32(dr["AWARDID"].ToString()),
                        dr["Award_Description"].ToString(),
                        dr["AWARD_NAME"].ToString(), dr["AWARD_Category"].ToString(),
                        dr["AWARD_YEAR"].ToString(), System.Convert.ToBoolean(dr["ACTIVE_FLAG"])
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }


            //DataSet ds = new DataSet();
            //ds = displayAward();

            //if (ds.Tables.Count > 0)
            //{
            //    foreach (DataRow dr in ds.Tables[0].Rows)
            //    {
            //        Award newMultiSelectItem = new Award(System.Convert.ToInt32(dr.ItemArray[0]),
            //            System.Convert.ToString(dr.ItemArray[1]), System.Convert.ToBoolean(dr.ItemArray[2]));
            //        coll.Add(newMultiSelectItem);
            //    }
            //}
            //return (coll);


            //SponsorCollection coll = new SponsorCollection();

            //Database db = DatabaseFactory.CreateDatabase();
            //System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetAllLUSponsors);
            //db.AddInParameter(dbCommand, "@Active", DbType.Int32, System.Convert.ToInt32(active));
            //using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            //{
            //    while (dr.Read())
            //    {
            //        Sponsor newMultiSelectItem = new Sponsor(
            //            System.Convert.ToInt32(dr["SPONSORID"].ToString()),
            //            dr["DESCRIPTION"].ToString(), dr["SPONSORCODE"].ToString(),
            //            dr["LONG_DESCRIPTION"].ToString(), System.Convert.ToBoolean(dr["ACTIVE_FLAG"])
            //        );
            //        coll.Add(newMultiSelectItem);
            //    }
            //    return (coll);
            //}
        }

        public static string GetAwardLUbyAwardid(string Awardid)
            {
                Database db = DatabaseFactory.CreateDatabase();
                string strSql = "SELECT AWARD_YEAR FROM LU_AWARDS where AWARDID = @AWARDID";
                string sYear = String.Empty;

                strSql = System.Text.RegularExpressions.Regex.Replace(strSql, "@AWARDID", Awardid);
                System.Data.Common.DbCommand dbCommand = db.GetSqlStringCommand(strSql);
                 
                ArrayList list = new ArrayList();
                DataSet styear = new DataSet();
                
                
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    DataTable schemaTable = dataReader.GetSchemaTable();
                    DataTable dataTable = new DataTable();

                    if (schemaTable != null)
                    {
                        for (int i = 0; i < schemaTable.Rows.Count; i++)
                        {
                            DataRow dataRow = schemaTable.Rows[i];
                            string columnName = (string)dataRow["ColumnName"];
                            DataColumn column = new DataColumn(columnName, (Type)dataRow["DataType"]);
                            dataTable.Columns.Add(column);
                        }
                        styear.Tables.Add(dataTable);
                        while (dataReader.Read())
                        {
                            DataRow dataRow = dataTable.NewRow();

                            for (int i = 0; i < dataReader.FieldCount; i++)
                                dataRow[i] = dataReader.GetValue(i);
                                dataTable.Rows.Add(dataRow);
                                sYear = Convert.ToString(dataRow.ItemArray[0]);
                        }

                    }
                   return sYear;
                }
}
        public static DataSet displayAward()
        {
            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_GetAllLUAwards);
            DataSet arAwdid = new DataSet();

            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                DataTable schemaTable = dataReader.GetSchemaTable();
                DataTable dataTable = new DataTable();

                if (schemaTable != null)
                {
                    for (int i = 0; i < schemaTable.Rows.Count; i++)
                    {
                        DataRow dataRow = schemaTable.Rows[i];
                        string columnName = (string)dataRow["ColumnName"];
                        DataColumn column = new DataColumn(columnName, (Type)dataRow["DataType"]);
                        dataTable.Columns.Add(column);
                    }
                    arAwdid.Tables.Add(dataTable);
                    while (dataReader.Read())
                    {
                        DataRow dataRow = dataTable.NewRow();

                        for (int i = 0; i < dataReader.FieldCount; i++)
                            dataRow[i] = dataReader.GetValue(i);

                        dataTable.Rows.Add(dataRow);
                    }
                }
                return arAwdid;
            }
        }

        // Query to Add the Award

        public static DataSet AddAward(string awdname, string awdyear, string awdcategory)
        {
            Database db = DatabaseFactory.CreateDatabase();

            string awdlevel = null;
            string awdicon = null;
            int active = 1;

            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_SetAwardByAwdid);
            db.AddInParameter(dbCommand, "@awdname", DbType.String, awdname);
            db.AddInParameter(dbCommand, "@awdyear", DbType.String, awdyear);
            db.AddInParameter(dbCommand, "@awdcategory", DbType.String, awdcategory);
            db.AddInParameter(dbCommand, "@awdlevel", DbType.String, awdlevel);
            db.AddInParameter(dbCommand, "@awdicon", DbType.String, awdicon);
            db.AddInParameter(dbCommand, "@active", DbType.String, active);

            DataSet ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }

        // Query to Delete the Award
        
        public static DataSet DeleteAwardLU(int Awdid)
        {
            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_DeleteAwardByAwdid);
            db.AddInParameter(dbCommand, "@Awdid", DbType.String, Awdid);
            DataSet ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }

        // Query to Update the Award
        public static DataSet UpdateAwardLU(int Awdid, string Awdname, string Awdyear, string Awdcategory, int Active)
        {
            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_UpdateAwardByAwdid);
            db.AddInParameter(dbCommand, "@Awdid", DbType.String, Awdid);
            db.AddInParameter(dbCommand, "@Awdname", DbType.String, Awdname);
            db.AddInParameter(dbCommand, "@Awdyear", DbType.String, Awdyear);
            db.AddInParameter(dbCommand, "@Awdcategory", DbType.String, Awdcategory);
            db.AddInParameter(dbCommand, "@Active", DbType.String, Active);
            DataSet ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }

        //Query to Check if the Award exisits in Associate Tables
        public static Boolean AwdExist(int Awdid)
        {
            Boolean awdExist = false;
            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_CheckExistAwardByAwdid);
            db.AddInParameter(dbCommand, "@Awdid", DbType.String, Awdid);
            DataSet ds = db.ExecuteDataSet(dbCommand);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int aEx = System.Convert.ToInt32(dr.ItemArray[0]);
                    awdExist = Convert.ToBoolean(aEx);
                }
            }
            return awdExist;
        }
        #endregion


        //******************************************************************************************/
        //* Lookup - Sponsor
        //******************************************************************************************/

        #region Sponsor
        public static SponsorCollection GetAllLuSponsors(bool active)
        {
            SponsorCollection coll = new SponsorCollection();

            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetAllLUSponsors);
            db.AddInParameter(dbCommand, "@Active", DbType.Int32, System.Convert.ToInt32(active));
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    Sponsor newMultiSelectItem = new Sponsor(
                        System.Convert.ToInt32(dr["SPONSORID"].ToString()),
                        dr["DESCRIPTION"].ToString(),  dr["SPONSORCODE"].ToString(),
                        dr["LONG_DESCRIPTION"].ToString(), System.Convert.ToBoolean(dr["ACTIVE_FLAG"])
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }

        //public static SponsorCollection GetSponsorsbyKeyword(string keyword, bool active)
        //{
        //    SponsorCollection coll = new SponsorCollection();

            //Database db = DatabaseFactory.CreateDatabase();
            //System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetSponsorByKeyword);
            //db.AddInParameter(dbCommand, "@Key", DbType.AnsiString, keyword);
            //db.AddInParameter(dbCommand, "@Active", DbType.Int32, System.Convert.ToInt32(active));            
            //using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            //{
            //    while (dr.Read())
            //    {
            //        Sponsor newMultiSelectItem = new Sponsor(
            //            System.Convert.ToInt32(dr["SPONSORID"].ToString()),
            //            dr["DESCRIPTION"].ToString(), dr["SPONSORCODE"].ToString(),
            //            dr["LONG_DESCRIPTION"].ToString(), System.Convert.ToBoolean(dr["ACTIVE_FLAG"])
            //        );
            //        coll.Add(newMultiSelectItem);
            //    }               
            //    return (coll);
            //}
       // }

        public static string GetSponsorIDsbyKeyword(string keyword, bool active)
        {
            SponsorCollection coll = new SponsorCollection();

            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetSponsorByKeyword);
            db.AddInParameter(dbCommand, "@Key", DbType.AnsiString, keyword);
            db.AddInParameter(dbCommand, "@Active", DbType.Int32, System.Convert.ToInt32(active));  

            DataSet ds = new DataSet();
            ds = db.ExecuteDataSet(dbCommand);

            string strSponsorIds = "";
            if (ds.Tables[0].Rows.Count > 0)
            {

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    strSponsorIds += ds.Tables[0].Rows[i].ItemArray[0].ToString() + ",";

                }
                strSponsorIds = strSponsorIds.Substring(0, strSponsorIds.Length - 1);
            }

            return strSponsorIds;

        }

        public static SponsorCollection GetSponsorsbySponsoridbyKeyword(string sponsorids, bool active)
        {
            SponsorCollection coll = new SponsorCollection();

            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetSponsorsBySponsoridsIdByKeyword);
            db.AddInParameter(dbCommand, "@Sponsorids", DbType.AnsiString, sponsorids);
            db.AddInParameter(dbCommand, "@Active", DbType.Int32, System.Convert.ToInt32(active));
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    Sponsor newMultiSelectItem = new Sponsor(
                        System.Convert.ToInt32(dr["SPONSORID"].ToString()),
                        dr["DESCRIPTION"].ToString(), dr["SPONSORCODE"].ToString(),
                        dr["LONG_DESCRIPTION"].ToString(), System.Convert.ToBoolean(dr["ACTIVE_FLAG"])
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }



        public static SponsorCollection GetSponsorsbyDesc(string longdescription)
        {
            SponsorCollection coll = new SponsorCollection();

            
            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetSponsorsByDesc);
            db.AddInParameter(dbCommand, "@longdescription", DbType.AnsiString, longdescription.Trim());

            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    Sponsor newMultiSelectItem = new Sponsor(
                        System.Convert.ToInt32(dr["SPONSORID"].ToString()),
                        dr["DESCRIPTION"].ToString(), dr["SPONSORCODE"].ToString(),
                        dr["LONG_DESCRIPTION"].ToString(), System.Convert.ToBoolean(dr["ACTIVE_FLAG"])
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }

        public static SponsorCollection GetSponsorbySponsorid(int sponsorid)
        {      

            SponsorCollection coll = new SponsorCollection();

            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetSponsorBySpid);
            db.AddInParameter(dbCommand, "@sponsorid", DbType.Int32, sponsorid);
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    Sponsor newMultiSelectItem = new Sponsor(
                        System.Convert.ToInt32(dr["SPONSORID"].ToString()),
                        dr["DESCRIPTION"].ToString(), dr["SPONSORCODE"].ToString(),
                        dr["LONG_DESCRIPTION"].ToString(), System.Convert.ToBoolean(dr["ACTIVE_FLAG"])
                    );
                    coll.Add(newMultiSelectItem);
                }

                if (dr.NextResult())
                {
                    while (dr.Read())
                    {
                        Sponsor newMultiSelectItem = new Sponsor(
                        System.Convert.ToInt32(dr["SPONSORID"].ToString()),
                        dr["DESCRIPTION"].ToString(), dr["SPONSORCODE"].ToString(),
                        dr["LONG_DESCRIPTION"].ToString(), System.Convert.ToBoolean(dr["ACTIVE_FLAG"])
                        );
                        coll.Add(newMultiSelectItem);
                    }
                }
                return (coll);
            }
        }
        
        public static int AddSponsor(string longdescription, string sponsorcode)
        {
            Database db = DatabaseFactory.CreateDatabase();
            //int active = 1;

            int iNewSponsorid;

            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_SetSponsorBySpid);
            db.AddInParameter(dbCommand, "@sponsorcode", DbType.String, sponsorcode);
            db.AddInParameter(dbCommand, "@Longdescription", DbType.String, longdescription);
            db.AddOutParameter(dbCommand, "@sponsorid", DbType.Int32,10);

           
            db.ExecuteNonQuery(dbCommand);
            iNewSponsorid = Convert.ToInt32(db.GetParameterValue(dbCommand, "@sponsorid"));          
           

            return iNewSponsorid;
        }

        public static Boolean SponsorExist(int Sponsorid)
        {
            Boolean SponsorExist = false;
            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_CheckExistSponsorBySpid);
            db.AddInParameter(dbCommand, "@sponsorid", DbType.Int32, Sponsorid);
            DataSet ds = db.ExecuteDataSet(dbCommand);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int sEx = System.Convert.ToInt32(dr.ItemArray[0]);
                    SponsorExist = Convert.ToBoolean(sEx);
                }
            }
            return SponsorExist;
        }

        // Query to Delete the Sponsor
        public static DataSet DeleteSponsorLU(int Sponid)
        {
            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_DeleteSponsorBySponid);
            db.AddInParameter(dbCommand, "@sponsorid", DbType.Int32, Sponid);
            DataSet ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }

        // Query to Update the Sponsor
        public static DataSet UpdateSponsorLU(int Sponid, string Sponcode, string LongDesc, int Active)
        {
            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_UpdateSponsorBySponid);
            db.AddInParameter(dbCommand, "@sponsorid", DbType.Int32, Sponid);
            db.AddInParameter(dbCommand, "@sponsorcode", DbType.AnsiString, Sponcode);
            db.AddInParameter(dbCommand, "@longdescription", DbType.AnsiString, LongDesc);
            db.AddInParameter(dbCommand, "@active", DbType.Int32, Active);           
            DataSet ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }

        #endregion

        #region Owner
        public static OwnerCollection GetAllLuOwners(bool active)
        {
            OwnerCollection coll = new OwnerCollection();

            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetAllLUOwners);
            db.AddInParameter(dbCommand, "@Active", DbType.Int32, System.Convert.ToInt32(active));
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    Owner newMultiSelectItem = new Owner(
                        System.Convert.ToInt32(dr["OWNERID"].ToString()),
                        dr["OWNERFIRST_NAME"].ToString(), dr["OWNERLAST_NAME"].ToString(),
                        dr["OWNERMIDDLE_INITIAL"].ToString(),
                        dr["OWNERLONG_NAME"].ToString(), System.Convert.ToBoolean(dr["ACTIVE_FLAG"])
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }

        public static OwnerCollection GetOwnerByFullname(string fname, string lname, string minitial)
        {
            OwnerCollection coll = new OwnerCollection();

            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetOwnerByFullname);
            db.AddInParameter(dbCommand, "@fname", DbType.AnsiString, fname.Trim());
            db.AddInParameter(dbCommand, "@lname", DbType.AnsiString, lname.Trim());
            db.AddInParameter(dbCommand, "@minitial", DbType.AnsiString, minitial.Trim());

            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    Owner newMultiSelectItem = new Owner(
                        System.Convert.ToInt32(dr["OWNERID"].ToString()),
                        dr["OWNERLAST_NAME"].ToString(), dr["OWNERFIRST_NAME"].ToString(),
                        dr["OWNERMIDDLE_INITIAL"].ToString(), dr["OWNERLONG_NAME"].ToString()
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }

        public static int AddOwner(string fname, string lname, string minitial)
        {
            Database db = DatabaseFactory.CreateDatabase();           
            int iNewOwnerid;

            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_SetOwnerByOwnerid);
            db.AddInParameter(dbCommand, "@fname", DbType.String, fname);
            db.AddInParameter(dbCommand, "@lname", DbType.String, lname);
            db.AddInParameter(dbCommand, "@minitial", DbType.String, minitial);
            db.AddOutParameter(dbCommand, "@ownerid", DbType.Int32, 10);


            db.ExecuteNonQuery(dbCommand);
            iNewOwnerid = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ownerid"));


            return iNewOwnerid;
        }

        //public static OwnerCollection GetOwnersbyKeyword(string keyword)
        //{
        //    OwnerCollection coll = new OwnerCollection();

        //    Database db = DatabaseFactory.CreateDatabase();
        //    System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetOwnersByKeyword);
        //    db.AddInParameter(dbCommand, "@Key", DbType.AnsiString, keyword);
        //    using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
        //    {
        //        while (dr.Read())
        //        {
        //            Owner newMultiSelectItem = new Owner(
        //                System.Convert.ToInt32(dr["OWNERID"].ToString()),dr["OWNERFIRST_NAME"].ToString(),
        //                dr["OWNERLAST_NAME"].ToString(), dr["OWNERMIDDLE_INITIAL"].ToString(), 
        //                dr["OWNERLONG_NAME"].ToString(),
        //                System.Convert.ToBoolean(dr["ACTIVE_FLAG"])
        //            );
        //            coll.Add(newMultiSelectItem);
        //        }
        //        return (coll);
        //    }
        //}

        public static string GetOwnerIDsbyKeyword(string keyword)
        {
            OwnerCollection coll = new OwnerCollection();

            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetOwnersByKeyword);
            db.AddInParameter(dbCommand, "@Key", DbType.AnsiString, keyword);

            DataSet ds = new DataSet();
            ds=db.ExecuteDataSet(dbCommand);

            string strOwnerIds = "";
            if(ds.Tables[0].Rows.Count>0)
            {
                
                for(int i=0; i<ds.Tables[0].Rows.Count; i++)
                {
                    strOwnerIds += ds.Tables[0].Rows[i].ItemArray[0].ToString() + ",";

                }
                strOwnerIds = strOwnerIds.Substring(0, strOwnerIds.Length - 1);
            }

            return strOwnerIds;
            
        }

        public static OwnerCollection GetOwnersbyOwneridbyKeyword(string ownerids)
        {
            OwnerCollection coll = new OwnerCollection();

            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetOwnersByOwnerIdByKeyword);
            db.AddInParameter(dbCommand, "@Ownerids", DbType.String, ownerids);
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    Owner newMultiSelectItem = new Owner(
                        System.Convert.ToInt32(dr["OWNERID"].ToString()), dr["OWNERFIRST_NAME"].ToString(),
                        dr["OWNERLAST_NAME"].ToString(), dr["OWNERMIDDLE_INITIAL"].ToString(),
                        dr["OWNERLONG_NAME"].ToString(),
                        System.Convert.ToBoolean(dr["ACTIVE_FLAG"])
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }

        public static DataSet UpdateOwnerLU(int Ownerid, string Firstname, string Lastname, string MiddleInit)
        {
            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_UpdateOwnerByOwnerid);
            db.AddInParameter(dbCommand, "@ownerid", DbType.Int32, Ownerid);
            db.AddInParameter(dbCommand, "@fname", DbType.AnsiString, Firstname);
            db.AddInParameter(dbCommand, "@lname", DbType.AnsiString, Lastname);
            db.AddInParameter(dbCommand, "@minitial", DbType.AnsiString, MiddleInit);            
            DataSet ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }

        public static Boolean OwnerExist(int Ownerid)
        {
            Boolean OwnerExist = false;
            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_CheckExistOwnerByOwnerid);
            db.AddInParameter(dbCommand, "@ownerid", DbType.Int32, Ownerid);
            DataSet ds = db.ExecuteDataSet(dbCommand);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int sEx = System.Convert.ToInt32(dr.ItemArray[0]);
                    OwnerExist = Convert.ToBoolean(sEx);
                }
            }
            return OwnerExist;
        }

        // Query to Delete the Sponsor
        public static DataSet DeleteOwnerLU(int Ownerid)
        {
            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_DeleteOwnerByOwnerid);
            db.AddInParameter(dbCommand, "@ownerid", DbType.Int32, Ownerid);
            DataSet ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }
        #endregion

        //******************************************************************************************/
        //* Lookup - CancerType  - Sapna Linus
        //******************************************************************************************/
        
        #region CancerType

        public static CancerTypeCollection GetCancerTypeLU()
        {
            CancerTypeCollection coll = new CancerTypeCollection();
            DataSet ds = new DataSet();
            ds = displayCancerType();

            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    CancerType newMultiSelectItem = new CancerType(System.Convert.ToInt32(dr.ItemArray[0]),
                        System.Convert.ToString(dr.ItemArray[1]), System.Convert.ToBoolean(dr.ItemArray[2])

                        );
                    coll.Add(newMultiSelectItem);
                }
            }
            return (coll);
        }

        public static DataSet displayCancerType()
        {
            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_GetAllLUCancerType);
            DataSet arCtpyid = new DataSet();

            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                DataTable schemaTable = dataReader.GetSchemaTable();
                DataTable dataTable = new DataTable();

                if (schemaTable != null)
                {
                    for (int i = 0; i < schemaTable.Rows.Count; i++)
                    {
                        DataRow dataRow = schemaTable.Rows[i];
                        string columnName = (string)dataRow["ColumnName"];
                        DataColumn column = new DataColumn(columnName, (Type)dataRow["DataType"]);
                        dataTable.Columns.Add(column);
                    }
                    arCtpyid.Tables.Add(dataTable);
                    while (dataReader.Read())
                    {
                        DataRow dataRow = dataTable.NewRow();

                        for (int i = 0; i < dataReader.FieldCount; i++)
                            dataRow[i] = dataReader.GetValue(i);

                        dataTable.Rows.Add(dataRow);
                    }
                }
                return arCtpyid;
            }
        }

        // Query to Add the Cancer Type

        public static DataSet AddCancerType(string Ctpydesc, string Ctpycode)
        {
            Database db = DatabaseFactory.CreateDatabase();

            string Ctpydesc7 = null;
            string Ctpydesc12 = null;
            int active = 1;

            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_SetCancerTypeByCtpyid);
            db.AddInParameter(dbCommand, "@Ctpycode", DbType.String, Ctpycode);
            db.AddInParameter(dbCommand, "@Ctpydesc", DbType.String, Ctpydesc);
            db.AddInParameter(dbCommand, "@Ctpydesc7", DbType.String, Ctpydesc7);
            db.AddInParameter(dbCommand, "@Ctpydesc12", DbType.String, Ctpydesc12);
            db.AddInParameter(dbCommand, "@active", DbType.String, active);

            DataSet ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }

        // Query to Delete the CancerType
        public static DataSet DeleteCancerTypeLU(int Ctpyid)
        {
            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_DeleteCancerTypeByCtpyid);
            db.AddInParameter(dbCommand, "@Ctpyid", DbType.String, Ctpyid);
            DataSet ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }

        // Query to Update the CancerType
        public static DataSet UpdateCancerTypeLU(int Ctpyid, string Ctpyname, int active)
        {
            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_UpdateCancerTypeByCtypid);
            db.AddInParameter(dbCommand, "@Ctpyid", DbType.String, Ctpyid);
            db.AddInParameter(dbCommand, "@Ctpyname", DbType.String, Ctpyname);
            db.AddInParameter(dbCommand, "@active", DbType.String, active);
            DataSet ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }

        //Query to Check if the CancerType exisits in Associate Tables
        public static Boolean CtpyExist(int Ctpyid)
        {
            Boolean CtpyExist = false;
            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_CheckExistCancerTypeByCtpyid);
            db.AddInParameter(dbCommand, "@Ctpyid", DbType.String, Ctpyid);
            DataSet ds = db.ExecuteDataSet(dbCommand);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int aEx = System.Convert.ToInt32(dr.ItemArray[0]);
                    CtpyExist = Convert.ToBoolean(aEx);
                }
            }
            return CtpyExist;
        }
        #endregion
     
        //******************************************************************************************/
        //* Lookup - Language - Sapna Linus
        //******************************************************************************************/
     
        #region Language

        public static LanguageCollection GetLanguageLU()
        {
            LanguageCollection coll = new LanguageCollection();
            DataSet ds = new DataSet();
            ds = displayLanguage();

            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Language newMultiSelectItem = new Language(System.Convert.ToInt32(dr.ItemArray[0]),
                        System.Convert.ToString(dr.ItemArray[1]), System.Convert.ToBoolean(dr.ItemArray[2])

                        );
                    coll.Add(newMultiSelectItem);
                }
            }
            return (coll);
        }

        public static DataSet displayLanguage()
        {
            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_GetAllLULang);
            DataSet arLngid = new DataSet();

            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                DataTable schemaTable = dataReader.GetSchemaTable();
                DataTable dataTable = new DataTable();

                if (schemaTable != null)
                {
                    for (int i = 0; i < schemaTable.Rows.Count; i++)
                    {
                        DataRow dataRow = schemaTable.Rows[i];
                        string columnName = (string)dataRow["ColumnName"];
                        DataColumn column = new DataColumn(columnName, (Type)dataRow["DataType"]);
                        dataTable.Columns.Add(column);
                    }
                    arLngid.Tables.Add(dataTable);
                    while (dataReader.Read())
                    {
                        DataRow dataRow = dataTable.NewRow();

                        for (int i = 0; i < dataReader.FieldCount; i++)
                            dataRow[i] = dataReader.GetValue(i);

                        dataTable.Rows.Add(dataRow);
                    }
                }
                return arLngid;
            }
        }

        // Query to Add the Language

        public static DataSet AddLanguage(string Lngdesc, string Lngcode)
        {
            Database db = DatabaseFactory.CreateDatabase();

            string Lngdesc7 = null;
            string Lngdesc12 = null;
            int active = 1;

            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_SetLangByLngid);
            db.AddInParameter(dbCommand, "@Lngcode", DbType.String, Lngcode);
            db.AddInParameter(dbCommand, "@Lngdesc", DbType.String, Lngdesc);
            db.AddInParameter(dbCommand, "@Lngdesc7", DbType.String, Lngdesc7);
            db.AddInParameter(dbCommand, "@Lngdesc12", DbType.String, Lngdesc12);
            db.AddInParameter(dbCommand, "@active", DbType.String, active);

            DataSet ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }

        // Query to Delete the Language
        public static DataSet DeleteLanguageLU(int Lngid)
        {
            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_DeleteLanguageByLngid);
            db.AddInParameter(dbCommand, "@Lngid", DbType.String, Lngid);
            DataSet ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }

        // Query to Update the Language
        public static DataSet UpdateLanguageLU(int Lngid, string Lngname, int active)
        {
            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_UpdateLanguageByLngid);
            db.AddInParameter(dbCommand, "@Lngid", DbType.String, Lngid);
            db.AddInParameter(dbCommand, "@Lngname", DbType.String, Lngname);
            db.AddInParameter(dbCommand, "@active", DbType.String, active);
            DataSet ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }

        //Query to Check if the Language exisits in Associate Tables
        public static Boolean LngExist(int Lngid)
        {
            Boolean LngExist = false;
            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_CheckExistLanguageByCtpyid);
            db.AddInParameter(dbCommand, "@Lngid", DbType.String, Lngid);
            DataSet ds = db.ExecuteDataSet(dbCommand);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int aEx = System.Convert.ToInt32(dr.ItemArray[0]);
                    LngExist = Convert.ToBoolean(aEx);
                }
            }
            return LngExist;
        }
        #endregion

        //******************************************************************************************/
        //* Lookup - Product Format - Sapna Linus
        //******************************************************************************************/
      
        #region ProductFormat

        public static ProductFormatCollection GetProductFormatLU()
        {
            ProductFormatCollection coll = new ProductFormatCollection();
            DataSet ds = new DataSet();
            ds = displayProductFormat();

            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ProductFormat newMultiSelectItem = new ProductFormat(System.Convert.ToInt32(dr.ItemArray[0]),
                        System.Convert.ToString(dr.ItemArray[1]), System.Convert.ToBoolean(dr.ItemArray[2])

                        );
                    coll.Add(newMultiSelectItem);
                }
            }
            return (coll);
        }

        public static DataSet displayProductFormat()
        {
            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_GetAllLUProdFormat);
            DataSet arPftid = new DataSet();

            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                DataTable schemaTable = dataReader.GetSchemaTable();
                DataTable dataTable = new DataTable();

                if (schemaTable != null)
                {
                    for (int i = 0; i < schemaTable.Rows.Count; i++)
                    {
                        DataRow dataRow = schemaTable.Rows[i];
                        string columnName = (string)dataRow["ColumnName"];
                        DataColumn column = new DataColumn(columnName, (Type)dataRow["DataType"]);
                        dataTable.Columns.Add(column);
                    }
                    arPftid.Tables.Add(dataTable);
                    while (dataReader.Read())
                    {
                        DataRow dataRow = dataTable.NewRow();

                        for (int i = 0; i < dataReader.FieldCount; i++)
                            dataRow[i] = dataReader.GetValue(i);

                        dataTable.Rows.Add(dataRow);
                    }
                }
                return arPftid;
            }
        }

        // Query to Add the Product Format

        public static DataSet AddProductFormat(string Pftdesc, string Pftcode)
        {
            Database db = DatabaseFactory.CreateDatabase();

            string Pftdesc7 = null;
            string Pftdesc12 = null;
            int active = 1;

            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_SetProductFormatByPftid);
            db.AddInParameter(dbCommand, "@Pftcode", DbType.String, Pftcode);
            db.AddInParameter(dbCommand, "@Pftdesc", DbType.String, Pftdesc);
            db.AddInParameter(dbCommand, "@Pftdesc7", DbType.String, Pftdesc7);
            db.AddInParameter(dbCommand, "@Pftdesc12", DbType.String, Pftdesc12);
            db.AddInParameter(dbCommand, "@active", DbType.String, active);

            DataSet ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }

        // Query to Delete the Product Format
        public static DataSet DeleteProductFormatLU(int Pftid)
        {
            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_DeleteProductFormatByPftid);
            db.AddInParameter(dbCommand, "@Pftid", DbType.String, Pftid);
            DataSet ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }

        // Query to Update the Product Format
        public static DataSet UpdateProductFormatLU(int Pftid, string Pftname, int active)
        {
            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_UpdateProductFormatByPftid);
            db.AddInParameter(dbCommand, "@Pftid", DbType.String, Pftid);
            db.AddInParameter(dbCommand, "@Pftname", DbType.String, Pftname);
            db.AddInParameter(dbCommand, "@active", DbType.String, active);
            DataSet ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }

        //Query to Check if the ProductFormat exisits in Associate Tables
        public static Boolean PftExist(int Pftid)
        {
            Boolean PftExist = false;
            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_CheckExistProdFormatByPftid);
            db.AddInParameter(dbCommand, "@Pftid", DbType.String, Pftid);
            DataSet ds = db.ExecuteDataSet(dbCommand);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int aEx = System.Convert.ToInt32(dr.ItemArray[0]);
                    PftExist = Convert.ToBoolean(aEx);
                }
            }
            return PftExist;
        }
        #endregion

        //******************************************************************************************/
        //* Lookup - Reading Level - Sapna Linus
        //******************************************************************************************/
     
        #region ReadingLevel

        public static ReadingLevelCollection GetReadingLevelLU()
        {
            ReadingLevelCollection coll = new ReadingLevelCollection();
            DataSet ds = new DataSet();
            ds = displayReadingLevel();

            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ReadingLevel newMultiSelectItem = new ReadingLevel(System.Convert.ToInt32(dr.ItemArray[0]),
                        System.Convert.ToString(dr.ItemArray[1]), System.Convert.ToBoolean(dr.ItemArray[2])

                        );
                    coll.Add(newMultiSelectItem);
                }
            }
            return (coll);
        }

        public static DataSet displayReadingLevel()
        {
            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_GetAllLUReadinglevel);
            DataSet arRdlid = new DataSet();

            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                DataTable schemaTable = dataReader.GetSchemaTable();
                DataTable dataTable = new DataTable();

                if (schemaTable != null)
                {
                    for (int i = 0; i < schemaTable.Rows.Count; i++)
                    {
                        DataRow dataRow = schemaTable.Rows[i];
                        string columnName = (string)dataRow["ColumnName"];
                        DataColumn column = new DataColumn(columnName, (Type)dataRow["DataType"]);
                        dataTable.Columns.Add(column);
                    }
                    arRdlid.Tables.Add(dataTable);
                    while (dataReader.Read())
                    {
                        DataRow dataRow = dataTable.NewRow();

                        for (int i = 0; i < dataReader.FieldCount; i++)
                            dataRow[i] = dataReader.GetValue(i);

                        dataTable.Rows.Add(dataRow);
                    }
                }
                return arRdlid;
            }
        }

        // Query to Add the Reading Level
        public static DataSet AddReadingLevel(string Rdldesc, string Rdlcode)
        {
            Database db = DatabaseFactory.CreateDatabase();

            string Rdldesc7 = null;
            string Rdldesc12 = null;
            int active = 1;

            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_SetReadingLevelByRdlid);
            db.AddInParameter(dbCommand, "@Rdlcode", DbType.String, Rdlcode);
            db.AddInParameter(dbCommand, "@Rdldesc", DbType.String, Rdldesc);
            db.AddInParameter(dbCommand, "@Rdldesc7", DbType.String, Rdldesc7);
            db.AddInParameter(dbCommand, "@Rdldesc12", DbType.String, Rdldesc12);
            db.AddInParameter(dbCommand, "@active", DbType.String, active);

            DataSet ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }

        // Query to Delete the Reading Level
        public static DataSet DeleteReadingLevelLU(int Rdlid)
        {
            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_DeleteReadinglevelByRdlid);
            db.AddInParameter(dbCommand, "@Rdlid", DbType.String, Rdlid);
            DataSet ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }

        // Query to Update the Reading Level
        public static DataSet UpdateReadingLevelLU(int Rdlid, string Rdlname, int active)
        {
            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_UpdateReadinglevelByRdlid);
            db.AddInParameter(dbCommand, "@Rdlid", DbType.String, Rdlid);
            db.AddInParameter(dbCommand, "@Rdlname", DbType.String, Rdlname);
            db.AddInParameter(dbCommand, "@active", DbType.String, active);
            DataSet ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }

        //Query to Check if the Reading Level exisits in TBL_Products Tables
        public static Boolean RdlExist(int Rdlid)
        {
            Boolean RdlExist = false;
            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_CheckExistReadinglevelByRdlid);
            db.AddInParameter(dbCommand, "@Rdlid", DbType.String, Rdlid);
            DataSet ds = db.ExecuteDataSet(dbCommand);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int aEx = System.Convert.ToInt32(dr.ItemArray[0]);
                    RdlExist = Convert.ToBoolean(aEx);
                }
            }
            return RdlExist;
        }
        #endregion

        //******************************************************************************************/
        //* Lookup - Race - Sapna Linus
        //******************************************************************************************/
     
        #region Race

        public static RaceCollection GetRaceLU()
        {
            RaceCollection coll = new RaceCollection();
            DataSet ds = new DataSet();
            ds = displayRace();

            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Race newMultiSelectItem = new Race(System.Convert.ToInt32(dr.ItemArray[0]),
                        System.Convert.ToString(dr.ItemArray[1]), System.Convert.ToBoolean(dr.ItemArray[2])

                        );
                    coll.Add(newMultiSelectItem);
                }
            }
            return (coll);
        }

        public static DataSet displayRace()
        {
            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_GetAllLURace);
            DataSet arRceid = new DataSet();

            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                DataTable schemaTable = dataReader.GetSchemaTable();
                DataTable dataTable = new DataTable();

                if (schemaTable != null)
                {
                    for (int i = 0; i < schemaTable.Rows.Count; i++)
                    {
                        DataRow dataRow = schemaTable.Rows[i];
                        string columnName = (string)dataRow["ColumnName"];
                        DataColumn column = new DataColumn(columnName, (Type)dataRow["DataType"]);
                        dataTable.Columns.Add(column);
                    }
                    arRceid.Tables.Add(dataTable);
                    while (dataReader.Read())
                    {
                        DataRow dataRow = dataTable.NewRow();

                        for (int i = 0; i < dataReader.FieldCount; i++)
                            dataRow[i] = dataReader.GetValue(i);

                        dataTable.Rows.Add(dataRow);
                    }
                }
                return arRceid;
            }
        }

        // Query to Add the Race
        public static DataSet AddRace(string Rcedesc, string Rcecode)
        {
            Database db = DatabaseFactory.CreateDatabase();

            string Rcedesc7 = null;
            string Rcedesc12 = null;
            int active = 1;

            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_SetRaceByRceid);
            db.AddInParameter(dbCommand, "@Rcecode", DbType.String, Rcecode);
            db.AddInParameter(dbCommand, "@Rcedesc", DbType.String, Rcedesc);
            db.AddInParameter(dbCommand, "@Rcedesc7", DbType.String, Rcedesc7);
            db.AddInParameter(dbCommand, "@Rcedesc12", DbType.String, Rcedesc12);
            db.AddInParameter(dbCommand, "@active", DbType.String, active);

            DataSet ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }

        // Query to Delete the Race 
        public static DataSet DeleteRaceLU(int Rceid)
        {
            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_DeleteRaceByRceid);
            db.AddInParameter(dbCommand, "@Rceid", DbType.String, Rceid);
            DataSet ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }
        // Query to Update the Race
        public static DataSet UpdateRaceLU(int Rceid, string Rcename, int active)
        {
            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_UpdateRaceByRceid);
            db.AddInParameter(dbCommand, "@Rceid", DbType.String, Rceid);
            db.AddInParameter(dbCommand, "@Rcename", DbType.String, Rcename);
            db.AddInParameter(dbCommand, "@active", DbType.String, active);
            DataSet ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }

        //Query to Check if the Race exisits in Associate Tables
        public static Boolean RceExist(int Rceid)
        {
            Boolean RceExist = false;
            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_CheckExistRaceByRceid);
            db.AddInParameter(dbCommand, "@Rceid", DbType.String, Rceid);
            DataSet ds = db.ExecuteDataSet(dbCommand);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int aEx = System.Convert.ToInt32(dr.ItemArray[0]);
                    RceExist = Convert.ToBoolean(aEx);
                }
            }
            return RceExist;
        }
        #endregion

        //******************************************************************************************/
        //* Lookup - Series - Sapna Linus
        //******************************************************************************************/

        #region Series

        //public static SeriesCollection GetSeriesLU()
        //{
        //    SeriesCollection coll = new SeriesCollection();
        //    DataSet ds = new DataSet();
        //    ds = displaySeries();

        //    if (ds.Tables.Count > 0)
        //    {
        //        foreach (DataRow dr in ds.Tables[0].Rows)
        //        {
        //            Series newMultiSelectItem = new Series(System.Convert.ToInt32(dr.ItemArray[0]),
        //                System.Convert.ToString(dr.ItemArray[1]), System.Convert.ToBoolean(dr.ItemArray[2])

        //                );
        //            coll.Add(newMultiSelectItem);
        //        }
        //    }
        //    return (coll);
        //}
        //Modified for NCIPL_CC
        public static SeriesCollection GetSeriesLU()
        {
            SeriesCollection coll = new SeriesCollection();
            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_GetAllLUSeries);

            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    Series collItem = new Series(
                                                 dr.GetInt32(dr.GetOrdinal("SERIESID")),
                                                (dr["DESCRIPTION"] == DBNull.Value) ? "" : dr["DESCRIPTION"].ToString(),
                                                (dr["ACTIVE_FLAG"] == DBNull.Value) ? false : (dr.GetInt32(dr.GetOrdinal("ACTIVE_FLAG")) == 1) ? true: false,
                                                (dr["NCIPL"] == DBNull.Value) ? false : (dr.GetInt32(dr.GetOrdinal("NCIPL")) == 1) ? true: false,
                                                (dr["NCIPL_CC"] == DBNull.Value) ? false : (dr.GetInt32(dr.GetOrdinal("NCIPL_CC")) == 1) ? true: false
                                                  );
                    coll.Add(collItem);
                }
            }
            return (coll);
        }

        //public static DataSet displaySeries()
        //{
        //    Database db = DatabaseFactory.CreateDatabase();
        //    System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_GetAllLUSeries);
        //    DataSet arSreid = new DataSet();

        //    using (IDataReader dataReader = db.ExecuteReader(dbCommand))
        //    {
        //        DataTable schemaTable = dataReader.GetSchemaTable();
        //        DataTable dataTable = new DataTable();

        //        if (schemaTable != null)
        //        {
        //            for (int i = 0; i < schemaTable.Rows.Count; i++)
        //            {
        //                DataRow dataRow = schemaTable.Rows[i];
        //                string columnName = (string)dataRow["ColumnName"];
        //                DataColumn column = new DataColumn(columnName, (Type)dataRow["DataType"]);
        //                dataTable.Columns.Add(column);
        //            }
        //            arSreid.Tables.Add(dataTable);
        //            while (dataReader.Read())
        //            {
        //                DataRow dataRow = dataTable.NewRow();

        //                for (int i = 0; i < dataReader.FieldCount; i++)
        //                    dataRow[i] = dataReader.GetValue(i);

        //                dataTable.Rows.Add(dataRow);
        //            }
        //        }
        //        return arSreid;
        //    }
        //}
        //Modified for NCIPL_CC
        public static DataSet displaySeries()
        {
            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_GetAllLUSeries);
            DataSet arSreid = new DataSet();

            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                DataTable schemaTable = dataReader.GetSchemaTable();
                DataTable dataTable = new DataTable();

                if (schemaTable != null)
                {
                    for (int i = 0; i < schemaTable.Rows.Count; i++)
                    {
                        DataRow dataRow = schemaTable.Rows[i];
                        string columnName = (string)dataRow["ColumnName"];
                        DataColumn column = new DataColumn(columnName, (Type)dataRow["DataType"]);
                        dataTable.Columns.Add(column);
                    }
                    arSreid.Tables.Add(dataTable);
                    while (dataReader.Read())
                    {
                        DataRow dataRow = dataTable.NewRow();

                        for (int i = 0; i < dataReader.FieldCount; i++)
                            dataRow[i] = dataReader.GetValue(i);

                        dataTable.Rows.Add(dataRow);
                    }
                }
                return arSreid;
            }
        }

        //// Query to Add the Series
        //public static DataSet AddSeries(string Sredesc, string Srecode)
        //{
        //    Database db = DatabaseFactory.CreateDatabase();

        //    string Sredesc7 = null;
        //    string Sredesc12 = null;
        //    int active = 1;

        //    System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_SetSeriesBySreid);
        //    db.AddInParameter(dbCommand, "@Srecode", DbType.String, Srecode);
        //    db.AddInParameter(dbCommand, "@Sredesc", DbType.String, Sredesc);
        //    db.AddInParameter(dbCommand, "@Sredesc7", DbType.String, Sredesc7);
        //    db.AddInParameter(dbCommand, "@Sredesc12", DbType.String, Sredesc12);
        //    db.AddInParameter(dbCommand, "@active", DbType.String, active);

        //    DataSet ds = db.ExecuteDataSet(dbCommand);

        //    return ds;
        //}
        //Query to Add the Series - Modified for NCIPL_CC
        public static DataSet AddSeries(string Sredesc, string Srecode, int NCIPL, int NCIPL_CC)
        {
            Database db = DatabaseFactory.CreateDatabase();

            string Sredesc7 = null;
            string Sredesc12 = null;
            int active = 1;

            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_SetSeriesBySreid);
            db.AddInParameter(dbCommand, "@Srecode", DbType.String, Srecode);
            db.AddInParameter(dbCommand, "@Sredesc", DbType.String, Sredesc);
            db.AddInParameter(dbCommand, "@Sredesc7", DbType.String, Sredesc7);
            db.AddInParameter(dbCommand, "@Sredesc12", DbType.String, Sredesc12);
            db.AddInParameter(dbCommand, "@active", DbType.String, active);
            
            if (NCIPL == 1)
                db.AddInParameter(dbCommand, "@NCIPL", DbType.Int32, NCIPL);
            if (NCIPL_CC == 1)
                db.AddInParameter(dbCommand, "@NCIPL_CC", DbType.Int32, NCIPL_CC);

            DataSet ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }

        // Query to Delete the Series
        public static DataSet DeleteSeriesLU(int Sreid)
        {
            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_DeleteSeriesBySreid);
            db.AddInParameter(dbCommand, "@Sreid", DbType.String, Sreid);
            DataSet ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }

        // Query to Update the Series
        public static DataSet UpdateSeriesLU(int Sreid, string Srename, int active)
        {
            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_UpdateSeriesBySreid);
            db.AddInParameter(dbCommand, "@Sreid", DbType.String, Sreid);
            db.AddInParameter(dbCommand, "@Srename", DbType.String, Srename);
            db.AddInParameter(dbCommand, "@active", DbType.String, active);
            DataSet ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }
        // Query to Update the Series - Modified for NCIPL_CC.  -- Called from Grid Update
        public static DataSet UpdateSeriesLU(int Sreid, string Srename, int active, int NCIPL, int NCIPL_CC)
        {
            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_UpdateSeriesBySreid_2);
            db.AddInParameter(dbCommand, "@Sreid", DbType.String, Sreid);
            db.AddInParameter(dbCommand, "@Srename", DbType.String, Srename);
            db.AddInParameter(dbCommand, "@active", DbType.String, active);

            if (NCIPL == 1)
                db.AddInParameter(dbCommand, "@NCIPL", DbType.Int32, NCIPL);
            if (NCIPL_CC == 1)
                db.AddInParameter(dbCommand, "@NCIPL_CC", DbType.Int32, NCIPL_CC);

            DataSet ds = db.ExecuteDataSet(dbCommand);

            return ds;
        }

        //Query to Check if the Series exisits in Associate Tables
        public static Boolean SreExist(int Sreid)
        {
            Boolean SreExist = false;
            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_CheckExistSeriesBySreid);
            db.AddInParameter(dbCommand, "@Sreid", DbType.String, Sreid);
            DataSet ds = db.ExecuteDataSet(dbCommand);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int aEx = System.Convert.ToInt32(dr.ItemArray[0]);
                    SreExist = Convert.ToBoolean(aEx);
                }
            }
            return SreExist;
        }
        
        //Added for NCIPL_CC
        public static Boolean CheckSeriesInterfaceAssociation(int SeriesId, string Interface)
        {
            bool retvalue = false;
            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand cmd = db.GetStoredProcCommand(SP_ADMIN_CheckSeriesByInterfaceBySeriesId);
            db.AddInParameter(cmd, "@SeriesId", DbType.Int32, SeriesId);
            db.AddInParameter(cmd, "@Interface", DbType.String, Interface);
            db.AddOutParameter(cmd, "resultout", DbType.Int32, 0);
            db.ExecuteNonQuery(cmd);
            int intvalue = (int)db.GetParameterValue(cmd, "resultout");
            if (intvalue == 1) retvalue = true;
            return retvalue;
        }

        #endregion

        #endregion Lookups
    }
     
 }

