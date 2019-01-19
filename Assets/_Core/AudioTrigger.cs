using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Character;
namespace RPG.Core
{

	public class AudioTrigger : MonoBehaviour
	{
		[SerializeField] AudioClip clip;
		[SerializeField] int layerFilter = 0;
		[SerializeField] float triggerRadius = 5f;
		[SerializeField] bool isOneTimeOnly = true;

		bool hasPlayed = false;
		AudioSource audioSource;
		GameObject player;
		void Start()
		{
			audioSource = gameObject.AddComponent<AudioSource>();
			audioSource.playOnAwake = false;
			audioSource.clip = clip;
			player = FindObjectOfType<PlayerControl>().gameObject;
		}

		private void Update()
		{
			if(!hasPlayed  && Vector3.Distance(player.transform.position,transform.position)<=triggerRadius)
			{
				RequestPlayAudioClip();
			}
		
		}
		void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.tag.Equals("Player"))
			{
				RequestPlayAudioClip();
			}
		}

		void RequestPlayAudioClip()
		{
			if (isOneTimeOnly && hasPlayed)
			{
				return;
			}
			else if (!audioSource.isPlaying)
			{
				audioSource.Play();
				hasPlayed = true;
			}
		}

		void OnDrawGizmos()
		{
			Gizmos.color = new Color(0, 255f, 0, .5f);
			Gizmos.DrawWireSphere(transform.position, triggerRadius);
		}
	}

}
