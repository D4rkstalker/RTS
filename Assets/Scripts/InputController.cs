using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
	public float panSpeed;
	public GameObject selectedObject;
	private Unit selectedUnit;
    // Start is called before the first frame update
    void Start()
    {
		//selectedUnit = new Unit();

	}

    // Update is called once per frame
    void Update()
    {
		if (Input.GetMouseButtonDown(0))
		{
			LeftClick();
		};
    }

	public void LeftClick()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		Debug.Log("LClick");
		if (Physics.Raycast(ray, out hit, 100))
		{
			if (hit.collider.tag == "Ground")
			{
				selectedUnit.selected = false;
				selectedObject = null;
			}
			else if(hit.collider.tag == "Selectable"){
				selectedObject = hit.collider.gameObject;
				selectedUnit = selectedObject.GetComponent<Unit>();

				selectedUnit.selected = true;
				Debug.Log(selectedUnit.name);
			}
		}
	}

}
