using UnityEngine;
using System.Collections;
using DNA.Units;
using DNA.EventSystem;

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

		void OnEnable () { Events.instance.AddListener<PointerDownEvent> (OnPointerDownEvent); }
		void OnDisable () { Events.instance.RemoveListener<PointerDownEvent> (OnPointerDownEvent); }

		void Start () {
			if (onClick != null)
				onClick (null);
		}

		void OnPointerDownEvent (PointerDownEvent e) {
			if (onClick != null)
				onClick (e.ClickedObject.GetType ());

		}
	}
}