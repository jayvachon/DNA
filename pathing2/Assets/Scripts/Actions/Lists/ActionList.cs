using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameActions {

	public abstract class ActionList<T> where T : Action {

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
			actions.Add (id, action);
		}

		public void Enable (string id) {
			Action action;
			if (Actions.TryGetValue (id, out action)) {
				enabledActions.Add (id, action as T);
			}
			OnEnable (id);
		}

		public virtual void OnEnable (string id) {}

		public void Disable (string id) {
			enabledActions.Remove (id);
			OnDisable (id);
		}

		public virtual void OnDisable (string id) {}

		public void DisableAll () {
			enabledActions.Clear ();
			OnDisableAll ();
		}

		public virtual void OnDisableAll () {}

		public T Get (string id) {
			return Actions[id] as T;
		}

		public void RefreshEnabledActions () {
			enabledActions.Clear ();
			foreach (var keyval in actions) {
				Action action = keyval.Value;
				if (action.Enabled) {
					enabledActions.Add (keyval.Key, action as T);
				}
			}

			// Debugging
			UpdateDrawer ();
		}

		/**
		 *	Debugging
		 */

		ActionDrawer drawer = null;

		public List<PerformerAction> EnabledActionsList {
			get {
				List<PerformerAction> list = new List<PerformerAction> ();
				foreach (var action in enabledActions) {
					list.Add (action.Value as PerformerAction);
				}
				return list;
			}
		}

		public void SetDrawer (ActionDrawer drawer) {
			this.drawer = drawer;
		}

		void UpdateDrawer () {
			if (drawer != null) {
				drawer.UpdateList (EnabledActionsList);
			}
		}

		public virtual void Print () {
			foreach (var action in actions) {
				Debug.Log (action.Key);
			}
		}
	}
}