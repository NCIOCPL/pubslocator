using System;
using System.Collections;
using NCIPLex.DAL;
using NCIPLex.BLL;


namespace NCIPLex
{
	
/// <summary>
/// 
/// </summary>

    [Serializable]
    public class Transaction{

        /*** PRIVATE FIELDS ***/
        private Person _shipto;
        private Person _billto;
        private CreditCard _cc;
        private ProductCollection _cart;


        /*** CONSTRUCTORS ***/
        public Transaction()		//default constructor...
        {
        }

		public Transaction(Person shipto, Person billto, CreditCard cc, ProductCollection cart)		//default constructor...
		{
            _shipto = shipto;
            _billto = billto;
            _cc = cc;
            _cart = cart;
		}

        public bool Save(string s, string pubids, string pubqtys, int confid, string shiplocation, out int returnvalue, out int returnordernum)
        {
            return (SQLDataAccess.SaveNewOrder(this, confid, s, pubids, pubqtys, out returnvalue, out returnordernum));
        }

        public Person ShipTo
        {
            get { return _shipto; }
            set { _shipto = value; }
        }
        public Person BillTo
        {
            get { return _billto; }
            set { _billto = value; }
        }

        //JPJ 03-10-11 NCIPLex does not accept credit card orders
        //public CreditCard  CC
        //{
        //    get { return _cc; }
        //    set { _cc = value; }
        //}

        public ProductCollection  Cart
        {
            get { return _cart ; }
            set { _cart = value; }
        }

  }
}
