using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ConstructionUnit : BuilderUnit
{
	public Unit self;
	public float buildRange = 1;

	public override void OnStartBuild(bool assisting = false)
	{
		if (assisting)
		{
			eCost = -(currentUnit.energy * buildPower / currentUnit.buildtime);
			mCost = -(currentUnit.mass * buildPower / currentUnit.buildtime);
			StartCoroutine(BuildTick());

		}
		else
		{
			base.OnStartBuild(assisting);
		}
	}

	public override void OnCreate()
	{
		base.OnCreate();
		self = gameObject.GetComponent<Unit>();
		self.builderType = BuilderTypes.engineer;
	}

	public override void UpdateUnit()
	{
		if ((!currentUnit && buildQueue.Count > 0) || (currentUnit && self.task == Tasks.Assisting))
		{
			Vector3 destination;
			if (!(self.task == Tasks.Assisting))
			{
				destination = buildQueue.First.Value.transform.position;
			}
			else
			{
				destination = currentUnit.transform.position;
			}
			if (Vector3.Distance(transform.position, destination) > buildRange && self is MobileUnit)
			{
				self.GetComponent<NavMeshAgent>().destination = destination;
			}
			if (Vector3.Distance(transform.position, destination) < buildRange)
			{
				if (self is MobileUnit)
				{
					self.GetComponent<NavMeshAgent>().isStopped = true;
				}
				OnStartBuild(self.task == Tasks.Assisting);
			}
		}
	}

	public override bool CreateUnit(Unit unitToSpawn)
	{
		if (self.task == Tasks.Assisting)
		{
			return true;
		}

		buildPoint.transform.position = unitToSpawn.transform.position;
		return base.CreateUnit(unitToSpawn);
	}
	public override void OnUnitBuilt(Unit unitbuilt)
	{
		if(self.task == Tasks.Assisting)
		{
			currentUnit = null;
		}
		else
		{
			base.OnUnitBuilt(unitbuilt);
		}
		self.GetComponent<NavMeshAgent>().isStopped = false;
		self.GetComponent<NavMeshAgent>().ResetPath();
	}

}
