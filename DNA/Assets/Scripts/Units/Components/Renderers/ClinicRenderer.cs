using UnityEngine;
using System.Collections;

namespace DNA.Units {

	public class ClinicRenderer : UnitRenderer {
		public override Vector3 Offset { get { return new Vector3 (0, 0.5f, 0); } }
		void Awake () { SetColors (new Color (1f, 0.898f, 0.231f)); }
	}
}