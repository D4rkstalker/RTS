using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StrategicIcon : MonoBehaviour
{
	Camera cam;
	private Unit unit;
	void Start()
	{
		cam = Camera.main;
		unit = transform.parent.parent.parent.gameObject.GetComponent<Unit>();
		gameObject.GetComponent<RawImage>().texture = unit.icon;
	}
	void Update()
	{
		transform.position = cam.WorldToScreenPoint(unit.transform.position);
	}
}
