using System;
using System.Collections;
using PubEntAdmin.DAL;

namespace PubEntAdmin.BLL
{


	public class CancerTypeCollection:MultiSelectListBoxItemCollection 
	{

        private static readonly string strCancerTypeItemType = "PubEntAdmin.BLL.CancerType";

		public new CancerType this[ int index ]  
		{
			get  { return( (CancerType) List[index] );}
			set  { List[index] = value;}
		}

		public int Add( CancerType value )  
		{
			return( List.Add( value ) );
		}

		public int IndexOf( CancerType value )  
		{
			return( List.IndexOf( value ) );
		}

		public void Insert( int index, CancerType value )  
		{
			List.Insert( index, value );
		}

		public void Remove( CancerType value )  
		{
			List.Remove( value );
		}

		public bool Contains( CancerType value )  
		{
			return( List.Contains( value ) );
		}

		protected override void OnInsert( int index, Object value )  
		{
			if ( value.GetType() != Type.GetType(strCancerTypeItemType) )
				throw new ArgumentException( "value must be of type CancerType.", "value" );
		}

		protected override void OnRemove( int index, Object value )  
		{
			if ( value.GetType() != Type.GetType(strCancerTypeItemType) )
				throw new ArgumentException( "value must be of type CancerType.", "value" );
		}

		protected override void OnSet( int index, Object oldValue, Object newValue )  
		{
			if ( newValue.GetType() != Type.GetType(strCancerTypeItemType) )
				throw new ArgumentException( "newValue must be of type CancerType.", "newValue" );
		}

		protected override void OnValidate( Object value )  
		{
			if ( value.GetType() != Type.GetType(strCancerTypeItemType))
				throw new ArgumentException( "value must be of type CancerType." );
		}
	}
}
