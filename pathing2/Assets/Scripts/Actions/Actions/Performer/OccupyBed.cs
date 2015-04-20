using UnityEngine;
using System.Collections;
using GameInventory;
using Units;

namespace GameActions {

	public class OccupyBed : InventoryAction<BedHolder> {

		public override string Name {
			get { return "OccupyBed"; }
		}

		public override bool CanPerform {
			get { return !occupying && AcceptorHolder != null && !AcceptorHolder.Full; }
		}

		Elder elder = null;
		Elder Elder {
			get {
				if (elder == null) {
					elder = Performer as Elder;
				}
				return elder;
			}
		}
		
		bool occupying = false;

		BedItem bedItem;

		public OccupyBed () : base (0, false, false, null) {}

		public override void OnEnd () {
			bedItem = new BedItem ();
			AcceptorHolder.Add (bedItem);
			Elder.HealthManager.SetDegradeRate (AcceptorHolder.Quality);
			occupying = true;
		}

		public void Remove () {
			if (occupying)
				AcceptorHolder.Remove<BedItem> (bedItem);
		}
	}
}