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
using System.Text.RegularExpressions;
using PubEntAdmin.DAL;
using PubEntAdmin.BLL;

namespace PubEntAdmin.UserControl
{
    public partial class Search : System.Web.UI.UserControl
    {
        #region Event Handling

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.BindOptions();
            }

            this.SecVal();
        }

        protected void NIH_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if ((this.txtNIHNum1.Text.Trim().Length == 0 && this.txtNIHNum2.Text.Trim().Length != 0) ||
                (this.txtNIHNum1.Text.Trim().Length != 0 && this.txtNIHNum2.Text.Trim().Length == 0) ||
                (this.txtNIHNum1.Text.Trim().Length != 0 && this.txtNIHNum2.Text.Trim().Length != 0))
            {
                Regex r = new Regex(@"^\d{2}$");
                Regex r2 = new Regex(@"^[a-zA-Z0-9]{3,5}$");
                bool blnnih1 = true;
                bool blnnih2 = true;

                if (this.txtNIHNum1.Text.Trim().Length > 0)
                {
                    if (!r.IsMatch(this.txtNIHNum1.Text.Trim()))
                        blnnih1 = false;
                }
               
                if (this.txtNIHNum2.Text.Trim().Length > 0)
                {
                    if (!r2.IsMatch(this.txtNIHNum2.Text.Trim()))
                        blnnih2 = false;
                }

                if (blnnih1 && blnnih2)
                    args.IsValid = true;
                else
                    args.IsValid = false;
            }
            else
                args.IsValid = true;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Page.Validate();
            errormsg.Visible = false;
            errormsgnu.Visible = false;
            DateTime cfrom;
            DateTime cto;
            Boolean newst = this.NewUpStatus.Items.FindByValue("New").Selected;
            Boolean updatest = this.NewUpStatus.Items.FindByValue("Updated").Selected;

            if (Page.IsValid)
            {
                errormsgnu.Visible = false;
                errormsg.Visible = false;
                this.SecVal();

                if (((this.txtSrRecordStartDate.Text != String.Empty) &&
                    (this.txtEnRecordEndDate.Text != String.Empty))) 
                    //|| ((updatest == true) && (newst == true)))
                {
                    cfrom = Convert.ToDateTime(this.txtSrRecordStartDate.Text);
                    cto = Convert.ToDateTime(this.txtEnRecordEndDate.Text);

                    int resultc = DateTime.Compare(cfrom, cto);

                    if (resultc > 0)
                    {
                        datevalidate();
                    }
                    else 
                    {
                        Server.Transfer("SearchResult.aspx");
                    }
                }
                else
                    Server.Transfer("SearchResult.aspx");
            }
            
            //if (((this.txtSrRecordStartDate.Text != String.Empty) && (this.txtEnRecordEndDate.Text != String.Empty)) || ((updatest == true) && (newst == true)))
            //{
            //    if ((this.txtSrRecordStartDate.Text != String.Empty) && (this.txtEnRecordEndDate.Text != String.Empty))
            //    {
            //        cfrom = Convert.ToDateTime(this.txtSrRecordStartDate.Text);
            //        cto = Convert.ToDateTime(this.txtEnRecordEndDate.Text);

            //        int resultc = DateTime.Compare(cfrom, cto);

            //        if (resultc > 0)
            //        {
            //            datevalidate();
            //        }
            //        else if (Page.IsValid)
            //        {
            //            errormsgnu.Visible = false;
            //            errormsg.Visible = false;
            //            this.SecVal();
            //            Server.Transfer("SearchResult.aspx");
            //        }
            //    }
                

            //    if ((updatest == true) && (newst == true))
            //    {
            //        statusvalidate();
            //     }
            //    else if (Page.IsValid)
            //    {
            //        errormsgnu.Visible = false;
            //        errormsg.Visible = false;
            //        this.SecVal();
            //        Server.Transfer("SearchResult.aspx");
            //    }
            //} 
            //else if (Page.IsValid)
            //{
            //    errormsgnu.Visible = false;
            //    errormsg.Visible = false;
            //    this.SecVal();
            //    Server.Transfer("SearchResult.aspx");
            //}
           
        }

        #endregion

        

        #region Methods
        protected void BindOptions()
        {
            this.listLang.DataSource = PE_DAL.GetAllLang(true);
            this.listLang.DataTextField = "name";
            this.listLang.DataValueField = "id";
            this.listLang.DataBind();

            this.listAudience.DataSource = PE_DAL.GetAllAudience(true);
            this.listAudience.DataTextField = "name";
            this.listAudience.DataValueField = "id";
            this.listAudience.DataBind();

            this.listCancerType.DataSource = PE_DAL.GetAllCancerType(true);
            this.listCancerType.DataTextField = "name";
            this.listCancerType.DataValueField = "id";
            this.listCancerType.DataBind();

            this.listProdFormat.DataSource = PE_DAL.GetAllProdFormat(true);
            this.listProdFormat.DataTextField = "name";
            this.listProdFormat.DataValueField = "id";
            this.listProdFormat.DataBind();

            this.listReadingLevel.DataSource = PE_DAL.GetAllReadinglevel(true);
            this.listReadingLevel.DataTextField = "name";
            this.listReadingLevel.DataValueField = "id";
            this.listReadingLevel.DataBind();

            this.listSeries.DataSource = PE_DAL.GetAllSeries(true);
            this.listSeries.DataTextField = "name";
            this.listSeries.DataValueField = "id";
            this.listSeries.DataBind();

            this.listRace.DataSource = PE_DAL.GetAllRace(true);
            this.listRace.DataTextField = "name";
            this.listRace.DataValueField = "id";
            this.listRace.DataBind();

            this.listBookStatus.DataSource = PE_DAL.GetAllBookstatus(true);
            this.listBookStatus.DataTextField = "name";
            this.listBookStatus.DataValueField = "id";
            this.listBookStatus.DataBind();

            this.listSubject.DataSource = PE_DAL.GetAllSubject(true);
            this.listSubject.DataTextField = "name";
            this.listSubject.DataValueField = "id";
            this.listSubject.DataBind();

            this.listROOSubject.DataSource = PE_DAL.GetAllROOUsedSubject();
            this.listROOSubject.DataTextField = "name";
            this.listROOSubject.DataValueField = "id";
            this.listROOSubject.DataBind();

            this.listAward.DataSource = PE_DAL.GetAllAward(true);
            this.listAward.DataTextField = "name";
            this.listAward.DataValueField = "id";
            this.listAward.DataBind();
            //this.listtestcancertype.DataSource = State.GetStates();
            //this.listtestcancertype.DataTextField = "StateName";
            //this.listtestcancertype.DataValueField = "StateAbbr";
            //this.listtestcancertype.DataBind();

            //this.rdbtnListYesNoMCL.DataBind();

            this.listOwner.DataSource = PE_DAL.GetAllOwners(true);
            this.listOwner.DataTextField = "name";
            this.listOwner.DataValueField = "id";
            this.listOwner.DataBind();

            this.listSponsor.DataSource = PE_DAL.GetAllSponsors(true);
            this.listSponsor.DataTextField = "name";
            this.listSponsor.DataValueField = "id";
            this.listSponsor.DataBind();
        }

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
            if ((!PubEntAdminManager.LenVal(this.txtKeyword.Text, 400)) ||
                (!PubEntAdminManager.LenVal(this.txtNIHNum1.Text, 2)) ||
                (!PubEntAdminManager.LenVal(this.txtNIHNum2.Text, 5)) ||
                (!PubEntAdminManager.LenVal(this.txtSrRecordStartDate.Text, 10)) ||
                (!PubEntAdminManager.LenVal(this.txtEnRecordEndDate.Text, 10)))
            {
                Response.Redirect("InvalidInput.aspx");
            }
        }

        private void TypeVal()
        {
            if (this.txtNIHNum1.Text.Trim().Length > 0)
            {
                if (!PubEntAdminManager.ContentVal(this.txtNIHNum1.Text.Trim(), @"^\d{2}$"))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            if (this.txtNIHNum2.Text.Trim().Length > 0)
            {
                if (!PubEntAdminManager.ContentVal(this.txtNIHNum2.Text.Trim(), @"^[a-zA-Z0-9]{3,5}$"))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }
        }

        private void TagVal()
        {
            if ((PubEntAdminManager.OtherVal(this.txtKeyword.Text)) ||
                (PubEntAdminManager.OtherVal(this.txtNIHNum1.Text)) ||
                (PubEntAdminManager.OtherVal(this.txtNIHNum2.Text)) ||
                (PubEntAdminManager.OtherVal(this.txtSrRecordStartDate.Text)) ||
                (PubEntAdminManager.OtherVal(this.txtEnRecordEndDate.Text)))
            {
                Response.Redirect("InvalidInput.aspx");
            }

            foreach (ListItem li in this.LiveIntSelSearch.Items)
            {
                if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in listCancerType.Items)
            {
                if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in listSubject.Items)
            {
                if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in listAudience.Items)
            {
                if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in listLang.Items)
            {
                if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in listProdFormat.Items)
            {
                if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in listSeries.Items)
            {
                if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in listRace.Items)
            {
                if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in listBookStatus.Items)
            {
                if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in listReadingLevel.Items)
            {
                if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            if (PubEntAdminManager.OtherVal(this.chboxMostCommonList.Text))
            {
                Response.Redirect("InvalidInput.aspx");
            }
        }

        private void SpecialVal()
        {

            if ((PubEntAdminManager.SpecialVal2(this.txtKeyword.Text.Replace(" ", ""))) ||
                (PubEntAdminManager.SpecialVal2(this.txtNIHNum1.Text.Replace(" ", ""))) ||
                (PubEntAdminManager.SpecialVal2(this.txtNIHNum2.Text.Replace(" ", ""))) ||
                (PubEntAdminManager.SpecialVal2(this.txtSrRecordStartDate.Text.Replace(" ", ""))) ||
                (PubEntAdminManager.SpecialVal2(this.txtEnRecordEndDate.Text.Replace(" ", "")))
                )
            {
                Response.Redirect("InvalidInput.aspx");
            }

            foreach (ListItem li in LiveIntSelSearch.Items)
            {
                if (PubEntAdminManager.SpecialVal2(li.Text.Replace(" ", "")) ||
                    PubEntAdminManager.SpecialVal2(li.Value.Replace(" ", "")))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in listCancerType.Items)
            {
                if (PubEntAdminManager.SpecialVal2(li.Text.Replace(" ", "")) ||
                    PubEntAdminManager.SpecialVal2(li.Value.Replace(" ", "")))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in listSubject.Items)
            {
                if (PubEntAdminManager.SpecialVal2(li.Text.Replace(" ", "")) ||
                    PubEntAdminManager.SpecialVal2(li.Value.Replace(" ", "")))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in listAudience.Items)
            {
                if (PubEntAdminManager.SpecialVal2(li.Text.Replace(" ", "")) ||
                    PubEntAdminManager.SpecialVal2(li.Value.Replace(" ", "")))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in listLang.Items)
            {
                if (PubEntAdminManager.SpecialVal2(li.Text.Replace(" ", "")) ||
                    PubEntAdminManager.SpecialVal2(li.Value.Replace(" ", "")))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in listProdFormat.Items)
            {
                if (PubEntAdminManager.SpecialVal2(li.Text.Replace(" ", "")) ||
                    PubEntAdminManager.SpecialVal2(li.Value.Replace(" ", "")))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in listSeries.Items)
            {
                if (PubEntAdminManager.SpecialVal2(li.Text.Replace(" ", "")) ||
                    PubEntAdminManager.SpecialVal2(li.Value.Replace(" ", "")))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in listRace.Items)
            {
                if (PubEntAdminManager.SpecialVal2(li.Text.Replace(" ", "")) ||
                    PubEntAdminManager.SpecialVal2(li.Value.Replace(" ", "")))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in listBookStatus.Items)
            {
                if (PubEntAdminManager.SpecialVal2(li.Text.Replace(" ", "")) ||
                    PubEntAdminManager.SpecialVal2(li.Value.Replace(" ", "")))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in listReadingLevel.Items)
            {
                if (PubEntAdminManager.SpecialVal2(li.Text.Replace(" ", "")) ||
                    PubEntAdminManager.SpecialVal2(li.Value.Replace(" ", "")))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            if (PubEntAdminManager.SpecialVal2(chboxMostCommonList.Text.Replace(" ", "")) )
            {
                Response.Redirect("InvalidInput.aspx");
            }
        }
        #endregion

        #region Date Validate

        protected void datevalidate()
        {
            string l_lbl = "";
            l_lbl += " Date 'Created From' can not be greater than Date 'Created To' ";
            errormsg.Text = l_lbl;
            errormsg.Visible = true;
        }

        protected void statusvalidate()
        {
            string l_lbl = "";
            l_lbl += "Please note that 'New' and 'Updated' both cannot be checked for search";
            errormsgnu.Text = l_lbl;
            errormsgnu.Visible = true;
        }
        #endregion
        #endregion
        
    }
}