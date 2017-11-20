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
using System.IO;

using PubEntAdmin.BLL;
using PubEntAdmin.DAL;
using GlobalUtils;

namespace PubEntAdmin.UserControl
{
    public partial class Attachments : System.Web.UI.UserControl
    {
        
        #region Fields
        private int intPubID;
        protected UrlBuilder myUrlBuilder = new UrlBuilder(HttpContext.Current.Request.Url.AbsoluteUri, new Base64Encoder());
        #endregion

        #region Events Handling
        protected void Page_Load(object sender, EventArgs e)
        {
            this.SecVal();
            this.BindAttachments();

            if ((Session[PubEntAdminManager.strPubGlobalMode].ToString().ToLower() == PubEntAdminManager.strPubGlobalVMode)||
                (Session[PubEntAdminManager.strPubGlobalMode].ToString().ToLower() == PubEntAdminManager.strPubGlobalAMode))
            {
                this.pAttach.Visible = false;
            }
            else
            {
                this.pAttach.Visible = true;

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
                //this.RegisterMonitoredChanges();
            }
            
        }

        protected void grdAttachments_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                int fileId = (int)grdAttachments.DataKeys[e.Item.ItemIndex];
                int cnt = PE_DAL.DeleteAttachmentByFileID(fileId);

                BindAttachments();
            }
        }

        protected void grdAttachments_ItemDataBound(Object s, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Attachment currentFile = (Attachment)e.Item.DataItem;
                Label lblFileSize = (Label)e.Item.FindControl("lblFileSize");
                lblFileSize.Text = currentFile.FormatFileSize(currentFile.FileSize);
                HtmlImage fileOpenImage = (HtmlImage)e.Item.FindControl("FileOpenImage");
                fileOpenImage.Attributes.Remove("onclick");

                if (PubEntAdminManager.TamperProof)
                {
                    myUrlBuilder.PageName = "ShowFile.aspx";
                    myUrlBuilder.Query = PubEntAdminManager.strFileID + "=" + currentFile.AttachmentId.ToString();

                    fileOpenImage.Attributes.Add("onclick", "window.open('" + myUrlBuilder.ToString(true) + "','showattach','');");
                    
                }
                else
                {
                    fileOpenImage.Attributes.Add("onclick", "window.open('ShowFile.aspx?" +
                        PubEntAdminManager.strFileID + "=" + currentFile.AttachmentId.ToString() + "','showattach','');");
                }

                if ((string)Session[PubEntAdminManager.strPubGlobalMode] ==
                            PubEntAdminManager.strPubGlobalVMode)
                {
                    e.Item.Cells[5].Visible = false;
                }
            }
            else if (e.Item.ItemType == ListItemType.Header)
            {
                if ((string)Session[PubEntAdminManager.strPubGlobalMode] ==
                            PubEntAdminManager.strPubGlobalVMode)
                {
                    e.Item.Cells[5].Visible = false;
                }
            }
        }

        #endregion

        #region Methods
        public void BindAttachments()
        {
            if (this.PubID > 0)
            {
                grdAttachments.DataSource = PE_DAL.GetAttachmentsByPubID(this.PubID);
                grdAttachments.DataKeyField = "AttachmentId";
                grdAttachments.DataBind();
            }
        }

        public bool Save()
        {
            this.SecVal();
            return this.AttachSave();
        }

        protected bool AttachSave()
        {
            bool proceed = true;
            // Check to see if file was uploaded

            if (txtfilAttach.PostedFile != null)
            {
                // Get a reference to PostedFile object
                HttpPostedFile postedFile = txtfilAttach.PostedFile;

                //// Get size of uploaded file
                int fileLength = postedFile.ContentLength;
                if (fileLength > 4000000)
                {
                    lbl2big.Visible = true;
                    proceed = false;
                }

                if (proceed)
                {
                    //// make sure the size of the file is > 0
                    if (fileLength > 0)
                    {
                        // Allocate a buffer for reading of the file
                        byte[] fileData = new byte[fileLength];

                        // Read uploaded file from the Stream
                        postedFile.InputStream.Read(fileData, 0, fileLength);

                        // Create a name for the file to store
                        string fileName = Path.GetFileName(postedFile.FileName);

                        //Create new issue file in database.
                        int fileid = PE_DAL.SetAttachment(this.PubID,
                            ((CustomPrincipal)HttpContext.Current.User).UserID,
                            ((CustomPrincipal)HttpContext.Current.User).FullName,
                            fileName, fileLength, postedFile.ContentType, fileData);

                        //Rebind datagrid
                        this.BindAttachments();
                    }
                }
            }
            return proceed;
        }

        #region MonitorChanges
        protected void RegisterMonitoredChanges()
        {
            PubEntAdminManager.MonitorChanges2(this.Page, this, this.txtfilAttach);

        }
        #endregion

        #region Sec Val
        private void SecVal()
        {
            this.TagVal();
            this.SpecialVal();
        }

        private void TagVal()
        {
            if ((PubEntAdminManager.OtherVal(this.txtfilAttach.Value)))
            {
                Response.Redirect("InvalidInput.aspx");
            }
        }

        private void SpecialVal()
        {
            if ((PubEntAdminManager.SpecialVal2(this.txtfilAttach.Value.Replace(" ", ""))))
            {
                Response.Redirect("InvalidInput.aspx");
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
        #endregion

    }
}