//namespace PubEnt.usercontrols
//{
//    using System;
//    using System.Data;
//    using System.Drawing;
//    using System.Web;
//    using System.Web.UI.WebControls;
//    using System.Web.UI.HtmlControls;
//    using PubEnt.BLL;

//    /// <summary>
//    ///		Summary description for CheckList
//    /// </summary>
//    public class CheckList: System.Web.UI.UserControl
//    {

//        #region Web Form Designer generated code
//        override protected void OnInit(EventArgs e)
//        {
//            //
//            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
//            //
//            InitializeComponent();
//            base.OnInit(e);
//        }
		
//        /// <summary>
//        ///		Required method for Designer support - do not modify
//        ///		the contents of this method with the code editor.
//        /// </summary>
//        private void InitializeComponent()
//        {
//            this.Load += new System.EventHandler(this.Page_Load);
//            this.PreRender += new System.EventHandler(this.CheckList_PreRender);

//        }
//        #endregion

//        private KVPairCollection _DataSource;
//        protected System.Web.UI.WebControls.DataGrid DataGrid1;
//        protected System.Web.UI.WebControls.CheckBoxList CheckBoxListLeft;
//        protected System.Web.UI.WebControls.TextBox TextLeft;
//        protected System.Web.UI.WebControls.CheckBoxList CheckBoxListRight;
//        protected System.Web.UI.WebControls.TextBox TextValues;
		
//        private bool _DisplayDefault = true;


//        public bool DisplayDefault 
//        {
//            get { return _DisplayDefault; }
//            set { _DisplayDefault = value; }
//        }

//        public string Value 
//        {
//            //get { return TextValues.Text ; }
//            get { 
//                string ret="";
//                string sep="";
//                foreach (ListItem item in CheckBoxListLeft.Items)
//                {					
//                    if(item.Selected) 
//                    {
//                        if (item.Value == "-99") return "";
//                        ret += sep + item.Value;
//                        sep = ",";
//                    }
//                }	
//                return ret;
//            }
//            set{
//                foreach (ListItem item in this.CheckBoxListLeft.Items)
//                {
//                    string s = "," + value + ",";
//                    if (s.Contains("," + item.Value+",")){
//                        item.Selected = true;
//                    }
//                    else{
//                        item.Selected = false;
//                    }
//                }	
//            }
//        }

//        public string SelectedText
//        {
//            get
//            {
//                string ret = "";
//                string sep = "";
//                foreach (ListItem item in CheckBoxListLeft.Items)
//                {
//                    if (item.Selected)
//                    {
//                        if (item.Value == "-99") return "";
//                        ret += sep + item.Text;
//                        sep = ", ";
//                    }
//                }
//                return ret;
//            }
//        }

//        public KVPairCollection DataSource 
//        {
//            get { return _DataSource; }
//            set { _DataSource = value; }
//        }

//        public override void DataBind() 
//        {

//            CheckBoxListLeft.DataSource = _DataSource;
//            CheckBoxListLeft.DataTextField = "val";
//            CheckBoxListLeft.DataValueField = "key";
//            CheckBoxListLeft.DataBind();
//            if (_DisplayDefault)
//                CheckBoxListLeft.Items.Insert(0, new ListItem( "All", "-99" ) );
//        }

//        private void Page_Load(object sender, System.EventArgs e)
//        {

//        }

//        private void CheckList_PreRender(object sender, EventArgs e)
//        {
//            this.CheckBoxListLeft.Attributes.Add("onclick","Left2Right('" + this.ClientID+ "')");
//            TextLeft.Text  = "";
//            string sep = "";
//            foreach (ListItem item in CheckBoxListLeft.Items)
//            {
//                //item.Attributes.Add("alt", item.Value);				
//                TextLeft.Text += sep + item.Value;
//                sep = ",";
//            }	

//        }

//    }
//}
