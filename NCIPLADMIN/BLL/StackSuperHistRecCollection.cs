using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//added
using System.Collections;
using PubEntAdmin.DAL;
namespace PubEntAdmin.BLL
{
    [Serializable]
    public class StackSuperHistRecCollection : CollectionBase
    {
        public StackHistRecCollection this[int index]
        {
            get { return ((StackHistRecCollection)List[index]); }
            set { List[index] = value; }
        }

        public int Add(StackHistRecCollection value)
        {
            return (List.Add(value));
        }
        public bool Contains(StackHistRecCollection value)
        {
            return (List.Contains(value));
        }
        public void Remove(StackHistRecCollection value)
        {
            List.Remove(value);
        }
    }
}
