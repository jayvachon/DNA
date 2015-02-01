using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameActions {

	public class AcceptedActions {
		
		List<System.Type> types = new List<System.Type> ();

		public void Add<T> () {
			types.Add (typeof (T));
		}

		public bool Has (Action action) {
			foreach (System.Type type in types) {
				if (action.GetType () == type) {
					return true;
				}
			}
			return false;
		}
	}
}