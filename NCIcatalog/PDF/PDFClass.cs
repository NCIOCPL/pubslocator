using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//added
using NCICatalog.DAL;
using PDFlib_dotnet;
using System.Configuration;
using NCICatalog.BLL;
using System.Collections; //For ArrayList

namespace NCICatalog.PDF
{   
    //The class contains various methods for generating the PDF
    public class PDFClass
    {
        //variable declarations
        private PDFlib _pdflib;
        private string _searchpath = ConfigurationManager.AppSettings["PDFlibSearchPath"];
        private string _loggingon = ConfigurationManager.AppSettings["PDFlibLogging"];
        private string _logpath = ConfigurationManager.AppSettings["PDFlibLogPath"];
        int _id, _id_artifact, _font;
        int _id_art, _id_sect1;
        int _tf;
        string pagesize = "width=letter.width height=letter.height"; //"width=a4.width height=a4.height";
        string fontopt = "fontname=Helvetica encoding=unicode fontsize=12";
        byte[] buffer;
        int textendX, textendY;
        int Yoffset=15;
        double dblTextYPos, dblTextHeight, dblTextWidth;

        private ArrayList arrlstCategory;
        private bool boolNewPage = false;
        private double MinHeight = 100;
        private CatalogRecordCollection collEnglishWYNTK;
        private CatalogRecordCollection collSpanishWYNTK;
        private bool boolHasWYNTK = false;
        private ArrayList arrlstTOC = new ArrayList(); //Table of contents text array.
        private ArrayList arrlstTOCPageNum = new ArrayList(); //Table of contents page number;

        private int destnamecounter = 0;
        private CatalogRecordCollection collNonWYNTK;

        //constructor
        public PDFClass() {}

        //Get the data from the Session collection and create the PDF output
        //Buffer the output and return to the caller.

