using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System;

namespace RPG.Character
{

	public class SpecialAbilities : MonoBehaviour
	{
		[SerializeField] SpecialAbilityConfig[] abilities;
		[SerializeField] Image energyBar = null;
		[SerializeField] float maxEnergyPoint = 100f;
		[SerializeField] float regenPointsPerSecond = 1f;
		[SerializeField] AudioClip outOfMana;

		float currentEnergPoints = 100f;

		AudioSource audioSource;

		float EnergyAsPercentage { get { return currentEnergPoints / maxEnergyPoint; } }


		// Start is called before the first frame update
		void Start()
		{
			audioSource = GetComponent<AudioSource>();
			currentEnergPoints = maxEnergyPoint;
			UpdateEnergyBar();
			AttachInitialAbilities();
		}


		void Update()
		{
			if (currentEnergPoints < maxEnergyPoint)
			{
				AddEnergyPoints();
				UpdateEnergyBar();
			}
		}


		private void AttachInitialAbilities()
		{
			for (int i = 0; i < abilities.Length; i++)
			{
				abilities[i].AttachAbilityTo(this.gameObject);
			}
		}

		private void AddEnergyPoints()
		{
			var pointsToAdd = regenPointsPerSecond * Time.deltaTime;
			currentEnergPoints = Mathf.Clamp(currentEnergPoints + pointsToAdd, 0, maxEnergyPoint);
		}

		public void ConsumeEnergy(float amount)
		{

			float newEnergyPoints = currentEnergPoints - amount;
			currentEnergPoints = Mathf.Clamp(newEnergyPoints, 0, maxEnergyPoint);
			UpdateEnergyBar();
		}


		public void UpdateEnergyBar()
		{
			energyBar.fillAmount = EnergyAsPercentage;
		}

		public void AttempotsSpecialAbility(int abilittIndex, GameObject target = null)
		{

			float energyCost = abilities[abilittIndex].GetEnergyCost();
			if (energyCost <= currentEnergPoints)//TODO read from ability
			{

				ConsumeEnergy(energyCost);
				print("using ability" + abilittIndex);

				abilities[abilittIndex].Use(target);
			}
			else
			{

				audioSource.PlayOneShot(outOfMana);
			}
		}
		public int GetNumberOfAbilitys()
		{
			return abilities.Length;
		}
	}

}
