using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryUnit : BuilderUnit
{
	public GameObject rallypoint;
	public MarkerMove moveMarker;
	public override void OnCreate()
	{
		base.OnCreate();
		self.builderType = BuilderTypes.factory;
	}
	public void SetRallyPoint(Vector3 point)
	{
		//print("changing rally point");
		rallypoint.transform.position = point;
	}
	public override void OnUnitBuilt(Unit unitbuilt)
	{
		base.OnUnitBuilt(unitbuilt);
		MarkerMove marker = Instantiate(moveMarker, rallypoint.transform.position, transform.rotation) as MarkerMove;
		marker.numUnits = 1;
		unitbuilt.ToggleActive(true);
		unitbuilt.GetComponent<MobileUnit>().AddMarker(null, marker, false, Tasks.Moving);

	}

	public override void OnStartBuild(bool assisting = false)
	{
		if (CreateUnit(buildQueue.First.Value))
		{
			eCost = -(currentUnit.energy * buildPower / currentUnit.buildtime);
			mCost = -(currentUnit.mass * buildPower / currentUnit.buildtime);
			StartCoroutine(BuildTick());
		}
		else
		{
			currentUnit = null;
			return;
		}
	}


	public override bool CanPlaceUnit(Unit thingToBuild)
	{
		Collider[] hitColliders = Physics.OverlapBox(transform.position, thingToBuild.transform.localScale / 2, Quaternion.identity, LayerMask.GetMask("Units"));
		foreach (Collider collider in hitColliders)
		{
			if (collider.gameObject.GetComponent<MobileUnit>())
			{
				return false;
			}
		}
		return true;
	}

}
