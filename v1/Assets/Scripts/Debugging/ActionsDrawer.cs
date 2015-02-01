using UnityEngine;
using System.Collections;
using GameActions;

public class ActionsDrawer : MBRefs, IActionable {

	public ActionsList ActionsList { get; set; }

	public static void Create (Transform otherTransform, ActionsList drawActionsList) {
		GameObject go = new GameObject("ActionsDrawer", typeof (ActionsDrawer));
		go.transform.SetParent (otherTransform);
		ActionsDrawer drawer = go.GetComponent<ActionsDrawer>();
		drawer.ActionsList = drawActionsList;
	}

	void OnGUI () {
		Vector3 pos = Camera.main.WorldToScreenPoint(MyTransform.position);
		Vector2 labelPos = new Vector2 (pos.x, Screen.height - pos.y);
		GUI.color = Color.black;
		GUI.Label (new Rect (labelPos.x-50, labelPos.y, 200, 200), ActionsListContents ());
	}

	string ActionsListContents () {
		string contents = "";
		foreach (Action action in ActionsList.Actions) {
			contents += string.Format ("{0}\n", action.Name);
		}
		return contents;
	}

	public void OnEndAction () {}
}
