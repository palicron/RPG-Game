using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace RPG.Character
{

	[ExecuteInEditMode]
	public class WeaponPickUpPoint : MonoBehaviour
	{

		[SerializeField] WeaponConfig weapongConfig;
		[SerializeField] AudioClip audioPick;
		AudioSource audioSource;
		// Start is called before the first frame update
		void Start()
		{
			audioSource = GetComponent<AudioSource>();
		}

		// Update is called once per frame
		void Update()
		{
			if(!Application.isPlaying)
			{
				DestroyChildren();
				InstantiateWeapon();
			}
			
		}

		private void DestroyChildren()
		{
			foreach (Transform child in transform)
				DestroyImmediate(child.gameObject);
		}

		private void InstantiateWeapon()
		{
			var weapon = weapongConfig.GetWeaponnPrefab();
			weapon.transform.position = Vector3.zero;
			Instantiate(weapon, gameObject.transform);
		}

		private void OnTriggerEnter(Collider other)
		{
			var weaponSystem = other.gameObject.GetComponent<WeaponSystem>();
			weaponSystem.PutWeaponInHand(weapongConfig);
			audioSource.PlayOneShot(audioPick);
		}
	}

}
