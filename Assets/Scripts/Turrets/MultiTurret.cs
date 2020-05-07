using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiTurret : Turret
{
	public List<Muzzle> muzzles;
	public bool cycle;

	private int current = 0;

	public override void Fire()
	{
		if (cycle)
		{
			Muzzle firePoint = muzzles[current];

			Projectile bullet = Instantiate(projectile, firePoint.transform.position, transform.rotation) as Projectile;
			bullet.damage = damage;
			bullet.transform.Rotate(new Vector3(0, Random.Range(firingVariation, -firingVariation), 0));
			bullet.player = parentUnit.playerID;
			bullet.timeOut *= projectileLifetimeMulti;
			if (bullet.tracking)
			{
				bullet.target = targetUnit;
			}
			bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * projectileVelocity, ForceMode.VelocityChange);
			current++;
			if (current > muzzles.Count-1)
			{
				current = 0;
			}
		}
		else
		{
			foreach (Muzzle firePoint in muzzles)
			{
				Projectile bullet = Instantiate(projectile, firePoint.transform.position, transform.rotation) as Projectile;
				bullet.damage = damage;
				bullet.transform.Rotate(new Vector3(0, Random.Range(firingVariation, -firingVariation), 0));
				bullet.player = parentUnit.playerID;
				bullet.timeOut *= projectileLifetimeMulti;
				if (bullet.tracking)
				{
					bullet.target = targetUnit;
				}
				bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * projectileVelocity, ForceMode.VelocityChange);

			}
		}

	}
}