        public byte[] CreatePDF2(int mode) //Mode 1 - for throwing away, //Mode 2 - for final pdf
        {
            //if the y co-ordinate (after a textflow or table write) is lesser than or equal to minheight, then PDFlib
            //is forced to continue with a new page
            MinHeight = Double.Parse(ConfigurationManager.AppSettings["MinHeight"]);

            _pdflib = new PDFlib();

            try // 'catch' can be found on line 1585
            {

                string licensekey = ConfigurationManager.AppSettings["PDFlibLicenseKey"];
                //_pdflib.set_parameter("license", "W800102-019000-121351-PESUF2-MX4ND2");
                _pdflib.set_parameter("license", licensekey);
                _pdflib.set_parameter("errorpolicy", "return");
                if (string.Compare(_loggingon, "1") == 0)
                    _pdflib.set_parameter("logging", "filename={" + _logpath + @"\" + "pdflib.log} remove");
                _pdflib.set_parameter("SearchPath", _searchpath);

                if (_pdflib.begin_document("", "tagged=true lang=en ") == -1)
                    throw new Exception("Error: " + _pdflib.get_errmsg());

                _pdflib.set_info("Subject", "Cancer");
                _pdflib.set_info("Creator", "NCI Catalog"); //the app name (not needed for Section 508)
                _pdflib.set_info("Title", "National Cancer Institute Publications Catalog and Order Form");
                _pdflib.set_info("Author", "National Cancer Institute");
                _pdflib.set_info("Keywords", "Treatment Options, Clinical Trials, Coping with Cancer, Testing for Cancer, Nutrition, Genetics, Tobacco/Smoking, Research, Cancer");

                _pdflib.set_parameter("autospace", "true");

                //Open a structure element of type "Article"*/
                _id_art = _pdflib.begin_item("Art", "Title=Article");

                //Begin a structure element of type Section for all text contents
                _id_sect1 = _pdflib.begin_item("Sect", "Title={Main Section}");

                /*************************************HOPEFULLY ALL CODE CAN GO IN HERE*/

                double llx = 60, lly = 50, urx = 570, ury = 745; //Main Position Variables
                int pagecounter = 2; // 1; //page counter
                int artifact_font; //artifact font
                string tf_text = "";
                int font, fsize = 8;
                string result = "";

                /*Some table variables*/
                int _tbl = -1;
                string optlist, optlistHeaderCol1, optlistHeaderCol2, optlistBodyCol1, optlistBodyCol2;
                string linebreak = "\n";
                string alignment;
                /*End of some table variables*/

                string tf_tempoptlist = "charref fontname=Helvetica-Bold encoding=unicode fontsize=8 ";

                if (mode == 2)
                    this.create_toc2(ref _pdflib); //Add the TOC page

                #region first_page
                _pdflib.begin_page_ext(0, 0, pagesize); //begin the page
                #endregion

                arrlstCategory = GetCategoryArray();
                foreach (string CategoryName in arrlstCategory) //Begin Main Outer Category Loop
                {

                    #region TextFlow - Category Name

                    tf_text = CategoryName.ToUpper();
                    _tf = -1;

                    tf_tempoptlist = "charref fontname=Arial encoding=unicode fontsize=9 fontstyle=bold "
                                        + "matchbox={name={" + tf_text + "}} fillcolor={#00006A} charspacing=3 ";
                    arrlstTOC.Add(tf_text); //add to the table of contents arraylist

                    _tf = _pdflib.add_textflow(_tf, tf_text + linebreak, tf_tempoptlist); //add textflow
                    if (_tf == -1)
                        throw new Exception("Error: " + _pdflib.get_errmsg());

                    #region Blind Text Flow
                    do
                    {
                        /*Begin available height check*/
                        boolNewPage = false; //new page flag
                        if (ury <= MinHeight) //Min height is a pre-defined value
                        {
                            boolNewPage = true; //set to true if min height condition is satisfied
                            #region new_page
                            this.DrawFooter(ref _pdflib, ref pagecounter);

                            _pdflib.end_page_ext(""); //end current page

                            pagecounter++;
                            _pdflib.begin_page_ext(0, 0, pagesize); //Begin a new page
                            llx = 60; lly = 50; urx = 570; ury = 745; //Reset Position Variables
                            #endregion
                            continue; //proceed to next iteration
                        }
                        /*End avaliable height check*/

                        result = _pdflib.fit_textflow(_tf, llx, lly, urx, ury, "blind"); //blind fit textflow

                        if (result == "_boxfull")
                        {
                            #region new_page
                            this.DrawFooter(ref _pdflib, ref pagecounter);

                            _pdflib.end_page_ext(""); //end current page

                            pagecounter++;
                            _pdflib.begin_page_ext(0, 0, pagesize); //Begin a new page
                            llx = 60; lly = 50; urx = 570; ury = 745; //Reset Position Variables
                            #endregion
                        }
                    } while (result == "_boxfull" || boolNewPage);

                    /* Check for errors */
                    if (result != "_stop")
                    {
                        /* "_boxempty" happens if the box is very small and doesn't
                         * hold any text at all.
                         */
                        if (result == "_boxempty")
                            throw new Exception("Error: Textflow box too small");
                        else
                        {
                            /* Any other return value is a user exit caused by
                             * the "return" option; this requires dedicated code to
                             * deal with.
                             */
                            throw new Exception("User return '" + result +
                                    "' found in Textflow");
                        }
                    }

                    #endregion

                    #region Background Color Rectangle

                    dblTextYPos = _pdflib.info_textflow(_tf, "textendy"); //Get Y Pos of text flow (it's the upper y)
                    //dblTextHeight = _pdflib.info_textflow(_tf, "textheight"); //height of text
                    //dblTextWidth = _pdflib.info_textflow(_tf, "textwidth"); //width of text

                    /*Begin - Draw a rectangle with the retrieved width and height */
                    //_pdflib.setcolor("fill", "rgb", 0.0, 0.8, 0.8, 0.0);
                    _pdflib.setcolor("fillstroke", "#A7A7A7", 0.0, 0.0, 0.0, 0.0);
                    //_pdflib.rect(0, dblTextYPos, dblTextWidth, dblTextHeight);
                    _pdflib.rect(0, dblTextYPos + 3, 572, 20); //Can be fixed also
                    _pdflib.fill();
                    /*End - Draw rectangle*/
                    #endregion


                    #region Place Text Flow
                    /*Place Text Flow*/
                    /* Loop until all of the text is placed; create new pages
                    * as long as more text needs to be placed. Two columns will
                    * be created on all pages.
                    */
                    do
                    {
                        /*Begin available height check*/
                        boolNewPage = false; //new page flag
                        if (ury <= MinHeight) //Min height is a pre-defined value
                        {
                            boolNewPage = true; //set to true if min height condition is satisfied
                            #region new_page
                            this.DrawFooter(ref _pdflib, ref pagecounter);

                            _pdflib.end_page_ext(""); //end current page

                            pagecounter++;
                            _pdflib.begin_page_ext(0, 0, pagesize); //Begin a new page
                            llx = 60; lly = 50; urx = 570; ury = 745; //Reset Position Variables
                            #endregion
                            continue; //proceed to next iteration
                        }
                        /*End avaliable height check*/

                        tf_tempoptlist = "charref fontname=Helvetica-Bold encoding=winansi fontsize=8 ";

                        result = _pdflib.fit_textflow(_tf, llx, lly, urx, ury, "rewind=-1"); //fit textflow

                        /* "_boxfull" means we must continue because there is more text;
                         * "_nextpage" is interpreted as "start new column"
                         */
                        if (result == "_boxfull")
                        {
                            #region new_page
                            this.DrawFooter(ref _pdflib, ref pagecounter);

                            _pdflib.end_page_ext(""); //end current page

                            pagecounter++;
                            _pdflib.begin_page_ext(0, 0, pagesize); //Begin a new page
                            llx = 60; lly = 50; urx = 570; ury = 745; //Reset Position Variables
                            #endregion
                        }
                    } while (result == "_boxfull" || boolNewPage);

                    /* Check for errors */
                    if (result != "_stop")
                    {
                        /* "_boxempty" happens if the box is very small and doesn't
                         * hold any text at all.
                         */
                        if (result == "_boxempty")
                            throw new Exception("Error: Textflow box too small");
                        else
                        {
                            /* Any other return value is a user exit caused by
                             * the "return" option; this requires dedicated code to
                             * deal with.
                             */
                            throw new Exception("User return '" + result +
                                    "' found in Textflow");
                        }
                    }
                    #endregion

                    textendY = (int)_pdflib.info_textflow(_tf, "textendy");
                    _pdflib.delete_textflow(_tf);
                    //ury -= fsize * 1.2; //ury starts with max value (at top) and is reduced 
                    //as text is written from top to bottom
                    ury = textendY; //To get text closer this can be ury = textendY;
                    /*End Place Text Flow*/

                    arrlstTOCPageNum.Add(pagecounter.ToString()); //TOC Page Number

                    #endregion

                    #region Bookmark For Categories
                    if (mode == 2)
                    {
                        double uryadjust = 21; //used to adjust ury (of the category) so that the bookmark's destination position is above the category name
                        string bookmarkdest = "destination" + destnamecounter.ToString(); //define a name for the named destination - no space allowed
                        _pdflib.add_nameddest(bookmarkdest, "type=fixed left=" + 0 + " top=" + (ury + uryadjust)); //add the named destination
                        int bookmarkaction = _pdflib.create_action("GoTo", " destname=" + bookmarkdest);
                        _pdflib.create_bookmark(CategoryName, " action={activate=" + bookmarkaction + "}");
                        destnamecounter++; //increment the counter to get a unique destination name everytime the bookmark is created
                    }
                    #endregion

                    CatalogRecordCollection Coll = getPubsPerCategory(CategoryName);

                    #region - Category Pubs without SubCategory or SubSubCategory
                    /*Begin Category Pubs without SubCategory or SubSubCategory*/
                    if (Coll.Count > 0) //Category Pubs without SubCategory or SubSubCategory
                    {
                        collEnglishWYNTK = new CatalogRecordCollection();
                        collSpanishWYNTK = new CatalogRecordCollection();
                        collNonWYNTK = new CatalogRecordCollection();

                        //Begin Data Cell - Begin Loop
                        foreach (CatalogRecord CollItem in Coll)
                        {
                            //Check for WYNTK
                            boolHasWYNTK = false;
                            if (string.Compare(CollItem.CatalogWYNTK, "1") == 0)
                            {
                                collEnglishWYNTK.Add(CollItem);
                                boolHasWYNTK = true;

                            }
                            if (string.Compare(CollItem.SpanishWYNTK, "1") == 0)
                            {
                                collSpanishWYNTK.Add(CollItem);
                                boolHasWYNTK = true;
                            }
                            //if (boolHasWYNTK)
                            //    continue; //Add the row to table only if the record is not WYNTK
                            
                            if (!boolHasWYNTK) 
                                collNonWYNTK.Add(CollItem); //This is a non WYNTK Pub
                            //End Check for WYNKT

                        }
                        //End Data Cell - End Loop


                        if (collNonWYNTK.Count > 0) //Draw table only if there is atleast one pub
                        {
                            /*begin table structure elements*/
                            int _id_table, _id_thead, _id_row, _id_th, _id_body, _id_td, curr_row, curr_col;
                            _id_table = _pdflib.begin_item("Table", "Title={Catalogs Table}"); //Begin Table
                            _id_thead = _pdflib.begin_item("THead", "Title={Header}"); //begin header
                            _id_row = _pdflib.begin_item("TR", "Title={Header Row}"); //header row

                            string tf_optlist;
                            string text;

                            #region table_definition
                            /*Begin Header Cells*/
                            font = _pdflib.load_font("Helvetica-Bold", "unicode", ""); //Set a font type
                            if (font == -1)
                                throw new Exception("Error: " + _pdflib.get_errmsg());

                            alignment = "left";
                            optlistHeaderCol1 = "fittextline={position=" + alignment + " font=" + font + " fontsize=9 fillcolor={gray 0}} ";

                            curr_col = 1; curr_row = 1; _tbl = -1; //initialize

                            _id_th = _pdflib.begin_item("TH", "Scope=Column");
                            //_id_th = _pdflib.begin_item("TH", "");
                            _tbl = _pdflib.add_table_cell(_tbl, curr_col, curr_row, "Title/Description", optlistHeaderCol1 + " colwidth=90% margin=3");
                            if (_tbl == -1)
                                throw new Exception("Error: " + _pdflib.get_errmsg());
                            _pdflib.end_item(_id_th);

                            alignment = "right"; //assign a new value for alignment
                            optlistHeaderCol2 =
                                        "fittextline={position=" + alignment + " font=" + font + " fontsize=9 fillcolor={gray 0} } ";

                            curr_col++; curr_row = 1;
                            _id_th = _pdflib.begin_item("TH", "Scope=Column");
                            //_id_th = _pdflib.begin_item("TH", "");
                            _tbl = _pdflib.add_table_cell(_tbl, curr_col, curr_row, "Inventory Number", optlistHeaderCol1 + " colwidth=10% margin=3");
                            if (_tbl == -1)
                                throw new Exception("Error: " + _pdflib.get_errmsg());
                            _pdflib.end_item(_id_th);
                            /*End Header Cells*/

                            _pdflib.end_item(_id_row); //End header row
                            _pdflib.end_item(_id_thead); //End Header
                            _id_body = _pdflib.begin_item("TBody", "Title={Body}"); //Begin Body

                            /*Begin Data Cells*/
                            font = _pdflib.load_font("Helvetica", "unicode", ""); //Set a font type
                            if (font == -1)
                                throw new Exception("Error: " + _pdflib.get_errmsg());
                            optlistBodyCol1 =
                                    "fittextline={position=left font=" + font + " fontsize=10} ";
                            optlistBodyCol2 =
                                    "fittextline={position=left font=" + font + " fontsize=10} ";

                            

                            /*End of table structure elements*/
                            #endregion

                            foreach (CatalogRecord CollItem in collNonWYNTK)
                            {
                                _id_row = _pdflib.begin_item("TR", "Title={Body Row}"); //body row

                                curr_row++;

                                #region Column 1

                                curr_col = 1; //column number
                                _tf = -1; //initialize text flow object handle

                                tf_optlist = "charref fontname=Helvetica-Bold encoding=winansi fontsize=8 ";

                                ////Initial line break 
                                //_tf = _pdflib.add_textflow(_tf, linebreak, tf_optlist);
                                //if (_tf == -1)
                                //    throw new Exception("Error: " + _pdflib.get_errmsg());

                                //Category, SubCategory, SubSubCategory
                                #region Debug (Uncomment)
                                //if (CollItem.Category.Length > 0 && CollItem.SubCategory.Length > 0 && CollItem.SubSubCategory.Length > 0)
                                //    text = "Category: " + CollItem.Category + @"/" + CollItem.SubCategory + @"/" + CollItem.SubSubCategory;
                                //else if (CollItem.Category.Length > 0 && CollItem.SubCategory.Length > 0)
                                //    text = "Category: " + CollItem.Category + @"/" + CollItem.SubCategory;
                                //else
                                //    text = "Category: " + CollItem.Category;


                                ////_tf = _pdflib.add_textflow(_tf, "Data Row with one column. Row number is: " + curr_row.ToString() + linebreak, tf_optlist);
                                //_tf = _pdflib.add_textflow(_tf, text + linebreak, tf_optlist);
                                //if (_tf == -1)
                                //    throw new Exception("Error: " + _pdflib.get_errmsg());
                                #endregion

                                //Title
                                tf_optlist = "charref fontname=Helvetica-Bold encoding=unicode fontsize=10";
                                text = CollItem.LongTitle;
                                if (text.Length > 0)
                                {
                                    _tf = _pdflib.add_textflow(_tf, text + linebreak, tf_optlist);
                                    if (_tf == -1)
                                        throw new Exception("Error: " + _pdflib.get_errmsg());
                                }
                                #region URL
                                string url = CollItem.URL;
                                if (url.Length > 0)
                                {
                                    int normalfont = _pdflib.load_font("Helvetica", "unicode", "");
                                    string optlistLink =
                                        "font=" + normalfont + " fontsize=9 " +
                                        "matchbox={name=lnkURL} fillcolor={#00006A} underline=false";

                                    _tf = _pdflib.add_textflow(_tf, url, optlistLink);
                                    if (_tf == -1)
                                        throw new Exception("Error: " + _pdflib.get_errmsg());

                                    /* create URI action */
                                    optlistLink = "url={" + url + "}";
                                    int act = _pdflib.create_action("URI", optlistLink);

                                    /* create Link annotation on matchbox "kraxi" */
                                    optlistLink = "action={activate " + act + "} linewidth=0 usematchbox={lnkURL}";
                                }
                                #endregion

                                //Add a space and a line break after URL
                                if (url.Length > 0)
                                {
                                    _tf = _pdflib.add_textflow(_tf, " " + linebreak, tf_optlist);
                                    if (_tf == -1)
                                        throw new Exception("Error: " + _pdflib.get_errmsg());
                                }

                                //Abstract and Month Year
                                tf_optlist = "charref fontname=Helvetica encoding=unicode fontsize=9 fillcolor={rgb 0 0 0} leading=30%";
                                _tf = _pdflib.add_textflow(_tf, "\n", tf_optlist);
                                if (_tf == -1)
                                    throw new Exception("Error: " + _pdflib.get_errmsg());

                                tf_optlist = "charref fontname=Helvetica encoding=unicode fontsize=9 fillcolor={rgb 0 0 0} leading=100%";
                                text = CollItem.Abstract + " " + CollItem.MonthYear;
                                if (text.Length > 0)
                                {
                                    _tf = _pdflib.add_textflow(_tf, text + linebreak, tf_optlist);
                                    if (_tf == -1)
                                        throw new Exception("Error: " + _pdflib.get_errmsg());
                                }

                                //Order Limit
                                tf_optlist = "charref fontname=Helvetica encoding=unicode fontsize=9 fillcolor={#00006A}";
                                text = CollItem.Limit;
                                if (text.Length > 0)
                                {
                                    _tf = _pdflib.add_textflow(_tf, "Order Limit: " + text, tf_optlist);
                                    if (_tf == -1)
                                        throw new Exception("Error: " + _pdflib.get_errmsg());
                                }

                                optlistBodyCol1 = " textflow=" + _tf;

                                _id_td = _pdflib.begin_item("TD", "");
                                _tbl = _pdflib.add_table_cell(_tbl, curr_col, curr_row, "", optlistBodyCol1 + " colwidth=90% margintop=10");
                                if (_tbl == -1)
                                    throw new Exception("Error: " + _pdflib.get_errmsg());
                                _pdflib.end_item(_id_td);

                                #endregion

                                #region Column 2
                                curr_col = 2; //column number
                                _tf = -1; //initialize text flow object handle

                                //Add a line break
                                //tf_optlist = "charref fontname=Helvetica encoding=winansi fontsize=8 ";
                                //_tf = _pdflib.add_textflow(_tf, linebreak, tf_optlist);
                                //if (_tf == -1)
                                //    throw new Exception("Error: " + _pdflib.get_errmsg());

                                tf_optlist = "fittextline={position=right fontname=Helvetica-Bold encoding=unicode fontsize=10 fillcolor={gray 0}}";
                                optlistBodyCol2 = tf_optlist;
                                text = CollItem.ProductId;
                                if (text.Length == 0)
                                    text = "NONE"; // "____";

                                _id_td = _pdflib.begin_item("TD", "");
                                _tbl = _pdflib.add_table_cell(_tbl, curr_col, curr_row, text, optlistBodyCol2 + " colwidth=10% margintop=10");
                                if (_tbl == -1)
                                    throw new Exception("Error: " + _pdflib.get_errmsg());
                                _pdflib.end_item(_id_td);
                                #endregion

                                _pdflib.end_item(_id_row); //End body row

                            }

                            /*begin table structure elements*/
                            _pdflib.end_item(_id_body); //End Body
                            _pdflib.end_item(_id_table); //End Table
                            /*End of table structure elements*/

                            #region Place Table
                            ///*Begin Place the Table*/
                            /* ---------- Place the table on one or more pages ---------- */

                            /*
                             * Loop until all of the table is placed; create new pages
                             * as long as more table instances need to be placed.
                             */
                            do
                            {
                                /*Begin available height check*/
                                boolNewPage = false; //new page flag
                                if (ury <= MinHeight) //Min height is a pre-defined value
                                {
                                    boolNewPage = true; //set to true if min height condition is satisfied
                                    #region new_page
                                    this.DrawFooter(ref _pdflib, ref pagecounter);

                                    _pdflib.end_page_ext(""); //end current page

                                    pagecounter++;
                                    _pdflib.begin_page_ext(0, 0, pagesize); //Begin a new page
                                    llx = 60; lly = 50; urx = 570; ury = 745; //Reset Position Variables
                                    #endregion
                                    continue; //proceed to next iteration
                                }
                                /*End avaliable height check*/

                                optlist =
                                        "stroke={line=hor1}";

                                /* Place the table instance */
                                result = _pdflib.fit_table(_tbl, llx, lly, urx, ury, optlist);

                                if (result == "_boxfull")
                                {
                                    #region new_page
                                    this.DrawFooter(ref _pdflib, ref pagecounter);

                                    _pdflib.end_page_ext(""); //end current page

                                    pagecounter++;
                                    _pdflib.begin_page_ext(0, 0, pagesize); //Begin a new page
                                    llx = 60; lly = 50; urx = 570; ury = 745; //Reset Position Variables
                                    #endregion
                                }

                            } while (result == "_boxfull" || boolNewPage);  //while (result == "_boxfull") ;

                            /* Check the result; "_stop" means all is ok. */
                            if (result != "_stop")
                            {
                                if (result == "_error")
                                {
                                    throw new Exception("Error when placing table: " +
                                            _pdflib.get_errmsg());
                                }
                                else
                                {
                                    /* Any other return value is a user exit caused by
                                     * the "return" option; this requires dedicated code to
                                     * deal with.
                                     */
                                    throw new Exception("User return found in Textflow");
                                }
                            }

                            /* This will also delete Textflow handles used in the table */
                            textendY = (int)_pdflib.info_table(_tbl, "height");
                            ury = ury - textendY - Yoffset; //fsize * 1.2; //Adjust to fit remaining text
                            _pdflib.delete_table(_tbl, "");

                            ///*End Place the Table*/

                        #endregion
                        }
                        collNonWYNTK = null;

                        #region WYNTK tables
                        ///CREATE THE ENGLISH WYNTK TABLE
                        if (collEnglishWYNTK.Count > 0)
                        {
                            this.CreateWYNTKPDFTable(ref _pdflib, ref collEnglishWYNTK, ref pagecounter, ref ury, MinHeight, "English");
                        }
                        ///CREATE THE SPANISH WYNTK TABLE
                        if (collSpanishWYNTK.Count > 0)
                        {
                            this.CreateWYNTKPDFTable(ref _pdflib, ref collSpanishWYNTK, ref pagecounter, ref ury, MinHeight, "Spanish");
                        }
                        collEnglishWYNTK = null;
                        collSpanishWYNTK = null;
                        #endregion
                    }
                    /*End Category Pubs without SubCategory or SubSubCategory*/
                    #endregion - Category Pubs without SubCategory or SubSubCategory

                    Coll = null;

                    #region Pubs With SubCategories
                    ArrayList SubCategories = GetSubCategoryArray(CategoryName);
                    foreach (string SubCategoryName in SubCategories)
                    {
                        #region TextFlow - SubCategoryName1

                        tf_text = SubCategoryName.ToUpper();
                        _tf = -1;
                        tf_tempoptlist = "charref fontname=Helvetica-Bold encoding=unicode fontsize=9 fillcolor={#00006A} ";
                        _tf = _pdflib.add_textflow(_tf, tf_text + linebreak, tf_tempoptlist); //add textflow
                        if (_tf == -1)
                            throw new Exception("Error: " + _pdflib.get_errmsg());

                        ury -= 5; //Adjust some Y space before the subcategory name

                        #region Blind Text Flow 2
                        do
                        {
                            /*Begin available height check*/
                            boolNewPage = false; //new page flag
                            if (ury <= MinHeight) //Min height is a pre-defined value
                            {
                                boolNewPage = true; //set to true if min height condition is satisfied
                                #region new_page
                                this.DrawFooter(ref _pdflib, ref pagecounter);

                                _pdflib.end_page_ext(""); //end current page

                                pagecounter++;
                                _pdflib.begin_page_ext(0, 0, pagesize); //Begin a new page
                                llx = 60; lly = 50; urx = 570; ury = 745; //Reset Position Variables
                                #endregion
                                continue; //proceed to next iteration
                            }
                            /*End avaliable height check*/

                            tf_tempoptlist = "charref fontname=Helvetica-Bold encoding=unicode fontsize=9 fillcolor={#00006A} ";

                            result = _pdflib.fit_textflow(_tf, llx, lly, urx, ury, "blind"); //blind fit textflow

                            /* "_boxfull" means we must continue because there is more text;
                             * "_nextpage" is interpreted as "start new column"
                             */
                            if (result == "_boxfull")
                            {
                                #region new_page
                                this.DrawFooter(ref _pdflib, ref pagecounter);

                                _pdflib.end_page_ext(""); //end current page

                                pagecounter++;
                                _pdflib.begin_page_ext(0, 0, pagesize); //Begin a new page
                                llx = 60; lly = 50; urx = 570; ury = 745; //Reset Position Variables
                                #endregion
                            }
                        } while (result == "_boxfull" || boolNewPage);

                        /* Check for errors */
                        if (result != "_stop")
                        {
                            /* "_boxempty" happens if the box is very small and doesn't
                             * hold any text at all.
                             */
                            if (result == "_boxempty")
                                throw new Exception("Error: Textflow box too small");
                            else
                            {
                                /* Any other return value is a user exit caused by
                                 * the "return" option; this requires dedicated code to
                                 * deal with.
                                 */
                                throw new Exception("User return '" + result +
                                        "' found in Textflow");
                            }
                        }
                        #endregion

                        #region Background Color Rectangle

                        dblTextYPos = _pdflib.info_textflow(_tf, "textendy"); //Get Y Pos of text flow (it's the upper y)
                        //dblTextHeight = _pdflib.info_textflow(_tf, "textheight"); //height of text
                        //dblTextWidth = _pdflib.info_textflow(_tf, "textwidth"); //width of text

                        /*Begin - Draw a rectangle with the retrieved width and height */
                        _pdflib.setcolor("fillstroke", "#A7A7A7", 0.0, 0.0, 0.0, 0.0);
                        _pdflib.rect(30, dblTextYPos + 5, 542, 15); //Can be fixed also
                        _pdflib.fill();
                        /*End - Draw rectangle*/
                        #endregion

                        #region Text Flow 2
                        /*Place Text Flow*/
                        /* Loop until all of the text is placed; create new pages
                        * as long as more text needs to be placed. Two columns will
                        * be created on all pages.
                        */
                        do
                        {
                            /*Begin available height check*/
                            boolNewPage = false; //new page flag
                            if (ury <= MinHeight) //Min height is a pre-defined value
                            {
                                boolNewPage = true; //set to true if min height condition is satisfied
                                #region new_page
                                this.DrawFooter(ref _pdflib, ref pagecounter);

                                _pdflib.end_page_ext(""); //end current page

                                pagecounter++;
                                _pdflib.begin_page_ext(0, 0, pagesize); //Begin a new page
                                llx = 60; lly = 50; urx = 570; ury = 745; //Reset Position Variables
                                #endregion
                                continue; //proceed to next iteration
                            }
                            /*End avaliable height check*/

                            tf_tempoptlist = "charref fontname=Helvetica-Bold encoding=unicode fontsize=9 fillcolor={#00006A} ";

                            result = _pdflib.fit_textflow(_tf, llx, lly, urx, ury, "rewind=-1"); //fit textflow

                            /* "_boxfull" means we must continue because there is more text;
                             * "_nextpage" is interpreted as "start new column"
                             */
                            if (result == "_boxfull")
                            {
                                #region new_page
                                this.DrawFooter(ref _pdflib, ref pagecounter);

                                _pdflib.end_page_ext(""); //end current page

                                pagecounter++;
                                _pdflib.begin_page_ext(0, 0, pagesize); //Begin a new page
                                llx = 60; lly = 50; urx = 570; ury = 745; //Reset Position Variables
                                #endregion
                            }
                        } while (result == "_boxfull" || boolNewPage);

                        /* Check for errors */
                        if (result != "_stop")
                        {
                            /* "_boxempty" happens if the box is very small and doesn't
                             * hold any text at all.
                             */
                            if (result == "_boxempty")
                                throw new Exception("Error: Textflow box too small");
                            else
                            {
                                /* Any other return value is a user exit caused by
                                 * the "return" option; this requires dedicated code to
                                 * deal with.
                                 */
                                throw new Exception("User return '" + result +
                                        "' found in Textflow");
                            }
                        }

                        #endregion

                        textendY = (int)_pdflib.info_textflow(_tf, "textendy");
                        _pdflib.delete_textflow(_tf);
                        //ury -= fsize * 1.2; //ury starts with max value (at top) and is reduced 
                        //as text is written from top to bottom
                        ury = textendY; //To get text closer this can be ury = textendY;
                        /*End Place Text Flow*/
                        #endregion

                        #region - Get Pubs without subsubcategories
                        CatalogRecordCollection Coll2 = getPubsPerCategory(CategoryName,
                                                                            SubCategoryName,
                                                                            "",
                                                                            false);
                        //BEGIN CODE SECTON FOR Coll2
                        if (Coll2.Count > 0)
                        {


                            /*begin table structure elements*/
                            int _id_table, _id_thead, _id_row, _id_th, _id_body, _id_td, curr_row, curr_col;
                            _id_table = _pdflib.begin_item("Table", "Title={Catalogs Table}"); //Begin Table
                            _id_thead = _pdflib.begin_item("THead", "Title={Header}"); //begin header
                            _id_row = _pdflib.begin_item("TR", "Title={Header Row}"); //header row

                            #region table_definition
                            /*Begin Header Cells*/
                            font = _pdflib.load_font("Helvetica-Bold", "unicode", ""); //Set a font type
                            if (font == -1)
                                throw new Exception("Error: " + _pdflib.get_errmsg());

                            alignment = "left";
                            optlistHeaderCol1 =
                                       "fittextline={position=" + alignment + " font=" + font + " fontsize=10 fillcolor={gray 0} } ";

                            curr_col = 1; curr_row = 1; _tbl = -1; //initialize

                            _id_th = _pdflib.begin_item("TH", "Scope=Column");
                            //_id_th = _pdflib.begin_item("TH", "");
                            _tbl = _pdflib.add_table_cell(_tbl, curr_col, curr_row, "Title/Description", optlistHeaderCol1 + " colwidth=90% margin=3");
                            if (_tbl == -1)
                                throw new Exception("Error: " + _pdflib.get_errmsg());
                            _pdflib.end_item(_id_th);

                            alignment = "right"; //assign a new value for alignment
                            optlistHeaderCol2 =
                                        "fittextline={position=" + alignment + " font=" + font + " fontsize=10} fillcolor={gray 0} fillcolor={gray 0} ";

                            curr_col++; curr_row = 1;

                            _id_th = _pdflib.begin_item("TH", "Scope=Column");
                            //_id_th = _pdflib.begin_item("TH", "");
                            _tbl = _pdflib.add_table_cell(_tbl, curr_col, curr_row, "Inventory Number", optlistHeaderCol1 + " colwidth=10% margin=3");
                            if (_tbl == -1)
                                throw new Exception("Error: " + _pdflib.get_errmsg());
                            _pdflib.end_item(_id_th);
                            /*End Header Cells*/

                            _pdflib.end_item(_id_row); //End header row
                            _pdflib.end_item(_id_thead); //End Header
                            _id_body = _pdflib.begin_item("TBody", "Title={Body}"); //Begin Body

                            /*Begin Data Cells*/
                            font = _pdflib.load_font("Helvetica", "unicode", ""); //Set a font type
                            if (font == -1)
                                throw new Exception("Error: " + _pdflib.get_errmsg());
                            optlistBodyCol1 =
                                    "fittextline={position=left font=" + font + " fontsize=9} ";
                            optlistBodyCol2 =
                                    "fittextline={position=left font=" + font + " fontsize=9} ";

                            string tf_optlist;
                            string text;

                            /*End of table structure elements*/
                            #endregion

                            collEnglishWYNTK = new CatalogRecordCollection();
                            collSpanishWYNTK = new CatalogRecordCollection();

                            foreach (CatalogRecord CollItem in Coll2)
                            {
                                //Check for WYNTK
                                boolHasWYNTK = false;
                                if (string.Compare(CollItem.CatalogWYNTK, "1") == 0)
                                {
                                    collEnglishWYNTK.Add(CollItem);
                                    boolHasWYNTK = true;

                                }
                                if (string.Compare(CollItem.SpanishWYNTK, "1") == 0)
                                {
                                    collSpanishWYNTK.Add(CollItem);
                                    boolHasWYNTK = true;
                                }
                                if (boolHasWYNTK)
                                    continue; //Add the row to table only if the record is not WYNTK
                                //End Check for WYNKT

                                _id_row = _pdflib.begin_item("TR", "Title={Body Row}");

                                curr_row++;

                                #region Column 1

                                curr_col = 1; //column number
                                _tf = -1; //initialize text flow object handle

                                tf_optlist = "charref fontname=Helvetica-Bold encoding=winansi fontsize=8 ";

                                ////Initial line break 
                                //_tf = _pdflib.add_textflow(_tf, linebreak, tf_optlist);
                                //if (_tf == -1)
                                //    throw new Exception("Error: " + _pdflib.get_errmsg());

                                ////Category, SubCategory, SubSubCategory
                                //if (CollItem.Category.Length > 0 && CollItem.SubCategory.Length > 0 && CollItem.SubSubCategory.Length > 0)
                                //    text = "Category: " + CollItem.Category + @"/" + CollItem.SubCategory + @"/" + CollItem.SubSubCategory;
                                //else if (CollItem.Category.Length > 0 && CollItem.SubCategory.Length > 0)
                                //    text = "Category: " + CollItem.Category + @"/" + CollItem.SubCategory;
                                //else
                                //    text = "Category: " + CollItem.Category;

                                ////_tf = _pdflib.add_textflow(_tf, "Data Row with one column. Row number is: " + curr_row.ToString() + linebreak, tf_optlist);
                                //_tf = _pdflib.add_textflow(_tf, text + linebreak, tf_optlist);
                                //if (_tf == -1)
                                //    throw new Exception("Error: " + _pdflib.get_errmsg());

                                //Title
                                tf_optlist = "charref fontname=Helvetica-Bold encoding=unicode fontsize=10 ";
                                text = CollItem.LongTitle;
                                if (text.Length > 0)
                                {
                                    _tf = _pdflib.add_textflow(_tf, text + linebreak, tf_optlist);
                                    if (_tf == -1)
                                        throw new Exception("Error: " + _pdflib.get_errmsg());
                                }
                                #region URL
                                string url = CollItem.URL;
                                if (url.Length > 0)
                                {
                                    //int normalfont = _pdflib.load_font("Helvetica", "unicode", "");
                                    int normalfont = _pdflib.load_font("Helvetica", "unicode", "");
                                    string optlistLink =
                                        "font=" + normalfont + " fontsize=9 " +
                                        "matchbox={name=lnkURL} fillcolor={#00006A} underline=false";

                                    _tf = _pdflib.add_textflow(_tf, url, optlistLink);
                                    if (_tf == -1)
                                        throw new Exception("Error: " + _pdflib.get_errmsg());

                                    /* create URI action */
                                    optlistLink = "url={" + url + "}";
                                    int act = _pdflib.create_action("URI", optlistLink);

                                    /* create Link annotation on matchbox "kraxi" */
                                    optlistLink = "action={activate " + act + "} linewidth=0 usematchbox={lnkURL}";
                                }
                                #endregion

                                tf_optlist = "charref fontname=Helvetica encoding=unicode fontsize=9 fillcolor={rgb 0 0 0} underline=false";

                                //Add a space and a line break after URL
                                if (url.Length > 0)
                                {
                                    _tf = _pdflib.add_textflow(_tf, " " + linebreak, tf_optlist);
                                    if (_tf == -1)
                                        throw new Exception("Error: " + _pdflib.get_errmsg());
                                }

                                //Abstract and Month Year
                                tf_optlist = "charref fontname=Helvetica encoding=unicode fontsize=9 fillcolor={rgb 0 0 0} leading=30%";
                                _tf = _pdflib.add_textflow(_tf, "\n", tf_optlist);
                                if (_tf == -1)
                                    throw new Exception("Error: " + _pdflib.get_errmsg());

                                tf_optlist = "charref fontname=Helvetica encoding=unicode fontsize=9 fillcolor={rgb 0 0 0} leading=100%";
                                text = CollItem.Abstract + " " + CollItem.MonthYear;
                                if (text.Length > 0)
                                {
                                    _tf = _pdflib.add_textflow(_tf, text + linebreak, tf_optlist);
                                    if (_tf == -1)
                                        throw new Exception("Error: " + _pdflib.get_errmsg());
                                }

                                //Order Limit
                                text = CollItem.Limit;
                                if (text.Length > 0)
                                {
                                    _tf = _pdflib.add_textflow(_tf, "Order Limit: " + text, tf_optlist);
                                    if (_tf == -1)
                                        throw new Exception("Error: " + _pdflib.get_errmsg());
                                }

                                optlistBodyCol1 = " textflow=" + _tf;

                                _id_td = _pdflib.begin_item("TD", "");
                                _tbl = _pdflib.add_table_cell(_tbl, curr_col, curr_row, "", optlistBodyCol1 + " colwidth=90% margintop=10");
                                if (_tbl == -1)
                                    throw new Exception("Error: " + _pdflib.get_errmsg());
                                _pdflib.end_item(_id_td);

                                #endregion

                                #region Column 2
                                curr_col = 2; //column number
                                _tf = -1; //initialize text flow object handle

                                //Add a line break
                                //tf_optlist = "charref fontname=Helvetica encoding=winansi fontsize=8 ";
                                //_tf = _pdflib.add_textflow(_tf, linebreak, tf_optlist);
                                //if (_tf == -1)
                                //    throw new Exception("Error: " + _pdflib.get_errmsg());

                                tf_optlist = "fittextline={position=right fontname=Helvetica-Bold encoding=unicode fontsize=10 fillcolor={gray 0} }";
                                optlistBodyCol2 = tf_optlist;
                                text = CollItem.ProductId;
                                if (text.Length == 0)
                                    text = "NONE"; // "____";

                                _id_td = _pdflib.begin_item("TD", "");
                                _tbl = _pdflib.add_table_cell(_tbl, curr_col, curr_row, text, optlistBodyCol2 + " colwidth=10% margintop=10");
                                if (_tbl == -1)
                                    throw new Exception("Error: " + _pdflib.get_errmsg());
                                _pdflib.end_item(_id_td);
                                #endregion

                                _pdflib.end_item(_id_row); //End body row
                            }

                            /*begin table structure elements*/
                            _pdflib.end_item(_id_body); //End Body
                            _pdflib.end_item(_id_table); //End Table
                            /*End of table structure elements*/

                            #region Place Table
                            ///*Begin Place the Table*/
                            /* ---------- Place the table on one or more pages ---------- */

                            /*
                             * Loop until all of the table is placed; create new pages
                             * as long as more table instances need to be placed.
                             */
                            do
                            {
                                /*Begin available height check*/
                                boolNewPage = false; //new page flag
                                if (ury <= MinHeight) //Min height is a pre-defined value
                                {
                                    boolNewPage = true; //set to true if min height condition is satisfied
                                    #region new_page
                                    this.DrawFooter(ref _pdflib, ref pagecounter);

                                    _pdflib.end_page_ext(""); //end current page

                                    pagecounter++;
                                    _pdflib.begin_page_ext(0, 0, pagesize); //Begin a new page
                                    llx = 60; lly = 50; urx = 570; ury = 745; //Reset Position Variables
                                    #endregion
                                    continue; //proceed to next iteration
                                }
                                /*End avaliable height check*/

                                optlist =
                                        "stroke={line=hor1}";

                                /* Place the table instance */
                                result = _pdflib.fit_table(_tbl, llx, lly, urx, ury, optlist);

                                if (result == "_boxfull")
                                {
                                    #region new_page
                                    this.DrawFooter(ref _pdflib, ref pagecounter);

                                    _pdflib.end_page_ext(""); //end current page

                                    pagecounter++;
                                    _pdflib.begin_page_ext(0, 0, pagesize); //Begin a new page
                                    llx = 60; lly = 50; urx = 570; ury = 745; //Reset Position Variables
                                    #endregion
                                }

                            } while (result == "_boxfull" || boolNewPage);  //while (result == "_boxfull") ;

                            /* Check the result; "_stop" means all is ok. */
                            if (result != "_stop")
                            {
                                if (result == "_error")
                                {
                                    throw new Exception("Error when placing table: " +
                                            _pdflib.get_errmsg());
                                }
                                else
                                {
                                    /* Any other return value is a user exit caused by
                                     * the "return" option; this requires dedicated code to
                                     * deal with.
                                     */
                                    throw new Exception("User return found in Textflow");
                                }
                            }

                            /* This will also delete Textflow handles used in the table */
                            textendY = (int)_pdflib.info_table(_tbl, "height");
                            ury = ury - textendY - Yoffset; //fsize * 1.2; //Adjust to fit remaining text
                            _pdflib.delete_table(_tbl, "");

                            ///*End Place the Table*/

                            #endregion

                            #region WYNTK tables
                            ///CREATE THE ENGLISH WYNTK TABLE
                            if (collEnglishWYNTK.Count > 0)
                            {
                                this.CreateWYNTKPDFTable(ref _pdflib, ref collEnglishWYNTK, ref pagecounter, ref ury, MinHeight, "English");
                            }
                            ///CREATE THE SPANISH WYNTK TABLE
                            if (collSpanishWYNTK.Count > 0)
                            {
                                this.CreateWYNTKPDFTable(ref _pdflib, ref collSpanishWYNTK, ref pagecounter, ref ury, MinHeight, "Spanish");
                            }
                            collEnglishWYNTK = null;
                            collSpanishWYNTK = null;
                            #endregion

                        }
                        //END CODE SECTION FOR Coll2
                        Coll2 = null;
                        #endregion

                        #region - Get Pubs with subsubcategories
                        ArrayList SubSubCategories = GetSubSubCategoryArray(CategoryName, SubCategoryName);
                        foreach (string SubSubCategoryName in SubSubCategories)
                        {

                            CatalogRecordCollection Coll3 = getPubsPerCategory(CategoryName,
                                                                            SubCategoryName,
                                                                            SubSubCategoryName,
                                                                            true);
                            //BEGIN CODE SECTON FOR Coll3
                            if (Coll3.Count > 0)
                            {
                                #region TextFlow - SubSubCategoryName

                                tf_text = SubSubCategoryName.ToUpper();

                                //01-21-11 Added code for page break adjustment
                                foreach (string str in GlobalUtilities.UtilityMethods.strArrAdjustPage())
                                {
                                    if (string.Compare(tf_text, str, true) == 0)
                                    {
                                        ury = MinHeight;
                                        break;
                                    }
                                }
                                //End code

                                _tf = -1;
                                tf_tempoptlist = "charref fontname=Helvetica-Bold encoding=unicode fontsize=9 underline fillcolor={#00006A} strokecolor={#00006A}";
                                _tf = _pdflib.add_textflow(_tf, tf_text + linebreak, tf_tempoptlist); //add textflow
                                if (_tf == -1)
                                    throw new Exception("Error: " + _pdflib.get_errmsg());

                                /*Place Text Flow*/
                                /* Loop until all of the text is placed; create new pages
                                * as long as more text needs to be placed. Two columns will
                                * be created on all pages.
                                */
                                do
                                {
                                    /*Begin available height check*/
                                    boolNewPage = false; //new page flag
                                    if (ury <= MinHeight) //Min height is a pre-defined value
                                    {
                                        boolNewPage = true; //set to true if min height condition is satisfied
                                        #region new_page
                                        this.DrawFooter(ref _pdflib, ref pagecounter);

                                        _pdflib.end_page_ext(""); //end current page

                                        pagecounter++;
                                        _pdflib.begin_page_ext(0, 0, pagesize); //Begin a new page
                                        llx = 60; lly = 50; urx = 570; ury = 745; //Reset Position Variables
                                        #endregion
                                        continue; //proceed to next iteration
                                    }
                                    /*End avaliable height check*/

                                    tf_tempoptlist = "charref fontname=Helvetica encoding=unicode fontsize=9 fillcolor={#00006A} ";

                                    result = _pdflib.fit_textflow(_tf, llx, lly, urx, ury, ""); //fit textflow

                                    /* "_boxfull" means we must continue because there is more text;
                                     * "_nextpage" is interpreted as "start new column"
                                     */
                                    if (result == "_boxfull")
                                    {
                                        #region new_page
                                        this.DrawFooter(ref _pdflib, ref pagecounter);

                                        _pdflib.end_page_ext(""); //end current page

                                        pagecounter++;
                                        _pdflib.begin_page_ext(0, 0, pagesize); //Begin a new page
                                        llx = 60; lly = 50; urx = 570; ury = 745; //Reset Position Variables
                                        #endregion
                                    }
                                } while (result == "_boxfull" || boolNewPage);

                                /* Check for errors */
                                if (result != "_stop")
                                {
                                    /* "_boxempty" happens if the box is very small and doesn't
                                     * hold any text at all.
                                     */
                                    if (result == "_boxempty")
                                        throw new Exception("Error: Textflow box too small");
                                    else
                                    {
                                        /* Any other return value is a user exit caused by
                                         * the "return" option; this requires dedicated code to
                                         * deal with.
                                         */
                                        throw new Exception("User return '" + result +
                                                "' found in Textflow");
                                    }
                                }

                                textendY = (int)_pdflib.info_textflow(_tf, "textendy");
                                _pdflib.delete_textflow(_tf);
                                //ury -= fsize * 1.2; //ury starts with max value (at top) and is reduced 
                                //as text is written from top to bottom
                                ury = textendY; //To get text closer this can be ury = textendY;
                                /*End Place Text Flow*/
                                #endregion


                                ///Do checks here - New Code
                                collEnglishWYNTK = new CatalogRecordCollection();
                                collSpanishWYNTK = new CatalogRecordCollection();
                                bool boolSubSubPubs = false;
                                foreach (CatalogRecord CollItem in Coll3)
                                {
                                    //Check for WYNTK
                                    boolHasWYNTK = false;
                                    if (string.Compare(CollItem.CatalogWYNTK, "1") == 0)
                                    {
                                        collEnglishWYNTK.Add(CollItem);
                                        boolHasWYNTK = true;

                                    }
                                    if (string.Compare(CollItem.SpanishWYNTK, "1") == 0)
                                    {
                                        collSpanishWYNTK.Add(CollItem);
                                        boolHasWYNTK = true;
                                    }
                                    if (boolHasWYNTK)
                                        continue; //Add the row to table only if the record is not WYNTK
                                    //End Check for WYNKT

                                    if (!boolSubSubPubs)
                                        boolSubSubPubs = true;
                                }
                                ///End of New Code


                                if (boolSubSubPubs) //Only if there is a sub sub pub that is not WYNTK
                                {

                                    /*begin table structure elements*/
                                    int _id_table, _id_thead, _id_row, _id_th, _id_body, _id_td, curr_row, curr_col;
                                    _id_table = _pdflib.begin_item("Table", "Title={Catalogs Table}"); //Begin Table
                                    _id_thead = _pdflib.begin_item("THead", "Title={Header}"); //begin header
                                    _id_row = _pdflib.begin_item("TR", "Title={Header Row}"); //header row

                                    #region table_definition
                                    /*Begin Header Cells*/
                                    font = _pdflib.load_font("Helvetica-Bold", "unicode", ""); //Set a font type
                                    if (font == -1)
                                        throw new Exception("Error: " + _pdflib.get_errmsg());

                                    alignment = "left";
                                    optlistHeaderCol1 =
                                               "fittextline={position=" + alignment + " font=" + font + " fontsize=9 fillcolor={gray 0}} ";

                                    curr_col = 1; curr_row = 1; _tbl = -1; //initialize

                                    _id_th = _pdflib.begin_item("TH", "Scope=Column");
                                    //_id_th = _pdflib.begin_item("TH", "");
                                    _tbl = _pdflib.add_table_cell(_tbl, curr_col, curr_row, "Title/Description", optlistHeaderCol1 + " colwidth=90% margin=3");
                                    if (_tbl == -1)
                                        throw new Exception("Error: " + _pdflib.get_errmsg());
                                    _pdflib.end_item(_id_th);

                                    alignment = "right"; //assign a new value for alignment
                                    optlistHeaderCol2 =
                                                "fittextline={position=" + alignment + " font=" + font + " fontsize=10} fillcolor={gray 0}} ";

                                    curr_col++; curr_row = 1;

                                    _id_th = _pdflib.begin_item("TH", "Scope=Column");
                                    //_id_th = _pdflib.begin_item("TH", "");
                                    _tbl = _pdflib.add_table_cell(_tbl, curr_col, curr_row, "Inventory Number", optlistHeaderCol1 + " colwidth=10% margin=3");
                                    if (_tbl == -1)
                                        throw new Exception("Error: " + _pdflib.get_errmsg());
                                    _pdflib.end_item(_id_th);
                                    /*End Header Cells*/

                                    _pdflib.end_item(_id_row); //End header row
                                    _pdflib.end_item(_id_thead); //End Header
                                    _id_body = _pdflib.begin_item("TBody", "Title={Body}"); //Begin Body

                                    /*Begin Data Cells*/
                                    font = _pdflib.load_font("Helvetica", "winansi", ""); //Set a font type
                                    if (font == -1)
                                        throw new Exception("Error: " + _pdflib.get_errmsg());
                                    optlistBodyCol1 =
                                            "fittextline={position=left font=" + font + " fontsize=10} ";
                                    optlistBodyCol2 =
                                            "fittextline={position=left font=" + font + " fontsize=10} ";

                                    string tf_optlist;
                                    string text;

                                    /*End of table structure elements*/
                                    #endregion


                                    foreach (CatalogRecord CollItem in Coll3)
                                    {
                                        //Check for WYNTK
                                        if (string.Compare(CollItem.CatalogWYNTK, "1") == 0 || string.Compare(CollItem.SpanishWYNTK, "1") == 0)
                                            continue; //Discard this pass
                                        //End Check for WYNKT

                                        _id_row = _pdflib.begin_item("TR", "Title={Body Row}"); //body row

                                        curr_row++;

                                        #region Column 1

                                        curr_col = 1; //column number
                                        _tf = -1; //initialize text flow object handle

                                        tf_optlist = "charref fontname=Helvetica-Bold encoding=winansi fontsize=8 ";

                                        ////Initial line break 
                                        //_tf = _pdflib.add_textflow(_tf, linebreak, tf_optlist);
                                        //if (_tf == -1)
                                        //    throw new Exception("Error: " + _pdflib.get_errmsg());

                                        //Category, SubCategory, SubSubCategory
                                        //if (CollItem.Category.Length > 0 && CollItem.SubCategory.Length > 0 && CollItem.SubSubCategory.Length > 0)
                                        //    text = "Category: " + CollItem.Category + @"/" + CollItem.SubCategory + @"/" + CollItem.SubSubCategory;
                                        //else if (CollItem.Category.Length > 0 && CollItem.SubCategory.Length > 0)
                                        //    text = "Category: " + CollItem.Category + @"/" + CollItem.SubCategory;
                                        //else
                                        //    text = "Category: " + CollItem.Category;

                                        ////_tf = _pdflib.add_textflow(_tf, "Data Row with one column. Row number is: " + curr_row.ToString() + linebreak, tf_optlist);
                                        //_tf = _pdflib.add_textflow(_tf, text + linebreak, tf_optlist);
                                        //if (_tf == -1)
                                        //    throw new Exception("Error: " + _pdflib.get_errmsg());

                                        //Title
                                        tf_optlist = "charref fontname=Helvetica-Bold encoding=unicode fontsize=10 ";
                                        text = CollItem.LongTitle;
                                        if (text.Length > 0)
                                        {
                                            _tf = _pdflib.add_textflow(_tf, text + linebreak, tf_optlist);
                                            if (_tf == -1)
                                                throw new Exception("Error: " + _pdflib.get_errmsg());
                                        }
                                        #region URL
                                        string url = CollItem.URL;
                                        if (url.Length > 0)
                                        {
                                            int normalfont = _pdflib.load_font("Helvetica", "unicode", "");
                                            string optlistLink =
                                                "font=" + normalfont + " fontsize=9 " +
                                                "matchbox={name=lnkURL} fillcolor={#00006A} underline=false";

                                            _tf = _pdflib.add_textflow(_tf, url, optlistLink);
                                            if (_tf == -1)
                                                throw new Exception("Error: " + _pdflib.get_errmsg());

                                            /* create URI action */
                                            optlistLink = "url={" + url + "}";
                                            int act = _pdflib.create_action("URI", optlistLink);

                                            /* create Link annotation on matchbox "kraxi" */
                                            optlistLink = "action={activate " + act + "} linewidth=0 usematchbox={lnkURL}";
                                        }
                                        #endregion

                                        //Add a space and a line break after URL
                                        if (url.Length > 0)
                                        {
                                            _tf = _pdflib.add_textflow(_tf, " " + linebreak, tf_optlist);
                                            if (_tf == -1)
                                                throw new Exception("Error: " + _pdflib.get_errmsg());
                                        }

                                        //Abstract and Month Year
                                        tf_optlist = "charref fontname=Helvetica encoding=unicode fontsize=9 fillcolor={rgb 0 0 0} leading=30%";
                                        _tf = _pdflib.add_textflow(_tf, "\n", tf_optlist);
                                        if (_tf == -1)
                                            throw new Exception("Error: " + _pdflib.get_errmsg());

                                        tf_optlist = "charref fontname=Helvetica encoding=unicode fontsize=9 fillcolor={rgb 0 0 0} leading=100%";
                                        text = CollItem.Abstract + " " + CollItem.MonthYear;
                                        if (text.Length > 0)
                                        {
                                            _tf = _pdflib.add_textflow(_tf, text + linebreak, tf_optlist);
                                            if (_tf == -1)
                                                throw new Exception("Error: " + _pdflib.get_errmsg());
                                        }

                                        //Order Limit
                                        tf_optlist = "charref fontname=Helvetica encoding=unicode fontsize=9 fillcolor={#00006A}";
                                        text = CollItem.Limit;
                                        if (text.Length > 0)
                                        {
                                            _tf = _pdflib.add_textflow(_tf, "Order Limit: " + text, tf_optlist);
                                            if (_tf == -1)
                                                throw new Exception("Error: " + _pdflib.get_errmsg());
                                        }

                                        optlistBodyCol1 = " textflow=" + _tf;

                                        _id_td = _pdflib.begin_item("TD", "");
                                        _tbl = _pdflib.add_table_cell(_tbl, curr_col, curr_row, "", optlistBodyCol1 + " colwidth=90% margintop=10");
                                        if (_tbl == -1)
                                            throw new Exception("Error: " + _pdflib.get_errmsg());
                                        _pdflib.end_item(_id_td);

                                        #endregion

                                        #region Column 2
                                        curr_col = 2; //column number
                                        _tf = -1; //initialize text flow object handle

                                        //Add a line break
                                        //tf_optlist = "charref fontname=Helvetica encoding=winansi fontsize=8 ";
                                        //_tf = _pdflib.add_textflow(_tf, linebreak, tf_optlist);
                                        //if (_tf == -1)
                                        //    throw new Exception("Error: " + _pdflib.get_errmsg());

                                        tf_optlist = "fittextline={position=right fontname=Helvetica-Bold encoding=unicode fontsize=10 fillcolor={gray 0}}";
                                        optlistBodyCol2 = tf_optlist;
                                        text = CollItem.ProductId;
                                        if (text.Length == 0)
                                            text = "NONE"; // "____";

                                        _id_td = _pdflib.begin_item("TD", "");
                                        _tbl = _pdflib.add_table_cell(_tbl, curr_col, curr_row, text, optlistBodyCol2 + " colwidth=10% margintop=10");
                                        if (_tbl == -1)
                                            throw new Exception("Error: " + _pdflib.get_errmsg());
                                        _pdflib.end_item(_id_td);
                                        #endregion

                                        _pdflib.end_item(_id_row); //End body row

                                    }

                                    /*begin table structure elements*/
                                    _pdflib.end_item(_id_body); //End Body
                                    _pdflib.end_item(_id_table); //End Table
                                    /*End of table structure elements*/


                                    #region Place Table

                                    ///*Begin Place the Table*/
                                    /* ---------- Place the table on one or more pages ---------- */

                                    /*
                                     * Loop until all of the table is placed; create new pages
                                     * as long as more table instances need to be placed.
                                     */
                                    do
                                    {
                                        /*Begin available height check*/
                                        boolNewPage = false; //new page flag
                                        if (ury <= MinHeight) //Min height is a pre-defined value
                                        {
                                            boolNewPage = true; //set to true if min height condition is satisfied
                                            #region new_page
                                            this.DrawFooter(ref _pdflib, ref pagecounter);

                                            _pdflib.end_page_ext(""); //end current page

                                            pagecounter++;
                                            _pdflib.begin_page_ext(0, 0, pagesize); //Begin a new page
                                            llx = 60; lly = 50; urx = 570; ury = 745; //Reset Position Variables
                                            #endregion
                                            continue; //proceed to next iteration
                                        }
                                        /*End avaliable height check*/

                                        optlist =
                                                "stroke={line=hor1}";

                                        /* Place the table instance */
                                        result = _pdflib.fit_table(_tbl, llx, lly, urx, ury, optlist);

                                        if (result == "_boxfull")
                                        {
                                            #region new_page
                                            this.DrawFooter(ref _pdflib, ref pagecounter);

                                            _pdflib.end_page_ext(""); //end current page

                                            pagecounter++;
                                            _pdflib.begin_page_ext(0, 0, pagesize); //Begin a new page
                                            llx = 60; lly = 50; urx = 570; ury = 745; //Reset Position Variables
                                            #endregion
                                        }

                                    } while (result == "_boxfull" || boolNewPage);  //while (result == "_boxfull") ;

                                    /* Check the result; "_stop" means all is ok. */
                                    if (result != "_stop")
                                    {
                                        if (result == "_error")
                                        {
                                            throw new Exception("Error when placing table: " +
                                                    _pdflib.get_errmsg());
                                        }
                                        else
                                        {
                                            /* Any other return value is a user exit caused by
                                             * the "return" option; this requires dedicated code to
                                             * deal with.
                                             */
                                            throw new Exception("User return found in Textflow");
                                        }
                                    }

                                    /* This will also delete Textflow handles used in the table */
                                    textendY = (int)_pdflib.info_table(_tbl, "height");
                                    ury = ury - textendY - Yoffset; //fsize * 1.2; //Adjust to fit remaining text
                                    _pdflib.delete_table(_tbl, "");

                                    ///*End Place the Table*/
                                    #endregion

                                } //End boolSubSubPub condition

                                #region WYNTK tables
                                ///CREATE THE ENGLISH WYNTK TABLE
                                if (collEnglishWYNTK.Count > 0)
                                {
                                    this.CreateWYNTKPDFTable(ref _pdflib, ref collEnglishWYNTK, ref pagecounter, ref ury, MinHeight, "English");
                                }
                                ///CREATE THE SPANISH WYNTK TABLE
                                if (collSpanishWYNTK.Count > 0)
                                {
                                    this.CreateWYNTKPDFTable(ref _pdflib, ref collSpanishWYNTK, ref pagecounter, ref ury, MinHeight, "Spanish");
                                }
                                collEnglishWYNTK = null;
                                collSpanishWYNTK = null;
                                #endregion

                            }
                            //END CODE SECTION FOR Coll3
                            Coll3 = null;
                        }
                        #endregion
                    }
                    #endregion

                }//End of Main Outer Cateogry Loop


                //Reached the last page
                #region last_page
                this.DrawFooter(ref _pdflib, ref pagecounter);
                _pdflib.end_page_ext(""); //end last page
                #endregion

                this.AddStaticPages(ref _pdflib, ref pagecounter, mode);

                /*************************************/

                //End Section Element
                _pdflib.end_item(_id_sect1);

                /* Close the structure element of type "Article" */
                _pdflib.end_item(_id_art);
                _pdflib.end_document("");
                buffer = _pdflib.get_buffer();

                //_pdflib.Dispose(); // will move to finally later on

                //return buffer
                return buffer;

            }
            catch (Exception Ex) // 'try' can be found on line 59
            {
                ////Write to log
                //Microsoft.Practices.EnterpriseLibrary.Logging.LogEntry logEnt = new Microsoft.Practices.EnterpriseLibrary.Logging.LogEntry();
                //logEnt.Message = "\r\n" + "Error Occurred in PDFClass." + "\r\n" + "Source: " + Ex.Source + "\r\n" + "Description: " + Ex.Message + "\r\n" + "Stack Trace: " + Ex.StackTrace;
                //Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(logEnt, "Logs");
                throw;
            }
            finally 
            {
                if (_pdflib != null)
                    _pdflib.Dispose();
            }

        }

