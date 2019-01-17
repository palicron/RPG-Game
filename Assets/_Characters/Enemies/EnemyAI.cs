using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RPG.Core;

namespace RPG.Character
{
	[RequireComponent(typeof(WeaponSystem))]
	public class EnemyAI : MonoBehaviour
	{




		[SerializeField] float chaseWeapong = 2f;
		float CurrentWeapongRange = 1f;
		WeaponSystem weaponSystem;
		PlayerControl player = null;
		bool isAttacking = false;



		private void Start()
		{
			player = FindObjectOfType<PlayerControl>();
			weaponSystem = GetComponent<WeaponSystem>();	
		}
		private void Update()
		{

			float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
			CurrentWeapongRange = weaponSystem.GetCurrentWeapongConfig().MaxAttackRange;

		}


	



		void OnDrawGizmos()
		{

			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, CurrentWeapongRange);
			Gizmos.color = Color.blue;
			
		}

	
	}
}
