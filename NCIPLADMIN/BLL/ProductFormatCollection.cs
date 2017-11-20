using System;
using System.Collections;
using PubEntAdmin.DAL;

namespace PubEntAdmin.BLL
{


	public class ProductFormatCollection:MultiSelectListBoxItemCollection 
	{

        private static readonly string strProductFormatItemType = "PubEntAdmin.BLL.ProductFormat";

        public new ProductFormat this[int index]  
		{
            get { return ((ProductFormat)List[index]); }
			set  { List[index] = value;}
		}

        public int Add(ProductFormat value)  
		{
			return( List.Add( value ) );
		}

        public int IndexOf(ProductFormat value)  
		{
			return( List.IndexOf( value ) );
		}

        public void Insert(int index, ProductFormat value)  
		{
			List.Insert( index, value );
		}

        public void Remove(ProductFormat value)  
		{
			List.Remove( value );
		}

        public bool Contains(ProductFormat value)  
		{
			return( List.Contains( value ) );
		}

		protected override void OnInsert( int index, Object value )  
		{
            if (value.GetType() != Type.GetType(strProductFormatItemType))
                throw new ArgumentException("value must be of type Publication Format.", "value");
		}

		protected override void OnRemove( int index, Object value )  
		{
            if (value.GetType() != Type.GetType(strProductFormatItemType))
                throw new ArgumentException("value must be of type Publication Format.", "value");
		}

		protected override void OnSet( int index, Object oldValue, Object newValue )  
		{
            if (newValue.GetType() != Type.GetType(strProductFormatItemType))
                throw new ArgumentException("newValue must be of type Publication Format.", "newValue");
		}

		protected override void OnValidate( Object value )  
		{
            if (value.GetType() != Type.GetType(strProductFormatItemType))
                throw new ArgumentException("value must be of type Publication Format.");
		}
	}
}
