﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PubEnt.BLL;
using PubEnt.DAL;
using System.Configuration;
using System.IO;
//using System.Data.SqlClient;
using System.Data;

namespace PubEnt
{
    public partial class detail_old : System.Web.UI.Page
    {
        private int _pubid, temp;
        private string _prodid;
        public string tocjavascript = "";
        public string tocfirst = "";
        public string hiddenornot = "";
        public string conferenceid = "";

        private void Page_Load(object sender, System.EventArgs e)
        {
            if (Request.QueryString["ConfID"] == null)
                throw new ArgumentException("Required parameter missing: ", "Conference ID");


            conferenceid = Request.QueryString["ConfID"].ToString();
            
            //Other important checks - redirect to default page
            if (Request.QueryString["prodid"] == null || Request.QueryString["prodid"].Length > 10)
                throw new ArgumentException("Missing parameter in detail.aspx", "value");

            //***EAC Hide ViewCart button or not...Better to do on PageLoad for each page than in Master.cs!
            if (Session["KIOSK_Qtys"].ToString() != "")
                Master.FindControl("btnViewCart").Visible = true;
            else
            Master.FindControl("btnViewCart").Visible = false;
            Master.FindControl("btnFinish").Visible = false;
            Master.FindControl("lblFreePubsInfo").Visible = false;

            ContinueSearch.Attributes.Add("onmousedown", "this.src='images/continuered_on.jpg'");
            ContinueSearch.Attributes.Add("onmouseup", "this.src='images/continuered_off.jpg'");
            OrderPublication.Attributes.Add("onmousedown", "this.src='images/addtocart_on.jpg'");
            OrderPublication.Attributes.Add("onmouseup", "this.src='images/addtocart_off.jpg'");
            EmailPublication.Attributes.Add("onmousedown", "this.src='images/addurl_on.jpg'");
            EmailPublication.Attributes.Add("onmouseup", "this.src='images/addurl_off.jpg'");


            if (!Page.IsPostBack)
            {
                //***EAC decided to use product ID instead on PUBID to dicourage users from guessing this param
                if (Request.QueryString["prodid"] != null)
                {
                    GlobalUtils.Utils UtilMethodClean = new GlobalUtils.Utils();
                    _prodid = UtilMethodClean.Clean(Request.QueryString["prodid"].ToString());
                    UtilMethodClean = null;

                    Product p = Product.GetPubByProductID(_prodid);

                    //Checking for a valid Product Id
                    if (p == null)
                        throw new ArgumentException("Unable to find product", "value");



                    //***EAC Everything checks out at this point...start populating fields

                    #region Load TOC images with Javascript
                    try
                    {
                        string tocUrl = ConfigurationManager.AppSettings["TOCImageURL"];
                        string tocpath = Server.MapPath(tocUrl);
                        string[] x = System.IO.Directory.GetFiles(tocpath, _prodid + "_toc*.jpg");

                        //***EAC TODO: you may have to pre-load images if images are delayed
                        tocjavascript = "tocmin=0;var i=tocmin;toc = new Array;";
                        if (x.Count() > 0)
                        {
                            tocfirst = "pubimages/kiosktocs/" + Path.GetFileName(x[0]);
                            tocjavascript = tocjavascript + "tocmax=" + x.Count().ToString() + "-1;";
                            for (int i = 0; i < x.Count(); i++)
                            {
                                tocjavascript += "toc[" + i.ToString() + "] = 'pubimages/kiosktocs/" + Path.GetFileName(x[i]) + "';";
                            }
                        }
                        else
                        {
                            tocfirst = "pubimages/kiosktocs/blank_toc.jpg";
                            tocjavascript = tocjavascript + "tocmax=0; toc[0]='pubimages/kiosktocs/blank_toc.jpg';";
                        }

                        if (x.Count() > 1)
                            hiddenornot = "arrowright_off.jpg";
                        else
                            hiddenornot = "arrowright_disabled.jpg";
                    }
                    catch (Exception myerr)
                    {
                        //do nothing..no images so what?!?  Just give them the text so they can still order using buttons
                    }

                    #endregion


                    lblTitle.Text = p.LongTitle;
                    lblFormat.Text = p.Format;
                    if (lblFormat.Text.Length <= 0)
                        lblFormatText.Visible = false;
                    lblNumPages.Text = p.NumPages;
                    if (lblNumPages.Text.Length <= 0)
                        lblNumPagesText.Visible = false;

                    lblAud.Text = p.Audience;
                    if (lblAud.Text.Length <= 0)
                        lblAudText.Visible = false;
                    lblLang.Text = p.Language;
                    if (lblLang.Text.Length <= 0)
                        lblLangText.Visible = false;

                    lblDesc.Text = p.Abstract;
                    if (lblDesc.Text.Length <= 0)
                        lblDescText.Visible = false;


                    string LastUpdatedMon = "";
                    string LastUpdatedDay = "";
                    string LastUpdatedYear = "";
                    string LastUpdatedText = "";

                    LastUpdatedMon = p.RevisedMonth;
                    LastUpdatedDay = p.RevisedDay;
                    LastUpdatedYear = p.RevisedYear;

                    if (LastUpdatedMon.Length > 0 && LastUpdatedDay.Length > 0 && LastUpdatedYear.Length > 0)
                        LastUpdatedText = LastUpdatedMon + " " + LastUpdatedDay + ", " + LastUpdatedYear;
                    else if (LastUpdatedMon.Length > 0 && LastUpdatedYear.Length > 0)
                        LastUpdatedText = LastUpdatedMon + " " + LastUpdatedYear;
                    else if (LastUpdatedYear.Length > 0)
                        LastUpdatedText = LastUpdatedYear;
                    else
                        LastUpdatedText = "";

                    lblLastupd.Text = LastUpdatedText;
                    lblLastupdText.Text = p.RevisedDateType;
                    if (lblLastupd.Text.Length <= 0)
                        lblLastupdText.Visible = false;
                    lblProductID.Text = p.ProductId;

                    lblNIH.Text = p.NIHNum;
                    if (lblNIH.Text.Length <= 0)
                        lblNIHText.Visible = false;
                    if (lblNIHText.Visible == true)
                    {
                        //if (p.NIHNum.Length > 7)
                        //    lblNIHText.Text = "NIH Number(s):&nbsp;";
                    }
                    string imagepath = ConfigurationManager.AppSettings["PubImagesURL"];

                    _pubid = p.PubId;


                    //Code for physical description
                    string Dimension = "";
                    string Color = "";
                    string Other = "";
                    //Get the physical description of the product from the database
                    IDataReader dr = DAL.DAL.GetPubPhysicalDesc(_pubid);
                    try
                    {
                        using (dr)
                        {
                            while (dr.Read())
                            {
                                Dimension = dr["DIMENSION"].ToString();
                                Color = dr["COLOR"].ToString();
                                Other = dr["OTHER"].ToString();
                            }
                        }
                    }
                    catch (Exception Ex)
                    {
                        //TO DO: log any error
                        if (!dr.IsClosed)
                            dr.Close();
                    }

                    //Assigning to label
                    string phydesc = "";
                    if (Dimension.Length > 0)
                        phydesc += Dimension + "; ";
                    if (Color.Length > 0)
                        phydesc += Color + "; ";
                    if (Other.Length > 0)
                        phydesc += Other + "; ";

                    char[] temparr = { ';', ' ' };

                    if (phydesc.Length > 2)
                        phydesc = phydesc.TrimEnd(temparr);

                    lblPhysicalDesc.Text = phydesc;

                    if (lblPhysicalDesc.Text.Length <= 0)
                        lblPhysicalDescText.Visible = false;


                    #region SHOW EBOOK URLS IF ANY
                    pnlHtmlPdf.Visible = false;
                    pnlKindle.Visible = false;
                    pnlOther.Visible = false;

                    string htmlpdf = DAL2.DAL.EbookUrl(p.PubId, "htmlpdf");
                    if (htmlpdf.Length > 0)
                    {
                        pnlHtmlPdf.Visible = true;
                        imgHtmlPdf.ImageUrl = Server.HtmlDecode("https://chart.googleapis.com/chart?cht=qr&chs=120x120&chld=M&choe=UTF-8&chl=" + htmlpdf);
                    }

                    string kindle = DAL2.DAL.EbookUrl(p.PubId, "kindle");
                    if (kindle.Length > 0)
                    {
                        pnlKindle.Visible = true;
                        imgKindle.ImageUrl = Server.HtmlDecode("https://chart.googleapis.com/chart?cht=qr&chs=120x120&chld=M&choe=UTF-8&chl=" + kindle);
                    }

                    string epub = DAL2.DAL.EbookUrl(p.PubId, "epub");
                    if (epub.Length > 0)
                    {
                        pnlOther.Visible = true;
                        imgOther.ImageUrl = Server.HtmlDecode("https://chart.googleapis.com/chart?cht=qr&chs=120x120&chld=M&choe=UTF-8&chl=" + epub);
                    }
                    #endregion 
                    //**EAC hide order/email buttons for now
                    OrderPublication.Visible = false;
                    EmailPublication.Visible = false;

                    if (p.OrderDisplayStatus.Contains("ORDER"))
                    {
                        OrderPublication.Visible = true;
                        OrderPublication.ImageUrl = "images/addtocart_off.jpg";
                        OrderPublication.Enabled = true;

                        //***EAC IS ITEM IN CART?
                        if (IsItemInCart(p.PubId.ToString()))
                        {
                            OrderPublication.ImageUrl = "images/alreadyincart.jpg";
                            OrderPublication.Enabled = false;
                        }

                        //***EAC TODO: CAN WE STILL ORDER?
                        if (LimitReached())
                        {
                            OrderPublication.ImageUrl = "images/limitreached.jpg";
                            OrderPublication.Enabled = false;
                        }
                    }
                    else if (p.OnlineDisplayStatus.Contains("ONLINE"))
                    {
                        EmailPublication.Visible = true;
                        EmailPublication.ImageUrl = "images/addurl_off.jpg";
                        EmailPublication.Enabled = true;

                        //***EAC IS ITEM IN CART?
                        if (IsItemInCart(p.PubId.ToString()))
                        {
                            EmailPublication.ImageUrl = "images/alreadyincart.jpg";
                            EmailPublication.Enabled = false;
                        }
                    }

                }
                else throw new ArgumentException("Missing parameter in detail.aspx", "value");



            }
        }



