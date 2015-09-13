using UnityEngine;
using System.Collections;
using DNA.InputSystem;

namespace DNA.Units {

	// Inherit from this class to handle selection on click
	// rename to UnitCollider

	[RequireComponent (typeof (Collider))]
	public class UnitClickable : UnitComponent, IClickable, ISelectable {

		protected override int ParentUnit { get { return 1; } }
		
		bool canSelect = true;
		public bool CanSelect { 
			get { return canSelect; }
			set { canSelect = value; }
		}

		public virtual InputLayer[] IgnoreLayers {
			get { return new InputLayer[] { InputLayer.UI }; }
		}

		public virtual void OnClick (ClickSettings clickSettings) {
			if (!CanSelect) return;
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