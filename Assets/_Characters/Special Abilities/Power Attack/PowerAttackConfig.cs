using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Character
{
	[CreateAssetMenu(menuName = ("RPG/Special Ability/Power Attack"))]
	public class PowerAttackConfig : SpecialAbilityConfig
	{

		[Header("Power Attack Specifics")]
		[SerializeField] float extraDamage = 5f;


		public override AbilityBehavior getBehaviorCOmponent(GameObject objetToAttachTo)
		{
			return objetToAttachTo.AddComponent<PowerAttackBehavior>();
		}

		public float GetExtraDamage()
		{
			return extraDamage;
		}
	}

}
