using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameActions {

	public class ActionsList {

		List<Action> actions = new List<Action> ();
		public List<Action> Actions {
			get { return actions; }
		}

		// get rid of this?
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

		public void Remove<T> () where T : Action {
			actions.Remove (Get<T> ());
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
			
			AcceptedActions acceptedActions = acceptor.AcceptedActions;

			if (acceptedActions.Has (activeAction)) {
				return activeAction;
			}

			foreach (Action action in actions) {
				if (acceptedActions.Has (action))
					return action;
			}
			return null;
		}
	}
}