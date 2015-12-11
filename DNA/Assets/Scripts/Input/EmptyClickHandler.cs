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

		public delegate void OnClick (ISelectable selectable);

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
					if (Physics.Raycast (ray, out hit, Mathf.Infinity)) {
						Debug.Log ((hit.transform.GetComponent<MonoBehaviour> ()).GetType ()); // TODO: pass the type into onClick and have SelectSettings check for *types* in SelectionCancellers
						onClick (hit.transform.GetComponent<MonoBehaviour> () as ISelectable);
					} else {
						onClick (null);
					}
				}
			}
		}
	}
}