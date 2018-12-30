using System;
using UnityEngine;

using UnityEngine.AI;
using RPG.CameraUI;


namespace RPG.Character
{

	[RequireComponent(typeof(NavMeshAgent))]
	[RequireComponent(typeof(AICharacterControl))]
	[RequireComponent(typeof(ThirdPersonCharacter))]
	public class PlayerMovement : MonoBehaviour
	{
		bool IsInDirectMode = false;
		ThirdPersonCharacter thirPersonCharacter = null;   // A reference to the ThirdPersonCharacter on the object
		CameraRaycaster cameraRaycaster = null;
		Vector3 cuerrentDestination, clickPoint;
		AICharacterControl aICharacterControl = null;
		GameObject walktTarget = null;
		//TODO serialize problem
	
		void Start()
		{
			cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
			thirPersonCharacter = GetComponent<ThirdPersonCharacter>();
			cuerrentDestination = transform.position;
			aICharacterControl = GetComponent<AICharacterControl>();
			walktTarget = new GameObject("walkTarget");
	

			cameraRaycaster.OnMouseOverPotetiallWalkable += processMovement;
			cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
		}

		private void OnMouseOverEnemy(Enemy enemy)
		{
			if(Input.GetMouseButton(0) || Input.GetMouseButtonDown(1))
			{
				aICharacterControl.SetTarget(enemy.transform);
			}
		}

		void processMovement(Vector3 destination)
		{
			if(Input.GetMouseButton(0) )
			{
				walktTarget.transform.position = destination;
				aICharacterControl.SetTarget(walktTarget.transform);
			}
		
		}
	

		
		// Fixed update is called in sync with physics
		private void FixedUpdate()
		{

			cuerrentDestination = transform.position;
			WalkToDestination();
		}
		//TODO arreglar bug de mantener precionado el mause se peude mover a zonas que no


		private void WalkToDestination()
		{
			var playerToClickPoint = cuerrentDestination - transform.position;
			if (playerToClickPoint.magnitude >= 0)
				thirPersonCharacter.Move(playerToClickPoint, false, false);
			else
			{
				thirPersonCharacter.Move(Vector3.zero, false, false);
			}
		}


		//TODO repari
		private void ProcessDirectMoement()
		{
			float h = Input.GetAxis("Horizontal");
			float v = Input.GetAxis("Vertical");

			Vector3 comeraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
			Vector3 movement = v * comeraForward + h * Camera.main.transform.right;

			thirPersonCharacter.Move(movement, false, false);
		}

		Vector3 ShortDestination(Vector3 destination, float shortening)
		{
			Vector3 reductionVector = (destination - transform.position).normalized * shortening;
			return destination - reductionVector;
		}



	}

}
