using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameActions {

	public abstract class ActionList {

		Dictionary<string, Action> actions = new Dictionary<string, Action> ();
		public Dictionary<string, Action> Actions {
			get { return actions; }
			protected set { actions = value; }
		}

		public void AddAction (string id, Action action=null) {
			actions.Add (id, action);
		}

		/**
		 *	Debugging
		 */

		public virtual void Print () {
			foreach (var action in actions) {
				Debug.Log (action.Key);
			}
		}
	}
}