        #region UtilityMethods
        
        //Get Available Category Names
        private ArrayList GetCategoryArray()
        {
            ArrayList arrCategory = new ArrayList();
            string currCategory;
            CatalogRecordCollection Coll = (CatalogRecordCollection)HttpContext.Current.Session["NCI_CatalogColl"];
            foreach (CatalogRecord CollItem in Coll)
            {
                currCategory = CollItem.Category;
                if (!arrCategory.Contains(currCategory))
                    arrCategory.Add(currCategory);
            }
            return arrCategory;
        }

        //Get Sub Category Names for a given Category
        private ArrayList GetSubCategoryArray(string Category)
        {
            string currCategory = Category;
            ArrayList arrLst = new ArrayList();
            CatalogRecordCollection Coll = (CatalogRecordCollection)HttpContext.Current.Session["NCI_CatalogColl"];
            foreach (CatalogRecord CollItem in Coll)
            {
                if (string.Compare(CollItem.Category, currCategory, true) == 0)
                {
                    if (CollItem.SubCategory.Length > 0)
                    {
                        if (!arrLst.Contains(CollItem.SubCategory))
                            arrLst.Add(CollItem.SubCategory);
                    }
                }
            }
            return arrLst;
        }

        //Get SubSubCategory Names for a give Category and SubCategory
        private ArrayList GetSubSubCategoryArray(string Category, string SubCategory)
        {
            string currCategory = Category;
            string currSubCategory = SubCategory;
            ArrayList arrLst = new ArrayList();
            CatalogRecordCollection Coll = (CatalogRecordCollection)HttpContext.Current.Session["NCI_CatalogColl"];
            foreach (CatalogRecord CollItem in Coll)
            {
                if (string.Compare(CollItem.Category, currCategory) == 0
                    && string.Compare(CollItem.SubCategory,currSubCategory, true) == 0)
                {
                    if (CollItem.SubSubCategory.Length > 0)
                    {
                        if (!arrLst.Contains(CollItem.SubSubCategory))
                            arrLst.Add(CollItem.SubSubCategory);
                    }
                }
            }
            return arrLst;
        }


        //return Category Records Collection without subcategories or subsubcategories
        private CatalogRecordCollection getPubsPerCategory(string Category)
        {
            string currCategory = Category;
            CatalogRecordCollection catRecords = new CatalogRecordCollection();
            CatalogRecordCollection Coll = (CatalogRecordCollection)HttpContext.Current.Session["NCI_CatalogColl"];
            foreach (CatalogRecord CollItem in Coll)
            {
                if (string.Compare(CollItem.Category, currCategory, true) == 0
                        && string.Compare(CollItem.SubCategory, "") == 0
                            && string.Compare(CollItem.SubSubCategory, "") == 0)
                    catRecords.Add(CollItem);
            }
            return catRecords;

        }

        ////return Category Records Collection without subsubcategories
        //public CatalogRecordCollection getPubsPerCategory(string Category, string SubCategory)
        //{
        //    string currCategory = Category;
        //    CatalogRecordCollection catRecords = new CatalogRecordCollection();
        //    CatalogRecordCollection Coll = (CatalogRecordCollection)HttpContext.Current.Session["NCI_CatalogColl"];
        //    foreach (CatalogRecord CollItem in Coll)
        //    {
        //        if (string.Compare(CollItem.Category, currCategory, true) == 0
        //                && CollItem.SubCategory.Length > 0
        //                    && string.Compare(CollItem.SubSubCategory, "") == 0)
        //            catRecords.Add(CollItem);
        //    }
        //    return catRecords;
        //}

        //return Category Records Collection with subcategories and with or without subsubcategories, depending on flag
        public CatalogRecordCollection getPubsPerCategory(string Category,string SubCategory, string SubSubCategory, bool IsSubSubCategoryPresent)
        {
            string currCategory = Category;
            CatalogRecordCollection catRecords = new CatalogRecordCollection();
            CatalogRecordCollection Coll = (CatalogRecordCollection)HttpContext.Current.Session["NCI_CatalogColl"];
            if (IsSubSubCategoryPresent == true)
            {

                foreach (CatalogRecord CollItem in Coll)
                {
                    if (string.Compare(CollItem.Category, currCategory, true) == 0
                            && string.Compare(CollItem.SubCategory, SubCategory, true) == 0
                                && string.Compare(CollItem.SubSubCategory, SubSubCategory, true) == 0)
                        catRecords.Add(CollItem);
                }
            }
            else
            {
                foreach (CatalogRecord CollItem in Coll)
                {
                    if (string.Compare(CollItem.Category, currCategory, true) == 0
                            && string.Compare(CollItem.SubCategory, SubCategory, true) == 0
                                && string.Compare(CollItem.SubSubCategory, "") == 0)
                        catRecords.Add(CollItem);
                }
            }
            return catRecords;
        }
        #endregion

        ////Another utility method
        //private string GetCancerTypeText(string CancerTypeId)
        //{
        //    string CancerTypeText = "";

        //    if (CancerTypeId == "") //Exit Condition
        //        return "";

        //    KVPairCollection kvPairColl = (KVPairCollection)HttpContext.Current.Session["NCI_CancerTypeColl"];
        //    foreach (KVPair kvPairItem in kvPairColl)
        //    {
        //        if (string.Compare(kvPairItem.Key, CancerTypeId, true) == 0)
        //        {
        //            CancerTypeText = kvPairItem.Val;
        //            break;
        //        }
        //    }

        //    return CancerTypeText;
        //}

