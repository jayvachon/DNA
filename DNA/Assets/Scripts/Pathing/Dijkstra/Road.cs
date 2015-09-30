using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.InputSystem;

namespace DNA.Paths {

	public class Road : MBRefs, IPoolable {

		new Renderer renderer = null;
		Renderer Renderer {
			get {
				if (renderer == null) {
					renderer = GetComponent<Renderer> ();
				}
				return renderer;
			}
		}

		bool CanHighlight {
			get { return (!built && SelectionManager.NoneSelected); }
		}

		bool built = false;

		void OnEnable () {
			SetVisible (false);
		}

		public void SetRendererScale (float length) {
			MyTransform.localScale = new Vector3 (0.1f, 0.1f, length);
			MyTransform.SetLocalPositionZ (length*0.5f);
		}

		public void Build () {
			built = true;
			SetVisible (true);
		}

		void SetVisible (bool enabled) {
			Renderer.enabled = enabled;
		}

		public void OnPoolCreate () {}
		public void OnPoolDestroy () {}
	}
}