using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Transactions;

using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using PubEntAdmin.BLL;

namespace PubEntAdmin.DAL
{
    public class PE_DAL
    {
        #region Static variable

        private static readonly string strSP_ADMIN_GetUserbyLoginApp = "sp_getuserbyLoginApp";

        private static readonly string strSP_ADMIN_GetAllAudience = "SP_ADMIN_GetAllAudience";
        private static readonly string strSP_ADMIN_GetAudience = "SP_ADMIN_GetAudience";
        private static readonly string strSP_ADMIN_GetAudienceByPubID = "SP_ADMIN_GetAudienceByPubID";
        private static readonly string strSP_ADMIN_SetAudienceByPubID = "SP_ADMIN_SetAudienceByPubID";

        private static readonly string strSP_ADMIN_GetAllCancerType = "SP_ADMIN_GetAllCancerType";
        private static readonly string strSP_ADMIN_GetCancerType = "SP_ADMIN_GetCancerType";
        private static readonly string strSP_ADMIN_GetCancerByPubID = "SP_ADMIN_GetCancerByPubID";
        private static readonly string strSP_ADMIN_SetCancerByPubID = "SP_ADMIN_SetCancerByPubID";

        private static readonly string strSP_ADMIN_GetAllLang = "SP_ADMIN_GetAllLang";
        private static readonly string strSP_ADMIN_GetLang = "SP_ADMIN_GetLang";
        private static readonly string strSP_ADMIN_GetLangByPubID = "SP_ADMIN_GetLangByPubID";
        private static readonly string strSP_ADMIN_SetLangByPubID = "SP_ADMIN_SetLangByPubID";

        private static readonly string strSP_ADMIN_GetAllProdFormat = "SP_ADMIN_GetAllProdFormat";
        private static readonly string strSP_ADMIN_GetProdFormat = "SP_ADMIN_GetProdFormat";
        private static readonly string strSP_ADMIN_GetProdFormatByPubID = "SP_ADMIN_GetProdFormatByPubID";
        private static readonly string strSP_ADMIN_SetProdFormatByPubID = "SP_ADMIN_SetProdFormatByPubID";

        private static readonly string strSP_ADMIN_GetAllSeries = "SP_ADMIN_GetAllSeries";
        private static readonly string strSP_ADMIN_GetSeries = "SP_ADMIN_GetSeries";
        private static readonly string strSP_ADMIN_GetSeriesByPubID = "SP_ADMIN_GetSeriesByPubID";
        private static readonly string strSP_ADMIN_SetSeriesByPubID = "SP_ADMIN_SetSeriesByPubID";

        private static readonly string strSP_ADMIN_GetAllRace = "SP_ADMIN_GetAllRace";
        private static readonly string strSP_ADMIN_GetRace = "SP_ADMIN_GetRace";
        private static readonly string strSP_ADMIN_GetRaceByPubID = "SP_ADMIN_GetRaceByPubID";
        private static readonly string strSP_ADMIN_SetRaceByPubID = "SP_ADMIN_SetRaceByPubID";

        private static readonly string strSP_ADMIN_GetAllBookstatus = "SP_ADMIN_GetAllBookstatus";
        private static readonly string strSP_ADMIN_GetBookstatus = "SP_ADMIN_GetBookstatus";

        private static readonly string strSP_ADMIN_GetAllReadinglevel = "SP_ADMIN_GetAllReadinglevel";
        private static readonly string strSP_ADMIN_GetReadinglevel = "SP_ADMIN_GetReadinglevel";
        private static readonly string strSP_ADMIN_GetReadlevelByPubID = "SP_ADMIN_GetReadlevelByPubID";
        private static readonly string strSP_ADMIN_SetReadlevelByPubID = "SP_ADMIN_SetReadlevelByPubID";

        private static readonly string strSP_ADMIN_GetAllAwards = "SP_ADMIN_GetAllAwards";
        private static readonly string strSP_ADMIN_GetAwardByPubID = "SP_ADMIN_GetAwardByPubID";
        private static readonly string strSP_ADMIN_SetAwardByPubID = "SP_ADMIN_SetAwardByPubID";

        private static readonly string strSP_ADMIN_GetAllDisplayStatus = "SP_ADMIN_GetAllDisplayStatus";
        private static readonly string strSP_ADMIN_GetNCIPLDisplayStatusByPubID = "SP_ADMIN_GetNCIPLDisplayStatusByPubID";
        private static readonly string strSP_ADMIN_GetROODisplayStatusByPubID = "SP_ADMIN_GetROODisplayStatusByPubID";
        private static readonly string strSP_ADMIN_GetExhDisplayStatusByPubID = "SP_ADMIN_GetExhDisplayStatusByPubID";
        private static readonly string strsp_ADMIN_SetNCIPLDisplayStatusByPubID = "sp_ADMIN_SetNCIPLDisplayStatusByPubID";
        private static readonly string strsp_ADMIN_SetROODisplayStatusByPubID = "sp_ADMIN_SetROODisplayStatusByPubID";
        private static readonly string strsp_ADMIN_SetExhDisplayStatusByPubID = "sp_ADMIN_SetExhDisplayStatusByPubID";

        private static readonly string strSP_ADMIN_GetProductGeneralData = "SP_ADMIN_GetProductGeneralData";
        private static readonly string strSP_ADMIN_SetProductGeneralData = "SP_ADMIN_SetProductGeneralData";
        private static readonly string strSP_ADMIN_GetProductCommonData = "SP_ADMIN_GetProductCommonData";
        private static readonly string strSP_ADMIN_SetProductCommonData = "SP_ADMIN_SetProductCommonData";

        private static readonly string strSP_ADMIN_GetLiveInterfaceByPubID = "SP_ADMIN_GetLiveInterfaceByPubID";
        private static readonly string strSP_ADMIN_SetLiveInterfaceByPubID = "SP_ADMIN_SetLiveInterfaceByPubID";
        /*------------------------------------------------*/
        private static readonly string strSP_ADMIN_GetProdHistByPubID = "SP_ADMIN_GetProdHistByPubID";
        private static readonly string strSP_ADMIN_GetPubHistCnt = "SP_ADMIN_GetPubHistCnt";
        private static readonly string strSP_ADMIN_SetProdHistByPubID = "SP_ADMIN_SetProdHistByPubID";
        private static readonly string strSP_ADMIN_GetOrigPubDate = "SP_ADMIN_GetOrigPubDate";
        private static readonly string strSP_ADMIN_UpdateProdHistByPhdID = "SP_ADMIN_UpdateProdHistByPhdID";
        private static readonly string strSP_ADMIN_GetStatusByPubID = "SP_ADMIN_GetStatusByPubID";
        #region GetInterfaceData
        private static readonly string strSP_ADMIN_GetNCIPLInterfaceDataByPubID = "SP_ADMIN_GetNCIPLInterfaceDataByPubID";
        private static readonly string strSP_ADMIN_GetROOInterfaceDataByPubID = "SP_ADMIN_GetROOInterfaceDataByPubID";
        private static readonly string strSP_ADMIN_GetExhInterfaceDataByPubID = "SP_ADMIN_GetExhInterfaceDataByPubID";
        private static readonly string strSP_ADMIN_GetCatalogInterfaceDataByPubID = "SP_ADMIN_GetCatalogInterfaceDataByPubID";
        #endregion

        #region SetInterfaceData
        private static readonly string strSP_ADMIN_SetNCIPLInterfaceDataByPubID = "SP_ADMIN_SetNCIPLInterfaceDataByPubID";
        private static readonly string strSP_ADMIN_SetROOInterfaceDataByPubID = "SP_ADMIN_SetROOInterfaceDataByPubID";
        private static readonly string strSP_ADMIN_SetExhInterfaceDataByPubID = "SP_ADMIN_SetExhInterfaceDataByPubID";
        private static readonly string strSP_ADMIN_SetExhKioskInterfaceDataByPubID = "SP_ADMIN_SetExhKioskInterfaceDataByPubID";
        private static readonly string strSP_ADMIN_SetCatalogInterfaceDataByPubID = "SP_ADMIN_SetCatalogInterfaceDataByPubID";
        #endregion

        #region Subject
        private static readonly string strSP_ADMIN_GetAllSubject = "SP_ADMIN_GetAllSubject";
        private static readonly string strSP_ADMIN_GetAllNCIPLSubject = "SP_ADMIN_GetAllNCIPLSubject";
        private static readonly string strSP_ADMIN_GetAllConference = "SP_ADMIN_GetAllConference";
        private static readonly string strSP_ADMIN_GetAllROOSubject = "SP_ADMIN_GetAllROOSubject";
        private static readonly string strSP_ADMIN_GetAllNCIPLUsedSubject = "SP_ADMIN_GetAllNCIPLUsedSubject";
        private static readonly string strSP_ADMIN_GetAllROOUsedSubject = "SP_ADMIN_GetAllROOUsedSubject";
        private static readonly string strSP_ADMIN_GetAllCatalogSubject = "SP_ADMIN_GetAllCatalogSubject";
        private static readonly string strSP_ADMIN_SetSubject = "SP_ADMIN_SetSubject";
        private static readonly string strSP_ADMIN_DeleteSubjectBySubjID = "SP_ADMIN_DeleteSubjectBySubjID";
        private static readonly string strSP_ADMIN_EnableSubjectBySubjID = "SP_ADMIN_EnableSubjectBySubjID";

        private static readonly string strSP_ADMIN_GetAllCatalogSubjectHvSubcat = "SP_ADMIN_GetAllCatalogSubjectHvSubcat";
        private static readonly string strSP_ADMIN_GetAllSubjectHvSubcat = "SP_ADMIN_GetAllSubjectHvSubcat";

        private static readonly string strSP_ADMIN_GetAllCatalogWYNTKSubjectForNew = "SP_ADMIN_GetAllCatalogWYNTKSubjectForNew";
        private static readonly string strSP_ADMIN_GetAllCatalogWYNTKSubjectForExist = "SP_ADMIN_GetAllCatalogWYNTKSubjectForExist";


        private static readonly string strSP_ADMIN_GetNCIPLSubjectByPubID = "SP_ADMIN_GetNCIPLSubjectByPubID";
        private static readonly string strSP_ADMIN_GetROOSubjectByPubID = "SP_ADMIN_GetROOSubjectByPubID";

        private static readonly string strSP_ADMIN_SetNCIPLSubjectByPubID = "SP_ADMIN_SetNCIPLSubjectByPubID";
        private static readonly string strSP_ADMIN_SetROOSubjectByPubID = "SP_ADMIN_SetROOSubjectByPubID";

        #region Catalog subject seq

        private static readonly string strSP_ADMIN_GetAllCatalogSubjectSeq = "SP_ADMIN_GetAllCatalogSubjectSeq";
        private static readonly string strSP_ADMIN_SetAllCatalogSubjectSeq = "SP_ADMIN_SetAllCatalogSubjectSeq";

        private static readonly string strSP_ADMIN_GetAllCatalogSubCategorySeq = "SP_ADMIN_GetAllCatalogSubCategorySeq";
        private static readonly string strSP_ADMIN_SetAllCatalogSubCatSeq = "SP_ADMIN_SetAllCatalogSubCatSeq";

        private static readonly string strSP_ADMIN_GetAllCatalogSubSubCategorySeq = "SP_ADMIN_GetAllCatalogSubSubCategorySeq";
        private static readonly string strSP_ADMIN_SetAllCatalogSubSubCatSeq = "SP_ADMIN_SetAllCatalogSubSubCatSeq";
        #endregion

        #endregion

        #region Subcategory
        private static readonly string strSP_ADMIN_GetSubCatalogSubjectBySubjID = "SP_ADMIN_GetSubCatalogSubjectBySubjID";
        private static readonly string strSP_ADMIN_GetAllCatalogSubcatHvSubSubcat = "SP_ADMIN_GetAllCatalogSubcatHvSubSubcat";
        private static readonly string strSP_ADMIN_GetSubCatalogSubjectBySubCatalogID = "SP_ADMIN_GetSubCatalogSubjectBySubCatalogID";
        private static readonly string strSP_ADMIN_GetSubCatalogSubjectBySubCatalogID2 = "SP_ADMIN_GetSubCatalogSubjectBySubCatalogID2";
        private static readonly string strSP_ADMIN_SetSubCat = "SP_ADMIN_SetSubCat";
        #endregion

        #region SubSubcategory
        private static readonly string strSP_ADMIN_GetSubSubCatalogSubjectBySubCatID = "SP_ADMIN_GetSubSubCatalogSubjectBySubCatID";
        private static readonly string strSP_ADMIN_GetSubSubCatalogSubjectBySubSubCatalogID = "SP_ADMIN_GetSubSubCatalogSubjectBySubSubCatalogID";
        private static readonly string strSP_ADMIN_GetAllSubSubCategory = "SP_ADMIN_GetAllSubSubCategory";
        private static readonly string strSP_ADMIN_SetSubSubCat = "SP_ADMIN_SetSubSubCat";
        #endregion

        private static readonly string strSP_ADMIN_GetNCIPLInterfaceViewByPubID = "SP_ADMIN_GetNCIPLInterfaceViewByPubID";
        private static readonly string strSP_ADMIN_GetNCIPLSubjectViewByPubID = "SP_ADMIN_GetNCIPLSubjectViewByPubID";

        private static readonly string strSP_ADMIN_GetROOInterfaceViewByPubID = "SP_ADMIN_GetROOInterfaceViewByPubID";
        private static readonly string strSP_ADMIN_GetROOSubjectViewByPubID = "SP_ADMIN_GetROOSubjectViewByPubID";

        private static readonly string strSP_ADMIN_GetExhInterfaceViewByPubID = "SP_ADMIN_GetExhInterfaceViewByPubID";

        private static readonly string strSP_ADMIN_GetCatalogInterfaceViewByPubID = "SP_ADMIN_GetCatalogInterfaceViewByPubID";

        private static readonly string strSP_ADMIN_SetNewComment = "SP_ADMIN_SetNewComment";
        private static readonly string strSP_ADMIN_GetCommentsByPubId = "SP_ADMIN_GetCommentsByPubId";

        private static readonly string strSP_ADMIN_SetNewAttachment = "SP_ADMIN_SetNewAttachment";
        private static readonly string strSP_ADMIN_GetAttachmentsByPubId = "SP_ADMIN_GetAttachmentsByPubId";
        private static readonly string strSP_ADMIN_GetAttachmentByFileId = "SP_ADMIN_GetAttachmentByFileId";
        private static readonly string strSP_ADMIN_DeleteAttachment = "SP_ADMIN_DeleteAttachment";
        private static readonly string strSP_ADMIN_DeleteAttachmentsByPubId = "SP_ADMIN_DeleteAttachmentsByPubId";

        private static readonly string strSP_ADMIN_GetProdInTranslation = "SP_ADMIN_GetProdInTranslation";
        private static readonly string strSP_ADMIN_SetProdInTranslation = "SP_ADMIN_SetProdInTranslation";
        private static readonly string strSP_ADMIN_DeleteRelatedTranslation = "SP_ADMIN_DeleteRelatedTranslation";

        private static readonly string strSP_ADMIN_GetRelatedProd = "SP_ADMIN_GetRelatedProd";
        private static readonly string strSP_ADMIN_SetRelatedProd = "SP_ADMIN_SetRelatedProd";
        private static readonly string strSP_ADMIN_DeleteRelatedPub = "SP_ADMIN_DeleteRelatedPub";

        private static readonly string strSP_ADMIN_Search = "SP_ADMIN_Search";
        private static readonly string strSP_ADMIN_SearchFromPubID = "SP_ADMIN_SearchFromPubID";

        private static readonly string strSP_ADMIN_GetAllAnnouncement = "SP_ADMIN_GetAllAnnouncement";
        private static readonly string strSP_ADMIN_SetAnnouncement = "SP_ADMIN_SetAnnouncement";

        #region VK_LP

        private static readonly string strSP_ADMIN_GetProdInt = "SP_ADMIN_GetProdInt";

        private static readonly string strSP_ADMIN_GetAllVK_LP = "SP_ADMIN_GetAllVK_LP";
        private static readonly string strSP_ADMIN_DeleteVK_LP = "SP_ADMIN_DeleteVK_LP";

        private static readonly string strSP_ADMIN_SetNewKITPUB = "SP_ADMIN_SetNewKITPUB";
        private static readonly string strSP_ADMIN_SetKITPUB = "SP_ADMIN_SetKITPUB";
        private static readonly string strSP_ADMIN_SetKITPUBs = "SP_ADMIN_SetKITPUBs";

        private static readonly string strSP_ADMIN_GetLnkPub = "SP_ADMIN_GetLnkPub";
        private static readonly string strSP_ADMIN_GetKitPub = "SP_ADMIN_GetKitPub";
       
        #endregion

        private static readonly string strSP_ADMIN_GetPubInfoByPubID = "SP_ADMIN_GetPubInfoByPubID";
        private static readonly string strSP_ADMIN_GetPubInfoByProdID = "SP_ADMIN_GetPubInfoByProdID";


        private static readonly string strSP_ADMIN_GetKioskConfByPubID = "SP_ADMIN_GetKioskConfByPubID";
        private static readonly string strSP_ADMIN_GetKioskConfRotateByPubID = "SP_ADMIN_GetKioskConfRotateByPubID";

        private static readonly string strSP_ADMIN_GetKioskConfViewByPubID = "SP_ADMIN_GetKioskConferenceViewByPubID";
        private static readonly string strSP_ADMIN_GetKioskConfRotateViewByPubID = "SP_ADMIN_GetKioskConferenceRotateViewByPubID";
        private static readonly string strSP_ADMIN_SP_ADMIN_SetRankByPubId = "SP_ADMIN_SetRankByPubId";

        private static readonly string strSP_ADMIN_GetAllFeaturedPusSeq = "SP_ADMIN_GetAllFeaturedPusSeq";
        private static readonly string strSP_ADMIN_SetAllFeaturedPusSeq = "SP_ADMIN_SetAllFeaturedPusSeq";

        /*Begin CR-36 (11-001-36) - Featured Pub Stacks*/
        #region Featured_Publication_Stacks
        //below used on Stacks set up page
        private static readonly string strSP_ADMIN_GetFeaturedStacks = "sp_ADMIN_GetFeaturedStacks";
        private static readonly string strSP_ADMIN_SetFeaturedStacks = "sp_ADMIN_SetFeaturedStacks";
        private static readonly string strSP_ADMIN_GetFeaturedStacksHistory = "sp_ADMIN_GetFeaturedStacksHistory";
        private static readonly string strSP_ADMIN_GetFeaturedStacksAccess = "sp_ADMIN_GetFeaturedStacksAccess";
        //below used on NCIPL Tab
        private static readonly string strSP_ADMIN_GetNCIPLStacksViewByPubId = "sp_ADMIN_GetFeaturedStacksByPubID";
        private static readonly string strSP_ADMIN_GetAllNCIPLStacks = "sp_ADMIN_GetFeaturedStacksByPubID";
        private static readonly string strSP_ADMIN_GetNCIPLStacksByPubId = "sp_ADMIN_GetFeaturedStacksByPubID";
        private static readonly string strSP_ADMIN_SetNCIPLStacksByPubId = "sp_ADMIN_SetFeaturedStacksByPubID";
        private static readonly string strSP_ADMIN_SetFeaturedStacksHistoryByPubId = "sp_ADMIN_SetFeaturedStacksHistoryByPubId";
        #endregion
        /*Begin CR-36*/

        #region Collections_(NCIPL_CC_Changes)
        //SP_ADMIN_GetAllSeries_ByInterface
        //SP_ADMIN_GetSeries_ByInterfaceByPubId
        //SP_ADMIN_SetSeries_ByInterfaceByPubId
        //For NCIPL Tab
        private static readonly string strSP_ADMIN_GetAllSeries_ByInterface = "SP_ADMIN_GetAllSeries_ByInterface";
        private static readonly string strSP_ADMIN_GetSeries_ByInterfaceByPubId = "SP_ADMIN_GetSeries_ByInterfaceByPubId";
        private static readonly string strSP_ADMIN_SetSeries_ByInterfaceByPubId = "SP_ADMIN_SetSeries_ByInterfaceByPubId";
        #endregion

        #region TypeOfCustomer(For ROO)
        private static readonly string strSP_ADMIN_GetAllTypeofCustomers = "SP_ADMIN_GetAllTypeofCustomers";
        private static readonly string strSP_ADMIN_SetTypeofCustomer = "SP_ADMIN_SetTypeofCustomer";
        #endregion

        #region OrderMedia (For ROO)
        private static readonly string strSP_ADMIN_GetAllOrderMedia = "SP_ADMIN_GetAllOrderMedia";
        private static readonly string strSP_ADMIN_SetOrderMedia = "SP_ADMIN_SetOrderMedia";
        #endregion

        #region Owner
        private static readonly string strSP_ADMIN_SP_ADMIN_GetAllOwners = "SP_ADMIN_GetAllOwners";
        private static readonly string strSP_ADMIN_SetOwnerByPubID = "SP_ADMIN_SetOwnerByPubID";
        private static readonly string strSP_ADMIN_GetOwnerStatusbyOwnerID = "SP_ADMIN_GetOwnerStatusbyOwnerID";
        private static readonly string strSP_ADMIN_DeleteOwnerByPubid = "SP_ADMIN_DeleteOwnerByPubid";
           
        #endregion

        #region Sponsor        
        private static readonly string strSP_ADMIN_SP_ADMIN_GetAllSponsors = "SP_ADMIN_GetAllSponsors";
        private static readonly string strSP_ADMIN_SetSponsorByPubID = "SP_ADMIN_SetSponsorByPubID";
        private static readonly string strSP_ADMIN_GetSponsorStatusbyOwnerID = "SP_ADMIN_GetSponsorStatusbyOwnerID";
        private static readonly string strSP_ADMIN_DeleteSponsorByPubid = "SP_ADMIN_DeleteSponsorByPubid";
        
        #endregion


        #region Cancer Type


        public static CancerTypeCollection GetAllCancerType(bool active)
        {
            CancerTypeCollection coll = new CancerTypeCollection();

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetAllCancerType);
            db.AddInParameter(dbCommand, "@Active", DbType.Int32, System.Convert.ToInt32(active));
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    CancerType newMultiSelectItem = new CancerType(
                        System.Convert.ToInt32(dr["CANCERTYPEID"].ToString()),
                        dr["DESCRIPTION"].ToString(),
                        System.Convert.ToBoolean(dr["ACTIVE_FLAG"])
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }

