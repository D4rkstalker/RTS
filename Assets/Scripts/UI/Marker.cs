using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class Marker : MonoBehaviour
{
	public int numUnits = 1;

	public AIMain AIBrain;

	public virtual void OnCreateMarker()
	{
		StartCoroutine(UpdateLoop());
	}

	public IEnumerator UpdateLoop()
	{
		while (true)
		{
			UpdateMarker();
			yield return new WaitForSeconds(GlobalSettings.GameSpeed);

		}
	}
	public virtual void UpdateMarker()
	{
		if (numUnits <= 0)
		{
			DeleteMarker();
		}
	}

	public void Start()
	{
		OnCreateMarker();
	}

	public virtual void DeleteMarker()
	{
		Destroy(gameObject);
	}

}
