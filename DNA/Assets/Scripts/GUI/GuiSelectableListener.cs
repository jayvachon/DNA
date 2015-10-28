using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.InputSystem;

// TODO: have inventory, tasks, name/description inherit from this
public class GuiSelectableListener : MonoBehaviour {

	protected void Init () {
		SelectionHandler.onUpdateSelection += OnUpdateSelection;
	}

	protected virtual void OnUpdateSelection (List<ISelectable> selected) {}
}
