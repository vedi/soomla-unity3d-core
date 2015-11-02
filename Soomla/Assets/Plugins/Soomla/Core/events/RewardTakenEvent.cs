using UnityEngine;
using System.Collections;

namespace Soomla
{
	public class RewardTakenEvent : SoomlaEvent
	{
		public readonly Reward mReward;

		public RewardTakenEvent (string rewardId) : this(rewardId, null)
		{

		}

		public RewardTakenEvent (Reward reward) : this(reward, null)
        {

		}

        public RewardTakenEvent(string rewardId, Object sender) : base(sender)
        {
            mReward = Reward.GetReward(rewardId);
        }

        public RewardTakenEvent(Reward reward, Object sender) : base(sender)
        {
            mReward = reward;
        }
    }
}