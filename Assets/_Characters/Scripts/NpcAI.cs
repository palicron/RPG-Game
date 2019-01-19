﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Character
{
	[RequireComponent(typeof(Character))]
	public class NpcAI : MonoBehaviour
	{
		[SerializeField] float chaseRadius = 2f;
		[SerializeField] WayPopintContainer patrolPath;
		[SerializeField] float wayPointTolerance = 0.1f;
		[SerializeField] float TalkRange = 1f;
	

		Character character;
		public float distanceToPlayer;
		int nextWaypoint = 0;
		PlayerControl player = null;

		enum State { idle, chasing, patrolling,talking }
		State state = State.idle;

		// Start is called before the first frame update
		void Start()
		{
			player = FindObjectOfType<PlayerControl>();
			character = GetComponent<Character>();
		}

		// Update is called once per frame
		void Update()
		{
			distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

			bool inChaseRange =  distanceToPlayer <= chaseRadius;
			bool inTalkRange = distanceToPlayer <= TalkRange;
			if(!inChaseRange && state != State.patrolling)
			{
				StopAllCoroutines();
				StartCoroutine(Patrol());
			}
			if (inChaseRange && state != State.chasing)
			{

				StopAllCoroutines();
				StartCoroutine(ChasePLayer());
			}
			if (inTalkRange)
			{
				StopAllCoroutines();
				state = State.talking;
				transform.LookAt(player.transform);
			
			}
		}



		IEnumerator ChasePLayer()
		{
			state = State.chasing;
			while (distanceToPlayer >= TalkRange)
			{
				character.SetDestination(player.transform.position);
				yield return new WaitForEndOfFrame();
			}

		}


		IEnumerator Patrol()
		{
			state = State.patrolling;
			while (patrolPath != null)
			{
				Vector3 nextWayPoint = patrolPath.transform.GetChild(nextWaypoint).position;
				character.SetDestination(nextWayPoint);
				CycleWaypointWhentClose(nextWayPoint);
				yield return new WaitForSeconds(0.5f);

			}

		}

		private void CycleWaypointWhentClose(Vector3 nextWayPoint)
		{
			if (Vector3.Distance(transform.position, nextWayPoint) <= wayPointTolerance)
			{
				nextWaypoint = (nextWaypoint + 1) % patrolPath.transform.childCount;
			}

		}


		void OnDrawGizmos()
		{

			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(transform.position, TalkRange);
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere(transform.position, chaseRadius);
		}

	}

}
