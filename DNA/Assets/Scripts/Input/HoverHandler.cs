using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameInput {

	public class HoverManager {

		List<HoverHandler> hovers = new List<HoverHandler> ();

		public HoverManager (int[] layers) {
			for (int i = 0; i < layers.Length; i ++) {
				hovers.Add (new HoverHandler (layers[i]));
			}
		}

		public void HandleMouseOver () {
			for (int i = 0; i < hovers.Count; i ++) {
				hovers[i].HandleMouseOver ();
			}
		}
	}

	public class HoverHandler : MouseButtonHandler<IHoverable> {

		IHoverable hoveringOver = null;

		public HoverHandler (int layer) : base (true, layer) {}

		public void HandleMouseOver () {
			IHoverable newHover = GetMouseOver ();
			if (newHover == null) {
				ExitHover ();
			} else {
				if (newHover == hoveringOver) {
					hoveringOver.OnHover ();
				} else {
					ExitHover ();
					hoveringOver = newHover;
					hoveringOver.OnHoverEnter ();
				}
			}
		}

		void ExitHover () {
			if (hoveringOver != null) {
				hoveringOver.OnHoverExit ();
				hoveringOver = null;
			}
		}
	}
}