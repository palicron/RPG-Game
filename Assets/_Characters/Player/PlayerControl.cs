
using UnityEngine;

using RPG.CameraUI; //TODO consider RE-write

namespace RPG.Character
{
	public class PlayerControl : MonoBehaviour
	{

		CameraRaycaster cameraRaycaster;
		Character charater;
		Enemy CurrentEnemy = null;
		SpecialAbilities specialAbilitys;
		WeaponSystem weaponSystem;






		private void Start()
		{
			charater = GetComponent<Character>();
			RegisterForMouseEvent();
			weaponSystem = GetComponent<WeaponSystem>();
			specialAbilitys = GetComponent<SpecialAbilities>();
		}

		private void RegisterForMouseEvent()
		{
			cameraRaycaster = FindObjectOfType<CameraRaycaster>();
			cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
			cameraRaycaster.OnMouseOverPotetiallWalkable += OnMouseOverPotencialWalkable;
		}

		void OnMouseOverPotencialWalkable(Vector3 destination)
		{
			if(Input.GetMouseButton(0))
			{
				charater.SetDestination(destination);
			}
		}

		void OnMouseOverEnemy(Enemy enemy)
		{
			CurrentEnemy = enemy;
			if (Input.GetMouseButtonDown(0) && IsTargetInrange(enemy.gameObject))
			{
				weaponSystem.AttackTarget(enemy.gameObject);
			}
			else if (Input.GetMouseButtonDown(1))
			{
				specialAbilitys.AttempotsSpecialAbility(0, CurrentEnemy.gameObject);
			}
		}
		private void Update()
		{
			var healthAsPercentage = GetComponent<HealthSystem>().healthAsPercentage;
			if (healthAsPercentage > Mathf.Epsilon)
			{
				ScanForAbilityKeyPress();
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















	}
}
