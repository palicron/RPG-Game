using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Character
{

	public class WayPopintContainer : MonoBehaviour
	{


		private void OnDrawGizmos()
		{
			Vector3 firts = transform.GetChild(0).position;
			Vector3 previusPosition = firts;
			foreach (Transform waypoint in transform)
			{
				Gizmos.color = Color.red;
				Gizmos.DrawSphere(waypoint.position, .2f);
				Gizmos.color = Color.green;
				Gizmos.DrawLine(previusPosition, waypoint.position);
				previusPosition = waypoint.position;
			}
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(previusPosition, firts);
		}
	}

}
