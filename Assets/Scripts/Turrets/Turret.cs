using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Utilities;
using Assets.Scripts;

public class Turret : MonoBehaviour
{
	public bool turretEnabled = true;
	public float maxRange;
	public float minRange;
	public float chargeTime;
	public float rateOfFire;
	public float salvoSize = 1;
	public float salvoFireRate;
	public float turnRate;
	public float maxTurnAngle;
	public float trackRangeMulti = 1;
	public bool leadTarget = true;
	public float projectileVelocity;
	public float projectileLifetimeMulti = 1;
	public float damage;
	public float firingVariation;
	public bool mainGun;
	public float firingTolerance = 90;

	public Unit parentUnit;
	public Projectile projectile;
	public Muzzle muzzle;

	public TurretTypes tType;

	protected Unit targetUnit;
	protected bool firing = false;

	void Start()
	{
		targetUnit = null;
	}

	// Update is called once per frame
	void Update()
	{
		CheckFire();
	}

	public void PointToTarget(Unit target)
	{
		Vector3 interceptPoint = target.transform.position;
		if (leadTarget)
		{
			interceptPoint = Utilities.FirstOrderIntercept(
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

			if (Vector3.Distance(targetUnit.transform.position, transform.position) < maxRange * trackRangeMulti)
			{

				PointToTarget(targetUnit);

			}
			if (!firing && Vector3.Distance(targetUnit.transform.position, transform.position) < maxRange && Vector3.Distance(targetUnit.transform.position, transform.position) > minRange)
			{

				firing = true;
				StartCoroutine(InitFiringSequence());
			}

		}

	}

	public virtual IEnumerator InitFiringSequence()
	{
		while (targetUnit != null && Vector3.Distance(targetUnit.transform.position, transform.position) < maxRange && Vector3.Distance(targetUnit.transform.position, transform.position) > minRange)
		{
			float angle = Vector3.Angle(transform.forward, targetUnit.transform.position - transform.position );
			if(angle < firingTolerance)
			{
				
				for (int i = 0; i < salvoSize; i++)
				{
					Invoke("Fire", chargeTime);
					yield return new WaitForSeconds(salvoFireRate);
				}
				yield return new WaitForSeconds(rateOfFire);
			}
			yield return null;
		}
		firing = false;
		targetUnit = null;
	}

	public virtual void Fire()
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


	private List<Unit> GetTargets()
	{
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, maxRange);
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


}
