using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Scripts.ScriptingUtilities;

public class IconManager : MonoBehaviour
{
	//Represents unit type
	public Texture2D iconCorvette;
	public Texture2D iconFrigate;
	public Texture2D iconDestroyer;
	public Texture2D iconCruiser; // battlecruiser
	public Texture2D iconCapital; //battleship,carrier
	public Texture2D iconSupercapital; // dreadnaught, titan
	public Texture2D iconFlagship;
	public Texture2D iconFactory;
	public Texture2D iconStructure;

	//Represents unit role
	public Texture2D roleAssault;
	public Texture2D roleArmored;
	public Texture2D roleArtillery;
	public Texture2D roleSupport;
	public Texture2D roleIntel;
	public Texture2D roleCounterIntel;
	public Texture2D roleCarrier;
	public Texture2D rolePowergen;
	public Texture2D roleMassgen;
	public Texture2D roleEngineer;

	private Texture2D icon;
	private Texture2D role;


	public Texture2D GenerateIcons(List<Categories> categories)
	{
		foreach (Categories category in categories)
		{
			switch (category)
			{
				//Base types;
				case Categories.Corvette:
					icon = iconCorvette;
					break;
				case Categories.Frigate:
					icon = iconFrigate;
					break;
				case Categories.Destroyer:
					icon = iconDestroyer;
					break;
				case Categories.Cruiser:
					icon = iconCruiser;
					break;
				case Categories.Capital:
					icon = iconCapital;
					break;
				case Categories.Supercapital:
					icon = iconSupercapital;
					break;
				case Categories.Flagship:
					icon = iconFlagship;
					break;
				case Categories.Factory:
					icon = iconFactory;
					break;
				case Categories.Building:
					icon = iconStructure;
					break;

				//base roles
				case Categories.Assault:
					role = roleAssault;
					break;
				case Categories.Armored:
					role = roleAssault;
					break;
				case Categories.Artillery:
					role = roleArtillery;
					break;
				case Categories.Support:
					role = roleSupport;
					break;
				case Categories.Intel:
					role = roleIntel;
					break;
				case Categories.CounterIntel:
					role = roleCounterIntel;
					break;
				case Categories.rCarrier:
					role = roleCarrier;
					break;
				case Categories.MassCreator:
					role = roleMassgen;
					break;
				case Categories.EnergyCreator:
					role = rolePowergen;
					break;
				case Categories.Engineer:
					role = roleEngineer;
					break;

				default:
					icon = iconStructure;
					role = roleAssault;
					break;
			}
		}
		return icon.AlphaBlend(role);
	}
}
