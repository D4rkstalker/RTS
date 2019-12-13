using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrategicIcon : MonoBehaviour
{
	Camera cam;
	public float size = 0.005f;
	void Awake()
	{
		cam = Camera.main;
	}
	void LateUpdate()
	{
		transform.localScale = new Vector3(1, 1, 1) * size * cam.orthographicSize;
	}
}
