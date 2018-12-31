﻿using System;
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
			abilities[0].AttachComponentTo(this.gameObject);
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
			if(Input.GetMouseButton(0) && IsTargetInrange(enemy.gameObject))
			{
				AttackTarget(enemy);
			}
			else if(Input.GetMouseButtonDown(1))
			{
				AttempotsSpecialAbility(0,enemy);
			}
		}

		private void AttempotsSpecialAbility(int abilittIndex,Enemy enemy)
		{
			var energyComponent = GetComponent<Energy>();
			float energyCost = abilities[abilittIndex].GetEnergyCost();
			if (energyComponent.IsEnergyAvailable(10f))//TODO read from ability
			{
				energyComponent.ConsumeEnergy(energyCost);
				var abilityParams = new AbilityUseParams(enemy, baseDamage);
				abilities[abilittIndex].Use(abilityParams);
			}
		}

		private void AttackTarget(Enemy enemy)
		{
		
			if (Time.time - lastHittime > weaponinUse.MinTimeBetween)
			{
				animator.SetTrigger(ATTACK_TRIGGER); //TODO maeka const
				enemy.TakeDamage(baseDamage);
				lastHittime = Time.time;
			}
		}

		private bool IsTargetInrange( GameObject target)
		{
			float disttanceTotarget = (target.transform.position - transform.position).magnitude;

			return disttanceTotarget <= weaponinUse.MaxAttackRange;
			
		}

		public void TakeDamage(float damage)
		{
			if (currentHelhPoints - damage <= 0)
			{
				ReduceHealt(damage);
				StartCoroutine(KillPlayer());
			
			}
			else
			{
				audioSource.Stop();
				audioSource.clip = DamgeSounds[UnityEngine.Random.Range(0, DamgeSounds.Length)];
				audioSource.Play();
				ReduceHealt(damage);
			}
				
		
			
		}

		IEnumerator KillPlayer()
		{
			animator.SetTrigger(DEATH_TRIGGER);

			audioSource.Stop();
			audioSource.clip = DeathSounds[UnityEngine.Random.Range(0, DeathSounds.Length)];
			audioSource.Play();
			
			yield return new WaitForSecondsRealtime(audioSource.clip.length+0.1f);//todo use audiclipo lenth
			SceneManager.LoadSceneAsync(0);
		}
		private void ReduceHealt(float damage)
		{
			currentHelhPoints = Mathf.Clamp(currentHelhPoints - damage, 0f, maxHealhPoints);
		}
	}
}
