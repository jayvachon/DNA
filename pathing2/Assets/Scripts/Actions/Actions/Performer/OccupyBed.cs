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

		public Clinic Clinic {
			// TODO: This is a really roundabout/unintuitive way of get the clinic
			get { return AcceptorHolder.Inventory.holder as Clinic; }
		}
		
		bool occupying = false;
		public bool Occupying { get { return occupying; } }

		BedItem bedItem;

		public OccupyBed () : base (0, false, false, null) {}

		public override void OnEnd () {
			bedItem = new BedItem (Performer);
			AcceptorHolder.Add (bedItem);
			if (Elder != null) Elder.HealthManager.SetDegradeRate (AcceptorHolder.Quality);
			occupying = true;
		}

		public void Remove () {
			if (occupying) {
				AcceptorHolder.Remove<BedItem> (bedItem);
				Debug.Log (AcceptorHolder.Inventory.Get<BedHolder> ().Count);
				occupying = false;
			}
		}
	}
}