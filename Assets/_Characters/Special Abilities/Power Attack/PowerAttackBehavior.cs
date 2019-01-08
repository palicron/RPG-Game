using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Character
{

	public class PowerAttackBehavior : AbilityBehavior
	{
	

		public override void Use(AbilityUseParams useParams)
		{
			useParams.target.TakeDamage(useParams.baseDamage + (config as PowerAttackConfig).GetExtraDamage());
			PlayParticalEffect(transform.position,true);
		}
	

		public void setConfing(PowerAttackConfig config)
		{
			this.config = config;
		}
	}

}
