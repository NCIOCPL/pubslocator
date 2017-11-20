using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using PubEntAdmin.DAL;
using PubEntAdmin.BLL;

namespace PubEntAdmin.UserControl
{
    public partial class ExhTabEditInfo : System.Web.UI.UserControl
    {
        #region Fields
        private int intPubID;
        private bool bnlExh;
        #endregion

        #region Events Handling
        protected void Page_Load(object sender, EventArgs e)
        {           

            this.BindOption();
            this.BindValues();
            this.SecVal();            
            this.BindConferences();

            if (Session[PubEntAdminManager.strTabContentPrevActTabIndex] == null)
            {
                this.RegisterMonitoredChanges();
                Session[PubEntAdminManager.strTabContentPrevActTabIndex] =
                    Session[PubEntAdminManager.strTabContentCurrActTabIndex];
            }
            else
            {
                if (Session[PubEntAdminManager.strTabContentPrevActTabIndex] !=
                    Session[PubEntAdminManager.strTabContentCurrActTabIndex])
                {
                    this.RegisterMonitoredChanges();
                    Session[PubEntAdminManager.strTabContentPrevActTabIndex] =
                    Session[PubEntAdminManager.strTabContentCurrActTabIndex];
                }
            }
        }
        #endregion

        #region Methods
        protected void BindOption()
        {
            this.rdbtnListYesNoEveryOrder.DataBind();
            this.rdbtnListYesNoShowInSearchRes.DataBind();
            this.ckboxListDisplayStatusExh.DataBind();


            this.listConf.DataSource = PE_DAL.GetAllConference(true, true);
            this.listConf.DataTextField = "name";
            this.listConf.DataValueField = "id";
            this.listConf.DataBind();


        }

        protected void BindValues()
        {
            //If the mode is add, apply the following;
            //else retrieve from DB
            if (Session[PubEntAdminManager.strPubGlobalMode] != null)
            {
                if (Session[PubEntAdminManager.strPubGlobalMode].ToString() == PubEntAdminManager.strPubGlobalAMode)//add
                {
                    this.rdbtnListYesNoEveryOrder.No = true;
                    this.rdbtnListYesNoShowInSearchRes.Yes = true;
                    //this.ckboxListDisplayStatusExh.IsOrder = true;
                }
                else
                {
                    MultiSelectListBoxItemCollection rcoll = PE_DAL.GetExhDisplayStatusByPubID(this.PubID);
                    foreach (DisplayStatus p in rcoll)
                    {
                        ListItem matchItem = this.ckboxListDisplayStatusExh.Items.FindByValue(p.DisplayStatusID.ToString());
                        if (matchItem != null)
                        {
                            matchItem.Selected = true;
                        }
                    }

                    ExhCollection l = PE_DAL.GetExhInterface(this.PubID);
                    if (l.Count > 0)
                    {
                        Exh l_Exh = l[0];

                        if (l_Exh.EVERYORDER_EXHIBIT > 0)
                            this.rdbtnListYesNoEveryOrder.Yes = true;
                        else if (l_Exh.EVERYORDER_EXHIBIT == 0)
                            this.rdbtnListYesNoEveryOrder.No = true;

                        //if (!this.rdbtnListYesNoEveryOrder.Selected())
                        //{
                        //    this.rdbtnListYesNoEveryOrder.No = true;
                        //    this.rdbtnListYesNoShowInSearchRes.Yes = true;
                        //}

                        if (l_Exh.ISSEARCHABLE_EXHIBIT > 0)
                            this.rdbtnListYesNoShowInSearchRes.Yes = true;
                        else if (l_Exh.ISSEARCHABLE_EXHIBIT == 0)
                            this.rdbtnListYesNoShowInSearchRes.No = true;

                        //this.rdbtnListYesNoEveryOrder.Yes = l_Exh.EVERYORDER_EXHIBIT ? true : false;
                        //this.rdbtnListYesNoShowInSearchRes.Yes = l_Exh.ISSEARCHABLE_EXHIBIT ? true : false;

                        this.txtMaxQtyExh.Text = l_Exh.MAXQTY_EXHIBIT.ToString();
                        this.txtMaxQtyIntl.Text = l_Exh.MAXINTL_EXHIBIT.ToString();

                    }

                    rcoll = PE_DAL.GetKioskConfByPubID(this.PubID);

                    this.listSeledConf.DataSource = rcoll;
                    this.listSeledConf.DataTextField = "name";
                    this.listSeledConf.DataValueField = "id";
                    this.listSeledConf.DataBind();

                    
                    MultiSelectListBoxItemCollection  lstConfSource= this.listConf.DataSource;
                    foreach (PubEntAdmin.BLL.Conf p in rcoll)
                    {
                        MultiSelectListBoxItem li = new MultiSelectListBoxItem();
                        li.Name = p.ConfName.ToString();
                        li.ID = p.ConfID;                        
                        
                        int count = lstConfSource.Count;

                        for (int i = 0; i < count; i++)
                        {
                            MultiSelectListBoxItem item = lstConfSource[i];
                            if (item.Name == li.Name)
                            {
                                lstConfSource.RemoveAt(i);
                                i--;
                                count--;
                            }

                        }                        
                        li = null;

                        this.txtSeledConf.Text += p.ID + ",";
                    }

                    listConf.DataSource = lstConfSource;
                    listConf.DataBind();

                    MultiSelectListBoxItemCollection rcollR = PE_DAL.GetKioskConfRotateByPubID(this.PubID);
                    this.listRotate.DataSource = rcollR;
                    this.listRotate.DataTextField = "name";
                    this.listRotate.DataValueField = "id";
                    this.listRotate.DataBind();
                    foreach (PubEntAdmin.BLL.Conf p in rcollR)
                    {
                        this.txtRotate.Text += p.ID + ",";
                    }

                }
            }            
          
        }

