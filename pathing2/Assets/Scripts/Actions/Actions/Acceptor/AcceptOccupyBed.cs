using UnityEngine;
using System.Collections;
using GameInventory;

namespace GameActions {

	public class AcceptOccupyBed : AcceptInventoryAction<BedHolder> {

		public override bool Enabled {
			get { return !Holder.Full; }
		}

		public AcceptOccupyBed () : base (null) {}			
	}
}
