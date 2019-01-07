using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Character
{

	public class PowerAttackBehavior : AbilityBehavior
	{
		PowerAttackConfig config;


		public override void Use(AbilityUseParams useParams)
		{
			useParams.target.TakeDamage(useParams.baseDamage + config.GetExtraDamage());
			PlayParticalEffect();
		}
		private void PlayParticalEffect()
		{

			var prefab = GameObject.Instantiate(config.GetParticlePrefab(), transform.position, Quaternion.identity);
			ParticleSystem VfxParticleSystem = prefab.GetComponent<ParticleSystem>();
			VfxParticleSystem.Play();
			GameObject.Destroy(prefab, VfxParticleSystem.main.duration);

		}

		public void setConfing(PowerAttackConfig config)
		{
			this.config = config;
		}
	}

}
