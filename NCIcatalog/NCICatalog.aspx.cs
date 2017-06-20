using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//Added
using NCICatalog.BLL;
using NCICatalog.DAL;
using NCICatalog.GlobalUtilities;
using NCICatalog.PDF;
using System.Configuration;

namespace NCICatalog
{
    public partial class NCICatalog : System.Web.UI.Page
    {
        //Variable Declarations
        CategoryCollection CatagoriesList;// = SQLDataAcccess.GetCategories();
        //Category CatagoryItem;
        CategoryCollection SubCatagoriesList;
        //Category SubCategoryItem;
        CategoryCollection SubSubCatagoriesList;
        //Category SubSubCategoryItem;

        Int32 NumCPubs; //Number of Catalog Pubs
        //CatalogPub CPub; //Catalog Pub

        CatalogPubsCollection CPubsListWithoutSubCategories;
        CatalogPubsCollection CPubsListWithoutSubSubCategories;
        CatalogPubsCollection CPubsListWithSubSubCategories;

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["NCI_CatalogColl"] = null; //Initialize
            Session["NCI_CancerTypeColl"] = null; //Initialize

            CategoryCollection CatagoriesList = SQLDataAcccess.GetCategories();
            foreach (Category CatagoryItem in CatagoriesList)
            {
                NumCPubs = SQLDataAcccess.GetNumPubsPerCategory(CatagoryItem.SubjectId);
                if (NumCPubs > 0) //Begin Here
                {
                    #region List Pubs Without SubCategories
                    /*Begin*/
                    CPubsListWithoutSubCategories = SQLDataAcccess.GetCatalogPubsWithoutSubCategories(CatagoryItem.SubjectId);
                    foreach (CatalogPub CPub in CPubsListWithoutSubCategories)
                    {
                        UtilityMethods.SaveCatalogsToSession(CatagoryItem.SubjectDesc,
                                                    "",
                                                    "",
                                                    CPub.CatalogWYNTK,
                                                    CPub.SpanishWYNTK,
                                                    CPub.IsFactSheet,
                                                    CPub.ProductId,
                                                    CPub.LongTitle,
                                                    CPub.Url,
                                                    CPub.Abstract,
                                                    CPub.Month,
                                                    CPub.Year,
                                                    CPub.Limit,
                                                    CPub.CatalogWYNTKCancerType);

                    }
                    /*End*/
                    #endregion

                    #region Pubs With SubCategories
                    /*Begin*/
                    SubCatagoriesList = SQLDataAcccess.GetSubCategories(CatagoryItem.SubjectId);
                    foreach (Category SubCategoryItem in SubCatagoriesList)
                    {
                        //Get Pubs without subsubcategories
                        CPubsListWithoutSubSubCategories = SQLDataAcccess.GetCatalogPubsWithoutSubSubCategories(CatagoryItem.SubjectId, SubCategoryItem.SubCategoryId);
                        foreach (CatalogPub CPub in CPubsListWithoutSubSubCategories)
                        {
                            UtilityMethods.SaveCatalogsToSession(CatagoryItem.SubjectDesc,
                                                    SubCategoryItem.SubCategoryDesc,
                                                    "",
                                                    CPub.CatalogWYNTK,
                                                    CPub.SpanishWYNTK,
                                                    CPub.IsFactSheet,
                                                    CPub.ProductId,
                                                    CPub.LongTitle,
                                                    CPub.Url,
                                                    CPub.Abstract,
                                                    CPub.Month,
                                                    CPub.Year,
                                                    CPub.Limit,
                                                    CPub.CatalogWYNTKCancerType);
                        }

                        //Get Pubs with subsubcategories
                        SubSubCatagoriesList = SQLDataAcccess.GetSubSubCategories(SubCategoryItem.SubCategoryId);
                        foreach (Category SubSubCategoryItem in SubSubCatagoriesList)
                        {

                            CPubsListWithSubSubCategories = SQLDataAcccess.GetCatalogPubsWithSubSubCategories(CatagoryItem.SubjectId, SubCategoryItem.SubCategoryId, SubSubCategoryItem.SubSubCategoryId);

                            foreach (CatalogPub CPub in CPubsListWithSubSubCategories)
                            {
                                UtilityMethods.SaveCatalogsToSession(CatagoryItem.SubjectDesc,
                                                    SubCategoryItem.SubCategoryDesc,
                                                    SubSubCategoryItem.SubSubCategoryDesc,
                                                    CPub.CatalogWYNTK,
                                                    CPub.SpanishWYNTK,
                                                    CPub.IsFactSheet,
                                                    CPub.ProductId,
                                                    CPub.LongTitle,
                                                    CPub.Url,
                                                    CPub.Abstract,
                                                    CPub.Month,
                                                    CPub.Year,
                                                    CPub.Limit,
                                                    CPub.CatalogWYNTKCancerType);
                            }
                        }

                    }
                    /*End*/
                    #endregion

                }                 //End Here  
            }

            //Get Cancer Types from the database and save to session
            UtilityMethods.SaveCancerTypesToSession();

            //End Get Cancer Types

            //To Test - Display Catalog Records
            //CatalogRecordCollection Coll = (CatalogRecordCollection)Session["NCI_CatalogColl"];
            //foreach (CatalogRecord CollItem in Coll)
            //{

            //    this.WriteHTMLCatalogRecord2(CollItem.Category,
            //                                    CollItem.SubCategory,
            //                                    CollItem.SubSubCategory,
            //                                    CollItem.CatalogWYNTK,
            //                                    CollItem.SpanishWYNTK,
            //                                    CollItem.IsFactSheet,
            //                                    CollItem.ProductId,
            //                                    CollItem.LongTitle,
            //                                    CollItem.Abstract,
            //                                    CollItem.URL,
            //                                    CollItem.MonthYear,
            //                                    CollItem.Limit);
            //}
            //Response.Write(Coll.Count.ToString());

            #region GetPDF

            //Create PDF and get the buffer
            PDFClass pdfClass = new PDFClass();
            //byte[] buff = pdfClass.CreatePDF(); //Whole PDF for test
            byte[] buff1 = pdfClass.CreatePDF2(1); ///Initial Pass - This one is for calculating table of contents. It will be discarded.

            Array.Resize<byte>(ref buff1, 0); //Clean up
            buff1 = null; //Clean up
           
            byte[] buff = pdfClass.CreatePDF2(2); ///Final Pass - Pass the buffered PDF to the output stream
            pdfClass = null; //Clean up

            //Output the buffer to the browser
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            if (string.Compare(ConfigurationManager.AppSettings["InLineAttachment"], "1") == 0)
                Response.AppendHeader("Content-Disposition", "inline; filename=NCICatalog.pdf");
            else
                Response.AppendHeader("Content-Disposition", "attachment; filename=NCICatalog.pdf"); //weird attachment name changes dynamically            
            Response.AppendHeader("Content-Length", buff.Length.ToString());
            Response.BinaryWrite(buff);

            Array.Resize<byte>(ref buff, 0); //Clean up
            buff = null; //Clean up

            Response.End();
            #endregion

        }
    }
}
