using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
namespace RPG.Character
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
			//TODO reiplement
		//if(collision.gameObject.layer != shooter.layer)
		//{
		//	Component damageable = collision.gameObject.GetComponent(typeof(HealthSystem));
		//	if (damageable)
		//	{
		//		(damageable as HealthSystem).TakeDamage(damageCaused);
		//	}
		//}
		//Destroy(gameObject, DESTRPOID_DELAY);
	}

	public void setDmg(float dmg)
	{
		damageCaused = dmg;
	}
}

}
