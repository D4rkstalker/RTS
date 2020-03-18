using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.ScriptingUtilities;

public class ResourceManager : MonoBehaviour
{
	public enum ResourceTypes {Mass, Energy, ShipCap }
	public ResourceTypes resourceType;

	public float energy, energyCap, energyTrend;
	public float mass, massCap, massTrend;
	public float currentUnits;
	public float unitCap;
	public float totalMassIn, totalEnergyIn;

	private float oldMass, oldEnergy;

	// Start is called before the first frame update

	void Start()
	{
		ResourceUpdateLoop();
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

	IEnumerator ResourceUpdate()
	{
		while (true)
		{
			energyTrend = energy - oldEnergy;
			massTrend = mass - oldMass;
			oldEnergy = energy;
			oldMass = mass;
			yield return new WaitForSeconds(GlobalSettings.GameSpeed * 10);
		}
	}

	public void ResourceUpdateLoop()
	{
		StartCoroutine(ResourceUpdate());
	}
}
