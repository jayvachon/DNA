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

		public void Bind (IBinder binder) {
			
			List<PerformerAction> matchingActions = new List<PerformerAction> ();
			IActionPerformer performer     = binder as IActionPerformer;
			PerformableActions performable = performer.PerformableActions;
			AcceptableActions acceptable   = binder.BoundAcceptor.AcceptableActions;

			performable.RefreshEnabledActions ();
			acceptable.RefreshEnabledActions ();

			foreach (var action in performable.EnabledActions) {
				if (acceptable.EnabledActions.ContainsKey (action.Key)) {
					matchingActions.Add (action.Value);
				}
			}

			StartCoroutine (PerformActions (binder, matchingActions));
		}

		public void StartAction (PerformerAction action) {
			StartCoroutine (Perform (action));
		}

		IEnumerator Perform (PerformerAction action) {

			float time = action.Duration;
			float eTime = 0;

			while (eTime < time) {
				eTime += Time.deltaTime;
				action.Perform (eTime / time);
				yield return null;
			}

			action.End ();
		}

		IEnumerator PerformActions (IBinder binder, List<PerformerAction> actions) {

			while (actions.Count > 0) {
				yield return StartCoroutine (Perform (actions[0]));
				if (!actions[0].Enabled) {
					actions.RemoveAt (0);
				}
			}

			binder.OnEndActions ();
		}
	}
}