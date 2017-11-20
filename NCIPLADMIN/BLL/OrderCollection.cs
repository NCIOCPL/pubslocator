using System;
using System.Collections;
using PubEntAdmin.DAL;


namespace PubEntAdmin.BLL
{

    [Serializable]
    public class OrderCollection : CollectionBase
    {

        public Order this[int index]
        {
            get { return ((Order)List[index]); }
            set  { List[index] = value;}
        }

        public int Add(Order value)
        {
            return( List.Add( value ) );
        }

        public int IndexOf(Order value)
        {
            return( List.IndexOf( value ) );
        }

        public void Insert(int index, Order value)
        {
            List.Insert( index, value );
        }

        public void Remove(Order value)
        {
            List.Remove( value );
        }

        public bool Contains(Order value)
        {
            return( List.Contains( value ) );
        }

        protected override void OnInsert( int index, Object value )  {
            if (value.GetType() != Type.GetType("PubEntAdmin.BLL.Order"))
                throw new ArgumentException( "value must be of type Order.", "value" );
        }

        protected override void OnRemove( int index, Object value )  {
            if (value.GetType() != Type.GetType("PubEntAdmin.BLL.Order"))
                throw new ArgumentException("value must be of type Order.", "value");
        }

        protected override void OnSet( int index, Object oldValue, Object newValue )  {
            if (newValue.GetType() != Type.GetType("PubEntAdmin.BLL.Order"))
                throw new ArgumentException("newValue must be of type Order.", "newValue");
        }

        protected override void OnValidate( Object value )  {
            if (value.GetType() != Type.GetType("PubEntAdmin.BLL.Order"))
                throw new ArgumentException("value must be of type Order.");
        }
        public static OrderCollection GetRepeatOrders(string s)
        {
            return (PE_DAL.GetRepeatOrders(s));
        }
        public static OrderCollection GetPrankOrders(string s)
        {
            return (PE_DAL.GetPrankOrders(s));
        }
  }
}
