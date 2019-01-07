using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Character
{ 
public class SelftHealBehavior : AbilityBehavior
	{
		SelftHealConfig config;
		Player player;
		AudioSource audioSource = null;
		private void Start()
		{
			player = FindObjectOfType<Player>();
			audioSource = GetComponent<AudioSource>();
		}
		public override void Use(AbilityUseParams useParams)
		{
			player.Heal(config.GetExtraHeal());
			audioSource.Stop();
			audioSource.clip = config.GetAudioCLip();
			audioSource.Play();
			PlayParticalEffect();
		}

		private void PlayParticalEffect()
		{

			var prefab = GameObject.Instantiate(config.GetParticlePrefab(), transform.position, Quaternion.identity);
			prefab.transform.parent = this.transform;
			ParticleSystem VfxParticleSystem = prefab.GetComponent<ParticleSystem>();
			VfxParticleSystem.Play();
			GameObject.Destroy(prefab, VfxParticleSystem.main.duration);

		}

		public void setConfing(SelftHealConfig config)
		{
			this.config = config;
		}
	}
}
