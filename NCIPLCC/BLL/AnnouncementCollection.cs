using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
using System.Collections;
using PubEnt.DAL;

namespace PubEnt.BLL
{
    public class AnnouncementCollection:CollectionBase
    {
        public Announcement this[int index]
        {
            get { return ((Announcement)List[index]); }
            set { List[index] = value; }
        }

        public int Add(Announcement value)
        {
            return (List.Add(value));
        }

        public int IndexOf(Announcement value)
        {
            return (List.IndexOf(value));
        }

        public void Insert(int index, Announcement value)
        {
            List.Insert(index, value);
        }

        public void Remove(Announcement value)
        {
            List.Remove(value);
        }

        public bool Contains(Announcement value)
        {
            return (List.Contains(value));
        }

        protected override void OnInsert(int index, Object value)
        {
            if (value.GetType() != Type.GetType("PubEnt.BLL.Announcement"))
                throw new ArgumentException("value must be of type Announcement.", "value");
        }

        protected override void OnRemove(int index, Object value)
        {
            if (value.GetType() != Type.GetType("PubEnt.BLL.Announcement"))
                throw new ArgumentException("value must be of type Announcement.", "value");
        }

        protected override void OnSet(int index, Object oldValue, Object newValue)
        {
            if (newValue.GetType() != Type.GetType("PubEnt.BLL.Announcement"))
                throw new ArgumentException("newValue must be of type Announcement.", "newValue");
        }

        protected override void OnValidate(Object value)
        {
            if (value.GetType() != Type.GetType("PubEnt.BLL.Announcement"))
                throw new ArgumentException("value must be of type Announcement.");
        }
    }
}
