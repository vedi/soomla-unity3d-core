using UnityEngine;
using System.Collections;

namespace Soomla
{
	public class RewardGivenEvent : SoomlaEvent
	{
		public readonly Reward reward;

		public RewardGivenEvent (string rewardId) : this(rewardId, null)
		{

		}

		public RewardGivenEvent (Reward reward) : this(reward, null)
        {

		}

        public RewardGivenEvent(string rewardId, Object sender) : base(sender)
        {
            reward = Reward.GetReward(rewardId);
        }

        public RewardGivenEvent(Reward reward, Object sender) : base(sender)
        {
            this.reward = reward;
        }
    }
}