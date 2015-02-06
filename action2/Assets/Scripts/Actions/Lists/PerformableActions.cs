using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameActions {

	public class PerformableActions : ActionList {

		Dictionary<string, Action> activeActions = new Dictionary<string, Action> ();
		public Dictionary<string, Action> ActiveActions {
			get { return activeActions; }
		}
		
		IActionPerformer performer;

		public PerformableActions (IActionPerformer performer) {
			this.performer = performer;
		}

		public void Add (string id, Action action) {
			action.Performer = performer;
			AddAction (id, action);
		}

		public void Activate (string id) {
			Action action;
			if (Actions.TryGetValue (id, out action)) {
				activeActions.Add (id, action);
			}
		}

		public void Deactivate (string id) {
			activeActions.Remove (id);
		}

		/**
		 *	Debugging
		 */

		public override void Print () {
			Debug.Log ("All actions:");
			base.Print ();
			Debug.Log ("Active actions:");
			foreach (var action in activeActions) {
				Debug.Log (action.Key);
			}
		}
	}
}