using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Character
{ 
public class SelftHealBehavior : AbilityBehavior
	{
	
		Player player;
		AudioSource audioSource = null;
		private void Start()
		{
			player = FindObjectOfType<Player>();
			audioSource = GetComponent<AudioSource>();
		}
		public override void Use(AbilityUseParams useParams)
		{
			player.Heal((config as SelftHealConfig).GetExtraHeal());
			audioSource.Stop();
			audioSource.clip = config.GetAudioCLip();
			audioSource.Play();
			PlayParticalEffect(transform.position,true);
		}



		public void setConfing(SelftHealConfig config)
		{
			this.config = config;
		}
	}
}
