using System;
using UnityEngine;
using UnityEngine.AI;
using RPG.CameraUI;


namespace RPG.Character
{
	[SelectionBase]
	[RequireComponent(typeof(NavMeshAgent))]
	
	public class Character : MonoBehaviour
	{
		[Header("Setup Settings")]
		[SerializeField] RuntimeAnimatorController animatorController;
		[SerializeField] AnimatorOverrideController aniamtorOverdriveControler;
		[SerializeField] Avatar characterAvatar;
		[Header("Colider Setup")]
		[SerializeField] Vector3 capsulColliderCentar = new Vector3(0, 0.9f, 0);
		[SerializeField] float capsulColliderRadius = 0.3f;
		[SerializeField] float capsulColliderHeight = 1.88f;

		[Header("Movement Properties")]
		[SerializeField] float MovingTurnSpeed = 360;
		[SerializeField] float StationaryTurnSpeed = 180;
		[SerializeField] float RunCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
		[SerializeField] float moveSpeedMultiplier = 0.7f;
		[SerializeField] float moveThresHold = 1f;
		[SerializeField] float stopingDistance = 1f;

	
		float m_TurnAmount;
		float m_ForwardAmount;
		bool IsInDirectMode = false;
		Vector3 m_GroundNormal;
	
		Animator animator;
		Rigidbody m_Rigidbody;
		Vector3 cuerrentDestination, clickPoint;
		NavMeshAgent agent;
		CapsuleCollider capsulCollider;

		//TODO serialize problem

		private void Awake()
		{
			AddRquireComponets();
		}

		private void AddRquireComponets()
		{
			animator = gameObject.AddComponent<Animator>();
			animator.runtimeAnimatorController = animatorController;
			animator.avatar = characterAvatar;
			capsulCollider = gameObject.AddComponent<CapsuleCollider>();
			capsulCollider.center = capsulColliderCentar;
			capsulCollider.radius = capsulColliderRadius;
			capsulCollider.height = capsulColliderHeight;

		}

		void Start()
		{
			CameraRaycaster cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
			
			cuerrentDestination = transform.position;
			animator = GetComponent<Animator>();
			m_Rigidbody = GetComponent<Rigidbody>();
			m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
		
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
				Move(agent.desiredVelocity);
			}
			else
			{
				Move(Vector3.zero);
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

				agent.SetDestination(destination);
				
			}

		}

		public void Move(Vector3 move)
		{

			if (move.magnitude > moveThresHold)
			{
				move.Normalize();
			}
			move = transform.InverseTransformDirection(move);
			move = Vector3.ProjectOnPlane(move, m_GroundNormal);
			m_TurnAmount = Mathf.Atan2(move.x, move.z);
			m_ForwardAmount = move.z;
			ApplyExtraTurnRotation();
			UpdateAnimator();
		}



		void UpdateAnimator()
		{


			animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
			animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);

		}

		void ApplyExtraTurnRotation()
		{
			// help the character turn faster (this is in addition to root rotation in the animation)
			float turnSpeed = Mathf.Lerp(StationaryTurnSpeed, MovingTurnSpeed, m_ForwardAmount);
			transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
		}
	



		private void OnAnimatorMove()
		{
			if (Time.deltaTime > 0)
			{
				Vector3 velocity = (animator.deltaPosition * moveSpeedMultiplier) / Time.deltaTime;

				velocity.y = m_Rigidbody.velocity.y;
				m_Rigidbody.velocity = velocity;
			}
		}




		//TODO repari
		private void ProcessDirectMoement()
		{
			float h = Input.GetAxis("Horizontal");
			float v = Input.GetAxis("Vertical");

			Vector3 comeraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
			Vector3 movement = v * comeraForward + h * Camera.main.transform.right;

			Move(movement);
		}

		Vector3 ShortDestination(Vector3 destination, float shortening)
		{
			Vector3 reductionVector = (destination - transform.position).normalized * shortening;
			return destination - reductionVector;
		}

		public void Kill()
		{

		}

	}

}
