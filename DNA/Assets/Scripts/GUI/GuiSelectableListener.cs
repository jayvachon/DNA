using UnityEngine;
using System.Collections;
using DNA.InputSystem;

// TODO: have inventory, tasks, name/description inherit from this
public class GuiSelectableListener : MonoBehaviour {

	void Init () {
		SelectionHandler.onUpdateSelection += OnUpdateSelection;
	}

	protected virtual void OnUpdateSelection (ISelectable selectable) {

	}
}
