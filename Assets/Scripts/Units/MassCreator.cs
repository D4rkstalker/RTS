using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MassCreator : Unit
{ 
	public float massGenRate;
	public float energyUsage;
	public ResourceManager RM;
	public override void UpdateUnit()
	{
		base.UpdateUnit();
		RM.UpdateMass(massGenRate * Time.deltaTime);
		RM.UpdateEnergy(energyUsage * Time.deltaTime);
	}

}
