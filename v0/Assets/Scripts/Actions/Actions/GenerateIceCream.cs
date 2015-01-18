using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class GenerateIceCream : Action {

		public override string Name {
			get { return "Generate Ice Cream"; }
		}

		IceCreamHolder iceCreamHolder;

		public GenerateIceCream (IceCreamHolder iceCreamHolder, float duration=5) : base (duration, true, true) {
			this.iceCreamHolder = iceCreamHolder;
		}

		public override void End () {
			iceCreamHolder.Add (new IceCreamItem (Flavor.Vanilla));
			base.End ();
		}
	}
}