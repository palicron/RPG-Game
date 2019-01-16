using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace RPG.Character
{
	public class HealthSystem : MonoBehaviour
	{
		const string DEATH_TRIGGER = "Death";

		[SerializeField] float maxHealhPoints = 100f;
		[SerializeField] Image healtBar;
		[SerializeField] AudioClip[] DamgeSounds;
		[SerializeField] AudioClip[] DeathSounds;
		[SerializeField] float deathVanishSeconds = 2f;

		float currentHelhPoints = 100f;

		Animator animator;
		AudioSource audioSource;
		Character characterMovement;
		//TODO character kill

		public float healthAsPercentage { get { return currentHelhPoints / maxHealhPoints; } }


		void Start()
		{
			animator = GetComponent<Animator>();
			audioSource = GetComponent<AudioSource>();
			characterMovement = GetComponent<Character>();
		}

		void Update()
		{
			UpdateHealthBar();
		}

		private void UpdateHealthBar()
		{
			if (healtBar)
			{
				healtBar.fillAmount = healthAsPercentage;
			}
		}

		public void TakeDamage(float damage)
		{
			bool characterDies = (currentHelhPoints - damage <= 0);
			currentHelhPoints = Mathf.Clamp(currentHelhPoints - damage, 0f, maxHealhPoints);
			var clip = DamgeSounds[UnityEngine.Random.Range(0, DamgeSounds.Length)];
			audioSource.PlayOneShot(clip);
			if (characterDies)
			{
				StartCoroutine(KillCharacter());
			}

		}
		public void Heal(float points)
		{
			currentHelhPoints = Mathf.Clamp(currentHelhPoints + points, 0f, maxHealhPoints);
		}


		IEnumerator KillCharacter()
		{
			StopAllCoroutines();
			characterMovement.Kill();
			animator.SetTrigger(DEATH_TRIGGER);
			var playerComponet = GetComponent<PlayerControl>();
			if (playerComponet && playerComponet.isActiveAndEnabled)
			{
				audioSource.Stop();
				audioSource.clip = DeathSounds[UnityEngine.Random.Range(0, DeathSounds.Length)];
				audioSource.Play();
				yield return new WaitForSecondsRealtime(audioSource.clip.length + 0.1f);//todo use audiclipo lenth
				SceneManager.LoadSceneAsync(0);
			}
			else
			{
				Destroy(this.gameObject, deathVanishSeconds);
			}
		}
	}
}
