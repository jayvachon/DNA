using UnityEngine;
using System.Collections;
using GameInventory;
using Units;

namespace GameActions {

	public class OccupyBed : InventoryAction<BedHolder> {

		public override string Name {
			get { return "OccupyBed"; }
		}

		/*public override bool CanPerform {
			get { return !occupying && AcceptorHolder != null && !AcceptorHolder.Full; }
		}*/

		Elder elder = null;
		Elder Elder {
			get {
				if (elder == null) {
					elder = Performer as Elder;
				}
				return elder;
			}
		}

		EnabledState enabledState;
		public override EnabledState EnabledState {
			get {
				if (enabledState == null) {
					enabledState = new OccupyBedEnabledState (Occupying);
				}
				return enabledState;
			}
		}

		float ElderDegradeRate {
			set { if (Elder != null) Elder.HealthManager.SetDegradeRate (value); }
		}

		public Clinic Clinic {
			// TODO: This is a really roundabout/unintuitive way of get the clinic
			get { return AcceptorHolder.Inventory.holder as Clinic; }
		}
		
		bool occupying = false;
		public bool Occupying { 
			get { return occupying; } 
			private set {
				occupying = value;
				OccupyBedEnabledState es = EnabledState as OccupyBedEnabledState;
				es.Occupying = occupying;
			}
		}

		BedItem bedItem;

		public OccupyBed () : base (0, false, false) {}

		public override void OnEnd () {
			bedItem = new BedItem (Performer);
			AcceptorHolder.Add (bedItem);
			ElderDegradeRate = AcceptorHolder.Quality;
			Occupying = true;
		}

		public void Remove () {
			if (Occupying) {
				ElderDegradeRate = 0;
				AcceptorHolder.Remove<BedItem> (bedItem);
				Occupying = false;
			}
		}
	}
}