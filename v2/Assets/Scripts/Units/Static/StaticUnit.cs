using UnityEngine;
using System.Collections;
using GameInput;
using Pathing;

namespace Units {

	public class StaticUnit : Unit {

		StaticUnitTransform staticTransform;
		protected StaticUnitTransform StaticTransform {
			get {
				if (staticTransform == null) {
					staticTransform = unitTransform as StaticUnitTransform;
				}
				return staticTransform;
			}
		}

		public void OnDragEnter (DragSettings dragSettings) {
			StaticTransform.OnDragEnter (dragSettings);
		}

		public void OnDragExit (DragSettings dragSettings) {
			StaticTransform.OnDragExit (dragSettings);
		}
	}
}