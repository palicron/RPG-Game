using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Character
{ 
public class SelftHealBehavior : AbilityBehavior
	{
	
		Player player;

		private void Start()
		{
			player = FindObjectOfType<Player>();
		
		}
		public override void Use(AbilityUseParams useParams)
		{
			player.Heal((config as SelftHealConfig).GetExtraHeal());
			PlayAbilitySound();
			PlayParticalEffect(transform.position,true);
		}



		public void setConfing(SelftHealConfig config)
		{
			this.config = config;
		}
	}
}
