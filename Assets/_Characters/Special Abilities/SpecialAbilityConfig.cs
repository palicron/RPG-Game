using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
namespace RPG.Character
{

	public abstract class SpecialAbilityConfig : ScriptableObject
	{
		[Header("Special Ability General")]
		[SerializeField] float energyCost = 10f;
		[SerializeField] GameObject particlePrefab = null;
		[SerializeField] AudioClip[] audioClips = null;
		[SerializeField] AnimationClip AbilityAnimation;
		protected AbilityBehavior behaviour;

		public abstract AbilityBehavior getBehaviorCOmponent(GameObject objetToAttachTo);

		 public void AttachAbilityTo(GameObject GoToAttach)
		{
			AbilityBehavior behaviorComponent = getBehaviorCOmponent(GoToAttach);
			behaviorComponent.setConfing(this);
			behaviour = behaviorComponent;
		}
		


		public void Use(GameObject target)
		{
			behaviour.Use(target);
		}

		public float GetEnergyCost()
		{
			return energyCost;
		}

		public AnimationClip GetAbilityAnimation()
		{
			return AbilityAnimation;
		}
		public GameObject GetParticlePrefab()
		{
			return particlePrefab;
		}
		public AudioClip GetAudioCLip()
		{
			return audioClips[Random.Range(0, audioClips.Length)];
		}
	}


}
