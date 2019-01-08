using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Character
{
	[CreateAssetMenu(menuName = ("RPG/Special Ability/Selft Heal"))]
	public class SelftHealConfig : SpecialAbilityConfig
	{
		[Header("Power Attack Specifics")]
		[SerializeField] float extraHeal = 10f;


		public float GetExtraHeal()
		{
			return extraHeal;
		}

		public override AbilityBehavior getBehaviorCOmponent(GameObject objetToAttachTo)
		{
			return objetToAttachTo.AddComponent<SelftHealBehavior>();
		}
	}

}
