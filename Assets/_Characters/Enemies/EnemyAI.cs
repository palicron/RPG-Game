using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RPG.Core;
using System;

namespace RPG.Character
{
	[RequireComponent(typeof(WeaponSystem))]
	[RequireComponent(typeof(Character))]
	public class EnemyAI : MonoBehaviour
	{




		[SerializeField] float chaseRadius = 2f;
		[SerializeField] WayPopintContainer patrolPath;
		[SerializeField] float wayPointTolerance = 1f;
		public float distanceToPlayer;
		Character character;
		float CurrentWeapongRange = 1f;
		WeaponSystem weaponSystem;
		PlayerControl player = null;
		int nextWaypoint = 0;
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
				StartCoroutine(Patrol());
			}
			if(distanceToPlayer <= chaseRadius && distanceToPlayer>CurrentWeapongRange && state != State.chasing )
			{
				
				StopAllCoroutines();
				weaponSystem.StopAttacking();
				StartCoroutine(ChasePLayer());
			}
			if(distanceToPlayer <= CurrentWeapongRange && state!=State.attacking)
			{
				StopAllCoroutines();
				state = State.attacking;
				weaponSystem.AttackTarget(player.gameObject);
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
	  

		IEnumerator Patrol()
		{
			state = State.patrolling;
			while(patrolPath!=null)
			{
				Vector3 nextWayPoint = patrolPath.transform.GetChild(nextWaypoint).position;
				character.SetDestination(nextWayPoint);
				CycleWaypointWhentClose(nextWayPoint);
				yield return new WaitForSeconds(0.5f);

			}
		
		}

		private void CycleWaypointWhentClose(Vector3 nextWayPoint)
		{
			if(Vector3.Distance(transform.position, nextWayPoint)<=wayPointTolerance)
			{
				nextWaypoint = (nextWaypoint + 1) % patrolPath.transform.childCount;
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
