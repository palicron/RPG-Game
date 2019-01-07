using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Character
{

	public abstract class AbilityBehavior : MonoBehaviour
	{

		SpecialAbilityConfig config;

		public abstract void Use(AbilityUseParams useParams);
	}

}
