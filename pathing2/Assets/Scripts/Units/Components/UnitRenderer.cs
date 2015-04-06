using UnityEngine;
using System.Collections;

namespace Units {

	[RequireComponent (typeof (Renderer))]
	public class UnitRenderer : UnitComponent {

		public UnitColorHandler colorHandler = new UnitColorHandler ();

		protected override int ParentUnit { get { return 1; } }

		new void Awake () {
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