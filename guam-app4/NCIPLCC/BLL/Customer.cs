using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PubEnt.BLL
{
    public class Customer
    {
        /*** PRIVATE FIELDS ***/
        private int _CustID;
        private string _ShipToName = "";
        private string _ShipToOrg = "";
        private string _ShipToAddr1 = "";
        private string _ShipToAddr2 = "";
        private string _ShipToCity = "";
        private string _ShipToState = "";
        private string _ShipToZip5 = "";
        private string _ShipToZip4 = "";
        private string _ShipToPhone = "";
        private string _ShipToFax = "";
        private string _ShipToEmail = "";
        private string _ShipToProvince = "";
        private string _ShipToCountry = "";

        private string _BillToName = "";
        private string _BillToOrg = "";
        private string _BillToAddr1 = "";
        private string _BillToAddr2 = "";
        private string _BillToCity = "";
        private string _BillToState = "";
        private string _BillToZip5 = "";
        private string _BillToZip4 = "";
        private string _BillToPhone = "";
        private string _BillToFax = "";
        private string _BillToEmail = "";
        private string _BillToProvince = "";
        private string _BillToCountry = "";

        /*** CONSTRUCTORS ***/
		public Customer()		//default constructor...
		{
		}

        public Customer(int CustID,
            string ShipToName,
            string ShipToOrg,
            string ShipToAddr1,
            string ShipToAddr2,
            string ShipToCity,
            string ShipToState,
            string ShipToZip5,
            string ShipToZip4,
            string ShipToPhone,
            string ShipToFax,
            string ShipToEmail,
            string ShipToProvince,
            string ShipToCountry) 
		{
            _CustID = CustID;
            _ShipToName = ShipToName;
            _ShipToOrg = ShipToOrg;
            _ShipToAddr1 = ShipToAddr1;
            _ShipToAddr2 = ShipToAddr2;
            _ShipToCity = ShipToCity;
            _ShipToState = ShipToState;
            _ShipToZip5 = ShipToZip5;
            _ShipToZip4 = ShipToZip4;
            _ShipToPhone = ShipToPhone;
            _ShipToFax = ShipToFax;
            _ShipToEmail = ShipToEmail;
            _ShipToProvince = ShipToProvince;
            _ShipToCountry = ShipToCountry;
		}

        public Customer(int CustID,
            string ShipToName,
            string ShipToOrg,
            string ShipToAddr1,
            string ShipToAddr2,
            string ShipToCity,
            string ShipToState,
            string ShipToZip5,
            string ShipToZip4,
            string ShipToPhone,
            string ShipToFax,
            string ShipToEmail,
            string ShipToProvince,
            string ShipToCountry,
            string BillToName,
            string BillToOrg,
            string BillToAddr1,
            string BillToAddr2,
            string BillToCity,
            string BillToState,
            string BillToZip5,
            string BillToZip4,
            string BillToPhone,
            string BillToFax,
            string BillToEmail,
            string BillToProvince,
            string BillToCountry)
        {
            _CustID = CustID;
            _ShipToName = ShipToName;
            _ShipToOrg = ShipToOrg;
            _ShipToAddr1 = ShipToAddr1;
            _ShipToAddr2 = ShipToAddr2;
            _ShipToCity = ShipToCity;
            _ShipToState = ShipToState;
            _ShipToZip5 = ShipToZip5;
            _ShipToZip4 = ShipToZip4;
            _ShipToPhone = ShipToPhone;
            _ShipToFax = ShipToFax;
            _ShipToEmail = ShipToEmail;
            _ShipToProvince = ShipToProvince;
            _ShipToCountry = ShipToCountry;

            _BillToName = BillToName;
            _BillToOrg = BillToOrg;
            _BillToAddr1 = BillToAddr1;
            _BillToAddr2 = BillToAddr2;
            _BillToCity = BillToCity;
            _BillToState = BillToState;
            _BillToZip5 = BillToZip5;
            _BillToZip4 = BillToZip4;
            _BillToPhone = BillToPhone;
            _BillToFax = BillToFax;
            _BillToEmail = BillToEmail;
            _BillToProvince = BillToProvince;
            _BillToCountry = BillToCountry;
        }

        /*** PROPERTIES ***/
        public int CustID
        {
            get { return _CustID; }
            set { _CustID = value; }
        }

        public string ShipToName
        {
            get { return _ShipToName; }
            set { _ShipToName = value; }
        }
        public string ShipToOrg
        {
            get { return _ShipToOrg; }
            set { _ShipToOrg = value; }
        }
        public string ShipToAddr1
        {
            get { return _ShipToAddr1; }
            set { _ShipToAddr1 = value; }
        }
        public string ShipToAddr2
        {
            get { return _ShipToAddr2; }
            set { _ShipToAddr2 = value; }
        }
        public string ShipToCity
        {
            get { return _ShipToCity; }
            set { _ShipToCity = value; }
        }
        public string ShipToState
        {
            get { return _ShipToState; }
            set { _ShipToState = value; }
        }
        public string ShipToZip5
        {
            get { return _ShipToZip5; }
            set { _ShipToZip5 = value; }
        }
        public string ShipToZip4
        {
            get { return _ShipToZip4; }
            set { _ShipToZip4 = value; }
        }
        public string ShipToPhone
        {
            get { return _ShipToPhone; }
            set { _ShipToPhone = value; }
        }
        public string ShipToFax
        {
            get { return _ShipToFax; }
            set { _ShipToFax = value; }
        }
        public string ShipToEmail
        {
            get { return _ShipToEmail; }
            set { _ShipToEmail = value; }
        }
        public string ShipToProvince
        {
            get { return _ShipToProvince; }
            set { _ShipToProvince = value; }
        }
        public string ShipToCountry
        {
            get { return _ShipToCountry; }
            set { _ShipToCountry = value; }
        }

        public string BillToName
        {
            get { return _BillToName; }
            set { _BillToName = value; }
        }
        public string BillToOrg
        {
            get { return _BillToOrg; }
            set { _BillToOrg = value; }
        }
        public string BillToAddr1
        {
            get { return _BillToAddr1; }
            set { _BillToAddr1 = value; }
        }
        public string BillToAddr2
        {
            get { return _BillToAddr2; }
            set { _BillToAddr2 = value; }
        }
        public string BillToCity
        {
            get { return _BillToCity; }
            set { _BillToCity = value; }
        }
        public string BillToState
        {
            get { return _BillToState; }
            set { _BillToState = value; }
        }
        public string BillToZip5
        {
            get { return _BillToZip5; }
            set { _BillToZip5 = value; }
        }
        public string BillToZip4
        {
            get { return _BillToZip4; }
            set { _BillToZip4 = value; }
        }
        public string BillToPhone
        {
            get { return _BillToPhone; }
            set { _BillToPhone = value; }
        }
        public string BillToFax
        {
            get { return _BillToFax; }
            set { _BillToFax = value; }
        }
        public string BillToEmail
        {
            get { return _BillToEmail; }
            set { _BillToEmail = value; }
        }
        public string BillToProvince
        {
            get { return _BillToProvince; }
            set { _BillToProvince = value; }
        }
        public string BillToCountry
        {
            get { return _BillToCountry; }
            set { _BillToCountry = value; }
        }
    }
}
