using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//Added
using System.Collections;
using PubEntAdmin.DAL;

namespace PubEntAdmin.BLL
{
    public class StackCollection:CollectionBase
    {
        private string _stackCollTitle = "";
        private int _stackCollSequence = -1;

        public string StackCollTitle //Collection level property
        {
            get { return _stackCollTitle; }
            set { _stackCollTitle = value; }
        }

        public int StackCollSequence
        {
            get { return _stackCollSequence; }
            set { _stackCollSequence = value; }
        }

        public Stack this[int index]
        {
            get { return ((Stack)List[index]); }
            set { List[index] = value; }
        }

        public int Add(Stack value)
        {
            return (List.Add(value));
        }

        public int IndexOf(Stack value)
        {
            return (List.IndexOf(value));
        }

        public void Insert(int index, Stack value)
        {
            List.Insert(index, value);
        }

        public void Remove(Stack value)
        {
            List.Remove(value);
        }

        public bool Contains(Stack value)
        {
            return (List.Contains(value));
        }

        protected override void OnInsert(int index, Object value)
        {
            if (value.GetType() != Type.GetType("PubEntAdmin.BLL.Stack"))
                throw new ArgumentException("value must be of type Stack.", "value");
        }

        protected override void OnRemove(int index, Object value)
        {
            if (value.GetType() != Type.GetType("PubEntAdmin.BLL.Stack"))
                throw new ArgumentException("value must be of type Stack.", "value");
        }

        protected override void OnSet(int index, Object oldValue, Object newValue)
        {
            if (newValue.GetType() != Type.GetType("PubEntAdmin.BLL.Stack"))
                throw new ArgumentException("newValue must be of type Stack.", "newValue");
        }

        protected override void OnValidate(Object value)
        {
            if (value.GetType() != Type.GetType("PubEntAdmin.BLL.Stack"))
                throw new ArgumentException("value must be of type Stack.");
        }

    }
}
