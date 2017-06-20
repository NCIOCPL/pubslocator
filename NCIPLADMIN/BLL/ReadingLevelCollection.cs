using System;
using System.Collections;
using PubEntAdmin.DAL;

namespace PubEntAdmin.BLL
{


    public class ReadingLevelCollection : MultiSelectListBoxItemCollection 
	{

        private static readonly string strReadingLevelItemType = "PubEntAdmin.BLL.ReadingLevel";

        public new ReadingLevel this[int index]  
		{
            get { return ((ReadingLevel)List[index]); }
			set  { List[index] = value;}
		}

        public int Add(ReadingLevel value)  
		{
			return( List.Add( value ) );
		}

        public int IndexOf(ReadingLevel value)  
		{
			return( List.IndexOf( value ) );
		}

        public void Insert(int index, ReadingLevel value)  
		{
			List.Insert( index, value );
		}

        public void Remove(ReadingLevel value)  
		{
			List.Remove( value );
		}

        public bool Contains(ReadingLevel value)  
		{
			return( List.Contains( value ) );
		}

		protected override void OnInsert( int index, Object value )  
		{
            if (value.GetType() != Type.GetType(strReadingLevelItemType))
                throw new ArgumentException("value must be of type Reading Level.", "value");
		}

		protected override void OnRemove( int index, Object value )  
		{
            if (value.GetType() != Type.GetType(strReadingLevelItemType))
                throw new ArgumentException("value must be of type Reading Level.", "value");
		}

		protected override void OnSet( int index, Object oldValue, Object newValue )  
		{
            if (newValue.GetType() != Type.GetType(strReadingLevelItemType))
                throw new ArgumentException("newValue must be of type Reading Level.", "newValue");
		}

		protected override void OnValidate( Object value )  
		{
            if (value.GetType() != Type.GetType(strReadingLevelItemType))
                throw new ArgumentException("value must be of type Reading Level.");
		}
	}
}
