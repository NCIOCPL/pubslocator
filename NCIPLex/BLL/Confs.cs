﻿using System;
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

namespace NCIPLex.BLL
{
    public class Confs : CollectionBase
    {

        private static readonly string strConfItemType = "NCIPLex.BLL.Conf";

        public Conf this[int index]
        {
            get { return ((Conf)List[index]); }
            set { List[index] = value; }
        }

        public int Add(Conf value)
        {
            return (List.Add(value));
        }

        public int IndexOf(Conf value)
        {
            return (List.IndexOf(value));
        }

        public void Insert(int index, Conf value)
        {
            List.Insert(index, value);
        }

        public void Remove(Conf value)
        {
            List.Remove(value);
        }

        public bool Contains(Conf value)
        {
            return (List.Contains(value));
        }

        protected override void OnInsert(int index, Object value)
        {
            if (value.GetType() != Type.GetType(strConfItemType))
                throw new ArgumentException("value must be of type Conf.", "value");
        }

        protected override void OnRemove(int index, Object value)
        {
            if (value.GetType() != Type.GetType(strConfItemType))
                throw new ArgumentException("value must be of type Conf.", "value");
        }

        protected override void OnSet(int index, Object oldValue, Object newValue)
        {
            if (newValue.GetType() != Type.GetType(strConfItemType))
                throw new ArgumentException("newValue must be of type Conf.", "newValue");
        }

        protected override void OnValidate(Object value)
        {
            if (value.GetType() != Type.GetType(strConfItemType))
                throw new ArgumentException("value must be of type Conf.");
        }
    }
}
