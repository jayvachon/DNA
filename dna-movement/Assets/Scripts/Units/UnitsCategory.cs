using UnityEngine;
using System.Collections;

[System.Serializable]
public class UnitsCategory : System.Object {

	public string name;
	public StaticUnit[] units;
	
	string[] unitNames = new string[0];
	public string[] UnitNames {
		get {
			if (unitNames.Length == 0) {
				unitNames = new string[units.Length];
				for (int i = 0; i < unitNames.Length; i ++) {
					unitNames[i] = units[i].name;
				}
			}
			return unitNames; 
		}
	}

	public StaticUnit GetUnit (string unitName) {
		for (int i = 0; i < units.Length; i ++) {
			if (units[i].name == unitName)
				return units[i];
		}
		return null;
	}
}