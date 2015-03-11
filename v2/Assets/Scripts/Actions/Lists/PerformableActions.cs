using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameInventory;

namespace GameActions {

	public class PerformableActions : ActionList<PerformerAction> {
		
		IActionPerformer performer;

		public PerformableActions (IActionPerformer performer) {
			this.performer = performer;
		}

		public void Add (string id, PerformerAction action) {
			action.Performer = performer;
			AddAction (id, action);
		}

		public void Start (string id) {
			Get (id).Start ();
		}

		/**
		 *	Debugging
		 */

		public override void Print () {
			Debug.Log ("All actions:");
			base.Print ();
			Debug.Log ("Active actions:");
			foreach (var action in EnabledActions) {
				Debug.Log (action.Key);
			}
		}
	}
}