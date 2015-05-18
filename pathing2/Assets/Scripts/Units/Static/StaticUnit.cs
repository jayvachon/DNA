using UnityEngine;
using System.Collections;
using GameInput;
using Pathing;
using GameActions;

namespace Units {

	public class StaticUnit : Unit, IActionAcceptor {

		StaticUnitTransform staticTransform;
		public StaticUnitTransform StaticTransform {
			get {
				if (staticTransform == null) {
					staticTransform = unitTransform as StaticUnitTransform;
				}
				return staticTransform;
			}
		}
		
		public virtual bool PathPointEnabled {
			get { return true; }
		}

		public PathPoint PathPoint { get; set; }
		public AcceptableActions AcceptableActions { get; protected set; }
	}
}