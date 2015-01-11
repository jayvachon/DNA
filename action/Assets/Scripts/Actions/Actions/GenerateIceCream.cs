using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class GenerateIceCream : Action {

		IceCreamHolder iceCreamHolder;

		public GenerateIceCream (IceCreamHolder iceCreamHolder, float duration=5) : base (duration, true, true) {
			this.iceCreamHolder = iceCreamHolder;
		}

		public override void Start () {
			base.Start ();
		}

		public override void End () {
			iceCreamHolder.Add (new IceCreamItem (Flavor.Vanilla));
			base.End ();
		}
	}
}