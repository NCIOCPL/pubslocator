using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using PubEntAdmin.BLL;
using PubEntAdmin.DAL;
//using PubEntAdmin.UserControl;
using System.Configuration;
using System.Web.SessionState;

namespace PubEntAdmin
{
    public partial class cssetup : System.Web.UI.Page
    {

        KVPairCollection kvpaircoll;
        System.Web.UI.UserControl userControl;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session.IsNewSession)
            {
                Response.Redirect("Home.aspx");
            }
            userControl =
            (System.Web.UI.UserControl)this.LoadControl("UserControl/AdminMenu.ascx");
            this.plcHldMenu.Controls.Add(userControl);
            if (!IsPostBack)
            {
                ///Uncomment below code after integrated with Admin Tool
                if (!((CustomPrincipal)Context.User).IsInRole(PubEntAdminManager.AdminRole))
                {
                    PubEntAdminManager.UnathorizedAccess();
                }
                this.PageTitle = "Canned Search Setup";
            }

            //Code for Hailstorm
            if (ucCancerTypeAdd != null) ucCancerTypeAdd.SecurityCheck();
            if (ucSubjectAdd != null) ucCancerTypeAdd.SecurityCheck();
            if (ucPubFormatAdd != null) ucCancerTypeAdd.SecurityCheck();
            if (ucRaceAdd != null) ucCancerTypeAdd.SecurityCheck();
            if (ucAudienceAdd != null) ucCancerTypeAdd.SecurityCheck();
            if (ucLanguageAdd != null) ucCancerTypeAdd.SecurityCheck();
            if (ucCollectionsAdd != null) ucCancerTypeAdd.SecurityCheck();
            //End of code for Hailstorm

        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            CustomValidator1.IsValid = true;
            RequiredFieldValidator1.IsValid = true;

