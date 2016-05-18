using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DNA.Units {

	// ObjectPool wrapper for quick lookup of units
	public static class UnitManager {

		static Dictionary<Type, List<Unit>> units = new Dictionary<Type, List<Unit>> ();

		public static T Instantiate<T> (Vector3 position=new Vector3 ()) where T : Unit {
			T obj = ObjectPool.Instantiate<T> (position);
			RegisterUnit<T> (obj);
			return obj;
		}

		public static void Destroy (Unit unit) {
			UnregisterUnit (unit);
			ObjectPool.Destroy (unit.transform);
		}

		static void RegisterUnit<T> (T unit) where T : Unit {
			List<Unit> list;
			if (units.TryGetValue (typeof (T), out list)) {
				list.Add (unit);
			} else {
				units.Add (typeof (T), new List<Unit> () { unit });
			}
		}

		static void UnregisterUnit (Unit unit) {
			try {
				units[unit.GetType ()].Remove (unit);
			} catch (KeyNotFoundException e) {
				throw new System.Exception ("The unit " + unit + " can not be unregistered from the UnitManager because it was not instantiated through the UnitManager\n" + e);
			}
		}
	}
}