using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BuildIcon : MonoBehaviour
{
	public Texture buildIcon;
	public Unit unit;
	public List<FactoryUnit> builders;

	public void IssueBuildOrder()
	{
		
		foreach(FactoryUnit factory in builders)
		{
			print("Building");
			factory.AddToQueue(unit);
		}
	}


	public void SetUpIcon()
	{
		gameObject.GetComponent<RawImage>().texture = buildIcon;
	}
}
