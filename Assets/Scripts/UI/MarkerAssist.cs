using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerAssist : Marker
{
	public Unit target;
	public List<Unit> assistees;
	public override void UpdateMarker()
	{
		base.UpdateMarker();
		if (!target)
		{
			Destroy(gameObject);
		}
		else
		{
			transform.position = target.transform.position;
		}
	}

}
