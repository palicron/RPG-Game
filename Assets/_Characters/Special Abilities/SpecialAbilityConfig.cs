using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
namespace RPG.Character
{
	public struct AbilityUseParams
	{
		public IDamagaeble target;
		public float baseDamage;

		public AbilityUseParams(IDamagaeble target, float baseDamage)
		{
			this.target = target;
			this.baseDamage = baseDamage;
		}
	}
	public abstract class SpecialAbilityConfig : ScriptableObject
	{
		[Header("Special Ability General")]
		[SerializeField] float energyCost = 10f;
		[SerializeField] GameObject particlePrefab = null;
		[SerializeField] AudioClip Sound=null;
		protected ISpecialAbility behaviour;

		

		abstract public void AttachComponentTo(GameObject GoToAttach);
		
		public void Use(AbilityUseParams useParams)
		{
			behaviour.Use(useParams);
		}

		public float GetEnergyCost()
		{
			return energyCost;
		}

		public GameObject GetParticlePrefab()
		{
			return particlePrefab;
		}
		public AudioClip GetAudioCLip()
		{
			return Sound;
		}
	}

	public interface ISpecialAbility
	{
		void Use(AbilityUseParams useParams);
	}

}
