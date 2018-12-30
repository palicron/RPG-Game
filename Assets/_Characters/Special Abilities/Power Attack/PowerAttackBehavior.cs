﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Character
{

	public class PowerAttackBehavior : MonoBehaviour, ISpecialAbility
	{
		PowerAttackConfig config;

		// Start is called before the first frame update
		void Start()
		{
			print("Power attack behacior attach to " + gameObject.name);
		}

		// Update is called once per frame
		void Update()
		{

		}
		public void Use(AbilityUseParams useParams)
		{
			useParams.target.TakeDamage(useParams.baseDamage + config.GetExtraDamage());
			
		}

		public void setConfing(PowerAttackConfig config)
		{
			this.config = config;
		}
	}

}
