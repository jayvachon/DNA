using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameInventory;

namespace GameActions {

	public delegate void OnStartAction (string id);

	public class PerformableActions : ActionList<PerformerAction> {
		
		public OnStartAction OnStartAction { get; set; }		
		IActionPerformer performer;

		// If given an (optional) input name, actions will be shown in the unit info box
		// and can be selected by a player
		// TODO: Use ActionDisplaySettings instead
		Dictionary<string, string> inputs = new Dictionary<string, string> ();
		public Dictionary<string, string> Inputs {
			get { return inputs; }
		}

		Queue<PerformerAction> actionQueue = new Queue<PerformerAction> ();
		bool debug = false;

		public bool Performing { get; private set; }

		public PerformableActions (IActionPerformer performer, bool debug=false) {
			this.performer = performer;
			this.debug = debug;
		}

		public void Add (PerformerAction action, string inputName="") {
			action.Performer = performer;
			AddAction (action);
			if (inputName != "") {
				inputs.Add (action.Name, inputName);
			}
		}

		public void Start (string id) {
			PerformerAction action = Get (id);
			if (action.autoStart) {
				StartAction (action);
			} else if (QueueAction (action)) {
				StartQueued ();
			}
		}

		public void Stop (string id) {
			PerformerAction action = Get (id);
			action.Stop ();
			if (!action.autoStart)
				StartQueued ();
		}

		public void StopAll () {
			bool stoppedQueuedAction = false;
			foreach (var keyval in EnabledActions) {
				PerformerAction action = keyval.Value as PerformerAction;
				if (!action.autoStart) 
					stoppedQueuedAction = true;
				action.Stop ();
			}
			if (stoppedQueuedAction) 
				StartQueued ();
		}

		public void OnActionStop (PerformerAction action) {
			if (!action.autoStart)
				StartQueued ();
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

		bool QueueAction (PerformerAction action) {
			actionQueue.Enqueue (action);
			return actionQueue.Count == 1;
		}

		void StartQueued () {
			if (actionQueue.Count < 1) {
				Log ("stop");
				Performing = false;
				return;
			}
			Performing = true;
			StartAction (actionQueue.Dequeue ());
		}

		void StartAction (PerformerAction action) {
			Log ("start " + action + " " + actionQueue.Count);
			action.Start ();
			if (OnStartAction != null)
				OnStartAction (action.Name);
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

		void Log (string message) {
			if (debug) Debug.Log (message);
		}
	}
}