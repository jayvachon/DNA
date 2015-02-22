 using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameActions {

	public class ActionDrawer : MBRefs {

		List<PerformerAction> actions = new List<PerformerAction> ();
		string ActionsContent {
			get {
				string content = "";
				foreach (PerformerAction action in actions) {
					content += action.Name + "\n";
				}
				return content;
			}
		}

		public static ActionDrawer Create (Transform parent, List<PerformerAction> actions) {
			GameObject go = new GameObject ("ActionDrawer", typeof (ActionDrawer));
			go.transform.SetParent (parent);
			ActionDrawer drawer = go.GetComponent<ActionDrawer> ();
			drawer.Init (actions);
			return drawer;
		}

		public void Init (List<PerformerAction> actions) {
			UpdateList (actions);
		}

		public void UpdateList (List<PerformerAction> actions) {
			this.actions = actions;
		}

		void OnGUI () {
			Vector2 labelPos = V2Position;
			GUI.color = Color.black;
			GUI.Label (new Rect (labelPos.x-50, labelPos.y+50, 200, 200), ActionsContent);
		}
	}
}