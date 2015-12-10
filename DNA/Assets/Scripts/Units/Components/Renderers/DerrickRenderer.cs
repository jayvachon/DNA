using UnityEngine;
using System.Collections;

namespace DNA.Units {

	public class DerrickRenderer : UnitRenderer {
		public override Vector3 Offset { get { return new Vector3 (0, 0.5f, 0); } }
		void Awake () { SetColors (new Color (0.294f, 0.741f, 0.847f)); }
	}
}