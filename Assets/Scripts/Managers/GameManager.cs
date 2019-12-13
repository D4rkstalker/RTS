using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public List<GameObject> playerControllers;
	public int activePlayer;
    void Start()
    {
		foreach (Transform child in gameObject.transform)
		{
			playerControllers.Add(child.gameObject);
		}
	}

	// Update is called once per frame
	void Update()
    {
		foreach(GameObject player in playerControllers)
		{
			if(player.GetComponent<Player>().playerID != activePlayer)
			{
				player.GetComponent<InputController>().playerEnabled = false;
			}
		}
    }
}
