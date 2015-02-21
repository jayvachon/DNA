using UnityEngine;
using System.Collections;

namespace Units {

	public class UnitTransform : MBRefs {

		public Vector3 Position {
			get { return MyTransform.position; }
			set { MyTransform.position = value; }
		}

		public virtual void OnSelect () {}
		public virtual void OnUnselect () {}
	}
}