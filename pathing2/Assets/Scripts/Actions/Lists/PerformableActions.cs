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

		/*public void EnableAcceptedActionsBetweenAcceptors (List<IActionAcceptor> acceptors) {
			// DisableAll ();
			EnableAcceptedActions (GetAcceptedActionsBetweenAcceptors (acceptors));
		} 

		List<KeyValuePair<string, IActionAcceptor>> GetAcceptedActionsBetweenAcceptors (List<IActionAcceptor> acceptors) {
			
			List<KeyValuePair<string, IActionAcceptor>> acceptedActions = 
				new List<KeyValuePair<string, IActionAcceptor>> ();

			foreach (IActionAcceptor acceptor in acceptors) {
				
				List<string> actions = GetAcceptedActions (acceptor);
				foreach (string action in actions) {
					acceptedActions.Add (
						new KeyValuePair<string, IActionAcceptor> (action, acceptor)
					);
				}
			}
			return acceptedActions;
		}

		void EnableAcceptedActions (List<KeyValuePair<string, IActionAcceptor>> acceptedActions) {

			Dictionary<KeyValuePair<string, IActionAcceptor>, System.Type> unpairedActions = 
				new Dictionary<KeyValuePair<string, IActionAcceptor>, System.Type> ();

			foreach (var acceptedAction in acceptedActions) {

				string actionName 										= acceptedAction.Key;
				PerformerAction action 									= Get (actionName);
				System.Type requiredPair								= action.RequiredPair;
				KeyValuePair<string, IActionAcceptor> acceptorAction	= new KeyValuePair<string, IActionAcceptor> (actionName, acceptedAction.Value);

				// Enable the action if it doesn't require a pair
				if (requiredPair == null) {
					// SetEnabled (actionName, true);
					continue;
				}

				// If unpairedActions is empty, add the action
				if (unpairedActions.Count == 0) {
					unpairedActions.Add (acceptorAction, action.GetType ());
					continue;
				}

				// If unpairedActions isn't empty, try to find the pair in unpairedActions
				// Both actions are enabled if they are pairs & not from the same IActionAcceptor
				string pair = "";
				foreach (var unpairedAction in unpairedActions) {
					var keyVal = unpairedAction.Key;
					if (unpairedAction.Value == requiredPair && keyVal.Value != acceptorAction.Value) {
						pair = keyVal.Key;
						// SetEnabled (pair, true);
						// SetEnabled (actionName, true);
						break;
					}
				}

				// If no pair was found, add the action to the dictionary
				if (pair == "") {
					unpairedActions.Add (acceptorAction, action.GetType ());
				} else {

					// But if a pair WAS found, remove it from the dictionary
					unpairedActions.Remove (acceptorAction);
				}
			}
		}

		List<string> GetAcceptedActions (IActionAcceptor acceptor) {
			List<string> acceptedActions = new List<string> ();
			if (acceptor == null) {
				return acceptedActions;
			}
			foreach (var action in acceptor.AcceptableActions.ActiveActions) {
				string key = action.Key;
				if (ActiveActions.ContainsKey (key))
					acceptedActions.Add (key);
			}
			return acceptedActions;
		}*/

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