using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionUnit : BuilderUnit
{
	public MobileUnit self;
	public Unit target;

	public override void OnCreate()
	{
		base.OnCreate();
		self.builder = true;
	}

}
