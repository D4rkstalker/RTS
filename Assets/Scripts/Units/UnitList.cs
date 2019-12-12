using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitList : MonoBehaviour
{
	public List<Unit> units;

	public List<Unit> GetBuildables(List<FactoryUnit> factories)
	{
		bool buildable = false;
		List<Unit> buildableUnits = new List<Unit>();
		foreach (Unit unit in units)
		{
			foreach (FactoryUnit factory in factories)
			{

				foreach (string category in factory.buildableCategories)
				{

					//print(unit.categories[0]);
					if (unit.categories.Contains(category))
					{
						buildable = true;
						//print(category);
					}
					else
					{
						buildable = false;
						break;
					}
				}
			}
			if (buildable)
			{
				buildableUnits.Add(unit);
				buildable = false;
			}
		}
		return buildableUnits;
	}
}
