using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIStateInstances
{
	public class InstanceGroup 
	{
		//public Unit constructors;
		//public List<Unit> thingsToBuild;
		public InstanceTypes instance;
		public int unitCount;
		public InstanceGroup( InstanceTypes massBuilder, int unitCount)
		{
			this.instance = massBuilder;
			this.unitCount = unitCount;
		}

	}
	public enum InstanceTypes
	{
		EnergyBuilder,
		MassBuilder,
	}
}
