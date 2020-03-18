using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIStates
{
	Initial,
	Scouting,
	Raiding,
	Expanding,
	Attacking,
	Defending,
}

public enum ResourceState
{
	NeedMass,
	NeedEnergy,
	OK,
	SpendMass,
	SpendEnergy,
}
