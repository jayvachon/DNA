using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.InputSystem;

namespace DNA.Paths {

	public class Road : MBRefs, IPathElementObject {

		public void Init (float length) {
			MyTransform.localScale = new Vector3 (0.1f, 0.1f, length);
			MyTransform.SetLocalPositionZ (length*0.5f);
		}
	}
}