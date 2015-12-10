using UnityEngine;
using System.Collections;

namespace DNA.Units {

	public class CoffeePlantRenderer : UnitRenderer {
		public override Vector3 Offset { get { return new Vector3 (0, 0.5f, 0); } }
		void Awake () { SetColors (new Color (0.204f, 0.612f, 0.325f)); }
	}
}