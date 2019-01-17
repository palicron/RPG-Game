using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;

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
		float lastHittime = 0;
		// Start is called before the first frame update
		void Start()
		{
			animator = GetComponent<Animator>();
			character = GetComponent<Character>();
			animatorOverrideController = character.GetOverDriveController();
			PutWeaponInHand(currentWeaponConfig);
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
			print("attacking" + target);
			//TODO use a reper attac corrutine
		}
		private void SetAttackAnimation()
		{

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



	}

}