        public static CancerTypeCollection GetCancerTypeByPubID(int PubID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand(strSP_ADMIN_GetCancerByPubID);
            db.AddInParameter(cw, "@PubID", DbType.String, PubID);
            using (System.Data.IDataReader dr = ExecuteReader(db, cw))
            {
                CancerTypeCollection coll = new CancerTypeCollection();
                while (dr.Read())
                {
                    CancerType newCancerType = new CancerType(Convert.ToInt32(dr["CANCERTYPEID"].ToString()),
                        dr["DESCRIPTION"].ToString(),
                        Convert.ToBoolean(dr["ACTIVE_FLAG"]));
                    coll.Add(newCancerType);
                }
                return (coll);
            }
        }
        
        public static bool SetCancerTypeByPubID(int PubID, string l, char delim)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_SetCancerByPubID);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);
            db.AddInParameter(dbCommand, "@Params", DbType.AnsiString, l);
            db.AddInParameter(dbCommand, "@delim", DbType.AnsiString, delim);

            
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    Int32 pk = db.ExecuteNonQuery(dbCommand);
                    transaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    transaction.Rollback();
                    return (false);
                }
            }
            
        }
        
        #endregion

        #region Audience
        /// <summary>
        /// GetAllAudience()
        /// </summary>
        /// <returns>AudienceCollection</returns>
        public static AudienceCollection GetAllAudience(bool active)
        {
            AudienceCollection coll = new AudienceCollection();

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetAllAudience);
            db.AddInParameter(dbCommand, "@Active", DbType.Int32, System.Convert.ToInt32(active));
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))            
            {
                while (dr.Read())
                {
                    Audience newMultiSelectItem = new Audience(
                        System.Convert.ToInt32(dr["AUDIENCEID"].ToString()),
                        dr["DESCRIPTION"].ToString(),
                        System.Convert.ToBoolean(dr["ACTIVE_FLAG"])
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }

            //DataSet ds = new DataSet();
            //ds.ReadXml(HttpContext.Current.Server.MapPath("Xml/LkAud.xml"));

            //foreach (DataRow dr in ds.Tables[0].Rows)
            //{
                //MultiSelectListBoxItem newMultiSelectItem = new MultiSelectListBoxItem(System.Convert.ToInt32(dr.ItemArray[1]), 
                //    System.Convert.ToString(dr.ItemArray[0])
                //    //,
                //    //System.Convert.ToBoolean(System.Convert.ToInt32(dr.ItemArray[2]))
                //    );
                //coll.Add(newMultiSelectItem);
            //}
            //return (coll);
        }

        public static AudienceCollection GetAudienceByPubID(int PubID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand(strSP_ADMIN_GetAudienceByPubID);
            db.AddInParameter(cw, "@PubID", DbType.String, PubID);
            using (System.Data.IDataReader dr = ExecuteReader(db, cw))
            {
                AudienceCollection coll = new AudienceCollection();
                while (dr.Read())
                {
                    Audience newCancerType = new Audience(Convert.ToInt32(dr["AUDIENCEID"].ToString()),
                        dr["DESCRIPTION"].ToString(),
                        Convert.ToBoolean(dr["ACTIVE_FLAG"]));
                    coll.Add(newCancerType);
                }
                return (coll);
            }
        }

        public static bool SetAudienceByPubID(int PubID, string l, char delim)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_SetAudienceByPubID);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);
            db.AddInParameter(dbCommand, "@Params", DbType.AnsiString, l);
            db.AddInParameter(dbCommand, "@delim", DbType.AnsiString, delim);

            
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    Int32 pk = db.ExecuteNonQuery(dbCommand);
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    transaction.Rollback();
                    return (false);
                }
            }
            
        }

        
        #endregion

        #region Language

        public static LangCollection GetLangByPubID(int PubID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand(strSP_ADMIN_GetLangByPubID);
            db.AddInParameter(cw, "@PubID", DbType.String, PubID);
            using (System.Data.IDataReader dr = ExecuteReader(db, cw))
            {
                LangCollection coll = new LangCollection();
                while (dr.Read())
                {
                    Lang newCancerType = new Lang(Convert.ToInt32(dr["LANGUAGEID"].ToString()),
                        dr["DESCRIPTION"].ToString(),
                        Convert.ToBoolean(dr["ACTIVE_FLAG"]));
                    coll.Add(newCancerType);
                }
                return (coll);
            }
        }

        public static LangCollection GetAllLang(bool active)
        {
            LangCollection coll = new LangCollection();

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetAllLang);
            db.AddInParameter(dbCommand, "@Active", DbType.Int32, System.Convert.ToInt32(active));
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    Lang newMultiSelectItem = new Lang(
                        System.Convert.ToInt32(dr["LANGUAGEID"].ToString()),
                        dr["DESCRIPTION"].ToString(),
                        System.Convert.ToBoolean(dr["ACTIVE_FLAG"])
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }

        public static bool SetLangByPubID(int PubID, string l, char delim)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_SetLangByPubID);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);
            db.AddInParameter(dbCommand, "@Params", DbType.AnsiString, l);
            db.AddInParameter(dbCommand, "@delim", DbType.AnsiString, delim);

            
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    Int32 pk = db.ExecuteNonQuery(dbCommand);
                    transaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    transaction.Rollback();
                    return (false);
                }
            }
            
        }
        
        #endregion

        #region Prod Format

        public static ProdFormatCollection GetAllProdFormat(bool active)
        {
            ProdFormatCollection coll = new ProdFormatCollection();

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetAllProdFormat);
            db.AddInParameter(dbCommand, "@Active", DbType.Int32, System.Convert.ToInt32(active));
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    ProdFormat newMultiSelectItem = new ProdFormat(
                        System.Convert.ToInt32(dr["FORMATID"].ToString()),
                        dr["DESCRIPTION"].ToString(),
                        System.Convert.ToBoolean(dr["ACTIVE_FLAG"])
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }

        public static ProdFormatCollection GetProdFormatByPubID(int PubID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand(strSP_ADMIN_GetProdFormatByPubID);
            db.AddInParameter(cw, "@PubID", DbType.String, PubID);
            using (System.Data.IDataReader dr = ExecuteReader(db, cw))
            {
                ProdFormatCollection coll = new ProdFormatCollection();
                while (dr.Read())
                {
                    ProdFormat newCancerType = new ProdFormat(Convert.ToInt32(dr["FORMATID"].ToString()),
                        dr["DESCRIPTION"].ToString(),
                        Convert.ToBoolean(dr["ACTIVE_FLAG"]));
                    coll.Add(newCancerType);
                }
                return (coll);
            }
        }

        public static bool SetProdFormatByPubID(int PubID, string l, char delim)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_SetProdFormatByPubID);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);
            db.AddInParameter(dbCommand, "@Params", DbType.AnsiString, l);
            db.AddInParameter(dbCommand, "@delim", DbType.AnsiString, delim);

            
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    Int32 pk = db.ExecuteNonQuery(dbCommand);
                    transaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    transaction.Rollback();
                    return (false);
                }
            }
            
        }
        
        #endregion

        #region Series

        public static SeriesCollection GetAllSeries(bool active)
        {
            SeriesCollection coll = new SeriesCollection();

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetAllSeries);
            db.AddInParameter(dbCommand, "@Active", DbType.Int32, System.Convert.ToInt32(active));
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    Series newMultiSelectItem = new Series(
                        System.Convert.ToInt32(dr["SERIESID"].ToString()),
                        dr["DESCRIPTION"].ToString(),
                        System.Convert.ToBoolean(dr["ACTIVE_FLAG"])
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }

        public static SeriesCollection GetSeriesByPubID(int PubID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand(strSP_ADMIN_GetSeriesByPubID);
            db.AddInParameter(cw, "@PubID", DbType.String, PubID);
            using (System.Data.IDataReader dr = ExecuteReader(db, cw))
            {
                SeriesCollection coll = new SeriesCollection();
                while (dr.Read())
                {
                    Series newCancerType = new Series(Convert.ToInt32(dr["SERIESID"].ToString()),
                        dr["DESCRIPTION"].ToString(),
                        Convert.ToBoolean(dr["ACTIVE_FLAG"]));
                    coll.Add(newCancerType);
                }
                return (coll);
            }
        }

        public static bool SetSeriesByPubID(int PubID, string l, char delim)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_SetSeriesByPubID);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);
            db.AddInParameter(dbCommand, "@Params", DbType.AnsiString, l);
            db.AddInParameter(dbCommand, "@delim", DbType.AnsiString, delim);

            
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    Int32 pk = db.ExecuteNonQuery(dbCommand);
                    transaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    transaction.Rollback();
                    return (false);
                }
            }
            
        }

        #endregion

        #region Race

        public static RaceCollection GetAllRace(bool active)
        {
            RaceCollection coll = new RaceCollection();

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetAllRace);
            db.AddInParameter(dbCommand, "@Active", DbType.Int32, System.Convert.ToInt32(active));
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    Race newMultiSelectItem = new Race(
                        System.Convert.ToInt32(dr["RACEID"].ToString()),
                        dr["DESCRIPTION"].ToString(),
                        System.Convert.ToBoolean(dr["ACTIVE_FLAG"])
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }

        public static RaceCollection GetRaceByPubID(int PubID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand(strSP_ADMIN_GetRaceByPubID);
            db.AddInParameter(cw, "@PubID", DbType.String, PubID);
            using (System.Data.IDataReader dr = ExecuteReader(db, cw))
            {
                RaceCollection coll = new RaceCollection();
                while (dr.Read())
                {
                    Race newCancerType = new Race(Convert.ToInt32(dr["RACEID"].ToString()),
                        dr["DESCRIPTION"].ToString(),
                        Convert.ToBoolean(dr["ACTIVE_FLAG"]));
                    coll.Add(newCancerType);
                }
                return (coll);
            }
        }

        public static bool SetRaceByPubID(int PubID, string l, char delim)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_SetRaceByPubID);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);
            db.AddInParameter(dbCommand, "@Params", DbType.AnsiString, l);
            db.AddInParameter(dbCommand, "@delim", DbType.AnsiString, delim);

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    Int32 pk = db.ExecuteNonQuery(dbCommand);
                    transaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    transaction.Rollback();
                    return (false);
                }
            }
            
        }
        #endregion

        #region Book Status

        public static BookstatusCollection GetAllBookstatus(bool active)
        {
            BookstatusCollection coll = new BookstatusCollection();

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetAllBookstatus);
            db.AddInParameter(dbCommand, "@Active", DbType.Int32, System.Convert.ToInt32(active));
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    Bookstatus newMultiSelectItem = new Bookstatus(
                        System.Convert.ToInt32(dr["STATUSID"].ToString()),
                        dr["DESCRIPTION"].ToString(),
                        System.Convert.ToBoolean(dr["ACTIVE_FLAG"])
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }

        #endregion

        #region Reading Level

        public static ReadlevelCollection GetAllReadinglevel(bool active)
        {
            ReadlevelCollection coll = new ReadlevelCollection();

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetAllReadinglevel);
            db.AddInParameter(dbCommand, "@Active", DbType.Int32, System.Convert.ToInt32(active));
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    Readlevel newMultiSelectItem = new Readlevel(
                        System.Convert.ToInt32(dr["READLEVELID"].ToString()),
                        dr["DESCRIPTION"].ToString(),
                        System.Convert.ToBoolean(dr["ACTIVE_FLAG"])
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }

        public static ReadlevelCollection GetReadlevelByPubID(int PubID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand(strSP_ADMIN_GetReadlevelByPubID);
            db.AddInParameter(cw, "@PubID", DbType.String, PubID);
            using (System.Data.IDataReader dr = ExecuteReader(db, cw))
            {
                ReadlevelCollection coll = new ReadlevelCollection();
                while (dr.Read())
                {
                    Readlevel newCancerType = new Readlevel(Convert.ToInt32(dr["READLEVELID"].ToString()),
                        dr["DESCRIPTION"].ToString(),
                        Convert.ToBoolean(dr["ACTIVE_FLAG"]));
                    coll.Add(newCancerType);
                }
                return (coll);
            }
        }
        
        public static bool SetReadlevelByPubID(int PubID, int l)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_SetReadlevelByPubID);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);
            db.AddInParameter(dbCommand, "@Params", DbType.Int32, l);

            if (l != 0)
            {
                using (DbConnection connection = db.CreateConnection())
                {
                    connection.Open();
                    DbTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        Int32 pk = db.ExecuteNonQuery(dbCommand);
                        transaction.Commit();

                        return true;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                        transaction.Rollback();
                        return (false);
                    }
                }
            }
            else
                return true;
        }

        
        #endregion

        #region Subject

        public static SubjectCollection GetAllSubject(bool active)
        {
            SubjectCollection coll = new SubjectCollection();

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetAllSubject);
            db.AddInParameter(dbCommand, "@Active", DbType.Int32, System.Convert.ToInt32(active));
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    Subject newMultiSelectItem = new Subject(
                        System.Convert.ToBoolean(dr["CannotRem"]),
                        System.Convert.ToInt32(dr["SUBJECTID"].ToString()),
                        dr["DESCRIPTION"].ToString(),
                        System.Convert.ToBoolean(dr["NCIPL"]),
                        System.Convert.ToBoolean(dr["ROO"]),
                        System.Convert.ToBoolean(dr["Exhibit"]),
                        System.Convert.ToBoolean(dr["CATALOG"]),
                        System.Convert.ToBoolean(dr["HASSUBCATEGORIES_FLAG"]),
                        System.Convert.ToBoolean(dr["ACTIVE_FLAG"])
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }

        public static SubjectCollection GetAllNCIPLSubject(bool active)
        {
            SubjectCollection coll = new SubjectCollection();

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetAllNCIPLSubject);
            db.AddInParameter(dbCommand, "@Active", DbType.Int32, System.Convert.ToInt32(active));
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    Subject newMultiSelectItem = new Subject(
                        System.Convert.ToInt32(dr["SUBJECTID"].ToString()),
                        dr["DESCRIPTION"].ToString(),
                        System.Convert.ToBoolean(dr["ACTIVE_FLAG"])
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }

        

        public static SubjectCollection GetNCIPLSubjectByPubID(int PubID )
        {
            SubjectCollection coll = new SubjectCollection();

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetNCIPLSubjectByPubID);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, System.Convert.ToInt32(PubID));
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    Subject newMultiSelectItem = new Subject(
                        System.Convert.ToInt32(dr["SUBJECTID"].ToString()),
                        dr["DESCRIPTION"].ToString(),
                        System.Convert.ToBoolean(dr["ACTIVE_FLAG"])
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }
        
        public static SubjectCollection GetROOSubjectByPubID(int PubID )
        {
            SubjectCollection coll = new SubjectCollection();

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetROOSubjectByPubID);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, System.Convert.ToInt32(PubID));
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    Subject newMultiSelectItem = new Subject(
                        System.Convert.ToInt32(dr["SUBJECTID"].ToString()),
                        dr["DESCRIPTION"].ToString(),
                        System.Convert.ToBoolean(dr["ACTIVE_FLAG"])
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }
        
        public static SubjectCollection GetAllROOSubject(bool active)
        {
            SubjectCollection coll = new SubjectCollection();

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetAllROOSubject);
            db.AddInParameter(dbCommand, "@Active", DbType.Int32, System.Convert.ToInt32(active));
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    Subject newMultiSelectItem = new Subject(
                        System.Convert.ToInt32(dr["SUBJECTID"].ToString()),
                        dr["DESCRIPTION"].ToString(),
                        System.Convert.ToBoolean(dr["ACTIVE_FLAG"])
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }
        /// <summary>
        /// For populate the list for search
        /// </summary>
        /// <returns></returns>
        
        public static SubjectCollection GetAllROOUsedSubject()
        {
            SubjectCollection coll = new SubjectCollection();

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetAllROOUsedSubject);
            //db.AddInParameter(dbCommand, "@Active", DbType.Int32, System.Convert.ToInt32(active));
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    Subject newMultiSelectItem = new Subject(
                        System.Convert.ToInt32(dr["SUBJECTID"].ToString()),
                        dr["DESCRIPTION"].ToString(),
                        System.Convert.ToBoolean(dr["ACTIVE_FLAG"])
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }

        public static List<Subject> GetAllCatalogSubject(bool active)
        {
            List<Subject> coll = new List<Subject>();

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetAllCatalogSubject);
            db.AddInParameter(dbCommand, "@Active", DbType.Int32, System.Convert.ToInt32(active));
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    Subject newMultiSelectItem = new Subject(
                        System.Convert.ToInt32(dr["SUBJECTID"].ToString()),
                        dr["DESCRIPTION"].ToString(),
                        System.Convert.ToBoolean(dr["NCIPL"]),
                        System.Convert.ToBoolean(dr["ROO"]),
                        System.Convert.ToBoolean(dr["Exhibit"]),
                        System.Convert.ToBoolean(dr["CATALOG"]),
                        System.Convert.ToBoolean(dr["HASSUBCATEGORIES_FLAG"]),
                        System.Convert.ToInt32(dr["SORT_SEQ"]),
                        System.Convert.ToBoolean(dr["ACTIVE_FLAG"])
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }

        public static SubjectCollection GetAllCatalogWYNTKSubjectForNew(bool active)
        {
            SubjectCollection coll = new SubjectCollection();

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetAllCatalogWYNTKSubjectForNew);
            db.AddInParameter(dbCommand, "@Active", DbType.Int32, System.Convert.ToInt32(active));
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    Subject newMultiSelectItem = new Subject(
                        System.Convert.ToInt32(dr["CANCERTYPEID"].ToString()),
                        dr["DESCRIPTION"].ToString(),
                        System.Convert.ToBoolean(dr["ACTIVE_FLAG"])
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }

        public static SubjectCollection GetAllCatalogWYNTKSubjectForExist(int PubID, bool active)
        {
            SubjectCollection coll = new SubjectCollection();

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetAllCatalogWYNTKSubjectForExist);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);
            db.AddInParameter(dbCommand, "@Active", DbType.Int32, System.Convert.ToInt32(active));
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    Subject newMultiSelectItem = new Subject(
                        System.Convert.ToInt32(dr["CANCERTYPEID"].ToString()),
                        dr["DESCRIPTION"].ToString(),
                        System.Convert.ToBoolean(dr["ACTIVE_FLAG"])
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }

        public static bool SetSubject(ref int SubjectID, string DESCRIPTION,
            int NCIPL, int ROO, int EXH, int CATALOG, int HASSUBCATEGORIES_FLAG)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_SetSubject);

            db.AddParameter(dbCommand, "@SUBJECTID", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, SubjectID);

            if (DESCRIPTION != null)
                db.AddInParameter(dbCommand, "@DESCRIPTION", DbType.String, DESCRIPTION);
            else
                db.AddInParameter(dbCommand, "@DESCRIPTION", DbType.String, DBNull.Value);

            if (NCIPL == 1)
                db.AddInParameter(dbCommand, "@NCIPL", DbType.Int32, NCIPL);
            else
                db.AddInParameter(dbCommand, "@NCIPL", DbType.Int32, DBNull.Value);

            if (ROO == 1)
                db.AddInParameter(dbCommand, "@ROO", DbType.Int32, ROO);
            else
                db.AddInParameter(dbCommand, "@ROO", DbType.Int32, DBNull.Value);

            if (EXH == 1)
                db.AddInParameter(dbCommand, "@EXH", DbType.Int32, EXH);
            else
                db.AddInParameter(dbCommand, "@EXH", DbType.Int32, DBNull.Value);

            if (CATALOG == 1)
                db.AddInParameter(dbCommand, "@CATALOG", DbType.Int32, CATALOG);
            else
                db.AddInParameter(dbCommand, "@CATALOG", DbType.Int32, DBNull.Value);

            if (HASSUBCATEGORIES_FLAG == 1)
                db.AddInParameter(dbCommand, "@HASSUBCATEGORIES_FLAG", DbType.Int32, HASSUBCATEGORIES_FLAG);
            else
                db.AddInParameter(dbCommand, "@HASSUBCATEGORIES_FLAG", DbType.Int32, DBNull.Value);

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    Int32 pk = db.ExecuteNonQuery(dbCommand);
                    transaction.Commit();
                    SubjectID = (int)db.GetParameterValue(dbCommand, "@SubjectID");

                    if (pk != -1)
                        return true;
                    else
                        return false;
                    
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    transaction.Rollback();
                    return (false);
                }
            }
        }

        public static bool DeleteSubject(int SubjectID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_DeleteSubjectBySubjID);
            db.AddInParameter(dbCommand, "@SUBJECTID", DbType.Int32, SubjectID);

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    Int32 pk = db.ExecuteNonQuery(dbCommand);
                    transaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    transaction.Rollback();
                    return (false);
                }
            }

        }

        public static bool EnableSubject(int SubjectID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_EnableSubjectBySubjID);
            db.AddInParameter(dbCommand, "@SUBJECTID", DbType.Int32, SubjectID);

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    Int32 pk = db.ExecuteNonQuery(dbCommand);
                    transaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    transaction.Rollback();
                    return (false);
                }
            }

        }
        
        
        #endregion
       

        #region Subcategory

        public static List<Subject> GetAllSubjectHvSubcat(bool active)
        {
            List<Subject> coll = new List<Subject>();

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetAllSubjectHvSubcat);
            db.AddInParameter(dbCommand, "@Active", DbType.Int32, System.Convert.ToInt32(active));
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    Subject newMultiSelectItem = new Subject(
                        System.Convert.ToInt32(dr["SUBJECTID"].ToString()),
                        dr["DESCRIPTION"].ToString(),
                        System.Convert.ToBoolean(dr["NCIPL"]),
                        System.Convert.ToBoolean(dr["ROO"]),
                        System.Convert.ToBoolean(dr["Exhibit"]),
                        System.Convert.ToBoolean(dr["CATALOG"]),
                        System.Convert.ToBoolean(dr["HASSUBCATEGORIES_FLAG"]),
                        System.Convert.ToInt32(dr["SORT_SEQ"]),
                        System.Convert.ToBoolean(dr["ACTIVE_FLAG"])
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }

        public static List<SubCat> GetSubCatBySubID(int SubjID)
        {
            List<SubCat> coll = new List<SubCat>();

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetSubCatalogSubjectBySubjID);
            db.AddInParameter(dbCommand, "@SubjID", DbType.Int32, SubjID);
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    SubCat newMultiSelectItem = new SubCat(
                        System.Convert.ToInt32(dr["SUBCATEGORYID"].ToString()),
                        dr["DESCRIPTION"].ToString(),
                        System.Convert.ToInt32(dr["FK_SUBJECTCODE"]),
                        System.Convert.ToBoolean(dr["HASSUBSUBCATEGORIES_FLAG"])
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }

        public static List<SubCat> GetSubCatBySubCatID(int SubCatID)
        {
            List<SubCat> coll = new List<SubCat>();

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetSubCatalogSubjectBySubCatalogID);
            db.AddInParameter(dbCommand, "@SubCatalogID", DbType.Int32, SubCatID);
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    SubCat newMultiSelectItem = new SubCat(
                        System.Convert.ToInt32(dr["SUBCATEGORYID"].ToString()),
                        dr["DESCRIPTION"].ToString(),
                        System.Convert.ToInt32(dr["FK_SUBJECTCODE"]),
                        System.Convert.ToBoolean(dr["HASSUBSUBCATEGORIES_FLAG"]),
                        System.Convert.ToInt32(dr["HvNumSubSub"])
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }

        public static SubCat GetSubCatBySubCatID2(int SubCatID)
        {
            SubCat newMultiSelectItem = null;

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetSubCatalogSubjectBySubCatalogID2);
            db.AddInParameter(dbCommand, "@SubCatalogID", DbType.Int32, SubCatID);
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    newMultiSelectItem = new SubCat(
                        System.Convert.ToInt32(dr["SUBCATEGORYID"].ToString()),
                        dr["DESCRIPTION"].ToString(),
                        System.Convert.ToInt32(dr["SubjectID"]),
                        dr["SubjectName"].ToString());
                }
                return (newMultiSelectItem);
            }
        }

        public static bool SetSubCat(ref int SUBCATEGORYID, string DESCRIPTION,
            int FK_SUBJECTCODE, int HASSUBSUBCATEGORIES_FLAG, int ACTIVE_FLAG)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_SetSubCat);

            db.AddParameter(dbCommand, "@SUBCATEGORYID", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, SUBCATEGORYID);

            if (DESCRIPTION != null)
                db.AddInParameter(dbCommand, "@DESCRIPTION", DbType.String, DESCRIPTION);
            else
                db.AddInParameter(dbCommand, "@DESCRIPTION", DbType.String, DBNull.Value);

            if (FK_SUBJECTCODE != -1)
                db.AddInParameter(dbCommand, "@FK_SUBJECTCODE", DbType.Int32, FK_SUBJECTCODE);
            else
                db.AddInParameter(dbCommand, "@FK_SUBJECTCODE", DbType.Int32, DBNull.Value);

            if (HASSUBSUBCATEGORIES_FLAG != -1)
                db.AddInParameter(dbCommand, "@HASSUBSUBCATEGORIES_FLAG", DbType.Int32, HASSUBSUBCATEGORIES_FLAG);
            else
                db.AddInParameter(dbCommand, "@HASSUBSUBCATEGORIES_FLAG", DbType.Int32, DBNull.Value);

            db.AddInParameter(dbCommand, "@ACTIVE_FLAG", DbType.Int32, ACTIVE_FLAG);

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    Int32 pk = db.ExecuteNonQuery(dbCommand);
                    transaction.Commit();
                    SUBCATEGORYID = (int)db.GetParameterValue(dbCommand, "@SUBCATEGORYID");
                    if (pk != -1)
                        return true;
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    transaction.Rollback();
                    return (false);
                }
            }
        }
        
        #endregion

        #region SubSubCategory
        public static List<SubSubCat> GetSubSubCatBySubID(int SubCatID)
        {
            List<SubSubCat> coll = new List<SubSubCat>();

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetSubSubCatalogSubjectBySubCatID);
            db.AddInParameter(dbCommand, "@SubCatID", DbType.Int32, SubCatID);
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    SubSubCat newMultiSelectItem = new SubSubCat(
                        System.Convert.ToInt32(dr["SUBSUBCATEGORYID"].ToString()),
                        dr["DESCRIPTION"].ToString(),
                        System.Convert.ToInt32(dr["FK_SUBCATEGORYID"])
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }

        public static List<SubSubCat> GetAllSubSubCat(bool active)
        {
            List<SubSubCat> coll = new List<SubSubCat>();

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetAllSubSubCategory);
            db.AddInParameter(dbCommand, "@Active", DbType.Int32, active);
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    SubSubCat newMultiSelectItem = new SubSubCat(
                        System.Convert.ToInt32(dr["SUBSUBCATEGORYID"].ToString()),
                        dr["DESCRIPTION"].ToString(),
                        System.Convert.ToInt32(dr["FK_SUBCATEGORYID"])
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }

        public static List<SubSubCat> GetSubSubCatalogSubjectBySubSubCatalogID(int SubSubCatID)
        {
            List<SubSubCat> coll = new List<SubSubCat>();

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetSubSubCatalogSubjectBySubSubCatalogID);
            db.AddInParameter(dbCommand, "@SubSubCatalogID", DbType.Int32, SubSubCatID);
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    SubSubCat newMultiSelectItem = new SubSubCat(
                        System.Convert.ToInt32(dr["SUBSUBCATEGORYID"].ToString()),
                        dr["SUBSUBCATEGORYDESC"].ToString(),
                        System.Convert.ToInt32(dr["SUBCATEGORYID"]),
                        dr["SUBCATEGORYDESC"].ToString(),
                        System.Convert.ToInt32(dr["SUBJECTID"]),
                        dr["SUBDESC"].ToString()
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }

        public static bool SetSubSubCat(ref int SUBSUBCATEGORYID, string DESCRIPTION,
            int FK_SUBCATEGORYID, int ACTIVE_FLAG)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_SetSubSubCat);

            db.AddParameter(dbCommand, "@SUBSUBCATEGORYID", DbType.Int32,
                ParameterDirection.InputOutput, null, DataRowVersion.Default, SUBSUBCATEGORYID);

            if (DESCRIPTION != null)
                db.AddInParameter(dbCommand, "@DESCRIPTION", DbType.String, DESCRIPTION);
            else
                db.AddInParameter(dbCommand, "@DESCRIPTION", DbType.String, DBNull.Value);

            if (FK_SUBCATEGORYID != -1)
                db.AddInParameter(dbCommand, "@FK_SUBCATEGORYID", DbType.Int32, FK_SUBCATEGORYID);
            else
                db.AddInParameter(dbCommand, "@FK_SUBCATEGORYID", DbType.Int32, DBNull.Value);

            db.AddInParameter(dbCommand, "@ACTIVE_FLAG", DbType.Int32, ACTIVE_FLAG);

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    Int32 pk = db.ExecuteNonQuery(dbCommand);
                    transaction.Commit();
                    SUBSUBCATEGORYID = (int)db.GetParameterValue(dbCommand, "@SUBSUBCATEGORYID");
                    if (pk != -1)
                        return true;
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    transaction.Rollback();
                    return (false);
                }
            }
        }
        
        #endregion

        #region Catalog Seq
        /// <summary>
        /// Populate catalog subjects which have subcategory
        /// </summary>
        public static List<Subject> GetAllCatalogSubjectHvSubcat(bool active)
        {
            List<Subject> coll = new List<Subject>();

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetAllCatalogSubjectHvSubcat);
            db.AddInParameter(dbCommand, "@Active", DbType.Int32, System.Convert.ToInt32(active));
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    Subject newMultiSelectItem = new Subject(
                        System.Convert.ToInt32(dr["SUBJECTID"].ToString()),
                        dr["DESCRIPTION"].ToString(),
                        System.Convert.ToBoolean(dr["NCIPL"]),
                        System.Convert.ToBoolean(dr["ROO"]),
                        System.Convert.ToBoolean(dr["Exhibit"]),
                        System.Convert.ToBoolean(dr["CATALOG"]),
                        System.Convert.ToBoolean(dr["HASSUBCATEGORIES_FLAG"]),
                        System.Convert.ToInt32(dr["SORT_SEQ"]),
                        System.Convert.ToBoolean(dr["ACTIVE_FLAG"])
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }

        /// <summary>
        /// Populate sub category
        /// </summary>
        public static List<SubCat> GetAllCatalogSubcatHvSubSubcat(bool active)
        {
            List<SubCat> coll = new List<SubCat>();

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetAllCatalogSubcatHvSubSubcat);
            db.AddInParameter(dbCommand, "@Active", DbType.Int32, System.Convert.ToInt32(active));
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    SubCat newMultiSelectItem = new SubCat(
                        System.Convert.ToInt32(dr["SUBCATEGORYID"].ToString()),
                        dr["DESCRIPTION"].ToString(),
                        System.Convert.ToInt32(dr["FK_SUBJECTCODE"]),
                        System.Convert.ToBoolean(dr["HASSUBSUBCATEGORIES_FLAG"])
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }

        /// <summary>
        /// Populate Featured pubs seq
        /// </summary>
        public static List<Seq> GetAllFeaturedPubsSeq(string SortBy,bool SortAsc)
        {
            List<Seq> coll = new List<Seq>();

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetAllFeaturedPusSeq);
            db.AddInParameter(dbCommand, "@SortBy", DbType.String, SortBy);
            db.AddInParameter(dbCommand, "@SortAsc", DbType.String, SortAsc ? "ASC" : "DESC");
            //db.AddInParameter(dbCommand, "@Active", DbType.Int32, System.Convert.ToInt32(active));
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    Seq newMultiSelectItem = new Seq(
                        System.Convert.ToInt32(dr["PUBID"].ToString()),
                        dr["PRODUCTID"].ToString(),
                        System.Convert.ToInt32(dr["NCIPLFEATURED_SEQUENCE"])
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }

        public static bool SetAllFeaturedPubsSeq(string id_seqs, string pairDelim,
            string indDelim)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_SetAllFeaturedPusSeq);
            db.AddInParameter(dbCommand, "@id_seqs", DbType.String, id_seqs);
            db.AddInParameter(dbCommand, "@pairDelim", DbType.String, pairDelim);
            db.AddInParameter(dbCommand, "@indDelim", DbType.String, indDelim);

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    Int32 pk = db.ExecuteNonQuery(dbCommand);
                    transaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    transaction.Rollback();
                    return (false);
                }
            }
        }

        /*Begin CR-36*/

        #region ForFeaturedSetUpPage
        public static StackCollection GetFeaturedStacks(int Active, string Title)
        {
            StackCollection Coll = new StackCollection();
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetFeaturedStacks);

            db.AddInParameter(dbCommand, "@searchactive", DbType.Int32, Active);
            db.AddInParameter(dbCommand, "@searchtitle", DbType.String, Title);

            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    Stack newStack = new Stack(
                        dr.GetInt32(dr.GetOrdinal("stackid")),
                        (dr["stackprodids"] == DBNull.Value) ? "" : dr["stackprodids"].ToString(),
                        dr["stacktitle"].ToString(),
                        dr.GetInt32(dr.GetOrdinal("stackactive")),
                        dr.GetInt32(dr.GetOrdinal("stacksequence"))
                    );
                    Coll.Add(newStack);
                }
            }
            
            return Coll;
        }

        public static bool SetFeaturedStacks(string Flag, string StackIds, 
                                             string Titles, string Sequences, string Delim)
        {
            
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_SetFeaturedStacks);

            db.AddInParameter(dbCommand, "@flag", DbType.String, Flag);
            db.AddInParameter(dbCommand, "@stackids", DbType.String, StackIds);
            db.AddInParameter(dbCommand, "@titles", DbType.String, Titles);
            db.AddInParameter(dbCommand, "@sequences", DbType.String, Sequences);
            db.AddInParameter(dbCommand, "@delim", DbType.String, Delim);

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    Int32 pk = db.ExecuteNonQuery(dbCommand);
                    transaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    transaction.Rollback();
                    return false;
                }
            }
        }

        //Returns the Stack History 3 level collection
        public static StackSuperHistRecCollection GetFeaturedStacksHistory(DateTime StartDt, DateTime EndDt)
        {
            StackSuperHistRecCollection SuperHistColl = new StackSuperHistRecCollection();

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetFeaturedStacksHistory);

            if (StartDt != DateTime.MinValue && EndDt != DateTime.MinValue)
            {
                db.AddInParameter(dbCommand, "@sdate", DbType.DateTime, StartDt);
                db.AddInParameter(dbCommand, "@edate", DbType.DateTime, EndDt);
            }

            StackHistRecCollection newHistColl = new StackHistRecCollection();
            int refid = -99;
            
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    refid = dr.GetInt32(dr.GetOrdinal("Id"));
                    if (newHistColl.Id == refid || newHistColl.Count == 0)
                    {
                        //do nothing
                    }
                    else
                    {
                        SuperHistColl.Add(newHistColl); //add existing
                        newHistColl = null;
                        newHistColl = new StackHistRecCollection();
                    }
                    
                    StackHistRec newHistRec = new StackHistRec(
                        dr.GetInt32(dr.GetOrdinal("Id")),
                        dr.GetInt32(dr.GetOrdinal("StackId")),
                        dr["Title"].ToString(),
                        dr.GetInt32(dr.GetOrdinal("pubid")),
                        (dr["productid"] == DBNull.Value) ? "" : dr["productid"].ToString(),
                        (dr["longtitle"] == DBNull.Value) ? "" : dr["longtitle"].ToString(),
                        (dr["StackStartDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)dr["StackStartDate"],
                        (dr["StackEndDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)dr["StackEndDate"],
                        (dr["PubStartDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)dr["PubStartDate"],
                        (dr["PubEndDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)dr["PubEndDate"]
                    );

                    newHistColl.Id = newHistRec.RefId;
                    newHistColl.StackId = newHistRec.StackId;
                    newHistColl.StackTitle = newHistRec.StackTitle;
                    newHistColl.StackStartDate = newHistRec.StackStartDate;
                    newHistColl.StackEndDate = newHistRec.StackEndDate;
                    newHistColl.Add(newHistRec);
               
                }
            }

            if (newHistColl.Count > 0) //the last one remains
                SuperHistColl.Add(newHistColl);
            
            return SuperHistColl;
        }

        public static IDataReader GetFeaturedStacksAccess(DateTime StartDt, DateTime EndDt)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetFeaturedStacksAccess);

            if (StartDt != DateTime.MinValue && EndDt != DateTime.MinValue)
            {
                db.AddInParameter(dbCommand, "@sdate", DbType.DateTime, StartDt);
                db.AddInParameter(dbCommand, "@edate", DbType.DateTime, EndDt);
            }
         
            return db.ExecuteReader(dbCommand);
        }

        #endregion

        #region ForNCIPLTab
        //Get all stacks irrespective of active status, presense of assigned pubs
        //public static StackCollection GetAllNCIPLStacks()
        public static MultiSelectListBoxItemCollection GetAllNCIPLStacks()
        {
            //StackCollection Coll = new StackCollection();
            MultiSelectListBoxItemCollection collList = new MultiSelectListBoxItemCollection();

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetAllNCIPLStacks);

            //db.AddInParameter(dbCommand, "@pubid", DbType.Int32, -1);

            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    Stack newStack = new Stack(
                        dr.GetInt32(dr.GetOrdinal("stackid")),
                        (dr["stackprodids"] == DBNull.Value) ? "" : dr["stackprodids"].ToString(),
                        dr["stacktitle"].ToString(),
                        dr.GetInt32(dr.GetOrdinal("stackactive")),
                        dr.GetInt32(dr.GetOrdinal("stacksequence"))
                    );
                    //Coll.Add(newStack);
                    MultiSelectListBoxItem Item = new MultiSelectListBoxItem(
                            newStack.StackId, 
                            newStack.StackTitle
                    );
                    collList.Add(Item);
                    
                    newStack = null;
                    Item = null;
                }
            }

            //return Coll;
            return collList;
        }

        //Get the stacks only assigned to the particular pub, irrespective of active status
        //public static StackCollection GetNCIPLStacksByPubId(int pubid)
        public static MultiSelectListBoxItemCollection GetNCIPLStacksByPubId(int pubid)
        {
            //StackCollection Coll = new StackCollection();
            MultiSelectListBoxItemCollection collList = new MultiSelectListBoxItemCollection();
            
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetNCIPLStacksByPubId);

            db.AddInParameter(dbCommand, "@pubid", DbType.Int32, pubid);

            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    Stack newStack = new Stack(
                        dr.GetInt32(dr.GetOrdinal("stackid")),
                        (dr["stackprodids"] == DBNull.Value) ? "" : dr["stackprodids"].ToString(),
                        dr["stacktitle"].ToString(),
                        dr.GetInt32(dr.GetOrdinal("stackactive")),
                        dr.GetInt32(dr.GetOrdinal("stacksequence"))
                    );
                    //Coll.Add(newStack);
                    MultiSelectListBoxItem Item = new MultiSelectListBoxItem(
                            newStack.StackId,
                            newStack.StackTitle
                    );
                    collList.Add(Item);
                    
                    newStack = null;
                    Item = null;
                }
            }

            //return Coll;
            return collList;
        }

        //Assign stacks for the pubid
        public static bool SetNCIPLStacksByPubId(int pubid, string stackids, string Delim, out string ErrMsg)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_SetNCIPLStacksByPubId);

            db.AddInParameter(dbCommand, "@pubid", DbType.Int32, pubid);
            db.AddInParameter(dbCommand, "@stackids", DbType.String, @stackids);
            db.AddInParameter(dbCommand, "@delim", DbType.AnsiString, Delim);

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    Int32 pk = db.ExecuteNonQuery(dbCommand);
                    transaction.Commit();
                    ErrMsg = "";
                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    transaction.Rollback();
                    ErrMsg = ex.Message;
                    return false;
                }
            }
        }

        public static bool SetNCIPLStackPubHistoryByPubId(int PubId, string StackIds, int NCIPLFeaturedFlag)
        {   
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_SetFeaturedStacksHistoryByPubId);

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                db.AddInParameter(dbCommand, "@pubid", DbType.Int32, PubId);
                db.AddInParameter(dbCommand, "@stackids", DbType.AnsiString, StackIds);
                db.AddInParameter(dbCommand, "@NCIPLFeaturedFlag", DbType.Int32, NCIPLFeaturedFlag);
                
                try
                {
                    Int32 pk = db.ExecuteNonQuery(dbCommand);
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    transaction.Rollback();
                    return false;
                }
               
            }

        }
        
        #endregion

        /*End CR-36*/

        #region NCIPL_CC
        //Get all active collections
        public static SeriesCollection GetAllCollectionsByInterface(string Interface)
        {
            SeriesCollection collList = new SeriesCollection();

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetAllSeries_ByInterface);

            db.AddInParameter(dbCommand, "@Interface", DbType.String, Interface);

            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    Series newMultiSelectItem = new Series(
                        System.Convert.ToInt32(dr["SERIESID"].ToString()),
                        dr["DESCRIPTION"].ToString(),
                        System.Convert.ToBoolean(dr["ACTIVE_FLAG"])
                    );
                    collList.Add(newMultiSelectItem);
                }
            }
            return collList;
        }
        //Get collections by pubid and interface
        public static SeriesCollection GetCollectionsByInterfaceByPubId(string Interface, int PubId)
        {
            SeriesCollection collList = new SeriesCollection();

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetSeries_ByInterfaceByPubId);

            db.AddInParameter(dbCommand, "@PubId", DbType.String, PubId);
            db.AddInParameter(dbCommand, "@Interface", DbType.String, Interface);

            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    Series newMultiSelectItem = new Series(
                        System.Convert.ToInt32(dr["SERIESID"].ToString()),
                        dr["DESCRIPTION"].ToString(),
                        System.Convert.ToBoolean(dr["ACTIVE_FLAG"])
                    );
                    collList.Add(newMultiSelectItem);
                }
            }
            return collList;
        }
        //Set Collections
        //strSP_ADMIN_SetSeries_ByInterfaceByPubId
        public static bool SetSeries_ByInterfaceByPubId(int PubId, string SeriesIdParams, string Delim, string Interface)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_SetSeries_ByInterfaceByPubId);

            db.AddInParameter(dbCommand, "@PubId", DbType.Int32, PubId);
            db.AddInParameter(dbCommand, "@SeriesIdParams", DbType.String, SeriesIdParams);
            db.AddInParameter(dbCommand, "@delim", DbType.String, Delim);
            db.AddInParameter(dbCommand, "@Interface", DbType.String, Interface);

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    Int32 pk = db.ExecuteNonQuery(dbCommand);
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    transaction.Rollback();
                    return false;
                }
            }
        }
        #endregion


        /// <summary>
        /// Populate catalog subject seq
        /// </summary>
        public static List<Seq> GetAllCatalogSubjectSeq(string SortBy,
            bool SortAsc, bool active)
        {
            List<Seq> coll = new List<Seq>();

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetAllCatalogSubjectSeq);
            db.AddInParameter(dbCommand, "@SortBy", DbType.String, SortBy);
            db.AddInParameter(dbCommand, "@SortAsc", DbType.String, SortAsc ? "ASC" : "DESC");
            db.AddInParameter(dbCommand, "@Active", DbType.Int32, System.Convert.ToInt32(active));
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    Seq newMultiSelectItem = new Seq(
                        System.Convert.ToInt32(dr["SUBJECTID"].ToString()),
                        dr["DESCRIPTION"].ToString(),
                        System.Convert.ToInt32(dr["SORT_SEQ"])
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }

        public static List<Seq> GetAllCatalogSubCategorySeq(int SubjID, string SortBy,
            bool SortAsc, bool active)
        {
            List<Seq> coll = new List<Seq>();

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetAllCatalogSubCategorySeq);
            db.AddInParameter(dbCommand, "@SubjID", DbType.Int32, SubjID);
            db.AddInParameter(dbCommand, "@SortBy", DbType.String, SortBy);
            db.AddInParameter(dbCommand, "@SortAsc", DbType.String, SortAsc ? "ASC" : "DESC");
            db.AddInParameter(dbCommand, "@Active", DbType.Int32, System.Convert.ToInt32(active));
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    Seq newMultiSelectItem = new Seq(
                        System.Convert.ToInt32(dr["SUBCATEGORYID"].ToString()),
                        dr["DESCRIPTION"].ToString(),
                        System.Convert.ToInt32(dr["SORT_SEQ"])
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }

        public static List<Seq> GetAllCatalogSubSubCategorySeq(int SubcatID, string SortBy,
            bool SortAsc, bool active)
        {
            List<Seq> coll = new List<Seq>();

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetAllCatalogSubSubCategorySeq);
            db.AddInParameter(dbCommand, "@SubjID", DbType.Int32, SubcatID);
            db.AddInParameter(dbCommand, "@SortBy", DbType.String, SortBy);
            db.AddInParameter(dbCommand, "@SortAsc", DbType.String, SortAsc ? "ASC" : "DESC");
            db.AddInParameter(dbCommand, "@Active", DbType.Int32, System.Convert.ToInt32(active));
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    Seq newMultiSelectItem = new Seq(
                        System.Convert.ToInt32(dr["SUBSUBCATEGORYID"].ToString()),
                        dr["DESCRIPTION"].ToString(),
                        System.Convert.ToInt32(dr["SORT_SEQ"])
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }

        public static bool SetAllCatalogSubjectSeq(string id_seqs, string pairDelim,
            string indDelim)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_SetAllCatalogSubjectSeq);
            db.AddInParameter(dbCommand, "@id_seqs", DbType.String, id_seqs);
            db.AddInParameter(dbCommand, "@pairDelim", DbType.String, pairDelim);
            db.AddInParameter(dbCommand, "@indDelim", DbType.String, indDelim);

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    Int32 pk = db.ExecuteNonQuery(dbCommand);
                    transaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    transaction.Rollback();
                    return (false);
                }
            }
        }

        public static bool SetAllCatalogSubCatSeq(string id_seqs, string pairDelim,
            string indDelim)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_SetAllCatalogSubCatSeq);
            db.AddInParameter(dbCommand, "@id_seqs", DbType.String, id_seqs);
            db.AddInParameter(dbCommand, "@pairDelim", DbType.String, pairDelim);
            db.AddInParameter(dbCommand, "@indDelim", DbType.String, indDelim);

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    Int32 pk = db.ExecuteNonQuery(dbCommand);
                    transaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    transaction.Rollback();
                    return (false);
                }
            }
        }

        public static bool SetAllCatalogSubSubCatSeq(string id_seqs, string pairDelim,
            string indDelim)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_SetAllCatalogSubSubCatSeq);
            db.AddInParameter(dbCommand, "@id_seqs", DbType.String, id_seqs);
            db.AddInParameter(dbCommand, "@pairDelim", DbType.String, pairDelim);
            db.AddInParameter(dbCommand, "@indDelim", DbType.String, indDelim);

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    Int32 pk = db.ExecuteNonQuery(dbCommand);
                    transaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    transaction.Rollback();
                    return (false);
                }
            }
        }
        #endregion       

        #region Conference
        public static ConfCollection GetAllConference(bool active, bool availableConf)
        {
            ConfCollection coll = new ConfCollection();

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetAllConference);
            db.AddInParameter(dbCommand, "@Active", DbType.Int32, System.Convert.ToInt32(active));
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    Conf newMultiSelectItem = new Conf(
                        System.Convert.ToInt32(dr["CONFERENCEID"].ToString()),
                        dr["CONFERENCENAME"].ToString(),
                        System.Convert.ToInt32(dr["MAXORDER_INTL"].ToString()),
                        Convert.ToDateTime(dr["S_DATE"].ToString()),
                        Convert.ToDateTime(dr["E_DATE"].ToString()), System.Convert.ToBoolean(dr["ACTIVE_FLAG"])
                    );
                    if (availableConf)
                    {
                        if (Convert.ToDateTime(newMultiSelectItem.EndDate) > DateTime.Now.Date)
                            coll.Add(newMultiSelectItem);
                    }
                    else
                        coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }

        public static ConfCollection GetKioskConfByPubID(int PubID)
        {
            ConfCollection coll = new ConfCollection();

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetKioskConfByPubID);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, System.Convert.ToInt32(PubID));
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    Conf newMultiSelectItem = new Conf(
                        System.Convert.ToInt32(dr["CONFERENCEID"].ToString()),
                        dr["CONFERENCENAME"].ToString(),
                        System.Convert.ToInt32(dr["MAXORDER_INTL"].ToString()),
                        Convert.ToDateTime(dr["S_DATE"].ToString()),
                        Convert.ToDateTime(dr["E_DATE"].ToString())
                    );                   
                       coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }

        public static ConfCollection GetKioskConfRotateByPubID(int PubID)
        {
            ConfCollection coll = new ConfCollection();

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetKioskConfRotateByPubID);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, System.Convert.ToInt32(PubID));
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    Conf newMultiSelectItem = new Conf(
                        System.Convert.ToInt32(dr["CONFERENCEID"].ToString()),                        
                        dr["CONFERENCENAME"].ToString(), 
                        System.Convert.ToInt32(dr["MAXORDER_INTL"].ToString()),
                        Convert.ToDateTime(dr["S_DATE"].ToString()),
                        Convert.ToDateTime(dr["E_DATE"].ToString())
                    );                    
                        coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }
        #endregion

        #region Awards

        public static AwardCollection GetAllAward(bool active)
        {
            AwardCollection coll = new AwardCollection();

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetAllAwards);
            db.AddInParameter(dbCommand, "@Active", DbType.Int32, System.Convert.ToInt32(active));
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    Award newMultiSelectItem = new Award(
                        System.Convert.ToInt32(dr["AWARDID"].ToString()),
                        dr["Award_Description"].ToString(),
                        System.Convert.ToBoolean(dr["ACTIVE_FLAG"])
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }

        public static AwardCollection GetAwardByPubID(int PubID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand(strSP_ADMIN_GetAwardByPubID);
            db.AddInParameter(cw, "@PubID", DbType.String, PubID);
            using (System.Data.IDataReader dr = ExecuteReader(db, cw))
            {
                AwardCollection coll = new AwardCollection();
                while (dr.Read())
                {
                    Award newCancerType = new Award(Convert.ToInt32(dr["AWARDID"].ToString()),
                        dr["Award_Description"].ToString());
                    coll.Add(newCancerType);
                }
                return (coll);
            }
        }
        
        public static bool SetAwardByPubID(int PubID, string l, char delim)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_SetAwardByPubID);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);
            db.AddInParameter(dbCommand, "@Params", DbType.AnsiString, l);
            db.AddInParameter(dbCommand, "@delim", DbType.AnsiString, delim);

            
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    Int32 pk = db.ExecuteNonQuery(dbCommand);
                    transaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    transaction.Rollback();
                    return (false);
                }
            }
            
        }

        
        #endregion

        #region Display Status

        public static DisplayStatusCollection GetAllDisplayStatus(bool active)
        {
            DisplayStatusCollection coll = new DisplayStatusCollection();

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetAllDisplayStatus);
            db.AddInParameter(dbCommand, "@Active", DbType.Int32, System.Convert.ToInt32(active));
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    DisplayStatus newMultiSelectItem = new DisplayStatus(
                        System.Convert.ToInt32(dr["DISPLAYSTATUSID"].ToString()),
                        dr["DESCRIPTION"].ToString(),
                        System.Convert.ToBoolean(dr["ACTIVE_FLAG"])
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }

        public static DisplayStatusCollection GetNCIPLDisplayStatusByPubID(int PubID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand(strSP_ADMIN_GetNCIPLDisplayStatusByPubID);
            db.AddInParameter(cw, "@PubID", DbType.String, PubID);
            using (System.Data.IDataReader dr = ExecuteReader(db, cw))
            {
                DisplayStatusCollection coll = new DisplayStatusCollection();
                while (dr.Read())
                {
                    DisplayStatus newMultiSelectItem = new DisplayStatus(
                        System.Convert.ToInt32(dr["DISPLAYSTATUSID"].ToString()),
                        dr["DESCRIPTION"].ToString(),
                        System.Convert.ToBoolean(dr["ACTIVE_FLAG"])
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }

        public static DisplayStatusCollection GetROODisplayStatusByPubID(int PubID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand(strSP_ADMIN_GetROODisplayStatusByPubID);
            db.AddInParameter(cw, "@PubID", DbType.String, PubID);
            using (System.Data.IDataReader dr = ExecuteReader(db, cw))
            {
                DisplayStatusCollection coll = new DisplayStatusCollection();
                while (dr.Read())
                {
                    DisplayStatus newMultiSelectItem = new DisplayStatus(
                        System.Convert.ToInt32(dr["DISPLAYSTATUSID"].ToString()),
                        dr["DESCRIPTION"].ToString(),
                        System.Convert.ToBoolean(dr["ACTIVE_FLAG"])
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }

        public static NewUpdatedCollection GetNewUpdatedByPubID(int PubID, bool active)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand(strSP_ADMIN_GetStatusByPubID);
            db.AddInParameter(cw, "@PubID", DbType.String, PubID);
            using (System.Data.IDataReader dr = ExecuteReader(db, cw))
            {
                NewUpdatedCollection coll = new NewUpdatedCollection();
                while (dr.Read())
                {
                    NewUpdated newSingleSelectItem = new NewUpdated(
                        System.Convert.ToInt32(dr["NEW_PUB"]),
                        System.Convert.ToInt32(dr["UPDATED_PUB"]),
                        System.Convert.ToDateTime(dr["EXPIRATION_DT"]));
                    coll.Add(newSingleSelectItem);
                }
                return (coll);
            }
        }
        public static DisplayStatusCollection GetExhDisplayStatusByPubID(int PubID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand(strSP_ADMIN_GetExhDisplayStatusByPubID);
            db.AddInParameter(cw, "@PubID", DbType.String, PubID);
            using (System.Data.IDataReader dr = ExecuteReader(db, cw))
            {
                DisplayStatusCollection coll = new DisplayStatusCollection();
                while (dr.Read())
                {
                    DisplayStatus newMultiSelectItem = new DisplayStatus(
                        System.Convert.ToInt32(dr["DISPLAYSTATUSID"].ToString()),
                        dr["DESCRIPTION"].ToString(),
                        System.Convert.ToBoolean(dr["ACTIVE_FLAG"])
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }


        public static bool SetNCIPLDisplayStatusByPubID(int PubID, string l, char delim)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strsp_ADMIN_SetNCIPLDisplayStatusByPubID);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);
            db.AddInParameter(dbCommand, "@Params", DbType.AnsiString, l);
            db.AddInParameter(dbCommand, "@delim", DbType.AnsiString, delim);

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    Int32 pk = db.ExecuteNonQuery(dbCommand);
                    transaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    transaction.Rollback();
                    return (false);
                }
            }
        }
        
        public static bool SetROODisplayStatusByPubID(int PubID, string l, char delim)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strsp_ADMIN_SetROODisplayStatusByPubID);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);
            db.AddInParameter(dbCommand, "@Params", DbType.AnsiString, l);
            db.AddInParameter(dbCommand, "@delim", DbType.AnsiString, delim);

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    Int32 pk = db.ExecuteNonQuery(dbCommand);
                    transaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    transaction.Rollback();
                    return (false);
                }
            }
        }

        public static bool SetExhDisplayStatusByPubID(int PubID, string l, char delim)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strsp_ADMIN_SetExhDisplayStatusByPubID);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);
            db.AddInParameter(dbCommand, "@Params", DbType.AnsiString, l);
            db.AddInParameter(dbCommand, "@delim", DbType.AnsiString, delim);

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    Int32 pk = db.ExecuteNonQuery(dbCommand);
                    transaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    transaction.Rollback();
                    return (false);
                }
            }
        }
        #endregion

        #region NewUpdated Status

        //public static DisplayStatusCollection GetNewUpdatedStatus(bool active)
        //{
        //    DisplayStatusCollection coll = new DisplayStatusCollection();

        //    Database db = DatabaseFactory.CreateDatabase();
        //    DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetAllDisplayStatus);
        //    db.AddInParameter(dbCommand, "@Active", DbType.Int32, System.Convert.ToInt32(active));
        //    using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
        //    {
        //        while (dr.Read())
        //        {
        //            DisplayStatus newMultiSelectItem = new DisplayStatus(
        //                System.Convert.ToInt32(dr["DISPLAYSTATUSID"].ToString()),
        //                dr["DESCRIPTION"].ToString(),
        //                System.Convert.ToBoolean(dr["ACTIVE_FLAG"])
        //            );
        //            coll.Add(newMultiSelectItem);
        //        }
        //        return (coll);
        //    }
        //}
        #endregion

        #region Product General Data
        public static Pub GetProdGenData(int PubID)
        {
            Pub newMultiSelectItem = new Pub();

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetProductGeneralData);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    newMultiSelectItem = new Pub(
                        System.Convert.ToInt32(dr["PUBID"].ToString()),
                        dr["PRODUCTID"].ToString(),
                        dr["SHORTTITLE"].ToString(),
                        dr["LONGTITLE"].ToString(),
                        dr["NIH_NUM1"].ToString(),
                        dr["NIH_NUM2"].ToString(),
                        dr["NEW_PUB"].ToString(),
                        dr["UPDATED_PUB"].ToString(),
                        dr["FSNUM"].ToString(),
                        dr["SPANISH_ACCENT_LONGTITLE"].ToString(),
                        dr["SPANISH_NOACCENT_LONGTITLE"].ToString(),
                        dr["URL"].ToString(),
                        dr["URL2"].ToString(),
                        dr["PDFURL"].ToString(),
                        dr["KINDLEURL"].ToString(),
                        dr["EPUBURL"].ToString(),
                        dr["PRINTFILEURL"].ToString(),
                        System.Convert.ToBoolean(dr["NONEDITABLEFIELDSFLAG"]),
                        System.Convert.ToInt32(dr["MAXQTY"].ToString()),
                        System.Convert.ToInt32(dr["QUANTITY_AVAILABLE"].ToString()),
                        dr.GetInt32(dr.GetOrdinal("QUANTITY_THRESHOLD")),
                        dr["BookStatus"].ToString(),
                        System.Convert.ToDouble(dr["WEIGHT"].ToString()),
                        System.Convert.ToString(dr["OWNERLONG_NAME"].ToString()),
                        System.Convert.ToInt32(dr["OWNERID"].ToString()),
                        System.Convert.ToBoolean(dr["OWNERACTIVE"]),
                        System.Convert.ToBoolean(dr["OWNERARCHIVE"]),
                        System.Convert.ToString(dr["SPONSOR_NAME"].ToString()),
                        System.Convert.ToInt32(dr["SPONSORID"].ToString()),
                        System.Convert.ToBoolean(dr["SPONSORACTIVE"]),
                        System.Convert.ToBoolean(dr["SPONSORARCHIVE"]),
                        System.Convert.ToDateTime(dr["LAST_RECEIVED_DATE"]),
                        System.Convert.ToInt32(dr["PUBORIGDATE_M"].ToString()),
                        System.Convert.ToInt32(dr["PUBORIGDATE_D"].ToString()),
                        System.Convert.ToInt32(dr["PUBORIGDATE_Y"].ToString()),
                        System.Convert.ToInt32(dr["LAST_PRINT_DATE_M"].ToString()),
                        System.Convert.ToInt32(dr["LAST_PRINT_DATE_D"].ToString()),
                        System.Convert.ToInt32(dr["LAST_PRINT_DATE_Y"].ToString()),
                        System.Convert.ToInt32(dr["LAST_REVISED_DATE_M"].ToString()),
                        System.Convert.ToInt32(dr["LAST_REVISED_DATE_D"].ToString()),
                        System.Convert.ToInt32(dr["LAST_REVISED_DATE_Y"].ToString()),
                        System.Convert.ToDateTime(dr["ARCHIVED_DATE"])
                    );
                }
                return (newMultiSelectItem);
            }
        }

        public static int SetProdGenData(
            string PRODUCTID,
	        string SHORTTITLE,
	        string LONGTITLE,
            string FSNUM,
            string SPANISH_ACCENT_LONGTITLE,
	        string SPANISH_NOACCENT_LONGTITLE,
            string URL,
            string URL2,
            string PDFURL,
            string KINDLEURL,
            string EPUBURL,
            string PRINTFILEURL, 
            int MAXQTY,
            double WEIGHT,
            int QUANTITY_AVAILABLE,
            int QUANTITY_THRESHOLD,
            int BOOKSTATUSID,
	        ref int oPubID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_SetProductGeneralData);
            db.AddInParameter(dbCommand, "@PRODUCTID", DbType.String, PRODUCTID);
            db.AddInParameter(dbCommand, "@SHORTTITLE", DbType.String, SHORTTITLE);
            db.AddInParameter(dbCommand, "@LONGTITLE", DbType.String, LONGTITLE);

            if (FSNUM == null)
                db.AddInParameter(dbCommand, "@FSNUM", DbType.String, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@FSNUM", DbType.String, FSNUM);

            if (SPANISH_ACCENT_LONGTITLE == null)
                db.AddInParameter(dbCommand, "@SPANISH_ACCENT_LONGTITLE", DbType.String, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@SPANISH_ACCENT_LONGTITLE", DbType.String, SPANISH_ACCENT_LONGTITLE);

            if (SPANISH_NOACCENT_LONGTITLE == null)
                db.AddInParameter(dbCommand, "@SPANISH_NOACCENT_LONGTITLE", DbType.String, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@SPANISH_NOACCENT_LONGTITLE", DbType.String, SPANISH_NOACCENT_LONGTITLE);

            if (URL == null)
                db.AddInParameter(dbCommand, "@URL", DbType.AnsiString, SqlDateTime.Null);
            else
                db.AddInParameter(dbCommand, "@URL", DbType.AnsiString, URL);

            if (URL2 == null)
                db.AddInParameter(dbCommand, "@URL2", DbType.AnsiString, SqlDateTime.Null);
            else
                db.AddInParameter(dbCommand, "@URL2", DbType.AnsiString, URL2);

            if (PDFURL == null)
                db.AddInParameter(dbCommand, "@PDFURL", DbType.AnsiString, SqlDateTime.Null);
            else
                db.AddInParameter(dbCommand, "@PDFURL", DbType.AnsiString, PDFURL);

            if (KINDLEURL == null)
                db.AddInParameter(dbCommand, "@KINDLEURL", DbType.AnsiString, SqlDateTime.Null);
            else
                db.AddInParameter(dbCommand, "@KINDLEURL", DbType.AnsiString, KINDLEURL);

            if (EPUBURL == null)
                db.AddInParameter(dbCommand, "@EPUBURL", DbType.AnsiString, SqlDateTime.Null);
            else
                db.AddInParameter(dbCommand, "@EPUBURL", DbType.AnsiString, EPUBURL);

            if (PRINTFILEURL == null)
                db.AddInParameter(dbCommand, "@PRINTFILEURL", DbType.AnsiString, SqlDateTime.Null);
            else
                db.AddInParameter(dbCommand, "@PRINTFILEURL", DbType.AnsiString, PRINTFILEURL);

            if (MAXQTY == -1)
                db.AddInParameter(dbCommand, "@MAXQTY", DbType.Int32, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@MAXQTY", DbType.Int32, MAXQTY);

            if (WEIGHT == -1)
                db.AddInParameter(dbCommand, "@WEIGHT", DbType.Double, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@WEIGHT", DbType.Double, WEIGHT);

            //Begin NCIDC changes - Make fields editable
            if (QUANTITY_AVAILABLE == -1)
                db.AddInParameter(dbCommand, "@quantity_available", DbType.Int32, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@quantity_available", DbType.Int32, QUANTITY_AVAILABLE);

            if (QUANTITY_THRESHOLD == -1)
                db.AddInParameter(dbCommand, "@quantity_threshold", DbType.Int32, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@quantity_threshold", DbType.Int32, QUANTITY_THRESHOLD);
           
            if (BOOKSTATUSID > 0)
                db.AddInParameter(dbCommand, "@bookstatusid", DbType.Int32, BOOKSTATUSID);
            else if (BOOKSTATUSID == 0) //no bookstatus was selected
                db.AddInParameter(dbCommand, "@bookstatusid", DbType.Int32, -1); //hard coded value
            //End NCIDC changes

            db.AddInParameter(dbCommand, "@USERID", DbType.Int32, ((CustomPrincipal)HttpContext.Current.User).UserID);
            db.AddParameter(dbCommand, "@oPubID", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, oPubID);
            //using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            //{
                using (DbConnection connection = db.CreateConnection())
                {
                    connection.Open();
                    DbTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        Int32 pk = db.ExecuteNonQuery(dbCommand);
                        transaction.Commit();
                        oPubID = (int)db.GetParameterValue(dbCommand, "@oPubID");
                       
                        return pk;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                        transaction.Rollback();
                        return (-1);
                    }
                }

            //    scope.Complete();
            //}

        }
        
        #endregion

        #region Product Common Data
        public static Pub GetProdComData(int PubID)
        {
            Pub newMultiSelectItem = new Pub();

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetProductCommonData);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    newMultiSelectItem = new Pub(
                        System.Convert.ToInt32(dr["PUBID"].ToString()),
                        dr["SHORTTITLE"].ToString(),
                        dr["ABSTRACT"].ToString(),
                        dr["KEYWORDS"].ToString(),
                        dr["SUMMARY"].ToString(),
                        System.Convert.ToInt32(dr["COPYRIGHT"]),
                        dr["THUMBNAILFILE"].ToString(),
                        dr["LARGEIMAGEFILE"].ToString(),
                        System.Convert.ToInt32(dr["TOTAL_NUM_PAGE"].ToString()),
                        dr["DIMENSION"].ToString(),
                        dr["COLOR"].ToString(),
                        dr["OTHER"].ToString(),
                        dr["POS_INSTRUCTION"].ToString(),
                        
                        System.Convert.ToInt32(dr["NEW_PUB"]),
                        System.Convert.ToInt32(dr["UPDATED_PUB"]),
                        System.Convert.ToDateTime(dr["EXPIRATION_DT"])
                        );
                }
                return (newMultiSelectItem);
            }
        }

        public static bool SetProdComData(int PubID,
            string ABSTRACT,
	        string KEYWORDS,
	        string SUMMARY,
            int COPYRIGHT,
            string THUMBNAILFILE,
            string LGIMAGEFILE,
	        int TOTAL_NUM_PAGE,
            string DIMENSION,
            string COLOR,
	        string OTHER,
	        string POS_INSTRUCTION,

            int NEW_PUB, int UPDATED_PUB, DateTime EXPIRATION_DT
            )
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_SetProductCommonData);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);

            if (ABSTRACT == null)
                db.AddInParameter(dbCommand, "@ABSTRACT", DbType.AnsiString, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@ABSTRACT", DbType.AnsiString, ABSTRACT);

            if (KEYWORDS == null)
                db.AddInParameter(dbCommand, "@KEYWORDS", DbType.AnsiString, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@KEYWORDS", DbType.AnsiString, KEYWORDS);

            if (SUMMARY == null)
                db.AddInParameter(dbCommand, "@SUMMARY", DbType.AnsiString, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@SUMMARY", DbType.AnsiString, SUMMARY);

            if (COPYRIGHT == -1)
                db.AddInParameter(dbCommand, "@COPYRIGHT", DbType.Int32, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@COPYRIGHT", DbType.Int32, COPYRIGHT);

            if (THUMBNAILFILE == null)
                db.AddInParameter(dbCommand, "@THUMBNAILFILE", DbType.String, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@THUMBNAILFILE", DbType.String, THUMBNAILFILE);

            if (LGIMAGEFILE == null)
                db.AddInParameter(dbCommand, "@LGIMAGEFILE", DbType.String, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@LGIMAGEFILE", DbType.String, LGIMAGEFILE);


            if (TOTAL_NUM_PAGE == -1)
                db.AddInParameter(dbCommand, "@TOTAL_NUM_PAGE", DbType.Int32, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@TOTAL_NUM_PAGE", DbType.Int32, TOTAL_NUM_PAGE);

            if (DIMENSION == null)
                db.AddInParameter(dbCommand, "@DIMENSION", DbType.String, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@DIMENSION", DbType.String, DIMENSION);

            if (COLOR == null)
                db.AddInParameter(dbCommand, "@COLOR", DbType.String, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@COLOR", DbType.String, COLOR);

            if (OTHER == null)
                db.AddInParameter(dbCommand, "@OTHER", DbType.AnsiString, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@OTHER", DbType.AnsiString, OTHER);

            if (POS_INSTRUCTION == null)
                db.AddInParameter(dbCommand, "@POS_INSTRUCTION", DbType.AnsiString, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@POS_INSTRUCTION", DbType.AnsiString, POS_INSTRUCTION);

            if (NEW_PUB != -1)
                db.AddInParameter(dbCommand, "@NEW_PUB", DbType.Int32, NEW_PUB);
            else
                db.AddInParameter(dbCommand, "@NEW_PUB", DbType.Int32, DBNull.Value);

            if (UPDATED_PUB != -1)
                db.AddInParameter(dbCommand, "@UPDATED_PUB", DbType.Int32, UPDATED_PUB);
            else
                db.AddInParameter(dbCommand, "@UPDATED_PUB", DbType.Int32, DBNull.Value);

            if (EXPIRATION_DT == DateTime.MinValue)
                db.AddInParameter(dbCommand, "@EXPIRATION_DT", DbType.DateTime, SqlDateTime.Null);
            else
                db.AddInParameter(dbCommand, "@EXPIRATION_DT", DbType.DateTime, EXPIRATION_DT);

            //using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            //{
                using (DbConnection connection = db.CreateConnection())
                {
                    connection.Open();
                    DbTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        Int32 pk = db.ExecuteNonQuery(dbCommand);
                        transaction.Commit();

                        return true;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                        transaction.Rollback();
                        return (false);
                    }
                }

            //    scope.Complete();
            //}

        }
        
        #endregion

        #region LiveInterfaces
        public static LiveInt GetLiveIntByPubID(int PubID)
        {
            LiveInt newMultiSelectItem = new LiveInt();

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetLiveInterfaceByPubID);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    newMultiSelectItem = new LiveInt(
                        System.Convert.ToInt32(dr["NCIPL"].ToString()),
                        System.Convert.ToInt32(dr["ROO"].ToString()),
                        System.Convert.ToInt32(dr["EXHIBIT"].ToString()),
                        System.Convert.ToInt32(dr["CATALOG"].ToString())
                        );
                }
                return (newMultiSelectItem);
            }
        }

        public static bool SetLiveIntByPubID(int PubID,
            int NCIPL,
            int ROO,
            int EXHIBIT,
            int CATALOG)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_SetLiveInterfaceByPubID);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);

            if (NCIPL == -1)
                db.AddInParameter(dbCommand, "@NCIPL", DbType.Int32, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@NCIPL", DbType.Int32, NCIPL);

            if (ROO == -1)
                db.AddInParameter(dbCommand, "@ROO", DbType.Int32, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@ROO", DbType.Int32, ROO);

            if (EXHIBIT == -1)
                db.AddInParameter(dbCommand, "@EXHIBIT", DbType.Int32, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@EXHIBIT", DbType.Int32, EXHIBIT);

            if (CATALOG == -1)
                db.AddInParameter(dbCommand, "@CATALOG", DbType.Int32, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@CATALOG", DbType.Int32, CATALOG);

            //using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            //{
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    Int32 pk = db.ExecuteNonQuery(dbCommand);
                    transaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    transaction.Rollback();
                    return (false);
                }
            }

            //    scope.Complete();
            //}

        }

        public static LiveNewUpd GetLiveNewUpdatedByPubID(int PubID)
        {
            LiveNewUpd newMultiSelectItem = new LiveNewUpd();

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetStatusByPubID);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    newMultiSelectItem = new LiveNewUpd(
                        System.Convert.ToInt32(dr["NEW_PUB"].ToString()),
                        System.Convert.ToInt32(dr["UPDATED_PUB"].ToString())
                        );
                }
                return (newMultiSelectItem);
            }
        }
        public static bool SetLiveNewUpdatePubID(int PubID)
        {
            return (true);
        }
        #endregion

        #region Prod Hist
        public static PubHistCollection GetProdHist(int PubID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetProdHistByPubID);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                PubHistCollection coll = new PubHistCollection();
                while (dr.Read())
                {
                    PubHist newMultiSelectItem = new PubHist(
                    System.Convert.ToInt32(dr["PHDID"]),
                    System.Convert.ToInt32(dr["PUBID"]),
                    System.Convert.ToString(dr["NIH_NUM1"]),
                    System.Convert.ToString(dr["NIH_NUM2"]),
                    System.Convert.ToString(dr["SHORTTITLE"]),
                    System.Convert.ToInt32(dr["LAST_REVISED_DATE_M"]),
                    System.Convert.ToInt32(dr["LAST_REVISED_DATE_D"]),
                    System.Convert.ToInt32(dr["LAST_REVISED_DATE_Y"]),
                    System.Convert.ToInt32(dr["LAST_PRINT_DATE_M"]),
                    System.Convert.ToInt32(dr["LAST_PRINT_DATE_D"]),
                    System.Convert.ToInt32(dr["LAST_PRINT_DATE_Y"]),
                    System.Convert.ToDateTime(dr["RECEIVED_DATE"]),
                    System.Convert.ToInt32(dr["QUANTITY_RCVD"])
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }

        public static void GetProdOigDate(int PubID, ref int PubD, ref int PubM, ref int PubY)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetOrigPubDate);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);
            db.AddOutParameter(dbCommand, "@oPubD", DbType.Int32, Int32.MaxValue);
            db.AddOutParameter(dbCommand, "@oPubM", DbType.Int32, Int32.MaxValue);
            db.AddOutParameter(dbCommand, "@oPubY", DbType.Int32, Int32.MaxValue);

            db.ExecuteNonQuery(dbCommand);

            PubD = (int)db.GetParameterValue(dbCommand, "@oPubD");
            PubM = (int)db.GetParameterValue(dbCommand, "@oPubM");
            PubY = (int)db.GetParameterValue(dbCommand, "@oPubY");
            
        }
        
        public static int GetProdHistCnt(int PubID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetPubHistCnt);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);
            db.AddOutParameter(dbCommand, "@oPubHistCnt", DbType.Int32, Int32.MaxValue);

            db.ExecuteNonQuery(dbCommand);

            return ((int)db.GetParameterValue(dbCommand,"@oPubHistCnt"));
            
        }

        public static bool SetProdHist(int PubID,
            string NIH_NUM1,
	        string NIH_NUM2,
	        int REVISED_DATE_M,
            int REVISED_DATE_D,
            int REVISED_DATE_Y,
	        int PRINT_DATE_M,
            int PRINT_DATE_D,
            int PRINT_DATE_Y,
	        DateTime ARCHIVED_DATE,
	        DateTime RECEIVED_DATE,
	        int QUANTITY_RCVD,
	        int PUBORIGDATE_M,
            int PUBORIGDATE_D,
            int PUBORIGDATE_Y,
	        bool NODATEONPUB)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_SetProdHistByPubID);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);

            if (NIH_NUM1 == null)
                db.AddInParameter(dbCommand, "@NIH_NUM1", DbType.String, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@NIH_NUM1", DbType.String, NIH_NUM1);

            if (NIH_NUM2 == null)
                db.AddInParameter(dbCommand, "@NIH_NUM2", DbType.String, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@NIH_NUM2", DbType.String, NIH_NUM2);

            if (REVISED_DATE_M == 0)
                db.AddInParameter(dbCommand, "@REVISED_DATE_M", DbType.Int32, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@REVISED_DATE_M", DbType.Int32, REVISED_DATE_M);

            if (REVISED_DATE_D == 0)
                db.AddInParameter(dbCommand, "@REVISED_DATE_D", DbType.Int32, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@REVISED_DATE_D", DbType.Int32, REVISED_DATE_D);

            if (REVISED_DATE_Y == 0)
                db.AddInParameter(dbCommand, "@REVISED_DATE_Y", DbType.Int32, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@REVISED_DATE_Y", DbType.Int32, REVISED_DATE_Y);

            if (PRINT_DATE_M == 0)
                db.AddInParameter(dbCommand, "@PRINT_DATE_M", DbType.Int32, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@PRINT_DATE_M", DbType.Int32, PRINT_DATE_M);

            if (PRINT_DATE_D == 0)
                db.AddInParameter(dbCommand, "@PRINT_DATE_D", DbType.Int32, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@PRINT_DATE_D", DbType.Int32, PRINT_DATE_D);

            if (PRINT_DATE_Y == 0)
                db.AddInParameter(dbCommand, "@PRINT_DATE_Y", DbType.Int32, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@PRINT_DATE_Y", DbType.Int32, PRINT_DATE_Y);

            if (ARCHIVED_DATE == DateTime.MinValue)
                db.AddInParameter(dbCommand, "@ARCHIVED_DATE", DbType.DateTime, SqlDateTime.Null);
            else
                db.AddInParameter(dbCommand, "@ARCHIVED_DATE", DbType.DateTime, ARCHIVED_DATE);

            if (RECEIVED_DATE == DateTime.MinValue)
                db.AddInParameter(dbCommand, "@RECEIVED_DATE", DbType.DateTime, SqlDateTime.Null);
            else
                db.AddInParameter(dbCommand, "@RECEIVED_DATE", DbType.DateTime, RECEIVED_DATE);

            if (QUANTITY_RCVD == -1)
                db.AddInParameter(dbCommand, "@QUANTITY_RCVD", DbType.Int32, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@QUANTITY_RCVD", DbType.Int32, QUANTITY_RCVD);

            if (PUBORIGDATE_M == 0)
                db.AddInParameter(dbCommand, "@PUBORIGDATE_M", DbType.Int32, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@PUBORIGDATE_M", DbType.Int32, PUBORIGDATE_M);

            if (PUBORIGDATE_D == 0)
                db.AddInParameter(dbCommand, "@PUBORIGDATE_D", DbType.Int32, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@PUBORIGDATE_D", DbType.Int32, PUBORIGDATE_D);

            if (PUBORIGDATE_Y == 0)
                db.AddInParameter(dbCommand, "@PUBORIGDATE_Y", DbType.Int32, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@PUBORIGDATE_Y", DbType.Int32, PUBORIGDATE_Y);

            db.AddInParameter(dbCommand, "@NODATEONPUB", DbType.Int32, NODATEONPUB?1:0);

            db.AddInParameter(dbCommand, "@USERID", DbType.Int32, ((CustomPrincipal)HttpContext.Current.User).UserID);

            //using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            //{
                using (DbConnection connection = db.CreateConnection())
                {
                    connection.Open();
                    DbTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        Int32 pk = db.ExecuteNonQuery(dbCommand);
                        transaction.Commit();

                        return true;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                        transaction.Rollback();
                        return (false);
                    }
                }

            //    scope.Complete();
            //}

        }

        public static bool UpdateProdHist(int PhdID, int PubID,
            string NIH_NUM1,
            string NIH_NUM2,
            int REVISED_DATE_M,
            int REVISED_DATE_D,
            int REVISED_DATE_Y,
            int PRINT_DATE_M,
            int PRINT_DATE_D,
            int PRINT_DATE_Y,
            //DateTime ARCHIVED_DATE,
            DateTime RECEIVED_DATE,
            int QUANTITY_RCVD
            //int PUBORIGDATE_M,
            //int PUBORIGDATE_D,
            //int PUBORIGDATE_Y,
            //bool NODATEONPUB
            )
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_UpdateProdHistByPhdID);
            db.AddInParameter(dbCommand, "@PhdID", DbType.Int32, PhdID);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);

            if (NIH_NUM1 == null)
                db.AddInParameter(dbCommand, "@NIH_NUM1", DbType.String, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@NIH_NUM1", DbType.String, NIH_NUM1);

            if (NIH_NUM2 == null)
                db.AddInParameter(dbCommand, "@NIH_NUM2", DbType.String, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@NIH_NUM2", DbType.String, NIH_NUM2);

            if (REVISED_DATE_M == 0)
                db.AddInParameter(dbCommand, "@REVISED_DATE_M", DbType.Int32, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@REVISED_DATE_M", DbType.Int32, REVISED_DATE_M);

            if (REVISED_DATE_D == 0)
                db.AddInParameter(dbCommand, "@REVISED_DATE_D", DbType.Int32, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@REVISED_DATE_D", DbType.Int32, REVISED_DATE_D);

            if (REVISED_DATE_Y == 0)
                db.AddInParameter(dbCommand, "@REVISED_DATE_Y", DbType.Int32, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@REVISED_DATE_Y", DbType.Int32, REVISED_DATE_Y);


            if (PRINT_DATE_M == 0)
                db.AddInParameter(dbCommand, "@PRINT_DATE_M", DbType.Int32, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@PRINT_DATE_M", DbType.Int32, PRINT_DATE_M);

            if (PRINT_DATE_D == 0)
                db.AddInParameter(dbCommand, "@PRINT_DATE_D", DbType.Int32, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@PRINT_DATE_D", DbType.Int32, PRINT_DATE_D);

            if (PRINT_DATE_Y == 0)
                db.AddInParameter(dbCommand, "@PRINT_DATE_Y", DbType.Int32, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@PRINT_DATE_Y", DbType.Int32, PRINT_DATE_Y);

            if (RECEIVED_DATE == DateTime.MinValue)
                db.AddInParameter(dbCommand, "@RECEIVED_DATE", DbType.DateTime, SqlDateTime.Null);
            else
                db.AddInParameter(dbCommand, "@RECEIVED_DATE", DbType.DateTime, RECEIVED_DATE);

            if (QUANTITY_RCVD == -1)
                db.AddInParameter(dbCommand, "@QUANTITY_RCVD", DbType.Int32, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@QUANTITY_RCVD", DbType.Int32, QUANTITY_RCVD);

            db.AddInParameter(dbCommand, "@USERID", DbType.Int32, ((CustomPrincipal)HttpContext.Current.User).UserID);

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    Int32 pk = db.ExecuteNonQuery(dbCommand);
                    transaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    transaction.Rollback();
                    return (false);
                }
            }

        }
        #endregion

        #region NCIPL
        public static NCIPLCollection GetNCIPLInterface(int PubID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetNCIPLInterfaceDataByPubID);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                NCIPLCollection coll = new NCIPLCollection();
                while (dr.Read())
                {
                    NCIPL newMultiSelectItem = new NCIPL(System.Convert.ToInt32(dr["PUBID"]),
                    System.Convert.ToString(dr["SHORTTITLE"]),
                    System.Convert.ToInt32(dr["MAXQTY_NCIPL"]),
                    System.Convert.ToInt32(dr["MAXINTL_NCIPL"]),
                    System.Convert.ToInt32(dr["EVERYORDER_NCIPL"]),
                    System.Convert.ToInt32(dr["ISSEARCHABLE_NCIPL"]),                  
                    System.Convert.ToInt32(dr["NCIPLFeatured"]),
                    System.Convert.ToInt32(dr["Rank"]),
                    System.Convert.ToString(dr["NCIPLFEATURED_IMAGEFILE"]
                    )
                    //System.Convert.ToInt32(dr["NEW_PUB"]),
                    //System.Convert.ToInt32(dr["UPDATED_PUB"]),
                    //System.Convert.ToDateTime(dr["EXPIRATION_DT"]),                  

                    //System.Convert.ToBoolean(dr["EVERYORDER_NCIPL"]),
                    //System.Convert.ToBoolean(dr["ISSEARCHABLE_NCIPL"]),
                    //System.Convert.ToBoolean(dr["NCIPLFeatured"])
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }

        public static bool SetNCIPLInterface(int PubID, int MAXQTY_NCIPL,
            int MAXINTL_NCIPL, int EVERYORDER_NCIPL, int ISSEARCHABLE_NCIPL,
            //int NEW_PUB, int UPDATED_PUB, DateTime EXPIRATION_DT, 
            int NCIPLFeatured, string FeaturedImageFile)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_SetNCIPLInterfaceDataByPubID);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);

            if (MAXQTY_NCIPL != -1)
                db.AddInParameter(dbCommand, "@MAXQTY_NCIPL", DbType.Int32, MAXQTY_NCIPL);
            else
                db.AddInParameter(dbCommand, "@MAXQTY_NCIPL", DbType.Int32, DBNull.Value);

            if (MAXINTL_NCIPL != -1)
                db.AddInParameter(dbCommand, "@MAXINTL_NCIPL", DbType.Int32, MAXINTL_NCIPL);
            else
                db.AddInParameter(dbCommand, "@MAXINTL_NCIPL", DbType.Int32, DBNull.Value);

            if (EVERYORDER_NCIPL != -1)
                db.AddInParameter(dbCommand, "@EVERYORDER_NCIPL", DbType.Int32, EVERYORDER_NCIPL);
            else
                db.AddInParameter(dbCommand, "@EVERYORDER_NCIPL", DbType.Int32, DBNull.Value);

            if (ISSEARCHABLE_NCIPL != -1)
                db.AddInParameter(dbCommand, "@ISSEARCHABLE_NCIPL", DbType.Int32, ISSEARCHABLE_NCIPL);
            else
                db.AddInParameter(dbCommand, "@ISSEARCHABLE_NCIPL", DbType.Int32, DBNull.Value);



            //if (NEW_PUB != -1)
            //    db.AddInParameter(dbCommand, "@NEW_PUB", DbType.Int32, NEW_PUB);
            //else
            //    db.AddInParameter(dbCommand, "@NEW_PUB", DbType.Int32, DBNull.Value);

            //if (UPDATED_PUB != -1)
            //    db.AddInParameter(dbCommand, "@UPDATED_PUB", DbType.Int32, UPDATED_PUB);
            //else
            //    db.AddInParameter(dbCommand, "@UPDATED_PUB", DbType.Int32, DBNull.Value);

            //if (EXPIRATION_DT == DateTime.MinValue)
            //    db.AddInParameter(dbCommand, "@EXPIRATION_DT", DbType.DateTime, SqlDateTime.Null);
            //else
            //    db.AddInParameter(dbCommand, "@EXPIRATION_DT", DbType.DateTime, EXPIRATION_DT);

            if (NCIPLFeatured != -1)
                db.AddInParameter(dbCommand, "@NCIPLFeatured", DbType.Int32, NCIPLFeatured);
            else
                db.AddInParameter(dbCommand, "@NCIPLFeatured", DbType.Int32, DBNull.Value);

            if (FeaturedImageFile == null)
                db.AddInParameter(dbCommand, "@NCIPLFeaturedImageFile", DbType.String, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@NCIPLFeaturedImageFile", DbType.String, FeaturedImageFile);

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    Int32 pk = db.ExecuteNonQuery(dbCommand);
                    transaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    transaction.Rollback();
                    return (false);
                }
            }
        }

        public static bool SetNCIPLSubjectByPubID(int PubID, string l, char delim)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_SetNCIPLSubjectByPubID);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);
            db.AddInParameter(dbCommand, "@Params", DbType.AnsiString, l);
            db.AddInParameter(dbCommand, "@delim", DbType.AnsiString, delim);

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    Int32 pk = db.ExecuteNonQuery(dbCommand);
                    transaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    transaction.Rollback();
                    return (false);
                }
            }
        }

        public static bool SetNCIPLRankByPubID(int PubID, string Rank, int NCIPL)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_SP_ADMIN_SetRankByPubId);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);
            db.AddInParameter(dbCommand, "@rank", DbType.AnsiString, Rank);
            db.AddInParameter(dbCommand, "@NCIPL", DbType.Int32, NCIPL);

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    Int32 pk = db.ExecuteNonQuery(dbCommand);
                    transaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    transaction.Rollback();
                    return (false);
                }
            }
        }

        #region NCIPL View
        public static DataSet GetNCIPLDisplayStatusView(int PubID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetNCIPLDisplayStatusByPubID);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);

            return ExecutedataSet(db,dbCommand);
        }

        public static DataSet GetNCIPLInterfaceView(int PubID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetNCIPLInterfaceViewByPubID);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);

            return ExecutedataSet(db,dbCommand);
        }
        
        public static DataSet GetNCIPLSubjectView(int PubID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetNCIPLSubjectViewByPubID);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);

            return ExecutedataSet(db,dbCommand);
        }
        
        //Begin - CR-11-001-36 (Featured Stacks)
        //public static DataSet GetNCIPLStacksView(int PubID)
        public static StackCollection GetNCIPLStacksView(int PubID)
        {
            StackCollection Coll = new StackCollection();
            
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetNCIPLStacksViewByPubId);
            db.AddInParameter(dbCommand, "@pubid", DbType.Int32, PubID);

            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    Stack newStack = new Stack(
                        dr.GetInt32(dr.GetOrdinal("stackid")),
                        (dr["stackprodids"] == DBNull.Value) ? "" : dr["stackprodids"].ToString(),
                        dr["stacktitle"].ToString(),
                        dr.GetInt32(dr.GetOrdinal("stackactive")),
                        dr.GetInt32(dr.GetOrdinal("stacksequence"))
                    );
                    Coll.Add(newStack);
                }
            }

            return Coll;


            //return ExecutedataSet(db,dbCommand);
        }
        //End - CR-36

        #endregion
        
        #endregion

        #region ROO
        public static ROOCollection GetROOInterface(int PubID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetROOInterfaceDataByPubID);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                ROOCollection coll = new ROOCollection();
                while (dr.Read())
                {
                    ROO newMultiSelectItem = new ROO(System.Convert.ToInt32(dr["PUBID"]),
                    System.Convert.ToString(dr["SHORTTITLE"]),
                    System.Convert.ToInt32(dr["MAXQTY_ROO"]),
                    System.Convert.ToInt32(dr["EVERYORDER_ROO"]),
                    System.Convert.ToInt32(dr["ISSEARCHABLE_ROO"]),
                    System.Convert.ToInt32(dr["ISROO_KIT"]),
                    System.Convert.ToInt32(dr["ROO_COMMONLIST"]),
                    System.Convert.ToInt32(dr["FK_COMMONLISTSUBJ"])
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }

        public static bool SetROOInterface(int PubID, int MAXQTY_ROO,
            int EVERYORDER_ROO, int ISSEARCHABLE_ROO, int ISROO_KIT, int ROO_COMMONLIST,
            int FK_COMMONLISTSUBJ)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_SetROOInterfaceDataByPubID);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);

            if (MAXQTY_ROO != -1)
                db.AddInParameter(dbCommand, "@MAXQTY_ROO", DbType.Int32, MAXQTY_ROO);
            else
                db.AddInParameter(dbCommand, "@MAXQTY_ROO", DbType.Int32, DBNull.Value);

            if (EVERYORDER_ROO != -1)
                db.AddInParameter(dbCommand, "@EVERYORDER_ROO", DbType.Int32, EVERYORDER_ROO);
            else
                db.AddInParameter(dbCommand, "@EVERYORDER_ROO", DbType.Int32, DBNull.Value);

            if (ISSEARCHABLE_ROO != -1)
                db.AddInParameter(dbCommand, "@ISSEARCHABLE_ROO", DbType.Int32, ISSEARCHABLE_ROO);
            else
                db.AddInParameter(dbCommand, "@ISSEARCHABLE_ROO", DbType.Int32, DBNull.Value);

            if (ISROO_KIT != -1)
                db.AddInParameter(dbCommand, "@ISROO_KIT", DbType.Int32, ISROO_KIT);
            else
                db.AddInParameter(dbCommand, "@ISROO_KIT", DbType.Int32, DBNull.Value);

            if (ROO_COMMONLIST != -1)
                db.AddInParameter(dbCommand, "@ROO_COMMONLIST", DbType.Int32, ROO_COMMONLIST);
            else
                db.AddInParameter(dbCommand, "@ROO_COMMONLIST", DbType.Int32, DBNull.Value);

            if (FK_COMMONLISTSUBJ != -1)
                db.AddInParameter(dbCommand, "@FK_COMMONLISTSUBJ", DbType.Int32, FK_COMMONLISTSUBJ);
            else
                db.AddInParameter(dbCommand, "@FK_COMMONLISTSUBJ", DbType.Int32, DBNull.Value);

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    Int32 pk = db.ExecuteNonQuery(dbCommand);
                    transaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    transaction.Rollback();
                    return (false);
                }
            }
        }

        public static bool SetROOSubjectByPubID(int PubID, string l, char delim)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_SetROOSubjectByPubID);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);
            db.AddInParameter(dbCommand, "@Params", DbType.AnsiString, l);
            db.AddInParameter(dbCommand, "@delim", DbType.AnsiString, delim);

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    Int32 pk = db.ExecuteNonQuery(dbCommand);
                    transaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    transaction.Rollback();
                    return (false);
                }
            }
        }

        #region ROO View
        public static DataSet GetROODisplayStatusView(int PubID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetROODisplayStatusByPubID);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);

            return ExecutedataSet(db, dbCommand);
        }

        public static DataSet GetROOInterfaceView(int PubID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetROOInterfaceViewByPubID);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);

            return ExecutedataSet(db, dbCommand);
        }

        public static DataSet GetROOSubjectView(int PubID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetROOSubjectViewByPubID);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);

            return ExecutedataSet(db, dbCommand);
        }
        #endregion
        #endregion

        #region Exhibit
        public static ExhCollection GetExhInterface(int PubID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetExhInterfaceDataByPubID);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                ExhCollection coll = new ExhCollection();
                while (dr.Read())
                {
                    Exh newMultiSelectItem = new Exh(System.Convert.ToInt32(dr["PUBID"]),
                    System.Convert.ToString(dr["SHORTTITLE"]),
                    System.Convert.ToInt32(dr["MAXQTY_EXHIBIT"]),
                    System.Convert.ToInt32(dr["MAXINTL_EXHIBIT"]),
                    System.Convert.ToInt32(dr["EVERYORDER_EXHIBIT"]),
                    System.Convert.ToInt32(dr["ISSEARCHABLE_EXHIBIT"])
                    //System.Convert.ToBoolean(dr["EVERYORDER_EXHIBIT"]),
                    //System.Convert.ToBoolean(dr["ISSEARCHABLE_EXHIBIT"])
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }

        public static bool SetExhInterface(int PubID, int MAXQTY_EXHIBIT,
            int MAXINTL_EXHIBIT, int EVERYORDER_EXHIBIT, int ISSEARCHABLE_EXHIBIT)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_SetExhInterfaceDataByPubID);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);

            if (MAXQTY_EXHIBIT != -1)
                db.AddInParameter(dbCommand, "@MAXQTY_EXHIBIT", DbType.Int32, MAXQTY_EXHIBIT);
            else
                db.AddInParameter(dbCommand, "@MAXQTY_EXHIBIT", DbType.Int32, DBNull.Value);

            if (MAXINTL_EXHIBIT != -1)
                db.AddInParameter(dbCommand, "@MAXINTL_EXHIBIT", DbType.Int32, MAXINTL_EXHIBIT);
            else
                db.AddInParameter(dbCommand, "@MAXINTL_EXHIBIT", DbType.Int32, DBNull.Value);

            if (EVERYORDER_EXHIBIT != -1)
                db.AddInParameter(dbCommand, "@EVERYORDER_EXHIBIT", DbType.Int32, EVERYORDER_EXHIBIT);
            else
                db.AddInParameter(dbCommand, "@EVERYORDER_EXHIBIT", DbType.Int32, DBNull.Value);

            if (ISSEARCHABLE_EXHIBIT != -1)
                db.AddInParameter(dbCommand, "@ISSEARCHABLE_EXHIBIT", DbType.Int32, ISSEARCHABLE_EXHIBIT);
            else
                db.AddInParameter(dbCommand, "@ISSEARCHABLE_EXHIBIT", DbType.Int32, DBNull.Value);

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    Int32 pk = db.ExecuteNonQuery(dbCommand);
                    transaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    transaction.Rollback();
                    return (false);
                }
            }
        }

        public static bool SetExhKioskInterface(int PubID, string lc, string lr, char delim)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_SetExhKioskInterfaceDataByPubID);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);
            db.AddInParameter(dbCommand, "@Params", DbType.AnsiString, lc);
            db.AddInParameter(dbCommand, "@Params1", DbType.AnsiString, lr);
            db.AddInParameter(dbCommand, "@delim", DbType.AnsiString, delim);

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    Int32 pk = db.ExecuteNonQuery(dbCommand);
                    transaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    transaction.Rollback();
                    return (false);
                }
            }
        }

        #region Exhibit View
        public static DataSet GetExhDisplayStatusView(int PubID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetExhDisplayStatusByPubID);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);

            return ExecutedataSet(db, dbCommand);
        }

        public static DataSet GetExhInterfaceView(int PubID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetExhInterfaceViewByPubID);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);

            return ExecutedataSet(db, dbCommand);
        }

        public static DataSet GetExhConfView(int PubID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetKioskConfViewByPubID);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);

            return ExecutedataSet(db, dbCommand);
        }

        public static DataSet GetExhConfRotateView(int PubID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetKioskConfRotateViewByPubID);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);

            return ExecutedataSet(db, dbCommand);
        }
        
       
        #endregion
       
        #endregion

        #region Catalog
        public static CatalogCollection GetCatalogInterface(int PubID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetCatalogInterfaceDataByPubID);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                CatalogCollection coll = new CatalogCollection();
                while (dr.Read())
                {
                    Catalog newMultiSelectItem = new Catalog(System.Convert.ToInt32(dr["PUBID"]),
                    System.Convert.ToString(dr["SHORTTITLE"]),
                    System.Convert.ToInt32(dr["Category"]),
                    System.Convert.ToInt32(dr["SubCategory"]),
                    System.Convert.ToInt32(dr["SubSubCategory"]),
                    System.Convert.ToInt32(dr["WYNTK"]),
                    System.Convert.ToInt32(dr["SPANISH_WYNTK"]),
                    System.Convert.ToInt32(dr["CatalogSubject"])
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }

        public static bool SetCatalogInterface(int PubID, int CATALOG_SUBJ2_FK_CODE,
            int CATALOG_SUBCATEGORY_FK, int CATALOG_SUBSUBCATEGORY_FK, int CATALOG_WYNTK_FLAG,
            int SPANISH_WYNTK_FLAG, int CATALOG_SUBJ1_FK_CODE)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_SetCatalogInterfaceDataByPubID);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);

            if (CATALOG_SUBJ2_FK_CODE != -1)
                db.AddInParameter(dbCommand, "@CATALOG_SUBJ2_FK_CODE", DbType.Int32, CATALOG_SUBJ2_FK_CODE);
            else
                db.AddInParameter(dbCommand, "@CATALOG_SUBJ2_FK_CODE", DbType.Int32, DBNull.Value);

            if (CATALOG_SUBCATEGORY_FK != -1)
                db.AddInParameter(dbCommand, "@CATALOG_SUBCATEGORY_FK", DbType.Int32, CATALOG_SUBCATEGORY_FK);
            else
                db.AddInParameter(dbCommand, "@CATALOG_SUBCATEGORY_FK", DbType.Int32, DBNull.Value);

            if (CATALOG_SUBSUBCATEGORY_FK != -1)
                db.AddInParameter(dbCommand, "@CATALOG_SUBSUBCATEGORY_FK", DbType.Int32, CATALOG_SUBSUBCATEGORY_FK);
            else
                db.AddInParameter(dbCommand, "@CATALOG_SUBSUBCATEGORY_FK", DbType.Int32, DBNull.Value);

            if (CATALOG_WYNTK_FLAG != -1)
                db.AddInParameter(dbCommand, "@CATALOG_WYNTK_FLAG", DbType.Int32, CATALOG_WYNTK_FLAG);
            else
                db.AddInParameter(dbCommand, "@CATALOG_WYNTK_FLAG", DbType.Int32, DBNull.Value);

            if (SPANISH_WYNTK_FLAG != -1)
                db.AddInParameter(dbCommand, "@SPANISH_WYNTK_FLAG", DbType.Int32, SPANISH_WYNTK_FLAG);
            else
                db.AddInParameter(dbCommand, "@SPANISH_WYNTK_FLAG", DbType.Int32, DBNull.Value);

            if (CATALOG_SUBJ1_FK_CODE != -1)
                db.AddInParameter(dbCommand, "@CATALOG_SUBJ1_FK_CODE", DbType.Int32, CATALOG_SUBJ1_FK_CODE);
            else
                db.AddInParameter(dbCommand, "@CATALOG_SUBJ1_FK_CODE", DbType.Int32, DBNull.Value);

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    Int32 pk = db.ExecuteNonQuery(dbCommand);
                    transaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    transaction.Rollback();
                    return (false);
                }
            }
        }

        #region Catalog View
        public static DataSet GetCatalogInterfaceView(int PubID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetCatalogInterfaceViewByPubID);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);

            return ExecutedataSet(db, dbCommand);
        }
        #endregion

        #endregion

        #region Attachment
        public static int SetAttachment(int PubID, int CreatorID,
            string CreatorUsername, string FileName, int FileSize,
            string FileContentType, byte[] FileData)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_SetNewAttachment);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);
            db.AddInParameter(dbCommand, "@FileCreatorID", DbType.Int32, CreatorID);
            db.AddInParameter(dbCommand, "@FileCreatorName", DbType.AnsiString, CreatorUsername);
            db.AddInParameter(dbCommand, "@FileName", DbType.AnsiString, FileName);
            db.AddInParameter(dbCommand, "@FileSize", DbType.Int32, FileSize);
            db.AddInParameter(dbCommand, "@FileContentType", DbType.AnsiString, FileContentType);
            db.AddInParameter(dbCommand, "@FileData", DbType.Binary, FileData);

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    Int32 pk = db.ExecuteNonQuery(dbCommand);
                    transaction.Commit();

                    return ((1));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    transaction.Rollback();
                    return (0);
                }
            }

        }

        public static AttachmentCollection GetAttachmentsByPubID(int PubID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetAttachmentsByPubId);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);

            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                AttachmentCollection coll = new AttachmentCollection();
                while (dr.Read())
                {
                    Attachment newMultiSelectItem = new Attachment(
                    System.Convert.ToInt32(dr["FileId"]),
                    System.Convert.ToInt32(dr["PUBID"]),
                    System.Convert.ToInt32(dr["FileCreatorID"]),
                    System.Convert.ToString(dr["FileCreatorName"]),
                    System.Convert.ToString(dr["FileName"]),
                    System.Convert.ToInt32(dr["FileSize"]),
                    System.Convert.ToString(dr["FileContentType"]),
                    System.Convert.ToDateTime(dr["DateCreated"])
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }

        }

        public static Attachment GetAttachmentByFileID(int FileID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetAttachmentByFileId);
            db.AddInParameter(dbCommand, "@FileId", DbType.Int32, FileID);
            Attachment newMultiSelectItem = null;

            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    newMultiSelectItem = new Attachment(
                    System.Convert.ToInt32(dr["FileId"]),
                    System.Convert.ToInt32(dr["PUBID"]),
                    System.Convert.ToInt32(dr["FileCreatorID"]),
                    System.Convert.ToString(dr["FileCreatorName"]),
                    System.Convert.ToString(dr["FileName"]),
                    System.Convert.ToInt32(dr["FileSize"]),
                    System.Convert.ToString(dr["FileContentType"]),
                    System.Convert.ToDateTime(dr["DateCreated"]),
                    (byte[])(dr["FileData"])
                    );
                }
                return (newMultiSelectItem);
            }
        }

        public static int DeleteAttachmentByFileID(int FileID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_DeleteAttachment);
            db.AddInParameter(dbCommand, "@FileIdToDelete", DbType.Int32, FileID);
            return db.ExecuteNonQuery(dbCommand);
        }

        public static int DeleteAttachmentsByPubID(int PubID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_DeleteAttachmentsByPubId);
            db.AddInParameter(dbCommand, "@PubIdToDelete", DbType.Int32, PubID);
            return db.ExecuteNonQuery(dbCommand);
        }
        #endregion
        public static PersonCollection  GetDNMList(string s)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand("sp_ADMIN_GetDNMList");
            db.AddInParameter(cw, "@s", DbType.AnsiString, s);
            using (System.Data.IDataReader dr = db.ExecuteReader(cw))
            {
                PersonCollection coll = new PersonCollection();
                while (dr.Read())
                {
                    Person k = new Person(dr.GetInt32(dr.GetOrdinal("autoid")),"","","",
                                            (dr["addr1"] == DBNull.Value) ? "" : dr["addr1"].ToString(),
                                            (dr["ipaddr"] == DBNull.Value) ? "" : dr["ipaddr"].ToString(),  //***EAC Reuse addr2 as the ipaddr field
                                            "","",
                                            (dr["zip5"] == DBNull.Value) ? "" : dr["zip5"].ToString(),
                                            "","");
                                            
                    coll.Add(k);
                }
                return (coll);
            }
        }
        public static bool UpdateDNMRecord(int id, string addr, string zip5, string ipaddr)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbc;
            dbc = db.GetStoredProcCommand("sp_ADMIN_UpdateDNMRecord");
            db.AddInParameter(dbc, "@id", DbType.Int32, id);
            db.AddInParameter(dbc, "@addr", DbType.String, addr);
            db.AddInParameter(dbc, "@zip5", DbType.String, zip5);
            db.AddInParameter(dbc, "@ipaddr", DbType.String, ipaddr);

            try
            {
                db.ExecuteNonQuery(dbc);
                return (true);
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return (false); //***EAC dont care what problem was...
            }

        }
        public static bool DeleteDNMRecord(int id)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbc;
            dbc = db.GetStoredProcCommand("sp_ADMIN_DeleteDNMRecord");
            db.AddInParameter(dbc, "@id", DbType.String, id);
            try
            {
                db.ExecuteNonQuery(dbc);
                return (true);
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return (false); //***EAC dont care what problem was...
            }
        }

        public static bool AddDNMRecord(string addr, string zip5, string ipaddr)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbc;
            dbc = db.GetStoredProcCommand("sp_ADMIN_AddDNMRecord");
            db.AddInParameter(dbc, "@addr", DbType.String, addr);
            db.AddInParameter(dbc, "@zip5", DbType.String, zip5);
            db.AddInParameter(dbc, "@ipaddr", DbType.String, ipaddr);
            

            try
            {
                db.ExecuteNonQuery(dbc);
                return (true);
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return (false); //***EAC dont care what problem was...
            }

        }
        #region CISUser
        public static KVPairCollection GetGuamRoles()
        {
            Database db = DatabaseFactory.CreateDatabase("GuamConn");
            DbCommand cw = db.GetStoredProcCommand("sp_getRoles");

            using (System.Data.IDataReader dr = db.ExecuteReader(cw))
            {
                KVPairCollection coll = new KVPairCollection();
                while (dr.Read())
                {
                    KVPair k = new KVPair(dr["roleid"].ToString(), dr["name"].ToString(), "");
                    coll.Add(k);
                }
                return (coll);
            }
        }
        public static CISUser GetGuamUserById(int userid)
        {
            Database db = DatabaseFactory.CreateDatabase("GuamConn");
            DbCommand cw = db.GetStoredProcCommand("sp_getUserById");
            db.AddInParameter(cw, "@userid", DbType.Int32, userid);
            using (System.Data.IDataReader dr = db.ExecuteReader(cw))
            {
                
                if (dr.Read())
                {
                    CISUser k = new CISUser(dr.GetInt32(dr.GetOrdinal("userid")),
                                            dr["username"].ToString(),
                                            dr["username"].ToString(),
                                            (dr["guamroles"] == DBNull.Value) ? "" : dr["guamroles"].ToString(),
                                            (dr["isactive"] == DBNull.Value) ? "" : dr["isactive"].ToString(),
                                            (dr["isenabled"] == DBNull.Value) ? "" : dr["isenabled"].ToString(),
                                            (dr["islockedout"] == DBNull.Value) ? "" : dr["islockedout"].ToString(),
                                            (dr["ispasswordexpired"] == DBNull.Value) ? "" : dr["ispasswordexpired"].ToString()
                                            );
                    return (k);
                }
                else
                    return (null);
            }
        }
        public static CISUserCollection GetGuamUsers(string s)
        {
            Database db = DatabaseFactory.CreateDatabase("GuamConn");
            DbCommand cw = db.GetStoredProcCommand("sp_getUsers");
            db.AddInParameter(cw, "@s", DbType.AnsiString, s);
            using (System.Data.IDataReader dr = db.ExecuteReader(cw))
            {
                CISUserCollection coll = new CISUserCollection();
                while (dr.Read())
                {
                    CISUser k = new CISUser(dr.GetInt32(dr.GetOrdinal("userid")),
                                            dr["username"].ToString(),
                                            dr["username"].ToString(),
                                            (dr["guamroles"] == DBNull.Value) ? "" : dr["guamroles"].ToString(),
                                            (dr["isactive"] == DBNull.Value) ? "" : dr["isactive"].ToString(),
                                            (dr["isenabled"] == DBNull.Value) ? "" : dr["isenabled"].ToString(),
                                            (dr["islockedout"] == DBNull.Value) ? "" : dr["islockedout"].ToString(),
                                            (dr["ispasswordexpired"] == DBNull.Value) ? "" : dr["ispasswordexpired"].ToString()
                                            );
                    coll.Add(k);
                }
                return (coll);
            }
        }
        public static bool SaveGuamUser(CISUser c)
        {
            Database db = DatabaseFactory.CreateDatabase("GuamConn");
            DbCommand dbc;
            dbc = db.GetStoredProcCommand("sp_saveUserById");
            db.AddInParameter(dbc, "@userid", DbType.String, c.ID);
            db.AddInParameter(dbc, "@role", DbType.String, c.Role);//***EAC assume there is only 1 role selected, otherwise the save wil fail

            try
            {
                db.ExecuteNonQuery(dbc);
                return (true);
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return (false); //***EAC dont care what problem was...
            }

        }
        public static CISUser GetCisUser(string login, int app)
        {
            //CR 11-001-36 Database db = DatabaseFactory.CreateDatabase(PubEntAdminManager.strOracleConnectionInstance);
            //DbCommand cw = db.GetStoredProcCommand(strSP_ADMIN_GetUserbyLoginApp);
            //CR-36 db.AddInParameter(cw, "l", DbType.AnsiString, login);
            //CR-36 db.AddInParameter(cw, "app", DbType.Int32, app);

            //Begin CR-36
            string AuthenticationDB = "1"; //Default (1) is SQL Server, 2 is Oracle
            if (ConfigurationManager.AppSettings["AuthenticateAgainst"] != null)
                AuthenticationDB = ConfigurationManager.AppSettings["AuthenticateAgainst"];

            Database db;
            if (AuthenticationDB == "2")
               db = DatabaseFactory.CreateDatabase(PubEntAdminManager.strOracleConnectionInstance);
            else
               db = DatabaseFactory.CreateDatabase(); //CR-36 Authenticate against SQL Server
            
            DbCommand cw = db.GetStoredProcCommand(strSP_ADMIN_GetUserbyLoginApp);

            if (AuthenticationDB == "2")
            {
                db.AddInParameter(cw, "l", DbType.AnsiString, login);
                db.AddInParameter(cw, "app", DbType.Int32, app);
            }
            else
            {
                db.AddInParameter(cw, "@l", DbType.AnsiString, login);
                db.AddInParameter(cw, "@app", DbType.Int32, app);
            }
            //End CR-36

            CISUser l_c = null;

            using (System.Data.IDataReader dr = ExecuteReader(db, cw))
            {

                while (dr.Read())
                {
                    l_c = new CISUser(System.Convert.ToInt32(dr["IID"]),
                        System.Convert.ToString(dr["login"]),
                        System.Convert.ToString(dr["email"]),
                        System.Convert.ToString(dr["group_name"]),
                        System.Convert.ToString(dr["first_name"]),
                        System.Convert.ToString(dr["regionno"]),
                        System.Convert.ToString(dr["last_name"])
                        );

                }
                return (l_c);
            }
           
        }
        #endregion

        #region Comments
        public static int SetComment(int PubID, int CreatorID,
            string CreatorUsername, string Comment)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_SetNewComment);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);
            db.AddInParameter(dbCommand, "@CreatorID", DbType.Int32, CreatorID);
            db.AddInParameter(dbCommand, "@CreatorUsername", DbType.AnsiString, CreatorUsername);
            db.AddInParameter(dbCommand, "@Comment", DbType.AnsiString, Comment);

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    Int32 pk = db.ExecuteNonQuery(dbCommand);
                    transaction.Commit();

                    return ((1));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    transaction.Rollback();
                    return (0);
                }
            }

        }

        public static CommentCollection GetCommentsByPubID(int PubID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetCommentsByPubId);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);

            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                CommentCollection coll = new CommentCollection();
                while (dr.Read())
                {
                    Comment newMultiSelectItem = new Comment(
                    System.Convert.ToInt32(dr["CommentId"]),
                    System.Convert.ToInt32(dr["PUBID"]),
                    System.Convert.ToString(dr["Comment"]),
                    System.Convert.ToInt32(dr["UserID"]),
                    System.Convert.ToString(dr["CreatorUserName"]),
                    System.Convert.ToDateTime(dr["DateCreated"])
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }

        }
        #endregion

        #region Related Pub
        public static PubCollection GetRelatedPubDisplay(int PubID)
        {
            PubCollection coll = new PubCollection();
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetRelatedProd);
            db.AddInParameter(dbCommand, "@P_PUBID", DbType.Int32, PubID);

            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    Pub newMultiSelectItem = new Pub(
                    System.Convert.ToInt32(dr["PUBID"]),
                    System.Convert.ToString(dr["PRODUCTID"]),
                    System.Convert.ToString(dr["SHORTTITLE"])
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }

        public static int SetRelatedPub(int P_PUBID, string S_ProdID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_SetRelatedProd);
            db.AddInParameter(dbCommand, "@P_PUBID", DbType.Int32, P_PUBID);
            db.AddInParameter(dbCommand, "@S_ProdID", DbType.String, S_ProdID);
            db.AddOutParameter(dbCommand, "@Ret", DbType.Int32, int.MaxValue);

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    db.ExecuteNonQuery(dbCommand);
                    transaction.Commit();

                    return (System.Convert.ToInt32(db.GetParameterValue(dbCommand,"@Ret")));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    transaction.Rollback();
                    return (0);
                }
            }

        }

        public static int DeleteRelatedPub(int P_PUBID, int S_PUBID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_DeleteRelatedPub);
            db.AddInParameter(dbCommand, "@P_PubIdToDelete", DbType.Int32, P_PUBID);
            db.AddInParameter(dbCommand, "@S_PubIdToDelete", DbType.Int32, S_PUBID);

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    Int32 pk = db.ExecuteNonQuery(dbCommand);
                    transaction.Commit();

                    return ((pk));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    transaction.Rollback();
                    return (0);
                }
            }

        }
        #endregion

        #region Translation
        public static PubCollection GetTranslationDisplay(int PubID)
        {
            PubCollection coll = new PubCollection();
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetProdInTranslation);
            db.AddInParameter(dbCommand, "@P_PUBID", DbType.Int32, PubID);

            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    Pub newMultiSelectItem = new Pub(
                    System.Convert.ToInt32(dr["PUBID"]),
                    System.Convert.ToString(dr["PRODUCTID"]),
                    System.Convert.ToString(dr["SHORTTITLE"]),
                    System.Convert.ToString(dr["LANGUAGE"])
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }

        public static int SetRelatedTranslation(int P_PUBID, string S_ProdID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_SetProdInTranslation);
            db.AddInParameter(dbCommand, "@P_PUBID", DbType.Int32, P_PUBID);
            db.AddInParameter(dbCommand, "@S_ProdID", DbType.String, S_ProdID);
            db.AddOutParameter(dbCommand, "@Ret", DbType.Int32, int.MaxValue);

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    db.ExecuteNonQuery(dbCommand);
                    transaction.Commit();

                    return (System.Convert.ToInt32(db.GetParameterValue(dbCommand, "@Ret")));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    transaction.Rollback();
                    return (0);
                }
            }

        }

        public static int DeleteRelatedTranslation(int P_PUBID, int S_PUBID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_DeleteRelatedTranslation);
            db.AddInParameter(dbCommand, "@P_PubIdToDelete", DbType.Int32, P_PUBID);
            db.AddInParameter(dbCommand, "@S_PubIdToDelete", DbType.Int32, S_PUBID);

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    Int32 pk = db.ExecuteNonQuery(dbCommand);
                    transaction.Commit();

                    return ((pk));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    transaction.Rollback();
                    return (0);
                }
            }

        }
        #endregion

        #region Search
        public static List<Pub> AdminDoSearch(
            int ncipl,
	        int roo,
	        int exh,
	        int catalog,
	        string terms,
	        string nih1,
	        string nih2,
            int New,
            int Updated,
            DateTime crfrom,
            DateTime crto,
            string cantype,
	        string subj,
	        string aud,
	        string lang,
	        string format,
	        string serie,
	        string race,
	        string bookstatus,
	        string readlevel,
	        int roocom,
	        string roocomsubj,
	        string award,
            string owner,
            string sponsor,
            string pubids           
        )
        {
            
            List<Pub> coll = new List<Pub>();

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_Search);

            if (ncipl == 0)
                db.AddInParameter(dbCommand, "@ncipl", DbType.Int32, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@ncipl", DbType.Int32, ncipl);

            if (roo == 0)
                db.AddInParameter(dbCommand, "@roo", DbType.Int32, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@roo", DbType.Int32, roo);

            if (exh == 0)
                db.AddInParameter(dbCommand, "@exh", DbType.Int32, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@exh", DbType.Int32, exh);

            if (catalog == 0)
                db.AddInParameter(dbCommand, "@catalog", DbType.Int32, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@catalog", DbType.Int32, catalog);

            if (terms == null)
                db.AddInParameter(dbCommand, "@terms", DbType.AnsiString, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@terms", DbType.AnsiString, terms);

            if (nih1 == null)
                db.AddInParameter(dbCommand, "@nih1", DbType.String, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@nih1", DbType.String, nih1);

            if (nih2 == null)
                db.AddInParameter(dbCommand, "@nih2", DbType.String, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@nih2", DbType.String, nih2);


            if (crfrom == DateTime.MinValue)
                db.AddInParameter(dbCommand, "@crfrom", DbType.DateTime, SqlDateTime.Null);
            else
                db.AddInParameter(dbCommand, "@crfrom", DbType.DateTime, crfrom);

            if (crto == DateTime.MinValue)
                db.AddInParameter(dbCommand, "@crto", DbType.DateTime, SqlDateTime.Null);
            else
                db.AddInParameter(dbCommand, "@crto", DbType.DateTime, crto);

            if (New == 0)
                db.AddInParameter(dbCommand, "@New", DbType.Int32, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@New", DbType.Int32, New);

            if (Updated == 0)
                db.AddInParameter(dbCommand, "@Update", DbType.Int32, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@Update", DbType.Int32, Updated);

            if (cantype == null)
                db.AddInParameter(dbCommand, "@cantype", DbType.String, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@cantype", DbType.String, cantype);

            if (subj == null)
                db.AddInParameter(dbCommand, "@subj", DbType.String, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@subj", DbType.String, subj);

            if (aud == null)
                db.AddInParameter(dbCommand, "@aud", DbType.String, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@aud", DbType.String, aud);

            if (lang == null)
                db.AddInParameter(dbCommand, "@lang", DbType.String, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@lang", DbType.String, lang);

            if (format == null)
                db.AddInParameter(dbCommand, "@format", DbType.String, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@format", DbType.String, format);

            if (serie == null)
                db.AddInParameter(dbCommand, "@serie", DbType.String, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@serie", DbType.String, serie);

            if (race == null)
                db.AddInParameter(dbCommand, "@race", DbType.String, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@race", DbType.String, race);

            if (bookstatus == null)
                db.AddInParameter(dbCommand, "@bookstatus", DbType.String, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@bookstatus", DbType.String, bookstatus);

            if (readlevel == null)
                db.AddInParameter(dbCommand, "@readlevel", DbType.String, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@readlevel", DbType.String, readlevel);

            if (roocom == 0)
                db.AddInParameter(dbCommand, "@roocom", DbType.Int32, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@roocom", DbType.Int32, roocom);

            if (roocomsubj == null)
                db.AddInParameter(dbCommand, "@roocomsubj", DbType.String, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@roocomsubj", DbType.String, roocomsubj);

            if (award == null)
                db.AddInParameter(dbCommand, "@award", DbType.String, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@award", DbType.String, award);

            if (owner == null)
                db.AddInParameter(dbCommand, "@owner", DbType.String, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@owner", DbType.String, owner);
            
            if (sponsor == null)
                db.AddInParameter(dbCommand, "@sponsor", DbType.String, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@sponsor", DbType.String, sponsor);           

            if (pubids == null)
                db.AddInParameter(dbCommand, "@pubids", DbType.String, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@pubids", DbType.String, pubids);
            dbCommand.CommandTimeout = 200;

            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    Pub newMultiSelectItem = new Pub(
                    System.Convert.ToInt32(dr["PUBID"]),
                    System.Convert.ToString(dr["PRODUCTID"]),
                    (dr["LONGTITLE"] != DBNull.Value && dr["LONGTITLE"].ToString().Length>0) ? System.Convert.ToString(dr["LONGTITLE"]) : "No Title Provided",
                    dr["QUANTITY_AVAILABLE"]!=DBNull.Value?System.Convert.ToInt32(dr["QUANTITY_AVAILABLE"]):-1,
                    dr["bookstatus"] != DBNull.Value ? System.Convert.ToString(dr["bookstatus"]) : String.Empty,
                    System.Convert.ToDateTime(dr["RECORDCREATEDATE"]),
                    dr["RANK"]!= DBNull.Value ? System.Convert.ToString(dr["Rank"]) : String.Empty
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }

        public static List<Pub> AdminDoSearchByPubID(
	        string PubIDs,
            string SortBy,
            int SortAsc
        )
        {
            List<Pub> coll = new List<Pub>();

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_SearchFromPubID);
            db.AddInParameter(dbCommand, "@PubIDs", DbType.AnsiString, PubIDs);
            db.AddInParameter(dbCommand, "@SortBy", DbType.String, SortBy);
            db.AddInParameter(dbCommand, "@SortAsc", DbType.Int32, SortAsc);

            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    Pub newMultiSelectItem = new Pub(
                    System.Convert.ToInt32(dr["PUBID"]),
                    System.Convert.ToString(dr["PRODUCTID"]),
                    (dr["LONGTITLE"] != DBNull.Value && dr["LONGTITLE"].ToString().Length > 0) ? System.Convert.ToString(dr["LONGTITLE"]) : "No Title Provided",
                    dr["QUANTITY_AVAILABLE"] != DBNull.Value ? System.Convert.ToInt32(dr["QUANTITY_AVAILABLE"]) : -1,
                    dr["bookstatus"] != DBNull.Value ? System.Convert.ToString(dr["bookstatus"]) : String.Empty,
                    System.Convert.ToDateTime(dr["RECORDCREATEDATE"]),dr["Rank"] != DBNull.Value ? System.Convert.ToString(dr["Rank"]) : String.Empty
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }
        
        #endregion

        #region Announcement

        public static List<PubEntAdmin.BLL.Announcement> GetAnnouncement(bool active)
        {
            List<PubEntAdmin.BLL.Announcement> coll = new List<PubEntAdmin.BLL.Announcement>();
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetAllAnnouncement);
            db.AddInParameter(dbCommand, "@Active", DbType.Int32, System.Convert.ToInt32(active));

            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    PubEntAdmin.BLL.Announcement newMultiSelectItem =
                        new PubEntAdmin.BLL.Announcement(System.Convert.ToInt32(dr["ANNOUNCEMENTID"]),
                        System.Convert.ToString(dr["ANNOUNCEMENT_DESC"]),
                        System.Convert.ToString(dr["ANNOUNCEMENT_URL"]),
                        System.Convert.ToDateTime(dr["S_DATE"]),
                        System.Convert.ToDateTime(dr["E_DATE"])
                        );
                    coll.Add(newMultiSelectItem);
                }
            }
            return (coll);
        }

        public static bool SetAnnouncement(ref int ANNOUNCEMENTID, string ANNOUNCEMENT_DESC, string ANNOUNCEMENT_URL,
            DateTime S_DATE, DateTime E_DATE, int ACTIVE_FLAG)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_SetAnnouncement);

            db.AddParameter(dbCommand, "@ANNOUNCEMENTID", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, ANNOUNCEMENTID);

            //db.AddInParameter(dbCommand, "@ANNOUNCEMENTID", DbType.Int32, ANNOUNCEMENTID);
            db.AddInParameter(dbCommand, "@ANNOUNCEMENT_DESC", DbType.AnsiString, ANNOUNCEMENT_DESC);
            db.AddInParameter(dbCommand, "@ANNOUNCEMENT_URL", DbType.AnsiString, ANNOUNCEMENT_URL);
            db.AddInParameter(dbCommand, "@S_DATE", DbType.DateTime, S_DATE);
            db.AddInParameter(dbCommand, "@E_DATE", DbType.DateTime, E_DATE);
            db.AddInParameter(dbCommand, "@ENABLED", DbType.Int32, ACTIVE_FLAG);

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    Int32 pk = db.ExecuteNonQuery(dbCommand);
                    transaction.Commit();
                    ANNOUNCEMENTID = (int)db.GetParameterValue(dbCommand, "@ANNOUNCEMENTID");

                    if (pk != -1)
                        return true;
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    transaction.Rollback();
                    return (false);
                }
            }

        }
        #endregion

        #region VK LP

        public static List<string> GetProdInt(string ProdID, int IsVK)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetProdInt);
            db.AddInParameter(dbCommand, "@ProdID", DbType.String, (ProdID));
            db.AddInParameter(dbCommand, "@IsVK", DbType.Int32, (IsVK));
            List<string> coll = new List<string>();

            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    coll.Add(dr["ExistInKit"].ToString());
                    coll.Add(dr["NCIPL CONFIG"].ToString());
                    coll.Add(dr["ROO CONFIG"].ToString());
                    coll.Add(dr["NCIPL"].ToString());
                    coll.Add(dr["ROO"].ToString());
                }
            }

            return coll;
        }
        
        public static List<Pub> GetALLVK_LP(bool IsVK)
        {
            List<Pub> coll = new List<Pub>();

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetAllVK_LP);
            db.AddInParameter(dbCommand, "@IsVK", DbType.Int32, System.Convert.ToBoolean(IsVK));

            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    PubEntAdmin.BLL.Pub newMultiSelectItem =
                        new PubEntAdmin.BLL.Pub(System.Convert.ToInt32(dr["KITID"]),
                        System.Convert.ToString(dr["PRODUCTID"]),
                        System.Convert.ToString(dr["SHORTTITLE"]),
                        dr["INTERFACE"].ToString() == "NCIPL" ? true : false, dr["INTERFACE"].ToString() == "ROO" ? true : false, IsVK
                        );
                    coll.Add(newMultiSelectItem);
                }
            }
            return (coll);
        }

        public static int DeleteVK_LP(int id, string _interface, int IsVK)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_DeleteVK_LP);
            db.AddInParameter(dbCommand, "@id", DbType.Int32, id);
            db.AddInParameter(dbCommand, "@interface", DbType.String, _interface);
            db.AddInParameter(dbCommand, "@IsVK", DbType.Int32, IsVK);
            return db.ExecuteNonQuery(dbCommand);
        }

        public static int SetNewKITPUB(string PRODID, int PubID, int NCIPL, int ROO,
            int NCIPL_QTY, int ROO_QTY, int VIRTUAL)
        {

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_SetNewKITPUB);

            db.AddInParameter(dbCommand, "@PRODID", DbType.String, PRODID);
            db.AddParameter(dbCommand, "@PUBID", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, PubID);

            if (NCIPL == 0)
                db.AddInParameter(dbCommand, "@NCIPL", DbType.Int32, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@NCIPL", DbType.Int32, @NCIPL);

            if (ROO == 0)
                db.AddInParameter(dbCommand, "@ROO", DbType.Int32, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@ROO", DbType.Int32, ROO);

            if (NCIPL_QTY == -1)
                db.AddInParameter(dbCommand, "@NCIPL_QTY", DbType.Int32, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@NCIPL_QTY", DbType.Int32, NCIPL_QTY);

            if (ROO_QTY == -1)
                db.AddInParameter(dbCommand, "@ROO_QTY", DbType.Int32, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@ROO_QTY", DbType.Int32, ROO_QTY);

            if (VIRTUAL == 0)
                db.AddInParameter(dbCommand, "@VIRTUAL", DbType.Int32, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@VIRTUAL", DbType.Int32, VIRTUAL);
            

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    Int32 pk = db.ExecuteNonQuery(dbCommand);
                    transaction.Commit();

                    if (pk != PubID && pk != -1)
                        return (int)db.GetParameterValue(dbCommand, "@PUBID");
                    else
                        return pk;
                    
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    transaction.Rollback();
                    return (-1);
                }
            }

        }

        public static bool SetKITPUB(int KITID, int PUBID, int NCIPL, int ROO,
            int NCIPL_QTY, int ROO_QTY, int VIRTUAL)
        {

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_SetKITPUB);

            db.AddInParameter(dbCommand, "@KITID", DbType.Int32, KITID);
            db.AddInParameter(dbCommand, "@PUBID", DbType.Int32, KITID);

            if (NCIPL == 0)
                db.AddInParameter(dbCommand, "@NCIPL", DbType.Int32, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@NCIPL", DbType.Int32, @NCIPL);

            if (ROO == 0)
                db.AddInParameter(dbCommand, "@ROO", DbType.Int32, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@ROO", DbType.Int32, ROO);

            if (NCIPL_QTY == -1)
                db.AddInParameter(dbCommand, "@NCIPL_QTY", DbType.Int32, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@NCIPL_QTY", DbType.Int32, NCIPL_QTY);

            if (ROO_QTY == -1)
                db.AddInParameter(dbCommand, "@ROO_QTY", DbType.Int32, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@ROO_QTY", DbType.Int32, ROO_QTY);

            if (VIRTUAL == 0)
                db.AddInParameter(dbCommand, "@VIRTUAL", DbType.Int32, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@VIRTUAL", DbType.Int32, VIRTUAL);
            

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    Int32 pk = db.ExecuteNonQuery(dbCommand);
                    transaction.Commit();

                    return (System.Convert.ToBoolean(pk));
                    
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    transaction.Rollback();
                    return (false);
                }
            }

        }

        public static bool SetKITPUBs(int KITID, string PUBID_QTYs, int NCIPL, int ROO,
            int VIRTUAL, string pairDelim, string indDelim)
        {

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_SetKITPUBs);

            db.AddInParameter(dbCommand, "@KITID", DbType.Int32, KITID);
            db.AddInParameter(dbCommand, "@PUBID_QTYs", DbType.String, PUBID_QTYs);

            if (NCIPL == 0)
                db.AddInParameter(dbCommand, "@NCIPL", DbType.Int32, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@NCIPL", DbType.Int32, NCIPL);

            if (ROO == 0)
                db.AddInParameter(dbCommand, "@ROO", DbType.Int32, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@ROO", DbType.Int32, ROO);

            if (VIRTUAL == 0)
                db.AddInParameter(dbCommand, "@VIRTUAL", DbType.Int32, DBNull.Value);
            else
                db.AddInParameter(dbCommand, "@VIRTUAL", DbType.Int32, VIRTUAL);

            db.AddInParameter(dbCommand, "@pairDelim", DbType.String, pairDelim);
            db.AddInParameter(dbCommand, "@indDelim", DbType.String, indDelim);

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    Int32 pk = db.ExecuteNonQuery(dbCommand);
                    transaction.Commit();

                    return (System.Convert.ToBoolean(pk));
                    
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    transaction.Rollback();
                    return (false);
                }
            }

        }
        

        public static List<Pub> GetPubDisplay(int KITID, string Interface)
        {
            List<Pub> coll = new List<Pub>();
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetLnkPub);
            db.AddInParameter(dbCommand, "@KitID", DbType.Int32, KITID);
            db.AddInParameter(dbCommand, "@Interface", DbType.String, Interface);

            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    PubEntAdmin.BLL.Pub newMultiSelectItem =
                        new PubEntAdmin.BLL.Pub(System.Convert.ToInt32(dr["PUBID"]),
                        System.Convert.ToString(dr["PRODUCTID"]),
                        System.Convert.ToString(dr["SHORTTITLE"]),
                        System.Convert.ToInt32(dr["QTY"])
                        );
                    coll.Add(newMultiSelectItem);
                }
            }
            return (coll);
        }

        public static List<Pub> GetKitDisplay(int KITID, string Interface)
        {
            List<Pub> coll = new List<Pub>();
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetKitPub);
            db.AddInParameter(dbCommand, "@KitID", DbType.Int32, KITID);
            db.AddInParameter(dbCommand, "@Interface", DbType.String, Interface);

            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    PubEntAdmin.BLL.Pub newMultiSelectItem =
                        new PubEntAdmin.BLL.Pub(System.Convert.ToInt32(dr["PUBID"]),
                        System.Convert.ToString(dr["PRODUCTID"]),
                        System.Convert.ToString(dr["SHORTTITLE"]),
                        System.Convert.ToInt32(dr["QTY"])
                        );
                    coll.Add(newMultiSelectItem);
                }
            }
            return (coll);
        }
        
        
        #endregion

        #region Pub


        public static Pub GetPubInfoByPubID(int PubID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetPubInfoByPubID);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);


            Pub newMultiSelectItem = null;

            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                
                while (dr.Read())
                {
                    newMultiSelectItem = new Pub(System.Convert.ToInt32(dr["PUBID"]),
                    System.Convert.ToString(dr["PRODUCTID"]),
                    System.Convert.ToString(dr["SHORTTITLE"])
                    );
                    
                }
                return (newMultiSelectItem);
            }
            
        }

        public static Pub GetPubInfoByProdID(string ProdID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetPubInfoByProdID);
            db.AddInParameter(dbCommand, "@ProdID", DbType.String, ProdID);


            Pub newMultiSelectItem = null;

            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {

                while (dr.Read())
                {
                    newMultiSelectItem = new Pub(System.Convert.ToInt32(dr["PUBID"]),
                    System.Convert.ToString(dr["PRODUCTID"]),
                    System.Convert.ToString(dr["SHORTTITLE"])
                    );

                }
                return (newMultiSelectItem);
            }

        }
        #endregion

        #region Owners
        public static OwnerCollection GetAllOwners(bool active)
        {
            OwnerCollection coll = new OwnerCollection();

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_SP_ADMIN_GetAllOwners);
            db.AddInParameter(dbCommand, "@Active", DbType.Int32, System.Convert.ToInt32(active));
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    Owner newMultiSelectItem = new Owner(
                        System.Convert.ToInt32(dr["OWNERID"].ToString()),
                        dr["OwnerFull_Name"].ToString()
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }

        public static bool SetOwnerByPubID(int PubID, int OwnerID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_SetOwnerByPubID);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);
            db.AddInParameter(dbCommand, "@OwnerID", DbType.Int32, OwnerID);      


            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    Int32 pk = db.ExecuteNonQuery(dbCommand);
                    transaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    transaction.Rollback();
                    return (false);
                }
            }

        }

        public static Boolean GetOwnerStatusByOwnerID(int OwnerID)
        {
            bool blnActive=false;

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetOwnerStatusbyOwnerID);
            db.AddInParameter(dbCommand, "@OwnerID", DbType.Int32, OwnerID);

            DataSet ds = db.ExecuteDataSet(dbCommand);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int OStatus = System.Convert.ToInt32(dr.ItemArray[0]);
                    blnActive = Convert.ToBoolean(OStatus);
                }
            }           
            return blnActive;
        }

        public static int DeleteOwnerByPubID(int PubID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_DeleteOwnerByPubid);
            db.AddInParameter(dbCommand, "@pubid", DbType.Int32, PubID);

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    Int32 pk = db.ExecuteNonQuery(dbCommand);
                    transaction.Commit();

                    return ((pk));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    transaction.Rollback();
                    return (0);
                }
            }

        }
        
        #endregion

        #region Sponsor
        public static SponsorCollection GetAllSponsors(bool active)
        {
            SponsorCollection coll = new SponsorCollection();

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_SP_ADMIN_GetAllSponsors);
            db.AddInParameter(dbCommand, "@Active", DbType.Int32, System.Convert.ToInt32(active));
            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    Sponsor newMultiSelectItem = new Sponsor(
                        System.Convert.ToInt32(dr["SPONSORID"].ToString()),
                        dr["Description"].ToString()
                    );
                    coll.Add(newMultiSelectItem);
                }
                return (coll);
            }
        }

        public static bool SetSponsorByPubID(int PubID, int SponsorID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_SetSponsorByPubID);
            db.AddInParameter(dbCommand, "@PubID", DbType.Int32, PubID);
            db.AddInParameter(dbCommand, "@SponsorID", DbType.Int32, SponsorID);


            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    Int32 pk = db.ExecuteNonQuery(dbCommand);
                    transaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    transaction.Rollback();
                    return (false);
                }
            }

        }

        public static Boolean GetSponsorStatusByOwnerID(int SponsorID)
        {
            bool blnActive = false;

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetSponsorStatusbyOwnerID);
            db.AddInParameter(dbCommand, "@SponsorID", DbType.Int32, SponsorID);

            DataSet ds = db.ExecuteDataSet(dbCommand);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int SpStatus = System.Convert.ToInt32(dr.ItemArray[0]);
                    blnActive = Convert.ToBoolean(SpStatus);
                }
            }
            return blnActive;
        }

        public static int DeleteSponsorByPubID(int PubID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_DeleteSponsorByPubid);
            db.AddInParameter(dbCommand, "@pubid", DbType.Int32, PubID);

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    Int32 pk = db.ExecuteNonQuery(dbCommand);
                    transaction.Commit();

                    return ((pk));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    transaction.Rollback();
                    return (0);
                }
            }

        }        

        #endregion
        

        #endregion

        #region Execution methods
        private static System.Data.IDataReader ExecuteReader(Database db, DbCommand dbCmd)
        {
            if (db == null) throw (new ArgumentNullException("db"));
            if (dbCmd == null) throw (new ArgumentNullException("dbCmd"));

            return (db.ExecuteReader(dbCmd));
            
        }

        private static System.Data.DataSet ExecutedataSet(Database db, DbCommand dbCmd)
        {
            if (db == null) throw (new ArgumentNullException("db"));
            if (dbCmd == null) throw (new ArgumentNullException("dbCmd"));

            return (db.ExecuteDataSet(dbCmd));
        }

        #endregion

        //public static List<PubEntAdmin.BLL.Owner> GetOwners()
        //{
        //    List<Owner> owners = new List<Owner>();
        //    //  load up the xml document
        //    XDocument xDoc = XDocument.Load(HttpContext.Current.Server.MapPath(@"Xml/Owner.xml"));

        //    //  populate the customer collection
        //    owners =
        //    (
        //        from c in xDoc.Descendants("owner")
        //        orderby c.Attribute("OwnerLongName").Value

        //        select new Owner
        //        {
        //            OwnerCode = c.Attribute("OwnerCode").Value,
        //            OwnerShortName = c.Attribute("OwnerShortName").Value,
        //            OwnerLongName = c.Attribute("OwnerLongName").Value,
        //            OwnerRecType = c.Attribute("OwnerRecType").Value
        //        }
        //    ).ToList();

        //    return owners;
        //}

        //public static List<PubEntAdmin.BLL.Sponsor> GetSponsors()
        //{
        //    List<Sponsor> sponsors = new List<Sponsor>();
        //    //  load up the xml document
        //    XDocument xDoc = XDocument.Load(HttpContext.Current.Server.MapPath(@"Xml/Sponsor.xml"));

        //    //  populate the customer collection
        //    sponsors =
        //    (
        //        from c in xDoc.Descendants("Sponsor")
        //        orderby c.Attribute("SponsorDesp").Value

        //        select new Sponsor
        //        {
        //            SponsorDesp12 = c.Attribute("SponsorDesp12").Value,
        //            SponsorDesp7 = c.Attribute("SponsorDesp7").Value,
        //            SponsorDesp = c.Attribute("SponsorDesp").Value,
        //            SponsorCode = c.Attribute("SponsorCode").Value
        //        }
        //    ).ToList();

        //    return sponsors;
        //}

        #region TypeOfCustomers(For ROO)
        public static MultiSelectListBoxItemCollection GetAllTypeofCustomers()
        {
            MultiSelectListBoxItemCollection MultiSelectColl = new MultiSelectListBoxItemCollection();
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetAllTypeofCustomers);

            //db.AddInParameter(dbCommand, "@Interface", DbType.String, Interface);

            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    MultiSelectListBoxItem newMultiSelectItem = new MultiSelectListBoxItem(
                        System.Convert.ToInt32(dr["CUSTOMERID"].ToString()),
                        dr["CUSTOMERTYPE"].ToString(),
                        System.Convert.ToBoolean(dr["ACTIVE_FLAG"])
                    );
                    MultiSelectColl.Add(newMultiSelectItem);
                }
            }
            return MultiSelectColl;

        }
        public static bool SetTypeofCustomer(int CustId, string UpdateFlag, //Flag can be 'A','U','D', ACTION
                                            string custType, int Active, int cc, int pos, int lm)
        {
            int output = -1;
            Database Db = DatabaseFactory.CreateDatabase();
            DbCommand Dbcommand = Db.GetStoredProcCommand(strSP_ADMIN_SetTypeofCustomer);

            Db.AddInParameter(Dbcommand, "@updateflag", DbType.String, UpdateFlag);

            if (UpdateFlag != "A")
                Db.AddInParameter(Dbcommand, "@custid", DbType.Int32, CustId);
            if (UpdateFlag == "ACTION")
                Db.AddInParameter(Dbcommand, "@active_flag", DbType.Int32, Active);
            if (UpdateFlag == "U")
                Db.AddInParameter(Dbcommand, "@custtype", DbType.String, custType);
            if (UpdateFlag == "A")
            {
                Db.AddInParameter(Dbcommand, "@active_flag", DbType.Int32, Active);
                Db.AddInParameter(Dbcommand, "@custtype", DbType.String, custType);
            }
            Db.AddInParameter(Dbcommand, "@cc", DbType.Int32, cc);
            Db.AddInParameter(Dbcommand, "@pos", DbType.Int32, pos);
            Db.AddInParameter(Dbcommand, "@lm", DbType.Int32, lm);
            using (DbConnection dbConn = Db.CreateConnection())
            {
                dbConn.Open();
                DbTransaction dbTrans = dbConn.BeginTransaction();
                try{
                    output = Db.ExecuteNonQuery(Dbcommand);
                    dbTrans.Commit();
                }catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    dbTrans.Rollback();
                }
            }
            if (output >= 0)
                return (true);
            else
                return (false);
        }
        #endregion

        #region OrderMedia (For ROO)
        public static MultiSelectListBoxItemCollection GetAllOrderMedia()
        {
            MultiSelectListBoxItemCollection MultiSelectColl = new MultiSelectListBoxItemCollection();
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(strSP_ADMIN_GetAllOrderMedia);

            //db.AddInParameter(dbCommand, "@Interface", DbType.String, Interface);

            using (System.Data.IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    MultiSelectListBoxItem newMultiSelectItem = new MultiSelectListBoxItem(
                        System.Convert.ToInt32(dr["MEDIAID"].ToString()),
                        dr["MEDIADESC"].ToString(),
                        System.Convert.ToBoolean(dr["ACTIVE_FLAG"])
                    );
                    MultiSelectColl.Add(newMultiSelectItem);
                }
            }
            return MultiSelectColl;

        }
        public static bool SetOrderMedia(int MediaId, string UpdateFlag, //Flag can be 'A','U','D', ACTION
                                            string MediaDesc, int Active, int cc, int pos, int lm)
        {
            int output = -1;
            Database Db = DatabaseFactory.CreateDatabase();
            DbCommand Dbcommand = Db.GetStoredProcCommand(strSP_ADMIN_SetOrderMedia);

            Db.AddInParameter(Dbcommand, "@updateflag", DbType.String, UpdateFlag);

            if (UpdateFlag != "A")
                Db.AddInParameter(Dbcommand, "@mediaid", DbType.Int32, MediaId);
            if (UpdateFlag == "ACTION")
                Db.AddInParameter(Dbcommand, "@active_flag", DbType.Int32, Active);
            if (UpdateFlag == "U")
                Db.AddInParameter(Dbcommand, "@mediadesc", DbType.String, MediaDesc);
            if (UpdateFlag == "A")
            {
                Db.AddInParameter(Dbcommand, "@active_flag", DbType.Int32, Active);
                Db.AddInParameter(Dbcommand, "@mediadesc", DbType.String, MediaDesc);
            }
            Db.AddInParameter(Dbcommand, "@cc", DbType.Int32, cc);
            Db.AddInParameter(Dbcommand, "@pos", DbType.Int32, pos);
            Db.AddInParameter(Dbcommand, "@lm", DbType.Int32, lm);
            using (DbConnection dbConn = Db.CreateConnection())
            {
                dbConn.Open();
                DbTransaction dbTrans = dbConn.BeginTransaction();
                try
                {
                    output = Db.ExecuteNonQuery(Dbcommand);
                    dbTrans.Commit();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    dbTrans.Rollback();
                }
            }
            if (output >= 0)
                return (true);
            else
                return (false);
        }
        #endregion
        public static OrderCollection GetRepeatOrders(string s)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand("sp_admin_GetRepeatOrders");
            
            using (System.Data.IDataReader dr = db.ExecuteReader(cw))
            {
                OrderCollection coll = new OrderCollection();
                while (dr.Read())
                {
                    Person shpto = new Person(0, dr["shiptoname"].ToString(), dr["shiptoorgname"].ToString(), dr["shiptoemail"].ToString(), dr["shiptoaddr1"].ToString(), dr["shiptoaddr2"].ToString(), dr["shiptocity"].ToString(), dr["shiptostate"].ToString(), dr["shiptozip"].ToString(), dr["shiptozip4"].ToString(), dr["shiptophone"].ToString());
                    Person bilto = new Person(0, dr["billtoname"].ToString(), dr["billtoorgname"].ToString(), dr["billtoemail"].ToString(), dr["billtoaddr1"].ToString(), dr["billtoaddr2"].ToString(), dr["billtocity"].ToString(), dr["billtostate"].ToString(), dr["billtozip"].ToString(), dr["billtozip4"].ToString(), dr["billtophone"].ToString());

                    Order k = new Order(dr.GetInt32(dr.GetOrdinal("orderseqnum")), shpto, bilto, DateTime.Parse(dr["createdate"].ToString()), dr["termcode"].ToString(), dr.GetInt32(dr.GetOrdinal("repeatid")), dr["ordercomment"].ToString(), dr["shipmethod"].ToString());
                    coll.Add(k);
                }
                return (coll);

            }
        }
        public static OrderCollection GetPrankOrders(string s)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand("sp_admin_GetPrankOrders");

            using (System.Data.IDataReader dr = db.ExecuteReader(cw))
            {
                OrderCollection coll = new OrderCollection();
                while (dr.Read())
                {
                    Person shpto = new Person(0, dr["shiptoname"].ToString(), dr["shiptoorgname"].ToString(), dr["shiptoemail"].ToString(), dr["shiptoaddr1"].ToString(), dr["shiptoaddr2"].ToString(), dr["shiptocity"].ToString(), dr["shiptostate"].ToString(), dr["shiptozip"].ToString(), dr["shiptozip4"].ToString(), dr["shiptophone"].ToString());
                    Person bilto = new Person(0, dr["billtoname"].ToString(), dr["billtoorgname"].ToString(), dr["billtoemail"].ToString(), dr["billtoaddr1"].ToString(), dr["billtoaddr2"].ToString(), dr["billtocity"].ToString(), dr["billtostate"].ToString(), dr["billtozip"].ToString(), dr["billtozip4"].ToString(), dr["billtophone"].ToString());

                    Order k = new Order(dr.GetInt32(dr.GetOrdinal("orderseqnum")), shpto, bilto, DateTime.Parse(dr["createdate"].ToString()), dr["termcode"].ToString(), dr.GetInt32(dr.GetOrdinal("repeatid")), dr["ordercomment"].ToString(), dr["shipmethod"].ToString());
                    coll.Add(k);
                }
                return (coll);

            }
        }
        public static Order GetOrderByOrderID(int orderid)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand("sp_ADMIN_getOrderByOrderid");
            db.AddInParameter(cw, "@orderid", DbType.Int32, orderid);
            using (System.Data.IDataReader dr = db.ExecuteReader(cw))
            {
                OrderCollection coll = new OrderCollection();
                if (dr.Read())
                {
                    Person shpto = new Person(0, dr["shiptoname"].ToString(), dr["shiptoorgname"].ToString(), dr["shiptoemail"].ToString(), dr["shiptoaddr1"].ToString(), dr["shiptoaddr2"].ToString(), dr["shiptocity"].ToString(), dr["shiptostate"].ToString(), dr["shiptozip"].ToString(), dr["shiptozip4"].ToString(), dr["shiptophone"].ToString());
                    Person bilto = new Person();

                    Order k = new Order(dr.GetInt32(dr.GetOrdinal("orderseqnum")), shpto, bilto, DateTime.Parse(dr["createdate"].ToString()), dr["termcode"].ToString(), dr.GetInt32(dr.GetOrdinal("repeatid")), dr["ordercomment"].ToString(), dr["shipmethod"].ToString());
                    return (k);
                }
                else
                    return (null);

            }
        }
        public static void DeleteOrder(int orderid, string who, out string returnmsg)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand("sp_ADMIN_DeleteOrder");
            db.AddInParameter(cw, "@orderid", DbType.Int32, orderid);
            db.AddInParameter(cw, "@who", DbType.String, who);
            db.AddOutParameter(cw, "msg", DbType.String, 100);
            db.ExecuteNonQuery(cw);
            returnmsg = (string)db.GetParameterValue(cw, "@msg"); //Get Output variable value from SQL Server

        }
        public static void ReleaseOrder(int orderid, string who, out string returnmsg)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand("sp_ADMIN_ReleaseOrder");
            db.AddInParameter(cw, "@orderid", DbType.Int32, orderid);
            db.AddInParameter(cw, "@who", DbType.String, who);
            db.AddOutParameter(cw, "msg", DbType.String, 100);
            db.ExecuteNonQuery(cw);
            returnmsg = (string)db.GetParameterValue(cw, "@msg"); //Get Output variable value from SQL Server
        }
        public static void MarkOrderBad(int orderid, string who, string reason, out string returnmsg)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cw = db.GetStoredProcCommand("sp_ADMIN_MarkOrderBad");
            db.AddInParameter(cw, "@orderid", DbType.Int32, orderid);
            db.AddInParameter(cw, "@who", DbType.String, who);
            db.AddInParameter(cw, "@reason", DbType.String, reason);
            db.AddOutParameter(cw, "msg", DbType.String, 100);
            db.ExecuteNonQuery(cw);
            returnmsg = (string)db.GetParameterValue(cw, "@msg"); //Get Output variable value from SQL Server

        }
        public static ProductCollection GetOrderDetails(int orderid)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("sp_ADMIN_GetOrderDetails");
            db.AddInParameter(dbCommand, "orderid", DbType.Int32, orderid);

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
        public static int GetHoldOrderCount(string startdt, string enddt)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("sp_admin_GetHoldOrderCount");
            db.AddInParameter(dbCommand, "@startdt", DbType.DateTime, DateTime.Parse(startdt));
            db.AddInParameter(dbCommand, "@enddt", DbType.DateTime, DateTime.Parse(enddt));
            db.AddOutParameter(dbCommand, "@retval", DbType.Int32, Int32.MaxValue);

            db.ExecuteNonQuery(dbCommand);

            return ((int)db.GetParameterValue(dbCommand, "@retval"));
        }
        public static int GetHoldDeletedCount(string startdt, string enddt)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("sp_admin_GetHoldDeletedCount");
            db.AddInParameter(dbCommand, "@startdt", DbType.DateTime, DateTime.Parse(startdt));
            db.AddInParameter(dbCommand, "@enddt", DbType.DateTime, DateTime.Parse(enddt));
            db.AddOutParameter(dbCommand, "@retval", DbType.Int32, Int32.MaxValue);

            db.ExecuteNonQuery(dbCommand);

            return ((int)db.GetParameterValue(dbCommand, "@retval"));
        }
        public static int GetHoldReleasedCount(string startdt, string enddt)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("sp_admin_GetHoldReleasedCount");
            db.AddInParameter(dbCommand, "@startdt", DbType.DateTime, DateTime.Parse(startdt));
            db.AddInParameter(dbCommand, "@enddt", DbType.DateTime, DateTime.Parse(enddt));
            db.AddOutParameter(dbCommand, "@retval", DbType.Int32, Int32.MaxValue);

            db.ExecuteNonQuery(dbCommand);

            return ((int)db.GetParameterValue(dbCommand, "@retval"));
        }
        public static int GetHoldDeletedPubCount(string startdt, string enddt)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("sp_admin_GetHoldDeletedPubCount");
            db.AddInParameter(dbCommand, "@startdt", DbType.DateTime, DateTime.Parse(startdt));
            db.AddInParameter(dbCommand, "@enddt", DbType.DateTime, DateTime.Parse(enddt));
            db.AddOutParameter(dbCommand, "@retval", DbType.Int32, Int32.MaxValue);

            db.ExecuteNonQuery(dbCommand);

            return ((int)db.GetParameterValue(dbCommand, "@retval"));
        }
        public static int GetTocStatusByRole(int id, string role)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("sp_admin_GetTocStatusByRole");
            db.AddInParameter(dbCommand, "@id", DbType.Int32, id);
            db.AddInParameter(dbCommand, "@role", DbType.String, role);
            db.AddOutParameter(dbCommand, "@retval", DbType.Int32, Int32.MaxValue);

            db.ExecuteNonQuery(dbCommand);

            return ((int)db.GetParameterValue(dbCommand, "@retval"));
        }
        public static int GetMediaStatusByRole(int id, string role)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("sp_admin_GetMediaStatusByRole");
            db.AddInParameter(dbCommand, "@id", DbType.Int32, id);
            db.AddInParameter(dbCommand, "@role", DbType.String, role);
            db.AddOutParameter(dbCommand, "@retval", DbType.Int32, Int32.MaxValue);

            db.ExecuteNonQuery(dbCommand);

            return ((int)db.GetParameterValue(dbCommand, "@retval"));
        }
    }
}
