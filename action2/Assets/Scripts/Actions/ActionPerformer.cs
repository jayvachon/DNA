using UnityEngine;
using System.Collections;

namespace GameActions {

	public class ActionPerformer : MonoBehaviour {
		
		static ActionPerformer instanceInternal = null;
		static public ActionPerformer instance {
			get {
				if (instanceInternal == null) {
					instanceInternal = Object.FindObjectOfType(typeof (ActionPerformer)) as ActionPerformer;
					if (instanceInternal == null) {
						GameObject go = new GameObject ("ActionPerformer");
						DontDestroyOnLoad(go);
						instanceInternal = go.AddComponent<ActionPerformer>();
					}
				}
				return instanceInternal;
			}
		}

		public void StartAction (Action action) {
			StartCoroutine (Perform (action));
		}

		IEnumerator Perform (Action action) {

			float time = action.Duration;
			float eTime = 0;

			while (eTime < time) {
				eTime += Time.deltaTime;
				action.Perform (eTime / time);
				yield return null;
			}

			action.End ();
		}
	}
}