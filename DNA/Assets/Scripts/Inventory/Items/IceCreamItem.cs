using UnityEngine;
using System.Collections;

namespace GameInventory {
	
	public enum Flavor { Vanilla, Chocolate, Pistachio, Coffee }
	
	public class IceCreamItem : Item {

		Flavor flavor;
		public Flavor Flavor {
			get { return flavor; }
		}

		public IceCreamItem (Flavor flavor) {
			this.flavor = flavor;
		}
	}
}