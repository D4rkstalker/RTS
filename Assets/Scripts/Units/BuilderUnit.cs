using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderUnit : MonoBehaviour
{
	public Buildpoint buildPoint;
	public float buildPower;
	public List<Categories> buildableCategories;
	public int player;

	[System.NonSerialized]
	public ResourceManager resourceManager;
	//[System.NonSerialized]
	//[SerializeField]
	public LinkedList<Unit> buildQueue = new LinkedList<Unit>();
	//[System.NonSerialized]
	public Unit currentUnit;
	//[System.NonSerialized]
	public bool paused, repeat;

	protected float eCost;
	protected float mCost;


	public virtual void AssistBuild(Unit unit) {
		currentUnit = unit;
	}


	public virtual void OnCreate()
	{
		player = gameObject.GetComponent<Unit>().player;
		GameObject[] results = GameObject.FindGameObjectsWithTag("PlayerManager");
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
		if (!currentUnit && buildQueue.Count > 0)
		{
			OnStartBuild();
		}
	}

	IEnumerator UpdateBuilderLoop()
	{
		while (true)
		{
			UpdateUnit();
			yield return new WaitForSeconds(GlobalSettings.GameSpeed);
		}
	}

	public virtual void AddToQueue(Unit unit)
	{
		buildQueue.AddLast(unit);
		print(buildQueue.Count);
	}

	public virtual void OnStartBuild(bool assisting = false)
	{
		if (CreateUnit(buildQueue.First.Value))
		{
			eCost = -(currentUnit.energy * buildPower / currentUnit.buildtime);
			mCost = -(currentUnit.mass * buildPower / currentUnit.buildtime);
			StartCoroutine(BuildTick());
		}
		else
		{
			buildQueue.RemoveFirst();
			currentUnit = null;
		}
	}

	public virtual IEnumerator BuildTick()
	{
		while (currentUnit)
		{
			//print(currentUnit.buildProgress);
			if (resourceManager.CheckAmount(eCost, mCost) && !paused)
			{
				currentUnit.buildProgress += buildPower;
			}
			if (currentUnit.buildProgress >= currentUnit.buildtime)
			{
				OnUnitBuilt(currentUnit);
				break;
			}
			yield return new WaitForSeconds(0.1f);
		}

		yield return new WaitForSeconds(0.1f);
	}

	public virtual void OnUnitBuilt(Unit unitbuilt)
	{
		if (repeat)
		{
			buildQueue.AddFirst(buildQueue.First.Value);
			
		}
		buildQueue.RemoveFirst();
		currentUnit = null;
	}

	public virtual bool CreateUnit(Unit unitToSpawn)
	{
		if (!CanPlaceUnit(unitToSpawn))
		{
			return false;
		}
		currentUnit = Instantiate(unitToSpawn, buildPoint.transform.position, new Quaternion()) as Unit;
		currentUnit.player = player;
		currentUnit.buildProgress = 0;
		currentUnit.OnCreate();
		return true;
	}

	public virtual void StopBuild()
	{
		buildQueue.Clear();
		currentUnit = null;
	}

	public virtual bool CanPlaceUnit(Unit thingToBuild)
	{
		Collider[] hitColliders = Physics.OverlapBox(transform.position, thingToBuild.transform.localScale / 2, Quaternion.identity, LayerMask.GetMask("Units"));
		foreach (Collider collider in hitColliders)
		{
			if (collider.gameObject.GetComponent<StructureUnit>())
			{
				return false;
			}
		}
		return true;
	}

	public void Start()
	{
		StartCoroutine(UpdateBuilderLoop());
		OnCreate();
	}



}
