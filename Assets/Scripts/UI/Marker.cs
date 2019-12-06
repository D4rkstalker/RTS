using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{
	public int numUnits = 1;

	void Update()
	{
		UpdateMarker();
		
	}
	public virtual void UpdateMarker()
	{
		if (numUnits <= 0)
		{
			Destroy(gameObject);
		}
	}
}