            //Page.Validate();
            if (Page.IsValid)
            {
                Session["CANNED_SearchParam"] = txtFind.Text.Trim();
                lstviewCannedRecords.EditIndex = -1;
                this.Bind();
                //DataPagerTop.SetPageProperties(0, NumValue, true);
                DataPager.SetPageProperties(0, DataPager.PageSize, true);
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            dvAdd.Visible = true;
            dvEdit.Visible = false;

            //Cancer Type List
            ucCancerTypeAdd.ClearValues();
            kvpaircoll = DAL.cs_dal.GetKVPair("sp_CANNED_getCancerTypes", 1, 0);
            foreach (KVPair kvpair in kvpaircoll)
            {
                ListItem lstItem = new ListItem();
                lstItem.Text = kvpair.Val;
                lstItem.Value = kvpair.Key;
                //if (kvpair.IsSelected == "1")
                //    lstItem.Selected = true;
                //else
                lstItem.Selected = false;
                ucCancerTypeAdd.listBoxCtrl.Items.Add(lstItem);
                lstItem = null;
            }
            kvpaircoll = null;

            //Subject List
            ucSubjectAdd.ClearValues();
            kvpaircoll = DAL.cs_dal.GetKVPair("sp_CANNED_getSubjects", 1, 0);
            foreach (KVPair kvpair in kvpaircoll)
            {
                ListItem lstItem = new ListItem();
                lstItem.Text = kvpair.Val;
                lstItem.Value = kvpair.Key;
                //if (kvpair.IsSelected == "1")
                //    lstItem.Selected = true;
                //else
                lstItem.Selected = false;
                ucSubjectAdd.listBoxCtrl.Items.Add(lstItem);
                lstItem = null;
            }
            kvpaircoll = null;

            //Publication Format List
            ucPubFormatAdd.ClearValues();
            kvpaircoll = DAL.cs_dal.GetKVPair("sp_CANNED_getProductFormats", 1, 0);
            foreach (KVPair kvpair in kvpaircoll)
            {
                ListItem lstItem = new ListItem();
                lstItem.Text = kvpair.Val;
                lstItem.Value = kvpair.Key;
                //if (kvpair.IsSelected == "1")
                //    lstItem.Selected = true;
                //else
                lstItem.Selected = false;
                ucPubFormatAdd.listBoxCtrl.Items.Add(lstItem);
                lstItem = null;
            }
            kvpaircoll = null;

            //Race List
            ucRaceAdd.ClearValues();
            kvpaircoll = DAL.cs_dal.GetKVPair("sp_CANNED_getRace", 1, 0);
            foreach (KVPair kvpair in kvpaircoll)
            {
                ListItem lstItem = new ListItem();
                lstItem.Text = kvpair.Val;
                lstItem.Value = kvpair.Key;
                //if (kvpair.IsSelected == "1")
                //    lstItem.Selected = true;
                //else
                lstItem.Selected = false;
                ucRaceAdd.listBoxCtrl.Items.Add(lstItem);
                lstItem = null;
            }
            kvpaircoll = null;

            //Audience List
            ucAudienceAdd.ClearValues();
            kvpaircoll = DAL.cs_dal.GetKVPair("sp_CANNED_getAudience", 1, 0);
            foreach (KVPair kvpair in kvpaircoll)
            {
                ListItem lstItem = new ListItem();
                lstItem.Text = kvpair.Val;
                lstItem.Value = kvpair.Key;
                //if (kvpair.IsSelected == "1")
                //    lstItem.Selected = true;
                //else
                lstItem.Selected = false;
                ucAudienceAdd.listBoxCtrl.Items.Add(lstItem);
                lstItem = null;
            }
            kvpaircoll = null;

            //Language List
            ucLanguageAdd.ClearValues();
            kvpaircoll = DAL.cs_dal.GetKVPair("sp_CANNED_getLanguages", 1, 0);
            foreach (KVPair kvpair in kvpaircoll)
            {
                ListItem lstItem = new ListItem();
                lstItem.Text = kvpair.Val;
                lstItem.Value = kvpair.Key;
                //if (kvpair.IsSelected == "1")
                //    lstItem.Selected = true;
                //else
                lstItem.Selected = false;
                ucLanguageAdd.listBoxCtrl.Items.Add(lstItem);
                lstItem = null;
            }
            kvpaircoll = null;

            //Collections List
            ucCollectionsAdd.ClearValues();
            kvpaircoll = DAL.cs_dal.GetKVPair("sp_CANNED_getCollections", 1, 0);
            foreach (KVPair kvpair in kvpaircoll)
            {
                ListItem lstItem = new ListItem();
                lstItem.Text = kvpair.Val;
                lstItem.Value = kvpair.Key;
                //if (kvpair.IsSelected == "1")
                //    lstItem.Selected = true;
                //else
                lstItem.Selected = false;
                ucCollectionsAdd.listBoxCtrl.Items.Add(lstItem);
                lstItem = null;
            }
            kvpaircoll = null;
        }
        public string PageTitle
        {
            set
            {
                ((Label)this.FindControl("lblPageTitle")).Text = value;
            }
            get
            {
                return ((Label)this.FindControl("lblPageTitle")).Text;
            }
        }
        //Add a new canned search record
        protected void btnSave_Click(object sender, EventArgs e)
        {
            this.SecurityCheck(3, txtHeaderText);

            CustomValidator1.Validate();
            if (Page.IsValid)
            {
                string recdidout = "";
                DAL.cs_dal.Save(
                                    txtHeaderText.Text,
                                    ucCancerTypeAdd.SelValues,
                                    ucSubjectAdd.SelValues,
                                    ucPubFormatAdd.SelValues,
                                    ucRaceAdd.SelValues,
                                    ucAudienceAdd.SelValues,
                                    ucLanguageAdd.SelValues,
                                    ucCollectionsAdd.SelValues,
                                    0,
                                    0,
                                    1,
                                    ref recdidout
                                );

                Session["CANNED_SearchParam"] = recdidout;
                this.Bind();
            }


        }

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (dvAdd.Visible == true)
            {
                if (ucCancerTypeAdd.SelValues.Length == 0
                        && ucSubjectAdd.SelValues.Length == 0
                        && ucPubFormatAdd.SelValues.Length == 0
                        && ucRaceAdd.SelValues.Length == 0
                        && ucAudienceAdd.SelValues.Length == 0
                        && ucLanguageAdd.SelValues.Length == 0
                        && ucCollectionsAdd.SelValues.Length == 0)
                    args.IsValid = false;
            }
        }

        private void Bind()
        {
            string SearchParam = "";
            dvAdd.Visible = false;
            dvEdit.Visible = true;

            ///If there is a session variable then the search will be based on its value.
            ///Otherwise all records will be pulled up
            if (Session["CANNED_SearchParam"] != null)
            {
                if (Session["CANNED_SearchParam"].ToString().Length > 0)
                    SearchParam = Session["CANNED_SearchParam"].ToString();
                else
                    SearchParam = "";
            }

            if (SearchParam.Length > 0) //Call the CSV sp
            {
                string param = "";
                string[] tempStr = SearchParam.Split(new Char[] { ' ' });
                for (var i = 0; i < tempStr.Length; i++)
                {
                    if (tempStr[i].Trim() != "")
                    {
                        if (i == tempStr.Length - 1)
                            param += "'" + tempStr[i] + "'";
                        else
                            param += "'" + tempStr[i] + "'" + ",";
                    }
                }

                lstviewCannedRecords.DataSource = DAL.cs_dal.GetRecordsByCSV(param);
                lstviewCannedRecords.DataBind();
            }
            else //Get All Canned Records
            {
                lstviewCannedRecords.DataSource = DAL.cs_dal.GetRecords();
                lstviewCannedRecords.DataBind();
            }
        }


        protected void lstviewCannedRecords_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {

                ListViewDataItem item = (ListViewDataItem)e.Item;
                cs_record currRecord = (cs_record)item.DataItem; //Get the Current Record Item in the collection

                #region ReadOnlyMode
                //Get the Controls
                HiddenField CannId = (HiddenField)e.Item.FindControl("CannId");
                Label lblRecordId = (Label)e.Item.FindControl("lblRecordId");
                Label lblHeaderText = (Label)e.Item.FindControl("lblHeaderText");
                Label lblUrl = (Label)e.Item.FindControl("lblUrl");
                Label lblCancerType = (Label)e.Item.FindControl("lblCancerType");
                Label lblSubject = (Label)e.Item.FindControl("lblSubject");
                Label lblPubFormat = (Label)e.Item.FindControl("lblPubFormat");
                Label lblRace = (Label)e.Item.FindControl("lblRace");
                Label lblAudience = (Label)e.Item.FindControl("lblAudience");
                Label lblLanguage = (Label)e.Item.FindControl("lblLanguage");
                Label lblCollections = (Label)e.Item.FindControl("lblCollections");
                Label lblActive = (Label)e.Item.FindControl("lblActive");

                //Assign Values
                if (CannId != null)
                    CannId.Value = currRecord.CannId.ToString();
                if (lblRecordId != null)
                    lblRecordId.Text = currRecord.RecId;
                if (lblHeaderText != null)
                    lblHeaderText.Text = currRecord.HeaderText;
                if (lblUrl != null)
                    lblUrl.Text = ConfigurationManager.AppSettings["NCIPLCannedSearchURL"] + currRecord.RecId;
                if (lblCancerType != null)
                    lblCancerType.Text = currRecord.CancerType;
                if (lblSubject != null)
                    lblSubject.Text = currRecord.Subject;
                if (lblPubFormat != null)
                    lblPubFormat.Text = currRecord.PubFormat;
                if (lblRace != null)
                    lblRace.Text = currRecord.Race;
                if (lblAudience != null)
                    lblAudience.Text = currRecord.Audience;
                if (lblLanguage != null)
                    lblLanguage.Text = currRecord.Language;
                if (lblCollections != null)
                    lblCollections.Text = currRecord.Collections;
                if (lblActive != null)
                {
                    if (currRecord.Active == 1)
                        lblActive.Text = "Yes";
                    else
                        lblActive.Text = "No";
                }

                #region SecurityCheck1
                if (CannId != null) this.SecurityCheck(1, CannId);
                #endregion

                #endregion



                #region EditMode
                //Get the Controls
                HiddenField CannIdEdit = (HiddenField)e.Item.FindControl("CannIdEdit");
                if (CannIdEdit != null)
                    CannIdEdit.Value = currRecord.CannId.ToString();

                Label lblRecordIdEdit = (Label)e.Item.FindControl("lblRecordIdEdit");
                if (lblRecordIdEdit != null)
                    lblRecordIdEdit.Text = currRecord.RecId;

                TextBox txtHeaderTextEdit = (TextBox)e.Item.FindControl("txtHeaderTextEdit");
                if (txtHeaderTextEdit != null)
                    txtHeaderTextEdit.Text = currRecord.HeaderText;

                //Type of Cancer
                PubEntAdmin.UserControl.cslistbox ucCancerTypeEdit = (PubEntAdmin.UserControl.cslistbox)e.Item.FindControl("ucCancerTypeEdit");
                if (ucCancerTypeEdit != null)
                {
                    kvpaircoll = DAL.cs_dal.GetKVPair("sp_CANNED_getCancerTypes", 0, currRecord.CannId);
                    foreach (KVPair kvpair in kvpaircoll)
                    {
                        ListItem lstItem = new ListItem();
                        lstItem.Text = kvpair.Val;
                        lstItem.Value = kvpair.Key;
                        if (kvpair.IsSelected == "1")
                            lstItem.Selected = true;
                        else
                            lstItem.Selected = false;
                        ucCancerTypeEdit.listBoxCtrl.Items.Add(lstItem);
                        lstItem = null;
                    }
                    kvpaircoll = null;
                }

                //Subject List
                PubEntAdmin.UserControl.cslistbox ucSubjectEdit = (PubEntAdmin.UserControl.cslistbox)e.Item.FindControl("ucSubjectEdit");
                if (ucSubjectEdit != null)
                {
                    kvpaircoll = DAL.cs_dal.GetKVPair("sp_CANNED_getSubjects", 0, currRecord.CannId);
                    foreach (KVPair kvpair in kvpaircoll)
                    {
                        ListItem lstItem = new ListItem();
                        lstItem.Text = kvpair.Val;
                        lstItem.Value = kvpair.Key;
                        if (kvpair.IsSelected == "1")
                            lstItem.Selected = true;
                        else
                            lstItem.Selected = false;
                        ucSubjectEdit.listBoxCtrl.Items.Add(lstItem);
                        lstItem = null;
                    }
                    kvpaircoll = null;
                }

                //Publication Format List
                PubEntAdmin.UserControl.cslistbox ucPubFormatEdit = (PubEntAdmin.UserControl.cslistbox)e.Item.FindControl("ucPubFormatEdit");
                if (ucPubFormatEdit != null)
                {
                    kvpaircoll = DAL.cs_dal.GetKVPair("sp_CANNED_getProductFormats", 0, currRecord.CannId);
                    foreach (KVPair kvpair in kvpaircoll)
                    {
                        ListItem lstItem = new ListItem();
                        lstItem.Text = kvpair.Val;
                        lstItem.Value = kvpair.Key;
                        if (kvpair.IsSelected == "1")
                            lstItem.Selected = true;
                        else
                            lstItem.Selected = false;
                        ucPubFormatEdit.listBoxCtrl.Items.Add(lstItem);
                        lstItem = null;
                    }
                    kvpaircoll = null;
                }

                //Race List
                PubEntAdmin.UserControl.cslistbox ucRaceEdit = (PubEntAdmin.UserControl.cslistbox)e.Item.FindControl("ucRaceEdit");
                if (ucRaceEdit != null)
                {
                    kvpaircoll = DAL.cs_dal.GetKVPair("sp_CANNED_getRace", 0, currRecord.CannId);
                    foreach (KVPair kvpair in kvpaircoll)
                    {
                        ListItem lstItem = new ListItem();
                        lstItem.Text = kvpair.Val;
                        lstItem.Value = kvpair.Key;
                        if (kvpair.IsSelected == "1")
                            lstItem.Selected = true;
                        else
                            lstItem.Selected = false;
                        ucRaceEdit.listBoxCtrl.Items.Add(lstItem);
                        lstItem = null;
                    }
                    kvpaircoll = null;
                }

                //Audience List
                PubEntAdmin.UserControl.cslistbox ucAudienceEdit = (PubEntAdmin.UserControl.cslistbox)e.Item.FindControl("ucAudienceEdit");
                if (ucAudienceEdit != null)
                {
                    kvpaircoll = DAL.cs_dal.GetKVPair("sp_CANNED_getAudience", 0, currRecord.CannId);
                    foreach (KVPair kvpair in kvpaircoll)
                    {
                        ListItem lstItem = new ListItem();
                        lstItem.Text = kvpair.Val;
                        lstItem.Value = kvpair.Key;
                        if (kvpair.IsSelected == "1")
                            lstItem.Selected = true;
                        else
                            lstItem.Selected = false;
                        ucAudienceEdit.listBoxCtrl.Items.Add(lstItem);
                        lstItem = null;
                    }
                    kvpaircoll = null;
                }

                //Language List
                PubEntAdmin.UserControl.cslistbox ucLanguageEdit = (PubEntAdmin.UserControl.cslistbox)e.Item.FindControl("ucLanguageEdit");
                if (ucLanguageEdit != null)
                {
                    kvpaircoll = DAL.cs_dal.GetKVPair("sp_CANNED_getLanguages", 0, currRecord.CannId);
                    foreach (KVPair kvpair in kvpaircoll)
                    {
                        ListItem lstItem = new ListItem();
                        lstItem.Text = kvpair.Val;
                        lstItem.Value = kvpair.Key;
                        if (kvpair.IsSelected == "1")
                            lstItem.Selected = true;
                        else
                            lstItem.Selected = false;
                        ucLanguageEdit.listBoxCtrl.Items.Add(lstItem);
                        lstItem = null;
                    }
                    kvpaircoll = null;
                }

                //Collections List
                PubEntAdmin.UserControl.cslistbox ucCollectionsEdit = (PubEntAdmin.UserControl.cslistbox)e.Item.FindControl("ucCollectionsEdit");
                if (ucCollectionsEdit != null)
                {
                    kvpaircoll = DAL.cs_dal.GetKVPair("sp_CANNED_getCollections", 0, currRecord.CannId);
                    foreach (KVPair kvpair in kvpaircoll)
                    {
                        ListItem lstItem = new ListItem();
                        lstItem.Text = kvpair.Val;
                        lstItem.Value = kvpair.Key;
                        if (kvpair.IsSelected == "1")
                            lstItem.Selected = true;
                        else
                            lstItem.Selected = false;
                        ucCollectionsEdit.listBoxCtrl.Items.Add(lstItem);
                        lstItem = null;
                    }
                    kvpaircoll = null;
                }

                DropDownList ddlActiveEdit = (DropDownList)e.Item.FindControl("ddlActiveEdit");
                if (ddlActiveEdit != null)
                {
                    if (currRecord.Active == 1)
                        ddlActiveEdit.SelectedIndex = 0;
                    else
                        ddlActiveEdit.SelectedIndex = 1;
                }

                #region SecurityCheck2
                if (CannIdEdit != null) this.SecurityCheck(1, CannIdEdit);
                if (txtHeaderTextEdit != null) this.SecurityCheck(3, txtHeaderTextEdit);
                if (ucCancerTypeEdit != null) ucCancerTypeEdit.SecurityCheck();
                if (ucSubjectEdit != null) ucSubjectEdit.SecurityCheck();
                if (ucPubFormatEdit != null) ucPubFormatEdit.SecurityCheck();
                if (ucRaceEdit != null) ucRaceEdit.SecurityCheck();
                if (ucAudienceEdit != null) ucAudienceEdit.SecurityCheck();
                if (ucLanguageEdit != null) ucLanguageEdit.SecurityCheck();
                if (ucCollectionsEdit != null) ucCollectionsEdit.SecurityCheck();
                if (ddlActiveEdit != null) this.SecurityCheck(2, ddlActiveEdit);
                #endregion

                #endregion

            }
        }

        protected void lstviewCannedRecords_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            lstviewCannedRecords.EditIndex = e.NewEditIndex;
            this.Bind();
        }

        protected void lstviewCannedRecords_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            //Get the controls
            HiddenField CannIdEdit = (HiddenField)lstviewCannedRecords.EditItem.FindControl("CannIdEdit");
            TextBox txtHeaderTextEdit = (TextBox)lstviewCannedRecords.EditItem.FindControl("txtHeaderTextEdit");
            PubEntAdmin.UserControl.cslistbox ucCancerTypeEdit = (PubEntAdmin.UserControl.cslistbox)lstviewCannedRecords.EditItem.FindControl("ucCancerTypeEdit");
            PubEntAdmin.UserControl.cslistbox ucSubjectEdit = (PubEntAdmin.UserControl.cslistbox)lstviewCannedRecords.EditItem.FindControl("ucSubjectEdit");
            PubEntAdmin.UserControl.cslistbox ucPubFormatEdit = (PubEntAdmin.UserControl.cslistbox)lstviewCannedRecords.EditItem.FindControl("ucPubFormatEdit");
            PubEntAdmin.UserControl.cslistbox ucRaceEdit = (PubEntAdmin.UserControl.cslistbox)lstviewCannedRecords.EditItem.FindControl("ucRaceEdit");
            PubEntAdmin.UserControl.cslistbox ucAudienceEdit = (PubEntAdmin.UserControl.cslistbox)lstviewCannedRecords.EditItem.FindControl("ucAudienceEdit");
            PubEntAdmin.UserControl.cslistbox ucLanguageEdit = (PubEntAdmin.UserControl.cslistbox)lstviewCannedRecords.EditItem.FindControl("ucLanguageEdit");
            PubEntAdmin.UserControl.cslistbox ucCollectionsEdit = (PubEntAdmin.UserControl.cslistbox)lstviewCannedRecords.EditItem.FindControl("ucCollectionsEdit");
            DropDownList ddlActiveEdit = (DropDownList)lstviewCannedRecords.EditItem.FindControl("ddlActiveEdit");

            if (ucCancerTypeEdit.SelValues.Length == 0
                        && ucSubjectEdit.SelValues.Length == 0
                        && ucPubFormatAdd.SelValues.Length == 0
                        && ucRaceEdit.SelValues.Length == 0
                        && ucAudienceEdit.SelValues.Length == 0
                        && ucLanguageEdit.SelValues.Length == 0
                        && ucCollectionsEdit.SelValues.Length == 0)
                CustomValidator1.IsValid = false;
            else
                lstviewCannedRecords.EditIndex = -1;

            if (Page.IsValid)
            {
                string recdidout = "";
                DAL.cs_dal.Save(
                                        txtHeaderTextEdit.Text,
                                        ucCancerTypeEdit.SelValues,
                                        ucSubjectEdit.SelValues,
                                        ucPubFormatEdit.SelValues,
                                        ucRaceEdit.SelValues,
                                        ucAudienceEdit.SelValues,
                                        ucLanguageEdit.SelValues,
                                        ucCollectionsEdit.SelValues,
                                        Int32.Parse(CannIdEdit.Value),
                                        1,
                                        Int32.Parse(ddlActiveEdit.SelectedValue),
                                        ref recdidout
                                    );

                //Session["CANNED_SearchParam"] = recdidout;
                this.Bind();
            }
        }

        protected void lstviewCannedRecords_ItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            lstviewCannedRecords.EditIndex = -1;
            this.Bind();
        }

        protected void lstviewCannedRecords_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            int startrowindex = e.StartRowIndex;
            DataPager.SetPageProperties(startrowindex, e.MaximumRows, false);
            lstviewCannedRecords.EditIndex = -1;
            this.Bind();
        }

        //Security Check
        public void SecurityCheck(int CaseNum, object Ctrl)
        {
            switch (CaseNum)
            {
                case 1: //Hidden Field
                    HiddenField hidField = (HiddenField)Ctrl;
                    if (!PubEntAdminManager.ContentNumVal(hidField.Value))
                        Response.Redirect("InvalidInput.aspx");
                    break;

                case 2: //Dropdown
                    DropDownList ddlField = (DropDownList)Ctrl;
                    foreach (ListItem li in ddlField.Items)
                    {
                        if (!PubEntAdminManager.ContentNumVal(li.Value))
                            Response.Redirect("InvalidInput.aspx");
                    }
                    break;

                case 3: //Textbox
                    TextBox txtField = (TextBox)Ctrl;
                    txtField.Text = txtField.Text.Trim();
                    if (!PubEntAdminManager.LenVal(txtField.Text, 500))
                        Response.Redirect("InvalidInput.aspx");
                    if (PubEntAdminManager.OtherVal(txtField.Text))
                        Response.Redirect("InvalidInput.aspx");
                    if (PubEntAdminManager.SpecialVal2(txtField.Text))
                        Response.Redirect("InvalidInput.aspx");
                    break;
            }
        }

    }
}
