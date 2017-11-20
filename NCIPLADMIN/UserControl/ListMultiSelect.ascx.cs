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

using PubEntAdmin.CustomControl;
using PubEntAdmin.BLL;

namespace PubEntAdmin.UserControl
{
    public partial class ListMultiSelect : System.Web.UI.UserControl
    {
        #region Varialbles
        private MultiSelectListBoxItemCollection _DataSource;
        private bool blnDisplayDefault = false;
        private bool blnTurnOffValidator = false;
        private bool blnReadOnly = false;
        #endregion

        public event EventHandler SelectedIndexesChanged;

        public ListMultiSelect()
        {
            this.lstMultiSelect = new ListBoxBase();

        }

        #region Properties
        /// <summary>
        /// USE THIS PROPERTY WITH CAUTION, 
        /// THE REASON THIS PROPERTY IS PROVIDED IS TO USE FOR ALERTING USER 
        /// TO SAVE PAGE BEFORE THEY LEAVE
        /// NO DIRECT USAGE SHOULD BE ALLOWED, NO EXCEPTION.
        /// </summary>
        public ListBoxBase InnerList
        {
            get { return this.lstMultiSelect; }
        }

        public override string ClientID
        {
            get
            {
                return this.lstMultiSelect.ClientID;
            }
        }

        public ListSelectionMode SelectionMode
        {
            get { return this.lstMultiSelect.SelectionMode; }
            set { this.lstMultiSelect.SelectionMode = value;}
        }

        public bool Enabled
        {
            get { return this.lstMultiSelect.Enabled; }
            set { this.lstMultiSelect.Enabled = value; }
        }

        public new bool Visible
        {
            get { return this.lstMultiSelect.Visible; }
            set { this.lstMultiSelect.Visible = value; }
        }

        public bool DisplayDefault
        {
            get { return this.blnDisplayDefault; }
            set { this.blnDisplayDefault = value; }
        }

        public string CssClass
        {
            get { return lstMultiSelect.CssClass; }
            set { lstMultiSelect.CssClass = value; }
        }

        public bool AutoPostBack
        {
            get { return lstMultiSelect.AutoPostBack; }
            set { lstMultiSelect.AutoPostBack = value; }
        }

        public bool TurnOffValidator
        {
            set
            {
                this.blnTurnOffValidator = value;
                this.Required = !(value);
                this.SelectionUndefinedVar = !(value);
            }
            get { return this.blnTurnOffValidator; }
        }

        public bool Required
        {
            get { return this.ReqFieldVal.Enabled; }
            set
            {
                this.ReqFieldVal.Enabled = value;
                this.ReqFieldVal.Visible = value;
            }
        }

        public bool SelectionUndefinedVar
        {
            get { return this.CustValidator.Enabled; }
            set
            {
                this.CustValidator.Enabled = value;
                this.CustValidator.Visible = value;
            }
        }

        public MultiSelectListBoxItemCollection DataSource
        {
            get { return _DataSource; }
            set { _DataSource = value; }
        }

        public int Rows
        {
            get { return this.lstMultiSelect.Rows; }
            set { this.lstMultiSelect.Rows = value; }
        }

        public Unit Height
        {
            get { return this.lstMultiSelect.Height; }
            set { this.lstMultiSelect.Height = (value); }
        }

        public Unit Width
        {
            get { return this.lstMultiSelect.Width; }
            set { this.lstMultiSelect.Width = (value); }
        }

        public ListItemCollection SelectedItems
        {
            get
            {
                ListItemCollection t = new ListItemCollection();
                foreach (ListItem li in this.lstMultiSelect.Items)
                {
                    if (li.Selected)
                        t.Add(li);
                }
                return t;
            }
        }

        public ListItemCollection Items
        {
            get
            {
                ListItemCollection t = new ListItemCollection();
                foreach (ListItem li in this.lstMultiSelect.Items)
                {
                    t.Add(li);
                }
                return t;
            }
        }

        public string[] SelectedValue
        {
            get
            {
                string[] pk = null;
                ListItemCollection l = this.SelectedItems;
                if (l == null)
                    return null;
                else
                {
                    pk = new string[l.Count];
                    for (int i = 0; i < l.Count; i++)
                    {
                        pk[i] = l[i].Value;
                    }
                }
                return pk;
            }
            set
            {
                if (value != null)
                {
                    for (int i = 0; i < value.Length; i++)
                    {
                        ListItem li = lstMultiSelect.Items.FindByValue(value[i]);
                        if (li != null)
                            li.Selected = true;
                    }
                }
            }
        }

        public string SelectedSingleValue
        {
            set { this.lstMultiSelect.SelectedValue = value; }
            get { return this.lstMultiSelect.SelectedValue; }
        }

