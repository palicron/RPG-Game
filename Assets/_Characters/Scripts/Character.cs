using System;
using UnityEngine;
using UnityEngine.AI;
using RPG.CameraUI;


namespace RPG.Character
{
	[SelectionBase]	
	public class Character : MonoBehaviour
	{
		[Header("Animator Settings")]
		[SerializeField] RuntimeAnimatorController animatorController;
		[SerializeField] AnimatorOverrideController aniamtorOverdriveControler;
		[SerializeField] Avatar characterAvatar;
		[SerializeField] [Range(.1f, 1f)] float animatorForwarCap =1f;

		[Header("Audio Setup")]
		[SerializeField] float audioSourceSpatingBlend = 0.5f;

		[Header("Colider Setup")]
		[SerializeField] Vector3 capsulColliderCentar = new Vector3(0, 0.9f, 0);
		[SerializeField] float capsulColliderRadius = 0.3f;
		[SerializeField] float capsulColliderHeight = 1.88f;
		

		[Header("Movement Properties")]
		[SerializeField] float MovingTurnSpeed = 360;
		[SerializeField] float StationaryTurnSpeed = 180;
		[SerializeField] float moveSpeedMultiplier = 0.7f;
		[SerializeField] float moveThresHold = 1f;
		[SerializeField] float stopingDistance = 1f;

		[Header("Navmesh Agent Setup")]
		[SerializeField] float navMeshSteeringSpeed = 4f;
		[SerializeField] float navMeshStopingDistance = 1.3f;

		float m_TurnAmount;
		float m_ForwardAmount;
		bool IsInDirectMode = false;
		Vector3 m_GroundNormal;
	
		Animator animator;
		Rigidbody m_Rigidbody;
		Vector3 cuerrentDestination;
		NavMeshAgent agent;
		CapsuleCollider capsulCollider;

		bool isAlive = true;
	

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

			m_Rigidbody = gameObject.AddComponent<Rigidbody>();
			m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;

			var audioSource = gameObject.AddComponent<AudioSource>();
			audioSource.playOnAwake = false;
			audioSource.spatialBlend = audioSourceSpatingBlend;

			agent = gameObject.AddComponent<NavMeshAgent>();
			agent.updateRotation = false;
			agent.updatePosition = true;
			agent.stoppingDistance = stopingDistance;
			agent.speed = navMeshSteeringSpeed;
			agent.stoppingDistance = navMeshStopingDistance;
			agent.autoBraking = true;
		}

	

		private void Update()
		{
		
		
			if (agent.remainingDistance>agent.stoppingDistance && isAlive)
			{
				Move(agent.desiredVelocity);
			}
			else
			{
				Move(Vector3.zero);
			}
		}



		void Move(Vector3 move)
		{

			if (move.magnitude > moveThresHold)
			{
				move.Normalize();
			}
			move = transform.InverseTransformDirection(move);
			move = Vector3.ProjectOnPlane(move, m_GroundNormal);
			m_TurnAmount = Mathf.Atan2(move.x, move.z);
			m_ForwardAmount = move.z;
			m_ForwardAmount =Mathf.Clamp(m_ForwardAmount, 0.1f, animatorForwarCap);
			ApplyExtraTurnRotation();
			UpdateAnimator();
		}

		public void SetDestination(Vector3 worldPos)
		{
			agent.SetDestination(worldPos);
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
			isAlive = false;
		}

		public AnimatorOverrideController GetOverDriveController()
		{
			return aniamtorOverdriveControler;
		}
		public float GetAnimSpeedMultiplier()
		{
			return animator.speed;
		}
	}

}
