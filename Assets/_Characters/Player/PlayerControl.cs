
using UnityEngine;
using UnityEngine.Assertions;
using RPG.CameraUI; //TODO consider RE-write

namespace RPG.Character
{
	public class PlayerControl : MonoBehaviour
	{
		const string ATTACK_TRIGGER = "Attack";
		
		const string DEFAULT_ATTACK = "DEFAULT ATTACK";
	
		[SerializeField] float baseDamage = 10f;
		[SerializeField] Weapon currentWeaponConfig;
		[SerializeField] AnimatorOverrideController animatorOverrideController;
		GameObject weapongObject;
		Enemy CurrentEnemy = null;
		//TODO Temporarily serializi for debug
		private Animator animator;
		//[SerializeField] GameObject WeaponnSocket;
		[Range(0f, 1.0f)] [SerializeField] float criticalHitChance = 0.1f;
		[SerializeField] float criticalHitMultiplier = 1.25f;
		CameraRaycaster cameraRaycaster;
		[SerializeField] ParticleSystem critParticle = null;
		float lastHittime = 0;
		SpecialAbilities specialAbilitys;
		Character charater;
		private void Start()
		{
			charater = GetComponent<Character>();
			RegisterForMouseEvent();
			PutWeaponInHand(currentWeaponConfig);
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
				AttackTarget();
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

		private void SetAttackAnimation()
		{
			animator = GetComponent<Animator>();
			animator.runtimeAnimatorController = animatorOverrideController;
			animatorOverrideController[DEFAULT_ATTACK] = currentWeaponConfig.getAnimClip();
		}

		private GameObject RequestDominatHands()
		{
			var dominanHands = GetComponentsInChildren<DominnannHand>();
			int numDominanHands = dominanHands.Length;
			Assert.AreNotEqual(numDominanHands, 0, "No dominat hand,add one");
			Assert.IsFalse(numDominanHands > 1, "Multiple hands on player");
			return dominanHands[0].gameObject;
		}

	



	



		private void AttackTarget()
		{

			if (Time.time - lastHittime > currentWeaponConfig.MinTimeBetween)
			{
				animator.SetTrigger(ATTACK_TRIGGER); //TODO maeka const
	
				lastHittime = Time.time;
			}
		}

		private float CalculateDamage()
		{
			bool isCriticalHit = UnityEngine.Random.Range(0f, 1f) <= criticalHitChance;
			float damagaBeforeCrit = baseDamage + currentWeaponConfig.GetAdditinalDamage();
			if (isCriticalHit)
			{
				critParticle.Play();
				return damagaBeforeCrit * criticalHitMultiplier;
			}
			else
			{
				return damagaBeforeCrit;
			}

		}

		private bool IsTargetInrange(GameObject target)
		{
			float disttanceTotarget = (target.transform.position - transform.position).magnitude;

			return disttanceTotarget <= currentWeaponConfig.MaxAttackRange;

		}



		public void PutWeaponInHand(Weapon weapongConfig)
		{
			currentWeaponConfig = weapongConfig;
			GameObject dominantHand = RequestDominatHands();
			var weaponprefab = weapongConfig.GetWeaponnPrefab();
			SetAttackAnimation();
			if (weapongObject != null)
				Destroy(weapongObject);

			weapongObject = Instantiate(weaponprefab, dominantHand.transform);
			weapongObject.transform.localPosition = currentWeaponConfig.gripTrasform.localPosition;
			weapongObject.transform.localRotation = currentWeaponConfig.gripTrasform.localRotation;
		}


	}
}