        private void CreateWYNTKPDFTable(ref PDFlib _pdflib, ref CatalogRecordCollection Coll, ref int pagecounter, ref double ury, double MinHeight, string WYNTKLanguage)
        {
            int _id_artifact, _tf, artifact_font, textendY;
            string tf_text, tf_tempoptlist, linebreak, result;
            linebreak = "\n";
            boolNewPage = false;
            double llx, lly, urx;
            
            if (string.Compare(WYNTKLanguage, "English", true) == 0)
            {
                if (Coll.Count > 0) //Sort Alphabetically
                    Coll.Sort(CatalogRecordCollection.CatalogSortFields.CatalogWYNTKCancerType, true);
                llx = 60; lly = 50; urx = 570; _tf = -1; //initialize;
                tf_tempoptlist = "charref=false fontname=Helvetica-Bold encoding=unicode fontsize=10 ";
                result = "";

                #region EnglishTextFlow
                //tf_text = "English WYNTK\n";
                tf_text = ConfigurationManager.AppSettings["WYNTKEnglishHeading"];
                _tf = _pdflib.add_textflow(_tf, tf_text + linebreak, tf_tempoptlist); //add textflow
                if (_tf == -1)
                    throw new Exception("Error: " + _pdflib.get_errmsg());

                tf_tempoptlist = "charref=false fontname=Helvetica encoding=unicode fontsize=9 ";
                tf_text = ConfigurationManager.AppSettings["WYNTKEnglishText"];
                _tf = _pdflib.add_textflow(_tf, tf_text + linebreak, tf_tempoptlist); //add textflow
                if (_tf == -1)
                    throw new Exception("Error: " + _pdflib.get_errmsg());

                //01-21-2011 ury = MinHeight; //Force this to go to next page
                
                do
                {
                    /*Begin available height check*/
                    boolNewPage = false; //new page flag
                    if (ury <= MinHeight) //Min height is a pre-defined value
                    {
                        boolNewPage = true; //set to true if min height condition is satisfied
                        #region new_page
                        this.DrawFooter(ref _pdflib, ref pagecounter);

                        _pdflib.end_page_ext(""); //end current page

                        pagecounter++;
                        _pdflib.begin_page_ext(0, 0, pagesize); //Begin a new page
                        llx = 60; lly = 50; urx = 570; ury = 745; //Reset Position Variables
                        #endregion
                        continue; //proceed to next iteration
                    }
                    /*End avaliable height check*/

                    tf_tempoptlist = "charref fontname=Helvetica-Bold encoding=winansi fontsize=8 ";

                    result = _pdflib.fit_textflow(_tf, llx, lly, urx, ury, ""); //fit textflow

                    /* "_boxfull" means we must continue because there is more text;
                     * "_nextpage" is interpreted as "start new column"
                     */
                    if (result == "_boxfull")
                    {
                        #region new_page
                        this.DrawFooter(ref _pdflib, ref pagecounter);

                        _pdflib.end_page_ext(""); //end current page

                        pagecounter++;
                        _pdflib.begin_page_ext(0, 0, pagesize); //Begin a new page
                        llx = 60; lly = 50; urx = 570; ury = 745; //Reset Position Variables
                        #endregion
                    }
                } while (result == "_boxfull" || boolNewPage);

                /* Check for errors */
                if (result != "_stop")
                {
                    /* "_boxempty" happens if the box is very small and doesn't
                     * hold any text at all.
                     */
                    if (result == "_boxempty")
                        throw new Exception("Error: Textflow box too small");
                    else
                    {
                        /* Any other return value is a user exit caused by
                         * the "return" option; this requires dedicated code to
                         * deal with.
                         */
                        throw new Exception("User return '" + result +
                                "' found in Textflow");
                    }
                }

                textendY = (int)_pdflib.info_textflow(_tf, "textendy");
                ury = textendY;
                _pdflib.delete_textflow(_tf);
                //ury -= fsize * 1.2; //ury starts with max value (at top) and is reduced 
                //as text is written from top to bottom
                //To get text closer this can be ury = textendY;
                /*End Place Text Flow*/

                #endregion

                llx = 100; lly = 50; urx = 500;


                #region WYNTK Table (English)
                //BEGIN TABLE
                /*begin table structure elements*/
                int _id_table, _tbl;
                int _id_thead, _id_row, _id_th, _id_body, _id_td, curr_row, curr_col;
                int font;
                string alignment, optlistHeaderCol1, optlistHeaderCol2, optlistHeaderCol3, optlistBodyCol1, optlistBodyCol2, optlistBodyCol3;

                _id_table = _pdflib.begin_item("Table", "Title={Catalogs WYNTK English Table}"); //Begin Table
                _id_thead = _pdflib.begin_item("THead", "Title={Header}"); //begin header
                _id_row = _pdflib.begin_item("TR", "Title={Header Row}"); //header row

                #region table_definition
                /*Begin Header Cells*/
                font = _pdflib.load_font("Helvetica-Bold", "unicode", ""); //Set a font type
                if (font == -1)
                    throw new Exception("Error: " + _pdflib.get_errmsg());
                
                alignment = "={ left bottom }";
                optlistHeaderCol1 =
                           "fittextline={position=" + alignment + " font=" + font + " fontsize=10 fillcolor={gray 0} } ";

                curr_col = 1; curr_row = 1; _tbl = -1; //initialize

                _id_th = _pdflib.begin_item("TH", "Scope=Column");
                //_id_th = _pdflib.begin_item("TH", "");
                _tbl = _pdflib.add_table_cell(_tbl, curr_col, curr_row, "Types of Cancer", optlistHeaderCol1 + " colwidth=40% marginbottom=3 rowheight=5 ");
                if (_tbl == -1)
                    throw new Exception("Error: " + _pdflib.get_errmsg());
                _pdflib.end_item(_id_th);

                alignment = "={ center bottom }"; //assign a new value for alignment
                optlistHeaderCol2 =
                            "fittextline={position=" + alignment + " font=" + font + " fontsize=10 fillcolor={gray 0} } ";

                curr_col++; curr_row = 1;

                _id_th = _pdflib.begin_item("TH", "Scope=Column");
                //_id_th = _pdflib.begin_item("TH", "");
                _tbl = _pdflib.add_table_cell(_tbl, curr_col, curr_row, "Inventory Number", optlistHeaderCol2 + " colwidth=40% marginbottom=3 rowheight=5 ");
                if (_tbl == -1)
                    throw new Exception("Error: " + _pdflib.get_errmsg());
                _pdflib.end_item(_id_th);

                alignment = "={ right bottom }"; //assign a new value for alignment
                optlistHeaderCol3 =
                            "fittextline={position=" + alignment + " font=" + font + " fontsize=10 fillcolor={gray 0} } ";

                curr_col++; curr_row = 1;

                _id_th = _pdflib.begin_item("TH", "Scope=Column");
                //_id_th = _pdflib.begin_item("TH", "");
                _tbl = _pdflib.add_table_cell(_tbl, curr_col, curr_row, "Order Limit", optlistHeaderCol3 + " colwidth=20% marginbottom=3 rowheight=5 ");
                if (_tbl == -1)
                    throw new Exception("Error: " + _pdflib.get_errmsg());
                _pdflib.end_item(_id_th);
                /*End Header Cells*/

                _pdflib.end_item(_id_row); //End header row
                _pdflib.end_item(_id_thead); //End Header
                _id_body = _pdflib.begin_item("TBody", "Title={Body}"); //Begin Body
                
                /*Begin Data Cells*/
                font = _pdflib.load_font("Helvetica", "unicode", ""); //Set a font type
                if (font == -1)
                    throw new Exception("Error: " + _pdflib.get_errmsg());
                optlistBodyCol1 =
                        "fittextline={position=left font=" + font + " fontsize=10} ";
                optlistBodyCol2 =
                        "fittextline={position=left font=" + font + " fontsize=10} ";
                optlistBodyCol3 =
                        "fittextline={position=right font=" + font + " fontsize=10} ";

                string tf_optlist;
                string text;

                /*End of table structure elements*/
                #endregion

                foreach (CatalogRecord CollItem in Coll)
                {
                    _id_row = _pdflib.begin_item("TR", "Title={Body Row}"); //body row
                    
                    curr_row++;

                    #region Column 1

                    curr_col = 1; //column number

                    _tf = -1; //initialize text flow object handle
                
                    //Cancer Type
                    tf_optlist = "fontname=Helvetica encoding=unicode fontsize=9 ";
                    //text = this.GetCancerTypeText(CollItem.CatalogWYNTKCancerType);
                    text = CollItem.CatalogWYNTKCancerTypeDesc;
                    if (text.Length == 0)
                        text = "NONE";
                    _tf = _pdflib.add_textflow(_tf, text, tf_optlist);
                    if (_tf == -1)
                        throw new Exception("Error: " + _pdflib.get_errmsg());

                    optlistBodyCol1 = " textflow=" + _tf;

                    _id_td = _pdflib.begin_item("TD", "");
                    _tbl = _pdflib.add_table_cell(_tbl, curr_col, curr_row, "", optlistBodyCol1 + " margintop=1 rowheight=5 ");
                    if (_tbl == -1)
                        throw new Exception("Error: " + _pdflib.get_errmsg());
                    _pdflib.end_item(_id_td);

                    #endregion

                    #region Column 2

                    curr_col = 2; //column number
                    _tf = -1; //initialize text flow object handle

                    //Inventory Number
                    tf_optlist = "fontname=Helvetica encoding=unicode fontsize=9 alignment=center ";
                    text = CollItem.ProductId;
                    if (text.Length == 0)
                        text = "NONE";
                    _tf = _pdflib.add_textflow(_tf, text, tf_optlist);
                    if (_tf == -1)
                        throw new Exception("Error: " + _pdflib.get_errmsg());

                    optlistBodyCol2 = " textflow=" + _tf;

                    _id_td = _pdflib.begin_item("TD", "");
                    _tbl = _pdflib.add_table_cell(_tbl, curr_col, curr_row, "", optlistBodyCol2 + " margintop=1 rowheight=5 ");
                    if (_tbl == -1)
                        throw new Exception("Error: " + _pdflib.get_errmsg());
                    _pdflib.end_item(_id_td);

                    #endregion

                    #region Column 3

                    curr_col = 3; //column number
                    _tf = -1; //initialize text flow object handle

                    //OrderLimit
                    tf_optlist = "fontname=Helvetica encoding=unicode fontsize=9 alignment=right";
                    text = CollItem.Limit;
                    _tf = _pdflib.add_textflow(_tf, text, tf_optlist);
                    if (_tf == -1)
                        throw new Exception("Error: " + _pdflib.get_errmsg());

                    optlistBodyCol3 = " textflow=" + _tf;

                    _id_td = _pdflib.begin_item("TD", "");
                    _tbl = _pdflib.add_table_cell(_tbl, curr_col, curr_row, "", optlistBodyCol3 + " margintop=1 rowheight=5 ");
                    if (_tbl == -1)
                        throw new Exception("Error: " + _pdflib.get_errmsg());
                    _pdflib.end_item(_id_td);

                    #endregion

                    _pdflib.end_item(_id_row); //End body row

                }

                /*begin table structure elements*/
                _pdflib.end_item(_id_body); //End Body
                _pdflib.end_item(_id_table); //End Table
                /*End of table structure elements*/

                #region Place Table
                ///*Begin Place the Table*/
                /* ---------- Place the table on one or more pages ---------- */

                /*
                 * Loop until all of the table is placed; create new pages
                 * as long as more table instances need to be placed.
                 */
                do
                {
                    /*Begin available height check*/
                    boolNewPage = false; //new page flag
                    if (ury <= MinHeight) //Min height is a pre-defined value
                    {
                        boolNewPage = true; //set to true if min height condition is satisfied
                        #region new_page
                        this.DrawFooter(ref _pdflib, ref pagecounter);

                        _pdflib.end_page_ext(""); //end current page

                        pagecounter++;
                        _pdflib.begin_page_ext(0, 0, pagesize); //Begin a new page
                        llx = 100; lly = 50; urx = 500; ury = 745;
                        #endregion
                        continue; //proceed to next iteration
                    }
                    /*End avaliable height check*/

                    string optlist = 
                            "stroke={line=hor1}";

                    /* Place the table instance */
                    result = _pdflib.fit_table(_tbl, llx, lly, urx, ury, optlist);

                    if (result == "_boxfull")
                    {
                        #region new_page
                        this.DrawFooter(ref _pdflib, ref pagecounter);

                        _pdflib.end_page_ext(""); //end current page

                        pagecounter++;
                        _pdflib.begin_page_ext(0, 0, pagesize); //Begin a new page
                        llx = 100; lly = 50; urx = 500; ury = 745; //Reset Position Variables
                        #endregion
                    }

                } while (result == "_boxfull" || boolNewPage);  //while (result == "_boxfull") ;

                /* Check the result; "_stop" means all is ok. */
                if (result != "_stop")
                {
                    if (result == "_error")
                    {
                        throw new Exception("Error when placing table: " +
                                _pdflib.get_errmsg());
                    }
                    else
                    {
                        /* Any other return value is a user exit caused by
                         * the "return" option; this requires dedicated code to
                         * deal with.
                         */
                        throw new Exception("User return found in Textflow");
                    }
                }

                /* This will also delete Textflow handles used in the table */
                textendY = (int)_pdflib.info_table(_tbl, "height");
                ury = ury - textendY - Yoffset; //fsize * 1.2; //Adjust to fit remaining text
                _pdflib.delete_table(_tbl, "");

                ///*End Place the Table*/

                #endregion


                //END TABLE
                #endregion

            }
            else if (string.Compare(WYNTKLanguage, "Spanish", true) == 0)
            {
                if (Coll.Count > 0) //Sort Alphabetically
                    Coll.Sort(CatalogRecordCollection.CatalogSortFields.CatalogWYNTKCancerType, true);
                llx = 60; lly = 50; urx = 570; _tf = -1; //initialize;
                tf_tempoptlist = "charref=false fontname=Helvetica-Bold encoding=unicode fontsize=10 ";
                result = "";

                #region SpanishTextFlow
                tf_text = ConfigurationManager.AppSettings["WYNTKSpanishHeading"];
                _tf = _pdflib.add_textflow(_tf, tf_text + linebreak, tf_tempoptlist); //add textflow
                if (_tf == -1)
                    throw new Exception("Error: " + _pdflib.get_errmsg());

                tf_tempoptlist = "charref=false fontname=Helvetica encoding=unicode fontsize=9 ";
                tf_text = ConfigurationManager.AppSettings["WYNTKSpanishText"];
                _tf = _pdflib.add_textflow(_tf, tf_text + linebreak, tf_tempoptlist); //add textflow
                if (_tf == -1)
                    throw new Exception("Error: " + _pdflib.get_errmsg());

                tf_tempoptlist = "charref=false fontname=Helvetica encoding=unicode fontsize=9 ";
                tf_text = ConfigurationManager.AppSettings["WYNTKSpanishText2"];
                _tf = _pdflib.add_textflow(_tf, linebreak + tf_text + linebreak, tf_tempoptlist); //add textflow
                if (_tf == -1)
                    throw new Exception("Error: " + _pdflib.get_errmsg());


                do
                {
                    /*Begin available height check*/
                    boolNewPage = false; //new page flag
                    if (ury <= MinHeight) //Min height is a pre-defined value
                    {
                        boolNewPage = true; //set to true if min height condition is satisfied
                        #region new_page
                        this.DrawFooter(ref _pdflib, ref pagecounter);

                        _pdflib.end_page_ext(""); //end current page

                        pagecounter++;
                        _pdflib.begin_page_ext(0, 0, pagesize); //Begin a new page
                        llx = 60; lly = 50; urx = 570; ury = 745; //Reset Position Variables
                        #endregion
                        continue; //proceed to next iteration
                    }
                    /*End avaliable height check*/

                    result = _pdflib.fit_textflow(_tf, llx, lly, urx, ury, ""); //fit textflow

                    /* "_boxfull" means we must continue because there is more text;
                     * "_nextpage" is interpreted as "start new column"
                     */
                    if (result == "_boxfull")
                    {
                        #region new_page
                        this.DrawFooter(ref _pdflib, ref pagecounter);

                        _pdflib.end_page_ext(""); //end current page

                        pagecounter++;
                        _pdflib.begin_page_ext(0, 0, pagesize); //Begin a new page
                        llx = 60; lly = 50; urx = 570; ury = 745; //Reset Position Variables
                        #endregion
                    }
                } while (result == "_boxfull" || boolNewPage);

                /* Check for errors */
                if (result != "_stop")
                {
                    /* "_boxempty" happens if the box is very small and doesn't
                     * hold any text at all.
                     */
                    if (result == "_boxempty")
                        throw new Exception("Error: Textflow box too small");
                    else
                    {
                        /* Any other return value is a user exit caused by
                         * the "return" option; this requires dedicated code to
                         * deal with.
                         */
                        throw new Exception("User return '" + result +
                                "' found in Textflow");
                    }
                }

                textendY = (int)_pdflib.info_textflow(_tf, "textendy");
                ury = textendY;
                _pdflib.delete_textflow(_tf);
                //ury -= fsize * 1.2; //ury starts with max value (at top) and is reduced 
                //as text is written from top to bottom
                //To get text closer this can be ury = textendY;
                /*End Place Text Flow*/

                #endregion

                llx = 100; lly = 50; urx = 500;

                #region WYNTK Table (Spanish)
                //BEGIN TABLE
                /*begin table structure elements*/
                int _id_table, _tbl;
                int _id_thead, _id_row, _id_th, _id_body, _id_td, curr_row, curr_col;
                int font;
                string alignment, optlistHeaderCol1, optlistHeaderCol2, optlistHeaderCol3, optlistBodyCol1, optlistBodyCol2, optlistBodyCol3;

                _id_table = _pdflib.begin_item("Table", "Title={Catalogs WYNTK Spanish Table}"); //Begin Table
                _id_thead = _pdflib.begin_item("THead", "Title={Header}"); //begin header
                _id_row = _pdflib.begin_item("TR", "Title={Header Row}"); //header row

                #region table_definition

                /*Header Cells as textflows*/
                //Cancer Type
                string headertext, tf_headeroptlist;
                tf_headeroptlist = "fontname=Helvetica-Bold encoding=unicode fontsize=10 ";
                headertext = "Tipos de cancer\n(Types of Cancer)";
                _tf = -1;
                _tf = _pdflib.add_textflow(_tf, headertext, tf_headeroptlist);
                if (_tf == -1)
                    throw new Exception("Error: " + _pdflib.get_errmsg());

                curr_col = 1; curr_row = 1; _tbl = -1; //initialize

                _id_th = _pdflib.begin_item("TH", "Scope=Column");
                //_id_th = _pdflib.begin_item("TH", "");
                _tbl = _pdflib.add_table_cell(_tbl, curr_col, curr_row, "", "textflow=" + _tf + " colwidth=40% marginbottom=3 rowheight=5 ");
                if (_tbl == -1)
                    throw new Exception("Error: " + _pdflib.get_errmsg());
                _pdflib.end_item(_id_th);

                //Inventory Numer
                tf_headeroptlist = "fontname=Helvetica-Bold encoding=unicode fontsize=10 alignment=center ";
                headertext = "Numero de inventario\n  (Inventory Number)";
                _tf = -1;
                _tf = _pdflib.add_textflow(_tf, headertext, tf_headeroptlist);
                if (_tf == -1)
                    throw new Exception("Error: " + _pdflib.get_errmsg());

                curr_col++; curr_row = 1;

                _id_th = _pdflib.begin_item("TH", "Scope=Column");
                //_id_th = _pdflib.begin_item("TH", "");
                _tbl = _pdflib.add_table_cell(_tbl, curr_col, curr_row, "", "textflow=" + _tf + " colwidth=40% marginbottom=3 rowheight=5 ");
                if (_tbl == -1)
                    throw new Exception("Error: " + _pdflib.get_errmsg());
                _pdflib.end_item(_id_th);

                //Order Limit
                tf_headeroptlist = "fontname=Helvetica-Bold encoding=unicode fontsize=10 ";
                headertext = "Limite de pedido\n    (Order Limit)";
                _tf = -1;
                _tf = _pdflib.add_textflow(_tf, headertext, tf_headeroptlist);
                if (_tf == -1)
                    throw new Exception("Error: " + _pdflib.get_errmsg());

                curr_col++; curr_row = 1;

                _id_th = _pdflib.begin_item("TH", "Scope=Column");
                //_id_th = _pdflib.begin_item("TH", "");
                _tbl = _pdflib.add_table_cell(_tbl, curr_col, curr_row, "", "textflow=" + _tf + " colwidth=20% marginbottom=3 rowheight=5 ");
                if (_tbl == -1)
                    throw new Exception("Error: " + _pdflib.get_errmsg());
                _pdflib.end_item(_id_th);

                /*End Header Cells textflows*/

                _pdflib.end_item(_id_row); //End header row
                _pdflib.end_item(_id_thead); //End Header
                _id_body = _pdflib.begin_item("TBody", "Title={Body}"); //Begin Body

                /*Begin Data Cells*/
                font = _pdflib.load_font("Helvetica", "unicode", ""); //Set a font type
                if (font == -1)
                    throw new Exception("Error: " + _pdflib.get_errmsg());
                optlistBodyCol1 =
                        "fittextline={position=left font=" + font + " fontsize=10} ";
                optlistBodyCol2 =
                        "fittextline={position=left font=" + font + " fontsize=10} ";
                optlistBodyCol3 =
                        "fittextline={position=left font=" + font + " fontsize=10} ";

                string tf_optlist;
                string text;

                /*End of table structure elements*/
                #endregion

                foreach (CatalogRecord CollItem in Coll)
                {
                    _id_row = _pdflib.begin_item("TR", "Title={Body Row}"); //body row
                    
                    curr_row++;

                    #region Column 1

                    curr_col = 1; //column number

                    _tf = -1; //initialize text flow object handle

                    //Cancer Type
                    tf_optlist = "fontname=Helvetica encoding=unicode fontsize=9 ";
                    //text = this.GetCancerTypeText(CollItem.CatalogWYNTKCancerType);
                    text = CollItem.CatalogWYNTKCancerTypeDesc;
                    if (text.Length == 0)
                        text = "NONE";
                    _tf = _pdflib.add_textflow(_tf, text, tf_optlist);
                    if (_tf == -1)
                        throw new Exception("Error: " + _pdflib.get_errmsg());

                    optlistBodyCol1 = " textflow=" + _tf;

                    _id_td = _pdflib.begin_item("TD", "");
                    _tbl = _pdflib.add_table_cell(_tbl, curr_col, curr_row, "", optlistBodyCol1 + " margintop=1 rowheight=5 ");
                    if (_tbl == -1)
                        throw new Exception("Error: " + _pdflib.get_errmsg());
                    _pdflib.end_item(_id_td);

                    #endregion

                    #region Column 2

                    curr_col = 2; //column number
                    _tf = -1; //initialize text flow object handle

                    //Inventory Number
                    tf_optlist = "fontname=Helvetica encoding=unicode fontsize=9 alignment=center ";
                    text = CollItem.ProductId;
                    if (text.Length == 0)
                        text = "NONE";
                    _tf = _pdflib.add_textflow(_tf, text, tf_optlist);
                    if (_tf == -1)
                        throw new Exception("Error: " + _pdflib.get_errmsg());

                    optlistBodyCol2 = " textflow=" + _tf;

                    _id_td = _pdflib.begin_item("TD", "");
                    _tbl = _pdflib.add_table_cell(_tbl, curr_col, curr_row, "", optlistBodyCol2 + " margintop=1 rowheight=5 ");
                    if (_tbl == -1)
                        throw new Exception("Error: " + _pdflib.get_errmsg());
                    _pdflib.end_item(_id_td);

                    #endregion

                    #region Column 3

                    curr_col = 3; //column number
                    _tf = -1; //initialize text flow object handle

                    //OrderLimit
                    tf_optlist = "fontname=Helvetica encoding=unicode fontsize=9 alignment=right";
                    text = CollItem.Limit;
                    _tf = _pdflib.add_textflow(_tf, text, tf_optlist);
                    if (_tf == -1)
                        throw new Exception("Error: " + _pdflib.get_errmsg());

                    optlistBodyCol3 = " textflow=" + _tf;

                    _id_td = _pdflib.begin_item("TD", "");
                    _tbl = _pdflib.add_table_cell(_tbl, curr_col, curr_row, "", optlistBodyCol3 + " margintop=1 rowheight=5 ");
                    if (_tbl == -1)
                        throw new Exception("Error: " + _pdflib.get_errmsg());
                    _pdflib.end_item(_id_td);

                    #endregion

                    _pdflib.end_item(_id_row); //End body row

                }

                /*begin table structure elements*/
                _pdflib.end_item(_id_body); //End Body
                _pdflib.end_item(_id_table); //End Table
                /*End of table structure elements*/

                #region Place Table
                ///*Begin Place the Table*/
                /* ---------- Place the table on one or more pages ---------- */

                /*
                 * Loop until all of the table is placed; create new pages
                 * as long as more table instances need to be placed.
                 */
                do
                {
                    /*Begin available height check*/
                    boolNewPage = false; //new page flag
                    if (ury <= MinHeight) //Min height is a pre-defined value
                    {
                        boolNewPage = true; //set to true if min height condition is satisfied
                        #region new_page
                        this.DrawFooter(ref _pdflib, ref pagecounter);

                        _pdflib.end_page_ext(""); //end current page

                        pagecounter++;
                        _pdflib.begin_page_ext(0, 0, pagesize); //Begin a new page
                        llx = 100; lly = 50; urx = 500; ury = 745;
                        #endregion
                        continue; //proceed to next iteration
                    }
                    /*End avaliable height check*/

                    string optlist =
                            "stroke={line=hor1}";

                    /* Place the table instance */
                    result = _pdflib.fit_table(_tbl, llx, lly, urx, ury, optlist);

                    if (result == "_boxfull")
                    {
                        #region new_page
                        this.DrawFooter(ref _pdflib, ref pagecounter);

                        _pdflib.end_page_ext(""); //end current page

                        pagecounter++;
                        _pdflib.begin_page_ext(0, 0, pagesize); //Begin a new page
                        llx = 100; lly = 50; urx = 500; ury = 745;
                        #endregion
                    }

                } while (result == "_boxfull" || boolNewPage);  //while (result == "_boxfull") ;

                /* Check the result; "_stop" means all is ok. */
                if (result != "_stop")
                {
                    if (result == "_error")
                    {
                        throw new Exception("Error when placing table: " +
                                _pdflib.get_errmsg());
                    }
                    else
                    {
                        /* Any other return value is a user exit caused by
                         * the "return" option; this requires dedicated code to
                         * deal with.
                         */
                        throw new Exception("User return found in Textflow");
                    }
                }

                /* This will also delete Textflow handles used in the table */
                textendY = (int)_pdflib.info_table(_tbl, "height");
                ury = ury - textendY - Yoffset; //fsize * 1.2; //Adjust to fit remaining text
                _pdflib.delete_table(_tbl, "");

                ///*End Place the Table*/

                #endregion


                //END TABLE
                #endregion WYNTK Table

            }

        }

