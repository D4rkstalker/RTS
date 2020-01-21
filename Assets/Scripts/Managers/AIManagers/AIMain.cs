using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMain: MonoBehaviour
{
	public bool active = false;
	public AIStates currentState = AIStates.Initial;
	public int player;
	public List<Unit> units;
	public EconomyManager economyManager;
	public UnitsManager unitsManager;
	// Start is called before the first frame update
	void Start()
    {
		InitAIBrain();
		StartCoroutine(AIUpdateLoop());
    }

	public virtual void InitAIBrain()
	{
		player = gameObject.transform.parent.GetComponent<Player>().playerID;
		units = AIUtilities.GetAllUnitsOnMap(player);
	}

	public virtual void AIUpdate()
	{
	}

	IEnumerator AIUpdateLoop()
	{
		while (true)
		{
			AIUpdate();
			yield return new WaitForSeconds(GlobalSettings.GameSpeed * 10);
		}
	}

}
