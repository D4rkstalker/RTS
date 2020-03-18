using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputController : MonoBehaviour
{
	public float panSpeed;
	public List<Unit> selectedUnits;
	private Vector2 startPos;
	private Vector2 endPos;
	private Camera cam;

	public GameObject selectionBox;
	public Texture box;
	public MarkerMove moveMarker;
	public MarkerAttack attackMarker;
	public MarkerAssist assistMarker;
	public MarkerBuild buildMarker;
	public Marker currentMarker;
	public CurrentMode currentMode;

	private Rect selectionBoxUI;
	private BuildController buildController;
	// Start is called before the first frame update
	void Start()
	{
		cam = Camera.main;
		buildController = gameObject.GetComponent<BuildController>();
	}

	// Update is called once per frame
	void Update()
	{
		selectedUnits.RemoveAll(item => item == null);
		CheckInputs();
		CheckCurrentMarker();

	}

	public void CheckCurrentMarker()
	{
		if (currentMarker)
		{
			Vector3 mouse = Input.mousePosition;
			Ray castPoint = Camera.main.ScreenPointToRay(mouse);
			RaycastHit hit;
			if (Physics.Raycast(castPoint, out hit, 100, LayerMask.GetMask("movement")))
			{
				currentMarker.transform.position = hit.point;
			}
		}

	}

	public void CheckInputs()
	{
		if (!EventSystem.current.IsPointerOverGameObject())
		{
			if (Input.GetButton("Right"))
			{
				RightClick();
			}

			if (Input.GetButton("Left"))
			{
				LeftClick();

			};
			if (Input.GetButton("Left") && startPos == Vector2.zero)
			{
				startPos = Input.mousePosition;
			}
			else if (Input.GetButton("Left") && startPos != Vector2.zero)
			{
				endPos = Input.mousePosition;
			}

			if (Input.GetButtonUp("Left"))
			{
				if (currentMode != CurrentMode.building)
				{
					MultiSelect(startPos, endPos);
				}
				startPos = Vector2.zero;
				endPos = Vector2.zero;
			}
			selectionBoxUI = new Rect(startPos.x, Screen.height - startPos.y, endPos.x - startPos.x, -1 * ((Screen.height - startPos.y) - (Screen.height - endPos.y)));
		}
		if (Input.GetButton("Stop"))
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

		if (!Input.GetButton("Shift"))
		{
			Deselect();
		}
		List<BuilderUnit> constructors = new List<BuilderUnit>();
		foreach (Collider selected in selections)
		{
			Unit selectedUnit = selected.gameObject.GetComponent<Unit>();
			if (selectedUnit)
			{
				if (selectedUnit.selectable)
				{
					if (SelectUnit(selectedUnit))
					{
						constructors.Add(selectedUnit.GetComponent<BuilderUnit>());
					}
				}

			}
		}
		if (constructors.Count > 0)
		{
			buildController.PopulateBuildableList(constructors);
		}
	}

	public void RightClick()
	{
		if (currentMode == CurrentMode.building)
		{
			if (currentMarker)
			{
				Destroy(currentMarker.gameObject);
			}
			CancelBuild();
		}
		else if (selectedUnits.Count > 0)
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
							unit.GetComponent<MobileUnit>().AddMarker(null, marker, Input.GetButton("Shift"), Tasks.Moving);
						}
						else if (unit.builderType == BuilderTypes.factory)
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
							unit.GetComponent<MobileUnit>().AddMarker(target, marker, Input.GetButton("Shift"), Tasks.Attacking);
						}
					}
					else
					{
						MarkerAssist marker = Instantiate(assistMarker, hit.point, transform.rotation) as MarkerAssist;
						marker.numUnits = selectedUnits.Count;
						marker.target = target;
						foreach (Unit unit in selectedUnits)
						{
							unit.GetComponent<MobileUnit>().AddMarker(target, marker, Input.GetButton("Shift"), Tasks.Assisting);
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
			if (!(currentMarker is MarkerBuild))
			{

				if (hit.collider.tag == "Ground" && !Input.GetButton("Shift"))
				{
					Deselect();
				}
				else if (hit.collider.tag == "Units")
				{
					SelectUnit(hit.collider.gameObject.GetComponent<Unit>());
				}
			}
			else if (currentMarker)
			{
				currentMarker.GetComponent<MarkerBuild>().PlaceMarker();
				
				if (Input.GetButton("Shift"))
				{
					MarkerBuild oldMarker = currentMarker.GetComponent<MarkerBuild>();
					MarkerBuild newMarker = Instantiate(buildMarker) as MarkerBuild;
					newMarker.unitToBuild = oldMarker.unitToBuild;
					newMarker.builders = oldMarker.builders;
					newMarker.numUnits = oldMarker.numUnits;
					currentMarker = newMarker;
				}
				else
				{
					CancelBuild();
				}

			}

		}
	}

	public void Deselect()
	{
		currentMode = CurrentMode.idle;
		if (selectedUnits.Count > 0)
		{
			foreach (Unit unit in selectedUnits)
			{
				if (unit != null)
				{
					unit.selected = false;
					unit.selectionIndicator.SetActive(false);
					unit.iconCam.SetActive(false);
				}
			}
			selectedUnits.Clear();
		}
		buildController.ClearBuildIcons();
	}

	public bool SelectUnit(Unit selectedUnit)
	{
		selectedUnit.selected = true;
		selectedUnit.selectionIndicator.SetActive(true);
		selectedUnit.iconCam.SetActive(true);
		selectedUnits.Add(selectedUnit);
		currentMode = CurrentMode.selectedUnit;
		if (selectedUnit.builderType != BuilderTypes.none)
		{
			return true;
		}
		return false;

	}

	private void OnGUI()
	{
		if (startPos != Vector2.zero && endPos != Vector2.zero)
		{
			GUI.DrawTexture(selectionBoxUI, box);
		}
	}

	private void CancelBuild()
	{
		Destroy(currentMarker.gameObject);
		currentMarker = null;
		currentMode = CurrentMode.idle;

	}
}

public enum CurrentMode
{
	idle,
	selectedUnit,
	building,
}
