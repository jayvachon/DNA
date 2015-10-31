using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.EventSystem;

public class StaticUnitsManager {

	static List<string> units = null;
	static List<string> Units {
		get {
			if (units == null) {
				units = new List<string> () {
					"GivingTree",
					"CoffeePlant",
					"University",
					"Jacuzzi",
					"Clinic"
				};
			}
			return units;
		}
	}

	static List<string> unlockedUnits = null;
	public static List<string> UnlockedUnits {
		get {
			if (unlockedUnits == null) {
				unlockedUnits = new List<string> () {
					"University",
					"Clinic",
					"Jacuzzi",
					"CoffeePlant"
				};
			}
			return unlockedUnits;
		}
	}

	public static void UnlockUnit (string id) {
		if (Units.Contains (id) && !UnlockedUnits.Contains (id)) {
			UnlockedUnits.Add (id);
			Events.instance.Raise (new UnlockUnitEvent (id));
		}
	}
}
