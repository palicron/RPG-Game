using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using System;

namespace RPG.Character
{
	public class AreaOfEffectBehavior : MonoBehaviour, ISpecialAbility
	{
		AreaOfEffectConfig config;

		public void Use(AbilityUseParams useParams)
		{
			DealRadialDamage(useParams);
			PlayParticalEffect();
		}

		private void PlayParticalEffect()
		{
			var particleprefab = config.GetParticlePrefab();
			var prefab = GameObject.Instantiate(particleprefab, transform.position, particleprefab.transform.rotation);
			ParticleSystem VfxParticleSystem = prefab.GetComponent<ParticleSystem>();
			VfxParticleSystem.Play();
			GameObject.Destroy(prefab, VfxParticleSystem.main.duration);

		}

		private void DealRadialDamage(AbilityUseParams useParams)
		{
			//static sphere
			RaycastHit[] hits = Physics.SphereCastAll(
				transform.position
				, config.GetMaximunRadius(),
				Vector3.up,
				config.GetMaximunRadius());

			foreach (RaycastHit hit in hits)
			{
				var damageable = hit.collider.gameObject.GetComponent<IDamagaeble>();
				if (damageable != null && !hit.collider.gameObject.tag.Equals("Player"))
				{
					float damageTodeal = useParams.baseDamage + config.GetExtraDamage();
					damageable.TakeDamage(damageTodeal);
				}
			}
		}

		public void setConfing(AreaOfEffectConfig config)
		{
			this.config = config;
		}
	}
}
