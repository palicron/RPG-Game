using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Character
{

	public class PowerAttackBehavior : AbilityBehavior
	{
	

		public override void Use(GameObject target)
		{
			target.GetComponent<HealthSystem>().TakeDamage( (config as PowerAttackConfig).GetExtraDamage());
			PlayParticalEffect(transform.position,true);
			PlayAbilitySound();
			PlayAbilityAnimation();
		}
	

		public void setConfing(PowerAttackConfig config)
		{
			this.config = config;
		}
	}

}
