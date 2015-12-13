using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.InputSystem;

public class GuiSelectableListener : MonoBehaviour {

	public GameObject uiGroup;

	protected void Init () {
		SelectionHandler.onUpdateSelection += OnUpdateSelection;
		OnUpdateSelection (new List<ISelectable> ());
	}

	protected virtual void OnUpdateSelection (List<ISelectable> selected) {}

	protected void SetGroupActive (bool active) {
		uiGroup.gameObject.SetActive (active);
	}
}
