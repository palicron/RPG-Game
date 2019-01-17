using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RPG.Core;

namespace RPG.Character
{
	[RequireComponent(typeof(WeaponSystem))]
	[RequireComponent(typeof(Character))]
	public class EnemyAI : MonoBehaviour
	{




		[SerializeField] float chaseRadius = 2f;

		public float distanceToPlayer;
		Character character;
		float CurrentWeapongRange = 1f;
		WeaponSystem weaponSystem;
		PlayerControl player = null;

		enum State { idle,attacking,chasing,patrolling}
		State state = State.idle;
		



		private void Start()
		{
			player = FindObjectOfType<PlayerControl>();
			weaponSystem = GetComponent<WeaponSystem>();
			character = GetComponent<Character>();
		}
		private void Update()
		{

			distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
			CurrentWeapongRange = weaponSystem.GetCurrentWeapongConfig().MaxAttackRange;
			if(distanceToPlayer > chaseRadius && state != State.patrolling)
			{
				StopAllCoroutines();
				state = State.patrolling;
				//resume patrol
			}
			if(distanceToPlayer <= chaseRadius && state != State.chasing)
			{
				StopAllCoroutines();
				StartCoroutine(ChasePLayer());
			}
			if(distanceToPlayer <= CurrentWeapongRange && state!=State.attacking)
			{
				StopAllCoroutines();
				state = State.attacking;
			}

		}


	
		IEnumerator ChasePLayer()
		{
			state = State.chasing;
			while(distanceToPlayer >= CurrentWeapongRange)
			{
				character.SetDestination(player.transform.position);
				yield return new WaitForEndOfFrame();
			}
			
		}


		void OnDrawGizmos()
		{

			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, CurrentWeapongRange);
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere(transform.position, chaseRadius);
		}

	
	}
}
