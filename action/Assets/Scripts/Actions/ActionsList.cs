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

		public void Add (Action action) {
			actions.Add (action);
			if (actions.Count == 1) {
				activeAction = actions[0];
			}
		}

		public void Start<T> (params object[] args) where T : Action {
			Get<T> ().Start (args);
		}

		Action Get<T> () where T : Action {
			foreach (Action action in actions) {
				if (action is T)
					return action;
			}
			Debug.LogError (string.Format ("Action {0} does not exist", typeof (T)));
			return null;
		}
	}
}