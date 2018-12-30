using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
namespace RPG.Weapons
{

public class Projectile : MonoBehaviour {
	[SerializeField]
	 float proyectileSpeed = 1;
	[SerializeField] GameObject shooter;
	 float damageCaused;//otehr cann set

	const float DESTRPOID_DELAY=0f;
	public void setShootre(GameObject shooter)
	{
		this.shooter = shooter;
	}

	// Update is called once per frame
	void Update () {
		
	}

	public float GetDefaultfaultLauchSpeed()
	{
		return proyectileSpeed;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.layer != shooter.layer)
		{
			Component damageable = collision.gameObject.GetComponent(typeof(IDamagaeble));
			if (damageable)
			{
				(damageable as IDamagaeble).TakeDamage(damageCaused);
			}
		}
		Destroy(gameObject, DESTRPOID_DELAY);
	}

	public void setDmg(float dmg)
	{
		damageCaused = dmg;
	}
}

}
