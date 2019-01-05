using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using RPG.CameraUI; //TODO consider RE-write
using RPG.Core;
using RPG.Weapons;
using UnityEngine.SceneManagement;
namespace RPG.Character
{
	public class Player : MonoBehaviour, IDamagaeble
	{
		const string ATTACK_TRIGGER = "Attack";
		const string DEATH_TRIGGER = "Death";
		[SerializeField] float maxHealhPoints = 100f;
		[SerializeField] float baseDamage = 10f;
		[SerializeField] Weapon weaponinUse;
		[SerializeField] AnimatorOverrideController animatorOverrideController;

		Enemy CurrentEnemy = null;
		//TODO Temporarily serializi for debug
		[SerializeField] SpecialAbilityConfig[] abilities;
		AudioSource audioSource;
		private Animator animator;
		//[SerializeField] GameObject WeaponnSocket;
		float currentHelhPoints = 100f;


		[SerializeField] AudioClip[] DamgeSounds;
		[SerializeField] AudioClip[] DeathSounds;
		CameraRaycaster cameraRaycaster;
		float lastHittime = 0;
		private void Start()
		{
			RegisterForMouseClik();
			currentHelhPoints = maxHealhPoints;
			audioSource = GetComponent<AudioSource>();
			PutWeaponInHand();
			SetupRunTimeAnimator();
			AttachInitialAbilities();

		}

		private void AttachInitialAbilities()
		{
			for (int i = 0; i < abilities.Length; i++)
			{
				abilities[i].AttachComponentTo(this.gameObject);
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

		private void SetupRunTimeAnimator()
		{
			animator = GetComponent<Animator>();
			animator.runtimeAnimatorController = animatorOverrideController;
			animatorOverrideController["DEFAULT ATTACK"] = weaponinUse.getAnimClip();
		}

		private void PutWeaponInHand()
		{
			GameObject dominantHand = RequestDominatHands();
			var weaponprefab = weaponinUse.GetWeaponnPrefab();
			var weapon = Instantiate(weaponprefab, dominantHand.transform);
			weapon.transform.localPosition = weaponinUse.gripTrasform.localPosition;
			weapon.transform.localRotation = weaponinUse.gripTrasform.localRotation;
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
			if (Input.GetMouseButton(0) && IsTargetInrange(enemy.gameObject))
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

			if (Time.time - lastHittime > weaponinUse.MinTimeBetween)
			{
				animator.SetTrigger(ATTACK_TRIGGER); //TODO maeka const
				CurrentEnemy.TakeDamage(baseDamage);
				lastHittime = Time.time;
			}
		}

		private bool IsTargetInrange(GameObject target)
		{
			float disttanceTotarget = (target.transform.position - transform.position).magnitude;

			return disttanceTotarget <= weaponinUse.MaxAttackRange;

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
