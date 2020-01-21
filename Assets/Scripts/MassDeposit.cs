using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassDeposit : MonoBehaviour
{
	public bool claimed = false;

	void CheckIfClaimed()
	{
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, 3);
		foreach (Collider collider in hitColliders)
		{
			if (collider.GetComponent<StructureUnit>())
			{
				if (collider.GetComponent<StructureUnit>().massExtractor)
				{
					claimed = true;
				}
				else
				{
					claimed = false;
				}
			}
		}

	}
	void Start()
	{
		StartCoroutine(MassDepositUpdateLoop());
	}

	IEnumerator MassDepositUpdateLoop()
	{
		while (true)
		{
			CheckIfClaimed();
			yield return new WaitForSeconds(GlobalSettings.GameSpeed * 10);
		}
	}
}
