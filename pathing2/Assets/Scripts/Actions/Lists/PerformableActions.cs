using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameInventory;

namespace GameActions {

	public delegate void StartAction (string id);

	public class PerformableActions : ActionList<PerformerAction> {
		
		public StartAction StartAction { get; set; }		
		IActionPerformer performer;

		// If given an (optional) input name, actions will be shown in the unit info box
		// and can be selected by a player
		Dictionary<string, string> inputs = new Dictionary<string, string> ();
		public Dictionary<string, string> Inputs {
			get { return inputs; }
		}

		public PerformableActions (IActionPerformer performer) {
			this.performer = performer;
		}

		public void Add (string id, PerformerAction action, string inputName="") {
			action.Performer = performer;
			AddAction (id, action);
			if (inputName != "") {
				inputs.Add (id, inputName);
			}
		}

		public void Start (string id) {
			Get (id).Start ();
			if (StartAction != null)
				StartAction (id);
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