using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//Added
using System.Collections;
using PubEnt.DAL;

namespace PubEnt.BLL
{
    [Serializable]
    public class SuperStackCollection : CollectionBase
    {
        public StackCollection this[int index]
        {
            get { return ((StackCollection)List[index]); }
            set { List[index] = value; }
        }

        public int Add(StackCollection value)
        {
            return (List.Add(value));
        }

        public int IndexOf(StackCollection value)
        {
            return (List.IndexOf(value));
        }

        public void Insert(int index, StackCollection value)
        {
            List.Insert(index, value);
        }

        public void Remove(StackCollection value)
        {
            List.Remove(value);
        }

        public bool Contains(StackCollection value)
        {
            return (List.Contains(value));
        }

        protected override void OnInsert(int index, Object value)
        {
            if (value.GetType() != Type.GetType("PubEnt.BLL.StackCollection"))
                throw new ArgumentException("value must be of type StackCollection.", "value");
        }

        protected override void OnRemove(int index, Object value)
        {
            if (value.GetType() != Type.GetType("PubEnt.BLL.StackCollection"))
                throw new ArgumentException("value must be of type StackCollection.", "value");
        }

        protected override void OnSet(int index, Object oldValue, Object newValue)
        {
            if (newValue.GetType() != Type.GetType("PubEnt.BLL.StackCollection"))
                throw new ArgumentException("newValue must be of type StackCollection.", "newValue");
        }

        protected override void OnValidate(Object value)
        {
            if (value.GetType() != Type.GetType("PubEnt.BLL.StackCollection"))
                throw new ArgumentException("value must be of type StackCollection.");
        }

        #region Sort
        public enum StackFields
        {
            StackCollSequence
        }

        private sealed class StackCollSeqComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                StackCollection first = (StackCollection)x;
                StackCollection second = (StackCollection)y;
                return first.StackCollSequence.CompareTo(second.StackCollSequence);
            }
        }

        public void Sort(StackFields sortField, bool isAscending)
        {
            switch (sortField)
            {
                case StackFields.StackCollSequence:
                    InnerList.Sort(new StackCollSeqComparer());
                    break;
            }
            if (!isAscending) InnerList.Reverse();
        }
        #endregion
    }
}
