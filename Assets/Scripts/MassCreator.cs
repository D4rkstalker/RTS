using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MassCreator : Unit
{ 
	public float massGenRate;
	public float energyUsage;
	public ResourceManager RM;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		RM.UpdateMass(massGenRate * Time.deltaTime);
		RM.UpdateEnergy(energyUsage * Time.deltaTime);
    }


}
