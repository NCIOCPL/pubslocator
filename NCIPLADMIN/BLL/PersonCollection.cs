using System;
using System.Collections;
using PubEntAdmin.DAL;


namespace PubEntAdmin.BLL
{

    [Serializable]
    public class PersonCollection : CollectionBase
    {

        public Person this[int index]
        {
            get { return ((Person)List[index]); }
            set  { List[index] = value;}
        }

        public int Add(Person value)
        {
            return( List.Add( value ) );
        }

        public int IndexOf(Person value)
        {
            return( List.IndexOf( value ) );
        }

        public void Insert(int index, Person value)
        {
            List.Insert( index, value );
        }

        public void Remove(Person value)
        {
            List.Remove( value );
        }

        public bool Contains(Person value)
        {
            return( List.Contains( value ) );
        }

        protected override void OnInsert( int index, Object value )  {
            if (value.GetType() != Type.GetType("PubEntAdmin.BLL.Person"))
                throw new ArgumentException( "value must be of type Person.", "value" );
        }

        protected override void OnRemove( int index, Object value )  {
            if (value.GetType() != Type.GetType("PubEntAdmin.BLL.Person"))
                throw new ArgumentException( "value must be of type Person.", "value" );
        }

        protected override void OnSet( int index, Object oldValue, Object newValue )  {
            if (newValue.GetType() != Type.GetType("PubEntAdmin.BLL.Person"))
                throw new ArgumentException( "newValue must be of type Person.", "newValue" );
        }

        protected override void OnValidate( Object value )  {
            if (value.GetType() != Type.GetType("PubEntAdmin.BLL.Person"))
                throw new ArgumentException( "value must be of type Person." );
        }
  }
}
