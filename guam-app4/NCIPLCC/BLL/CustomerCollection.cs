using System;
using System.Collections;
using PubEnt.DAL;

namespace PubEnt.BLL
{
    [Serializable]
    public class CustomerCollection : CollectionBase
    {

        public Customer this[int index]
        {
            get { return ((Customer)List[index]); }
            set { List[index] = value; }
        }

        public int Add(Customer value)
        {
            return (List.Add(value));
        }

        public int IndexOf(Customer value)
        {
            return (List.IndexOf(value));
        }

        public void Insert(int index, Customer value)
        {
            List.Insert(index, value);
        }

        public void Remove(Customer value)
        {
            List.Remove(value);
        }

        public bool Contains(Customer value)
        {
            return (List.Contains(value));
        }

        protected override void OnInsert(int index, Object value)
        {
            if (value.GetType() != Type.GetType("PubEnt.BLL.Customer"))
                throw new ArgumentException("value must be of type Customer.", "value");
        }

        protected override void OnRemove(int index, Object value)
        {
            if (value.GetType() != Type.GetType("PubEnt.BLL.Customer"))
                throw new ArgumentException("value must be of type Customer.", "value");
        }

        protected override void OnSet(int index, Object oldValue, Object newValue)
        {
            if (newValue.GetType() != Type.GetType("PubEnt.BLL.Customer"))
                throw new ArgumentException("newValue must be of type Customer.", "newValue");
        }

        protected override void OnValidate(Object value)
        {
            if (value.GetType() != Type.GetType("PubEnt.BLL.Customer"))
                throw new ArgumentException("value must be of type Customer.");
        }
    }
}
