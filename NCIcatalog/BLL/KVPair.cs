using System;
using System.Collections;
//added
using NCICatalog.DAL;

namespace NCICatalog.BLL
{
	
/// <summary>
/// 
/// </summary>

    [Serializable]
    public class KVPair{

        /*** PRIVATE FIELDS ***/
        private string		_key;
		private string		_val;

        /*** CONSTRUCTORS ***/
		public KVPair()		//default constructor...
		{
		}
		public KVPair( string key, string val)
		{
            _key             = key;
            _val          = val;		
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

		public static KVPairCollection GetKVPair(string s) 
		{
            //DataAccess DbLayer = DataAccessBaseClassHelper.GetDataAccesLayer();
            return (GetKVPair(s));
		}



  }
}
