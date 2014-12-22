using UnityEngine;
using System.Collections;

public class MouseController : MonoBehaviour {

	private float maxDistance = 5000f;

	void FixedUpdate () {
		if (Input.GetMouseButtonDown (0)) {
			Click (true);
		}
		if (Input.GetMouseButtonDown (1)) {
			Click (false);
		}
	}

	void Click (bool leftClick) {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, maxDistance)) {
			Transform t = hit.transform;
			IClickable clickable = t.GetScript<IClickable>();
			if (clickable != null) {
				if (leftClick) {
					clickable.LeftClick ();
				} else {
					clickable.RightClick ();
				}
			} else {
				Events.instance.Raise (new NullClickEvent ());
			}
		}
	}
}
