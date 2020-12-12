using AIStateInstances;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMain: MonoBehaviour
{
	public bool active = false;
	public AIStates currentState = AIStates.Initial;
	public ResourceState resourceState;
	public int playerID;
	public Categories faction;
	public List<Unit> units;
	public UnitList unitList;
	public int massStatus, energyStatus;
	public List<Unit> constructors;
	public int baseRadius;
	public int densityFactor;
	public List<Vector3> placementPositions;
	public MarkerBuild markerBuild;
	public bool allMassClaimed;

	public Player player;
	public BuilderGroupManager builderGroupManager;
	public EconomyManager economyManager;
	public UnitsManager unitsManager;
	public List<ExpansionManager> expansions;
	public ResourceManager resourceManager;

	public List<Categories> building;

	public Unit GetConstructor(Vector3 conRallyPoint,AIUnitRoles UnitRole)
	{
		return unitsManager.ConstructorCheck(conRallyPoint, UnitRole, faction);
		
	}

	public virtual void InitAIBrain()
	{
		builderGroupManager = gameObject.GetComponent<BuilderGroupManager>();
		economyManager = gameObject.GetComponent<EconomyManager>();
		unitsManager = gameObject.GetComponent<UnitsManager>();
		player = gameObject.transform.parent.GetComponent<Player>();
		faction = gameObject.transform.parent.GetComponent<Player>().faction;

		player.isAI = true;
		player.ai = this;
		playerID = player.playerID;
		units = AIUtilities.GetAllUnitsOnMap(playerID);

		GameObject gm = GameObject.FindGameObjectWithTag("GameManager");
		unitList = gm.GetComponent<UnitList>();

		GameObject[] pms = GameObject.FindGameObjectsWithTag("PlayerManager");
		foreach (GameObject pm in pms)
		{
			if (pm.GetComponent<Player>().playerID == playerID)
			{
				resourceManager = pm.GetComponent<ResourceManager>();
			}
		}

		GeneratePlacementPosition();
	}

	public virtual void GeneratePlacementPosition()
	{
		for (int i = 0; i < densityFactor; i++)
		{
			float angle = i * Mathf.PI * 2 / densityFactor;
			Vector3 pos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * baseRadius;
			placementPositions.Add(transform.position + pos);
		}

	}

	public virtual Vector3 PickBuildLocation()
	{
		if (placementPositions.Count < 1)
		{
			densityFactor *= 2;
			baseRadius += 20;
			GeneratePlacementPosition();
		}
		int num = Random.Range(0,placementPositions.Count -1);
		Vector3 placementPos = placementPositions[num];
		placementPositions.RemoveAt(num);
		return placementPos;
	}

	public virtual void AIUpdate()
	{
	}

	IEnumerator AIUpdateLoop()
	{
		while (true)
		{
			AIUpdate();
			yield return new WaitForSeconds(GlobalSettings.GameSpeed * 10);
		}
	}
	void Start()
	{
		InitAIBrain();
		StartCoroutine(AIUpdateLoop());
	}

	public void OnUnitBuiltCallback(Unit unit)
	{
		foreach (Categories category in unit.categories)
		{
			building.Remove(category);
		}
	}

}
