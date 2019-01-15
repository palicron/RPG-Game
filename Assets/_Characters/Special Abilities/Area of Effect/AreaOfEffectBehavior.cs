using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using System;

namespace RPG.Character
{
	public class AreaOfEffectBehavior : AbilityBehavior
	{
	
		Vector3 MousepOsition;
		public override void Use(GameObject target)
		{
			DefineMousePosition();
			DealRadialDamage();
			PlayParticalEffect(MousepOsition,false);
			PlayAbilitySound();
		}

		private void DefineMousePosition()
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;
			Physics.Raycast(ray, out hitInfo, 100f);
			MousepOsition = hitInfo.point;
		}


		private void DealRadialDamage( )
		{
			//static sphere
			RaycastHit[] hits = Physics.SphereCastAll(
				MousepOsition
				,(config as AreaOfEffectConfig).GetMaximunRadius(),
				Vector3.up,
				(config as AreaOfEffectConfig).GetMaximunRadius());

			foreach (RaycastHit hit in hits)
			{
				var damageable = hit.collider.gameObject.GetComponent<HealthSystem>();
				if (damageable != null && !hit.collider.gameObject.tag.Equals("Player"))
				{
					float damageTodeal = (config as AreaOfEffectConfig).GetExtraDamage();
					damageable.TakeDamage(damageTodeal);
				}
			}
		}

	
	}
}
