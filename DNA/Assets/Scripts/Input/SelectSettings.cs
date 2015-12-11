using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace DNA.InputSystem {

	public class SelectSettings {

		public bool CanSelect = true;
		public readonly bool AllowToggle = true;
		public readonly List<ISelectable> SelectionCancellers = null;
		public readonly PointerEventData.InputButton SelectButton = PointerEventData.InputButton.Left;
		public readonly PointerEventData.InputButton UnselectButton = PointerEventData.InputButton.Right;

		public SelectSettings (
			bool canSelect=true,
			bool allowToggle=true,
			List<ISelectable> selectionCancellers=null,
			PointerEventData.InputButton selectButton=PointerEventData.InputButton.Left, 
			PointerEventData.InputButton unselectButton=PointerEventData.InputButton.Right) {

			CanSelect = canSelect;
			AllowToggle = allowToggle;
			SelectionCancellers = selectionCancellers;
			SelectButton = selectButton;
			UnselectButton = unselectButton;
		}
	}
}