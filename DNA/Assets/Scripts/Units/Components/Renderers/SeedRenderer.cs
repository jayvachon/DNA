using UnityEngine;
using System.Collections;

namespace DNA.Units {

	public class SeedRenderer : UnitRenderer {
		public override Vector3 Offset { get { return Vector3.zero; } }
		void Awake () { SetColors (Palette.Yellow); }
	}
}