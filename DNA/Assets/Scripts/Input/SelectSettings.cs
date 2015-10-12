using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace DNA.InputSystem {

	public class SelectSettings {

		public bool CanSelect = true;
		public readonly bool AllowToggle = true;
		public readonly PointerEventData.InputButton SelectButton = PointerEventData.InputButton.Left;
		public readonly PointerEventData.InputButton UnselectButton = PointerEventData.InputButton.Right;

		public SelectSettings (
			bool canSelect=true,
			bool allowToggle=true,
			PointerEventData.InputButton selectButton=PointerEventData.InputButton.Left, 
			PointerEventData.InputButton unselectButton=PointerEventData.InputButton.Right) {

			CanSelect = canSelect;
			AllowToggle = allowToggle;
			SelectButton = selectButton;
			UnselectButton = unselectButton;
		}
	}
}