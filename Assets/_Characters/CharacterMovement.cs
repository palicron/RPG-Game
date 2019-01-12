using System;
using UnityEngine;
using UnityEngine.AI;
using RPG.CameraUI;


namespace RPG.Character
{

	[RequireComponent(typeof(NavMeshAgent))]
	[RequireComponent(typeof(ThirdPersonCharacter))]
	public class CharacterMovement : MonoBehaviour
	{
		[SerializeField] float stopingDistance = 1f;
		bool IsInDirectMode = false;
		ThirdPersonCharacter thirPersonCharacter;   // A reference to the ThirdPersonCharacter on the object

		Vector3 cuerrentDestination, clickPoint;
		NavMeshAgent agent;
		GameObject walktTarget = null;
		//TODO serialize problem

		void Start()
		{
			CameraRaycaster cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
			thirPersonCharacter = GetComponent<ThirdPersonCharacter>();
			cuerrentDestination = transform.position;
			walktTarget = new GameObject("walkTarget");
			cameraRaycaster.OnMouseOverPotetiallWalkable += processMovement;
			cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
			agent = GetComponent<NavMeshAgent>();
			agent.updateRotation = false;
			agent.updatePosition = true;
			agent.stoppingDistance = stopingDistance;
		}

		private void Update()
		{
		
		
			if (agent.remainingDistance>agent.stoppingDistance)
			{
				thirPersonCharacter.Move(agent.desiredVelocity);
			}
			else
			{
				thirPersonCharacter.Move(Vector3.zero);
			}
		}
		private void OnMouseOverEnemy(Enemy enemy)
		{
			if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(1))
			{
				//aICharacterControl.SetTarget(enemy.transform);
				agent.SetDestination(enemy.transform.position);

			}
		}

		void processMovement(Vector3 destination)
		{
			if (Input.GetMouseButton(0))
			{
				print(walktTarget.transform.position);

				//walktTarget.transform.position = destination;
				agent.SetDestination(destination);
				
			}

		}



		// Fixed update is called in sync with physics
	//	private void FixedUpdate()
		//{

		//	cuerrentDestination = transform.position;
	//		WalkToDestination();
		//}
		//TODO arreglar bug de mantener precionado el mause se peude mover a zonas que no


		private void WalkToDestination()
		{
			var playerToClickPoint = cuerrentDestination - transform.position;
			if (playerToClickPoint.magnitude >= 0)
				thirPersonCharacter.Move(playerToClickPoint);
			else
			{
				thirPersonCharacter.Move(Vector3.zero);
			}
		}








		//TODO repari
		private void ProcessDirectMoement()
		{
			float h = Input.GetAxis("Horizontal");
			float v = Input.GetAxis("Vertical");

			Vector3 comeraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
			Vector3 movement = v * comeraForward + h * Camera.main.transform.right;

			thirPersonCharacter.Move(movement);
		}

		Vector3 ShortDestination(Vector3 destination, float shortening)
		{
			Vector3 reductionVector = (destination - transform.position).normalized * shortening;
			return destination - reductionVector;
		}



	}

}
