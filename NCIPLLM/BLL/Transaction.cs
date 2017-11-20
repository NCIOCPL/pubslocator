using System;
using System.Collections;
using PubEnt.DAL;
using PubEnt.BLL;


namespace PubEnt {
	
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

        public bool Save(string s, string pubids, string pubqtys, out int returnvalue, out int returnordernum)
        {
            return (PubEnt.DAL2.DAL.SaveNewOrder(this, s, pubids, pubqtys, out returnvalue, out returnordernum));
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
        public CreditCard  CC
        {
            get { return _cc; }
            set { _cc = value; }
        }
        public ProductCollection  Cart
        {
            get { return _cart ; }
            set { _cart = value; }
        }

  }
}
