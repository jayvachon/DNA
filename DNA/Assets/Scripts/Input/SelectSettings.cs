using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace DNA.InputSystem {

	public class SelectSettings {

		public readonly List<System.Type> SelectionCancellers = null;
		public bool CanSelect = true;
		public readonly bool AllowToggle = true;
		public readonly PointerEventData.InputButton SelectButton = PointerEventData.InputButton.Left;
		public readonly PointerEventData.InputButton UnselectButton = PointerEventData.InputButton.Right;

		public SelectSettings (
			List<System.Type> selectionCancellers=null,
			bool canSelect=true,
			bool allowToggle=true,
			PointerEventData.InputButton selectButton=PointerEventData.InputButton.Left, 
			PointerEventData.InputButton unselectButton=PointerEventData.InputButton.Right) {

			SelectionCancellers = selectionCancellers;
			CanSelect = canSelect;
			AllowToggle = allowToggle;
			SelectButton = selectButton;
			UnselectButton = unselectButton;
		}
		
		public bool HasSelectionCanceller (System.Type type) {
			return (SelectionCancellers != null && SelectionCancellers.Contains (type));
		}
	}
}