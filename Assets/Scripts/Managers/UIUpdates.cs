using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUpdates : MonoBehaviour
{
	public InputController inControl;
	public SimpleHealthBar healthBar;
	public SimpleHealthBar shieldBar;
	public SimpleHealthBar armorBar;
	public CanvasGroup infopanel;
	public CanvasGroup buildpanel;
	public Text namePanel;

	private Unit unit;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (inControl.selectedUnits.Count > 0)
		{
			if (inControl.selectedUnits[0] != null)
			{

				unit = inControl.selectedUnits[0].GetComponent<Unit>();
				if (unit.hull <= 0)
				{
					infopanel.alpha = 0;
					infopanel.blocksRaycasts = false;
					infopanel.interactable = false;
				}
				healthBar.UpdateBar(unit.hull, unit.maxHull);
				shieldBar.UpdateBar(unit.shield, unit.maxShield);
				armorBar.UpdateBar(unit.armor, unit.maxArmor);
				healthBar.barText.text = unit.hull + "/" + unit.maxHull;
				namePanel.text = unit.unitName;
				infopanel.alpha = 1;
				infopanel.blocksRaycasts = true;
				infopanel.interactable = true;
			}
		}
		else
		{
			infopanel.alpha = 0;
			infopanel.blocksRaycasts = false;
			infopanel.interactable = false;
			unit = null;
		}
	}
}
