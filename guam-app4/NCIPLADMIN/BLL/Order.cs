using System;
using System.Collections;
using PubEntAdmin.DAL;
using PubEntAdmin.BLL;


namespace PubEntAdmin.BLL
{
	
/// <summary>
/// 
/// </summary>

    [Serializable]
    public class Order{

        /*** PRIVATE FIELDS ***/
        private int _orderid;
        private Person _shipto;
        private Person _billto;
        private ProductCollection _cart;
        private DateTime _datecreated;
        private string _termcode;
        private int _repeatid;
        private string _ordercomment;
        private string _shipmethod;

        /*** CONSTRUCTORS ***/
        public Order()		//default constructor...
        {
        }

        public Order(int orderid, Person shipto, Person billto, DateTime datecreated, 
                    string termcode, int repeatid, string ordercomment, string shipmethod)
        {
            _orderid = orderid;
            _shipto = shipto;
            _billto = billto;
            _datecreated = datecreated;
            _termcode = termcode;
            _repeatid = repeatid;
            _ordercomment = ordercomment;
            _shipmethod = shipmethod;
        }

//        public bool Save(string s, string pubids, string pubqtys, out int returnvalue, out int returnordernum)
//        {
//            return (PubEnt.DAL2.DAL.SaveNewOrder(this, s, pubids, pubqtys, out returnvalue, out returnordernum));
//        }

        public int OrderId
        {
            get { return _orderid; }
            set { _orderid = value; }
        }
        public DateTime DateCreated
        {
            get { return _datecreated ; }
            set { _datecreated = value; }
        }
        public string TermCode
        {
            get { return _termcode ; }
            set { _termcode = value; }
        }
        public int RepeatID
        {
            get { return _repeatid ; }
            set { _repeatid = value; }
        }
        public string OrderComment
        {
            get { return _ordercomment ; }
            set { _ordercomment = value; }
        }
        public string ShipMethod
        {
            get { return _shipmethod ; }
            set { _shipmethod = value; }
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

        public ProductCollection  Cart
        {
            get { return _cart ; }
            set { _cart = value; }
        }

        public string ShipToName //***EAC Redundant...Added specifically to databind the shiptoname field in the orderheld.aspx gridview
        {
            get { return this.ShipTo.Fullname; }
        }
        public static Order GetOrderByOrderID(int orderid)
        {
            return (PE_DAL.GetOrderByOrderID(orderid));
        }
        public static void DeleteOrder(int orderid, string who, out string returnmsg)
        {
            PE_DAL.DeleteOrder(orderid, who, out returnmsg );
        }
        public static void ReleaseOrder(int orderid, string who, out string returnmsg)
        {
            PE_DAL.ReleaseOrder(orderid, who, out returnmsg);
        }
        public static void MarkOrderBad(int orderid, string who, string reason, out string returnmsg)
        {
            PE_DAL.MarkOrderBad(orderid, who, reason, out returnmsg);
        }
  }
}
