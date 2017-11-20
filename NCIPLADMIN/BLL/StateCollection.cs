using System;
using System.Collections;
using PubEntAdmin.DAL;

namespace PubEntAdmin.BLL 
{


	public class StateCollection:MultiSelectListBoxItemCollection 
	{

        private static readonly string strStateItemType = "PubEntAdmin.BLL.State";

		public new State this[ int index ]  
		{
			get  { return( (State) List[index] );}
			set  { List[index] = value;}
		}

		public int Add( State value )  
		{
			return( List.Add( value ) );
		}

		public int IndexOf( State value )  
		{
			return( List.IndexOf( value ) );
		}

		public void Insert( int index, State value )  
		{
			List.Insert( index, value );
		}

		public void Remove( State value )  
		{
			List.Remove( value );
		}

		public bool Contains( State value )  
		{
			return( List.Contains( value ) );
		}

        protected override void OnInsert(int index, Object value)  
		{
			if ( value.GetType() != Type.GetType(strStateItemType) )
				throw new ArgumentException( "value must be of type State.", "value" );
		}

		protected override void OnRemove( int index, Object value )  
		{
			if ( value.GetType() != Type.GetType(strStateItemType) )
				throw new ArgumentException( "value must be of type State.", "value" );
		}

		protected override void OnSet( int index, Object oldValue, Object newValue )  
		{
			if ( newValue.GetType() != Type.GetType(strStateItemType) )
				throw new ArgumentException( "newValue must be of type State.", "newValue" );
		}

		protected override void OnValidate( Object value )  
		{
			if ( value.GetType() != Type.GetType(strStateItemType))
				throw new ArgumentException( "value must be of type State." );
		}
	}
}
