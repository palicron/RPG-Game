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
	public class Player : MonoBehaviour, IDamagaeble
	{
		const string ATTACK_TRIGGER = "Attack";
		const string DEATH_TRIGGER = "Death";
		const string DEFAULT_ATTACK = "DEFAULT ATTACK";
		[SerializeField] float maxHealhPoints = 100f;
		[SerializeField] float baseDamage = 10f;
		[SerializeField] Weapon currentWeaponConfig;
		[SerializeField] AnimatorOverrideController animatorOverrideController;
		GameObject weapongObject;
		Enemy CurrentEnemy = null;
		//TODO Temporarily serializi for debug
		[SerializeField] SpecialAbilityConfig[] abilities;
		AudioSource audioSource;
		private Animator animator;
		//[SerializeField] GameObject WeaponnSocket;
		float currentHelhPoints = 100f;
		[Range(0f, 1.0f)] [SerializeField] float criticalHitChance = 0.1f;
		[SerializeField] float criticalHitMultiplier = 1.25f;

		[SerializeField] AudioClip[] DamgeSounds;
		[SerializeField] AudioClip[] DeathSounds;
		CameraRaycaster cameraRaycaster;
		[SerializeField] ParticleSystem critParticle = null;

		float lastHittime = 0;
		private void Start()
		{

			RegisterForMouseClik();
			currentHelhPoints = maxHealhPoints;
			audioSource = GetComponent<AudioSource>();
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

		public float healthAsPercentage
		{
			get
			{
				return currentHelhPoints / maxHealhPoints;
			}
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
				CurrentEnemy.TakeDamage(CalculateDamage());
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

		public void TakeDamage(float damage)
		{
			if (currentHelhPoints - damage <= 0)
			{

				currentHelhPoints = Mathf.Clamp(currentHelhPoints - damage, 0f, maxHealhPoints);
				StartCoroutine(KillPlayer());

			}
			else
			{

				audioSource.Stop();
				audioSource.clip = DamgeSounds[UnityEngine.Random.Range(0, DamgeSounds.Length)];
				audioSource.Play();

				currentHelhPoints = Mathf.Clamp(currentHelhPoints - damage, 0f, maxHealhPoints);
			}


		}
		public void Heal(float points)
		{
			currentHelhPoints = Mathf.Clamp(currentHelhPoints + points, 0f, maxHealhPoints);
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
		IEnumerator KillPlayer()
		{
			animator.SetTrigger(DEATH_TRIGGER);

			audioSource.Stop();
			audioSource.clip = DeathSounds[UnityEngine.Random.Range(0, DeathSounds.Length)];
			audioSource.Play();

			yield return new WaitForSecondsRealtime(audioSource.clip.length + 0.1f);//todo use audiclipo lenth
			SceneManager.LoadSceneAsync(0);
		}

	}
}