        private bool IsItemInCart(string currentPub)
        {
            bool IsInCart = false;

            if (Session["KIOSK_Pubs"] == null)
                return IsInCart;

            string[] pubs = (Session["KIOSK_Pubs"].ToString() + Session["KIOSK_Urls"].ToString()).Split(new Char[] { ',' });
            for (var i = 0; i < pubs.Length; i++)
            {
                if (pubs[i].Trim() != "")
                {
                    if (string.Compare(currentPub, pubs[i], true) == 0)
                    {
                        IsInCart = true;
                        break;
                    }
                }
            }

            return IsInCart;
        }

        private bool LimitReached()
        {
            bool IsInCart = false;

            if (string.IsNullOrEmpty(Session["KIOSK_Qtys"].ToString()) || string.IsNullOrEmpty(Session["KIOSK_ShipLocation"].ToString()))
                return IsInCart;
            int tot=0,lim = 0;
            string[] qtys = Session["KIOSK_Qtys"].ToString().Split(new Char[] { ',' });
            for (int i = 0; i < qtys.Length-1; i++)
            {
                tot += int.Parse(qtys[i]);
            }

            if (Session["KIOSK_ShipLocation"].ToString() == "Domestic")
                lim = int.Parse(ConfigurationManager.AppSettings["DomesticOrderLimit"]);
            if (Session["KIOSK_ShipLocation"].ToString() == "International")
                //lim = int.Parse(ConfigurationManager.AppSettings["InternationalOrderLimit"]);
                lim = PubEnt.DAL.DAL.GetIntl_MaxOrder(int.Parse(conferenceid));
            if (tot >= lim)
                return true;
            else
                return (false);
        }
        //END COPIED CODE###########################################



