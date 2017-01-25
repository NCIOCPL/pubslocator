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
using AjaxControlToolkit;
using PubEntAdmin.DAL;
using PubEntAdmin.BLL;

namespace PubEntAdmin.UserControl
{
    public partial class PubHistTabEditInfo : System.Web.UI.UserControl
    {
        #region Fields
        private int intPubID;
        int hidCnt = 0;
        #endregion

        #region Events Handling
        protected void Page_Load(object sender, EventArgs e)
        {
            this.BindValues();
            if (this.PubID > 0)
                this.BindData();
            
            this.SecVal();

            if (Session[PubEntAdminManager.strTabContentPrevActTabIndex] == null)
            {
                this.RegisterMonitoredChanges();
                this.ByPassRegisterMonitoredChanges();
                Session[PubEntAdminManager.strTabContentPrevActTabIndex] =
                    Session[PubEntAdminManager.strTabContentCurrActTabIndex];
            }
            else
            {
                if (Session[PubEntAdminManager.strTabContentPrevActTabIndex] !=
                    Session[PubEntAdminManager.strTabContentCurrActTabIndex])
                {
                    this.RegisterMonitoredChanges();
                    this.ByPassRegisterMonitoredChanges();
                    Session[PubEntAdminManager.strTabContentPrevActTabIndex] =
                    Session[PubEntAdminManager.strTabContentCurrActTabIndex];
                }
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            #region ClientScript
            ScriptManager.RegisterStartupScript(this,
                typeof(PubHistTabEditInfo), "PubHistTabEditInfo_ClientScript",
            @"
                function Dirty(sucio){
                    //alert(sucio );
                    document.getElementById(sucio).value = '1';
                }

                function isDate(sDate) {
                   var re = /^\d{1,2}\/\d{1,2}\/\d{4}$/
                   if (re.test(sDate)) {
                      var dArr = sDate.split('/');
                      var d = new Date(sDate);
                      return d.getMonth() + 1 == dArr[0] && d.getDate() == dArr[1] && d.getFullYear() == dArr[2];
                   }
                   else {
                      return false;
                   }
                }

                function isleap(yr)
                {
                 if ((parseInt(yr)%4) == 0)
                 {
                  if (parseInt(yr)%100 == 0)
                  {
                    if (parseInt(yr)%400 != 0)
                    {
                      return 28;
                    }
                    if (parseInt(yr)%400 == 0)
                    {
                      return 29;
                    }
                  }
                  if (parseInt(yr)%100 != 0)
                  {
                    return 29;
                  }
                 }
                 if ((parseInt(yr)%4) != 0)
                 {
                   return 28;
                 } 
                }

                function txtReceivedDateAndQtyVal(oSrc, args)
                {
                    var e = window.event;
                    if (e){
					    var targ
					    if (e.target) targ = e.target
					    else if (e.srcElement) targ = e.srcElement
					    if ((targ.id == '" + ((PubRecord)this.Page).GetSaveBtn1 + @"')||
                            (targ.id == '" + ((PubRecord)this.Page).GetSaveBtn2 + @"'))
					    {
                            if ((document.getElementById('" + this.txtReceivedDate.ClientID + @"').value.length>0)&&
                                (document.getElementById('" + this.txtQty.ClientID + @"').value.length<=0))
                            {
                                args.IsValid = false;
                                alert('Quantity Received is required.');                  
                            }
                            else if ((document.getElementById('" + this.txtReceivedDate.ClientID + @"').value.length<=0)&&
                                (document.getElementById('" + this.txtQty.ClientID + @"').value.length>0))
                            {
                                args.IsValid = false;
                                alert('Date Received is required.');
                            }
                            else
                            {
                                if ((document.getElementById('" + this.txtReceivedDate.ClientID + @"').value.length==0)&&
                                (document.getElementById('" + this.txtQty.ClientID + @"').value.length==0))
                                {
                                    if (" + this.ComboDatepickerOrigPub.DTComboState.ToString().ToLower() + @" && 
                                        document.getElementById('" + this.ComboDatepickerOrigPub.ddlMonth.ClientID + @"').value != 0)
                                    {
                                        args.IsValid = false;
                                        alert('Date Received and Quantity Received are required.'); 
                                    }
                                    else if (" + this.ComboDatepickerOrigPub.DTComboState.ToString().ToLower() + @" && 
                                        document.getElementById('" + this.ComboDatepickerOrigPub.ddlDay.ClientID + @"').value != 0)
                                    {
                                        args.IsValid = false;
                                        alert('Date Received and Quantity Received are required.'); 
                                    }
                                    else if (" + this.ComboDatepickerOrigPub.DTComboState.ToString().ToLower() + @" && 
                                        document.getElementById('" + this.ComboDatepickerOrigPub.ddlYear.ClientID + @"').value != 0)
                                    {
                                        args.IsValid = false;
                                        alert('Date Received and Quantity Received are required.'); 
                                    }
                                    else if (" + this.ckboxNoPubDate.Enabled.ToString().ToLower() + @" && 
                                        document.getElementById('" + this.ckboxNoPubDate.ClientID + @"').checked )
                                    {
                                        args.IsValid = false;
                                        alert('Date Received and Quantity Received are required.'); 
                                    }
                                    else if (" + this.ComboDatepicker_PubPrint.DTComboState.ToString().ToLower() + @" && 
                                        document.getElementById('" + this.ComboDatepicker_PubPrint.ddlMonth.ClientID + @"').value != 0)
                                    {
                                        args.IsValid = false;
                                        alert('Date Received and Quantity Received are required.'); 
                                    }
                                    else if (" + this.ComboDatepicker_PubPrint.DTComboState.ToString().ToLower() + @" && 
                                        document.getElementById('" + this.ComboDatepicker_PubPrint.ddlDay.ClientID + @"').value != 0)
                                    {
                                        args.IsValid = false;
                                        alert('Date Received and Quantity Received are required.'); 
                                    }
                                    else if (" + this.ComboDatepicker_PubPrint.DTComboState.ToString().ToLower() + @" && 
                                        document.getElementById('" + this.ComboDatepicker_PubPrint.ddlYear.ClientID + @"').value != 0)
                                    {
                                        args.IsValid = false;
                                        alert('Date Received and Quantity Received are required.'); 
                                    }
                                    else if (" + this.ComboDatepicker_PubRevise.DTComboState.ToString().ToLower() + @" && 
                                        document.getElementById('" + this.ComboDatepicker_PubRevise.ddlMonth.ClientID + @"').value != 0)
                                    {
                                        args.IsValid = false;
                                        alert('Date Received and Quantity Received are required.');         
                                    }
                                    else if (" + this.ComboDatepicker_PubRevise.DTComboState.ToString().ToLower() + @" && 
                                        document.getElementById('" + this.ComboDatepicker_PubRevise.ddlDay.ClientID + @"').value != 0)
                                    {
                                        args.IsValid = false;
                                        alert('Date Received and Quantity Received are required.'); 
                                    }
                                    else if (" + this.ComboDatepicker_PubRevise.DTComboState.ToString().ToLower() + @" && 
                                        document.getElementById('" + this.ComboDatepicker_PubRevise.ddlYear.ClientID + @"').value != 0)
                                    {
                                        args.IsValid = false;
                                        alert('Date Received and Quantity Received are required.'); 
                                    }
                                    else if (document.getElementById('" + this.txtNIHNum1.ClientID + @"').value.length >0)
                                    {
                                        args.IsValid = false;
                                        alert('Date Received and Quantity Received are required.'); 
                                    }
                                    else if (document.getElementById('" + this.txtNIHNum2.ClientID + @"').value.length >0)
                                    {
                                        args.IsValid = false;
                                        alert('Date Received and Quantity Received are required.'); 
                                    }
                                    else if (document.getElementById('" + this.txtArchiveDate.ClientID + @"').value.length >0)
                                    {
                                        args.IsValid = false;
                                        alert('Date Received and Quantity Received are required.'); 
                                    }
                                    
                                    else
                                    {
                                        args.IsValid = true;
                                    }
                                }
                                else
                                {
                                    args.IsValid = true;
                                }
                            }
                        }
                        else
                        {
                            args.IsValid = true;
                        }
                    }
                }

                function txtQtyVal(oSrc, args)
                {
                    var e = window.event;
                    if (e){
					    var targ
					    if (e.target) targ = e.target
					    else if (e.srcElement) targ = e.srcElement
					    if ((targ.id == '" + ((PubRecord)this.Page).GetSaveBtn1 + @"')||
                            (targ.id == '" + ((PubRecord)this.Page).GetSaveBtn2 + @"'))
					    {
                            if (document.getElementById('" + this.txtQty.ClientID + @"').value.length>0)
                                args.IsValid = true;
                            else
                            {
                                args.IsValid = false;
                                alert('Quantity Received is required.');
                            }
                        }
                        else
                        {
                            args.IsValid = true;
                        }
                    }
                }

                function ComboDatepickerOrigPubDateVal(oSrc, args)
                {
                    var e = window.event;
					var targ
                    if (e){
					    if (e.target) targ = e.target
					    else if (e.srcElement) targ = e.srcElement
					    if ((targ.id == '" + ((PubRecord)this.Page).GetSaveBtn1 + @"')||
                            (targ.id == '" + ((PubRecord)this.Page).GetSaveBtn2 + @"'))
					    {
                            var errormsg = '';
                            
                            if (" + this.ComboDatepickerOrigPub.DTComboState.ToString().ToLower() + @")
                            {
                                " + this.ComboDatepickerOrigPub.ClientScript() + @"    
                            }

                            if (errormsg.length == 0)
                                args.IsValid = true;
                            else
                            {
                                args.IsValid = false;
                                //alert(oSrc.errormessage);
                                oSrc.errormessage = errormsg;
                                alert(errormsg);
                            }
                        }
                        else
                        {
                            args.IsValid = true;
                        }
                    }
                }
           
                function ComboDatepicker_PubPrintDateVal(oSrc, args)
                {
                    var e = window.event;
			
                    if (e){
                        var targ
                
					    if (e.target) targ = e.target
					    else if (e.srcElement) targ = e.srcElement
					    if ((targ.id == '" + ((PubRecord)this.Page).GetSaveBtn1 + @"')||
                            (targ.id == '" + ((PubRecord)this.Page).GetSaveBtn2 + @"'))
					    {
                            var errormsg = '';
                            
                             " + this.ComboDatepicker_PubPrint.ClientScript() + @"

                            if (errormsg.length == 0)
                                args.IsValid = true;
                            else
                            {
                                args.IsValid = false;
                                //alert(oSrc.errormessage);
                                //oSrc.errormessage = errormsg;
                                alert(errormsg);
                            }
                        }
                        else
                        {
                            args.IsValid = true;
                        }
                    }
                }


                function ComboDatepicker_PubReviseDateVal(oSrc, args)
                {
                    var e = window.event;
                    if (e){
					    var targ
					    if (e.target) targ = e.target
					    else if (e.srcElement) targ = e.srcElement
					    if ((targ.id == '" + ((PubRecord)this.Page).GetSaveBtn1 + @"')||
                            (targ.id == '" + ((PubRecord)this.Page).GetSaveBtn2 + @"'))
					    {
                            var errormsg = '';
                            
                            " + this.ComboDatepicker_PubRevise.ClientScript() + @"

                            if (errormsg.length == 0)
                                args.IsValid = true;
                            else
                            {
                                args.IsValid = false;
                                //alert(oSrc.errormessage);
                                //oSrc.errormessage = errormsg;
                                alert(errormsg);
                            }
                        }
                        else
                        {
                            args.IsValid = true;
                        }
                    }
                }

                function ValNIH(sender, args) {
                    var e = window.event;
                    if (e){
					    var targ
					    if (e.target) targ = e.target
					    else if (e.srcElement) targ = e.srcElement
            
                        if ((targ.id == '" + ((PubRecord)this.Page).GetSaveBtn1 + @"')||
                            (targ.id == '" + ((PubRecord)this.Page).GetSaveBtn2 + @"'))
					    {
                            var n1 = document.getElementById('" + this.txtNIHNum1.ClientID + @"');
                            var n2 = document.getElementById('" + this.txtNIHNum2.ClientID + @"');
                        
                            if (n1.value.length != 0 && n2.value.length != 0)
                            {
                                var blnnih1 = true;
                                var blnnih2 = true;

                                var r = /\d{2}/g;
                                var r2 = /[a-zA-Z0-9]{3,5}/g;
                 
                                if (n1.value.length > 0)
                                { 
                                    if (!r.test(n1.value))
                                        blnnih1 = false;
                                }
                       
                                if (n2.value.length > 0)
                                { 
                                    if (!r2.test(n2.value))
                                        blnnih2 = false;
                                }

                                if (blnnih1 && blnnih2)
                                   args.IsValid = true;
                                else
                                {
                                    args.IsValid = false; 
                                    alert('Invalid NIH # format.');
                                }
                            }
                            else if ((n1.value.length == 0 && n2.value.length != 0) ||
                                (n1.value.length != 0 && n2.value.length == 0))
                            {
                                args.IsValid = false;
                                alert('Invalid NIH # format.');
                            }
                            else
                                args.IsValid = true;
                        }
                        else
                            args.IsValid = true;
                    }
                }
             ", true);

            #endregion
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (Page.IsValid)
            {
                if (this.PubID > 0)
                {
                    if (this.Save())
                    {
                        //save succ
                        if ((Session[PubEntAdminManager.strPubGlobalMode].ToString().ToLower() == PubEntAdminManager.strPubGlobalAMode) ||
                            (Session[PubEntAdminManager.strPubGlobalMode].ToString().ToLower() == PubEntAdminManager.strPubGlobalEMode))
                        {
                            //Session[PubEntAdminManager.strReloadPubHist] = "true";
                            this.BindOptions();
                        }

                        //if (PubEntAdminManager.TamperProof)
                        //{
                        //    PubEntAdminManager.RedirectEncodedURLWithQS("PubRecord.aspx","mode=view&pubid=" + this.PubID);
                        //}
                        //else
                        //{
                        //    Response.Redirect("PubRecord.aspx?mode=view&pubid=" + this.PubID, true);
                        //}
                    }
                    this.lblErrmsg.Text = String.Empty;
                }
                else
                {
                    this.Cleanup();
                    this.lblErrmsg.Text = "This publication has not been created.  Please add any history after creating this publication.";
                }
            }
        }

        protected void gvResult_ItemCreated(object source, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                TextBox l_txtReceivedDate = ((TextBox)e.Item.Cells[2].FindControl("txtReceivedDate_dg"));

                CalendarExtender CalExtReceivedDate = new CalendarExtender();
                CalExtReceivedDate.ID = "CalExtReceivedDate";
                CalExtReceivedDate.TargetControlID = l_txtReceivedDate.ID;
                CalExtReceivedDate.CssClass = "MyCalendar";
                CalExtReceivedDate.Format = "MM/dd/yyyy";

                MaskedEditExtender MaskedEditExtReceivedDate = new MaskedEditExtender();
                MaskedEditExtReceivedDate.ID = "MaskedEditExtReceivedDate";
                MaskedEditExtReceivedDate.TargetControlID = l_txtReceivedDate.ID;
                MaskedEditExtReceivedDate.Mask = "99/99/9999";
                MaskedEditExtReceivedDate.MaskType = MaskedEditType.Date;

                MaskedEditValidator MaskedEditValReceivedDate = new MaskedEditValidator();
                MaskedEditValReceivedDate.ID = "MaskedEditValReceivedDate";
                MaskedEditValReceivedDate.ControlToValidate = l_txtReceivedDate.ID;
                MaskedEditValReceivedDate.ControlExtender = MaskedEditExtReceivedDate.ID;
                MaskedEditValReceivedDate.Display = ValidatorDisplay.Dynamic;
                MaskedEditValReceivedDate.IsValidEmpty = true;
                MaskedEditValReceivedDate.EmptyValueMessage = "A Date is Required";
                MaskedEditValReceivedDate.InvalidValueMessage = "Ths date is invalid";

                //RequiredFieldValidator RequiredFieldValReceivedDate = new RequiredFieldValidator();
                //RequiredFieldValReceivedDate.ID = "RequiredFieldValReceivedDate";
                //RequiredFieldValReceivedDate.ControlToValidate = l_txtReceivedDate.ID;
                //RequiredFieldValReceivedDate.Display = ValidatorDisplay.Dynamic;
                //RequiredFieldValReceivedDate.ErrorMessage = "Required";

                e.Item.Cells[2].Controls.Add(CalExtReceivedDate);
                e.Item.Cells[2].Controls.Add(MaskedEditExtReceivedDate);
                e.Item.Cells[2].Controls.Add(MaskedEditValReceivedDate);
                //e.Item.Cells[2].Controls.Add(RequiredFieldValReceivedDate);
               

                TextBox l_txtQty = ((TextBox)e.Item.Cells[3].FindControl("txtQty_dg"));

                FilteredTextBoxExtender ftbeQty = new FilteredTextBoxExtender();
                ftbeQty.ID = "ftbeQty";
                ftbeQty.TargetControlID = l_txtQty.ID;
                ftbeQty.FilterType = FilterTypes.Numbers;

                RequiredFieldValidator RequiredFieldValQty = new RequiredFieldValidator();
                RequiredFieldValQty.ID = "RequiredFieldValQty";
                RequiredFieldValQty.ControlToValidate = l_txtQty.ID;
                RequiredFieldValQty.Display = ValidatorDisplay.Dynamic;
                RequiredFieldValQty.ErrorMessage = "Required";

                e.Item.Cells[3].Controls.Add(ftbeQty);
                e.Item.Cells[3].Controls.Add(RequiredFieldValQty);


                CustomValidator cusValNIHNum = new CustomValidator();
                cusValNIHNum.ID = "cusValNIHNum";
                cusValNIHNum.Display = ValidatorDisplay.Dynamic;
                cusValNIHNum.ErrorMessage = "Incorrect NIH # format";
                //cusValNIHNum.ClientValidationFunction = "NIHinDgVal";
                cusValNIHNum.ServerValidate += new ServerValidateEventHandler(cusValNIHNumInDG_ServerValidate);

                e.Item.Cells[4].Controls.Add(cusValNIHNum);

                CustomValidator cusValPubPrint = new CustomValidator();
                cusValPubPrint.ID = "cusValPubPrint";
                cusValPubPrint.Display = ValidatorDisplay.Dynamic;
                cusValPubPrint.ErrorMessage = "Incorrect Date Printed format";
                cusValPubPrint.ServerValidate += new ServerValidateEventHandler(cusValPubPrint_ServerValidate);
                e.Item.Cells[5].Controls.Add(cusValPubPrint);
              

                CustomValidator cusValPubRev = new CustomValidator();
                cusValPubRev.ID = "cusValPubRev";
                cusValPubRev.Display = ValidatorDisplay.Dynamic;
                cusValPubRev.ErrorMessage = "Incorrect Date Revised format";
                cusValPubRev.ServerValidate += new ServerValidateEventHandler(cusValPubRev_ServerValidate);
                e.Item.Cells[6].Controls.Add(cusValPubRev);
                
            }
        }

        protected void gvResult_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            PubHistCollection dt = ((PubHistCollection)this.gvResult.DataSource);

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                PubEntAdmin.BLL.PubHist l_conf = dt[e.Item.ItemIndex];

                HtmlInputHidden l_HtmlInputHidden = ((HtmlInputHidden)e.Item.Cells[2].FindControl("hiddenDrity_dg"));
                //l_HtmlInputHidden.Value = "0";

                TextBox l_txtReceivedDate = ((TextBox)e.Item.Cells[2].FindControl("txtReceivedDate_dg"));
                l_txtReceivedDate.Text = Server.HtmlEncode(l_conf.ReceivedDate.ToString("MM/dd/yyyy"));
                l_txtReceivedDate.Attributes.Add("onchange", "Dirty('" + l_HtmlInputHidden.ClientID + "')");

                //----------------------------------------
                 TextBox l_txtQty_dg = ((TextBox)e.Item.Cells[3].FindControl("txtQty_dg"));
                 l_txtQty_dg.Text = Server.HtmlEncode(l_conf.QtyReceived.ToString());
                 l_txtQty_dg.Attributes.Add("onchange", "Dirty('" + l_HtmlInputHidden.ClientID + "')");
                //----------------------------------------

                ((TextBox)e.Item.Cells[4].FindControl("txtNIHNum1_dg")).Text = Server.HtmlEncode(l_conf.NIHNum1);
                ((TextBox)e.Item.Cells[4].FindControl("txtNIHNum2_dg")).Text = Server.HtmlEncode(l_conf.NIHNum2);

                ((TextBox)e.Item.Cells[4].FindControl("txtNIHNum1_dg")).Attributes.Add("onchange", "Dirty('" + l_HtmlInputHidden.ClientID + "')");
                ((TextBox)e.Item.Cells[4].FindControl("txtNIHNum2_dg")).Attributes.Add("onchange", "Dirty('" + l_HtmlInputHidden.ClientID + "')");
                //----------------------------------------
                ComboDatepicker l_LastestPrintDate = ((ComboDatepicker)e.Item.Cells[5].FindControl("ComboDatepicker_PubPrint_dg"));

                if (l_conf.LatestPrintDate_Y > 0)
                {
                    l_LastestPrintDate.Month = Server.HtmlEncode(l_conf.LatestPrintDate_M.ToString());
                    l_LastestPrintDate.Day = Server.HtmlEncode(l_conf.LatestPrintDate_D.ToString());
                    l_LastestPrintDate.Year = Server.HtmlEncode(l_conf.LatestPrintDate_Y.ToString());
                }
                else
                {
                    l_LastestPrintDate.SetDefault();
                }

                l_LastestPrintDate.ddlMonth.Attributes.Add("onchange", "Dirty('" + l_HtmlInputHidden.ClientID + "')");
                l_LastestPrintDate.ddlDay.Attributes.Add("onchange", "Dirty('" + l_HtmlInputHidden.ClientID + "')");
                l_LastestPrintDate.ddlYear.Attributes.Add("onchange", "Dirty('" + l_HtmlInputHidden.ClientID + "')");
                //----------------------------------------
                ComboDatepicker l_RevisedDate = ((ComboDatepicker)e.Item.Cells[6].FindControl("ComboDatepicker_PubRevise_dg"));

                if (l_conf.RevisedDate_Y > 0)
                {
                    l_RevisedDate.Month = Server.HtmlEncode(l_conf.RevisedDate_M.ToString());
                    l_RevisedDate.Day = Server.HtmlEncode(l_conf.RevisedDate_D.ToString());
                    l_RevisedDate.Year = Server.HtmlEncode(l_conf.RevisedDate_Y.ToString());
                }
                else
                {
                    l_RevisedDate.SetDefault();
                }

                l_RevisedDate.ddlMonth.Attributes.Add("onchange", "Dirty('" + l_HtmlInputHidden.ClientID + "')");
                l_RevisedDate.ddlDay.Attributes.Add("onchange", "Dirty('" + l_HtmlInputHidden.ClientID + "')");
                l_RevisedDate.ddlYear.Attributes.Add("onchange", "Dirty('" + l_HtmlInputHidden.ClientID + "')");
                //----------------------------------------

                PubEntAdminManager.MonitorChanges2(this.Page, this, l_txtReceivedDate);
                PubEntAdminManager.MonitorChanges2(this.Page, this, l_txtQty_dg);
                PubEntAdminManager.MonitorChanges2(this.Page, this, e.Item.Cells[4].FindControl("txtNIHNum1_dg"));
                PubEntAdminManager.MonitorChanges2(this.Page, this, e.Item.Cells[4].FindControl("txtNIHNum2_dg"));
                PubEntAdminManager.MonitorChanges2(this.Page, this, l_LastestPrintDate.ddlMonth);
                PubEntAdminManager.MonitorChanges2(this.Page, this, l_LastestPrintDate.ddlDay);
                PubEntAdminManager.MonitorChanges2(this.Page, this, l_LastestPrintDate.ddlYear);
                PubEntAdminManager.MonitorChanges2(this.Page, this, l_RevisedDate.ddlMonth);
                PubEntAdminManager.MonitorChanges2(this.Page, this, l_RevisedDate.ddlDay);
                PubEntAdminManager.MonitorChanges2(this.Page, this, l_RevisedDate.ddlYear);

                e.Item.VerticalAlign = VerticalAlign.Bottom;
            }
        }

