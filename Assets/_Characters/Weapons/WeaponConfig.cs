using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Character
{
	[CreateAssetMenu(menuName = ("RPG/Weapon"))]
	public class WeaponConfig : ScriptableObject
	{
		public Transform gripTrasform;

		[SerializeField] GameObject weaponPrefab;
		[SerializeField] AnimationClip atttackAnimation;
		[SerializeField] float timeBetweenAnimationCycles;
		[SerializeField] float maxAttackRange = 2f;
		[SerializeField] float additionalDamage = 10f;
		[SerializeField] float damageDelay = .5f;

		public float MaxAttackRange
		{
			get
			{
				return maxAttackRange;
			}

			set
			{
				maxAttackRange = value;
			}
		}

		public float timeBetweenAnimation
		{
			get
			{
				return  atttackAnimation.length;
			}

			set
			{
				timeBetweenAnimationCycles = value;
			}
		}

		public GameObject GetWeaponnPrefab()
		{
			return weaponPrefab;
		}
		public float getDamaDelay()
		{
			return damageDelay;
		}
		public float GetAdditinalDamage()
		{
			return additionalDamage;
		}

		public AnimationClip getAnimClip()
		{
			atttackAnimation.events = new AnimationEvent[0];//clear aniamtion events
			return atttackAnimation;
		}
	}
}
