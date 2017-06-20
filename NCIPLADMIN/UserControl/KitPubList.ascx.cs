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
using System.Collections.Generic;
using GlobalUtils;
using PubEntAdmin.BLL;
using PubEntAdmin.DAL;

namespace PubEntAdmin.UserControl
{
    public partial class KitPubList : System.Web.UI.UserControl
    {
        #region Constants
        private readonly string ChildPage = "KitPub.aspx";
        #endregion

        #region Controls
        protected UrlBuilder myUrlBuilder = new UrlBuilder(HttpContext.Current.Request.Url.AbsoluteUri, new Base64Encoder());
        protected bool blnIsVK;
        protected string s;
        #endregion

        #region Events Handling
        protected void Page_Load(object sender, EventArgs e)
        {
            string type = "";

            if (PubEntAdminManager.TamperProof)
            {
                if (myUrlBuilder.QueryString.ContainsKey(PubEntAdminManager.strVK_LPType))
                {
                    type = this.myUrlBuilder.QueryString[PubEntAdminManager.strVK_LPType];
                }
                //else
                //{
                //    PubEntAdminManager.UnathorizedAccess();
                //}
            }
            else
            {
                if (Request.QueryString[PubEntAdminManager.strVK_LPType] != null)
                {
                    type = Request.QueryString[PubEntAdminManager.strVK_LPType].ToString();
                }
                //else
                //{
                //    PubEntAdminManager.UnathorizedAccess();
                //}
            }

            if (type == PubEntAdminManager.strVKType)
            {
                this.blnIsVK = true;
            }
            else
            {
                this.blnIsVK = false;
            }

            if (!Page.IsPostBack)
            {
                if (type == PubEntAdminManager.strVKType)
                {
                    this.BindDataVK_LP(true);
                }
                else
                {
                    this.BindDataVK_LP(false);
                }
            }
        }

        protected void gvResult_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            List<Pub> dt = ((List<Pub>)this.gvResult.DataSource);
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Pub l_pub = dt[e.Item.ItemIndex];

                //if (l_pub.IsNCIPL && l_pub.IsROO)
                //{
                //    e.Item.Cells[2].Text = "NCIPL/ROO";
                //}
                if (l_pub.IsNCIPL && !l_pub.IsROO)
                {
                    e.Item.Cells[2].Text = PubEntAdminManager.NCIPL_INTERFACE;
                }
                else if (!l_pub.IsNCIPL && l_pub.IsROO)
                {
                    e.Item.Cells[2].Text = PubEntAdminManager.ROO_INTERFACE;
                }

                ////delete btn col
                Panel l_pnl = e.Item.Cells[4].Controls[1] as Panel;

                if (l_pub.IsVK)
                    ((Label)l_pnl.Controls[1]).Text = "Are you sure you want to delete this Virtual Kit<br />[" + 
                        l_pub.ProdID + @" - " + l_pub.Name + "] for "+ (l_pub.IsNCIPL?"NCIPL":"ROO") +" interface?";
                else
                    ((Label)l_pnl.Controls[1]).Text = "Are you sure you want to delete this Linked Pub<br />[" +
                        l_pub.ProdID + @" - " + l_pub.Name + "] for " + (l_pub.IsNCIPL ? "NCIPL" : "ROO") + " interface?";

            }
        }

        protected void gvResult_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandSource is Button)
            {
                if (e.CommandName == "Delete")
                {
                    int Id = (int)gvResult.DataKeys[e.Item.ItemIndex];
                    int cnt = PE_DAL.DeleteVK_LP(Id, e.Item.Cells[2].Text, System.Convert.ToInt32(this.IsVK));
                    BindDataVK_LP(this.IsVK);
                }
                else if (e.CommandName == "edit")
                {
                    int Id = (int)gvResult.DataKeys[e.Item.ItemIndex];
                    string interface_ = e.Item.Cells[2].Text;

                    if (PubEntAdminManager.TamperProof)
                    {
                        PubEntAdminManager.RedirectEncodedURLWithQS(ChildPage,
                            PubEntAdminManager.strPubID + "=" + Id.ToString() + "&" +
                            PubEntAdminManager.strInterface + "=" + this.WhatInt(interface_) +
                            "&" + PubEntAdminManager.strVK_LPType + "=" + (this.blnIsVK ? PubEntAdminManager.strVKType : PubEntAdminManager.strLPType));
                        
                    }
                    else
                    {
                        Response.Redirect(ChildPage + "?" + PubEntAdminManager.strPubID + "=" + Id.ToString() + "&" +
                            PubEntAdminManager.strInterface + "=" + this.WhatInt(interface_) +
                            "&" + PubEntAdminManager.strVK_LPType + "=" + (this.blnIsVK ? PubEntAdminManager.strVKType : PubEntAdminManager.strLPType), true);
                    }
                }
            }
            
        }
        #endregion

        #region Methods
        protected string WhatInt(string iInterface)
        {
            if (String.Compare(iInterface, PubEntAdminManager.NCIPL_INTERFACE, true) == 0)
                return PubEntAdminManager.NCIPL_INTERFACE;
            else if (String.Compare(iInterface, PubEntAdminManager.ROO_INTERFACE, true) == 0)
                return PubEntAdminManager.ROO_INTERFACE;

            return String.Empty;
        }

        protected void BindDataVK_LP(bool isVK)
        {
            this.gvResult.DataSource = PE_DAL.GetALLVK_LP(isVK);
            this.gvResult.DataKeyField = "PubID";
            this.gvResult.DataBind();
        }
        #endregion

        #region Properties
        protected bool IsVK
        {
            get { return this.blnIsVK; }
        }

        protected bool IsLP
        {
            get { return !(this.blnIsVK); }
        }
        #endregion
    }
}