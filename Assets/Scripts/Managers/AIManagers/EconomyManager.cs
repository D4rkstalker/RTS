using AIStateInstances;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public bool active = false;
    public List<MassDeposit> massPoints;
    public int massScanRadius;
    public int badMassThreshold;

    private float sleepTime;
    private AIMain aiMain;

    private Vector3 energyPlacement;
    private Vector3 massPlacement;


    public virtual void InitEcoAIBrain()
    {
        aiMain = gameObject.GetComponent<AIMain>();
        massPoints = AIUtilities.GetMassDepositsAroundPoint(transform.position, massScanRadius, true);
    }

    public virtual void EcoAIUpdate()
    {

        aiMain.energyStatus = EnergyBalanceCheck();
        aiMain.massStatus = MassBalanceCheck();
        BuildResourceCheck();
    }

    public void BuildResourceCheck()
    {
        if (aiMain.energyStatus < 0 && !aiMain.building.Contains(Categories.EnergyCreator))
        {
            BuildEnergy();
        }
        else if (aiMain.massStatus < 0 && !aiMain.building.Contains(Categories.MassCreator))
        {
            BuildMass();
        }
    }

    public void BuildEnergy()
    {
        //print("building energy");
        if (energyPlacement == Vector3.zero)
        {
            energyPlacement = aiMain.PickBuildLocation();
        }
        Unit constructor = aiMain.GetConstructor(energyPlacement, AIUnitRoles.EnergyBuilder);
        if (constructor)
        {
            //print("Constructor acquired for " + aiMain.faction);
            AIUtilities.BuildStructure(energyPlacement, new List<Categories>() { Categories.EnergyCreator, aiMain.faction, Categories.Building }, constructor, aiMain,  Categories.EnergyCreator);
            aiMain.building.Add(Categories.EnergyCreator);
        }
        else
        {
            print("waiting for constructor");
            sleepTime += 10;
        }


    }

    public void BuildMass()
    {
        massPoints.Clear();
        massPoints = AIUtilities.GetMassDepositsAroundPoint(transform.position, massScanRadius, true);
        Unit constructor = aiMain.GetConstructor(energyPlacement, AIUnitRoles.EnergyBuilder);
        if (massPoints.Count > 0)
        {
            if (constructor)
            {
                foreach (MassDeposit massPoint in massPoints)
                {
                    AIUtilities.BuildStructure(massPoint.transform.position, new List<Categories>() { Categories.MassCreator, aiMain.faction, Categories.Building }, constructor, aiMain, Categories.MassCreator);
                }
                aiMain.building.Add(Categories.MassCreator);

            }
            else
            {
                sleepTime += 10;
            }
        }
        else
        {
            massScanRadius += 5;
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
            sleepTime = 0;
            EcoAIUpdate();
            yield return new WaitForSeconds(GlobalSettings.GameSpeed * 10 + sleepTime);
        }
    }
}
