using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PubEntAdmin.BLL;
using PubEntAdmin.UIL;

namespace PubEntAdmin.UserControl
{
    public partial class TabStrip : System.Web.UI.UserControl, ITabMenu
    {
        private ArrayList colTabs = new ArrayList();

        private List<TabItem> TabSrc;

        protected static readonly string TabMenuSelectedItem = "TabMenuSelectedItem";
        protected static readonly string OldTabMenuSelectedItem = "OldTabMenuSelectedItem";

        protected static readonly string TabMenuSelectedIdx = "TabMenuSelectedIdx";
        protected static readonly string strActiveMenuItem = "ActiveMenuItem";
        protected static readonly string strDefaultTabPage = "Grant.aspx";
        protected static readonly string strFirstTimePage = "Grant.aspx";

        #region SelectionChangedEvent
        public class SelectionChangedEventArgs : EventArgs
        {
            public string TabNameSelected;
        }

        public delegate void SelectionChangedEventHandler(object sender, SelectionChangedEventArgs e);
        public event SelectionChangedEventHandler SelectionChanged;

        #endregion

        #region Events Handling
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!Page.IsPostBack)
                this.BindData();
        }

        protected void lstTabs_ItemCommand(object source, DataListCommandEventArgs e)
        {
            SelectTab(e.Item.ItemIndex);
        }

        #endregion

        #region Properties
        public string CurrentTabName
        {
            get
            {
                if (Context.Session[TabMenuSelectedItem] == null)
                {
                    Context.Session[TabMenuSelectedItem] = String.Empty;
                        
                }
                return (string)Context.Session[TabMenuSelectedItem];
            }
        }
        public ArrayList Source
        {
            get { return this.colTabs; }
        }
        public List<TabItem> TabSource
        {
            set { this.TabSrc = value; }
            get{return this.TabSrc;}

        }
        #endregion

        #region Methods
        protected void SetSelectedIndex(string selectedTab)
        {
            for (int i = 0; i < this.Source.Count; i++)
            {
                if (((TabItem)this.Source[i]).TabItemName == selectedTab)
                {
                    this.lstTabs.SelectedIndex = i + 1;
                    break;
                }
            }
        }

        public string GetTabName(object elem)
        {
            return ((TabItem)((DataListItem)elem).DataItem).TabItemName;
        }
        
        public string GetTabUrl(string tabName)
        {
            string url = strDefaultTabPage;
            for (int i = 0; i < this.Source.Count; i++)
            {
                if (((TabItem)this.Source[i]).TabItemName == tabName)
                {
                    url = ((TabItem)this.Source[i]).TabItemUrl;
                    break;
                }
            }
            return url;
        }

        private void BindData()
        {
            this.GetSource();
            lstTabs.DataSource = colTabs;
            lstTabs.DataBind();
        }

        protected string SetCssClass(object elem)
        {
            if (((TabItem)((DataListItem)elem).DataItem).TabItemName == CurrentTabName) return "selectedTab";
            return "defaultTab";
        }

        public void SelectTab(int index)
        {
            if (index < 0 || index > this.Source.Count) index = 0;
            if (Context.Session[TabMenuSelectedItem] != null)
                Context.Session[OldTabMenuSelectedItem] = Context.Session[TabMenuSelectedItem];
            Context.Session[TabMenuSelectedItem] = ((TabItem)this.Source[index]).TabItemName;

            BindData();
            SelectionChangedEventArgs ev = new SelectionChangedEventArgs();
            ev.TabNameSelected = (string)Context.Session[TabMenuSelectedItem];
            if (SelectionChanged != null) SelectionChanged(this, ev);
        }

        #region ITabMenu Implementation

        public void GetSource()
        {
            this.colTabs.Clear();
            foreach (TabItem t in this.TabSrc)
            {
                this.colTabs.Add(t);
            }
        }
        #endregion

        #endregion

    }
}