        protected void cusValNIHNumInDG_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DataGridItem l_DataGridItem = ((DataGridItem)((CustomValidator)(source)).NamingContainer);
            TextBox l_txtNIHNum1 = ((TextBox)l_DataGridItem.FindControl("txtNIHNum1_dg"));
            TextBox l_txtNIHNum2 = ((TextBox)l_DataGridItem.FindControl("txtNIHNum2_dg"));

            if (l_txtNIHNum1.Text.Trim().Length != 0 && l_txtNIHNum2.Text.Trim().Length != 0)
            {
                Regex r = new Regex(@"^\d{2}$");
                Regex r2 = new Regex(@"^[a-zA-Z0-9]{3,5}$");
                bool blnnih1 = false;
                bool blnnih2 = false;

                if (r.IsMatch(l_txtNIHNum1.Text.Trim()))
                    blnnih1 = true;

                if (r2.IsMatch(l_txtNIHNum2.Text.Trim()))
                    blnnih2 = true;

                if (blnnih1 && blnnih2)
                    args.IsValid = true;
                else
                    args.IsValid = false;
            }
            else if ((l_txtNIHNum1.Text.Trim().Length == 0 && l_txtNIHNum2.Text.Trim().Length != 0) ||
                (l_txtNIHNum1.Text.Trim().Length != 0 && l_txtNIHNum2.Text.Trim().Length == 0))
                args.IsValid = false;
            else
                args.IsValid = true;
        }

        protected void cusValPubPrint_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DataGridItem l_DataGridItem = ((DataGridItem)((CustomValidator)(source)).NamingContainer);
            ComboDatepicker l_ComboDatepicker_PubPrint_dg = ((ComboDatepicker)l_DataGridItem.Cells[5].Controls[1]);
            DropDownList l_ddlM = l_ComboDatepicker_PubPrint_dg.ddlMonth;
            DropDownList l_ddlD = l_ComboDatepicker_PubPrint_dg.ddlDay;
            DropDownList l_ddlY = l_ComboDatepicker_PubPrint_dg.ddlYear;

            string errormsg = "";
            string LabelName = "Publication Print Date";
            bool ret = true;

            if (l_ddlM.SelectedValue != "0" && l_ddlY.SelectedValue == "0")
            {
                if (errormsg.Length>0)
                    errormsg += '\n';

                ret = false;
                errormsg += LabelName+"(Year) is required";
            }
            else if (l_ddlD.SelectedValue != "0" &&
                (l_ddlY.SelectedValue == "0" ||
                 l_ddlM.SelectedValue == "0"))
            {
                if (errormsg.Length>0)
                        errormsg += '\n';

                ret = false;
                if (l_ddlY.SelectedValue == "0" &&
                    l_ddlM.SelectedValue == "0")
                    errormsg += LabelName+"(Year) and "+LabelName+@"(Month) are required";
                else if ((l_ddlY.SelectedValue == "0" && l_ddlM.SelectedValue != "0") ||
                        (l_ddlY.SelectedValue != "0" && l_ddlM.SelectedValue == "0"))
                     errormsg += "Invalid " + LabelName + @" format";
            }
            
            if (l_ddlD.SelectedValue != "0" &&
                l_ddlY.SelectedValue != "0" &&
                l_ddlM.SelectedValue != "0")
            {
                try
                {
                    DateTime t = System.Convert.ToDateTime(l_ddlM.SelectedValue + "/" +
                           l_ddlD.SelectedValue + "/" +
                           l_ddlY.SelectedValue);
                }
                catch (FormatException fex)
                {
                    ret = false;
                    if (errormsg.Length > 0)
                        errormsg += '\n';
                    errormsg += "Invalid " + LabelName + @" format";
                }
            }


            args.IsValid = ret;
        }

        protected void cusValPubRev_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DataGridItem l_DataGridItem = ((DataGridItem)((CustomValidator)(source)).NamingContainer);
            ComboDatepicker l_ComboDatepicker_PubRevise_dg = ((ComboDatepicker)l_DataGridItem.Cells[6].Controls[1]);
            DropDownList l_ddlM = l_ComboDatepicker_PubRevise_dg.ddlMonth;
            DropDownList l_ddlD = l_ComboDatepicker_PubRevise_dg.ddlDay;
            DropDownList l_ddlY = l_ComboDatepicker_PubRevise_dg.ddlYear;

            string errormsg = "";
            string LabelName = "Publication Revised Date";
            bool ret = true;

            if (l_ddlM.SelectedValue != "0" && l_ddlY.SelectedValue == "0")
            {
                if (errormsg.Length > 0)
                    errormsg += '\n';

                ret = false;
                errormsg += LabelName + "(Year) is required";
            }
            else if (l_ddlD.SelectedValue != "0" &&
                (l_ddlY.SelectedValue == "0" ||
                 l_ddlM.SelectedValue == "0"))
            {
                if (errormsg.Length > 0)
                    errormsg += '\n';

                ret = false;
                if (l_ddlY.SelectedValue == "0" &&
                    l_ddlM.SelectedValue == "0")
                    errormsg += LabelName + "(Year) and " + LabelName + @"(Month) are required";
                else if ((l_ddlY.SelectedValue == "0" && l_ddlM.SelectedValue != "0") ||
                        (l_ddlY.SelectedValue != "0" && l_ddlM.SelectedValue == "0"))
                    errormsg += "Invalid " + LabelName + @" format";
            }

            if (l_ddlD.SelectedValue != "0" &&
                l_ddlY.SelectedValue != "0" &&
                l_ddlM.SelectedValue != "0")
            {
                try
                {
                    DateTime t = System.Convert.ToDateTime(l_ddlM.SelectedValue + "/" +
                           l_ddlD.SelectedValue + "/" +
                           l_ddlY.SelectedValue);
                }
                catch (FormatException fex)
                {
                    ret = false;
                    if (errormsg.Length > 0)
                        errormsg += '\n';
                    errormsg += "Invalid " + LabelName + @" format";
                }
            }


            args.IsValid = ret;
        }
        #endregion

        #region Methods

        protected void BindOptions()
        {

            if (PE_DAL.GetProdHistCnt(this.PubID) > 0)//has hist already
            {
                this.ComboDatepickerOrigPub.DTComboState = false;
                //this.txtOrigPubDate.ReadOnly = true;
                //this.CalExtOrigPubDate.Enabled =
                //    this.MaskedEditExtOrigPubDate.Enabled =
                //    this.MaskedEditValOrigPubDate.Enabled = false;
            }
            else
            {
                this.ComboDatepickerOrigPub.DTComboState = true;
                //this.CalExtOrigPubDate.Enabled =
                //    this.MaskedEditExtOrigPubDate.Enabled =
                //    this.MaskedEditValOrigPubDate.Enabled = true;
            }
        }

        protected void BindValues()
        {
            int d = 0;
            int m = 0;
            int y = 0;

            if (this.PubID > 0)
                PE_DAL.GetProdOigDate(this.PubID, ref d, ref m, ref y);

            this.ComboDatepickerOrigPub.Month = m.ToString();
            this.ComboDatepickerOrigPub.Day = d.ToString();
            this.ComboDatepickerOrigPub.Year = y.ToString();

            if ((m != 0 && y != 0)|| (m != 0 && d != 0 && y != 0) ||y != 0)
            {
                this.ComboDatepickerOrigPub.DTComboState = this.ckboxNoPubDate.Enabled = false;
            }
            else
            {
                this.ComboDatepickerOrigPub.DTComboState = this.ckboxNoPubDate.Enabled = true;
            }
        }

        protected void BindData()
        {
            PubHistCollection l_phc = PE_DAL.GetProdHist(this.PubID);

            if (l_phc.Count > 0)
            {
                this.gvResult.DataSource = l_phc;
                this.gvResult.DataKeyField = "PHDID";
                this.gvResult.DataBind();
            }
            else
            {
                this.gvResult.Visible = false;
            }
        }

        private bool Update_Save()
        {
            ArrayList arrRes = new ArrayList();

            foreach (DataGridItem c in this.gvResult.Items)
            {
                if (c.ItemType == ListItemType.Item || c.ItemType == ListItemType.AlternatingItem)
                {
                    HtmlInputHidden l_hiddenDrity = ((HtmlInputHidden)c.FindControl("hiddenDrity_dg"));
                    if (l_hiddenDrity.Value != "0")
                    {
                        int l_phdid = System.Convert.ToInt32(c.Cells[0].Text);
                        string l_nih1 = ((TextBox)c.Cells[4].FindControl("txtNIHNum1_dg")).Text;
                        string l_nih2 = ((TextBox)c.Cells[4].FindControl("txtNIHNum2_dg")).Text;
                        string l_revised_M = ((ComboDatepicker)c.Cells[6].FindControl("ComboDatepicker_PubRevise_dg")).ddlMonth.SelectedValue;
                        string l_revised_D = ((ComboDatepicker)c.Cells[6].FindControl("ComboDatepicker_PubRevise_dg")).ddlDay.SelectedValue;
                        string l_revised_Y = ((ComboDatepicker)c.Cells[6].FindControl("ComboDatepicker_PubRevise_dg")).ddlYear.SelectedValue;
                        string l_print_M = ((ComboDatepicker)c.Cells[5].FindControl("ComboDatepicker_PubPrint_dg")).ddlMonth.SelectedValue;
                        string l_print_D = ((ComboDatepicker)c.Cells[5].FindControl("ComboDatepicker_PubPrint_dg")).ddlDay.SelectedValue;
                        string l_print_Y = ((ComboDatepicker)c.Cells[5].FindControl("ComboDatepicker_PubPrint_dg")).ddlYear.SelectedValue;
                        string l_rec = ((TextBox)c.Cells[2].FindControl("txtReceivedDate_dg")).Text;
                        string l_qty = ((TextBox)c.Cells[3].FindControl("txtQty_dg")).Text;

                        ///--------------------------------------
                        if ((!PubEntAdminManager.LenVal(l_rec, 10)) ||
                            (!PubEntAdminManager.LenVal(l_qty, 6)) ||
                            (!PubEntAdminManager.LenVal(l_nih1, 2)) ||
                            (!PubEntAdminManager.LenVal(l_nih2, 5)))
                        {
                            Response.Redirect("InvalidInput.aspx");
                        }
                        //---------------------------------------
                        if (l_qty.Trim().Length > 0)
                        {
                            if (!PubEntAdminManager.ContentNumVal(l_qty.Trim()))
                            {
                                Response.Redirect("InvalidInput.aspx");
                            }
                        }

                        if (l_rec.Trim().Length > 0)
                        {
                            if (!PubEntAdminManager.ContentDateVal(l_rec.Trim()))
                            {
                                Response.Redirect("InvalidInput.aspx");
                            }
                        }

                        if (l_nih1.Trim().Length > 0)
                        {
                            if (!PubEntAdminManager.ContentVal(l_nih1.Trim(), @"^\d{2}$"))
                            {
                                Response.Redirect("InvalidInput.aspx");
                            }
                        }

                        if (l_nih2.Trim().Length > 0)
                        {
                            if (!PubEntAdminManager.ContentVal(l_nih2.Trim(), @"^[a-zA-Z0-9]{3,5}$"))
                            {
                                Response.Redirect("InvalidInput.aspx");
                            }
                        }
                        //---------------------------------------
                        if ((PubEntAdminManager.OtherVal(l_rec)) ||
                            (PubEntAdminManager.OtherVal(l_qty)) ||
                            (PubEntAdminManager.OtherVal(l_nih1)) ||
                            (PubEntAdminManager.OtherVal(l_nih2)) ||
                            (PubEntAdminManager.OtherVal(l_print_M)) ||
                            (PubEntAdminManager.OtherVal(l_print_D)) ||
                            (PubEntAdminManager.OtherVal(l_print_Y)) ||
                            (PubEntAdminManager.OtherVal(l_revised_M)) ||
                            (PubEntAdminManager.OtherVal(l_revised_D)) ||
                            (PubEntAdminManager.OtherVal(l_revised_Y)))
                        {
                            Response.Redirect("InvalidInput.aspx");
                        }
                        //---------------------------------------
                        if ((PubEntAdminManager.SpecialVal2(l_rec.Replace(" ", ""))) ||
                            (PubEntAdminManager.SpecialVal2(l_qty.Replace(" ", ""))) ||
                            (PubEntAdminManager.SpecialVal2(l_nih1.Replace(" ", ""))) ||
                            (PubEntAdminManager.SpecialVal2(l_nih2.Replace(" ", ""))) ||
                            (PubEntAdminManager.SpecialVal2(l_print_M.Replace(" ", ""))) ||
                            (PubEntAdminManager.SpecialVal2(l_print_D.Replace(" ", ""))) ||
                            (PubEntAdminManager.SpecialVal2(l_print_Y.Replace(" ", ""))) ||
                            (PubEntAdminManager.SpecialVal2(l_revised_M.Replace(" ", ""))) ||
                            (PubEntAdminManager.SpecialVal2(l_revised_D.Replace(" ", ""))) ||
                            (PubEntAdminManager.SpecialVal2(l_revised_Y.Replace(" ", ""))))
                        {
                            Response.Redirect("InvalidInput.aspx");
                        }
                        ///--------------------------------------
                        bool res = PE_DAL.UpdateProdHist(l_phdid,this.PubID,
                            l_nih1.Trim().Length!=0?l_nih1.Trim():null,
                            l_nih2.Trim().Length!=0?l_nih2.Trim():null,
                            System.Convert.ToInt32(l_revised_M),
                            System.Convert.ToInt32(l_revised_D),
                            System.Convert.ToInt32(l_revised_Y),
                            System.Convert.ToInt32(l_print_M),
                            System.Convert.ToInt32(l_print_D),
                            System.Convert.ToInt32(l_print_Y),
                            l_rec.Trim().Length != 0? System.Convert.ToDateTime(l_rec.Trim()):DateTime.MinValue,
                            l_qty.Trim().Length != 0 ? System.Convert.ToInt32(l_qty.Trim()) : -1);

                        arrRes.Add(res.ToString());
                    }
                }
            }

            if (arrRes.Count == 0)
                return true;
            else
            {
                if (arrRes.Contains(Boolean.FalseString))
                    return false;
                else
                    return true;
            }
        }

        private bool PubHist_Save()
        {
            this.SecVal();
            bool pk = PE_DAL.SetProdHist(this.PubID,
                this.txtNIHNum1.Text.Trim().Length != 0 ? this.txtNIHNum1.Text.Trim() : null,
                this.txtNIHNum2.Text.Trim().Length != 0 ? this.txtNIHNum2.Text.Trim() : null,
                System.Convert.ToInt32(this.ComboDatepicker_PubRevise.Month),
                System.Convert.ToInt32(this.ComboDatepicker_PubRevise.Day),
                System.Convert.ToInt32(this.ComboDatepicker_PubRevise.Year),
                System.Convert.ToInt32(this.ComboDatepicker_PubPrint.Month),
                System.Convert.ToInt32(this.ComboDatepicker_PubPrint.Day),
                System.Convert.ToInt32(this.ComboDatepicker_PubPrint.Year),
                this.txtArchiveDate.Text.Trim().Length != 0 ? System.Convert.ToDateTime(this.txtArchiveDate.Text.Trim()) : DateTime.MinValue,
                this.txtReceivedDate.Text.Trim().Length != 0 ? System.Convert.ToDateTime(this.txtReceivedDate.Text.Trim()) : DateTime.MinValue,
                this.txtQty.Text.Trim().Length != 0 ? System.Convert.ToInt32(this.txtQty.Text.Trim()) : -1,
                System.Convert.ToInt32(this.ComboDatepickerOrigPub.Month),
                System.Convert.ToInt32(this.ComboDatepickerOrigPub.Day),
                System.Convert.ToInt32(this.ComboDatepickerOrigPub.Year),
                this.ckboxNoPubDate.Checked);

            bool update = false;

            if (pk)
            {
                this.Cleanup();

                update = this.Update_Save();
            }

            if (pk && update)
                return true;
            else
                return false;
        }

        public bool Save()
        {
            bool ret = false;

            this.SecVal();
            //Page.Validate();
            if (Page.IsValid)
            {
                if (this.PubID > 0)
                {
                    ret = this.PubHist_Save();
                    if (ret)
                    {
                        //save succ
                        if ((Session[PubEntAdminManager.strPubGlobalMode].ToString().ToLower() == PubEntAdminManager.strPubGlobalAMode) ||
                            (Session[PubEntAdminManager.strPubGlobalMode].ToString().ToLower() == PubEntAdminManager.strPubGlobalEMode))
                        {
                            //Session[PubEntAdminManager.strReloadPubHist] = "true";
                            this.BindOptions();
                        }

                        //if (PubEntAdminManager.TamperProof)
                        //{
                        //    PubEntAdminManager.RedirectEncodedURLWithQS("PubRecord.aspx","mode=view&pubid=" + this.PubID);
                        //}
                        //else
                        //{
                        //    Response.Redirect("PubRecord.aspx?mode=view&pubid=" + this.PubID, true);
                        //}
                    }
                    this.lblErrmsg.Text = String.Empty;
                }
                else
                {
                    this.Cleanup();
                    this.lblErrmsg.Text = "This publication has not been created.  Please add any history after creating this publication.";
                }
            }
            return ret;
        }

        protected void Cleanup()
        {
            
            this.txtArchiveDate.Text = String.Empty;
            this.ComboDatepicker_PubPrint.SetDefault();
            this.txtNIHNum1.Text = this.txtNIHNum2.Text = String.Empty;
            this.ComboDatepickerOrigPub.SetDefault();
            //this.txtOrigPubDate.Text = String.Empty;
            this.txtQty.Text = this.txtReceivedDate.Text = String.Empty;
            this.ComboDatepicker_PubRevise.SetDefault();
           
        }

        // ICallbackEventHandler Members
        //public string GetCallbackResult()
        //{
        //    return _callBackStatus;
        //}

        //public void RaiseCallbackEvent(string eventArgument)
        //{
        //    // do your validation coding here
        //    if (eventArgument == "junnark")
        //        _callBackStatus = "Valid";
        //} 

        #region MonitorChanges
        protected void RegisterMonitoredChanges()
        {
            PubEntAdminManager.MonitorChanges2(this.Page, this, this.txtReceivedDate);
            PubEntAdminManager.MonitorChanges2(this.Page, this, this.txtQty);
            PubEntAdminManager.MonitorChanges2(this.Page, this, this.ComboDatepickerOrigPub.ddlDay);
            PubEntAdminManager.MonitorChanges2(this.Page, this, this.ComboDatepickerOrigPub.ddlMonth);
            PubEntAdminManager.MonitorChanges2(this.Page, this, this.ComboDatepickerOrigPub.ddlYear);
            PubEntAdminManager.MonitorChanges2(this.Page, this, this.ckboxNoPubDate);
            PubEntAdminManager.MonitorChanges2(this.Page, this, this.ComboDatepicker_PubPrint.ddlDay);
            PubEntAdminManager.MonitorChanges2(this.Page, this, this.ComboDatepicker_PubPrint.ddlMonth);
            PubEntAdminManager.MonitorChanges2(this.Page, this, this.ComboDatepicker_PubPrint.ddlYear);
            PubEntAdminManager.MonitorChanges2(this.Page, this, this.ComboDatepicker_PubRevise.ddlDay);
            PubEntAdminManager.MonitorChanges2(this.Page, this, this.ComboDatepicker_PubRevise.ddlMonth);
            PubEntAdminManager.MonitorChanges2(this.Page, this, this.ComboDatepicker_PubRevise.ddlYear);
            PubEntAdminManager.MonitorChanges2(this.Page, this, this.txtNIHNum1);
            PubEntAdminManager.MonitorChanges2(this.Page, this, this.txtNIHNum2);
            PubEntAdminManager.MonitorChanges2(this.Page, this, this.txtArchiveDate);

        }

        protected void ByPassRegisterMonitoredChanges()
        {
            PubEntAdminManager.BypassModifiedMethod(this.btnAdd, false);
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
            if ((!PubEntAdminManager.LenVal(this.txtReceivedDate.Text, 10)) ||
                (!PubEntAdminManager.LenVal(this.txtQty.Text, 6)) ||
                (!PubEntAdminManager.LenVal(this.txtNIHNum1.Text, 2)) ||
                (!PubEntAdminManager.LenVal(this.txtNIHNum2.Text, 5)) ||
                (!PubEntAdminManager.LenVal(this.txtArchiveDate.Text, 10)) )
            {
                Response.Redirect("InvalidInput.aspx");
            }
        }

        private void TypeVal()
        {
            if (this.txtQty.Text.Trim().Length > 0)
            {
                if (!PubEntAdminManager.ContentNumVal(this.txtQty.Text.Trim()))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            if (this.txtReceivedDate.Text.Trim().Length > 0)
            {
                if (!PubEntAdminManager.ContentDateVal(this.txtReceivedDate.Text.Trim()))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            if (this.txtArchiveDate.Text.Trim().Length > 0)
            {
                if (!PubEntAdminManager.ContentDateVal(this.txtArchiveDate.Text.Trim()))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            if (this.txtNIHNum1.Text.Trim().Length > 0)
            {
                if (!PubEntAdminManager.ContentVal(this.txtNIHNum1.Text.Trim(),@"^\d{2}$"))
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
            if ((PubEntAdminManager.OtherVal(this.txtReceivedDate.Text)) ||
                (PubEntAdminManager.OtherVal(this.txtQty.Text)) ||
                (PubEntAdminManager.OtherVal(this.txtNIHNum1.Text)) ||
                (PubEntAdminManager.OtherVal(this.txtNIHNum2.Text)) ||
                (PubEntAdminManager.OtherVal(this.txtArchiveDate.Text)))
            {
                Response.Redirect("InvalidInput.aspx");
            }

            foreach (ListItem li in this.ComboDatepickerOrigPub.ddlMonth.Items)
            {
                if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in ComboDatepickerOrigPub.ddlDay.Items)
            {
                if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in ComboDatepickerOrigPub.ddlYear.Items)
            {
                if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in ComboDatepicker_PubPrint.ddlMonth.Items)
            {
                if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in ComboDatepicker_PubPrint.ddlDay.Items)
            {
                if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in ComboDatepicker_PubPrint.ddlYear.Items)
            {
                if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in ComboDatepicker_PubRevise.ddlMonth.Items)
            {
                if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in ComboDatepicker_PubRevise.ddlDay.Items)
            {
                if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in ComboDatepicker_PubRevise.ddlYear.Items)
            {
                if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }
        }

        private void SpecialVal()
        {
            
            if ((PubEntAdminManager.SpecialVal2(this.txtReceivedDate.Text.Replace(" ", ""))) ||
                (PubEntAdminManager.SpecialVal2(this.txtQty.Text.Replace(" ", ""))) ||
                (PubEntAdminManager.SpecialVal2(this.txtNIHNum1.Text.Replace(" ", ""))) ||
                (PubEntAdminManager.SpecialVal2(this.txtNIHNum2.Text.Replace(" ", ""))) ||
                (PubEntAdminManager.SpecialVal2(this.txtArchiveDate.Text.Replace(" ", ""))))
            {
                Response.Redirect("InvalidInput.aspx");
            }

            foreach (ListItem li in ComboDatepickerOrigPub.ddlMonth.Items)
            {
                if (PubEntAdminManager.SpecialVal2(li.Text.Replace(" ", "")) ||
                    PubEntAdminManager.SpecialVal2(li.Value.Replace(" ", "")))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in ComboDatepickerOrigPub.ddlDay.Items)
            {
                if (PubEntAdminManager.SpecialVal2(li.Text.Replace(" ", "")) ||
                    PubEntAdminManager.SpecialVal2(li.Value.Replace(" ", "")))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in ComboDatepickerOrigPub.ddlYear.Items)
            {
                if (PubEntAdminManager.SpecialVal2(li.Text.Replace(" ", "")) ||
                    PubEntAdminManager.SpecialVal2(li.Value.Replace(" ", "")))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in ComboDatepicker_PubPrint.ddlMonth.Items)
            {
                if (PubEntAdminManager.SpecialVal2(li.Text.Replace(" ", "")) ||
                    PubEntAdminManager.SpecialVal2(li.Value.Replace(" ", "")))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in ComboDatepicker_PubPrint.ddlDay.Items)
            {
                if (PubEntAdminManager.SpecialVal2(li.Text.Replace(" ", "")) ||
                    PubEntAdminManager.SpecialVal2(li.Value.Replace(" ", "")))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in ComboDatepicker_PubPrint.ddlYear.Items)
            {
                if (PubEntAdminManager.SpecialVal2(li.Text.Replace(" ", "")) ||
                    PubEntAdminManager.SpecialVal2(li.Value.Replace(" ", "")))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in ComboDatepicker_PubRevise.ddlMonth.Items)
            {
                if (PubEntAdminManager.SpecialVal2(li.Text.Replace(" ", "")) ||
                    PubEntAdminManager.SpecialVal2(li.Value.Replace(" ", "")))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in ComboDatepicker_PubRevise.ddlDay.Items)
            {
                if (PubEntAdminManager.SpecialVal2(li.Text.Replace(" ", "")) ||
                    PubEntAdminManager.SpecialVal2(li.Value.Replace(" ", "")))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in ComboDatepicker_PubRevise.ddlYear.Items)
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

        protected string GetSaveBtn
        {
            get { return ((PubRecord)this.Page).GetSaveBtn1; }
        }

        protected string GetSaveBtn2
        {
            get { return ((PubRecord)this.Page).GetSaveBtn2; }
        }
        #endregion

    }
}