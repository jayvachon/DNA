using UnityEngine;
using System.Collections;
using DNA.Units;

namespace DNA.InventorySystem {

	public class BedItem : Item {

		public bool IsElder {
			get { return false; } //return performer is Elder; }
		}

		public bool IsCorpse {
			get { return false; } //return performer is Corpse; }
		}

		//IActionPerformer performer;

		public BedItem (/*IActionPerformer performer*/) {
			//this.performer = performer;
		}
	}
}