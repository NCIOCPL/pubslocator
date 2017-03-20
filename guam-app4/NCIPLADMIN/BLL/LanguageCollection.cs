using System;
using System.Collections;
using PubEntAdmin.DAL;

namespace PubEntAdmin.BLL
{


	public class LanguageCollection:MultiSelectListBoxItemCollection 
	{

        private static readonly string strLanguageItemType = "PubEntAdmin.BLL.Language";

		public new Language this[ int index ]  
		{
			get  { return( (Language) List[index] );}
			set  { List[index] = value;}
		}

        public int Add(Language value)  
		{
			return( List.Add( value ) );
		}

        public int IndexOf(Language value)  
		{
			return( List.IndexOf( value ) );
		}

        public void Insert(int index, Language value)  
		{
			List.Insert( index, value );
		}

        public void Remove(Language value)  
		{
			List.Remove( value );
		}

        public bool Contains(Language value)  
		{
			return( List.Contains( value ) );
		}

		protected override void OnInsert( int index, Object value )  
		{
            if (value.GetType() != Type.GetType(strLanguageItemType))
                throw new ArgumentException("value must be of type Language.", "value");
		}

		protected override void OnRemove( int index, Object value )  
		{
            if (value.GetType() != Type.GetType(strLanguageItemType))
                throw new ArgumentException("value must be of type Language.", "value");
		}

		protected override void OnSet( int index, Object oldValue, Object newValue )  
		{
            if (newValue.GetType() != Type.GetType(strLanguageItemType))
                throw new ArgumentException("newValue must be of type Language.", "newValue");
		}

		protected override void OnValidate( Object value )  
		{
            if (value.GetType() != Type.GetType(strLanguageItemType))
                throw new ArgumentException("value must be of type Language.");
		}
	}
}
