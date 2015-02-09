 using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameActions {

	public class ActionDrawer : MonoBehaviour {

		/*ActionList actionList;
		Dictionary<System.Type, Action> actions;
		static int xLabel = 0;
		int myXLabel;

		public static void Create (Transform parent, ActionList actionList) {
			GameObject go = new GameObject ("ActionDrawer", typeof (ActionDrawer));
			go.transform.SetParent (parent);
			ActionDrawer drawer = go.GetComponent<ActionDrawer> ();
			drawer.Init (actionList);
		}

		public void Init (ActionList actionList) {
			this.actionList = actionList;
			actions = actionList.Actions;
			myXLabel = xLabel;
			xLabel += 350;
		}

		void OnGUI () {
			int y = 0;
			foreach (var pair in actions) {
				GUI.Label (new Rect (myXLabel, y, 350, 200), pair.Key.ToString ());
				y += 20;
			}
		}*/
	}
}