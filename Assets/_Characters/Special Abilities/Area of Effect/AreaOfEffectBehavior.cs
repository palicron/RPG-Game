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
		public override void Use(AbilityUseParams useParams)
		{
			DefineMousePosition();
			DealRadialDamage(useParams);
			PlayParticalEffect(MousepOsition,false);
		}

		private void DefineMousePosition()
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;
			Physics.Raycast(ray, out hitInfo, 100f);
			MousepOsition = hitInfo.point;
		}


		private void DealRadialDamage(AbilityUseParams useParams)
		{
			//static sphere
			RaycastHit[] hits = Physics.SphereCastAll(
				MousepOsition
				,(config as AreaOfEffectConfig).GetMaximunRadius(),
				Vector3.up,
				(config as AreaOfEffectConfig).GetMaximunRadius());

			foreach (RaycastHit hit in hits)
			{
				var damageable = hit.collider.gameObject.GetComponent<IDamagaeble>();
				if (damageable != null && !hit.collider.gameObject.tag.Equals("Player"))
				{
					float damageTodeal = useParams.baseDamage + (config as AreaOfEffectConfig).GetExtraDamage();
					damageable.TakeDamage(damageTodeal);
				}
			}
		}

	
	}
}
