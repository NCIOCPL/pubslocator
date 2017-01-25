using System;
using System.Collections;
using PubEntAdmin.DAL;

namespace PubEntAdmin.BLL
{
	
	/// <summary>
	/// 
	/// </summary>

	public class State  : MultiSelectListBoxItem
	{

		private string abbr;

		public State(int id , string name, string abbr):base(id,name)
		{	
			this.abbr = abbr;
		}

		public int StateID 
		{
			get {return this.ID;}
			set {this.ID = value;}
		}

		public string StateName
		{
			get {return this.Name;}
			set {this.Name = value;}
		}

		public string StateAbbr
		{
			get {return this.abbr;}
			set {this.abbr = value;}
		}

        //public static StateCollection GetStates()
        //{
        //    return (DAL.PE_DAL.GetState());
        //}
	}
}
