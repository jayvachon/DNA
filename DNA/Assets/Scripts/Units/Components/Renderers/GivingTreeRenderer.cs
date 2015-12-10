using UnityEngine;
using System.Collections;

namespace DNA.Units {

	public class GivingTreeRenderer : UnitRenderer {
		public override Vector3 Offset { get { return new Vector3 (0, 1f, 0); } }
		void Awake () { SetColors (new Color (0.808f, 0.945f, 0.604f)); }
	}
}