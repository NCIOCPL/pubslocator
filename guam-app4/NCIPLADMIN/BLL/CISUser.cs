using System;
using System.Collections;
using PubEntAdmin.DAL;

namespace PubEntAdmin.BLL
{
	
/// <summary>
/// 
/// </summary>
	[Serializable()]
    public class CISUser : MultiSelectListBoxItem{

		#region PRIVATE FIELDS
        //private int			_Id;
        private string		_Login;
		private string		_Email;
		private string		_Role;
//		private string		_Name;
		private string		_RegionNo;
        private string strDisplayName;
        private string strLastName;
        private string _isactive;
        private string _isenabled;
        private string _islocked;
        private string _ispwdexpired;
		#endregion

		#region CONSTRUCTORS
		public CISUser(int id, string name) : base(id,name){;}

		public CISUser(int id, string login, string email, 
            string role, string name, string regionno, string lastname) 
            :base(id, name)
		{
			//_Id             = id;
			_Login          = login;
			_Email			= email;
			_Role			= role;
			//			this._Name		= name;
			_RegionNo		= regionno;
            this.strLastName = lastname;
		}      
		public CISUser(int id, string login, string email, string role, string name) :base(id, name)
		{
            //_Id             = id;
            _Login          = login;
			_Email			= email;
			_Role			= role;
//			this._Name		= name;
        }
		public CISUser(int id, string login, string name ) :base(id,name)
		{
			//_Id             = id;
			_Login          = login;
		}

		public CISUser (int id, string name, string email, string role):base(id, name)
		{
			this._Email = email;
			this._Role = role;
		}
        public CISUser(int id, string name, string email, string role, string isactive, string isenabled, string islocked, string ispwdexpired)
            : base(id, name)
        {
            _Email = email;
            _Role = role;
            _isactive = isactive;
            _isenabled = isenabled;
            _islocked = islocked;
            _ispwdexpired = ispwdexpired;
        }
		#endregion

		#region PROPERTIES
//        public int Id 
//		{
//            get {return _Id;}
//			set {_Id = value;}
//        }

		public string Login
		{
			get {return _Login;}
			set {_Login = value;}
		}
		public string Email 
		{
			get {return _Email;}
			set {_Email = value;}
		}
		public string Role
		{
			get {return _Role;}
			set {_Role = value;}
		}
		public string RegionNo
		{
			get {return _RegionNo;}
			set {_RegionNo = value;}
		}
        public string FirstName
        {
            get { return base.Name; }
            set { base.Name = value; }
        }
        public string LastName
        {
            get { return this.strLastName; }
            set { this.strLastName = value; }
        }
        public string FullName
        {
            get { return base.Name + " " +this.strLastName; }
        }
        public string IsActive
        {
            get { return _isactive; }
        }
        public string IsEnabled
        {
            get { return _isenabled; }
        }
        public string IsLocked
        {
            get { return _islocked; }
        }
        public string IsPwdExpired
        {
            get { return _ispwdexpired; }
        }
		#endregion

        //public static CISUserCollection GetCisUser(string s, string appId)
        //{
        //    return (DAL.GetCisUser(s, appId));
        //}

        public static CISUser GetCisUser(string s, int app)
        {
            //CR 11-001-36 return (PE_DAL.GetCisUser("%"+s+"%",app));
            return (PE_DAL.GetCisUser(s, app)); //CR-36 pass the username as received
        }
        public static CISUserCollection  GetGuamUsers(string s)
        {
            return (PE_DAL.GetGuamUsers(s)); 
        }
        public static CISUser GetGuamUserById(int userid)
        {
            return (PE_DAL.GetGuamUserById(userid));
        }
  }
}
