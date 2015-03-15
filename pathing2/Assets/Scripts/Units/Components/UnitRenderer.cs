using UnityEngine;
using System.Collections;

namespace Units {

	[RequireComponent (typeof (Renderer))]
	public class UnitRenderer : MonoBehaviour {

		public UnitColorHandler colorHandler = new UnitColorHandler ();

		void Awake () {
			colorHandler.Init (renderer);
		}

		public void OnSelect () {
			colorHandler.Selected = true;
		}

		public void OnUnselect () {
			colorHandler.Selected = false;
		}
	}
}