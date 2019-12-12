using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryUnit : Unit
{
	public Buildpoint buildPoint;
	public List<Unit> buildQueue;

	public Unit currentUnit;
	public float currentBuild;
	public bool paused;
	public bool repeat;
	public bool building;
	public float buildPower;
	public List<string> buildableCategories;
	public ResourceManager resourceManager;
	public GameObject rallypoint;
	public MarkerMove moveMarker;


	private float eCost;
	private float mCost;

	public override void OnCreate()
	{
		base.OnCreate();
		GameObject[] results = GameObject.FindGameObjectsWithTag("GameManagers");
		foreach(GameObject result in results)
		{
			if(result.GetComponent<Player>().playerID == player)
			{
				resourceManager = result.GetComponent<ResourceManager>();
			}
		}
	}

	public override void UpdateUnit()
	{
		base.UpdateUnit();
		if(!building && currentUnit)
		{
			OnStartBuild();
		}
		if(!currentUnit && buildQueue.Count > 0)
		{
			currentUnit = buildQueue[0];
		}
	}

	public virtual void AddToQueue(Unit unit)
	{
		buildQueue.Add(unit);
	}

	public virtual void OnStartBuild()
	{
		building = true;
		eCost = -(currentUnit.energy * buildPower / currentUnit.buildtime);
		mCost = -(currentUnit.mass * buildPower / currentUnit.buildtime);
		StartCoroutine(BuildTick());
	}

	public virtual IEnumerator BuildTick()
	{
		while (currentUnit)
		{
			if (resourceManager.CheckAmount(eCost, mCost) && !paused)
			{
				currentBuild += buildPower;
			}
			if(currentBuild >= currentUnit.buildtime)
			{
				OnUnitBuilt();
				break;
			}
			yield return new WaitForSeconds(0.1f);
		}

		yield return new WaitForSeconds(0.1f);
	}

	public virtual void OnUnitBuilt()
	{
		building = false;
		currentBuild = 0;
		Unit unit = Instantiate(currentUnit,buildPoint.transform.position, transform.rotation) as Unit;
		unit.player = player;
		unit.GetComponent<MobileUnit>().OnCreate();
		MarkerMove marker = Instantiate(moveMarker, rallypoint.transform.position, transform.rotation) as MarkerMove;
		marker.numUnits = 1;
		
		unit.GetComponent<MobileUnit>().AddMarker(null, marker, false, Tasks.Moving);

		buildQueue.RemoveAt(0);
		if (repeat)
		{
			buildQueue.Add(currentUnit);
		}
		currentUnit = null;
	}

	public void SetRallyPoint(Vector3 point)
	{
		print("changing rally point");
		rallypoint.transform.position = point;
	}
}
