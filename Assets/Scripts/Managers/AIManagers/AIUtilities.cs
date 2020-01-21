using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIUtilities : MonoBehaviour
{
	public static List<Unit> GetAllUnitsOnMap(int player)
	{
		List<Unit> units = new List<Unit>();
		Unit[] temp = (Unit[])FindObjectsOfType(typeof(Unit));
		foreach (Unit unit in temp)
		{
			if (unit.player == player && unit.selectable)
			{
				units.Add(unit);
			}
		}
		return units;
	}

	public static List<FactoryUnit> GetAllFactoriesOnMap(int player)
	{
		List<FactoryUnit> units = new List<FactoryUnit>();
		FactoryUnit[] temp = (FactoryUnit[])FindObjectsOfType(typeof(FactoryUnit));
		foreach (FactoryUnit unit in temp)
		{
			if (unit.player == player)
			{
				units.Add(unit);
			}
		}
		return units;
	}


	public static List<Unit> GetAllConstructors(bool idleOnly, int player)
	{
		List<Unit> units = new List<Unit>();
		ConstructionUnit[] temp = (ConstructionUnit[])FindObjectsOfType(typeof(ConstructionUnit));
		foreach (ConstructionUnit cunit in temp)
		{
			if (cunit.self.player == player && cunit.self.selectable)
			{
				if (idleOnly)
				{
					if(cunit.self.task == Tasks.Idle)
					{
						units.Add(cunit.self);
					}
						
				}
				else
				{
					units.Add(cunit.self);
				}
			}
		}
		return units;
	}

	public static List<Unit> IdleCheck(List<Unit> units)
	{
		foreach(Unit unit in units)
		{
			if(unit.task != Tasks.Idle)
			{
				units.Remove(unit); 
			}
		}
		return units;
	}

	public static List<MassDeposit> GetMassDepositsAroundPoint(Vector3 center, int radius, bool unclaimedOnly)
	{
		Collider[] hitColliders = Physics.OverlapSphere(center, radius, LayerMask.GetMask("Units"));
		List<MassDeposit> massPoints = new List<MassDeposit>();
		print(hitColliders.Length);
		foreach (Collider collider in hitColliders)
		{
			MassDeposit massPoint = collider.GetComponent<MassDeposit>();
			if (massPoint)
			{
				if (unclaimedOnly)
				{
					if (!massPoint.claimed)
					{
						massPoints.Add(massPoint);
					}
				}
				else
				{
					massPoints.Add(massPoint);
				}
			}
		}
		return massPoints;
	}

	public static List<MassDeposit> GetAllMassDepositsOnMap()
	{
		List<MassDeposit> massPoints = new List<MassDeposit>();
		massPoints.AddRange((MassDeposit[])FindObjectsOfType(typeof(MassDeposit)));
		return massPoints;
	}
}
