using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameActions {

	public class ActionMatcher {

		Dictionary<string, Action> matchingActions = new Dictionary<string, Action> ();

		public void MatchActions (PerformableActions performable, AcceptableActions acceptable) {
			matchingActions.Clear ();
			foreach (var action in performable.ActiveActions) {
				if (acceptable.Actions.ContainsKey (action.Key)) {
					matchingActions.Add (action.Key, action.Value);
				}
			}
		}

		public void Print () {
			foreach (var action in matchingActions) {
				Debug.Log (action.Key);
			}
		}
	}
}