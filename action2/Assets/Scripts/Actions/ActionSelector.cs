using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameActions {

	public class ActionSelector {

		ActionList takerActions;
		ActionList receiverActions;
		ActionList matchingActions;

		public ActionSelector (ActionList takerActions, ActionList receiverActions) {
			this.takerActions = takerActions;
			this.receiverActions = receiverActions;
			MatchActions ();
		}

		void MatchActions () {
			matchingActions = new ActionList ();
			foreach (Action action in receiverActions.Actions) {
				if (takerActions.Has (action)) {
					matchingActions.Add (action);
				}
			}
		}
	}
}

// Given two ActionLists (from a Taker and Receiver), choose an Action
// that the Taker wants to take and the Receiver wants to receive
// There needs to be an ability to weight actions. e.g. if a Taker is
// transporting a sick Elder, it will want to drop the Elder off at
// the hospital rather than the Milkshakes it's holding

// orrrrr ----> the Taker takes all actions that it can possibly take!
// 1 thing to consider about this is that e.g. when a Taker visits a 
// house, it will try to pick up and drop off an Elder. need to have a 
// way of checking if e.g. the Elder is sick (the Item can be picked up)
// this is something that Items should handle though -- have a bool that
// says whether the Item is transportable

// SO:
// the ActionSelector grabs both ActionLists and finds Actions that match.
// it then has the Taker and Receiver perform each matching Action
// The MatchingActions list will need to be dynamic in the event that
// another Taker arrives at the Receiver before the matching action can be
// performed