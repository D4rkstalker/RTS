using AIStateInstances;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
	public bool active = false;
	public List<MassDeposit> massPoints;
	public int massScanRadius;
	public int badMassThreshold;
	private float sleepTime;
	private AIMain aiMain;
	private BuilderGroupManager bgm;

	public virtual void InitEcoAIBrain()
	{
		aiMain = gameObject.GetComponent<AIMain>();
		bgm = aiMain.bgm;
		massPoints = AIUtilities.GetMassDepositsAroundPoint(transform.position, massScanRadius,true);
	}

	public virtual void EcoAIUpdate()
	{
		
		aiMain.energyStatus = EnergyBalanceCheck();
		aiMain.massStatus = MassBalanceCheck();
		BuildResourceCheck();
	}

	public void BuildResourceCheck()
	{
		if (aiMain.energyStatus < 0)
		{
			BuildEnergy();
		}
		else if (aiMain.massStatus < 0)
		{
			BuildMass();
		}
	}

	public void BuildEnergy()
	{
		if(bgm.CountInstances(InstanceTypes.EnergyBuilder) < bgm.energyInstanceCounts)
		{

			print("Building energy");
			Vector3 placementPos = aiMain.PickBuildLocation();
			if (aiMain.ConstructorCheck(placementPos))
			{
				print("Constructor acquired for " + aiMain.faction);
				AIUtilities.BuildStructure(placementPos, new List<Categories>() { Categories.EnergyCreator, aiMain.faction, Categories.Building }, aiMain.constructors[0], aiMain, new InstanceGroup(InstanceTypes.EnergyBuilder,1));
			}

		}
	}

	public void BuildMass()
	{
		if (bgm.CountInstances(InstanceTypes.MassBuilder) < bgm.massInstanceCounts)
		{
			massPoints.Clear();
			massPoints = AIUtilities.GetMassDepositsAroundPoint(transform.position, massScanRadius, true);
			if (massPoints.Count > 0 && aiMain.ConstructorCheck(massPoints[0].transform.position))
			{
				InstanceGroup ig = new InstanceGroup( InstanceTypes.MassBuilder,1);
				foreach (MassDeposit massPoint in massPoints)
				{
					AIUtilities.BuildStructure(massPoint.transform.position, new List<Categories>() { Categories.MassCreator, aiMain.faction, Categories.Building },aiMain.constructors[0], aiMain,ig);
				}
				bgm.instanceGroups.Add(ig);
			}
			else
			{
				massScanRadius += 5;
			}
		}

	}

	//-2 Emergency
	//-1 Bad 
	//0 OK 
	//1 Good 
	//2 Too much
	public int EnergyBalanceCheck()
	{
		float eTrend = aiMain.resourceManager.energyTrend;
		float mTrend = aiMain.resourceManager.massTrend;
		if (eTrend < 1)
		{
			return -2;
		}
		else if (eTrend < mTrend)
		{
			return -1;
		}
		else if (eTrend <= 2 * mTrend)
		{
			return 0;
		}
		else if (eTrend < 10 * mTrend)
		{
			return 1;
		}
		else
		{
			return 2;
		}
	}

	//-2 Emergency
	//-1 Bad 
	//0 OK 
	//1 Good 
	//2 Too much 

	public int MassBalanceCheck()
	{
		float eTrend = aiMain.resourceManager.energyTrend;
		float mTrend = aiMain.resourceManager.massTrend;
		if (mTrend < badMassThreshold)
		{
			return -2;
		}
		else if (mTrend < 1)
		{
			return -1;
		}
		else if (mTrend < eTrend)
		{
			return 0;
		}
		else if (mTrend < 2 * eTrend)
		{
			return 1;
		}
		else
		{
			return 2;
		}
	}
	void Start()
	{
		InitEcoAIBrain();
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
