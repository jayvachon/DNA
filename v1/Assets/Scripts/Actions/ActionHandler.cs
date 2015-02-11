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
			
			PerformerAction matchingAction = null;
			IActionPerformer performer     = binder as IActionPerformer;
			PerformableActions performable = performer.PerformableActions;
			AcceptableActions acceptable   = binder.BoundAcceptor.AcceptableActions;

			performable.RefreshEnabledActions ();
			acceptable.Bind (performer);
			acceptable.RefreshEnabledActions ();

			foreach (var action in performable.EnabledActions) {
				// TODO: Left off here! Need to pass the AcceptCondition to the PerformerAction so that
				// e.g. it can make sure it only transfers sick ElderlyItems
				//AcceptorAction acceptorAction;
				//if ()
				if (acceptable.EnabledActions.ContainsKey (action.Key)) {
					PerformerAction performerAction = action.Value;
					//performerAction.Bind ()
					matchingAction = performerAction;
					break;
				}
			}

			StartCoroutine (PerformActions (binder, matchingAction));
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

		IEnumerator PerformActions (IBinder binder, PerformerAction action) {
			if (action != null) {
				yield return StartCoroutine (Perform (action));
				Bind (binder);
			} else {
				IActionPerformer performer = binder as IActionPerformer;
				performer.PerformableActions.RefreshEnabledActions ();
				binder.OnEndActions ();
			}
		}
	}
}