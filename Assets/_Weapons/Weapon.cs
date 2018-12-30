using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Weapons
{
	[CreateAssetMenu(menuName = ("RPG/Weapon"))]
	public class Weapon : ScriptableObject
	{
		public Transform gripTrasform;

		[SerializeField] GameObject weaponPrefab;
		[SerializeField] AnimationClip atttackAnimation;
		[SerializeField] float minTimeBetween = 0.5f;
		[SerializeField] float maxAttackRange = 2f;

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

		public float MinTimeBetween
		{
			get
			{
				return minTimeBetween;
			}

			set
			{
				minTimeBetween = value;
			}
		}

		public GameObject GetWeaponnPrefab()
		{
			return weaponPrefab;
		}

		public AnimationClip getAnimClip()
		{
			atttackAnimation.events = new AnimationEvent[0];//clear aniamtion events
			return atttackAnimation;
		}
	}
}
