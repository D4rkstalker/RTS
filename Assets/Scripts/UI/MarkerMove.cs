using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerMove : Marker
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<MobileUnit>())
		{
			other.GetComponent<MobileUnit>().UpdateMarker(this);
		}
	}
}
