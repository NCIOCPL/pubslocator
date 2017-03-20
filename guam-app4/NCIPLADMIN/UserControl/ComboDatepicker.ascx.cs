using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace PubEntAdmin.UserControl
{
    public partial class ComboDatepicker : System.Web.UI.UserControl
    {
        #region Fields
        private string strLableName = "";
        #endregion

        #region Events Handling
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.BindOptions();

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        #endregion

        #region Methods
        public string ClientScript()
        {
            StringBuilder s = new StringBuilder();
            s.Append(@"
                if (document.getElementById('" + this.ddlM.ClientID + @"').value != '0' &&
                    document.getElementById('" + this.ddly.ClientID + @"').value == '0')
                {
                    if (errormsg.length>0)
                        errormsg += '\n';

                    args.IsValid = false;
                    errormsg += '" + this.LabelName + @"(Year) is required.';
                }
                else if (document.getElementById('" + this.ddld.ClientID + @"').value != '0' &&
                    (document.getElementById('" + this.ddly.ClientID + @"').value == '0' ||
                     document.getElementById('" + this.ddlM.ClientID + @"').value == '0'))
                {
                    if (errormsg.length>0)
                            errormsg += '\n';

                    args.IsValid = false;
                    if (document.getElementById('" + this.ddly.ClientID + @"').value == '0' &&
                        document.getElementById('" + this.ddlM.ClientID + @"').value == '0')
                        errormsg += '" +this.LabelName+@"(Year) and "+this.LabelName+@"(Month) are required.';
                    else if ((document.getElementById('" + this.ddly.ClientID + @"').value == '0' &&
                            document.getElementById('" + this.ddlM.ClientID + @"').value != '0') ||
                            (document.getElementById('" + this.ddly.ClientID + @"').value != '0' &&
                            document.getElementById('" + this.ddlM.ClientID + @"').value == '0'))
                         errormsg += 'Invalid " +this.LabelName + @" format.';
                }
                
                if (document.getElementById('" + this.ddld.ClientID + @"').value != '0' &&
                    document.getElementById('" + this.ddly.ClientID + @"').value != '0' &&
                    document.getElementById('" + this.ddlM.ClientID + @"').value != '0')
                {
                    /*
                    var returndays = isleap(document.getElementById('" + this.ddly.ClientID + @"').value);
                    if (document.getElementById('" + this.ddld.ClientID + @"').value > returndays 
                        && document.getElementById('" + this.ddlM.ClientID + @"').value=='2')
                     {
                         args.IsValid = false;
                         if (errormsg.length>0)
                            errormsg += '\n';
                         errormsg += 'Invalid date format for leap year.';
                     }

                    if(document.getElementById('" + this.ddld.ClientID + @"').value % 2==0 &&
                        document.getElementById('" + this.ddld.ClientID + @"').value=='31')
                    {
                         args.IsValid = false;
                         if (errormsg.length>0)
                            errormsg += '\n';
                         errormsg += 'Invalid date format for February.';
                    }
                    */

                    if (!isDate(document.getElementById('" + this.ddlM.ClientID + @"').value+'/'+
                               document.getElementById('" + this.ddld.ClientID + @"').value+'/'+
                               document.getElementById('" + this.ddly.ClientID + @"').value)) 
                    {
                        args.IsValid = false;
                         if (errormsg.length>0)
                            errormsg += '\n';
                         errormsg += 'Invalid " + this.LabelName + @" format.';
                    }
                }
            ");
            return s.ToString();
        }

        protected void BindOptions()
        {
            ListItem a;
            for (int i = 0; i <= 12; i++)
            {
                if (i == 0)
                    a = new ListItem("NONE", "0");
                else
                    a = new ListItem(i.ToString(), i.ToString());
                this.ddlM.Items.Add(a);
            }

            for (int i = 0; i <= 31; i++)
            {
                if (i == 0)
                    a = new ListItem("NONE", "0");
                else
                    a = new ListItem(i.ToString(), i.ToString());
                this.ddld.Items.Add(a);
            }

            int j = 0;
            bool first = true;
            DateTime dt = DateTime.Now.AddYears(-PubEntAdminManager.PubHistStartYearNum);
            j = dt.Year;

            

            dt = DateTime.Now.AddYears(PubEntAdminManager.PubEntHistEndYearNum);

            for (; j <= dt.Year; j++)
            {
                if (first)
                {
                    a = new ListItem("YYYY", "0");
                    first = false;
                }
                else
                    a = new ListItem(j.ToString(), j.ToString());
                this.ddly.Items.Add(a);
            }
        }

        public void SetDefault()
        {
            this.DefaultDay();
            this.DefaultMonth();
            this.DefaultYear();

        }

        public void DefaultDay()
        {
            this.ddld.SelectedIndex = 0;
        }

        public void DefaultMonth()
        {
            this.ddlM.SelectedIndex = 0;
        }

        public void DefaultYear()
        {
            this.ddly.SelectedIndex = 0;
        }

        #endregion

        #region Properties
        public bool DTComboState
        {
            get
            {
                return this.ddld.Enabled;
            }
            set
            {
                this.ddld.Enabled = this.ddlM.Enabled = this.ddly.Enabled = value;
            }
        }

        public string Day
        {
            set
            {
                this.ddld.SelectedIndex = this.ddld.Items.IndexOf(this.ddld.Items.FindByValue(value));
            }
            get
            {
                return this.ddld.SelectedValue;
            }
        }

        public string Month
        {
            set
            {
                this.ddlM.SelectedIndex = this.ddlM.Items.IndexOf(this.ddlM.Items.FindByValue(value));
            }
            get
            {
                return this.ddlM.SelectedValue;
            }
        }

        public string Year
        {
            set
            {
                this.ddly.SelectedIndex = this.ddly.Items.IndexOf(this.ddly.Items.FindByValue(value));
            }
            get
            {
                return this.ddly.SelectedValue;
            }
        }

        public string DayClientID
        {
            get { return this.ddld.ClientID; }
        }

        public string MonthClientID
        {
            get { return this.ddlM.ClientID; }
        }

        public string YearClientID
        {
            get { return this.ddly.ClientID; }
        }

        public string DayID
        {
            get { return this.ddld.ID; }
        }

        public string MonthID
        {
            get { return this.ddlM.ID; }
        }

        public string YearID
        {
            get { return this.ddly.ID; }
        }

        public string LabelName
        {
            set { this.strLableName = value; }
            get { return this.strLableName; }
        }

        public DropDownList ddlMonth
        {
            get { return this.ddlM; }
        }

        public DropDownList ddlDay
        {
            get { return this.ddld; }
        }

        public DropDownList ddlYear
        {
            get { return this.ddly; }
        }
        #endregion
    }
}