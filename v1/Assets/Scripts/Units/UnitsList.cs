using UnityEngine;
using System.Collections;

public class UnitsList : MonoBehaviour {

	public Transform[] units;
	
	public static UnitsList instance;

	void Awake () {
		if (instance == null)
			instance = this;
	}

	public Transform GetUnit (string name) {
		for (int i = 0; i < units.Length; i ++) {
			if (units[i].name == name)
				return units[i];
		}
		return null;
	}
}
