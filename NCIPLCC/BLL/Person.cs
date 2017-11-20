using System;
using System.Collections;
using PubEnt.DAL;
using PubEnt.BLL;


namespace PubEnt {
	
/// <summary>
/// 
/// </summary>
    [Serializable]
    public class Person {

        /*** PRIVATE FIELDS ***/
        private int _Id;
        private string _Fullname = "";
        private string _Org = "";
        private string _Email = "";
        private string _Addr1 = "";
        private string _Addr2 = "";
        private string _City = "";
        private string _State = "";
        private string _Zip5 = "";
        private string _Zip4 = "";
        private string _Phone = "";
        private string _Firstname = "";
        private string _Lastname = "";
        private string _Country = "";

        /*** CONSTRUCTORS ***/
		public Person()		//default constructor...
		{
		}
        public Person(         int Id,
         string Fullname,
         string Org,
         string Email,
         string Addr1,
         string Addr2,
         string City,
         string State,
         string Zip5,
         string Zip4,
         string Phone) 
		{
        _Id = Id;
        _Fullname = Fullname;
        _Org = Org;
        _Email = Email ;
        _Addr1 = Addr1;
         _Addr2 = Addr2;
        _City = City;
          _State = State;
          _Zip5=Zip5 ;
          _Zip4=Zip4 ;
          _Phone=Phone ;
          _Country = "US";  //***EAC Assume 'US' if no country provided
		}


        public Person(int Id,
         string Fullname,
         string Org,
         string Email,
         string Addr1,
         string Addr2,
         string City,
         string State,
         string Zip5,
         string Zip4,
         string Phone,
         string Country)
        {
            _Id = Id;
            _Fullname = Fullname;
            _Org = Org;
            _Email = Email;
            _Addr1 = Addr1;
            _Addr2 = Addr2;
            _City = City;
            _State = State;
            _Zip5 = Zip5;
            _Zip4 = Zip4;
            _Phone = Phone;
            _Country = Country;
        }

        /*** PROPERTIES ***/
        public int Id 
		{
            get {return _Id;}
			set {_Id = value;}
        }

		public string Fullname
		{
            get { return _Fullname; }
            set { _Fullname = value; }
		}
        public string Organization
        {
            get { return _Org; }
            set { _Org = value; }
        }
        public string Addr1
        {
            get { return _Addr1; }
            set { _Addr1 = value; }
        }
        public string Addr2
        {
            get { return _Addr2; }
            set { _Addr2 = value; }
        }
        public string City
        {
            get { return _City; }
            set { _City = value; }
        }
        public string State
        {
            get { return _State; }
            set { _State = value; }
        }
        public string Zip5
        {
            get { return _Zip5; }
            set { _Zip5 = value; }
        }
        public string Zip4
        {
            get { return _Zip4; }
            set { _Zip4 = value; }
        }
        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }
        public string Phone
        {
            get { return _Phone; }
            set { _Phone = value; }
        }
        public string Firstname
        {
            get { return _Firstname ; }
            set { _Firstname = value; }
        }
        public string Lastname
        {
            get { return _Lastname ; }
            set { _Lastname = value; }
        }
        public string Country
        {
            get { return _Country; }
            set { _Country = value; }
        }        
        public static PersonCollection GetCisUser(string s, string appId) 
		{
			//return (DAL.GetCisUser(s,appId));
            return null;
		}

        public static Person GetCisUserbyId(int id) 
		{
			//return (DAL.GetCisUserbyId(id));
            return null;
		}

  }
}
