using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
	public GameObject iconCam;
	public Tasks task;
	public bool selected = false;
	public string unitName;
	public float shield = 0;
	public float maxShield = 0;
	public float armor = 0;
	public float maxArmor = 0;
	public float hull = 0;
	public float maxHull = 0;
	public float crew = 0;
	public float maxCrew = 0;
	public float turnRate = 0;
	public float mass = 0;
	public float energy = 0;
	public float buildtime = 0;
	public int player;
	public GameObject selectionIndicator;
	public bool targetable = true;
	public Unit target;
	public Turret mainGun;
	public List<string> categories;
	public Texture icon;

	public Queue<Marker> markers = new Queue<Marker>();

	public Marker currentMarker;

	void Start()
	{
		OnCreate();

	}

	void Update()
	{
		UpdateUnit();
	}

	public virtual void OnCreate()
	{
		selectionIndicator.SetActive(false);
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

	public virtual void UpdateUnit()
	{
		if (hull <= 0)
		{
			OnKill();
		}
		iconCam.SetActive(selected);
	}
	public virtual void UpdateMarker(Marker marker)
	{
		if (marker)
		{
			if (marker == currentMarker)
			{
				currentMarker.numUnits--;
			}
		}
		if (markers.Count <= 0)
		{
			task = Tasks.Idle;
			currentMarker = null;

		}
		else
		{
			currentMarker = markers.Dequeue();
			CheckMarker(currentMarker);
		}
	}

	public virtual void CheckMarker(Marker marker)
	{
		if (marker is MarkerAttack)
		{
			target = ((MarkerAttack)marker).target;
			task = Tasks.Attacking;
		}
	}


	public virtual void AddMarker(Unit Atarget, Marker marker, bool queue, Tasks task)
	{
		if(!queue){
			ClearOrders();
		}
		markers.Enqueue(marker);
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

	}

	public virtual void OnEnemyKill(MarkerAttack marker)
	{
		target = null;
		if(marker == currentMarker)
		{
			currentMarker = null;
		}
		UpdateMarker(marker);
	}

	public virtual void OnKill()
	{
		ClearOrders();
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

}
