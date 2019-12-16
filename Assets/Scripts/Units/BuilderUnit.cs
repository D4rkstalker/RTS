using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderUnit : MonoBehaviour
{
	public Buildpoint buildPoint;
	public float buildPower;
	public List<string> buildableCategories;
	public int player;

	[System.NonSerialized]
	public ResourceManager resourceManager;
	[System.NonSerialized]
	public List<Unit> buildQueue = new List<Unit>();
	[System.NonSerialized]
	public Unit currentUnit;
	[System.NonSerialized]
	public float currentBuild;
	[System.NonSerialized]
	public bool paused, repeat, building;

	private float eCost;
	private float mCost;

	void Start()
	{
		OnCreate();
	}

	void Update()
	{
		UpdateUnit();
	}

	public virtual void OnCreate()
	{
		player = gameObject.GetComponent<Unit>().player;
		GameObject[] results = GameObject.FindGameObjectsWithTag("GameManagers");
		foreach (GameObject result in results)
		{
			if (result.GetComponent<Player>().playerID == player)
			{
				resourceManager = result.GetComponent<ResourceManager>();
			}
		}
	}

	public virtual void UpdateUnit()
	{
		if (!building && currentUnit)
		{
			OnStartBuild();
		}
		if (!currentUnit && buildQueue.Count > 0)
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
			if (currentBuild >= currentUnit.buildtime)
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
		buildQueue.RemoveAt(0);
		CreateUnit(currentUnit);
		if (repeat)
		{
			buildQueue.Add(currentUnit);
		}
		currentUnit = null;
	}

	public virtual void CreateUnit(Unit unitToSpawn)
	{
		Unit unit = Instantiate(unitToSpawn, buildPoint.transform.position, transform.rotation) as Unit;
		unit.player = player;
		unit.OnCreate();
	}

}
