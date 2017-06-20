using System;
using System.Collections;
using NCIPLex.DAL2;

namespace NCIPLex.BLL
{
	
/// <summary>
/// 
/// </summary>

    public class Zipcode{

        /*** PRIVATE FIELDS ***/
        private string		_zip5;
		private string		_zip4;
        private string      _city;
        private string      _state;

        /*** CONSTRUCTORS ***/
		public Zipcode()		//default constructor...
		{
		}
		public Zipcode(string zip5, string zip4, string city, string state)
		{
            _zip5 = zip5;
            _zip4 = zip4;
            _city = city;
            _state = state;
        }
        /*** PROPERTIES ***/
        public string City 
		{
            get {return _city;}
            set {_city = value; }
        }

		public string State
		{
			get {return _state;}
            set {_state = value;}
		}
        public string Zip4
        {
            get { return _zip4; }
            set { _zip4 = value; }
        }
        public static Zipcode GetCSZ(int z)
        {
            return (NCIPLex.DAL2.DAL.GetCSZ(z));
        }
        public static KVPairCollection GetCities(int z)
        {
            return (NCIPLex.DAL2.DAL.GetCities(z));
        }

  }
}
