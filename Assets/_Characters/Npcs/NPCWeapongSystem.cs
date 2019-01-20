using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;
using RPG.Character;
namespace RPG.NPC
{

	public class NPCWeapongSystem : MonoBehaviour
	{

		[SerializeField] WeaponConfig currentWeaponConfig;

		GameObject weapongObject;

		private void Start()
		{
			PutWeaponInHand(currentWeaponConfig);
		}
		 void PutWeaponInHand(WeaponConfig weapongConfig)
		{
			currentWeaponConfig = weapongConfig;
			GameObject dominantHand = RequestDominatHands();
			var weaponprefab = weapongConfig.GetWeaponnPrefab();
			
			if (weapongObject != null)
				Destroy(weapongObject);

			weapongObject = Instantiate(weaponprefab, dominantHand.transform);
			weapongObject.transform.localPosition = currentWeaponConfig.gripTrasform.localPosition;
			weapongObject.transform.localRotation = currentWeaponConfig.gripTrasform.localRotation;
		}

		private GameObject RequestDominatHands()
		{
			var dominanHands = GetComponentsInChildren<DominnannHand>();
			int numDominanHands = dominanHands.Length;
			Assert.AreNotEqual(numDominanHands, 0, "No dominat hand,add one");
			Assert.IsFalse(numDominanHands > 1, "Multiple hands on player");
			return dominanHands[0].gameObject;
		}
	}

}
