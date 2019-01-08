using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Character
{
	[CreateAssetMenu(menuName = ("RPG/Special Ability/Area Of Effect"))]
	public class AreaOfEffectConfig : SpecialAbilityConfig
	{

		[Header("Area Effect Specifics")]
		[SerializeField] float radius = 5f;
		[SerializeField] float damageToEachTarget = 5f;



		public override AbilityBehavior getBehaviorCOmponent(GameObject objetToAttachTo)
		{
			return objetToAttachTo.AddComponent<AreaOfEffectBehavior>();
		}
		public float GetExtraDamage()
		{
			return damageToEachTarget;
		}
		public float GetMaximunRadius()
		{
			return radius;
		}
	}
}
