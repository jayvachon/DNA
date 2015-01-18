using UnityEngine;
using System.Collections;

namespace GameInput {
	
	public static class SelectionManager {

		static ISelectable selected = null;
		public static ISelectable Selected {
			get { return selected; }
			set {
				if (IsSelected (value))
					return;
				if (!NoneSelected) {
					selected.OnUnselect ();
				}
				if (value == null) {
					selected.OnUnselect ();
					selected = value;
				} else {
					selected = value;
					selected.OnSelect ();
				}
			}
		}

		public static bool NoneSelected {
			get { return selected == null; }
		}

		public static void Select (ISelectable selectable) {
			Selected = selectable;
		}

		public static void Unselect () {
			Selected = null;
		}

		public static void ToggleSelect (ISelectable selectable) {
			if (IsSelected (selectable)) {
				Selected = null;
			} else {
				Selected = selectable;
			}
		}

		public static bool IsSelected (ISelectable s) {
			return s == selected;
		}
	}
}