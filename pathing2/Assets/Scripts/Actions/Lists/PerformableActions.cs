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

		public override void OnEnable (string id) {
			string inputName;
			if (inputs.TryGetValue (id, out inputName)) {
				inputs.Add (id, inputName);
			}
		}

		public override void OnDisable (string id) {
			inputs.Remove (id);
		}

		public override void OnDisableAll () {
			inputs.Clear ();
		}

		public override void RefreshEnabledActions () {
			EnabledActions.Clear ();
			foreach (var keyval in Actions) {
				PerformerAction action = keyval.Value as PerformerAction;
				if (action.Enabled && action.CanPerform) {
					EnabledActions.Add (keyval.Key, action);
				}
			}
			NotifyActionsUpdated ();
		}

		public List<string> GetAcceptedActions (IActionAcceptor acceptor) {
			List<string> acceptedActions = new List<string> ();
			if (acceptor == null) {
				return acceptedActions;
			}
			foreach (var action in acceptor.AcceptableActions.Actions) {
				if (Actions[action.Key] != null) {
					acceptedActions.Add (action.Key);
				}
			}
			return acceptedActions;
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