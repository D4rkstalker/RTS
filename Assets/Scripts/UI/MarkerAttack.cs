using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerAttack : Marker
{
	public Unit target;
	public List<Unit> attackers;

	public override void UpdateMarker()
	{
		base.UpdateMarker();
		if (!target)
		{
			foreach(Unit unit in attackers)
			{
				unit.OnEnemyKill(this);
			}
			Destroy(gameObject);
		}
		else
		{
			transform.position = target.transform.position;
		}
	}
}
