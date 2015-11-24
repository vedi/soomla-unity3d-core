using System;
using System.Collections;
using System.Collections.Generic;

namespace Soomla
{
	public class SoomlaEvent
	{
        	public readonly Object Sender;
        	public readonly String Payload

        	public SoomlaEvent() { }
        	public SoomlaEvent(Object sender)
        	{
			Sender = sender;
        	}
		public SoomlaEvent(Object sender, String payload)
        	{
			this.Sender = sender;
			this.Payload = payload;
        	}
    	}
}
