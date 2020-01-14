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
		parent = transform.parent.parent.gameObject.GetComponent<Unit>();
		rotation = new Quaternion();
		rotation.eulerAngles = new Vector3(90, 0, 0);
	}
	void Update()
	{
		if (parent)
		{
			hull.UpdateBar(parent.hull, parent.maxHull);
			shield.UpdateBar(parent.shield, parent.maxShield);
			armor.UpdateBar(parent.armor, parent.maxArmor);
		}
	}
	void LateUpdate()
	{
		transform.rotation = rotation;
	}
}