        protected void ContinueClick(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("kiosksearch.aspx?ConfID=" + Request.QueryString["ConfID"].ToString(), true); //EAC assume ConfID is supplied to all pages
        }

        protected void OrderPublication_Click(object sender, ImageClickEventArgs e)
        {
            //***EAC Dirty way, but it works.
            Session["KIOSK_Pubs"] += DAL.DAL.GetPubIdFromProductId(Request.QueryString["prodid"].ToString()) + ",";
            Session["KIOSK_Qtys"] += "1" + ",";
            if (Session["KIOSK_ShipLocation"] == null || Session["KIOSK_ShipLocation"].ToString().Trim() == "")
                Response.Redirect("location.aspx?ConfID=" + Request.QueryString["ConfID"].ToString(), true);
            else
                Response.Redirect("cart.aspx?ConfID=" + Request.QueryString["ConfID"].ToString(), true);
        }

        protected void EmailPublication_Click(object sender, ImageClickEventArgs e)
        {
            //***EAC Dirty way, but it works.
            Session["KIOSK_Urls"] += DAL.DAL.GetPubIdFromProductId(Request.QueryString["prodid"].ToString()) + ",";
            if (Session["KIOSK_ShipLocation"] == null || Session["KIOSK_ShipLocation"].ToString().Trim() == "")
                Response.Redirect("location.aspx?ConfID=" + Request.QueryString["ConfID"].ToString(), true);
            else
                Response.Redirect("cart.aspx?ConfID=" + Request.QueryString["ConfID"].ToString(), true);
        }
    }

}
