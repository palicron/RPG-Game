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


		public override void AttachComponentTo(GameObject GoToAttach)
		{
			var behaviorComponent = GoToAttach.AddComponent<AreaOfEffectBehavior>();
			behaviorComponent.setConfing(this);
			behaviour = behaviorComponent;
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
