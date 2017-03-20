using System;
using System.Collections;
using PubEnt.DAL;

namespace PubEnt.BLL
{


    public class KVPairCollection:CollectionBase {

        public KVPair this[ int index ]  {
            get  { return( (KVPair) List[index] );}
            set  { List[index] = value;}
        }

        public int Add( KVPair value )  {
            return( List.Add( value ) );
        }

        public int IndexOf( KVPair value )  {
            return( List.IndexOf( value ) );
        }

        public void Insert( int index, KVPair value )  {
            List.Insert( index, value );
        }

        public void Remove( KVPair value )  {
            List.Remove( value );
        }

        public bool Contains( KVPair value )  {
            return( List.Contains( value ) );
        }

        protected override void OnInsert( int index, Object value )  {
            if ( value.GetType() != Type.GetType("PubEnt.BLL.KVPair") )
                throw new ArgumentException( "value must be of type KVPair.", "value" );
        }

        protected override void OnRemove( int index, Object value )  {
            if ( value.GetType() != Type.GetType("PubEnt.BLL.KVPair") )
                throw new ArgumentException( "value must be of type KVPair.", "value" );
        }

        protected override void OnSet( int index, Object oldValue, Object newValue )  {
            if ( newValue.GetType() != Type.GetType("PubEnt.BLL.KVPair") )
                throw new ArgumentException( "newValue must be of type KVPair.", "newValue" );
        }

        protected override void OnValidate( Object value )  {
            if ( value.GetType() != Type.GetType("PubEnt.BLL.KVPair") )
                throw new ArgumentException( "value must be of type KVPair." );
        }
  }
}
