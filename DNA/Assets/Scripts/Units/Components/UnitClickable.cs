using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using DNA.InputSystem;

namespace DNA.Units {

	// Inherit from this class to handle selection on click
	// rename to UnitCollider

	[RequireComponent (typeof (Collider))]
	public class UnitClickable : UnitComponent, IPointerDownHandler, ISelectable {//, IClickable, ISelectable {

		public SelectSettings SelectSettings {
			get { return new SelectSettings (false); }
		}

		protected override int ParentUnit { get { return 1; } }
		
		bool canSelect = true;
		public bool CanSelect { 
			get { return canSelect; }
			set { canSelect = value; }
		}

		public virtual InputLayer[] IgnoreLayers {
			get { return new InputLayer[] { InputLayer.UI }; }
		}

		#region IPointerDownHandler implementation
		public virtual void OnPointerDown (PointerEventData e) {
			SelectionHandler.ClickSelectable (this, e);
			/*if (!CanSelect) return;
			if (e.button == PointerEventData.InputButton.Left) {
				SelectionManager.ToggleSelect (this);
			} else {
				SelectionManager.Unselect ();
			}*/
		}
		#endregion

		/*public virtual void OnClick (ClickSettings clickSettings) {
			if (!CanSelect) return;
			if (clickSettings.left) {
				SelectionManager.ToggleSelect (this);
			} else {
				SelectionManager.Unselect ();
			}
		}*/
		
		public void OnSelect () {
			Unit.OnSelect ();
		}

		public void OnUnselect () {
			Unit.OnUnselect ();
		}
	}
}