using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerBuild : Marker
{
	public Unit unitToBuild;
	public List<BuilderUnit> builders;
	public GameObject no;

	private bool canBePlaced = true;
	private bool placed = false;


	public override void OnCreateMarker()
	{
		base.OnCreateMarker();
		gameObject.GetComponent<BoxCollider>().size = unitToBuild.GetComponent<BoxCollider>().size;
	}
	public override void UpdateMarker()
	{
		base.UpdateMarker();
		canBePlaced = Canbuild(placed);
		if (!canBePlaced)
		{
			if (!placed)
			{
				no.SetActive(true);
			}
		}
		else if (!placed)
		{
			no.SetActive(false);
		}
		if (placed && !canBePlaced)
		{
			foreach (BuilderUnit builder in builders)
			{
				builder.GetComponent<MobileUnit>().UpdateMarker(this);
			}
			Destroy(gameObject);

		}
	}

	public void PlaceMarker()
	{
		placed = true;
		foreach (ConstructionUnit constructor in builders)
		{
			constructor.GetComponent<Unit>().AddMarker(unitToBuild, this, Input.GetButton("Shift"), Tasks.Building);
		}

	}

	public bool Canbuild(bool placed)
	{
		bool canPlace = false;
		StructureUnit tempStructure = unitToBuild.GetComponent<StructureUnit>();
		Collider[] hitColliders = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity, LayerMask.GetMask("Units"));
		foreach (Collider collider in hitColliders)
		{
			if (collider.gameObject.GetComponent<StructureUnit>())
			{
				return false;
			}
			else if (tempStructure)
			{
				if (collider.gameObject.GetComponent<MassDeposit>())
				{
					if (!tempStructure.massExtractor)
					{
						return false;
					}
					else
					{
						canPlace = true;
					}
				}				
			}
			
		}
		if (!tempStructure.massExtractor)
		{
			return true;
		}
		return canPlace; 
	}

}
