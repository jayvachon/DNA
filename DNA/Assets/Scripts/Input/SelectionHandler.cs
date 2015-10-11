using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace DNA.InputSystem {

	public static class SelectionHandler {

		static List<ISelectable> selected = new List<ISelectable> ();

		static KeyCode[] multiSelect = new [] { 
			KeyCode.LeftControl, 
			KeyCode.RightControl, 
			KeyCode.LeftCommand, 
			KeyCode.RightCommand
		};

		static bool MultiSelectModifier {
			get {
				foreach (KeyCode key in multiSelect) {
					if (Input.GetKey (key))
						return true;
				}
				return false;
			}
		}

		public static void ClickSelectable (ISelectable selectable, PointerEventData e) {
			if (e.button == selectable.settings.SelectButton)
				HandleSelect (selectable);
		}

		static void HandleSelect (ISelectable selectable) {
			if (selectable.SelectSettings.AllowToggle) {
				ToggleSelect (selectable);
			} else {
				if (!IsSelected (selectable))
					Select (selectable);
			}
		}

		static void ToggleSelect (ISelectable selectable) {
			if (IsSelected (selectable)) {
				Unselect (selectable);
			} else {
				Select (selectable);
			}
		}

		static void Select (ISelectable selectable) {
			if (!MultiSelectModifier)
				UnselectAll ();
			selected.Add (selectable);
			selectable.OnSelect ();
		}

		static void Unselect (ISelectable selectable) {
			if (!MultiSelectModifier)
				UnselectAll ();
			selected.Remove (selectable);
			selectable.OnUnselect ();
		}

		static void UnselectAll () {
			foreach (ISelectable s in selected)
				s.OnUnselect ();
			selected.Clear ();
		}

		static bool IsSelected (ISelectable selectable) {
			return selected.Contains (selectable);
		}
	}
}