        protected void BindConferences()
        {
            //LiveInt l_liveInt = PE_DAL.GetLiveIntByPubID(this.PubID);
            //IsExh = l_liveInt.Exhibit == 0 ? false : true;
            DataSet ds = (DataSet)LU_DAL.GetKioskPub(this.PubID);
            if (ds!=null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    this.pnlkiosk.Visible = true;
                    IsExh = true;
                }
                else
                    IsExh = false;
            }
        }

        public bool Save()
        {
            string SeledConfValue="", SeledRotateValue="";
            if(txtSeledConf.Text.Trim()!="")
                SeledConfValue = this.txtSeledConf.Text.Substring(0,txtSeledConf.Text.Length-1);
            if(txtRotate.Text.Trim() != "")
                SeledRotateValue = this.txtRotate.Text.Substring(0, txtRotate.Text.Length - 1);
            this.SecVal();

            bool blnDisplayStatusSave = PE_DAL.SetExhDisplayStatusByPubID(this.PubID, this.ckboxListDisplayStatusExh.SelectedValueToString(), ',');

            bool blnNCIPLInterfaceSave = PE_DAL.SetExhInterface(this.PubID,
                this.txtMaxQtyExh.Text.Trim().Length != 0 ? System.Convert.ToInt32(this.txtMaxQtyExh.Text.Trim()) : -1,
                this.txtMaxQtyIntl.Text.Trim().Length != 0 ? System.Convert.ToInt32(this.txtMaxQtyIntl.Text.Trim()) : -1,
                System.Convert.ToInt32(this.rdbtnListYesNoEveryOrder.Yes),
                System.Convert.ToInt32(this.rdbtnListYesNoShowInSearchRes.Yes));

            if (IsExh)
            {
                bool blnExcPubs=true;
                string strExcPubsEr="";

                if (SeledRotateValue !="")
                {
                    Array a = SeledRotateValue.Split(',');
                    string[] aryString = new string[a.Length];


                    for (int i = 0; i < a.Length; i++)
                    {
                        DataSet ds = LU_DAL.displayRotationPubsData(Convert.ToInt32(a.GetValue(i)));
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 36)
                            {
                                blnExcPubs = false;
                                if (strExcPubsEr == "")
                                    strExcPubsEr = "There are already 36 rotation publications for " + ds.Tables[0].Rows[0].ItemArray[1].ToString();
                                else
                                    strExcPubsEr += ", " + ds.Tables[0].Rows[0].ItemArray[1].ToString();
                            }
                        }

                    }
                }

