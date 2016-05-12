using UnityEngine;
using System.Collections;

namespace DNA.Units {

	public class Turret : StaticUnit {
		
		float range = 10f;
		RangeRing ring;

		protected override void OnEnable () {
			base.OnEnable ();
			ring = RangeRing.Create (MyTransform);
			ring.Set (range, 40);
			ring.Hide ();
		}

		public override void OnSelect () {
			base.OnSelect ();
			ring.Show ();
		}

		public override void OnUnselect () {
			base.OnUnselect ();
			ring.Hide ();
		}
	}
}