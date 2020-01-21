using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
	public MarkerBuild markerBuild;
	public bool active = false;
	public List<MassDeposit> massPoints;
	public int massScanRadius;
	public bool goodMass = false;
	public bool goodEnergy = false;
	public List<Unit> constructors;
	private float sleepTime;
	private AIMain aiMain;
	private int player;
	private Categories faction;
	private UnitList unitList;

	public virtual void BuildExtractors(List<MassDeposit> massPoints)
	{
		foreach(MassDeposit massPoint in massPoints)
		{
			MarkerBuild buildMarker = Instantiate(markerBuild) as MarkerBuild;
			buildMarker.unitToBuild = (unitList.GetUnitByCategory(new List<Categories>() { Categories.MassCreator, faction, Categories.Building }))[0];
			buildMarker.unitToBuild.transform.position = massPoint.transform.position;
			buildMarker.builders.Add(constructors[0].GetComponent<BuilderUnit>());
			buildMarker.numUnits = 1;
			constructors[0].AddMarker(buildMarker.unitToBuild, buildMarker, true, Tasks.Building);
		}
	}

	public virtual void InitAIBrain()
	{
		GameObject result = GameObject.FindGameObjectWithTag("GameManager");
		unitList = result.GetComponent<UnitList>();

		aiMain = gameObject.GetComponent<AIMain>();
		player = gameObject.transform.parent.GetComponent<Player>().playerID;
		faction = gameObject.transform.parent.GetComponent<Player>().faction;
		massPoints = AIUtilities.GetMassDepositsAroundPoint(transform.position, massScanRadius,true);
	}

	public virtual void EcoAIUpdate()
	{
		massPoints.Clear();
		massPoints = AIUtilities.GetMassDepositsAroundPoint(transform.position, massScanRadius, true);
		if (massPoints.Count > 0)
		{
			if(constructors.Count < 1)
			{
				constructors = AIUtilities.GetAllConstructors(false,player);
				if(constructors.Count < 1)
				{
					sleepTime = 15;
					aiMain.unitsManager.BuildUnits(1,new List<Categories>() { Categories.Engineer, faction }, massPoints[0].transform.position);
					return;
				}
				else
				{
					constructors = AIUtilities.IdleCheck(constructors);
					if(constructors.Count < 1)
					{
						sleepTime = 15;
						return;
					}
					
				}
			}
			BuildExtractors(massPoints);
		}
		else
		{
			massScanRadius++;
		}
	}
	void Start()
	{
		InitAIBrain();
		StartCoroutine(EcoAIUpdateLoop());
	}


	IEnumerator EcoAIUpdateLoop()
	{
		while (true)
		{
			EcoAIUpdate();
			yield return new WaitForSeconds(GlobalSettings.GameSpeed * 10 + sleepTime);
		}
	}
}
