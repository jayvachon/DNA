using UnityEngine;
using System.Collections;
using GameEvents;

namespace GameInput {
	
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
				RaiseClickMessage (hit, leftClick);
			}
		}

		void RaiseClickMessage (RaycastHit hit, bool leftClick) {
			Events.instance.Raise (new MouseClickEvent (hit, leftClick));
		}
	}
}