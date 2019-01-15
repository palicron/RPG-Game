using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using RPG.CameraUI; //TODO consider RE-write
using RPG.Core;

using UnityEngine.SceneManagement;
namespace RPG.Character
{
	public class Player : MonoBehaviour
	{
		const string ATTACK_TRIGGER = "Attack";
		
		const string DEFAULT_ATTACK = "DEFAULT ATTACK";
	
		[SerializeField] float baseDamage = 10f;
		[SerializeField] Weapon currentWeaponConfig;
		[SerializeField] AnimatorOverrideController animatorOverrideController;
		GameObject weapongObject;
		Enemy CurrentEnemy = null;
		//TODO Temporarily serializi for debug
		[SerializeField] SpecialAbilityConfig[] abilities;
		
		private Animator animator;
		//[SerializeField] GameObject WeaponnSocket;
		
		[Range(0f, 1.0f)] [SerializeField] float criticalHitChance = 0.1f;
		[SerializeField] float criticalHitMultiplier = 1.25f;


		CameraRaycaster cameraRaycaster;
		[SerializeField] ParticleSystem critParticle = null;

		float lastHittime = 0;
		private void Start()
		{

			RegisterForMouseClik();
			PutWeaponInHand(currentWeaponConfig);
			AttachInitialAbilities();

		}

		private void AttachInitialAbilities()
		{
			for (int i = 0; i < abilities.Length; i++)
			{
				abilities[i].AttachAbilityTo(this.gameObject);
			}
		}

		private void Update()
		{
			var healthAsPercentage = GetComponent<HealthSystem>().healthAsPercentage;
			if (healthAsPercentage > Mathf.Epsilon)
			{
				ScanForAbilityKeyPress();
			}
		}

		private void ScanForAbilityKeyPress()
		{
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				AttempotsSpecialAbility(1);

			}
			else if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				AttempotsSpecialAbility(2);
			}
		}

		private void SetAttackAnimation()
		{
			animator = GetComponent<Animator>();
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

		private void RegisterForMouseClik()
		{
			cameraRaycaster = FindObjectOfType<CameraRaycaster>();
			cameraRaycaster.onMouseOverEnemy += MouseOverEnemy;
		}



		void MouseOverEnemy(Enemy enemy)
		{
			CurrentEnemy = enemy;
			if (Input.GetMouseButtonDown(0) && IsTargetInrange(enemy.gameObject))
			{
				AttackTarget();
			}
			else if (Input.GetMouseButtonDown(1))
			{
				AttempotsSpecialAbility(0);
			}
		}

		private void AttempotsSpecialAbility(int abilittIndex)
		{
			var energyComponent = GetComponent<Energy>();
			float energyCost = abilities[abilittIndex].GetEnergyCost();
			if (energyComponent.IsEnergyAvailable(10f))//TODO read from ability
			{

				energyComponent.ConsumeEnergy(energyCost);
				var abilityParams = new AbilityUseParams(CurrentEnemy, baseDamage);
				abilities[abilittIndex].Use(abilityParams);
			}
		}

		private void AttackTarget()
		{

			if (Time.time - lastHittime > currentWeaponConfig.MinTimeBetween)
			{
				animator.SetTrigger(ATTACK_TRIGGER); //TODO maeka const
	
				lastHittime = Time.time;
			}
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

		private bool IsTargetInrange(GameObject target)
		{
			float disttanceTotarget = (target.transform.position - transform.position).magnitude;

			return disttanceTotarget <= currentWeaponConfig.MaxAttackRange;

		}



		public void PutWeaponInHand(Weapon weapongConfig)
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


	}
}
