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

		public delegate void OnClick (System.Type type);

		public OnClick onClick;

		void Start () {
			if (onClick != null)
				onClick (null);
		}

		void Update () {
			if (Input.GetMouseButtonDown (0) || Input.GetMouseButtonDown (1)) {
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (onClick != null) {

					System.Type clickType = null;

					if (Physics.Raycast (ray, out hit, Mathf.Infinity, 1 << (int)InputLayer.UI)) {
						return;
					}

					if (Physics.Raycast (ray, out hit, Mathf.Infinity)) {
						MonoBehaviour mb = hit.transform.GetComponent<MonoBehaviour> ();
						if (mb != null)
							clickType = mb.GetType ();
					}

					onClick (clickType);
				}
			}
		}
	}
}