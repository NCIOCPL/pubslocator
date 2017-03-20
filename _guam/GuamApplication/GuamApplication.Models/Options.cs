using System;
using System.Runtime.CompilerServices;

namespace GuamApplication.Models
{
	public class Options : Attribute
	{
		public int[] Items
		{
			get;
			set;
		}

		public Options(int[] items)
		{
			this.Items = items;
		}
	}
}