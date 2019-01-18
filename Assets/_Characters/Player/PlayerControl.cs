using System.Collections;
using UnityEngine;
using RPG.CameraUI; 

namespace RPG.Character
{
	public class PlayerControl : MonoBehaviour
	{

		Character charater;
		EnemyAI CurrentEnemy = null;
		SpecialAbilities specialAbilitys;
		WeaponSystem weaponSystem;

		void Start()
		{
			charater = GetComponent<Character>();		
			weaponSystem = GetComponent<WeaponSystem>();
			specialAbilitys = GetComponent<SpecialAbilities>();

			RegisterForMouseEvent();
		}
		void Update()
		{
			var healthAsPercentage = GetComponent<HealthSystem>().healthAsPercentage;
			if (healthAsPercentage > Mathf.Epsilon)
			{
				ScanForAbilityKeyPress();
			}
		}
		private void RegisterForMouseEvent()
		{
			var cameraRaycaster = FindObjectOfType<CameraRaycaster>();
			cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
			cameraRaycaster.OnMouseOverPotetiallWalkable += OnMouseOverPotencialWalkable;
		}

		void OnMouseOverPotencialWalkable(Vector3 destination)
		{
			if (Input.GetMouseButton(0))
			{
				//StopAllCoroutines();
				weaponSystem.StopAttacking();
				charater.SetDestination(destination);
			}
		}

		void OnMouseOverEnemy(EnemyAI enemy)
		{
			CurrentEnemy = enemy;
			if (Input.GetMouseButtonDown(0) && IsTargetInrange(enemy.gameObject))
			{
				weaponSystem.AttackTarget(enemy.gameObject);
			}
			else if(Input.GetMouseButtonDown(0) && !IsTargetInrange(enemy.gameObject))
			{
				StartCoroutine(MoveToattack(enemy.gameObject));
			}
			else if (Input.GetMouseButtonDown(1) && IsTargetInrange(enemy.gameObject))
			{
				specialAbilitys.AttempotsSpecialAbility(0, CurrentEnemy.gameObject);
			}
			else if (Input.GetMouseButtonDown(1) && !IsTargetInrange(enemy.gameObject))
			{
				StartCoroutine(MoveToSpecialAbility(enemy.gameObject));
			}
		}


		private void ScanForAbilityKeyPress()
		{
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				specialAbilitys.AttempotsSpecialAbility(1);

			}
			else if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				specialAbilitys.AttempotsSpecialAbility(2);
			}
		}

		private bool IsTargetInrange(GameObject target)
		{
			float disttanceTotarget = (target.transform.position - transform.position).magnitude;

			return disttanceTotarget <= weaponSystem.GetCurrentWeapongConfig().MaxAttackRange;

		}

		IEnumerator MoveToTarget(GameObject target)
		{
			while (!IsTargetInrange(target))
			{
				charater.SetDestination(target.transform.position);
				yield return new WaitForEndOfFrame();
			}
			yield return new WaitForEndOfFrame();
		}
		IEnumerator MoveToattack(GameObject target)
		{
			yield return StartCoroutine(MoveToTarget(target));
			weaponSystem.AttackTarget(target);
		}

		IEnumerator MoveToSpecialAbility(GameObject target)
		{
			yield return StartCoroutine(MoveToTarget(target));
			specialAbilitys.AttempotsSpecialAbility(0, CurrentEnemy.gameObject);
		}



	}
}
