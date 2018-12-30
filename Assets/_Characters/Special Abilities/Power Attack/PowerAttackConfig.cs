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

		public override void AttachComponentTo(GameObject GoToAttach)
		{
			var behaviorComponent = GoToAttach.AddComponent<PowerAttackBehavior>();
			behaviorComponent.setConfing(this);
			behaviour = behaviorComponent;
		}

		public float GetExtraDamage()
		{
			return extraDamage;
		}
	}

}
