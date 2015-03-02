using UnityEngine;
using System.Collections;

namespace Units {

	public class UnitTransform : MBRefs {

		Unit unit = null;
		public Unit Unit {
			get {
				if (unit == null) {
					unit = transform.GetFirstParent ().GetScript<Unit> ();
				} 
				return unit;
			}
		}

		public Vector3 Position {
			get { return MyTransform.position; }
			set { MyTransform.position = value; }
		}

		public virtual void OnSelect () {}
		public virtual void OnUnselect () {}
	}
}