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

namespace PubEntAdmin.BLL
{
    public class SubSubCatCollection : MultiSelectListBoxItemCollection
    {

        private static readonly string strStateItemType = "PubEntAdmin.BLL.SubSubCat";

        public new SubSubCat this[int index]
        {
            get { return ((SubSubCat)List[index]); }
            set { List[index] = value; }
        }

        public int Add(SubSubCat value)
        {
            return (List.Add(value));
        }

        public int IndexOf(SubSubCat value)
        {
            return (List.IndexOf(value));
        }

        public void Insert(int index, SubSubCat value)
        {
            List.Insert(index, value);
        }

        public void Remove(SubSubCat value)
        {
            List.Remove(value);
        }

        public bool Contains(SubSubCat value)
        {
            return (List.Contains(value));
        }

        protected override void OnInsert(int index, Object value)
        {
            if (value.GetType() != Type.GetType(strStateItemType))
                throw new ArgumentException("value must be of type SubSubCat.", "value");
        }

        protected override void OnRemove(int index, Object value)
        {
            if (value.GetType() != Type.GetType(strStateItemType))
                throw new ArgumentException("value must be of type SubSubCat.", "value");
        }

        protected override void OnSet(int index, Object oldValue, Object newValue)
        {
            if (newValue.GetType() != Type.GetType(strStateItemType))
                throw new ArgumentException("newValue must be of type SubSubCat.", "newValue");
        }

        protected override void OnValidate(Object value)
        {
            if (value.GetType() != Type.GetType(strStateItemType))
                throw new ArgumentException("value must be of type SubSubCat.");
        }
    }
}
