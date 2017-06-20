using System;
using System.Collections;

namespace NCIPLex
{
	
/// <summary>
/// 
/// </summary>

    [Serializable]
    public class CreditCard {

        /*** PRIVATE FIELDS ***/
        private string _Company = "";
        private string _CCnum = "";
        private string _ExpMon = "";
        private string _ExpYr = "";
        private string _CVV2 = "";
        private double _Cost = 0.0;
        private string _TransID = "";
        private string _ApprovalCode = "";
        private string _CCType = "";

        /*** CONSTRUCTORS ***/
		public CreditCard()		//default constructor...
		{
            _Company = "";
            _CCnum = "";
            _ExpMon = "";
            _ExpYr = "";
            _CVV2 = "";
            _Cost = 0.0;
		}
        public CreditCard(string Company, string CCnum,
         string ExpMon, string ExpYr,
         string CVV2, double Cost, string CCType)
		{
            _Company = Company;
            _CCnum = CCnum;
            _ExpMon = ExpMon;
            _ExpYr = ExpYr;
            _CVV2 = CVV2;
            _Cost = Cost;
            _CCType = CCType;
		}

        public double Cost
        {
            get { return _Cost; }
            set { _Cost = value; }
        }
        public string Company
		{
            get { return _Company; }
            set { _Company = value; }
		}
        public string CompanyText
        {
            get {
                switch (_Company)
                {
                    case "V": { return "Visa"; break; }
                    case "M": { return "Mastercard"; break; }
                    case "A": { return "American Express"; break; }
                    default: return "";
                        break;
                }
            }
        }

        public string CCnum
        {
            get { return _CCnum; }
            set { _CCnum = value; }
        }
        public string ExpMon
        {
            get { return _ExpMon ; }
            set { _ExpMon = value; }
        }
        public string ExpYr
        {
            get { return _ExpYr; }
            set { _ExpYr = value; }
        }
        public string CVV2
        {
            get { return _CVV2; }
            set { _CVV2 = value; }
        }
        public string TransID
        {
            get { return _TransID; }
            set { _TransID = value; }
        }
        public string ApprovalCode
        {
            get { return _ApprovalCode; }
            set { _ApprovalCode = value; }
        }
        public string CCType
        {
            get { return _CCType; }
            set { _CCType = value; }
        }
  }
}
