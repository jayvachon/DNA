using UnityEngine;
using System.Collections;
using GameInput;

namespace Units {

	// Inherit from this class to handle selection on click
	// rename to UnitCollider

	[RequireComponent (typeof (Collider))]
	public class UnitClickable : UnitComponent, IClickable, ISelectable {

		public virtual InputLayer[] IgnoreLayers {
			get { return new InputLayer[] { InputLayer.UI }; }
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