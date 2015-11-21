using System;
using System.Collections;
using System.Collections.Generic;

namespace Soomla
{
	public class CustomEvent : SoomlaEvent
	{
		private string mName;
		private Dictionary<String, String> mExtra;

		public CustomEvent(String name, Dictionary<String, String> extra) : this(name, extra, null)
        {
            
        }

		public CustomEvent (String name, Dictionary<String, String> extra, Object sender): base(sender)
		{
			mName = name;
			mExtra = extra;
		}
	
		public String GetName ()
		{
			return mName;
		}
	
		public Dictionary<String, String> GetExtra ()
		{
			return mExtra;
		}
	}
}
