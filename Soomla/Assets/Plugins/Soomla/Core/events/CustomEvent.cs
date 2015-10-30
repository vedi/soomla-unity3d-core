using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Soomla
{
	public class CustomEvent
	{
		private string mName;
		private Dictionary<string, string> mExtra;

		public CustomEvent (string name, Dictionary<string, string> extra)
		{
			mName = name;
			mExtra = extra;
		}
	
		public string GetName ()
		{
			return mName;
		}
	
		public Dictionary<string, string> GetExtra ()
		{
			return mExtra;
		}
	}
}
