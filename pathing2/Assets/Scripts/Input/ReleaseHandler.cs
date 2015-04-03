using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameEvents;

namespace GameInput {

	public class ReleaseManager {

		protected const int LEFT = 0;
		protected const int RIGHT = 1;
		List<ReleaseHandler> releases = new List<ReleaseHandler> ();

		public ReleaseManager (int[] layers) {
			for (int i = 0; i < layers.Length; i ++) {
				releases.Add (new ReleaseHandler (true, layers[i]));
				releases.Add (new ReleaseHandler (false, layers[i]));
			}
		}

		public void HandleMouseDown (int mouseButton) {
			for (int i = mouseButton; i < releases.Count; i += 2) {
				releases[i].HandleMouseDown ();
			}
		}

		public void HandleMouseUp (int mouseButton) {
			for (int i = mouseButton; i < releases.Count; i += 2) {
				releases[i].HandleMouseUp ();
			}
		}
	}

	public class ReleaseHandler : MouseButtonHandler<IReleasable> {

		public ReleaseHandler (bool left, int layer) : base (left, layer) {}

		protected override void OnUp () {
			IReleasable released = GetMouseOver ();
			bool clicked = false;
			if (released != null) {
				clicked = (released == Moused);
			}
			ReleaseSettings releaseSettings = new ReleaseSettings (left, clicked);
			
			// "released" is what the mouse is currently over...
			if (released != null) {
				released.OnRelease (releaseSettings);
			}

			// ..."Moused" is what the mouse originally clicked
			if (Moused != null) {
				Moused.OnRelease (releaseSettings);
			}
			Events.instance.Raise (new ReleaseEvent (releaseSettings));
		}
	}

	public class ReleaseSettings {

		public readonly bool left;		// if left mouse button was pressed
		public readonly bool clicked;	// if button was released on the same thing it clicked

		public ReleaseSettings (bool left, bool clicked) {
			this.left = left;
			this.clicked = clicked;
		}
	}
}