using UnityEngine;
using System.Collections;

namespace DNA.Units {

	public class UniversityRenderer : UnitRenderer {
		public override Vector3 Offset { get { return new Vector3 (0, 0.5f, 0); } }
		void Awake () { SetColors (new Color (0.831f, 0.231f, 1f)); }
	}
}