using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using PubEntAdmin.BLL;
using PubEntAdmin.DAL;
using GlobalUtils;

namespace PubEntAdmin
{
    public partial class ShowFile : System.Web.UI.Page
    {
        #region Fields
        protected UrlBuilder myUrlBuilder = new UrlBuilder(HttpContext.Current.Request.Url.AbsoluteUri, new Base64Encoder());
        private int fileId = 0;
        #endregion

        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (PubEntAdminManager.TamperProof)
            {
                if (!myUrlBuilder.QueryString.ContainsKey(PubEntAdminManager.strFileID))
                //Request.QueryString["FileId"] == null || Request.QueryString["FileId"].Length < 1)
                {
                    this.spnMessage.InnerText = "Invalid Issue FileId Specified";
                    this.btnClose.Visible = true;
                }
                else
                {
                    try
                    {
                        fileId = int.Parse(this.myUrlBuilder.QueryString[PubEntAdminManager.strFileID]);
                    }
                    catch
                    {
                        spnMessage.InnerText = "Invalid Issue FileId Specified";
                        btnClose.Visible = true;
                        return;
                    }
                }
            }
            else
            {
                if (Request.QueryString[PubEntAdminManager.strFileID] == null ||
                    Request.QueryString[PubEntAdminManager.strFileID].Length < 1)
                {
                    this.spnMessage.InnerText = "Invalid Issue FileId Specified";
                    this.btnClose.Visible = true;
                }
                else
                {
                    try
                    {
                        fileId = int.Parse(Request.QueryString[PubEntAdminManager.strFileID]);
                    }
                    catch
                    {
                        spnMessage.InnerText = "Invalid Issue FileId Specified";
                        btnClose.Visible = true;
                        return;
                    }
                }
            }

            //Get the File data
            Attachment fileInfo = PE_DAL.GetAttachmentByFileID(fileId);

            //***EAC Let's see first if the person is allowed to view the document....
            //Project p = Aspen.Healthsys.Cisnet.BusinessLogicLayer.Project.GetProjectById(fileInfo.ProjectId);
            //if (p.CreatorId != ((CustomPrincipal)HttpContext.Current.User).UserID
            //    && !((CustomPrincipal)HttpContext.Current.User).IsInRole("Administrator")
            //    && !((CustomPrincipal)HttpContext.Current.User).IsInRole("Project Officer")
            //    )
            //{
            //    Response.Write("You are not allowed to view this file!");
            //    Response.End();
            //}

            // Clear Response buffer
            Response.Clear();

            // Set ContentType to the ContentType of our file
            Response.ContentType = fileInfo.ContentType;
            // Add a HTTP header to the output stream that specifies the default filename
            // for the browser's download dialog
            Response.AddHeader("Content-Disposition", "filename=" + fileInfo.FileName);
            // Add a HTTP header to the output stream that contains the 
            // content length(File Size). This lets the browser know how much data is being transfered
            Response.AddHeader("Content-Length", fileInfo.FileSize.ToString());

            //Response.AppendHeader(
            // Write data out of database into Output Stream

            //Byte[] tester = new byte[5]{100,121,55,66,44}; 
            //Response.OutputStream.Write(tester, 0, 5);

            Response.OutputStream.Write(fileInfo.FileData, 0, fileInfo.FileSize);
            Response.End();
        }
    }
}
