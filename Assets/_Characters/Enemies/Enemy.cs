using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RPG.Core;

namespace RPG.Character
{
	public class Enemy : MonoBehaviour, IDamagaeble
	{

		[SerializeField] float maxHealhPoints = 100f;
		float currentHelhPoints;
		[SerializeField] float attackRadius = 1f;
		[SerializeField] float moveRadius = 2f;
		[SerializeField] float firingPeriodInS = 0.5f;
		[SerializeField] float firingPeriodVariation = 0.1f;
		[SerializeField] float damagePerShot = 5f;
		[SerializeField] GameObject proyectileToUse;
		[SerializeField] GameObject proyectailSocket;
		[SerializeField] Vector3 AimOffSet = new Vector3(0, 1f, 0);
	
	
		Player player = null;
		bool isAttacking = false;

		public float healthAsPercentage
		{
			get
			{
				return currentHelhPoints / maxHealhPoints;
			}
		}


		private void Start()
		{
			player = FindObjectOfType<Player>();
			
			
			currentHelhPoints = maxHealhPoints;
		}
		private void Update()
		{
			if(player.healthAsPercentage <=Mathf.Epsilon)
			{
				StopAllCoroutines();
				Destroy(this);
			}
			float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);


			if (distanceToPlayer <= moveRadius)
			{
				if (distanceToPlayer >= attackRadius)
				{
					//aiCharacterControl.SetTarget(player.transform);

				}
				//else
					//aiCharacterControl.SetTarget(transform);
				//TODO fire
			}



			if (distanceToPlayer <= attackRadius && !isAttacking)
			{
				isAttacking = true;
				float randomiseDelay = Random.Range(firingPeriodInS - firingPeriodVariation, firingPeriodInS + firingPeriodVariation);
				InvokeRepeating("FireProjectile", 0f, randomiseDelay);

			}
			else if (distanceToPlayer > attackRadius && isAttacking)
			{
				CancelInvoke();
				isAttacking = false;
			}
		}
		public void TakeDamage(float damage)
		{
			currentHelhPoints = Mathf.Clamp(currentHelhPoints - damage, 0f, maxHealhPoints);
			if (currentHelhPoints <= 0)
				Destroy(gameObject);
		}

		void FireProjectile()
		{
			Projectile projectileComponen = spwannProyectile();

			Vector3 unitVectorToPLayer = (player.transform.position + AimOffSet - proyectailSocket.transform.position).normalized;
			projectileComponen.GetComponent<Rigidbody>().velocity = unitVectorToPLayer * projectileComponen.GetDefaultfaultLauchSpeed();
		}

		private Projectile spwannProyectile()
		{
			GameObject newproyectail = Instantiate(proyectileToUse, proyectailSocket.transform.position, Quaternion.identity);
			Projectile projectileComponen = newproyectail.GetComponent<Projectile>();
			projectileComponen.setDmg(damagePerShot);
			projectileComponen.setShootre(this.gameObject);
			return projectileComponen;
		}

		void OnDrawGizmos()
		{

			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, attackRadius);
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere(transform.position, moveRadius);
		}
	}
}
