using UnityEngine;
using System.Collections;

namespace GameActions {

	public class ActionsManager : MonoBehaviour {
		
		static ActionsManager instanceInternal = null;
		static public ActionsManager instance {
			get {
				if (instanceInternal == null) {
					instanceInternal = Object.FindObjectOfType(typeof (ActionsManager)) as ActionsManager;
					if (instanceInternal == null) {
						GameObject go = new GameObject ("ActionsManager");
						DontDestroyOnLoad(go);
						instanceInternal = go.AddComponent<ActionsManager>();
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