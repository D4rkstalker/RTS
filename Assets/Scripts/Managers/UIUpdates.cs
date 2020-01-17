using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUpdates : MonoBehaviour
{
	public SimpleHealthBar healthBar, shieldBar, armorBar;
	public SimpleHealthBar massBar, energyBar;
	public CanvasGroup infopanel;
	public CanvasGroup buildpanel;
	public Text namePanel;
	public Text currentE, trendE, maxE, currentM, trendM, maxM;
	public InputController inControl;


	private ResourceManager rm;
	private Unit unit;

	// Start is called before the first frame update
	void Start()
	{
		rm = gameObject.GetComponent<ResourceManager>();
	}

	// Update is called once per frame
	void LateUpdate()
	{
		if (gameObject.GetComponent<Player>().playerID == transform.parent.GetComponent<GameManager>().activePlayer)
		{
			UIUpdate();
		}
	}

	public void UIUpdate()
	{
		UpdateUnitInfoCard();
		UpdateResourceDisplay();


	}

	public void UpdateResourceDisplay()
	{
		currentE.text = "" + (int)rm.energy;
		currentM.text = "" + (int)rm.mass;
		maxE.text = "" + (int)rm.energyCap;
		maxM.text = "" + (int)rm.massCap;
		trendE.text = "" + (int)rm.energyTrend;
		trendM.text = "" + (int)rm.massTrend;
		massBar.UpdateBar(rm.mass, rm.massCap);
		energyBar.UpdateBar(rm.energy, rm.energyCap);
	}

	public void UpdateUnitInfoCard()
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
