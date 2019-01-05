using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Character
{ 
public class SelftHealBehavior : MonoBehaviour, ISpecialAbility
{
		SelftHealConfig config;
		Player player;
		private void Start()
		{
			player = FindObjectOfType<Player>();
		}
		public void Use(AbilityUseParams useParams)
		{
			player.TakeDamage(-config.GetExtraHeal());
			PlayParticalEffect();
		}

		private void PlayParticalEffect()
		{

			var prefab = GameObject.Instantiate(config.GetParticlePrefab(), transform.position, Quaternion.identity);
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
