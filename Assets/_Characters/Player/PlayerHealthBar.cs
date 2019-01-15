using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Character
{
	//TODO remobe
	[RequireComponent(typeof(Image))]
	public class PlayerHealthBar : MonoBehaviour
	{
		Image healtOrbImage;
		Player player;

		// Use this for initialization
		void Start()
		{
			player = FindObjectOfType<Player>();
			healtOrbImage = GetComponent<Image>();
		}

		// Update is called once per frame
		void Update()
		{
			//healtOrbImage.fillAmount = player.healthAsPercentage;
		}
	}

}
