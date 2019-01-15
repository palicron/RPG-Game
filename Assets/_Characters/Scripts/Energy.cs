using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System;

namespace RPG.Character
{

	public class Energy : MonoBehaviour
	{

		[SerializeField] Image energyBar = null;
		[SerializeField] float maxEnergyPoint = 100f;
		[SerializeField] float regenPointsPerSecond =1f;

		float currentEnergPoints = 100f;

		public float EnergyAsPercentage
		{
			get
			{
				return currentEnergPoints / maxEnergyPoint;
			}
		}


		// Start is called before the first frame update
		void Start()
		{

			currentEnergPoints = maxEnergyPoint;
			UpdateEnergyBar();
		}
		

		void Update()
		{
			if(currentEnergPoints<maxEnergyPoint)
			{
				AddEnergyPoints();
				UpdateEnergyBar();
			}
		}



		private void AddEnergyPoints()
		{
			var pointsToAdd = regenPointsPerSecond * Time.deltaTime;
			currentEnergPoints = Mathf.Clamp(currentEnergPoints + pointsToAdd, 0, maxEnergyPoint);
		}

		public bool IsEnergyAvailable(float amount)
		{
			return amount <= currentEnergPoints;
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

	
	}

}
