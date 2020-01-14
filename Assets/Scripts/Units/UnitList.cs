using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitList : MonoBehaviour
{
	public List<Unit> units;

	public List<Unit> GetBuildables(List<BuilderUnit> factories)
	{
		bool buildable = false;
		List<Unit> buildableUnits = new List<Unit>();
		foreach (Unit unit in units)
		{
			if (unit.categories != null)
			{
				foreach (BuilderUnit factory in factories)
				{
					foreach (Categories category in factory.buildableCategories)
					{
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
		}
		return buildableUnits;
	}
}
