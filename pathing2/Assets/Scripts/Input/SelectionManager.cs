using UnityEngine;
using System.Collections;
using GameEvents;

namespace GameInput {
	
	public static class SelectionManager {

		static ISelectable selected = null;
		public static ISelectable Selected {
			get { return selected; }
			set {
				if (IsSelected (value))
					return;
				if (!NoneSelected) {
					// TODO: This also calls the next block
					selected.OnUnselect ();
				} 
				if (value == null) {
					Events.instance.RemoveListener<ClickEvent> (OnClickEvent);
					selected.OnUnselect ();
					selected = value;
				} else {
					Events.instance.AddListener<ClickEvent> (OnClickEvent);
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
			Events.instance.Raise (new SelectEvent (selectable));
		}

		public static void Unselect () {
			Selected = null;
			Events.instance.Raise (new UnselectEvent ());
		}

		public static void ToggleSelect (ISelectable selectable) {
			if (IsSelected (selectable)) {
				Unselect ();
			} else {
				Select (selectable);
			}
		}

		public static bool IsSelected (ISelectable s) {
			return s == selected;
		}

		static void OnClickEvent (ClickEvent e) {
			if (e.LayerClicked (InputLayer.UI))
				return;
			if (!e.LayerClicked (InputLayer.Units)) {
				Unselect ();
			}
		}
	}
}