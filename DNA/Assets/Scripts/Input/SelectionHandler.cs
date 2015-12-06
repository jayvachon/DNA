using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace DNA.InputSystem {

	public delegate void OnUpdateSelection (List<ISelectable> selectable);

	public static class SelectionHandler {

		public static OnUpdateSelection onUpdateSelection;

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

		static SelectionHandler () {
			EmptyClickHandler.Instance.onClick += OnEmptyClick;
		}

		public static void SelectSingle (ISelectable selectable) {
			UnselectAll ();
			Select (selectable);
		}

		public static void ClickSelectable (ISelectable selectable, PointerEventData e) {
			
			SelectSettings settings = selectable.SelectSettings;

			List<ISelectableOverrider> selectablesWithOverride = GetSelectablesWithOverride (e.button);
			if (selectablesWithOverride.Count > 0) {
				foreach (ISelectableOverrider sel in selectablesWithOverride)
					sel.OnOverrideSelect (selectable);
			} else {
				if (!settings.CanSelect)
					return;
				if (e.button == settings.SelectButton) {
					HandleSelect (selectable);
				} else if (e.button == settings.UnselectButton) {
					Unselect (selectable);
				}
			}
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
			SendUpdateSelectionMessage ();
		}

		static void Unselect (ISelectable selectable) {
			if (!MultiSelectModifier)
				UnselectAll ();
			selected.Remove (selectable);
			selectable.OnUnselect ();
			SendUpdateSelectionMessage ();			
		}

		static void UnselectAll () {
			foreach (ISelectable s in selected)
				s.OnUnselect ();
			selected.Clear ();
		}

		static bool IsSelected (ISelectable selectable) {
			return selected.Contains (selectable);
		}

		static void OnEmptyClick () {
			UnselectAll ();
			SendUpdateSelectionMessage ();
		}

		static List<ISelectableOverrider> GetSelectablesWithOverride (PointerEventData.InputButton button) {
			return selected.FindAll (x => x is ISelectableOverrider && ((ISelectableOverrider)x).OverrideButton == button)
				.ConvertAll (x => (ISelectableOverrider)x);
		}

		static void SendUpdateSelectionMessage () {
			if (onUpdateSelection != null)
				onUpdateSelection (selected);
		}
	}
}