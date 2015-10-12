using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using DNA.InputSystem;

namespace DNA.Units {

	// Inherit from this class to handle selection on click
	// rename to UnitCollider

	[RequireComponent (typeof (Collider))]
	public class UnitClickable : UnitComponent, IPointerDownHandler, ISelectable {

		SelectSettings selectSettings;
		public SelectSettings SelectSettings {
			get { 
				if (selectSettings == null) {
					selectSettings = new SelectSettings ();
				}
				return selectSettings;
			}
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

		void OnEnable () {
			gameObject.SetActive (false);
		}

		#region IPointerDownHandler implementation
		public virtual void OnPointerDown (PointerEventData e) {
			SelectionHandler.ClickSelectable (this, e);
		}
		#endregion
		
		public void OnSelect () {
			Unit.OnSelect ();
		}

		public void OnUnselect () {
			Unit.OnUnselect ();
		}
	}
}