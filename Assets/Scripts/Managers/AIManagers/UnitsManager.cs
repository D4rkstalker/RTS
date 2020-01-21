using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsManager : MonoBehaviour
{
	public bool active = false;
	public int player;
	public List<FactoryUnit> factories;
	private UnitList unitList;

	public virtual void BuildUnits(int count, List<Categories> categories, Vector3 rallyPoint)
	{
		List<Unit> units = unitList.GetUnitByCategory(categories);
		int unitIndex = 0;
		for(int i = 0; i<count; i += 0)
		{
			foreach(FactoryUnit factory in factories)
			{
				if (i < count)
				{
					factory.AddToQueue(units[unitIndex]);
					factory.SetRallyPoint(rallyPoint);
					i++;
					if(unitIndex < units.Count)
					{
						unitIndex++;
					}
					else
					{
						unitIndex = 0;
					}
				}
				else
				{
					return;
				}
			}
		}
	}
	public virtual void UnitsAIUpdate()
	{
		factories = AIUtilities.GetAllFactoriesOnMap(player);
	}

	void Start()
	{
		InitAIBrain();
		StartCoroutine(UnitsAIUpdateLoop());
	}

	public virtual void InitAIBrain()
	{
		GameObject result = GameObject.FindGameObjectWithTag("GameManager");
		unitList = result.GetComponent<UnitList>();
		
		player = gameObject.transform.parent.GetComponent<Player>().playerID;
	}

	IEnumerator UnitsAIUpdateLoop()
	{
		while (true)
		{
			UnitsAIUpdate();
			yield return new WaitForSeconds(GlobalSettings.GameSpeed * 10);
		}
	}

}
