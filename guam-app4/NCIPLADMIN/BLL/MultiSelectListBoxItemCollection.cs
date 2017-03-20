using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections;

namespace PubEntAdmin.BLL
{
    [Serializable()]
    public class MultiSelectListBoxItemCollection : CollectionBase
    {
        private static readonly string strMultiSelectListBoxItemType = "PubEntAdmin.BLL.MultiSelectListBoxItem";

        public MultiSelectListBoxItem this[int index]
        {
            get { return ((MultiSelectListBoxItem)List[index]); }
            set { List[index] = value; }
        }

        public int Add(MultiSelectListBoxItem value)
        {
            return (List.Add(value));
        }

        public int IndexOf(MultiSelectListBoxItem value)
        {
            return (List.IndexOf(value));
        }

        public void Insert(int index, MultiSelectListBoxItem value)
        {
            List.Insert(index, value);
        }

        public void Remove(MultiSelectListBoxItem value)
        {
            List.Remove(value);
        }

        public bool Contains(MultiSelectListBoxItem value)
        {
            return (List.Contains(value));
        }

        protected override void OnInsert(int index, Object value)
        {
            if (value.GetType() != Type.GetType(strMultiSelectListBoxItemType))
                throw new ArgumentException("value must be of type MultiSelectListBoxItem.", "value");
        }

        protected override void OnRemove(int index, Object value)
        {
            if (value.GetType() != Type.GetType(strMultiSelectListBoxItemType))
                throw new ArgumentException("value must be of type OverallDescRole.", "value");
        }

        protected override void OnSet(int index, Object oldValue, Object newValue)
        {
            if (newValue.GetType() != Type.GetType(strMultiSelectListBoxItemType))
                throw new ArgumentException("newValue must be of type MultiSelectListBoxItem.", "newValue");
        }

        protected override void OnValidate(Object value)
        {
            if (value.GetType() != Type.GetType(strMultiSelectListBoxItemType))
                throw new ArgumentException("value must be of type MultiSelectListBoxItem.");
        }
    }
}
