using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputController : MonoBehaviour
{
	public float panSpeed;
	public List<Unit> selectedUnits;
	bool m_Started;
	private Vector2 startPos;
	private Vector2 endPos;
	private Camera cam;

	public GameObject selectionBox;
	public Texture box;
	public MarkerMove moveMarker;
	public MarkerAttack attackMarker;
	public MarkerAssist assistMarker;

	private Rect selectionBoxUI;
	private GameObject[] units;
	private BuildController buildController;
	// Start is called before the first frame update
	void Start()
	{
		cam = Camera.main;
		buildController = gameObject.GetComponent<BuildController>();
		m_Started = true;
	}

	// Update is called once per frame
	void Update()
	{
		selectedUnits.RemoveAll(item => item == null);

		if (!EventSystem.current.IsPointerOverGameObject())
		{
			if (Input.GetMouseButtonDown(1))
			{
				RightClick();
			}

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
				MultiSelect(startPos, endPos);
				startPos = Vector2.zero;
				endPos = Vector2.zero;
			}
			selectionBoxUI = new Rect(startPos.x, Screen.height - startPos.y, endPos.x - startPos.x, -1 * ((Screen.height - startPos.y) - (Screen.height - endPos.y)));
		}
		if (Input.GetKeyDown(KeyCode.S))
		{
			foreach (Unit unit in selectedUnits)
			{
				unit.StopOrder();
			}
		}
	}

	public void MultiSelect(Vector2 startIn, Vector2 endIn)
	{
		Vector3 start = cam.ScreenToWorldPoint(startIn);

		Vector3 end = cam.ScreenToWorldPoint(endIn);


		Vector3 center = (start + end) / 2;
		Vector3 halfExtends = (start - end) / 2;
		center.y = 0;
		halfExtends.y = 10;
		halfExtends.x = Mathf.Abs(halfExtends.x);
		halfExtends.z = Mathf.Abs(halfExtends.z);
		LayerMask mask = LayerMask.GetMask("Units");
		Collider[] selections = Physics.OverlapBox(center, halfExtends, new Quaternion(), mask);

		if (!Input.GetKey(KeyCode.LeftShift))
		{
			Deselect();
		}
		List<FactoryUnit> factories = new List<FactoryUnit>();
		foreach (Collider selected in selections)
		{
			Unit selectedUnit = selected.gameObject.GetComponent<Unit>();
			selectedUnit.selected = true;
			selectedUnit.selectionIndicator.SetActive(true);
			selectedUnits.Add(selectedUnit);
			if (selectedUnit is FactoryUnit)
			{
				factories.Add(selectedUnit.GetComponent<FactoryUnit>());
			}
		}
		if (factories.Count > 0)
		{
			buildController.PopulateBuildableList(factories);
		}
	}

	public void RightClick()
	{
		if (selectedUnits.Count > 0)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 100))
			{
				if (hit.collider.tag == "Ground")
				{
					MarkerMove marker = Instantiate(moveMarker, hit.point, transform.rotation) as MarkerMove;
					marker.numUnits = 0;
					foreach (Unit unit in selectedUnits)
					{
						if (unit is MobileUnit)
						{
							marker.numUnits++;
							unit.GetComponent<MobileUnit>().AddMarker(null, marker, Input.GetKey(KeyCode.LeftShift), Tasks.Moving);
						}
						else if (unit is FactoryUnit)
						{
							unit.GetComponent<FactoryUnit>().SetRallyPoint(marker.transform.position);
						}
					}

				}
				else if (hit.collider.tag == "Selectable")
				{
					Unit target = hit.collider.gameObject.GetComponent<Unit>();
					if (selectedUnits[0].player != target.player)
					{
						MarkerAttack marker = Instantiate(attackMarker, hit.point, transform.rotation) as MarkerAttack;
						marker.target = target;
						marker.numUnits = selectedUnits.Count;
						marker.attackers = selectedUnits;
						foreach (Unit unit in selectedUnits)
						{
							unit.GetComponent<MobileUnit>().AddMarker(target, marker, Input.GetKey(KeyCode.LeftShift), Tasks.Attacking);
						}
					}
					else
					{
						MarkerAssist marker = Instantiate(assistMarker, hit.point, transform.rotation) as MarkerAssist;
						marker.numUnits = selectedUnits.Count;
						foreach (Unit unit in selectedUnits)
						{
							unit.GetComponent<MobileUnit>().AddMarker(target, marker, Input.GetKey(KeyCode.LeftShift), Tasks.Assisting);
						}

					}


				}
			}
		}
	}


	public void LeftClick()
	{

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 100))
		{
			if (hit.collider.tag == "Ground" && !Input.GetKey(KeyCode.LeftShift))
			{
				Deselect();
			}
			else if (hit.collider.tag == "Units")
			{
				Select(hit.collider.gameObject);
			}
		}
	}

	public void Deselect()
	{

		if (selectedUnits.Count > 0)
		{
			foreach (Unit unit in selectedUnits)
			{
				if (unit != null)
				{
					unit.selected = false;
					unit.selectionIndicator.SetActive(false);
				}
			}
			selectedUnits.Clear();
		}
		buildController.ClearBuildIcons();
	}

	public void Select(GameObject selectedObject)
	{
		Unit unit = selectedObject.GetComponent<Unit>();
		unit.selected = true;
		if (Input.GetKey(KeyCode.LeftShift))
		{
			unit.selectionIndicator.SetActive(true);
			selectedUnits.Add(unit);
		}
		else
		{
			Deselect();
			unit.selectionIndicator.SetActive(true);
			selectedUnits.Add(unit);
		}
	}

	private void OnGUI()
	{
		if (startPos != Vector2.zero && endPos != Vector2.zero)
		{
			GUI.DrawTexture(selectionBoxUI, box);
		}
	}
}
