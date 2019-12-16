using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryUnit : BuilderUnit
{
	public GameObject rallypoint;
	public MarkerMove moveMarker;

	public void SetRallyPoint(Vector3 point)
	{
		print("changing rally point");
		rallypoint.transform.position = point;
	}

	public override void CreateUnit(Unit unitToSpawn)
	{
		Unit unit = Instantiate(unitToSpawn, buildPoint.transform.position, transform.rotation) as Unit;
		unit.player = player;
		unit.OnCreate();
		MarkerMove marker = Instantiate(moveMarker, rallypoint.transform.position, transform.rotation) as MarkerMove;
		marker.numUnits = 1;
		unit.GetComponent<MobileUnit>().AddMarker(null, marker, false, Tasks.Moving);
	}
}
