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

		public void Bind (IActionPerformer performer, IActionAcceptor acceptor) {
			
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

			StartActions (matchingActions);
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

		public void StartActions (List<PerformerAction> actions) {
			StartCoroutine (PerformActions (actions));
		}

		IEnumerator PerformActions (List<PerformerAction> actions) {

			while (actions.Count > 0) {
				yield return StartCoroutine (Perform (actions[0]));
				actions.RemoveAt (0);
			}

			// TODO: Notify the ActionPerformer that all actions have been performed
		}
	}
}