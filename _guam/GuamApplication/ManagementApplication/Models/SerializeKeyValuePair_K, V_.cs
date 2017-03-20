using System;
using System.Runtime.CompilerServices;

namespace ManagementApplication.Models
{
	[Serializable]
	public struct SerializeKeyValuePair<K, V>
	{
		public K Key
		{
			get;
			set;
		}

		public V Value
		{
			get;
			set;
		}
	}
}