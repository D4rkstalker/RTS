using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
	public enum ResourceTypes {Mass, Energy, ShipCap }
	public ResourceTypes resourceType;

	public float energy;
	public float energyCap;
	public float mass;
	public float massCap;
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
		energy = Mathf.Clamp(energy, 0, energyCap);
	}

	public void UpdateMass(float massIn)
	{
		mass += massIn;
		mass = Mathf.Clamp(mass,0, massCap);

	}

	public bool CheckAmount(float eCost, float mCost)
	{
		if(energy + eCost > 0 && mass + mCost > 0)
		{
			UpdateEnergy(eCost);
			UpdateMass(mCost);
			return true;
		}
		else
		{
			return false;
		}
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
