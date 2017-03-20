using System;
using System.Collections;
using PubEntAdmin.DAL;

namespace PubEntAdmin.BLL
{
	/// <summary>
	/// Summary description for OrgType.
	/// </summary>
	public class CancerType  : MultiSelectListBoxItem
    {
        #region Fields
        private bool blnChecked;
        #endregion

        #region Constructors
        public CancerType(int id , string name):base(id,name)
		{
			
		}

        public CancerType(int id, string name, bool _checked)
            : base(id, name)
        {
            blnChecked = _checked;
        }
        #endregion

        #region Methods
        public static bool SetCancer(int pubid, string selectedValue, char delim)
        {
            return PE_DAL.SetCancerTypeByPubID(pubid, selectedValue, delim);
        }
        #endregion

        #region Properties
        public int CancerTypeID 
		{
			get {return this.ID;}
			set {this.ID = value;}
		}

		public string CancerTypeName
		{
			get {return this.Name;}
			set {this.Name = value;}
        }
        public bool Checked
        {
            get { return this.blnChecked; }
            set { this.blnChecked = value; }
        }
        #endregion

        //public static CancerTypeCollection GetCancerType(string pubDesc)
        //{
        //    return (DAL.PE_DAL.GetCancerType(pubDesc));
        //}
	}
}
