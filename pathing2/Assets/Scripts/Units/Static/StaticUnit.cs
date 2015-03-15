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
	}
}