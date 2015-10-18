using UnityEngine;
using System.Collections;

namespace DNA.InputSystem {

	public class EmptyClickHandler : MonoBehaviour {

		static EmptyClickHandler instance = null;
		static public EmptyClickHandler Instance {
			get {
				if (instance == null) {
					instance = Object.FindObjectOfType (typeof (EmptyClickHandler)) as EmptyClickHandler;
					if (instance == null) {
						GameObject go = new GameObject ("EmptyClickHandler");
						DontDestroyOnLoad (go);
						instance = go.AddComponent<EmptyClickHandler>();
					}
				}
				return instance;
			}
		}

		public delegate void OnClick ();

		public OnClick onClick;

		void Update () {
			if (Input.GetMouseButtonDown (0) || Input.GetMouseButtonDown (1)) {
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (!Physics.Raycast (ray, out hit, Mathf.Infinity)) {
					if (onClick != null)
						onClick ();
				}
			}
		}
	}
}