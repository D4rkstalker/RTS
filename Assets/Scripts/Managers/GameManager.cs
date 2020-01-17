using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public List<GameObject> playerControllers;
	public int activePlayer;
	public List<Unit> startingUnits;

    void Start()
    {
		foreach (Transform child in gameObject.transform)
		{
			playerControllers.Add(child.gameObject);
			child.gameObject.GetComponent<InputController>().enabled = false;
			child.gameObject.GetComponent<UIUpdates>().enabled = false;
		}
		UpdatePlayer();
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
}
