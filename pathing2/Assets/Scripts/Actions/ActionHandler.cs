using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameActions {

	public class ActionHandler : MonoBehaviour {
		
		static ActionHandler instanceInternal = null;
		static public ActionHandler instance {
			get {
				if (instanceInternal == null) {
					instanceInternal = Object.FindObjectOfType (typeof (ActionHandler)) as ActionHandler;
					if (instanceInternal == null) {
						GameObject go = new GameObject ("ActionHandler");
						DontDestroyOnLoad (go);
						instanceInternal = go.AddComponent<ActionHandler>();
					}
				}
				return instanceInternal;
			}
		}

		/**
		 * Perform multiple actions when binding to an ActionAcceptor
		 */

		public PerformerAction Bind (IBinder binder) {
			
			if (binder.BoundAcceptor == null) return null;
			
			IActionPerformer performer     = binder as IActionPerformer;
			PerformableActions performable = performer.PerformableActions;
			AcceptableActions acceptable   = binder.BoundAcceptor.AcceptableActions;

			performable.RefreshEnabledActions ();
			acceptable.Bind (performer);
			acceptable.RefreshEnabledActions ();

			List<PerformerAction> matching = new List<PerformerAction> ();
			foreach (var action in performable.EnabledActions) {
				AcceptorAction acceptorAction;
				if (acceptable.EnabledActions.TryGetValue (action.Key, out acceptorAction)) {
					PerformerAction performerAction = action.Value;
					matching.Add (performerAction);
				}
			}

			matching = PerformInstantActions (matching);

			if (matching.Count > 0) {
				StartCoroutine (PerformActions (binder, matching[0]));
				return matching[0];
			} else {
				StartCoroutine (PerformActions (binder, null));
				return null;
			}
		}
		
		IEnumerator PerformActions (IBinder binder, PerformerAction action) {
			if (action != null) {
				yield return StartCoroutine (BindPerform (action));
				Bind (binder);
			} else {
				IActionPerformer performer = binder as IActionPerformer;
				performer.PerformableActions.RefreshEnabledActions ();
				binder.OnEndActions ();
			}
		}

		List<PerformerAction> PerformInstantActions (List<PerformerAction> matchingActions) {
			List<PerformerAction> timedActions = new List<PerformerAction> ();
			foreach (PerformerAction action in matchingActions) {
				if (Mathf.Approximately (action.Duration, 0f)) {
					action.BindEnd ();
				} else {
					timedActions.Add (action);
				}
			}
			return timedActions;
		}

		/**
		 * Perform a single action
		 */

		public void StartAction (PerformerAction action) {
			StartCoroutine (Perform (action));
		}

		IEnumerator Perform (PerformerAction action) {
			
			float time = action.Duration;
			float eTime = 0;

			if (time == 0) {
				action.End ();
				yield break;
			}

			while (eTime < time) {
				eTime += Time.deltaTime;
				action.Progress = eTime / time;
				yield return null;
			}

			action.End ();
		}

		IEnumerator BindPerform (PerformerAction action) {
			
			float time = action.Duration;
			float eTime = 0;

			if (time == 0) {
				action.BindEnd ();
				yield break;
			}

			action.BindStart ();

			while (eTime < time) {
				eTime += Time.deltaTime;
				action.Progress = eTime / time;
				yield return null;
			}

			action.BindEnd ();
		}
	}
}