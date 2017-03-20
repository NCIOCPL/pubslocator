using System;
using System.Collections;
//using PubEnt.DAL;

namespace PubEntAdmin.BLL
{
	
/// <summary>
/// 
/// </summary>

    public class KVPair{

        /*** PRIVATE FIELDS ***/
        private string		_key;
		private string		_val;
        private string _isselected;

        /*** CONSTRUCTORS ***/
		public KVPair()		//default constructor...
		{
		}
		
        public KVPair( string key, string val, string isselected)
		{
            _key = key;
            _val = val;
            _isselected = isselected;
        }
        
        /*** PROPERTIES ***/
        public string Key 
		{
            get {return _key;}
			set {_key = value;}
        }

		public string Val
		{
			get {return _val;}
			set {_val = value;}
		}

        public String IsSelected
        {
            get { return _isselected; }
            set { _isselected = value; }
        }

        //public static KVPairCollection GetKVPair(string s) 
        //{
        //    //DataAccess DbLayer = DataAccessBaseClassHelper.GetDataAccesLayer();
        //    return (PubEnt.DAL.DAL.GetKVPair(s));
        //}
  }
}
