using UnityEngine;
using System.Collections;
using DNA.Units;

namespace DNA.InputSystem {

	public class EmptyClickHandler : MonoBehaviour {

		static EmptyClickHandler instance = null;
		static public EmptyClickHandler Instance {
			get {
				if (instance == null) {
					instance = Object.FindObjectOfType (typeof (EmptyClickHandler)) as EmptyClickHandler;
				}
				return instance;
			}
		}

		public delegate void OnClick ();

		public OnClick onClick;

		void Start () {
			if (onClick != null)
				onClick ();
		}

		void Update () {
			if (Input.GetMouseButtonDown (0) || Input.GetMouseButtonDown (1)) {
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit, Mathf.Infinity)) {
					// TODO: figure out a system for distinguishing what's an empty click
					if (hit.transform.GetComponent<MonoBehaviour> () is FogOfWar && onClick != null)
						onClick ();
				} else {
					if (onClick != null)
						onClick ();
				}
			}
		}
	}
}