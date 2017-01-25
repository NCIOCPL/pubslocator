using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PubEntAdmin.BLL
{
    public class TabItem : MultiSelectListBoxItem
    {
        private string url;

        public TabItem(int id, string name, string url)
            : base(id, name)
        {
            this.TabItemUrl = url;
        }

        public int TabItemID
        {
            get { return this.ID; }
            set { this.ID = value; }
        }

        public string TabItemName
        {
            get { return this.Name; }
            set { this.Name = value; }
        }

        public string TabItemUrl
        {
            get { return this.url; }
            set { this.url = value; }
        }
    }
}
