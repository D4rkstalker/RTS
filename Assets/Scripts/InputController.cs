using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
	public float panSpeed;
	public GameObject selectedObject;
	public Unit selectedUnit;

    private Vector2 startPos;
    private Vector2 endPos;
    public GameObject selectionBox;
    public Texture box;

    private Rect selectBox;
    private GameObject[] units;
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
        if (Input.GetMouseButton(0) && startPos == Vector2.zero)
        {
            startPos = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0) && startPos != Vector2.zero)
        {
            endPos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            units = GameObject.FindGameObjectsWithTag("Selectable");
            MultiSelect();
            startPos = Vector2.zero;
            endPos = Vector2.zero;
        }
        selectBox = new Rect(startPos.x, Screen.height - startPos.y, endPos.x - startPos.x, -1 * ((Screen.height - startPos.y) - (Screen.height - endPos.y)));
    }

    public void MultiSelect()
    {
        foreach(GameObject unit in units)
        {

        }
    }

	public void LeftClick()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 100))
		{
			if (hit.collider.tag == "Ground")
			{
                Deselect();
            }
			else if(hit.collider.tag == "Selectable"){
                Deselect();
                Select(hit.collider.gameObject);
			}
		}
	}

    public void Deselect()
    {
        if (selectedUnit != null)
        {
            selectedUnit.selected = false;
            selectedObject = null;
        }
    }

    public void Select(GameObject unit)
    {
        selectedObject = unit;
        selectedUnit = selectedObject.GetComponent<Unit>();

        selectedUnit.selected = true;
    }

    private void OnGUI()
    {
        if(startPos != Vector2.zero && endPos != Vector2.zero)
        {
            GUI.DrawTexture(selectBox,box);
        }
    }
}
