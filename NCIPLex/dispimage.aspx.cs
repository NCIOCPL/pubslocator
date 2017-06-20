using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//added
using NCIPLex.BLL;
using NCIPLex.DAL;
using System.Configuration;

namespace NCIPLex
{
    public partial class dispimage : System.Web.UI.Page
    {
        private string _prodid;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Missing Session
            if (Session["JSTurnedOn"] == null)
                Response.Redirect("PEAUHErrorScreen.htm?missingsession=true", true);
            //Other important checks - redirect to default page
            if (Request.QueryString["prodid"] == null)
                Response.Redirect("PEAUHErrorScreen.htm?redirect=dispimage", true);
            else if (Request.QueryString["prodid"].Length > 10)
                Response.Redirect("PEAUHErrorScreen.htm?redirect=dispimage", true);

            if (!Page.IsPostBack)
            {
                if (Request.QueryString["prodid"] != null)
                {
                    GlobalUtils.Utils UtilMethodClean = new GlobalUtils.Utils();
                    _prodid = UtilMethodClean.Clean(Request.QueryString["prodid"].ToString());
                    UtilMethodClean = null;

                    ProductCollection pColl = SQLDataAccess.GetProductbyProductIDWithRules(_prodid);

                    //Checking for a valid Product Id
                    if (pColl == null)
                        Response.Redirect("PEAUHErrorScreen.htm?redirect=dispimage", true);
                    else if (pColl.Pubs.Length == 0)
                        Response.Redirect("PEAUHErrorScreen.htm?redirect=dispimage", true);

                    Product p = pColl[0];

                    Image PubLargeImage = (Image)this.FindControl("PubLargeImage");
                    PubLargeImage.AlternateText = p.ShortTitle;
                    string imagepath = ConfigurationManager.AppSettings["PubLargeImagesURL"];
                    PubLargeImage.ImageUrl = imagepath + "/" + p.PubLargeImage;

                }
                else throw new ArgumentException("Missing parameter in dispimage.aspx", "value");
            }

        }
    }
}
