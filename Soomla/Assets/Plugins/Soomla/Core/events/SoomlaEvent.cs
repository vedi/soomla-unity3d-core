using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Soomla
{
	public class SoomlaEvent
	{
        public readonly Object Sender;

        public SoomlaEvent() { }
        public SoomlaEvent(Object sender)
        {
            Sender = sender;
        }
    }
}
