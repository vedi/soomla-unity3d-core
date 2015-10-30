using UnityEngine;
using System.Collections;

namespace Soomla
{
	public class RewardGivenEvent
	{
		public readonly Reward reward;

		public RewardGivenEvent (string rewardId)
		{
			reward = Reward.GetReward(rewardId);
		}

		public RewardGivenEvent (Reward reward)
		{
			this.reward = reward;
		}
	}
}