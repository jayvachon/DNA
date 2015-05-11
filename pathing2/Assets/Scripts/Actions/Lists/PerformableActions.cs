using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameInventory;

namespace GameActions {

	public delegate void StartAction (string id);

	public class PerformableActions : ActionList<PerformerAction> {
		
		public StartAction StartAction { get; set; }		
		IActionPerformer performer;

		// If given an (optional) input name, actions will be shown in the unit info box
		// and can be selected by a player
		// TODO: Use ActionDisplaySettings instead
		Dictionary<string, string> inputs = new Dictionary<string, string> ();
		public Dictionary<string, string> Inputs {
			get { return inputs; }
		}

		public PerformableActions (IActionPerformer performer) {
			this.performer = performer;
		}

		public void Add (PerformerAction action, string inputName="") {
			action.Performer = performer;
			action.Duration = TimerValues.GetActionTime (action.Name);
			AddAction (action);
			if (inputName != "") {
				inputs.Add (action.Name, inputName);
			}
		}

		public void Start (string id) {
			Get (id).Start ();
			if (StartAction != null)
				StartAction (id);
		}

		public void Stop (string id) {
			Get (id).Stop ();
		}

		public override void OnSetActive () {
			inputs.Clear ();
			foreach (var action in ActiveActions) {
				string name = action.Value.Name;
				if (action.Value.Active && inputs.ContainsKey (name)) {
					inputs.Add (name, inputs[name]);
				}
			}
		}

		public override void RefreshEnabledActions () {
			EnabledActions.Clear ();
			foreach (var keyval in ActiveActions) {
				PerformerAction action = keyval.Value as PerformerAction;
				if (action.Enabled) {
					EnabledActions.Add (keyval.Key, action);
				}
			}
			NotifyActionsUpdated ();
		}

		public void PairActionsBetweenAcceptors (List<IActionAcceptor> acceptors) {
			foreach (var action in ActiveActions) {
				action.Value.EnabledState.AttemptPair (acceptors);
			}
		}

		/**
		 *	Debugging
		 */

		public override void Print () {
			Debug.Log ("PERFORMABLE");
			Debug.Log ("All actions:");
			base.Print ();
			Debug.Log ("Enabled actions:");
			foreach (var action in EnabledActions) {
				Debug.Log (action.Key);
			}
			Debug.Log ("Active actions");
			foreach (var action in ActiveActions) {
				Debug.Log (action.Key);
			}
		}
	}
}