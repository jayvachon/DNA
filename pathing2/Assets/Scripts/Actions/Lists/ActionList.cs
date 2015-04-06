using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameActions {

	public delegate void ActionsUpdated ();

	public abstract class ActionList<T> where T : Action {

		public ActionsUpdated actionsUpdated;

		Dictionary<string, Action> actions = new Dictionary<string, Action> ();
		public Dictionary<string, Action> Actions {
			get { return actions; }
			protected set { actions = value; }
		}

		Dictionary<string, T> enabledActions = new Dictionary<string, T> ();
		public Dictionary<string, T> EnabledActions {
			get { return enabledActions; }
			protected set { enabledActions = value; }
		}

		public void AddAction (string id, Action action) {
			Actions.Add (id, action);
			EnabledActions.Add (id, action as T);
			NotifyActionsUpdated ();
		}

		public void Enable (string id) {
			Action action;
			if (Actions.TryGetValue (id, out action)) {
				EnabledActions.Add (id, action as T);
			}
			action.Enabled = true;
			OnEnable (id);
			NotifyActionsUpdated ();
		}

		public virtual void OnEnable (string id) {}

		public void Disable (string id) {
			EnabledActions.Remove (id);
			Get (id).Enabled = false;
			OnDisable (id);
			NotifyActionsUpdated ();
		}

		public virtual void OnDisable (string id) {}

		public void DisableAll () {
			foreach (var keyval in Actions) {
				Action action = keyval.Value;
				action.Enabled = false;
			}
			EnabledActions.Clear ();
			OnDisableAll ();
			NotifyActionsUpdated ();
		}

		public virtual void OnDisableAll () {}

		public T Get (string id) {
			return Actions[id] as T;
		}

		public virtual void RefreshEnabledActions () {
			EnabledActions.Clear ();
			foreach (var keyval in Actions) {
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
			foreach (var action in Actions) {
				Debug.Log (action.Key + " enabled ? " + action.Value.Enabled);
			}
		}
	}
}