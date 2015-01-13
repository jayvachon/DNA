using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameActions {

	public class ActionsList {

		List<Action> actions = new List<Action> ();
		public List<Action> Actions {
			get { return actions; }
		}

		Action activeAction = null;
		public Action ActiveAction {
			get { return activeAction; }
			set { activeAction = value; }
		}

		public int Count {
			get { return actions.Count; }
		}

		IActionable actionable;

		public ActionsList (IActionable actionable=null) {
			this.actionable = actionable;
		}

		public void Add (Action action) {
			if (actionable != null) action.Actionable = actionable;
			actions.Add (action);
			if (actions.Count == 1) {
				activeAction = actions[0];
			}
		}

		// Start the active action
		public void Start (IActionAcceptor acceptor=null) {
			Action acceptableAction = GetAcceptableAction (acceptor);
			if (acceptableAction != null) {
				acceptableAction.Start (acceptor);
			} else {
				actionable.OnEndAction ();
			}
		}

		// Start a specific action
		public void Start<T> (IActionAcceptor acceptor=null) where T : Action {
			Get<T> ().Start (acceptor);
		}

		Action Get<T> () where T : Action {
			foreach (Action action in actions) {
				if (action is T)
					return action;
			}
			Debug.LogError (string.Format ("Action {0} does not exist", typeof (T)));
			return null;
		}

		Action GetAcceptableAction (IActionAcceptor acceptor) {
			
			List<Action> acceptedActions = acceptor.AcceptedActions.Actions;
			
			// Check if we can do the active action (maybe this is not important & should be removed?)
			foreach (Action action in acceptedActions) {
				if (action.GetType () == activeAction.GetType ())
					return activeAction;
			}

			// Check if any other actions are acceptable
			foreach (Action acceptedAction in acceptedActions) {
				foreach (Action action in actions) {
					if (action.GetType () == acceptedAction.GetType ())
						return action;
				}
			}
			return null;
		}
	}
}