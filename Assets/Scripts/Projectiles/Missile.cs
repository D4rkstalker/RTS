using Assets.Scripts;
using Assets.Scripts.ScriptingUtilities;
using UnityEngine;

public class Missile : Projectile
{
	public float turnRate = 1;
	public override void ProjectileUpdate()
	{
		base.ProjectileUpdate();
		if (target != null)
		{
			PointToTarget(target);
			GetComponent<Rigidbody>().AddForce(transform.forward * velocity, ForceMode.Impulse);
		}
	}

	public void PointToTarget(Unit target)
	{
		Vector3 interceptPoint = target.transform.position;
		interceptPoint = ScriptingUtilities.FirstOrderIntercept(
			transform.position,
			gameObject.GetComponent<Rigidbody>() ? gameObject.GetComponent<Rigidbody>().velocity : Vector3.zero,
			velocity,
			target.transform.position,
			target.GetComponent<MobileUnit>() ? target.GetComponent<MobileUnit>().agent.velocity : Vector3.zero
		);

		Vector3 _direction = (interceptPoint - transform.position).normalized;

		Quaternion _lookRotation = Quaternion.LookRotation(_direction);

		transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * turnRate);

	}

}
