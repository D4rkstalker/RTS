using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class Marker : MonoBehaviour
{
	public int numUnits = 1;

	public virtual void OnCreateMarker()
	{
		StartCoroutine(UpdateLoop());
	}

	public IEnumerator UpdateLoop()
	{
		while (true)
		{
			UpdateMarker();
			yield return null;//new WaitForSeconds(GlobalSettings.GameSpeed);

		}
	}
	public virtual void UpdateMarker()
	{
		if (numUnits <= 0)
		{
			Destroy(gameObject);
		}
	}

	public void Start()
	{
		OnCreateMarker();
	}

}
