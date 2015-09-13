using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DNA.Units {

	[RequireComponent (typeof (Renderer))]
	public class UnitRenderer : UnitComponent {

		public UnitColorHandler colorHandler = new UnitColorHandler ();

		protected override int ParentUnit { get { return 1; } }

		new void Awake () {
			colorHandler.Init (GetComponent<Renderer>());
			SetRenderersInChildren ();
		}

		public void OnSelect () {
			colorHandler.Selected = true;
			SetRenderersInChildren ();
		}

		public void OnUnselect () {
			colorHandler.Selected = false;
			SetRenderersInChildren ();
		}

		void SetRenderersInChildren () {
			List<Transform> children = MyTransform.GetAllChildren ();
			Renderer colorHandlerRenderer = colorHandler.Renderer;
			foreach (Transform child in children) {
				Renderer childRenderer = child.GetComponent<Renderer> ();
				if (childRenderer != null) {
					childRenderer.sharedMaterial = colorHandlerRenderer.sharedMaterial;
				}
			}
		}
	}
}