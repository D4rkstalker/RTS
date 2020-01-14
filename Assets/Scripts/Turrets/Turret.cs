using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.ScriptingUtilities;
using Assets.Scripts;
using Lightbug.LaserMachine;

public class Turret : MonoBehaviour
{
	public bool turretEnabled = true;
	public bool mainGun;
	public float maxRange, minRange, chargeTime, rateOfFire, salvoFireRate, turnRate, maxTurnAngle, damage;
	public float salvoSize = 1;
	public float trackRangeMulti = 1;
	public float firingTolerance = 90;

	public Muzzle muzzle;
	public TurretTypes tType;
	public WeaponTypes wType;
	[Header("Kinetic Turret Data")]
	public bool leadTarget = true;
	public float projectileVelocity, firingVariation;
	public float projectileLifetimeMulti = 1;
	public Projectile projectile;
	[Header("Laser Turret Data")]
	public float beamLengthMulti = 1;
	public float beamDuration;

	[System.NonSerialized]
	public Unit parentUnit;

	public Unit targetUnit;
	public bool firing = false;

	void Start()
	{
		parentUnit = transform.parent.gameObject.GetComponent<Unit>();
		targetUnit = null;
		if (wType == WeaponTypes.Beam)
		{
			gameObject.GetComponent<LaserMachine>().SetupLaser(maxRange * beamLengthMulti);
		}
		StartCoroutine(TurretUpdateLoop());
	}

	public virtual void CheckFire()
	{
		if (targetUnit == null)
		{
			PickTarget(GetTargets());
			if (mainGun && targetUnit != null)
			{
				parentUnit.target = targetUnit;
			}
		}
		else
		{
			if (!firing && Vector3.Distance(targetUnit.transform.position, transform.position) < maxRange && Vector3.Distance(targetUnit.transform.position, transform.position) > minRange)
			{
				firing = true;

				StartCoroutine(InitFiringSequence());
			}
			if (Vector3.Distance(targetUnit.transform.position, transform.position) < maxRange * trackRangeMulti)
			{
				PointToTarget(targetUnit);
			}
			else
			{
				OnLostTarget();
			}

		}

	}

	public virtual IEnumerator InitFiringSequence()
	{
		while (targetUnit != null && Vector3.Distance(targetUnit.transform.position, transform.position) < maxRange && Vector3.Distance(targetUnit.transform.position, transform.position) > minRange)
		{
			float angle = Vector3.Angle(transform.forward, targetUnit.transform.position - transform.position);
			if (angle < firingTolerance)
			{

				for (int i = 0; i < salvoSize; i++)
				{
					Invoke("Fire", chargeTime);
					yield return new WaitForSeconds(salvoFireRate);
				}
				yield return new WaitForSeconds(rateOfFire + beamDuration);
			}
			yield return new WaitForSeconds(GlobalSettings.GameSpeed);
		}
		firing = false;
		targetUnit = null;
	}

	public virtual void Fire()
	{
		if (wType == WeaponTypes.Kinetic)
		{
			Projectile bullet = Instantiate(projectile, muzzle.transform.position, transform.rotation) as Projectile;
			bullet.damage = damage;
			bullet.transform.Rotate(new Vector3(0, Random.Range(firingVariation, -firingVariation), 0));
			bullet.player = parentUnit.player;
			bullet.timeOut *= projectileLifetimeMulti;
			if (bullet.tracking)
			{
				bullet.target = targetUnit;
			}
			bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * projectileVelocity, ForceMode.VelocityChange);

		}
		else if (wType == WeaponTypes.Beam)
		{
			StartCoroutine(gameObject.GetComponent<LaserMachine>().Fire(beamDuration, damage, parentUnit.player, maxRange * beamLengthMulti));
		}
	}

	public virtual void OnLostTarget()
	{
		targetUnit = null;
	}
	public IEnumerator TurretUpdateLoop()
	{
		while (true)
		{
			if (parentUnit.isBuilt)
			{
				CheckFire();
			}
			yield return new WaitForSeconds(GlobalSettings.GameSpeed);
		}
	}

	private List<Unit> GetTargets()
	{
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, maxRange * trackRangeMulti);
		if (hitColliders.Length > 0)
		{
			return CheckTargets(hitColliders);
		}
		return new List<Unit>();

	}

	private List<Unit> CheckTargets(Collider[] targets)
	{
		List<Unit> validTargets = new List<Unit>();
		foreach (Collider c in targets)
		{
			Unit target = c.gameObject.GetComponent<Unit>();
			if (target != null)
			{
				if ((target.player != parentUnit.player) && (Vector3.Distance(target.transform.position, parentUnit.transform.position) >= minRange) && target.targetable)
				{
					validTargets.Add(target);
				}
			}

		}
		return validTargets;
	}

	private void PickTarget(List<Unit> targets)
	{

		foreach (Unit target in targets)
		{
			if (target == parentUnit.target)
			{
				targetUnit = target;
				return;
			}
		}
		if (targets.Count > 0)
		{
			targetUnit = targets[0];
		}

	}

	public void PointToTarget(Unit target)
	{
		Vector3 interceptPoint = target.transform.position;
		if (leadTarget)
		{
			interceptPoint = ScriptingUtilities.FirstOrderIntercept(
				transform.position,
				gameObject.GetComponent<MobileUnit>() ? gameObject.GetComponent<MobileUnit>().agent.velocity : Vector3.zero,
				projectileVelocity,
				target.transform.position,
				target.GetComponent<MobileUnit>() ? target.GetComponent<MobileUnit>().agent.velocity : Vector3.zero
			);

		}
		if (mainGun && tType == TurretTypes.Spinal)
		{
			parentUnit.PointToTarget(interceptPoint);
		}
		else
		{
			Vector3 _direction = (interceptPoint - transform.position).normalized;

			Quaternion _lookRotation = Quaternion.LookRotation(_direction);

			transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * turnRate);

		}
	}
}
