using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartScript : MonoBehaviour
{

	public void SpawnStartingUnits()
	{
		int player = gameObject.GetComponent<Player>().playerID;
		List<Unit> startingUnits = gameObject.transform.parent.GetComponent<GameManager>().startingUnits;
		foreach(Unit unit in startingUnits)
		{
			Unit current = Instantiate(unit,transform) as Unit;
			current.playerID = player;
			current.buildProgress = current.buildtime;
		}
	}

	void Start()
	{
		SpawnStartingUnits();
	}
}
