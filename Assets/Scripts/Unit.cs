using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    public GameObject iconCam;
    public Tasks task;
    public bool selected;
    public string unitName;
    public float health;
    public float maxHealth;
    public float shield;
    public float maxshield;
    public float energy;
    public float maxEnergy;

    public void UpdateUnit()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        iconCam.SetActive(selected);
    }

}
