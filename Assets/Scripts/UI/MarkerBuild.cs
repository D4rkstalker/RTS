using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerBuild : Marker
{
	public Unit unitToBuild;
	public List<BuilderUnit> builders;
	private bool placed = false;

	public override void OnCreateMarker()
	{
		base.OnCreateMarker();
		gameObject.GetComponent<BoxCollider>().size = unitToBuild.GetComponent<BoxCollider>().size;
	}
	public override void UpdateMarker()
	{
		base.UpdateMarker();
		if (!placed)
		{
			if (Input.GetButtonDown("Left"))
			{
				if (Input.GetButton("Shift"))
				{
					GameObject newMarker = Instantiate(gameObject) as GameObject;
				}
				placed = true;
				foreach (ConstructionUnit constructor in builders)
				{
					constructor.GetComponent<Unit>().AddMarker(unitToBuild, this, Input.GetButton("Shift"), Tasks.Building);
				}

			}
			else if (Input.GetButton("Right"))
			{
				Destroy(gameObject);
			}

			Vector3 mouse = Input.mousePosition;
			Ray castPoint = Camera.main.ScreenPointToRay(mouse);
			RaycastHit hit;
			if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, LayerMask.GetMask("movement")))
			{
				transform.position = hit.point;
			}
		}
		else
		{
			Canbuild();
		}
	}

	public void Canbuild()
	{
		Collider[] hitColliders = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity, LayerMask.GetMask("Units"));
		foreach (Collider collider in hitColliders)
		{
			if (collider.gameObject.GetComponent<StructureUnit>())
			{
				if (collider.gameObject.GetComponent<StructureUnit>().isBuilt)
				{
					foreach (BuilderUnit builder in builders)
					{
						builder.GetComponent<MobileUnit>().UpdateMarker(this);
					}
					Destroy(gameObject);
					return;
				}
			}
		}
	}

}
