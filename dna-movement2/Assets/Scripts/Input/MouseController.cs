using UnityEngine;
using System.Collections;

public class MouseController : MonoBehaviour {

	void FixedUpdate () {
		if (Input.GetMouseButtonDown (0)) {
			Click (true);
		}
		if (Input.GetMouseButtonDown (1)) {
			Click (false);
		}
	}

	void Click (bool leftClick) {
		Transform t = HandleClick ();
		if (t != null) {
			IClickable clickable = t.GetScript<IClickable>();
			if (clickable == null) {
				SelectionManager.Unselect ();
				return;
			}
			if (leftClick) {
				clickable.LeftClick ();
			} else {
				clickable.RightClick ();
			}
		}
	}

	Transform HandleClick () {
		Vector2 mousePosition = Input.mousePosition;
		if (mousePosition.x < 100 && mousePosition.y > Screen.height-100)
			return null;
		Ray ray = Camera.main.ScreenPointToRay (mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, Mathf.Infinity)) {
			return hit.transform;
		}
		return null;
	}
}
