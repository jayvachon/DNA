using UnityEngine;
using System.Collections;

public class StaticUnitsHolder : MonoBehaviour {

	public UnitsCategory[] categories;
	static public StaticUnitsHolder instance;

	void Awake () {
		if (instance == null) instance = this;
	}

	public StaticUnit GetUnit (string categoryName, string unitName) {
		return GetCategory (categoryName).GetUnit (unitName);
	}

	public string[] GetNames (string categoryName) {
		return GetCategory (categoryName).UnitNames;
	}

	public UnitsCategory GetCategory (string categoryName) {
		for (int i = 0; i < categories.Length; i ++) {
			if (categories[i].name == categoryName)
				return categories[i];
		}
		return null;
	}
}