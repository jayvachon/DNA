using UnityEngine;
using System.Collections;
using GameInput;

namespace Units {

	// Inherit from this class to handle selection on click

	[RequireComponent (typeof (Collider))]
	public class UnitClickable : MonoBehaviour, IClickable, ISelectable {

		public Unit unit;

		public virtual void OnClick (ClickSettings clickSettings) {
			if (clickSettings.left) {
				SelectionManager.ToggleSelect (this);
			} else {
				SelectionManager.Unselect ();
			}
		}	
		
		public void OnSelect () {
			unit.OnSelect ();
		}

		public void OnUnselect () {
			unit.OnUnselect ();
		}
	}
}