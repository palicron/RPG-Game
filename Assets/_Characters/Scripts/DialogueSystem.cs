using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{

	[SerializeField] AudioClip[] dialogueClips;
	[SerializeField] [Range(0f, 1f)] float volume=0.5f;
	[SerializeField] float DialogueRange = 2f;
	AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
		setAudioSOurce();

	}

	private void setAudioSOurce()
	{
		audioSource = GetComponent<AudioSource>();
		if(audioSource==null)
		{
			audioSource = this.gameObject.AddComponent<AudioSource>();
		}
		audioSource.volume = volume;
	}
	
	public void PlayDialogue()
	{
		if(!audioSource.isPlaying)
		{
			AudioClip clip = dialogueClips[UnityEngine.Random.Range(0, dialogueClips.Length)];
			audioSource.PlayOneShot(clip);
		}
	}

	public float getDialogueRange()
	{
		return DialogueRange;
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, DialogueRange);
	}
}
