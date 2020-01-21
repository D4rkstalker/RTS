using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BuildIcon : MonoBehaviour
{
	public Texture buildIcon;
	public Unit unit;
	public List<BuilderUnit> builders;
	public MarkerBuild markerBuild;

	public InputController ic;

	public void IssueBuildOrder()
	{

		foreach (BuilderUnit constructor in builders)
		{
			if(constructor is FactoryUnit)
			{
				constructor.AddToQueue(unit);

			}
			else if (constructor is ConstructionUnit)
			{
				MarkerBuild marker = Instantiate(markerBuild, transform.position, transform.rotation) as MarkerBuild;
				marker.unitToBuild = unit;
				marker.builders = builders;
				marker.numUnits = builders.Count;
				ic.currentMarker = marker;
				ic.currentMode = CurrentMode.building;
			}
		}
	}


	public void SetUpIcon()
	{
		gameObject.GetComponent<RawImage>().texture = buildIcon;
	}
}
