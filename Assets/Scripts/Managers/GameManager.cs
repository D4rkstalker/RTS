using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public List<GameObject> playerControllers;
	public int activePlayer;
	public List<Unit> startingUnits;
	public float[] xBound = new float[] { -200, 200 };
	public float[] zBound = new float[] { -200, 200 };
	public CameraControl cameraController;
	public int borderThickness = 50;
	public int borderExtensionMult = 2;


	void Start()
    {
		foreach (Transform child in gameObject.transform)
		{
			playerControllers.Add(child.gameObject);
			child.gameObject.GetComponent<InputController>().enabled = false;
			child.gameObject.GetComponent<UIUpdates>().enabled = false;
		}
		UpdatePlayer();
		GenMapBorder();
	}
	void UpdatePlayer()
    {
		foreach(GameObject player in playerControllers)
		{
			if(player.GetComponent<Player>().playerID == activePlayer)
			{
				player.GetComponent<InputController>().enabled = true;
				player.GetComponent<UIUpdates>().enabled = true;
			}
			else
			{
				player.GetComponent<InputController>().enabled = false;
				player.GetComponent<UIUpdates>().enabled = false;
			}
		}
    }

	void GenMapBorder()
	{
		cameraController.xBound = xBound;
		cameraController.zBound = zBound;

		// Draw Map borders 

		// 500,0 top
		GameObject topBar = GameObject.CreatePrimitive(PrimitiveType.Cube);
		topBar.transform.position = new Vector3(0, 0, xBound[1] + borderThickness / 2);
		topBar.transform.localScale = new Vector3(xBound[1] * borderExtensionMult, 10, borderThickness);

		//-500,0 bottom
		GameObject bottomBar = GameObject.CreatePrimitive(PrimitiveType.Cube);
		bottomBar.transform.position = new Vector3(0, 0, xBound[0] - borderThickness / 2);
		bottomBar.transform.localScale = new Vector3(xBound[1] * borderExtensionMult, 10, borderThickness);

		//0,500 right
		GameObject rightBar = GameObject.CreatePrimitive(PrimitiveType.Cube);
		rightBar.transform.position = new Vector3(zBound[1] + borderThickness / 2, 0, 0 );
		rightBar.transform.localScale = new Vector3(borderThickness, 10, xBound[1] * borderExtensionMult);
		//0, -500 left
		GameObject leftBar = GameObject.CreatePrimitive(PrimitiveType.Cube);
		leftBar.transform.position = new Vector3(zBound[0] - borderThickness / 2, 0, 0 );
		leftBar.transform.localScale = new Vector3(borderThickness, 10, xBound[1] * borderExtensionMult);
	}
}