        public int SelectedIndex
        {
            set { this.lstMultiSelect.SelectedIndex = value; }
            get { return this.lstMultiSelect.SelectedIndex; }
        }

        public string DataTextField
        {
            set { this.lstMultiSelect.DataTextField = value; }
            get { return this.lstMultiSelect.DataTextField; }
        }

        public string DataValueField
        {
            set { this.lstMultiSelect.DataValueField = value; }
            get { return this.lstMultiSelect.DataValueField; }
        }

        public string DataTextFormatString
        {
            set { this.lstMultiSelect.DataTextFormatString = value; }
            get { return this.lstMultiSelect.DataTextFormatString; }
        }

        public bool ReadOnly
        {
            set {
                this.blnReadOnly = value; 
            }
            get { return this.blnReadOnly; }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.blnReadOnly)
            {
                this.lstMultiSelect.Attributes.CssStyle.Add("color", "#808080");
            }
        }

        public string selectedValueToStringWithSingleQuote(string sep)
        {
            string s = "";
            string[] t = this.SelectedValue;
            if (t != null)
            {
                for (int i = 0; i < t.Length; i++)
                {
                    if (i > 0 && i < t.Length)
                        s += sep;
                    s += "'" + t[i] + "'";
                }

            }
            return s;
        }

        public string selectedValueToStringWithSingleQuote()
        {
            return selectedValueToStringWithSingleQuote(",");
        }

        public string SelectedValueToString(string sep)
        {
            string s = "";
            string[] t = this.SelectedValue;
            
            if (t.Length > 0)
            {
                if (this.DisplayDefault)
                {
                    if (this.Items[0].Text.ToLower().IndexOf("all") >= 0 &&
                        this.Items[0].Value == t[0])//allow select all
                    {
                        //for (int i = 1; i < this.Items.Count; i++)//everything except the first one
                        //{
                        //    if (i > 1 && i < this.Items.Count)
                        //        s += sep;
                        //    s += this.Items[i].Value;
                        //}
                        s = t[0];
                    }
                    else
                    {
                        for (int i = 0; i < t.Length; i++)
                        {
                            if (i > 0 && i < t.Length)
                                s += sep;
                            s += t[i];
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < t.Length; i++)
                    {
                        if (i > 0 && i < t.Length)
                            s += sep;
                        s += t[i];
                    }
                }
            }
            return s;
        }

        public string SelectedValueToString()
        {
            return SelectedValueToString(",");
        }

        public string SelectedTextToString(string sep)
        {
            string s = "";
            ListItemCollection t = this.SelectedItems;
            if (t != null)
            {
                for (int i = 0; i < t.Count; i++)
                {
                    if (i > 0 && i < t.Count)
                        s += sep;
                    s += t[i].Text;
                }
            }
            return s;
        }

        public string SelectedTextToString()
        {
            return SelectedTextToString(",");
        }

        public override void DataBind()
        {
            base.DataBind();
            this.DataBind(null);
        }

        public void DataBind(string defaultOption)
        {
            ListItem li = null;
            //add for debug
            string[] t = null;
            if (this.lstMultiSelect.Items.Count != 0)
                t = this.SelectedValue;

            lstMultiSelect.Items.Clear();


            if (this.DataTextField.Length == 0 && this.DataValueField.Length == 0)
            {
                for (int i = 0; i < this.DataSource.Count; i++)
                {
                    li = new ListItem(this.DataSource[i].Name, this.DataSource[i].ID.ToString());

                    //li.Enabled = this.DataSource[i].Enabled;
                    
                    this.lstMultiSelect.Items.Add(li);
                }
            }
            else
            {
                this.lstMultiSelect.DataSource = this.DataSource;
                this.lstMultiSelect.DataBind();
            }

            if (this.DisplayDefault)
            {
                if (defaultOption == null)
                    li = new ListItem("ALL", "0");
                else
                    li = new ListItem(defaultOption, "0");
                this.lstMultiSelect.Items.Insert(0, li);
            }

            if (t != null)
                this.SelectedValue = t;
        }

        protected void OnSelectedIndexesChanged(EventArgs e)
        {
            if (this.SelectedIndexesChanged != null)
            {
                this.SelectedIndexesChanged(this, e);
            }
        }

        private void lstMultiSelect_SelectedIndexesChanged(object sender, EventArgs e)
        {
            OnSelectedIndexesChanged(e);
        }

        public void CustVal_MultiSelect_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string[] f;
            
            f = this.lstMultiSelect.SelectedValue.Split(new Char[] { ',' });
            for (int i = 0; i < f.Length; i++)
            {
                if (f[i] == "0")
                {
                    args.IsValid = false;
                    break;
                }
                args.IsValid = true;
            }
        }
    }
}