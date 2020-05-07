using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	public float velocity;
	public bool tracking;
	public float timeOut;
	public int player;
	public Unit target;
	public float damage;
	//Shield | Armor | Hull | Crew
	public float[] damageMulti = new float[4] { 1.0f,1.0f,1.0f,1.0f };
	public float[] piercingMulti = new float[4] { 1.0f, 1.0f, 1.0f, 1.0f };
	public DamageTypes dType;

	public float[] baseDamage = new float[4] { 2.0f, 0.8f, 1.0f, 0.1f };

	private float lifetime = 0;
	void Update()
	{
		ProjectileUpdate();
	}

	public virtual void ProjectileUpdate()
	{
		CheckTimeOut();
	}

	public virtual void CheckTimeOut()
	{
		lifetime++;
		if(lifetime > timeOut)
		{
			Destroy(gameObject);
		}
	}

	public virtual void OnImpact()
	{
		target.OnDamage(DoDamage());
		Destroy(gameObject);
	}

	public virtual float[] DoDamage()
	{
		//Shield | Armor | Hull | Crew
		float[] fullDamage = new float[4];
		if (target.shield > 0.0f)
		{
			//shield
			fullDamage[0] = damage * damageMulti[0] * piercingMulti[0] * baseDamage[0];
		}
		else
		{
			//armor
			fullDamage[1] = damage * damageMulti[1] * piercingMulti[1] * baseDamage[1];
			//hull
			fullDamage[2] = damage * damageMulti[2] * piercingMulti[2] * baseDamage[2] * (1.1f - target.armor/target.maxArmor);
			//crew
			fullDamage[3] = damage * damageMulti[3] * piercingMulti[3] * baseDamage[3] * (1.0f - target.armor / target.maxArmor);

		}
		return fullDamage;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.GetComponent<Unit>() != null)
		{
			if(other.gameObject.GetComponent<Unit>().playerID != player) {
				target = other.gameObject.GetComponent<Unit>();
				OnImpact();
			}
		}
	}

}
