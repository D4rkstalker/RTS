using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
	public bool selected;
	public string unitName;
	private NavMeshAgent agent;

	public float health;

    // Start is called before the first frame update
    void Start()
    {
		agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetMouseButtonDown(1) && selected)
		{
			RightClick();
		}
    }

	public void RightClick()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 100))
		{
			if (hit.collider.tag == "Ground")
			{
				agent.destination = hit.point;
				Debug.Log("Moving");
			}
		}
	}
}
