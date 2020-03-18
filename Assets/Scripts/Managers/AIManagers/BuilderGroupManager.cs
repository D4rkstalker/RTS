using AIStateInstances;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderGroupManager : MonoBehaviour
{
	public List<InstanceGroup> instanceGroups;
	public int massInstanceCounts, energyInstanceCounts;

	public void BGMonitering()
	{
		foreach (InstanceGroup ig in instanceGroups)
		{
			if(ig.unitCount < 1)
			{
				instanceGroups.Remove(ig);
			}
		}
	}
	public virtual int CountInstances(InstanceTypes instance)
	{
		int count = 0;
		foreach (InstanceGroup group in instanceGroups)
		{
			if (group.instance == instance)
			{
				count++;
			}
		}
		return count;
	}
	IEnumerator BGMoniteringLoop()
	{
		while (true)
		{
			BGMonitering();
			yield return new WaitForSeconds(GlobalSettings.GameSpeed);
		}
	}

    void Start()
    {
		instanceGroups = new List<InstanceGroup>();
		StartCoroutine(BGMoniteringLoop());
    }
}
