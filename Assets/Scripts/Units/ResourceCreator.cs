using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;


public class ResourceCreator : MonoBehaviour
{ 
	public float massGenRate;
	public float energyGenRate;
	public ResourceManager RM;
	public int player;

	public virtual void OnCreate()
	{
		player = gameObject.GetComponent<Unit>().player;
		GameObject[] results = GameObject.FindGameObjectsWithTag("PlayerManager");
		foreach (GameObject result in results)
		{
			if (result.GetComponent<Player>().playerID == player)
			{
				RM = result.GetComponent<ResourceManager>();
			}
		}

	}

	public virtual void UpdateUnit()
	{
		RM.UpdateMass(massGenRate);
		RM.UpdateEnergy(energyGenRate);
	}

	IEnumerator updateResourceCreatorLoop()
	{
		while (true)
		{
			UpdateUnit();
			yield return new WaitForSeconds(GlobalSettings.GameSpeed * 10);
		}
	}

	void Start()
	{
		OnCreate();
		StartCoroutine(updateResourceCreatorLoop());
	}

}
