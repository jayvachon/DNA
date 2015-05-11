using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameActions {

	public delegate void ActionsUpdated ();

	public abstract class ActionList<T> where T : Action {

		public ActionsUpdated actionsUpdated;

		Dictionary<string, Action> actions = new Dictionary<string, Action> ();

		////
		// TODO: Add an Active property to Actions. ActionPerformers/Acceptors can set an
		// action's active state to include/execlude it in the list of enableable actions
		// Basically - enabling/disabling should all be handled internally by the action system,
		// but Active/Inactive should be set externally
		Dictionary<string, T> activeActions = new Dictionary<string, T> ();
		public Dictionary<string, T> ActiveActions {
			get { return activeActions; }
			protected set { activeActions = value; }
		}
		////

		Dictionary<string, T> enabledActions = new Dictionary<string, T> ();
		public Dictionary<string, T> EnabledActions {
			get { return enabledActions; }
			protected set { enabledActions = value; }
		}

		public void AddAction (Action action) {
			string id = action.Name;
			actions.Add (id, action);
			EnabledActions.Add (id, action as T);
			ActiveActions.Add (id, action as T);
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

		public void DeactivateAll () {
			ActiveActions.Clear ();
			NotifyActionsUpdated ();
			OnSetActive ();
		}

		/*protected void SetEnabled (string id, bool enabled) {
			if (enabled)
				Enable (id);
			else
				Disable (id);
			NotifyActionsUpdated ();
		}

		void Enable (string id) {
			Action action;
			if (actions.TryGetValue (id, out action)) {
				if (!EnabledActions.ContainsKey (id))
					EnabledActions.Add (id, action as T);
			}
			action.Enabled = true;
		}

		void Disable (string id) {
			EnabledActions.Remove (id);
			Get (id).Enabled = false;
		}

		protected void DisableAll () {
			foreach (var keyval in actions) {
				Action action = keyval.Value;
				action.Enabled = false;
			}
			EnabledActions.Clear ();
			NotifyActionsUpdated ();
		}*/

		public T Get (string id) {
			return actions[id] as T;
		}

		public bool Has (string id) {
			return ActiveActions.ContainsKey (id);
		}

		public virtual void RefreshEnabledActions () {
			EnabledActions.Clear ();
			foreach (var keyval in actions) {
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