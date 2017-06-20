using System;
using System.Collections;
using PubEntAdmin.DAL;

namespace PubEntAdmin.BLL
{

	[Serializable()]
    public class CISUserCollection:MultiSelectListBoxItemCollection {

        private static readonly string strCISUserItemType = "PubEntAdmin.BLL.CISUser";
        public new CISUser this[ int index ]  {
            get  { return( (CISUser) List[index] );}
            set  { List[index] = value;}
        }

        public int Add( CISUser value )  {
            return( List.Add( value ) );
        }

        public int IndexOf( CISUser value )  {
            return( List.IndexOf( value ) );
        }

        public void Insert( int index, CISUser value )  {
            List.Insert( index, value );
        }

        public void Remove( CISUser value )  {
            List.Remove( value );
        }

        public bool Contains( CISUser value )  {
            return( List.Contains( value ) );
        }

        protected override void OnInsert( int index, Object value )  {
            if ( value.GetType() != Type.GetType(strCISUserItemType) )
                throw new ArgumentException( "value must be of type CISUser.", "value" );
        }

        protected override void OnRemove( int index, Object value )  {
            if ( value.GetType() != Type.GetType(strCISUserItemType) )
                throw new ArgumentException( "value must be of type CISUser.", "value" );
        }

        protected override void OnSet( int index, Object oldValue, Object newValue )  {
            if ( newValue.GetType() != Type.GetType(strCISUserItemType) )
                throw new ArgumentException( "newValue must be of type CISUser.", "newValue" );
        }

        protected override void OnValidate( Object value )  {
            if ( value.GetType() != Type.GetType(strCISUserItemType) )
                throw new ArgumentException( "value must be of type CISUser." );
        }
  }
}