                if (blnExcPubs)
                {
                    this.lblErrorRotate.Text = "";
                    bool blnKioskConfSave = PE_DAL.SetExhKioskInterface(this.PubID, SeledConfValue, SeledRotateValue, ',');
                    if (blnDisplayStatusSave && blnNCIPLInterfaceSave && blnKioskConfSave)
                        return true;
                    else
                        return false;
                }
                else
                {
                    this.lblErrorRotate.Text = strExcPubsEr + " conference(s).";
                    return false;
                }
            }
            else
            {
                if (blnDisplayStatusSave && blnNCIPLInterfaceSave)
                    return true;
                else
                    return false;
            }
        }


        #region MonitorChanges
        protected void RegisterMonitoredChanges()
        {
            PubEntAdminManager.MonitorChanges2(this.Page, this, this.ckboxListDisplayStatusExh);
            PubEntAdminManager.MonitorChanges2(this.Page, this, this.txtMaxQtyExh);
            PubEntAdminManager.MonitorChanges2(this.Page, this, this.txtMaxQtyIntl);
            PubEntAdminManager.MonitorChanges2(this.Page, this, this.rdbtnListYesNoEveryOrder);
            PubEntAdminManager.MonitorChanges2(this.Page, this, this.rdbtnListYesNoShowInSearchRes);

        }

        #endregion

        #region Sec Val
        private void SecVal()
        {
            this.LenVal();
            this.TypeVal();
            this.TagVal();
            this.SpecialVal();
        }

        private void LenVal()
        {
            if ((!PubEntAdminManager.LenVal(this.txtMaxQtyExh.Text, 8)))
            {
                Response.Redirect("InvalidInput.aspx");
            }

            if ((!PubEntAdminManager.LenVal(this.txtMaxQtyIntl.Text, 8)))
            {
                Response.Redirect("InvalidInput.aspx");
            }
        }

        private void TypeVal()
        {
            if (this.txtMaxQtyExh.Text.Trim().Length > 0)
            {
                if (!PubEntAdminManager.ContentNumVal(this.txtMaxQtyExh.Text.Trim()))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            if (this.txtMaxQtyExh.Text.Trim().Length > 0)
            {
                if (!PubEntAdminManager.ContentNumVal(this.txtMaxQtyIntl.Text.Trim()))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }
        }

        private void TagVal()
        {
            if ((PubEntAdminManager.OtherVal(this.txtMaxQtyExh.Text)))
            {
                Response.Redirect("InvalidInput.aspx");
            }

            if ((PubEntAdminManager.OtherVal(this.txtMaxQtyIntl.Text)))
            {
                Response.Redirect("InvalidInput.aspx");
            }

            foreach (ListItem li in this.ckboxListDisplayStatusExh.Items)
            {
                if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in rdbtnListYesNoEveryOrder.Items)
            {
                if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in rdbtnListYesNoShowInSearchRes.Items)
            {
                if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

        }

        private void SpecialVal()
        {

            if ((PubEntAdminManager.SpecialVal2(this.txtMaxQtyExh.Text.Replace(" ", ""))))
            {
                Response.Redirect("InvalidInput.aspx");
            }

            if ((PubEntAdminManager.SpecialVal2(this.txtMaxQtyIntl.Text.Replace(" ", ""))))
            {
                Response.Redirect("InvalidInput.aspx");
            }

            foreach (ListItem li in ckboxListDisplayStatusExh.Items)
            {
                if (PubEntAdminManager.SpecialVal2(li.Text.Replace(" ", "")) ||
                    PubEntAdminManager.SpecialVal2(li.Value.Replace(" ", "")))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in rdbtnListYesNoEveryOrder.Items)
            {
                if (PubEntAdminManager.SpecialVal2(li.Text.Replace(" ", "")) ||
                    PubEntAdminManager.SpecialVal2(li.Value.Replace(" ", "")))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in rdbtnListYesNoShowInSearchRes.Items)
            {
                if (PubEntAdminManager.SpecialVal2(li.Text.Replace(" ", "")) ||
                    PubEntAdminManager.SpecialVal2(li.Value.Replace(" ", "")))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }
        }
        #endregion

        #endregion
       
        #region Properties
        public int PubID
        {
            set
            {
                this.intPubID = value;
            }
            get
            {
                return this.intPubID;
            }
        }

        public bool IsExh
        {
            set
            {
                this.bnlExh = value;
            }
            get
            {
                return this.bnlExh;
            }
        }

        #endregion
    }
}