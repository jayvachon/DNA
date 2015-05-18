using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameActions {

	/**
	 *	ActionList
	 *	- PerformableActions
	 *		> PerformerAction
	 *		> IActionPerformer
	 *	- AcceptableActions
	 *		> AcceptorAction
	 *		> IActionAcceptor
	 *
	 */

	public delegate void ActionsUpdated ();

	public abstract class ActionList<T> where T : Action {

		public ActionsUpdated actionsUpdated;

		// All actions within this ActionList
		Dictionary<string, Action> actions = new Dictionary<string, Action> ();

		// Inactive actions are not considered for enabling
		Dictionary<string, T> activeActions = new Dictionary<string, T> ();
		public Dictionary<string, T> ActiveActions {
			get { return activeActions; }
			protected set { activeActions = value; }
		}

		// Actions are enabled if their EnabledState is true
		Dictionary<string, T> enabledActions = new Dictionary<string, T> ();
		public Dictionary<string, T> EnabledActions {
			get { return enabledActions; }
			protected set { enabledActions = value; }
		}

		public void AddAction (Action action) {
			string id = action.Name;
			actions.Add (id, action);
			ActiveActions.Add (id, action as T);
			EnabledActions.Add (id, action as T);
			NotifyActionsUpdated ();
		}

		public void SetActive (string id, bool active) {
			if (active) 
				Activate (id);
			else
				Deactivate (id);
			NotifyActionsUpdated ();
			OnSetActive ();	
		}

		void Activate (string id) {
			Action action;
			if (actions.TryGetValue (id, out action)) {
				if (!ActiveActions.ContainsKey (id)) {
					ActiveActions.Add (id, action as T);
				}
			}
			action.Active = true;
		}

		void Deactivate (string id) {
			ActiveActions.Remove (id);
			Get (id).Active = false;
		}

		public void ActivateAll () {
			ActiveActions.Clear ();
			foreach (var action in actions) {
				action.Value.Active = true;
				ActiveActions.Add (action.Key, action.Value as T);
			}
			NotifyActionsUpdated ();
			OnSetActive ();
		}

		public void DeactivateAll () {
			ActiveActions.Clear ();
			NotifyActionsUpdated ();
			OnSetActive ();
		}

		public T Get (string id) {
			return actions[id] as T;
		}

		public bool Has (string id) {
			return ActiveActions.ContainsKey (id);
		}

		public virtual void RefreshEnabledActions () {
			EnabledActions.Clear ();
			foreach (var keyval in ActiveActions) {
				Action action = keyval.Value;
				if (action.Enabled) {
					EnabledActions.Add (keyval.Key, action as T);
				}
			}
			NotifyActionsUpdated ();
		}

		public void NotifyActionsUpdated () {
			if (actionsUpdated != null) {
				actionsUpdated ();
			}
		}

		public bool PairActionsBetweenAcceptors (List<IActionAcceptor> acceptors) {
			bool hasPair = false;
			foreach (var action in ActiveActions) {
				if (action.Value.EnabledState.AttemptPair (acceptors))
					hasPair = true;
			}
			return hasPair;
		}

		public virtual void OnSetActive () {}

		/**
		 *	Debugging
		 */

		public List<PerformerAction> EnabledActionsList {
			get {
				List<PerformerAction> list = new List<PerformerAction> ();
				foreach (var action in EnabledActions) {
					list.Add (action.Value as PerformerAction);
				}
				return list;
			}
		}

		public virtual void Print () {
			foreach (var action in actions) {
				Debug.Log (action.Key 
					+ " enabled = " + action.Value.Enabled 
					+ ", active = " + action.Value.Active);
			}
		}
	}
}