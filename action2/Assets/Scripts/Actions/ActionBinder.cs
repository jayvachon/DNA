using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameActions {

	public static class ActionBinder {

		public static List<PerformerAction> Bind (IActionPerformer performer, IActionAcceptor acceptor) {
			
			List<PerformerAction> matchingActions = new List<PerformerAction> ();
			PerformableActions performable = performer.PerformableActions;
			AcceptableActions acceptable = acceptor.AcceptableActions;
			performable.RefreshEnabledActions ();
			acceptable.RefreshEnabledActions ();

			foreach (var action in performable.EnabledActions) {
				if (acceptable.EnabledActions.ContainsKey (action.Key)) {
					matchingActions.Add (action.Value);
				}
			}

			return matchingActions;
		}
	}
}