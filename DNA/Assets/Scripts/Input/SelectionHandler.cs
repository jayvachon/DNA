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

		static SelectionHandler () {
			EmptyClickHandler.Instance.onClick += OnEmptyClick;
		}

		public static void ClickSelectable (ISelectable selectable, PointerEventData e) {
			SelectSettings settings = selectable.SelectSettings;
			if (!settings.CanSelect)
				return;

			List<ISelectableOverrider> selectablesWithOverride = GetSelectablesWithOverride (e.button);
			if (selectablesWithOverride.Count > 0) {
				foreach (ISelectableOverrider sel in selectablesWithOverride)
					sel.OnOverrideSelect (selectable);
			} else {
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

		static void OnEmptyClick () {
			UnselectAll ();
		}

		static List<ISelectableOverrider> GetSelectablesWithOverride (PointerEventData.InputButton button) {
			return selected.FindAll (x => x is ISelectableOverrider && ((ISelectableOverrider)x).OverrideButton == button)
				.ConvertAll (x => (ISelectableOverrider)x);
		}
	}

	public class EmptyClickHandler : MonoBehaviour {

		static EmptyClickHandler instance = null;
		static public EmptyClickHandler Instance {
			get {
				if (instance == null) {
					instance = Object.FindObjectOfType (typeof (EmptyClickHandler)) as EmptyClickHandler;
					if (instance == null) {
						GameObject go = new GameObject ("EmptyClickHandler");
						DontDestroyOnLoad (go);
						instance = go.AddComponent<EmptyClickHandler>();
					}
				}
				return instance;
			}
		}

		public delegate void OnClick ();

		public OnClick onClick;

		void Update () {
			if (Input.GetMouseButtonDown (0) || Input.GetMouseButtonDown (1)) {
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (!Physics.Raycast (ray, out hit, Mathf.Infinity)) {
					if (onClick != null)
						onClick ();
				}
			}
		}
	}
}