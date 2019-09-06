using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUpdates : MonoBehaviour
{
    public InputController inControl;
    public SimpleHealthBar healthBar;
    public SimpleHealthBar energyBar;
    public SimpleHealthBar shieldBar;
    public CanvasGroup infopanel;
    public Text namePanel;

    private Unit unit;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (inControl.selectedObject != null)
        {


            unit = inControl.selectedObject.GetComponent<Unit>();
            if (unit.health <= 0)
            {
                infopanel.alpha = 0;
                infopanel.blocksRaycasts = false;
                infopanel.interactable = false;
            }
            healthBar.UpdateBar(unit.health, unit.maxHealth);
            energyBar.UpdateBar(unit.energy, unit.maxEnergy);
            healthBar.barText.text = unit.health + "/" + unit.maxHealth;
            energyBar.barText.text = unit.energy + "/" + unit.maxEnergy;
            namePanel.text = unit.unitName;
            infopanel.alpha = 1;
            infopanel.blocksRaycasts = true;
            infopanel.interactable = true;

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
