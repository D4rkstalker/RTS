using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitUI : MonoBehaviour
{
	Quaternion rotation;
	public SimpleHealthBar shield;
	public SimpleHealthBar hull;
	public SimpleHealthBar armor;
	public SimpleHealthBar energy;
	public Unit parent;
	void Awake()
	{
		parent = transform.parent.gameObject.GetComponent<Unit>();
		rotation = transform.rotation;
	}
	void Update()
	{
		hull.UpdateBar(parent.hull, parent.maxHull);
		shield.UpdateBar(parent.shield, parent.maxShield);
		armor.UpdateBar(parent.armor, parent.maxArmor);
	}
	void LateUpdate()
	{
		transform.rotation = rotation;
	}
}
