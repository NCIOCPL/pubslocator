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
    public class SingleSelectCheckBoxItemCollection : CollectionBase
    {
        private static readonly string strSingleSelectCheckBoxItem = "PubEntAdmin.BLL.SingleSelectCheckBoxItem";

        public SingleSelectCheckBoxItem this[int index]
        {
            get { return ((SingleSelectCheckBoxItem)List[index]); }
            set { List[index] = value; }
        }

        public int Add(SingleSelectCheckBoxItem value)
        {
            return (List.Add(value));
        }

        public int IndexOf(SingleSelectCheckBoxItem value)
        {
            return (List.IndexOf(value));
        }

        public void Insert(int index, SingleSelectCheckBoxItem value)
        {
            List.Insert(index, value);
        }

        public void Remove(SingleSelectCheckBoxItem value)
        {
            List.Remove(value);
        }

        public bool Contains(SingleSelectCheckBoxItem value)
        {
            return (List.Contains(value));
        }

        protected override void OnInsert(int index, Object value)
        {
            if (value.GetType() != Type.GetType(strSingleSelectCheckBoxItem))
                throw new ArgumentException("value must be of type SingleSelectCheckBoxItem.", "value");
        }

        protected override void OnRemove(int index, Object value)
        {
            if (value.GetType() != Type.GetType(strSingleSelectCheckBoxItem))
                throw new ArgumentException("value must be of type OverallDescRole.", "value");
        }

        protected override void OnSet(int index, Object oldValue, Object newValue)
        {
            if (newValue.GetType() != Type.GetType(strSingleSelectCheckBoxItem))
                throw new ArgumentException("newValue must be of type SingleSelectCheckBoxItem.", "newValue");
        }

        protected override void OnValidate(Object value)
        {
            if (value.GetType() != Type.GetType(strSingleSelectCheckBoxItem))
                throw new ArgumentException("value must be of type SingleSelectCheckBoxItem.");
        }
    }
}
