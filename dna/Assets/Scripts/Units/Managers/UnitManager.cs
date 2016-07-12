using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using DNA.EventSystem;

namespace DNA.Units {

	// ObjectPool wrapper for quick lookup of units
	public static class UnitManager {

		public delegate void OnUpdate ();

		static Dictionary<Type, List<Unit>> units = new Dictionary<Type, List<Unit>> ();
		public static OnUpdate onUpdate;

		/**
		 *	Instantiation
		 */

		public static T Instantiate<T> (Vector3 position=new Vector3 ()) where T : Unit {
			T obj = ObjectPool.Instantiate<T> (position);
			RegisterUnit<T> (obj);
			return obj;
		}

		public static void Destroy<T> (T unit) where T : Unit {
			UnregisterUnit<T> (unit);
			ObjectPool.Destroy<T> (unit.transform);
		}

		public static void Destroy (Unit unit) {
			Destroy<Unit> (unit);
		}

		/**
		 *	Queries
		 */

		public static List<T> GetUnitsOfType<T> () where T : Unit {

			List<Unit> list;
			if (units.TryGetValue (typeof (T), out list))
				return list.ConvertAll (x => (T)x);
			
			return new List<T> ();
		}

		public static List<T> GetAllUnitsOfType<T> () where T : class {
			List<T> list = new List<T> ();
			foreach (var unitList in units) {
				if (typeof (T).IsAssignableFrom (unitList.Key))
					list.AddRange (unitList.Value.ConvertAll (x => x as T));
			}
			return list;
		}

		public static T GetSingleUnit<T> () where T : Unit {
			try {
				return GetUnitsOfType<T> ()[0];
			} catch (IndexOutOfRangeException e) {
				throw new Exception ("No units of type '" + typeof (T) + "' have been registered");
			}
		}

		/**
		 *	Listeners
		 */

		public static void AddListener<T> (Events.EventDelegate<UpdateUnitsEvent<T>> e) where T : Unit {
			Events.instance.AddListener<UpdateUnitsEvent<T>> (e);
		}

		static void SendUpdateMessage<T> () where T : Unit {
			Events.instance.Raise (new UpdateUnitsEvent<T> (GetUnitsOfType<T> ()));
			SendUpdateMessage ();
		}

		static void SendUpdateMessage () {
			if (onUpdate != null)
				onUpdate ();
		}

		/**
		 *	Private methods
		 */

		static void RegisterUnit<T> (T unit) where T : Unit {
			List<Unit> list;
			if (units.TryGetValue (typeof (T), out list)) {
				list.Add (unit);
			} else {
				units.Add (typeof (T), new List<Unit> () { unit });
			}
			// if (typeof (T) == typeof (Shark))
				// Debug.Log ("REFISTER:" + units[typeof(Shark)].Count);
			SendUpdateMessage<T> ();
		}

		static void UnregisterUnit<T> (T unit) where T : Unit {
			try {
				units[unit.GetType ()].Remove (unit);
				SendUpdateMessage ();
				// if (typeof (T) == typeof (Shark))
					// Debug.Log ("UNregister: " + units[typeof (Shark)].Count);
			} catch (KeyNotFoundException e) {
				throw new Exception ("The unit " + unit + " can not be unregistered from the UnitManager because it was not instantiated through the UnitManager\n" + e);
			}
		}
	}

	public class UpdateUnitsEvent<T> : GameEvent where T : Unit {

		public readonly List<T> Units;

		public UpdateUnitsEvent (List<T> units) {
			Units = units;
		}
	}
}