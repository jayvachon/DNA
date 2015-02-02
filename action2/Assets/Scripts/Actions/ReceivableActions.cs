using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameActions {

	public class ReceivableActions {

		// could System.Type be replaced with Action?
		List<System.Type> types = new List<System.Type> ();

		public void Add<T> () {
			types.Add (typeof (T));
		}

		public bool Has (Action action) {
			return types.Find (action.GetType ());
		}
	}
}