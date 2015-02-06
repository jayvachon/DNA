using UnityEngine;
using System.Collections;

namespace GameActions {

	public class ActionHandler : MonoBehaviour {
		
		static ActionHandler instanceInternal = null;
		static public ActionHandler instance {
			get {
				if (instanceInternal == null) {
					instanceInternal = Object.FindObjectOfType(typeof (ActionHandler)) as ActionHandler;
					if (instanceInternal == null) {
						GameObject go = new GameObject ("ActionHandler");
						DontDestroyOnLoad(go);
						instanceInternal = go.AddComponent<ActionHandler>();
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