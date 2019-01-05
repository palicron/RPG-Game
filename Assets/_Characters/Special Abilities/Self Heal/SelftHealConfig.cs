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

		public override void AttachComponentTo(GameObject GoToAttach)
		{
			var behaviorComponent = GoToAttach.AddComponent<SelftHealBehavior>();
			behaviorComponent.setConfing(this);
			behaviour = behaviorComponent;
		}

		public float GetExtraHeal()
		{
			return extraHeal;
		}
	}

}
