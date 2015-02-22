using UnityEngine;
using System.Collections;
using GameActions;

public class ActionsDrawer : MBRefs {

	public PerformableActions Performable { get; set; }

	string ActionsListContents {
		get {
			string contents = "";
			/*foreach (Action action in ActionsList.Actions) {
				contents += string.Format ("{0}\n", action.Name);
			}*/
			return contents;
		}
	}

	public static void Create (Transform otherTransform, PerformableActions performable) {
		GameObject go = new GameObject("ActionsDrawer", typeof (ActionsDrawer));
		go.transform.SetParent (otherTransform);
		ActionsDrawer drawer = go.GetComponent<ActionsDrawer>();
		drawer.Performable = performable;
	}

	void OnGUI () {
		Vector3 pos = Camera.main.WorldToScreenPoint(MyTransform.position);
		Vector2 labelPos = new Vector2 (pos.x, Screen.height - pos.y);
		GUI.color = Color.black;
		GUI.Label (new Rect (labelPos.x-50, labelPos.y, 200, 200), ActionsListContents);
	}

	public void OnEndAction () {}
}