        /***********BEGIN TABLE OF CONTENTS METHOD************/
        private void create_toc2(ref PDFlib p) //begin = 1 to begin page, begin = 2 to end page
        {
            string tf_tempoptlist = "charref fontname=Helvetica encoding=unicode fontsize=10 fillcolor={white 0} ";
            string CoverPageText = "";
            int cover_font = -1;
            ///First create Cover Page
            #region CoverPage
            p.begin_page_ext(0, 0, pagesize + " taborder=structure "); //Begin Cover Page

            p.save(); //Save previous graphic state

            /*Begin - Draw background color rectangles with set width and height */
            //p.setcolor("fillstroke", "#00006A", 0.0, 0.0, 0.0, 0.0);
            p.setcolor("fillstroke", "#224C7C", 0.0, 0.0, 0.0, 0.0);
            //p.rect(0, 0, 100, 800);
            p.rect(0, 0, 135, 800);
            p.fill();
            //p.setcolor("fillstroke", "#A7A7A7", 0.0, 0.0, 0.0, 0.0);
            p.setcolor("fillstroke", "#A9B3BC", 0.0, 0.0, 0.0, 0.0);
            //p.rect(100, 0, 550, 800);
            p.rect(135, 0, 550, 800);
            p.fill();
            /*End - Draw rectangle*/
            
            //First line
            CoverPageText = "National Cancer Institute";
            cover_font = p.load_font("Times", "unicode", "fontstyle=bolditalic");
            //p.setcolor("fillstroke", "#00006A", 0.0, 0.0, 0.0, 0.0);
            p.setcolor("fillstroke", "#224C7C", 0.0, 0.0, 0.0, 0.0);
            p.setfont(cover_font, 30); //Set a font size
            p.show_xy(CoverPageText, 225, 550);
            
            //Second line
            CoverPageText = "Publications Catalog";
            //cover_font = p.load_font("Times", "unicode", "fontstyle=bolditalic");
            //_pdflib.setcolor("fillstroke", "#00006A", 0.0, 0.0, 0.0, 0.0);
            p.setfont(cover_font, 38); //Set a font size
            p.show_xy(CoverPageText, 225, 490);

            //third line
            CoverPageText = "and Order Form";
            cover_font = p.load_font("Helvetica", "unicode", "");
            _pdflib.setcolor("fillstroke", "white", 0.0, 0.0, 0.0, 0.0);
            p.setfont(cover_font, 18);
            p.show_xy(CoverPageText, 415, 440);

            //fourth line
            CoverPageText = "U.S. DEPARTMENT OF HEALTH AND HUMAN SERVICES";
            cover_font = p.load_font("Helvetica", "unicode", "fontstyle=bold");
            //_pdflib.setcolor("fillstroke", "#00006A", 0.0, 0.0, 0.0, 0.0);
            p.setfont(cover_font, 9);
            p.fit_textline(CoverPageText, 200, 65, "charspacing=2");

            //fifth line
            CoverPageText = "NATIONAL INSTITUTES OF HEALTH * NATIONAL CANCER INSTITUTE";
            cover_font = p.load_font("Helvetica", "unicode", "fontstyle=bold");
            //_pdflib.setcolor("fillstroke", "#00006A", 0.0, 0.0, 0.0, 0.0);
            p.setfont(cover_font, 6);
            p.fit_textline(CoverPageText, 208, 55, "charspacing=2");

            p.restore(); //Restore previous graphic state
            
            p.end_page_ext(""); //End Cover Page
            #endregion

            #region Cover_Bookmark
            string bookmarkdest = "destinationCover"; //define a name for the named destination - no space allowed
            p.add_nameddest(bookmarkdest, "type=fixed left=" + 0 + " top=" + 800); //add the named destination
            int bookmarkaction = p.create_action("GoTo", " destname=" + bookmarkdest);
            p.create_bookmark("National Cancer Institute Publications Catalog and Order Form", " action={activate=" + bookmarkaction + "}");
            #endregion


            ///Then Continue to Create TOC

            /* Option list to indicate the start of a matchbox */
            string startopts = "";
            
            /* Option list to indicate the end of a matchbox */
            string endopts = "matchbox=end";
            
             /* Standard option list for adding a Textflow.
             * "avoidemptybegin" deletes empty lines at the beginning of a fitbox.
             * "charref" enables the substitution of numeric and character entity
             * or glyph name references, e.g. of the character reference "&shy;"
             * for a soft hyphen.
             */
             string stdopts = "fontname=Helvetica fontsize=12 encoding=unicode " +
                                "leading=120% charref avoidemptybegin ";

             /* ---------------------------------------------------------------------
             * Construct the contents of the index page(s) based on the collected
             * pairs containing the indexed term plus the corresponding page number
             * ---------------------------------------------------------------------
             */

             /* Supply the standard options to the index Textflow. This has to be 
              * done only once for each Textflow. Further calls of add_textflow() for
              * this Textflow will use these settings by default.
              */

             //string tf_tempoptlist = "charref fontname=Helvetica-Bold encoding=winansi fontsize=8 ";
             tf_tempoptlist = "charref fontname=Arial encoding=unicode fontsize=26 ";

             int idx = -1;
             idx = p.add_textflow(-1, "", stdopts);
             if (idx == -1)
                 throw new Exception("Error: " + p.get_errmsg());

             /* Add the TOCHeaderText to the index Textflow */
             string TOCHeaderText = "Materials From the\nNational Cancer Institute";
             idx = p.add_textflow(idx, TOCHeaderText + "\n", tf_tempoptlist);
             if (idx == -1)
                 throw new Exception("Error: " + p.get_errmsg());

             tf_tempoptlist = "charref fontname=Times-Bold encoding=unicode fontsize=14 "; 
             string SubHeading = "For Patients, Health Professionals, and the Public";
             idx = p.add_textflow(idx, SubHeading + "\n\n", tf_tempoptlist);
             if (idx == -1)
                 throw new Exception("Error: " + p.get_errmsg());

             tf_tempoptlist = "charref fontname=Arial encoding=unicode fontsize=10 ";
             //string OtherText = "The National Cancer Institute (NCI) is part of the Federal Government. NCI's materials reflect the nation’s investment " +
             //                   "in improving the prevention, detection, diagnosis, and treatment for cancer. NCI provides the materials described in " +
             //                   "this catalog to give accurate and timely information to patients and their families; to health care providers and " +
             //                   "communicators; and to science writers, educators, and the interested public.";
             string OtherText = "The National Cancer Institute (NCI) is part of the Federal Government.  Materials provided by " +
                                "NCI reflect the nation’s investment in improving the prevention, detection, diagnosis, and " +
                                "treatment of cancer.  The materials described in this catalog are intended to give accurate and " +
                                "timely information to patients and their families; to health care providers and communicators; " +
                                "and to science writers, educators, and the interested public.";
             idx = p.add_textflow(idx, OtherText + "\n\n", tf_tempoptlist);
             if (idx == -1)
                 throw new Exception("Error: " + p.get_errmsg());

             tf_tempoptlist = "charref fontname=Helvetica encoding=unicode fontsize=10 fillcolor={white 0} "; 
             string OtherText2 = "The list of publications presently available from NCI is organized as follows:";
             idx = p.add_textflow(idx, OtherText2 + "\n\n", tf_tempoptlist);
             if (idx == -1)
                 throw new Exception("Error: " + p.get_errmsg());

            
            
             /* Add the collected and sorted index entries to the index Textflow */
             for (int i = 0; i < arrlstTOC.Count; i++)
             {
                 /* Add the indexed term of the index entry */
                 //idx = p.add_textflow(idx, arrlstTOC[i].ToString() + "",
                 //    "fillcolor={gray 0}");
                 idx = p.add_textflow(idx, arrlstTOC[i].ToString().ToUpper() + "\t",
                     " fontname=Times encoding=unicode fontsize=9 fillcolor={gray 0} leading=160% ruler=100% hortabmethod=ruler tabalignment=right leader={text={.}}");
                 if (idx == -1)
                     throw new Exception("Error: " + p.get_errmsg());

                 /* Add the page number of the index entry. In addition, define a
                  * matchbox with a sequence number as the name. This matchbox will 
                  * be used later to define a link annotation on it to jump to the 
                  * respective page. 
                  */
                 //idx = p.add_textflow(idx, arrlstTOCPageNum[i].ToString() + "",
                 //    " fontname=Times encoding=unicode fontsize=11 fillcolor={rgb 0 0.95 0.95} matchbox={name=" + i + "}");
                 idx = p.add_textflow(idx, arrlstTOCPageNum[i].ToString() + "",
                     " fontname=Times encoding=unicode fontsize=11 matchbox={name=" + i + "}");
                 if (idx == -1)
                     throw new Exception("Error: " + p.get_errmsg());

                 idx = p.add_textflow(idx, "\n", endopts);
                 if (idx == -1)
                     throw new Exception("Error: " + p.get_errmsg());

             }

             /* ---------------------------------------------------------------------
            * Place the index Textflow with each entry consisting of a text, a page
            * number, and a link annotation on the page number
            * ---------------------------------------------------------------------
            */

            /* Initialize the current number of the index entry */
            int entryno = 0;
            int pageno = 0;
            string result = "";
            double llx = 60, lly = 50, urx = 570, ury = 745; //Main Position Variables
            bool boolDrawColor = true; //Draw the blue color only once

            /* Loop until all index entries are placed; create new pages as long as
            * more index entries need to be placed
            */

            do
            {
                //p.begin_page_ext(pagewidth, pageheight, "");
                p.begin_page_ext(0, 0, pagesize + " taborder=structure "); //begin the page
                //pageno++;

                if (boolDrawColor)
                {
                    /*Begin - Draw a rectangle with the retrieved width and height */
                    _pdflib.setcolor("fillstroke", "#00006A", 0.0, 0.0, 0.0, 0.0);
                    _pdflib.rect(0, 570, 570, 20); //Hardcoded co-ordinates
                    _pdflib.fill();
                    /*End - Draw rectangle*/
                    boolDrawColor = false;
                }

                /* Fit the index Textflow */
                result = p.fit_textflow(idx, llx, lly, urx, ury, "");
                
                /* Collect the index entries by retrieving the number of matchboxes
                 * on the current page
                 */

                //mcount = p.info_matchbox("*", 1, "count");
                double mcount = p.info_matchbox("*", 1, "count");

                for (int i = 1; i <= mcount; i++)
                {   
                    /* Get the matchbox name which corresponds to the text of the
                     * index entry
                     */
                    double minfo = p.info_matchbox("*", i, "name");
                    string mname = p.get_parameter("string", minfo);

                    int destpagenum = Int32.Parse(arrlstTOCPageNum[i - 1].ToString());
                    destpagenum++; //Added to take into consideration the cover page
                    //int destpagenum = Int32.Parse(arrlstTOCPageNum[i-2].ToString());
                    //destpagenum++; //For PDFlib index page is page one, so increase one to get to the real page.

                    int action = p.create_action("GoTo", "destination={page=" +
                        (destpagenum.ToString()) + " type=fixed }");

                    /* With the "GoTo" action, create a Link annotation on the
                     * matchbox defined above. 0 rectangle coordinates will be
                     * replaced with matchbox coordinates.
                     */
                    p.create_annotation(0, 0, 0, 0, "Link",
                        "action={activate " + action + "} linewidth=0 " +
                        "usematchbox={" + mname + "}");

                    entryno++;


                }

                #region pageno
                    this.DrawFooter(ref _pdflib,ref pageno);
                #endregion


                p.end_page_ext("");


                pageno++;


            } while (result == "_boxfull" || result == "_nextpage");

            /* Check for errors */
            if (result != "_stop")
            {
                /* "_boxempty" happens if the box is very small and doesn't
                 * hold any text at all.
                 */

                if (result == "_boxempty")
                    throw new Exception("Error: Textflow box too small");
                else
                {
                    /* Any other return value is a user exit caused by
                     * the "return" option; this requires dedicated code to
                     * deal with.
                     */

                    throw new Exception("User return '" + result +
                        "' found in Textflow");

                }
            }
            p.delete_textflow(idx);

            #region TOC_Bookmark
            bookmarkdest = "destinationIndex"; //define a name for the named destination - no space allowed
            p.add_nameddest(bookmarkdest, "type=fixed left=" + 0 + " top=" + 800); //add the named destination
            bookmarkaction = p.create_action("GoTo", " destname=" + bookmarkdest);
            p.create_bookmark("Materials From the National Cancer Institute", " action={activate=" + bookmarkaction + "}");
            #endregion

         }
        /***********END TABLE OF CONTENTS METHOD    ************/

        private void DrawFooter(ref PDFlib p, ref int pagenum)
        {
            p.save(); //Save current graphic state
            
            /* The page number is created as an artifact; it will be
             * ignored when reflowing the page in Acrobat.
             */
            _id_artifact = _pdflib.begin_item("Artifact", "");
            int artifact_font = _pdflib.load_font("Helvetica", "unicode", ""); //Set a font type
            if (artifact_font == -1)
                throw new Exception("Error: " + _pdflib.get_errmsg());
            p.setfont(artifact_font, 8); //Set a font size
            //_pdflib.setcolor("fillstroke", "#C0C0C0", 0.0, 0.0, 0.0, 0.0); //silver color
            p.setcolor("fillstroke", "rgb", 0.0, 0.0, 0.0, 0.0); //set a color
            p.setlinewidth(0.5);
            //_pdflib.setdashpattern("dasharray={0.8 0.8}");
            p.moveto(60, 40);
            p.lineto(570, 40);
            p.stroke();
            p.show_xy("NCI Publications Catalog", 60, 25);
            if (pagenum == 0) //Only for TOC Page
            {
                p.show_xy("1", 310, 25);
                p.show_xy("Last Revised: " + GlobalUtilities.UtilityMethods.GetCurrentDate(), 60, 15);
            }
            else
                p.show_xy(pagenum.ToString(), 310, 25);
            p.end_item(_id_artifact);

            p.restore(); //Restore the graphic state
        }

