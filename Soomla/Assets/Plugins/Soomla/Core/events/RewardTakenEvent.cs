using UnityEngine;
using System.Collections;

namespace Soomla
{
	public class RewardTakenEvent
	{
		public readonly Reward reward;

		public RewardTakenEvent (string rewardId)
		{
			reward = Reward.GetReward(rewardId);
		}

		public RewardTakenEvent (Reward reward)
		{
			this.reward = reward;
		}
	}
}