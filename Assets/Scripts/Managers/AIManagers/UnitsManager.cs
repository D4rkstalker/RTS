using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitsManager : MonoBehaviour
{
    public bool active = false;
    public int player;
    public List<FactoryUnit> factories;
    public List<Unit> constructors;

    public Dictionary<AIUnitRoles, List<Unit>> unitGroups = new Dictionary<AIUnitRoles, List<Unit>>();


    private UnitList unitList;

    public virtual Unit ConstructorCheck(Vector3 conRallyPoint, AIUnitRoles type, Categories faction)
    {
        Unit unit = null;
        if (unitGroups[type].Count < 1)
        {
            constructors = AIUtilities.AssignmentCheck(AIUtilities.GetAllConstructors(false, player));

            if (constructors.Count < 1)
            {
                BuildUnits(1, new List<Categories>() { Categories.Engineer, faction }, conRallyPoint);
                return unit;
            }
            else
            {
                constructors[0].AIRole = type;
                unitGroups[type].Append(constructors[0]);
                print(unitGroups[type].Count);
                return constructors[0];
            }

        }
        else
        {
            foreach (Unit constructor in unitGroups[type])
            {
                if (constructor.task == Tasks.Idle)
                {
                    unit = constructor;
                    return unit;
                }
            }
        }
        return unit;
    }

    public virtual void BuildUnits(int count, List<Categories> categories, Vector3 rallyPoint)
    {
        List<Unit> units = unitList.GetUnitByCategory(categories);
        int unitIndex = 0;
        for (int i = 0; i < count; i += 0)
        {
            foreach (FactoryUnit factory in factories)
            {
                if (i < count)
                {
                    factory.AddToQueue(units[unitIndex]);
                    factory.SetRallyPoint(rallyPoint);
                    i++;
                    if (unitIndex < units.Count)
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
        PopulateUnitGroups();
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
    public virtual void PopulateUnitGroups()
    {
        foreach (AIUnitRoles role in (AIUnitRoles[])System.Enum.GetValues(typeof(AIUnitRoles)))
        {
            unitGroups.Add(role, new List<Unit>());
        }
    }

}

public enum AIUnitRoles
{
    EnergyBuilder,
    MassBuilder,
    Unassigned,
}
