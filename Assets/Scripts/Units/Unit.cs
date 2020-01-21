using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
	public bool targetable = true;
	public string unitName;
	public float shield, maxShield, armor, maxArmor, hull, maxHull, crew, maxCrew, turnRate, mass, energy, buildtime;
	public int player;
	public GameObject selectionIndicator, iconCam;
	public Turret mainGun;
	public List<Categories> categories;
	public Texture2D icon;
	
	[System.NonSerialized]
	public LinkedList<Marker> markers = new LinkedList<Marker>();
	[System.NonSerialized]
	public bool isBuilt, selected, selectable, isResourceCreator;
	[System.NonSerialized]
	public Unit target;
	[System.NonSerialized]
	public Unit assistTarget;
	//[System.NonSerialized]
	public Marker currentMarker;
	//[System.NonSerialized]
	public Tasks task;
	[System.NonSerialized]
	public BuilderTypes builderType = BuilderTypes.none;
	[System.NonSerialized]
	public ResourceCreator rc;
	[System.NonSerialized]
	public BuilderUnit builder;
	//[System.NonSerialized]
	public float buildProgress = 0f;

	void Awake()
	{
		OnCreate();
	}

	public IEnumerator CompletionCheck()
	{
		do
		{
			if (buildProgress >= buildtime)
			{
				OnBuilt();
			}
			yield return new WaitForSeconds(GlobalSettings.GameSpeed);
		} 
		while (!isBuilt);
		yield return null;
	}

	public virtual void OnCreate()
	{
		selectable = false;
		selectionIndicator.SetActive(false);
		//Setup builder units
		if (gameObject.GetComponent<FactoryUnit>())
		{
			builder = gameObject.GetComponent<FactoryUnit>();
			builderType = BuilderTypes.factory;
		}
		else if (gameObject.GetComponent<ConstructionUnit>())
		{
			builder = gameObject.GetComponent<ConstructionUnit>();
			builderType = BuilderTypes.engineer;
		}

		//Setup resource producers
		if (gameObject.GetComponent<ResourceCreator>())
		{
			rc = gameObject.GetComponent<ResourceCreator>();
			isResourceCreator = true;
		}

		GenIcon();
		ToggleActive(false);
		StartCoroutine(CompletionCheck());
	}

	public virtual void OnBuilt()
	{
		isBuilt = true;
		selectable = true;
		ToggleActive(true);
		StartCoroutine(UpdateUnitLoop());
	}

	public virtual void UpdateUnit()
	{
		if (hull <= 0)
		{
			OnKill();
		}

		if (target != null && (task == Tasks.Attacking || task == Tasks.Idle))
		{
			AttackUpdate();
		}
		else if (assistTarget != null && task == Tasks.Assisting)
		{
			AssistUpdate();
		}
	}


	public virtual void OnDamage(float[] damages)
	{
		shield -= damages[0];
		armor -= damages[1];
		hull -= damages[2];
		crew -= damages[3];
		shield = Mathf.Clamp(shield, 0.0f, maxShield);
		armor = Mathf.Clamp(armor, 0.0f, maxArmor);
		hull = Mathf.Clamp(hull, 0.0f, maxHull);
		crew = Mathf.Clamp(crew, 0.0f, maxCrew);
	}

	public virtual void UpdateMarker(Marker marker)
	{
		if (marker)
		{
			
			marker.numUnits--;
			markers.Remove(marker);
		}
		if (marker == currentMarker || !marker)
		{
			if (markers.Count <= 0)
			{
				task = Tasks.Idle;
				currentMarker = null;

			}
			else
			{
				currentMarker = markers.First.Value;
				markers.RemoveFirst();
				CheckMarker(currentMarker);
			}
		}
	}

	public virtual void CheckMarker(Marker marker)
	{
		if (marker is MarkerAttack)
		{
			target = ((MarkerAttack)marker).target;
			task = Tasks.Attacking;
		}
		else if (builderType == BuilderTypes.engineer && marker is MarkerBuild)
		{
			Unit buildThis = marker.GetComponent<MarkerBuild>().unitToBuild;
			buildThis.transform.position = marker.transform.position;
			builder.AddToQueue(buildThis);
			task = Tasks.Building;
		}
		else if(marker is MarkerAssist)
		{
			assistTarget = marker.GetComponent<MarkerAssist>().target;
			task = Tasks.Assisting;
		}

	}

	public virtual void AddMarker(Unit Atarget, Marker marker, bool queue, Tasks task)
	{
		if (!queue)
		{
			ClearOrders();
		}
		markers.AddLast(marker);
		if (!currentMarker)
		{
			UpdateMarker(null);
		}

	}

	public virtual void StopOrder()
	{
		ClearOrders();
		task = Tasks.Idle;
		if (target)
		{
			target = null;
		}
	}

	public virtual void ClearOrders()
	{
		if (currentMarker)
		{
			currentMarker.numUnits--;
			currentMarker = null;
		}
		if (markers.Count > 0)
		{
			foreach (Marker mkr in markers)
			{
				mkr.numUnits--;
			}
			markers.Clear();
		}
		if (builder)
		{
			builder.StopBuild();
		}
		if (gameObject.GetComponent<MobileUnit>())
		{
			gameObject.GetComponent<MobileUnit>().agent.isStopped = false;
		}
		target = null;
	}

	public virtual void OnEnemyKill(MarkerAttack marker)
	{
		target = null;
		if (marker == currentMarker)
		{
			currentMarker = null;
		}
		UpdateMarker(marker);
	}

	public virtual void OnKill()
	{
		ClearOrders();
		StopAllCoroutines();
		Destroy(gameObject);
	}
	public void PointToTarget(Vector3 target)
	{
		if (task != Tasks.Moving)
		{
			Vector3 _direction = (target - transform.position).normalized;

			Quaternion _lookRotation = Quaternion.LookRotation(_direction);

			transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * turnRate);

		}
	}

	public virtual void AttackUpdate()
	{

	}
	public virtual void AssistUpdate()
	{

	}

	public virtual void ToggleActive(bool toggle)
	{
		if (builder)
		{
			builder.enabled = toggle;
		}
		if (rc)
		{
			rc.enabled = toggle;
		}
	}

	public IEnumerator UpdateUnitLoop()
	{
		while (true)
		{
			UpdateUnit();
			yield return new WaitForSeconds(GlobalSettings.GameSpeed);
		}
	}

	public virtual void GenIcon()
	{
		IconManager im;
		GameObject[] results = GameObject.FindGameObjectsWithTag("GameManager");
		foreach (GameObject result in results)
		{
			im = result.GetComponent<IconManager>();
			icon = im.GenerateIcons(categories);
			return;
		}
	}

}
