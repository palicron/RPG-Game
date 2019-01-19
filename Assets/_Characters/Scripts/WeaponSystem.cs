using System.Collections;
using UnityEngine.Assertions;
using UnityEngine;
using System;

namespace RPG.Character
{

	public class WeaponSystem : MonoBehaviour
	{
		const string ATTACK_TRIGGER = "Attack";
		const string DEFAULT_ATTACK = "DEFAULT ATTACK";
		[SerializeField] float baseDamage = 10f;
		[Range(0f, 1.0f)] [SerializeField] float criticalHitChance = 0.1f;
		[SerializeField] float criticalHitMultiplier = 1.25f;
		[SerializeField] ParticleSystem critParticle = null;
		[SerializeField] WeaponConfig currentWeaponConfig;

		Animator animator;
		AnimatorOverrideController animatorOverrideController;
		Character character;
		GameObject target;
		GameObject weapongObject;
		[SerializeField] HealthSystem healthsystem;

		float lastHittime = 0;
	
		void Start()
		{
			animator = GetComponent<Animator>();
			character = GetComponent<Character>();
			animatorOverrideController = character.GetOverDriveController();
			PutWeaponInHand(currentWeaponConfig);
			healthsystem = GetComponent<HealthSystem>();
		}

		private void Update()
		{
			bool targetIsDead ;
			bool targetIsOutOfRange ;
			
			float characterHealt = healthsystem.healthAsPercentage;
			
			
			bool CharacterIsDead = (characterHealt <= Mathf.Epsilon);
			if (target == null)
			{
				targetIsDead = false;
				targetIsOutOfRange = false;
			}
			else
			{
				
				targetIsDead = target.GetComponent<HealthSystem>().healthAsPercentage <= Mathf.Epsilon;
			    targetIsOutOfRange = Vector3.Distance(transform.position, target.transform.position) > currentWeaponConfig.MaxAttackRange;
				
			}

			if(CharacterIsDead || targetIsDead || targetIsOutOfRange)
			{
				StopAllCoroutines();
			}
		}
		public void PutWeaponInHand(WeaponConfig weapongConfig)
		{
			currentWeaponConfig = weapongConfig;
			GameObject dominantHand = RequestDominatHands();
			var weaponprefab = weapongConfig.GetWeaponnPrefab();
			SetAttackAnimation();
			if (weapongObject != null)
				Destroy(weapongObject);

			weapongObject = Instantiate(weaponprefab, dominantHand.transform);
			weapongObject.transform.localPosition = currentWeaponConfig.gripTrasform.localPosition;
			weapongObject.transform.localRotation = currentWeaponConfig.gripTrasform.localRotation;
		}

		public WeaponConfig GetCurrentWeapongConfig()
		{
			return currentWeaponConfig;
		}
		public void AttackTarget(GameObject targetToattack)
		{
			target = targetToattack;

			StartCoroutine(AttackTargetReapeatedly());
		}

		IEnumerator AttackTargetReapeatedly()
		{
			bool attackrStillAlive = healthsystem.healthAsPercentage > Mathf.Epsilon;
			bool targetStillAlive = target.GetComponent<HealthSystem>().healthAsPercentage > Mathf.Epsilon;
			while (attackrStillAlive && targetStillAlive)
			{
				
				float weaponhitPeriod = currentWeaponConfig.timeBetweenAnimation / character.GetAnimSpeedMultiplier();
				float timeToWait = weaponhitPeriod + weaponhitPeriod;
				bool isTimeToHitAgain = Time.time - lastHittime > timeToWait;
				if (isTimeToHitAgain)
				{
					AttackTargetOnece();
					lastHittime = Time.time;
				}
				yield return new WaitForSeconds(timeToWait);
			}
		}
		
		private void AttackTargetOnece()
		{
			transform.LookAt(target.transform.position);
			animator.SetTrigger(ATTACK_TRIGGER);
			float damageDelay = currentWeaponConfig.getDamaDelay(); //TODO get the weapon
			SetAttackAnimation();
			StartCoroutine(DamageAdterDealy(damageDelay));
		}

		IEnumerator DamageAdterDealy(float damageDelay)
		{
			yield return new WaitForSecondsRealtime(damageDelay);
			if(target!=null)
			target.GetComponent<HealthSystem>().TakeDamage(CalculateDamage());
		}
		private void SetAttackAnimation()
		{
			if (!character.GetOverDriveController())
			{
				Debug.Break();
				Debug.LogAssertion("no animator overdrive se in " + gameObject);
			}
			animator.runtimeAnimatorController = animatorOverrideController;
			animatorOverrideController[DEFAULT_ATTACK] = currentWeaponConfig.getAnimClip();
		}

		private GameObject RequestDominatHands()
		{
			var dominanHands = GetComponentsInChildren<DominnannHand>();
			int numDominanHands = dominanHands.Length;
			Assert.AreNotEqual(numDominanHands, 0, "No dominat hand,add one");
			Assert.IsFalse(numDominanHands > 1, "Multiple hands on player");
			return dominanHands[0].gameObject;
		}


		private float CalculateDamage()
		{
			bool isCriticalHit = UnityEngine.Random.Range(0f, 1f) <= criticalHitChance;
			float damagaBeforeCrit = baseDamage + currentWeaponConfig.GetAdditinalDamage();
			if (isCriticalHit)
			{
				critParticle.Play();
				return damagaBeforeCrit * criticalHitMultiplier;
			}
			else
			{
				return damagaBeforeCrit;
			}

		}

		public void StopAttacking()
		{
			animator.StopPlayback();
			StopAllCoroutines();
		}

	}

}
