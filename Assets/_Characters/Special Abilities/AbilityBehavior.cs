using System.Collections;
using UnityEngine;

namespace RPG.Character
{

	public abstract class AbilityBehavior : MonoBehaviour
	{
		const string ATTACK_TRIGGER = "Attack";
		const string DEFAULT_ATTACK_STATE = "DEFAULT ATTACK";

		protected SpecialAbilityConfig config;
		const float PARTICLE_CLEAN_UP_DATE = 10f;
		public abstract void Use(GameObject target=null);

		public void setConfing(SpecialAbilityConfig configToSet)
		{
			this.config = configToSet;
		}


		protected void PlayParticalEffect(Vector3 spwanPOsition,bool PlayerParent)
		{
			var particleprefab = config.GetParticlePrefab();
			var particleObjet = GameObject.Instantiate(particleprefab, spwanPOsition, particleprefab.transform.rotation);
			if(PlayerParent)
			particleObjet.transform.parent = this.transform;
			particleObjet.GetComponent<ParticleSystem>().Play();
			StartCoroutine(DestroyParticleAfterWhenFInish(particleObjet));
		}


		protected void PlayAbilitySound()
		{
			var abilitySound = config.GetAudioCLip(); //TODO randon clip
			var audioSource = GetComponent<AudioSource>();
			audioSource.PlayOneShot(abilitySound);
		}

		protected void PlayAbilityAnimation()
		{
			var animatorOverDriveController = GetComponent<Character>().GetOverDriveController();
			var animator = GetComponent<Animator>();
			animator.runtimeAnimatorController = animatorOverDriveController;
			animatorOverDriveController[DEFAULT_ATTACK_STATE] = config.GetAbilityAnimation();
			animator.SetTrigger(ATTACK_TRIGGER);


		}
		IEnumerator DestroyParticleAfterWhenFInish(GameObject particlePrefab)
		{
			while(particlePrefab.GetComponent<ParticleSystem>().isPlaying)
			{
				yield return new WaitForSeconds(PARTICLE_CLEAN_UP_DATE);
			}
			Destroy(particlePrefab);
			yield return new WaitForEndOfFrame();
		}
	}

}
