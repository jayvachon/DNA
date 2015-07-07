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

		public Action this[string id] {
			get { return actions[id]; }
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
					ActiveActions.Add (id, (T)action);
				}
			}
			action.Active = true;
			if (action.Enabled) EnabledActions.Add (id, (T)action);
		}

		void Deactivate (string id) {
			EnabledActions.Remove (id);
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
			try {
				return actions[id] as T;
			} catch (System.Exception e) {
				Debug.LogError ("The action '" + id + "' does not exist in the list\n" + e);
				throw;
			}
		}

		public T GetEnabledAction () {
			foreach (var action in enabledActions) {
				return action.Value;
			}
			return null;
		}

		public T GetActiveAction () {
			foreach (var action in activeActions) {
				return action.Value;
			}
			return null;
		}

		public bool Has (string id) {
			return ActiveActions.ContainsKey (id);
		}

		public bool ActionEnabled (string id) {
			return EnabledActions.ContainsKey (id);
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

		/*public void PairActionsBetweenAcceptors (List<IActionAcceptor> acceptors) {
			foreach (var action in ActiveActions) {
				action.Value.EnabledState.AttemptPair (acceptors);
			}
		}

		public void PairActionsOnPath (Pathing.Path path) {
			PairActionsBetweenAcceptors (
				path.Points.Points.ConvertAll (x => x.StaticUnit as IActionAcceptor));
		}*/

		public List<string> GetBoundActions (List<string> acceptorActions) {
			//RefreshEnabledActions ();
			return acceptorActions.FindAll (x => Has (x));//ActionEnabled (x));
		}

		public bool AcceptorHasPair (IActionAcceptor acceptor, Action unpairedAction) {
			return unpairedAction.EnabledState.AttemptPair (acceptor);
		}

		public List<string> GetPairedActionsBetweenAcceptors (IActionAcceptor a, IActionAcceptor b) {
			AcceptableActions aa = a.AcceptableActions;
			AcceptableActions ba = b.AcceptableActions;
			aa.RefreshEnabledActions ();
			ba.RefreshEnabledActions ();
			List<string> paired = new List<string> ();
			Dictionary<string, AcceptorAction> aActions = aa.ActiveActions;//aa.EnabledActions;
			foreach (var action in aActions) {
				if (action.Value.EnabledState.AttemptPair (b))
					paired.Add (action.Key);
			}
			return paired;
		}

		public bool HasMatchingAction (Action action) {
			return activeActions.ContainsKey (action.Name);
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

		public void PrintEnabled () {
			foreach (var action in EnabledActions) {
				Debug.Log (action.Key);
			}
		}
	}
}