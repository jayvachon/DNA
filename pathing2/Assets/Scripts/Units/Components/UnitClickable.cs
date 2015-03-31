using UnityEngine;
using System.Collections;
using GameInput;

namespace Units {

	// Inherit from this class to handle selection on click
	// rename to UnitCollider

	[RequireComponent (typeof (Collider))]
	public class UnitClickable : MBRefs, IClickable, ISelectable {

		Unit unit = null;
		public Unit Unit {
			get {
				if (unit == null) {
					unit = transform.GetNthParent (1).GetScript<Unit> ();
				} 
				return unit;
			}
		}

		public virtual void OnClick (ClickSettings clickSettings) {
			if (clickSettings.left) {
				SelectionManager.ToggleSelect (this);
			} else {
				SelectionManager.Unselect ();
			}
		}	
		
		public void OnSelect () {
			Unit.OnSelect ();
		}

		public void OnUnselect () {
			Unit.OnUnselect ();
		}
	}
}