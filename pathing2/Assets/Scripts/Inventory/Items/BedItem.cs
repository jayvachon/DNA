using UnityEngine;
using System.Collections;
using GameActions;
using Units;

namespace GameInventory {

	public class BedItem : Item {

		public bool IsElder {
			get { return performer is Elder; }
		}

		public bool IsCorpse {
			get { return performer is Corpse; }
		}

		IActionPerformer performer;

		public BedItem (IActionPerformer performer) {
			this.performer = performer;
		}
	}
}