        private void AddStaticPages(ref PDFlib p, ref int pagecounter, int createmode)
        {
            string tf_optlist = "charref fontname=Helvetica encoding=unicode fontsize=10 fillcolor={gray 0} ";
            string text = "";
            string result = "";
            double llx = 60, lly = 50, urx = 570, ury = 745; //Main Position Variables
            _tf = -1; int _tbl = -1;

            #region FirstPage

            tf_optlist = "fontname=Times-Bold encoding=unicode fontsize=14 ";
            text = "How to Place an Order\n\n";
            _tf = p.add_textflow(_tf, text, tf_optlist + "alignment=center ");
            if (_tf == -1)
                throw new Exception("Error: " + p.get_errmsg());

            tf_optlist = "charref fontname=Arial encoding=unicode fontsize=10 alignment=left";
            //HITT 11987 text = "NCI publications are free. However, there is a shipping and handling fee of 15 cents per copy for" + 
            //HITT 11987 " orders of more than 20 items (bulk orders). To cover the basic accounting costs associated with" +
            //HITT 11987 " processing payments for bulk orders, NCI requires an $8 minimum charge.\n\n";
            text = "NCI publications are free, and there are no handling charges associated with them.\n\n";
            _tf = p.add_textflow(_tf, text, tf_optlist);
            if (_tf == -1)
                throw new Exception("Error: " + p.get_errmsg());

            //HITT 11987 text = "To ensure that publications are available to all requesters," +
            //HITT 11987     " we may set limits on the maximum number of copies that may be ordered." +
            //HITT 11987     " The number of copies of an individual title that may be ordered" + 
            //HITT 11987     " depends on the supply in our inventory.\n\n";
            text = "Domestic Orders\n\n";
            tf_optlist = "charref fontname=Times-Bold encoding=unicode fontsize=12 ";
            _tf = p.add_textflow(_tf, text, tf_optlist);
            if (_tf == -1)
                throw new Exception("Error: " + p.get_errmsg());

            tf_optlist = "charref fontname=Arial encoding=unicode fontsize=10 ";
            //HITT 11987 text = "Orders for NCI publications can be placed by Internet, phone, mail, and fax.\n\n";
            text = "There are no shipping charges for domestic orders of up to 20 items. Such an order may consist of 1 title (20 copies), " +
                    "if applicable, or of any combination of titles (up to 20 items in total).\n\n";
            _tf = p.add_textflow(_tf, text, tf_optlist);
            if (_tf == -1)
                throw new Exception("Error: " + p.get_errmsg());

            //Begin HITT 11987
            text = "Although the NCI Distribution Center does not charge for shipping domestic orders of more than 20 items (bulk " +
                   "orders), you must provide a FedEx or UPS shipping number when placing a bulk order, and the final cost you pay " +
                   "is between you and your carrier (FedEx/UPS). It will be based on the size and weight of the package, the method of " +
                   "delivery, and the rates you have established with the carrier and will be invoiced directly to you. To continue to place " +
                   "bulk orders, you must keep your FedEx or UPS shipping number in good standing. \n\n";
            _tf = p.add_textflow(_tf, text, tf_optlist);
            if (_tf == -1)
                throw new Exception("Error: " + p.get_errmsg());

            text = "Submit domestic orders via Internet, phone, mail, or fax:\n\n";
            _tf = p.add_textflow(_tf, text, tf_optlist);
            if (_tf == -1)
                throw new Exception("Error: " + p.get_errmsg());
            //End HITT 11987


            tf_optlist = "charref fontname=Arial encoding=unicode fontsize=10 fontstyle=bold";
            text = "\tInternet";
            _tf = p.add_textflow(_tf, text, tf_optlist);
            if (_tf == -1)
                throw new Exception("Error: " + p.get_errmsg());

            #region First_FitTextFlow
            do
            {
                p.begin_page_ext(0, 0, pagesize + " taborder=structure ");
                result = p.fit_textflow(_tf, llx, lly, urx, ury, "");
                //#region pageno
                //pagecounter++;
                //this.DrawFooter(ref p, ref pagecounter);
                //#endregion
                //p.end_page_ext("");

            } while (result == "_boxfull" || result == "_nextpage");

            /* Check for errors */
            if (result != "_stop")
            {
                /* "_boxempty" happens if the box is very small and doesn't
                 * hold any text at all.
                 */

                if (result == "_boxempty")
                    throw new Exception("Error: Textflow box too small");
                else
                {
                    /* Any other return value is a user exit caused by
                     * the "return" option; this requires dedicated code to
                     * deal with.
                     */

                    throw new Exception("User return '" + result +
                        "' found in Textflow");

                }
            }
            textendY = (int)_pdflib.info_textflow(_tf, "textendy");
            ury = textendY;
            ury += 10; //adjustment needed
            p.delete_textflow(_tf);
            _tf = -1;
            #endregion

            //tf_optlist = "charref fontname=Arial encoding=unicode fontsize=10";
            //text = "\n\tVisit the NCI Publications Locator Web site at http://www.cancer.gov/publications to\n\torder items 24 hours a day.";
            text = "<charref fontname=Arial encoding=unicode fontsize=10>\n\tVisit the NCI Publications Locator Web site at <underline=false fillcolor={#00006A} strokecolor={#00006A} matchbox={name=staticurl1 boxheight={fontsize descender}}>" +
                               "www.cancer.gov/publications<matchbox=end underline=false fillcolor={rgb 0 0 0} strokecolor={rgb 0 0 0}> " +
                               "to order\n\titems 24 hours a day.";
            _tf = p.create_textflow(text, "");

            #region Second_FitTextFlow
            do
            {
                //p.begin_page_ext(0, 0, pagesize + " taborder=structure ");
                result = p.fit_textflow(_tf, llx, lly, urx, ury, "");
                //#region pageno
                //pagecounter++;
                //this.DrawFooter(ref p, ref pagecounter);
                //#endregion
                //p.end_page_ext("");

            } while (result == "_boxfull" || result == "_nextpage");

            /* Check for errors */
            if (result != "_stop")
            {
                /* "_boxempty" happens if the box is very small and doesn't
                 * hold any text at all.
                 */

                if (result == "_boxempty")
                    throw new Exception("Error: Textflow box too small");
                else
                {
                    /* Any other return value is a user exit caused by
                     * the "return" option; this requires dedicated code to
                     * deal with.
                     */

                    throw new Exception("User return '" + result +
                        "' found in Textflow");

                }
            }
            textendY = (int)_pdflib.info_textflow(_tf, "textendy");
            ury = textendY;
            ury += 10; //adjustment needed
            p.delete_textflow(_tf);
            _tf = -1;
            #endregion

            /* Create URI action */
            string optlistLink = "url={" + "http://www.cancer.gov/publications" + "}";
            int action = p.create_action("URI", optlistLink);

            /* Create Link annotation on matchbox "kraxiweb" */
            optlistLink = "action={activate " + action +
                "} linewidth=0 usematchbox={staticurl1}";
            p.create_annotation(0, 0, 0, 0, "Link", optlistLink);

            /*--Continue creating next text flow--*/

            //_tf = p.add_textflow(_tf, text, tf_optlist);
            //if (_tf == -1)
            //    throw new Exception("Error: " + p.get_errmsg());

            tf_optlist = "charref fontname=Arial encoding=unicode fontsize=10 fontstyle=bold";
            text = "\n\n\tPhone";
            _tf = p.add_textflow(_tf, text, tf_optlist);
            if (_tf == -1)
                throw new Exception("Error: " + p.get_errmsg());

            tf_optlist = "charref fontname=Arial encoding=unicode fontsize=10";
            text = "\n\tCall 1-800-4-CANCER (1-800-422-6237) and select the option to order publications.\n\tOpen Monday through Friday, 8:00 a.m. to 8:00 p.m. Eastern time.";
            _tf = p.add_textflow(_tf, text, tf_optlist);
            if (_tf == -1)
                throw new Exception("Error: " + p.get_errmsg());

            tf_optlist = "charref fontname=Arial encoding=unicode fontsize=10 fontstyle=bold";
            text = "\n\n\tMail";
            _tf = p.add_textflow(_tf, text, tf_optlist);
            if (_tf == -1)
                throw new Exception("Error: " + p.get_errmsg());

            tf_optlist = "charref fontname=Arial encoding=unicode fontsize=10";
            //HITT 11987 text = "\n\tComplete the Publication Order Form and mail to:";
            text = "\n\tComplete the order form and mail to:";
            _tf = p.add_textflow(_tf, text, tf_optlist);
            if (_tf == -1)
                throw new Exception("Error: " + p.get_errmsg());

            tf_optlist = "charref fontname=Arial encoding=unicode fontsize=10";
            text = "\n\n\t\tNational Cancer Institute, NIH, DHHS\n\t\tPublications Ordering Service\n\t\tPost Office Box 24128\n\t\tBaltimore, MD 21227";
            _tf = p.add_textflow(_tf, text, tf_optlist);
            if (_tf == -1)
                throw new Exception("Error: " + p.get_errmsg());

            tf_optlist = "charref fontname=Arial encoding=unicode fontsize=10 fontstyle=bold";
            text = "\n\n\tFax";
            _tf = p.add_textflow(_tf, text, tf_optlist);
            if (_tf == -1)
                throw new Exception("Error: " + p.get_errmsg());

            tf_optlist = "charref fontname=Arial encoding=unicode fontsize=10";
            //HITT 11987 text = "\n\tFax your order form and payment information to the Publications Ordering Service at 301-330-7968.";
            text = "\n\tFax your order form and FedEx or UPS shipping number to 410-646-3117.\n\n";
            _tf = p.add_textflow(_tf, text, tf_optlist);
            if (_tf == -1)
                throw new Exception("Error: " + p.get_errmsg());

            //HITT 11987 Begin
            text = "International Orders\n\n";
            tf_optlist = "charref fontname=Times-Bold encoding=unicode fontsize=12 ";
            _tf = p.add_textflow(_tf, text, tf_optlist);
            if (_tf == -1)
                throw new Exception("Error: " + p.get_errmsg());
            //HITT 11987 End

            tf_optlist = "charref fontname=Arial encoding=unicode fontsize=10";
            //HITT 11987 text = "\n\nShipping to non-U.S. addresses: It is NCI policy to send up to five publications free to individuals or organizations outside of the United States. All foreign orders for six or more publications are charged actual shipping costs. Foreign orders are limited to 100 total copies, based on current inventory availability." +
            //HITT 11987     " To place an order to a\nnon-U.S. address, please contact the Publications Ordering Service by mail or fax.";
            //End Intl 2013 text = "If you live outside the United States, you may order up to five publications total (no bulk orders). There are no shipping " +
            //End Intl 2013       "charges for these international orders. Submit international orders via e-mail, fax, or mail:\n\n";
            text = "We no longer send hard copies of our publications to locations outside of the United States, but you can view and print most publications from our Cancer.gov Web site.\n\n";
            _tf = p.add_textflow(_tf, text, tf_optlist);
            if (_tf == -1)
                throw new Exception("Error: " + p.get_errmsg());

            #region Third_FitTextFlow
            do
            {
                //p.begin_page_ext(0, 0, pagesize + " taborder=structure ");
                result = p.fit_textflow(_tf, llx, lly, urx, ury, "");
                //#region pageno
                //pagecounter++;
                //this.DrawFooter(ref p, ref pagecounter);
                //#endregion
                //p.end_page_ext("");

            } while (result == "_boxfull" || result == "_nextpage");

            /* Check for errors */
            if (result != "_stop")
            {
                /* "_boxempty" happens if the box is very small and doesn't
                 * hold any text at all.
                 */

                if (result == "_boxempty")
                    throw new Exception("Error: Textflow box too small");
                else
                {
                    /* Any other return value is a user exit caused by
                     * the "return" option; this requires dedicated code to
                     * deal with.
                     */

                    throw new Exception("User return '" + result +
                        "' found in Textflow");

                }
            }
            textendY = (int)_pdflib.info_textflow(_tf, "textendy");
            ury = textendY;
            ury += 10; //Adjustment needed
            p.delete_textflow(_tf);
            _tf = -1;
            #endregion
            
            
            #region Commented_For_HITT_11987
            ///*
            // * For some reason, unless the following textflow is added there is difference 
            // * in the spacing between the last two lines of the previous textflow.
            // */
            //tf_optlist = "charref fontname=Arial encoding=unicode fontsize=10";
            //text = "\n";
            //_tf = p.add_textflow(_tf, text, tf_optlist);
            //if (_tf == -1)
            //    throw new Exception("Error: " + p.get_errmsg());

            //tf_optlist = "charref fontname=Times encoding=unicode fontsize=14 fontstyle=bold";
            //text = "\n\nPurchase Your Own NCI Materials (&quot;Riding On&quot; to a Government Order)";
            //_tf = p.add_textflow(_tf, text, tf_optlist);
            //if (_tf == -1)
            //    throw new Exception("Error: " + p.get_errmsg());

            //tf_optlist = "charref fontname=Arial encoding=unicode fontsize=10";
            //text = "\n\nOrganizations may purchase more copies of NCI materials by adding on to the Government's print order. Adding on to a Government print order is called a &quot;Ride-On&quot;. A Ride-On lets organizations purchase extra publications more economically because of lower Government printing costs. For more information about Ride-On printing, see the Ride-On printing fact sheet at http://ncipoet.cancer.gov/RideOnPrinting/RideOnPrintingFactSheet.pdf on the Internet or contact NCI's Office of Communications and Education by e-mail at ncipoetinfo@mail.nih.gov.";
            //_tf = p.add_textflow(_tf, text, tf_optlist);
            //if (_tf == -1)
            //    throw new Exception("Error: " + p.get_errmsg());
            #endregion


            //Begin HITT 11987
            //tf_optlist = "charref fontname=Arial encoding=unicode fontsize=10 fontstyle=bold";
            text = "<charref fontname=Arial encoding=unicode fontsize=10 fontstyle=bold>\tE-mail:  <underline=false fontname=Arial encoding=unicode fontsize=10 fontstyle=normal fillcolor={#00006A} strokecolor={#00006A} matchbox={name=staticurl2 boxheight={fontsize descender}}>" +
                   "ncioceocs@mail.nih.gov<matchbox=end underline=false fillcolor={rgb 0 0 0} strokecolor={rgb 0 0 0}>";
            text = "";  //***EAC Suppress the text above 
            _tf = p.create_textflow(text, "");
            if (_tf == -1)
                throw new Exception("Error: " + p.get_errmsg());

            #region Fourth_FitTextFlow
            do
            {
                //p.begin_page_ext(0, 0, pagesize + " taborder=structure ");
                result = p.fit_textflow(_tf, llx, lly, urx, ury, "");
                //#region pageno
                //pagecounter++;
                //this.DrawFooter(ref p, ref pagecounter);
                //#endregion
                //p.end_page_ext("");

            } while (result == "_boxfull" || result == "_nextpage");

            /* Check for errors */
            if (result != "_stop")
            {
                /* "_boxempty" happens if the box is very small and doesn't
                 * hold any text at all.
                 */

                if (result == "_boxempty")
                    throw new Exception("Error: Textflow box too small");
                else
                {
                    /* Any other return value is a user exit caused by
                     * the "return" option; this requires dedicated code to
                     * deal with.
                     */

                    throw new Exception("User return '" + result +
                        "' found in Textflow");

                }
            }
            textendY = (int)_pdflib.info_textflow(_tf, "textendy");
            ury = textendY;
            //ury += 10;
            p.delete_textflow(_tf);
            _tf = -1;
            #endregion

            /* Create URI action */
            optlistLink = "url={" + "mailto:ncioceocs@mail.nih.gov" + "}";
            action = p.create_action("URI", optlistLink);

            /* Create Link annotation on matchbox "kraxiweb" */
            optlistLink = "action={activate " + action +
                "} linewidth=0 usematchbox={staticurl2}";
            p.create_annotation(0, 0, 0, 0, "Link", optlistLink);

            /* Continue with the remaining text flows */

            tf_optlist = "charref fontname=Arial encoding=unicode fontsize=10 fontstyle=bold";
            text = "\n\tFax:";
            text = "";  //***EAC Suppress the text above 
            _tf = p.add_textflow(_tf, text, tf_optlist);
            if (_tf == -1)
                throw new Exception("Error: " + p.get_errmsg());

            tf_optlist = "charref fontname=Arial encoding=unicode fontsize=10";
            text = "      410-646-3117";
            text = "";  //***EAC Suppress the text above 
            _tf = p.add_textflow(_tf, text, tf_optlist);
            if (_tf == -1)
                throw new Exception("Error: " + p.get_errmsg());

            tf_optlist = "charref fontname=Arial encoding=unicode fontsize=10 fontstyle=bold";
            text = "\n\n\tMail:";
            text = "";  //***EAC Suppress the text above 
            _tf = p.add_textflow(_tf, text, tf_optlist);
            if (_tf == -1)
                throw new Exception("Error: " + p.get_errmsg());

            tf_optlist = "charref fontname=Arial encoding=unicode fontsize=10";            
            text = "      National Cancer Institute, NIH, DHHS\n\t\tPublications Ordering Service\n\t\tPost Office Box 24128\n\t\tBaltimore, MD 21227";
            text = "";  //***EAC Suppress the text above 
            _tf = p.add_textflow(_tf, text, tf_optlist);
            if (_tf == -1)
                throw new Exception("Error: " + p.get_errmsg());
            //End HITT 11987

            /* Loop until all index entries are placed; create new pages as long as
            * more index entries need to be placed
            */
            #region Final_FitTextFlow
            do
            {
                //p.begin_page_ext(0, 0, pagesize + " taborder=structure ");
                result = p.fit_textflow(_tf, llx, lly, urx, ury, "");
                #region pageno
                pagecounter++;
                this.DrawFooter(ref p, ref pagecounter);
                #endregion
                p.end_page_ext("");
                
            } while (result == "_boxfull" || result == "_nextpage");

            /* Check for errors */
            if (result != "_stop")
            {
                /* "_boxempty" happens if the box is very small and doesn't
                 * hold any text at all.
                 */

                if (result == "_boxempty")
                    throw new Exception("Error: Textflow box too small");
                else
                {
                    /* Any other return value is a user exit caused by
                     * the "return" option; this requires dedicated code to
                     * deal with.
                     */

                    throw new Exception("User return '" + result +
                        "' found in Textflow");

                }
            }
            p.delete_textflow(_tf);
            #endregion

            arrlstTOC.Add("HOW TO PLACE AN ORDER"); //For TOC
            arrlstTOCPageNum.Add(pagecounter);
            if (createmode == 2)
            {
                #region PLACEORDER_Bookmark
                string bookmarkdest = "destinationPlaceOrder"; //define a name for the named destination - no space allowed
                p.add_nameddest(bookmarkdest, "type=fixed left=" + 0 + " top=" + 800); //add the named destination
                int bookmarkaction = p.create_action("GoTo", " destname=" + bookmarkdest);
                p.create_bookmark("How to Place an Order", " action={activate=" + bookmarkaction + "}");
                #endregion
            }
            #endregion

            #region FirstPage_Extended
            text = ""; result = "";
            llx = 60; lly = 50; urx = 570; ury = 745;
            _tf = -1;

            text = "<charref fontname=Times-Bold encoding=unicode fontsize=14 alignment=center>Print Your Own NCI Materials\n\n" +
                    "<fontname=Arial encoding=unicode fontsize=10 fontstyle=normal alignment=left>If you wish to obtain more copies of NCI materials, you have several options, described below. More " +
                    "information about each option can be found on the NCI Publications Locator Self-Printing Options Web page at " + 
                    "<underline=false fontname=Arial encoding=unicode fontsize=10 fontstyle=normal fillcolor={#00006A} strokecolor={#00006A} matchbox={name=staticurl3 boxheight={fontsize descender}}>" +
           //         "https://cissecure.nci.nih.gov/ncipubs/nciplselfprint.aspx<matchbox=end underline=false fillcolor={rgb 0 0 0} strokecolor={rgb 0 0 0}>.";
                      "https://pubs.cancer.gov/ncipl/nciplselfprint.aspx<matchbox=end underline=false fillcolor={rgb 0 0 0} strokecolor={rgb 0 0 0}>.";
            _tf = p.create_textflow(text, "");
            if (_tf == -1)
                throw new Exception("Error: " + p.get_errmsg());

            #region First_FitTextFlow
            do
            {
                p.begin_page_ext(0, 0, pagesize + " taborder=structure ");
                result = p.fit_textflow(_tf, llx, lly, urx, ury, "");
                //#region pageno
                //pagecounter++;
                //this.DrawFooter(ref p, ref pagecounter);
                //#endregion
                //p.end_page_ext("");

            } while (result == "_boxfull" || result == "_nextpage");

            /* Check for errors */
            if (result != "_stop")
            {
                /* "_boxempty" happens if the box is very small and doesn't
                 * hold any text at all.
                 */

                if (result == "_boxempty")
                    throw new Exception("Error: Textflow box too small");
                else
                {
                    /* Any other return value is a user exit caused by
                     * the "return" option; this requires dedicated code to
                     * deal with.
                     */

                    throw new Exception("User return '" + result +
                        "' found in Textflow");

                }
            }
            textendY = (int)_pdflib.info_textflow(_tf, "textendy");
            ury = textendY;
            ury -= 10; //adjustment needed
            p.delete_textflow(_tf);
            _tf = -1;
            #endregion

            /* Create URI action */
           // optlistLink = "url={" + "https://cissecure.nci.nih.gov/ncipubs/nciplselfprint.aspx" + "}";
            optlistLink = "url={" + "https://pubs.cancer.gov/ncipl/nciplselfprint.aspx" + "}";
            action = p.create_action("URI", optlistLink);

            /* Create Link annotation on matchbox "kraxiweb" */
            optlistLink = "action={activate " + action +
                "} linewidth=0 usematchbox={staticurl3}";
            p.create_annotation(0, 0, 0, 0, "Link", optlistLink);

            /* Continue with the remaining text flows */
            text = "<charref fontname=Arial encoding=unicode fontsize=10 fontstyle=bold>Self-Printing: " +
                    "<fontname=Arial encoding=unicode fontsize=10 fontstyle=normal>Print individual copies of a publication by visiting the NCI Publications Locator at " +
                    "<underline=false fontname=Arial encoding=unicode fontsize=10 fontstyle=normal fillcolor={#00006A} strokecolor={#00006A} matchbox={name=staticurl4 boxheight={fontsize descender}}>" +
                    "www.cancer.gov/publications<matchbox=end underline=false fillcolor={rgb 0 0 0} strokecolor={rgb 0 0 0}>" +
                    ", searching for the publication, and choosing the Read as Web page option in your search results. Most " +
                    "publications can be printed using the View/Print PDF option in Page Options.";
                    
            _tf = p.create_textflow(text, "");

            #region Fit_MiddleTextFlow
            do
            { result = p.fit_textflow(_tf, llx, lly, urx, ury, ""); } 
            while (result == "_boxfull" || result == "_nextpage");
            /* Check for errors */
            if (result != "_stop")
            {
                if (result == "_boxempty")
                    throw new Exception("Error: Textflow box too small");
                else
                {
                    throw new Exception("User return '" + result +
                        "' found in Textflow");
                }
            }
            textendY = (int)_pdflib.info_textflow(_tf, "textendy");
            ury = textendY;
            ury -= 10; //adjustment needed
            p.delete_textflow(_tf);
            _tf = -1;
            #endregion

            /* Create URI action */
            optlistLink = "url={" + "http://www.cancer.gov/publications" + "}";
            action = p.create_action("URI", optlistLink);

            /* Create Link annotation on matchbox "kraxiweb" */
            optlistLink = "action={activate " + action +
                "} linewidth=0 usematchbox={staticurl4}";
            p.create_annotation(0, 0, 0, 0, "Link", optlistLink);

            /* Continue with the remaining text flows */
            text = "<charref fontname=Arial encoding=unicode fontsize=10 fontstyle=bold>Contents & Covers: " +
                    "<fontname=Arial encoding=unicode fontsize=10 fontstyle=normal>Print the contents of a publication, then either order professionally printed covers or print " +
                    "color covers and assemble the publication when needed. For more information and to see which publications are " +
                    "currently available in this format, visit the NCI Publications Locator at " +
                    "<underline=false fontname=Arial encoding=unicode fontsize=10 fontstyle=normal fillcolor={#00006A} strokecolor={#00006A} matchbox={name=staticurl5 boxheight={fontsize descender}}>" +
                    "www.cancer.gov/publications<matchbox=end underline=false fillcolor={rgb 0 0 0} strokecolor={rgb 0 0 0}>" +
                    " and look for Contents & Covers in the Collections section.";

            _tf = p.create_textflow(text, "");

            #region Fit_MiddleTextFlow
            do
            { result = p.fit_textflow(_tf, llx, lly, urx, ury, ""); }
            while (result == "_boxfull" || result == "_nextpage");
            /* Check for errors */
            if (result != "_stop")
            {
                if (result == "_boxempty")
                    throw new Exception("Error: Textflow box too small");
                else
                {
                    throw new Exception("User return '" + result +
                        "' found in Textflow");
                }
            }
            textendY = (int)_pdflib.info_textflow(_tf, "textendy");
            ury = textendY;
            ury -= 10; //adjustment needed
            p.delete_textflow(_tf);
            _tf = -1;
            #endregion

            /* Create URI action */
            optlistLink = "url={" + "http://www.cancer.gov/publications" + "}";
            action = p.create_action("URI", optlistLink);

            /* Create Link annotation on matchbox "kraxiweb" */
            optlistLink = "action={activate " + action +
                "} linewidth=0 usematchbox={staticurl5}";
            p.create_annotation(0, 0, 0, 0, "Link", optlistLink);

            /* Continue with the remaining text flows */
            text = "<charref fontname=Arial encoding=unicode fontsize=10 fontstyle=bold>Print Files: " +
                    "<fontname=Arial encoding=unicode fontsize=10 fontstyle=normal>To have a print shop in your area professionally print copies of NCI publications, you can request the print files for most of our publications by contacting us at " +
                    "<underline=false fontname=Arial encoding=unicode fontsize=10 fontstyle=normal fillcolor={#00006A} strokecolor={#00006A} matchbox={name=staticurl7 boxheight={fontsize descender}}>" +
                    "ncipoetinfo@mail.nih.gov<matchbox=end underline=false fillcolor={rgb 0 0 0} strokecolor={rgb 0 0 0}>." +
                    "\n\n<charref fontname=Arial encoding=unicode fontsize=10 fontstyle=bold>Ride-On: " +
                    "<fontname=Arial encoding=unicode fontsize=10 fontstyle=normal>You may purchase more copies of NCI materials by adding on to the government’s print order, which " +
                    "is called a \"Ride-On.\" A Ride-On allows requesters to purchase extra copies of publications more economically because " +
                    "of lower government printing costs. For more information about Ride-On printing, see the " +
                    "<fontname=Arial encoding=unicode fontsize=10 fontstyle=italic>Ride-On Printing: Add to NCI Print Orders " +
                    "<fontname=Arial encoding=unicode fontsize=10 fontstyle=normal>fact sheet at " +
                    "<underline=false fontname=Arial encoding=unicode fontsize=10 fontstyle=normal fillcolor={#00006A} strokecolor={#00006A} matchbox={name=staticurl6 boxheight={fontsize descender}}>" +
                   // "https://cissecure.nci.nih.gov/ncipubs/RideOnPrintingFactSheet.pdf<matchbox=end underline=false fillcolor={rgb 0 0 0} strokecolor={rgb 0 0 0}>" +
                     "https://pubs.cancer.gov/ncipl/RideOnPrintingFactSheet.pdf<matchbox=end underline=false fillcolor={rgb 0 0 0} strokecolor={rgb 0 0 0}>" +
                    " or contact us at " +
                    "<underline=false fontname=Arial encoding=unicode fontsize=10 fontstyle=normal fillcolor={#00006A} strokecolor={#00006A} matchbox={name=staticurl7 boxheight={fontsize descender}}>" +
                    "ncipoetinfo@mail.nih.gov<matchbox=end underline=false fillcolor={rgb 0 0 0} strokecolor={rgb 0 0 0}>.";

            _tf = p.create_textflow(text, "");

            /* Loop until all index entries are placed; create new pages as long as
            * more index entries need to be placed
            */
            #region Final_FitTextFlow
            do
            {
                //p.begin_page_ext(0, 0, pagesize + " taborder=structure ");
                result = p.fit_textflow(_tf, llx, lly, urx, ury, "");
                #region pageno
                pagecounter++;
                this.DrawFooter(ref p, ref pagecounter);
                #endregion
                //p.end_page_ext(""); //Has to be moved after the URL action

            } while (result == "_boxfull" || result == "_nextpage");

            /* Check for errors */
            if (result != "_stop")
            {
                /* "_boxempty" happens if the box is very small and doesn't
                 * hold any text at all.
                 */

                if (result == "_boxempty")
                    throw new Exception("Error: Textflow box too small");
                else
                {
                    /* Any other return value is a user exit caused by
                     * the "return" option; this requires dedicated code to
                     * deal with.
                     */

                    throw new Exception("User return '" + result +
                        "' found in Textflow");

                }
            }
            p.delete_textflow(_tf);
            #endregion

            /* Create URI action */
           // optlistLink = "url={" + "https://cissecure.nci.nih.gov/ncipubs/RideOnPrintingFactSheet.pdf" + "}";
            optlistLink = "url={" + "https://pubs.cancer.gov/ncipl/RideOnPrintingFactSheet.pdf" + "}";
            action = p.create_action("URI", optlistLink);

            /* Create Link annotation on matchbox "kraxiweb" */
            optlistLink = "action={activate " + action +
                "} linewidth=0 usematchbox={staticurl6}";
            p.create_annotation(0, 0, 0, 0, "Link", optlistLink);

            /* Create next URI action */
            optlistLink = "url={" + "mailto:ncipoetinfo@mail.nih.gov" + "}";
            action = p.create_action("URI", optlistLink);

            /* Create Link annotation on matchbox "kraxiweb" */
            optlistLink = "action={activate " + action +
                "} linewidth=0 usematchbox={staticurl7}";
            p.create_annotation(0, 0, 0, 0, "Link", optlistLink);

            p.end_page_ext(""); //End Page here because the links need to be activated before closing the page

            arrlstTOC.Add("PRINT YOUR OWN NCI MATERIALS"); //For TOC
            arrlstTOCPageNum.Add(pagecounter);
            if (createmode == 2)
            {
                #region PLACEORDER_Bookmark
                string bookmarkdest = "destinationPrintYourOwn"; //define a name for the named destination - no space allowed
                p.add_nameddest(bookmarkdest, "type=fixed left=" + 0 + " top=" + 800); //add the named destination
                int bookmarkaction = p.create_action("GoTo", " destname=" + bookmarkdest);
                p.create_bookmark("Print Your Own NCI Materials", " action={activate=" + bookmarkaction + "}");
                #endregion
            }

            
            #endregion

            #region SecondPage
            arrlstTOC.Add("ORDER FORM"); //For TOC
            arrlstTOCPageNum.Add(pagecounter + 1);

            llx = 60; lly = 50; urx = 570; ury = 745; //Reset position variables
            _tf = -1; //Reset textflow pointer

            //HITT 11987 tf_optlist = "charref fontname=Helvetica-Bold encoding=unicode fontsize=14 alignment=center";
            tf_optlist = "charref fontname=Times-Bold encoding=unicode fontsize=14 alignment=center";
            //HITT 11987 text = "PUBLICATIONS ORDER FORM";
            text = "Order Form";
            _tf = p.add_textflow(_tf, text, tf_optlist);
            if (_tf == -1)
                throw new Exception("Error: " + p.get_errmsg());

            p.begin_page_ext(0, 0, pagesize + " taborder=structure ");
            //HITT 11987 result = p.fit_textflow(_tf, 200, lly, urx, ury, "");
            result = p.fit_textflow(_tf, llx, lly, urx, ury, "");
            p.delete_textflow(_tf);

            _tf = -1;

            //tf_optlist = "charref fontname=Arial encoding=unicode fontsize=10 ";
            //HITT 11987 text = "\n\n\nThe first 20 copies of the entire order will be sent free of shipping and handling charges. Orders exceeding 20 total copies are considered bulk orders and will be charged a shipping and handling fee. All orders must be prepaid.";
            text = "\n\n\nList the inventory number, title, and quantity of each item in the spaces provided below. " +
                    "(If you run out of room, attach another piece of paper with the remainder of your order.) " +
                    "On the next page, fill out your shipping and, if ordering more than 20 copies total, billing information.";
            //_tf = p.add_textflow(_tf, text, tf_optlist);
            //if (_tf == -1)
            //    throw new Exception("Error: " + p.get_errmsg());

            tf_optlist = "charref fontname=Arial encoding=unicode fontsize=10 alignment=left";
            //text = "\n\nFree and bulk orders can be placed on the NCI Publications Locator Web site at\nhttp://www.cancer.gov/publications. Or, list the inventory number, title, and quantity of each item in the spaces provided below. If you run out of room, just attach another piece of paper with the remainder of your order. To ensure that materials are available to all requesters, NCI has set limits on the maximum number of copies that may be ordered by any one requester each month on a title-by-title basis. If you are requesting a large quantity of a title, please verify availability by calling 1–800–4–CANCER (1–800–422–6237) and selecting the option to order publications.";
            //HITT 11987 text = text + "\n\nFree and bulk orders can be placed on the NCI Publications Locator Web site at\n<underline=false fillcolor={rgb 0 0 0} strokecolor={rgb 0 0 0} matchbox={name=cancergovpub boxheight={fontsize descender}}>http://www.cancer.gov/publications<matchbox=end underline=false fillcolor={rgb 0 0 0} strokecolor={rgb 0 0 0}>. Or, list the inventory number, title, and quantity of each item in the spaces provided below. If you run out of room, just attach another piece of paper with the remainder of your order. To ensure that materials are available to all requesters, NCI has set limits on the maximum number of copies that may be ordered by any one requester each month on a title-by-title basis. If you are requesting a large quantity of a title, please verify availability by calling 1–800–4–CANCER (1–800–422–6237) and selecting the option to order publications.";
            text = text + "\n\nTo ensure publications are available to all requesters, NCI may set limits on the maximum number " +
                            "of copies of a particular title that may be ordered by any one requester each month. " +
                            "For information on current availability, visit <underline=false fillcolor={#00006A} strokecolor={#00006A} matchbox={name=cancergovpub boxheight={fontsize descender}}>www.cancer.gov/publications<matchbox=end underline=false fillcolor={rgb 0 0 0} strokecolor={rgb 0 0 0}> or call 1–800–4–CANCER (1–800–422–6237).";            
            //_tf = p.add_textflow(_tf, text, tf_optlist);
            _tf = p.create_textflow(text, tf_optlist);
            if (_tf == -1)
                throw new Exception("Error: " + p.get_errmsg());

            /* Loop until all index entries are placed; create new pages as long as
            * more index entries need to be placed
            */
            do
            {
                //result = p.fit_textflow(_tf, llx, lly, urx, ury, "");
                result = p.fit_textflow(_tf, llx, lly, urx, ury, "fitmethod=auto");
                #region pageno
                    pagecounter++;
                    this.DrawFooter(ref p, ref pagecounter);
                #endregion

            } while (result == "_boxfull" || result == "_nextpage");

            /* Check for errors */
            if (result != "_stop")
            {
                /* "_boxempty" happens if the box is very small and doesn't
                 * hold any text at all.
                 */

                if (result == "_boxempty")
                    throw new Exception("Error: Textflow box too small");
                else
                {
                    /* Any other return value is a user exit caused by
                     * the "return" option; this requires dedicated code to
                     * deal with.
                     */

                    throw new Exception("User return '" + result +
                        "' found in Textflow");

                }
            }
            textendY = (int)p.info_textflow(_tf, "textendy");
            p.delete_textflow(_tf);

            //Create URI action
            string urloptlist = "url={http://www.cancer.gov/publications}";
            int urlact = p.create_action("URI", urloptlist);
            //Create Link annotation on matchbox "cancergovpub"
            urloptlist = "action={activate " + urlact + "} linewidth=0 usematchbox={cancergovpub}";
            p.create_annotation(0, 0, 0, 0, "Link", urloptlist);

            llx = 60; lly = 50; urx = 570; ury = textendY-10;

            #region StaticTable
            

            ///Begin Table
            int _id_table, _id_thead, _id_row, _id_th, _id_body, _id_td, curr_row, curr_col;
            _id_table = p.begin_item("Table", "Title={Order Form Table}"); //Begin Table
            _id_thead = p.begin_item("THead", "Title={Header}"); //begin header
            _id_row = p.begin_item("TR", "Title={Header Row}"); //header row

            int font = p.load_font("Arial", "unicode", "fontstyle=bold"); //Set a font type
            if (font == -1)
                throw new Exception("Error: " + p.get_errmsg());
            
            string optlistColl = "fittextline={position={ center bottom } font=" + font + " fontsize=10 fillcolor={gray 0}} ";
            curr_col = 1; curr_row = 1; _tbl = -1;

            _id_th = _pdflib.begin_item("TH", "Scope=Column");
            //_id_th = p.begin_item("TH", "");
            _tbl = p.add_table_cell(_tbl, curr_col, curr_row, "Inventory #", optlistColl + " colwidth=30% margin=4 rowheight=20");
            if (_tbl == -1)
                throw new Exception("Error: " + p.get_errmsg());
            p.end_item(_id_th);

            curr_col++; curr_row = 1;

            _id_th = _pdflib.begin_item("TH", "Scope=Column");
            //_id_th = p.begin_item("TH", "");
            _tbl = p.add_table_cell(_tbl, curr_col, curr_row, "Title", optlistColl + " colwidth=50% margin=4 rowheight=20");
            if (_tbl == -1)
                throw new Exception("Error: " + p.get_errmsg());
            p.end_item(_id_th);

            curr_col++; curr_row = 1;

            _id_th = _pdflib.begin_item("TH", "Scope=Column");
            //_id_th = p.begin_item("TH", "");
            _tbl = p.add_table_cell(_tbl, curr_col, curr_row, "Quantity", optlistColl + " colwidth=20% margin=4 rowheight=20");
            if (_tbl == -1)
                throw new Exception("Error: " + p.get_errmsg());
            p.end_item(_id_th);

            p.end_item(_id_row); //End header row
            p.end_item(_id_thead); //End Header
            _id_body = p.begin_item("TBody", "Title={Body}"); //Begin Body

            //HITT 11987 for (int i = 0; i < 15; i++)
            for (int i = 0; i < 20; i++)
            {
                _id_row = p.begin_item("TR", "Title={Body Row}"); //body row
                
                curr_row++;

                _tf = -1;
                curr_col = 1;
                tf_optlist = "fontname=Arial encoding=unicode fontsize=10 fontstyle=bold";
                text = "";
                _tf = p.add_textflow(_tf, text, tf_optlist);
                if (_tf == -1)
                    throw new Exception("Error: " + p.get_errmsg());

                optlistColl = " textflow=" + _tf;
                _id_td = p.begin_item("TD", "");
                _tbl = p.add_table_cell(_tbl, curr_col, curr_row, "", optlistColl + " margintop=1 rowheight=20");
                if (_tbl == -1)
                    throw new Exception("Error: " + p.get_errmsg());
                p.end_item(_id_td);

                _tf = -1;
                curr_col = 2;
                //tf_optlist = "fontname=Helvetica encoding=unicode fontsize=9 ";
                text = "";
                _tf = p.add_textflow(_tf, text, tf_optlist);
                if (_tf == -1)
                    throw new Exception("Error: " + p.get_errmsg());

                optlistColl = " textflow=" + _tf;
                _id_td = p.begin_item("TD", "");
                _tbl = p.add_table_cell(_tbl, curr_col, curr_row, "", optlistColl + " margintop=1 rowheight=20");
                if (_tbl == -1)
                    throw new Exception("Error: " + p.get_errmsg());
                p.end_item(_id_td);

                _tf = -1;
                curr_col = 3;
                //tf_optlist = "fontname=Helvetica encoding=unicode fontsize=9 ";
                text = "";
                _tf = p.add_textflow(_tf, text, tf_optlist);
                if (_tf == -1)
                    throw new Exception("Error: " + p.get_errmsg());

                optlistColl = " textflow=" + _tf;
                _id_td = p.begin_item("TD", "");
                _tbl = p.add_table_cell(_tbl, curr_col, curr_row, "", optlistColl + " margintop=1 rowheight=20");
                if (_tbl == -1)
                    throw new Exception("Error: " + p.get_errmsg());
                p.end_item(_id_td);

                p.end_item(_id_row); //End body row

            }

            //Last Row
            _id_row = p.begin_item("TR", "Title={Body Row}"); //body row
            
            curr_row++;

            _tf = -1;
            curr_col = 1;
            text = "Total Number of Copies:  ";
            tf_optlist = "fontname=Arial encoding=unicode fontsize=10 fontstyle=bold alignment=right";
            _tf = p.add_textflow(_tf, text, tf_optlist);
            if (_tf == -1)
                throw new Exception("Error: " + p.get_errmsg());

            optlistColl = " textflow=" + _tf;
            _id_td = p.begin_item("TD", "");
            _tbl = p.add_table_cell(_tbl, curr_col, curr_row, "", optlistColl + " margintop=1 colspan=2 rowheight=20");
            if (_tbl == -1)
                throw new Exception("Error: " + p.get_errmsg());
            p.end_item(_id_td);

            curr_col=3;
            //optlistColl = "fittextline={position={ center bottom } font=" + font + " fontsize=10 fontstyle=bold fillcolor={gray 0}} ";
            _id_td = p.begin_item("TD", "");
            _tbl = p.add_table_cell(_tbl, curr_col, curr_row, "", "margintop=1 rowheight=20");
            if (_tbl == -1)
                throw new Exception("Error: " + p.get_errmsg());
            p.end_item(_id_td);

            p.end_item(_id_row); //End body row
            p.end_item(_id_body); //End Body
            p.end_item(_id_table); //End Table
            ///End Table


            ///*Begin Place the Table*/
            /* ---------- Place the table on one or more pages ---------- */

            /*
             * Loop until all of the table is placed; create new pages
             * as long as more table instances need to be placed.
             */
            do
            {
                /*Begin available height check*/
                boolNewPage = false; //new page flag
                if (ury <= MinHeight) //Min height is a pre-defined value
                {
                    boolNewPage = true; //set to true if min height condition is satisfied
                    #region new_page
                    this.DrawFooter(ref p, ref pagecounter);

                    p.end_page_ext(""); //end current page

                    pagecounter++;
                    p.begin_page_ext(0, 0, pagesize + " taborder=structure "); //Begin a new page
                    llx = 60; lly = 50; urx = 570; ury = 745; //Reset Position Variables
                    #endregion
                    continue; //proceed to next iteration
                }
                /*End avaliable height check*/

                string optlist = "stroke={{line=frame linewidth=0.8} {line=other linewidth=0.3}}";
                        //"stroke={line=hor1}";

                /* Place the table instance */
                result = p.fit_table(_tbl, llx, lly, urx, ury, optlist);

                if (result == "_boxfull")
                {
                    #region new_page
                    this.DrawFooter(ref p, ref pagecounter);

                    p.end_page_ext(""); //end current page

                    pagecounter++;
                    p.begin_page_ext(0, 0, pagesize + " taborder=structure "); //Begin a new page
                    llx = 60; lly = 50; urx = 570; ury = 745; //Reset Position Variables
                    #endregion
                }

            } while (result == "_boxfull" || boolNewPage);  //while (result == "_boxfull") ;

            /* Check the result; "_stop" means all is ok. */
            if (result != "_stop")
            {
                if (result == "_error")
                {
                    throw new Exception("Error when placing table: " +
                            p.get_errmsg());
                }
                else
                {
                    /* Any other return value is a user exit caused by
                     * the "return" option; this requires dedicated code to
                     * deal with.
                     */
                    throw new Exception("User return found in Textflow");
                }
            }

            /* This will also delete Textflow handles used in the table */
            textendY = (int)p.info_table(_tbl, "height");
            ury = ury - textendY - Yoffset; //fsize * 1.2; //Adjust to fit remaining text
            p.delete_table(_tbl, "");

            ///*End Place the Table*/
            #endregion

            llx = 60; lly = 50; urx = 570; _tf = -1;

            #region Commented_For_HITT_11987
            //tf_optlist = "charref fontname=Helvetica-Bold encoding=unicode fontsize=12 fillcolor={#00006A} ";
            //text = "CALCULATE COSTS";
            //_tf = p.add_textflow(_tf, text, tf_optlist);
            //if (_tf == -1)
            //    throw new Exception("Error: " + p.get_errmsg());

            //tf_optlist = "charref fontname=Arial encoding=unicode fontsize=10 fillcolor={gray 0} ";
            //text = "\n\nIf the total number of copies is between:";
            //_tf = p.add_textflow(_tf, text, tf_optlist);
            //if (_tf == -1)
            //    throw new Exception("Error: " + p.get_errmsg());

            ///* Loop until all index entries are placed; create new pages as long as
            //* more index entries need to be placed
            //*/
            //do
            //{
            //    /*Begin available height check*/
            //    boolNewPage = false; //new page flag
            //    if (ury <= MinHeight) //Min height is a pre-defined value
            //    {
            //        boolNewPage = true; //set to true if min height condition is satisfied
            //        #region new_page
            //        this.DrawFooter(ref p, ref pagecounter);

            //        p.end_page_ext(""); //end current page

            //        pagecounter++;
            //        p.begin_page_ext(0, 0, pagesize + " taborder=structure "); //Begin a new page
            //        llx = 60; lly = 50; urx = 570; ury = 745; //Reset Position Variables
            //        #endregion
            //        continue; //proceed to next iteration
            //    }
            //    /*End avaliable height check*/


            //    //p.begin_page_ext(0, 0, pagesize);
            //    result = p.fit_textflow(_tf, llx, lly, urx, ury, "");

            //    if (result == "_boxfull")
            //    {
            //        #region new_page
            //        this.DrawFooter(ref p, ref pagecounter);

            //        p.end_page_ext(""); //end current page

            //        pagecounter++;
            //        p.begin_page_ext(0, 0, pagesize + " taborder=structure "); //Begin a new page
            //        llx = 60; lly = 50; urx = 570; ury = 745; //Reset Position Variables
            //        #endregion
            //    }
            //    //p.end_page_ext("");

            //} while (result == "_boxfull" || result == "_nextpage");

            ///* Check for errors */
            //if (result != "_stop")
            //{
            //    /* "_boxempty" happens if the box is very small and doesn't
            //     * hold any text at all.
            //     */

            //    if (result == "_boxempty")
            //        throw new Exception("Error: Textflow box too small");
            //    else
            //    {
            //        /* Any other return value is a user exit caused by
            //         * the "return" option; this requires dedicated code to
            //         * deal with.
            //         */

            //        throw new Exception("User return '" + result +
            //            "' found in Textflow");

            //    }
            //}
            //textendY = (int)p.info_textflow(_tf, "textendy");
            //p.delete_textflow(_tf);
            //ury = textendY - 10; //ury - textendY - Yoffset;

            //#region StaticTable


            /////Begin Table
            ////int _id_table, _id_thead, _id_row, _id_th, _id_body, _id_td, curr_row, curr_col;
            //_id_table = p.begin_item("Table", "Title={Order Cost Table}"); //Begin Table
            //_id_thead = p.begin_item("THead", "Title={Header}"); //begin header
            //_id_row = p.begin_item("TR", "Title={Header Row}"); //header row

            //font = p.load_font("Arial", "unicode", "fontstyle=bold"); //Set a font type
            //if (font == -1)
            //    throw new Exception("Error: " + p.get_errmsg());

            //optlistColl = "fittextline={position={ center bottom } font=" + font + " fontsize=10 } ";
            //curr_col = 1; curr_row = 1; _tbl = -1;

            //_id_th = _pdflib.begin_item("TH", "Scope=Column");
            ////_id_th = p.begin_item("TH", "");
            //_tbl = p.add_table_cell(_tbl, curr_col, curr_row, "Number of Copies", optlistColl + " colwidth=30% margin=4 rowheight=20");
            //if (_tbl == -1)
            //    throw new Exception("Error: " + p.get_errmsg());
            //p.end_item(_id_th);

            //curr_col++; curr_row = 1;

            //_id_th = _pdflib.begin_item("TH", "Scope=Column");
            ////_id_th = p.begin_item("TH", "");
            //_tbl = p.add_table_cell(_tbl, curr_col, curr_row, "Cost Calculation", optlistColl + " colwidth=50% margin=4 rowheight=20");
            //if (_tbl == -1)
            //    throw new Exception("Error: " + p.get_errmsg());
            //p.end_item(_id_th);

            //curr_col++; curr_row = 1;

            //_id_th = _pdflib.begin_item("TH", "Scope=Column");
            ////_id_th = p.begin_item("TH", "");
            //_tbl = p.add_table_cell(_tbl, curr_col, curr_row, "Amount Due", optlistColl + " colwidth=20% margin=4 rowheight=20");
            //if (_tbl == -1)
            //    throw new Exception("Error: " + p.get_errmsg());
            //p.end_item(_id_th);

            //p.end_item(_id_row); //End header row
            //p.end_item(_id_thead); //End Header
            //_id_body = p.begin_item("TBody", "Title={Body}"); //Begin Body

            //for (int i = 0; i < 3; i++)
            //{
            //    _id_row = p.begin_item("TR", "Title={Body Row}"); //body row

            //    curr_row++;

            //    _tf = -1;
            //    curr_col = 1;
            //    tf_optlist = "fontname=Arial encoding=unicode fontsize=10 alignment=center";
            //    if (i == 0)
            //        text = "1-20";
            //    else if (i == 1)
            //        text = "21-75";
            //    else if (i == 2)
            //        text = "76 +";
            //    _tf = p.add_textflow(_tf, text, tf_optlist);
            //    if (_tf == -1)
            //        throw new Exception("Error: " + p.get_errmsg());

            //    optlistColl = " textflow=" + _tf;
            //    _id_td = p.begin_item("TD", "");
            //    _tbl = p.add_table_cell(_tbl, curr_col, curr_row, "", optlistColl + " margintop=1 rowheight=20");
            //    if (_tbl == -1)
            //        throw new Exception("Error: " + p.get_errmsg());
            //    p.end_item(_id_td);

            //    _tf = -1;
            //    curr_col = 2;
            //    //tf_optlist = "fontname=Helvetica encoding=unicode fontsize=9 ";
            //    if (i == 0)
            //        text = "FREE";
            //    else if (i == 1)
            //        text = "$8.00";
            //    else if (i == 2)
            //        text = "(number of copies - 20) x $0.15";
            //    _tf = p.add_textflow(_tf, text, tf_optlist);
            //    if (_tf == -1)
            //        throw new Exception("Error: " + p.get_errmsg());

            //    optlistColl = " textflow=" + _tf;
            //    _id_td = p.begin_item("TD", "");
            //    _tbl = p.add_table_cell(_tbl, curr_col, curr_row, "", optlistColl + " margintop=1 rowheight=20");
            //    if (_tbl == -1)
            //        throw new Exception("Error: " + p.get_errmsg());
            //    p.end_item(_id_td);

            //    _tf = -1;
            //    curr_col = 3;
            //    //tf_optlist = "fontname=Helvetica encoding=unicode fontsize=9 ";
            //    if (i == 0)
            //        text = "FREE";
            //    else if (i == 1)
            //        text = "$8.00";
            //    else if (i == 2)
            //        text = "";
            //    _tf = p.add_textflow(_tf, text, tf_optlist);
            //    if (_tf == -1)
            //        throw new Exception("Error: " + p.get_errmsg());

            //    optlistColl = " textflow=" + _tf;
            //    _id_td = p.begin_item("TD", "");
            //    _tbl = p.add_table_cell(_tbl, curr_col, curr_row, "", optlistColl + " margintop=1 rowheight=20");
            //    if (_tbl == -1)
            //        throw new Exception("Error: " + p.get_errmsg());
            //    p.end_item(_id_td);

            //    p.end_item(_id_row); //End body row

            //}

            ////Last Row
            //_id_row = p.begin_item("TR", "Title={Body Row}"); //body row

            //curr_row++;

            //_tf = -1;
            //curr_col = 1;
            //text = "Total Cost:  ";
            //tf_optlist = "fontname=Arial encoding=unicode fontsize=10 fontstyle=bold alignment=right ";
            //_tf = p.add_textflow(_tf, text, tf_optlist);
            //if (_tf == -1)
            //    throw new Exception("Error: " + p.get_errmsg());

            //optlistColl = " textflow=" + _tf;
            //_id_td = p.begin_item("TD", "");
            //_tbl = p.add_table_cell(_tbl, curr_col, curr_row, "", optlistColl + " margintop=1 colspan=2 rowheight=20");
            //if (_tbl == -1)
            //    throw new Exception("Error: " + p.get_errmsg());
            //p.end_item(_id_td);

            //curr_col = 3;
            ////optlistColl = "fittextline={position={ center bottom } font=" + font + " fontsize=10 fontstyle=bold fillcolor={gray 0}} ";
            //_id_td = p.begin_item("TD", "");
            //_tbl = p.add_table_cell(_tbl, curr_col, curr_row, "", "margintop=1 rowheight=20");
            //if (_tbl == -1)
            //    throw new Exception("Error: " + p.get_errmsg());
            //p.end_item(_id_td);

            //p.end_item(_id_row); //End body row
            //p.end_item(_id_body); //End Body
            //p.end_item(_id_table); //End Table
            /////End Table


            /////*Begin Place the Table*/
            ///* ---------- Place the table on one or more pages ---------- */

            ///*
            // * Loop until all of the table is placed; create new pages
            // * as long as more table instances need to be placed.
            // */
            //do
            //{
            //    /*Begin available height check*/
            //    boolNewPage = false; //new page flag
            //    if (ury <= MinHeight) //Min height is a pre-defined value
            //    {
            //        boolNewPage = true; //set to true if min height condition is satisfied
            //        #region new_page
            //        this.DrawFooter(ref p, ref pagecounter);

            //        p.end_page_ext(""); //end current page

            //        pagecounter++;
            //        p.begin_page_ext(0, 0, pagesize + " taborder=structure "); //Begin a new page
            //        llx = 60; lly = 50; urx = 570; ury = 745; //Reset Position Variables
            //        #endregion
            //        continue; //proceed to next iteration
            //    }
            //    /*End avaliable height check*/

            //    string optlist = "stroke={{line=frame linewidth=0.8} {line=other linewidth=0.3}}";
            //    //"stroke={line=hor1}";

            //    /* Place the table instance */
            //    result = p.fit_table(_tbl, llx, lly, urx, ury, optlist);

            //    if (result == "_boxfull")
            //    {
            //        #region new_page
            //        this.DrawFooter(ref p, ref pagecounter);

            //        p.end_page_ext(""); //end current page

            //        pagecounter++;
            //        p.begin_page_ext(0, 0, pagesize + " taborder=structure "); //Begin a new page
            //        llx = 60; lly = 50; urx = 570; ury = 745; //Reset Position Variables
            //        #endregion
            //    }

            //} while (result == "_boxfull" || boolNewPage);  //while (result == "_boxfull") ;

            ///* Check the result; "_stop" means all is ok. */
            //if (result != "_stop")
            //{
            //    if (result == "_error")
            //    {
            //        throw new Exception("Error when placing table: " +
            //                p.get_errmsg());
            //    }
            //    else
            //    {
            //        /* Any other return value is a user exit caused by
            //         * the "return" option; this requires dedicated code to
            //         * deal with.
            //         */
            //        throw new Exception("User return found in Textflow");
            //    }
            //}

            ///* This will also delete Textflow handles used in the table */
            //textendY = (int)p.info_table(_tbl, "height");
            //ury = ury - textendY - Yoffset; //fsize * 1.2; //Adjust to fit remaining text
            //p.delete_table(_tbl, "");

            /////*End Place the Table*/
            //#endregion

            #endregion

            p.end_page_ext("");

            if (createmode == 2)
            {
                #region ORDERFORM_Bookmark
                string bookmarkdest = "destinationOrderForm"; //define a name for the named destination - no space allowed
                p.add_nameddest(bookmarkdest, "type=fixed left=" + 0 + " top=" + 800); //add the named destination
                int bookmarkaction = p.create_action("GoTo", " destname=" + bookmarkdest);
                p.create_bookmark("Order Form", " action={activate=" + bookmarkaction + "}");
                #endregion
            }
            #endregion

            #region ThirdPage
            _tf = -1;
            p.begin_page_ext(0, 0, pagesize + " taborder=structure "); //Begin a new page
            llx = 60; lly = 50; urx = 570; ury = 745; //Reset Position Variables

            p.save();
            //HITT 11987 p.fit_textline("SHIPPING INFORMATION", llx, ury-20, "fontname=Helvetica-Bold fontsize=14 encoding=unicode fillcolor={#00006A}");
            p.fit_textline("SHIPPING INFORMATION", llx, ury - 20, "fontname=Helvetica-Bold fontsize=10 encoding=unicode ");
            
            /* Rectangle */
            p.setlinewidth(0.5);
            p.setcolor("stroke", "rgb", 0.0, 0.0, 0.0, 0.0);
            p.rect(60, 500, 510, 210);
            p.stroke();
            font = p.load_font("Arial", "unicode", "");
            
            string textopts = "fontname=Arial fontsize=11 encoding=unicode " + "boxsize={497 10} leader={alignment=right text=_}";
            //HITT 11987 p.fit_textline("NAME:", 65, 675, textopts);
            p.fit_textline("Name:", 65, 685, textopts);
            //HITT 11987 p.fit_textline("ORGANIZATION:", 65, 650, textopts);
            p.fit_textline("Organization:", 65, 660, textopts);
            //HITT 11987 p.fit_textline("ADDRESS:", 65, 625, textopts);
            p.fit_textline("Address Line 1:", 65, 635, textopts);
            //HITT 11987 p.fit_textline("ADDRESS:", 65, 600, textopts);
            p.fit_textline("Address Line 2:", 65, 610, textopts);
            //HITT 11987 p.fit_textline("CITY:", 65, 575, "fontname=Arial fontsize=11 encoding=unicode " + "boxsize={200 10} leader={alignment=right text=_}");
            p.fit_textline("City:", 65, 585, "fontname=Arial fontsize=11 encoding=unicode " + "boxsize={200 10} leader={alignment=right text=_}");
            //HITT 11987 p.fit_textline("STATE:", 265, 575, "fontname=Arial fontsize=11 encoding=unicode " + "boxsize={150 10} leader={alignment=right text=_}");
            p.fit_textline("State:", 265, 585, "fontname=Arial fontsize=11 encoding=unicode " + "boxsize={150 10} leader={alignment=right text=_}");
            //HITT 11987 p.fit_textline("ZIP CODE:", 415, 575, "fontname=Arial fontsize=11 encoding=unicode " + "boxsize={147 10} leader={alignment=right text=_}");
            p.fit_textline("ZIP Code:", 415, 585, "fontname=Arial fontsize=11 encoding=unicode " + "boxsize={147 10} leader={alignment=right text=_}");
            //HITT 11987 p.fit_textline("E-MAIL ADDRESS:", 65, 550, textopts);
            p.fit_textline("E-mail Address:", 65, 560, textopts);
            p.fit_textline("(An e-mail confirmation will be sent when your order has shipped.)", 65, 550, "fontname=Arial fontsize=9 fontstyle=italic encoding=unicode ");
            //HITT 11987 p.fit_textline("PHONE NUMBER:", 65, 525, "fontname=Arial fontsize=11 encoding=unicode " + "boxsize={275 10} leader={alignment=right text=_}");
            p.fit_textline("Phone:", 65, 535, "fontname=Arial fontsize=11 encoding=unicode " + "boxsize={275 10} leader={alignment=right text=_}");
            //HITT 11987 p.fit_textline("FAX NUMBER:", 340, 525, "fontname=Arial fontsize=11 encoding=unicode " + "boxsize={223 10} leader={alignment=right text=_}");
            p.fit_textline("Fax:", 340, 535, "fontname=Arial fontsize=11 encoding=unicode " + "boxsize={223 10} leader={alignment=right text=_}");
            //HITT 11987 p.fit_textline("NCI ACCOUNT NUMBER (if known):", 65, 500, textopts);
            p.fit_textline("POS Account Number (if known):", 65, 510, textopts);


            //HITT 11987 p.fit_textline("PAYMENT INFORMATION", llx, 450, "fontname=Helvetica-Bold fontsize=14 encoding=unicode fillcolor={#00006A}");
            p.fit_textline("BILLING INFORMATION, FOR BULK ORDERS (21+)", llx, 465, "fontname=Helvetica-Bold fontsize=10 encoding=unicode ");
            /* Rectangle */
            p.setlinewidth(0.5);
            p.setcolor("stroke", "rgb", 0.0, 0.0, 0.0, 0.0);
            //HITT 11987 p.rect(60, 295, 510, 125);
            p.rect(60, 170, 510, 280);
            p.stroke();
            
            /*checkbox rectangle*/
            //HITT 11987 p.setlinewidth(0.5);
            //HITT 11987 p.setcolor("stroke", "rgb", 0.0, 0.0, 0.0, 0.0);
            //HITT 11987 p.rect(68, 400, 6, 6);
            //HITT 11987 p.stroke();

            //p.fit_textline("&#x2751;", 68, 400, "charref fontname=ZapfDingbats fontsize=11 encoding=unicode ");
            //HITT 11987 p.fit_textline("CHECK OR MONEY ORDER (made payable to Publications Ordering Service)", 85, 400, "fontname=Arial fontsize=11 encoding=unicode ");
            //p.fit_textline("&#x2751;", 68, 375, "charref fontname=ZapfDingbats fontsize=11 encoding=unicode ");

            //HITT 11987 p.setlinewidth(0.5);
            //HITT 11987 p.setcolor("stroke", "rgb", 0.0, 0.0, 0.0, 0.0);
            //HITT 11987 p.rect(68, 375, 6, 6);
            //HITT 11987 p.stroke();

            #region FedEx
            p.fit_textline("FedEx Shipping Account Number:", 65, 420, "fontname=Arial fontsize=11 encoding=unicode " + "boxsize={497 10} leader={alignment=right text=_}");

            p.setlinewidth(0.5);
            p.setcolor("stroke", "rgb", 0.0, 0.0, 0.0, 0.0);
            p.rect(245, 407, 6, 6);
            p.stroke();
            p.fit_textline("Ground", 255, 407, "fontname=Arial fontsize=9 fontstyle=italic encoding=unicode ");

            p.setlinewidth(0.5);
            p.setcolor("stroke", "rgb", 0.0, 0.0, 0.0, 0.0);
            p.rect(325, 407, 6, 6);
            p.stroke();
            p.fit_textline("Overnight", 335, 407, "fontname=Arial fontsize=9 fontstyle=italic encoding=unicode ");

            p.setlinewidth(0.5);
            p.setcolor("stroke", "rgb", 0.0, 0.0, 0.0, 0.0);
            p.rect(405, 407, 6, 6);
            p.stroke();
            p.fit_textline("2-Day", 415, 407, "fontname=Arial fontsize=9 fontstyle=italic encoding=unicode ");

            p.setlinewidth(0.5);
            p.setcolor("stroke", "rgb", 0.0, 0.0, 0.0, 0.0);
            p.rect(485, 407, 6, 6);
            p.stroke();
            p.fit_textline("Express Saver", 495, 407, "fontname=Arial fontsize=9 fontstyle=italic encoding=unicode ");
            #endregion

            p.fit_textline("or", 65, 400, "fontname=Arial fontsize=11 encoding=unicode ");

            #region UPS
            p.fit_textline("UPS Shipping Account Number:", 65, 380, "fontname=Arial fontsize=11 encoding=unicode " + "boxsize={497 10} leader={alignment=right text=_}");

            p.setlinewidth(0.5);
            p.setcolor("stroke", "rgb", 0.0, 0.0, 0.0, 0.0);
            p.rect(245, 367, 6, 6);
            p.stroke();
            p.fit_textline("Ground", 255, 367, "fontname=Arial fontsize=9 fontstyle=italic encoding=unicode ");

            p.setlinewidth(0.5);
            p.setcolor("stroke", "rgb", 0.0, 0.0, 0.0, 0.0);
            p.rect(325, 367, 6, 6);
            p.stroke();
            p.fit_textline("Next-Day Air", 335, 367, "fontname=Arial fontsize=9 fontstyle=italic encoding=unicode ");

            p.setlinewidth(0.5);
            p.setcolor("stroke", "rgb", 0.0, 0.0, 0.0, 0.0);
            p.rect(405, 367, 6, 6);
            p.stroke();
            p.fit_textline("2nd-Day Air", 415, 367, "fontname=Arial fontsize=9 fontstyle=italic encoding=unicode ");

            p.setlinewidth(0.5);
            p.setcolor("stroke", "rgb", 0.0, 0.0, 0.0, 0.0);
            p.rect(485, 367, 6, 6);
            p.stroke();
            p.fit_textline("3-Day Select", 495, 367, "fontname=Arial fontsize=9 fontstyle=italic encoding=unicode ");
            #endregion
            
            p.setlinewidth(0.5);
            p.setcolor("stroke", "rgb", 0.0, 0.0, 0.0, 0.0);
            p.rect(65, 355, 6, 6);
            p.stroke();
            p.fit_textline("Same as shipping address", 75, 355, "fontname=Arial fontsize=10 fontstyle=normal encoding=unicode ");


            //HITT 11987 p.fit_textline("CREDIT CARD", 85, 375, "fontname=Arial fontsize=11 encoding=unicode ");
            //HITT 11987 p.fit_textline("VISA", 135, 355, "fontname=Arial fontsize=11 encoding=unicode ");
            //p.fit_textline("&#x2751;", 168, 355, "charref fontname=ZapfDingbats fontsize=11 encoding=unicode ");
            //HITT 11987 p.setlinewidth(0.5);
            //HITT 11987 p.setcolor("stroke", "rgb", 0.0, 0.0, 0.0, 0.0);
            //HITT 11987 p.rect(168, 355, 6, 6);
            //HITT 11987 p.stroke();
            //HITT 11987 p.fit_textline("MASTERCARD", 250, 355, "fontname=Arial fontsize=11 encoding=unicode ");
            //p.fit_textline("&#x2751;", 338, 355, "charref fontname=ZapfDingbats fontsize=11 encoding=unicode ");
            //HITT 11987 p.setlinewidth(0.5);
            //HITT 11987 p.setcolor("stroke", "rgb", 0.0, 0.0, 0.0, 0.0);
            //HITT 11987 p.rect(338, 355, 6, 6);
            //HITT 11987 p.stroke();
            //HITT 11987 p.fit_textline("AMERICAN EXPRESS", 400, 355, "fontname=Arial fontsize=11 encoding=unicode ");
            //p.fit_textline("&#x2751;", 523, 355, "charref fontname=ZapfDingbats fontsize=11 encoding=unicode ");
            //HITT 11987 p.setlinewidth(0.5);
            //HITT 11987 p.setcolor("stroke", "rgb", 0.0, 0.0, 0.0, 0.0);
            //HITT 11987 p.rect(523, 355, 6, 6);
            //HITT 11987 p.stroke();
            //HITT 11987 p.fit_textline("CARD #:", 135, 330, "fontname=Arial fontsize=11 encoding=unicode " + "boxsize={250 10} leader={alignment=right text=_}");
            //HITT 11987 p.fit_textline("EXP. DATE:", 385, 330, "fontname=Arial fontsize=11 encoding=unicode " + "boxsize={178 10} leader={alignment=right text=_}");
            //HITT 11987 p.fit_textline("CARD HOLDER NAME:", 135, 305, "fontname=Arial fontsize=11 encoding=unicode " + "boxsize={428 10} leader={alignment=right text=_}");

            //string textopts = "fontname=Arial fontsize=11 encoding=unicode " + "boxsize={497 10} leader={alignment=right text=_}";
            
            p.fit_textline("Name:", 65, 330, textopts);
            p.fit_textline("Organization:", 65, 305, textopts);
            p.fit_textline("Address Line 1:", 65, 280, textopts);
            p.fit_textline("Address Line 2:", 65, 255, textopts);
            p.fit_textline("City:", 65, 230, "fontname=Arial fontsize=11 encoding=unicode " + "boxsize={200 10} leader={alignment=right text=_}");
            p.fit_textline("State:", 265, 230, "fontname=Arial fontsize=11 encoding=unicode " + "boxsize={150 10} leader={alignment=right text=_}");
            p.fit_textline("ZIP Code:", 415, 230, "fontname=Arial fontsize=11 encoding=unicode " + "boxsize={147 10} leader={alignment=right text=_}");
            p.fit_textline("E-mail Address:", 65, 205, textopts);
            //p.fit_textline("(An e-mail confirmation will be sent when your order has shipped.)", 65, 540, "fontname=Arial fontsize=9 fontstyle=italic encoding=unicode ");
            p.fit_textline("Phone:", 65, 180, "fontname=Arial fontsize=11 encoding=unicode " + "boxsize={275 10} leader={alignment=right text=_}");
            p.fit_textline("Fax:", 340, 180, "fontname=Arial fontsize=11 encoding=unicode " + "boxsize={223 10} leader={alignment=right text=_}");

            p.restore();

            //HITT 11987 ury = 250; //Hard coded
            ury = 150;
            
            //Text flow for Publication Ordering Address and associated text
            _tf = -1;

            tf_optlist = "charref fontname=Arial encoding=unicode fontsize=11 ";
            //HITT 11987 text = "Mail the payment and Order Form to:";
            text = "Fax the completed form to 410–646–3117 or mail it to:";
            _tf = p.add_textflow(_tf, text, tf_optlist);
            if (_tf == -1)
                throw new Exception("Error: " + p.get_errmsg());

            text = "\n\n\tNational Cancer Institute, NIH, DHHS";
            _tf = p.add_textflow(_tf, text, tf_optlist);
            if (_tf == -1)
                throw new Exception("Error: " + p.get_errmsg());

            text = "\n\tPublications Ordering Service";
            _tf = p.add_textflow(_tf, text, tf_optlist);
            if (_tf == -1)
                throw new Exception("Error: " + p.get_errmsg());

            //HITT 11987 text = "\n\tPost Office Box 24128";
            text = "\n\tP.O. Box 24128";
            _tf = p.add_textflow(_tf, text, tf_optlist);
            if (_tf == -1)
                throw new Exception("Error: " + p.get_errmsg());

            text = "\n\tBaltimore, MD 21227";
            _tf = p.add_textflow(_tf, text, tf_optlist);
            if (_tf == -1)
                throw new Exception("Error: " + p.get_errmsg());

            //HITT 11987 text = "\n\nOr fax the Order Form and payment information to the Publications Ordering Service at 301-330-7968. To " +
            //HITT 11987         "order by phone, contact 1-800-4-CANCER and select the option to order publications. You may use our online " +
            //HITT 11987         "Publications Locator at";
            //HITT 11987 _tf = p.add_textflow(_tf, text, tf_optlist);
            //HITT 11987 if (_tf == -1)
            //HITT 11987    throw new Exception("Error: " + p.get_errmsg());

            /* Loop until all index entries are placed; create new pages as long as
            * more index entries need to be placed
            */
            do
            {
                result = p.fit_textflow(_tf, llx, lly, urx, ury, "");
                #region pageno
                pagecounter++;
                this.DrawFooter(ref p, ref pagecounter);
                #endregion

            } while (result == "_boxfull" || result == "_nextpage");

            /* Check for errors */
            if (result != "_stop")
            {
                /* "_boxempty" happens if the box is very small and doesn't
                 * hold any text at all.
                 */

                if (result == "_boxempty")
                    throw new Exception("Error: Textflow box too small");
                else
                {
                    /* Any other return value is a user exit caused by
                     * the "return" option; this requires dedicated code to
                     * deal with.
                     */

                    throw new Exception("User return '" + result +
                        "' found in Textflow");

                }
            }
            textendY = (int)p.info_textflow(_tf, "textendy");
            int textendX = (int)p.info_textflow(_tf, "textendx");
            p.delete_textflow(_tf);

            //HITT 11987 p.fit_textline("http://www.cancer.gov/publications", textendX + 5, textendY, "fontname=Arial fontsize=11 fontstyle=bold encoding=unicode fillcolor={#00006A}");
            //HITT 11987 int width = (int)p.info_textline("www.cancer.gov/publications", "width", "fontname=Arial fontsize=11 encoding=unicode ");
            //HITT 11987 p.fit_textline(".", textendX + width + 6, textendY, "fontname=Arial fontsize=11 encoding=unicode ");
            
            p.end_page_ext("");
            #endregion

            if (createmode == 2 && string.Compare(ConfigurationManager.AppSettings["TurnOnBackCover"],"1", true) == 0)
            {
                #region BackCoverPage
                p.begin_page_ext(0, 0, pagesize + " taborder=structure "); //Begin a new page
                //llx = 60; lly = 50; urx = 570; ury = 745; //Reset Position Variables
                int id_img, image;

                id_img = p.begin_item("Figure", "Alt={" + ConfigurationManager.AppSettings["BackCoverImageAlt"] + "}");

                image = p.load_image("auto", _searchpath + @"\" + ConfigurationManager.AppSettings["BackCoverImage"], "");
                if (image == -1)
                    throw new Exception("Error: " + p.get_errmsg());
                //p.fit_image(image, 0, 0, "position={center}");
                //p.fit_image(image, 0, 0, "");
                //TESTED OKAY int x = 0, y = 0, bw = 570, bh = 745;
                int x = 0, y = 0, bw = 650, bh = 800;
                //p.fit_image(image, x, y, "boxsize={" + bw + " " + bh + "} " + "position={center} fitmethod=meet showborder");
                p.fit_image(image, x, y, "boxsize={" + bw + " " + bh + "} " + "fitmethod=meet");
                p.close_image(image);

                p.end_item(id_img);
                p.end_page_ext("");

                #region BackCover_Bookmark
                string bookmarkdest = "destinationBackCover"; //define a name for the named destination - no space allowed
                p.add_nameddest(bookmarkdest, "type=fixed left=" + 0 + " top=" + 800); //add the named destination
                int bookmarkaction = p.create_action("GoTo", " destname=" + bookmarkdest);
                p.create_bookmark("Back Cover", " action={activate=" + bookmarkaction + "}");
                #endregion

                #endregion
            }
        }
    }
}
//e");

//            /* Check for errors */
//            if (result != "_stop")
//            {
//                /* "_boxempty" happens if the box is very small and doesn't
//                 * hold any text at all.
//                 */

//                if (result == "_boxempty")
//                    throw new Exception("Error: Textflow box too small");
//                else
//                {
//                    /* Any other return value is a user exit caused by
//                     * the "return" option; this requires dedicated code to
//                     * deal with.
//                     */

//                    throw new Exception("User return '" + result +
//                        "' found in Textflow");

//                }
//            }
//            textendY = (int)_pdflib.info_textflow(_tf, "textendy");
//            ury = textendY;
//            //ury += 10;
//            p.delete_textflow(_tf);
//            _tf = -1;
//            #endregion

//            /* Create URI action */
//            optlistLink = "url={" + "mailto:ncioceocs@mail.nih.gov" + "}";
//            action = p.create_action("URI", optlistLink);

//            /* Create Link annotation on matchbox "kraxiweb" */
//            optlistLink = "action={activate " + action +
//                "} linewidth=0 usematchbox={staticurl2}";
//            p.create_annotation(0, 0, 0, 0, "Link", optlistLink);

//            /* Continue with the remaining text flows */

//            tf_optlist = "charref fontname=Arial encoding=unicode fontsize=10 fontstyle=bold";
//            text = "\n\tFax:";
//            text = "";  //***EAC Suppress the text above 
//            _tf = p.add_textflow(_tf, text, tf_optlist);
//            if (_tf == -1)
//                throw new Exception("Error: " + p.get_errmsg());

//            tf_optlist = "charref fontname=Arial encoding=unicode fontsize=10";
//            text = "      410-646-3117";
//            text = "";  //***EAC Suppress the text above 
//            _tf = p.add_textflow(_tf, text, t