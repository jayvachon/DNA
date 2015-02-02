using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameActions {

	public class ActionList {

		List<Action> actions = new List<Action> ();
		public List<Action> Actions {
			get { return actions; }
		}

		public void Add<T> () where T : Action {
			actions.Add (typeof (T));
		}

		public bool Has (Action action) {
			return actions.Find (action.GetType ());
		}
	}
}