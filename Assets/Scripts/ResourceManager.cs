using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
	public enum ResourceTypes {Mass, Energy, ShipCap }
	public ResourceTypes resourceType;

	public float energy;
	public float mass;
	public float currentUnits;
	public float unitCap;

	public Text EText;
	public Text MText;
	public Text unitCapDisp;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		EText.text = ""+(int)energy;
		MText.text = "" + (int)mass;
		//unitCapDisp.text = "/" + currentUnits + "/" + unitCap;
	}

	public void UpdateEnergy(float energyIn)
	{
		energy += energyIn;
	}

	public void UpdateMass(float massIn)
	{
		mass += massIn;

	}

	public void UpdateShips(float ships)
	{
		currentUnits += ships;
	}

	public void UpdateShipCap(float shipCap)
	{
		unitCap += shipCap;
	}
}
