using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BuildController : MonoBehaviour
{
	public List<Unit> buildable;
	public GameObject buildQueuePanel;
	public GameObject buildablePanel;
	public BuildIcon buildIcon;

	public List<BuildIcon> activeBuildIcons;
	public void PopulateBuildableList(List<FactoryUnit> factories)
	{

		buildable = gameObject.GetComponent<UnitList>().GetBuildables(factories);

		foreach (Unit buildableUnit in buildable)
		{
			BuildIcon icon = Instantiate(buildIcon, transform.position, transform.rotation) as BuildIcon;
			icon.unit = buildableUnit;
			icon.buildIcon = buildableUnit.icon;
			icon.SetUpIcon();
			icon.transform.SetParent(buildablePanel.transform,false);
			icon.builders = factories;
			activeBuildIcons.Add(icon);
		}
	}

	public void ClearBuildIcons()
	{
		foreach (BuildIcon icon in activeBuildIcons)
		{
			if (icon)
			{
				Destroy(icon.gameObject);
			}
		}
		activeBuildIcons.RemoveAll(item => item == null);
	}
}
