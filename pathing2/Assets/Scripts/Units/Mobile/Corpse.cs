using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;
using GameInput;

namespace Units {

	public class Corpse : MobileUnit, IActionPerformer {

		public override string Name { get { return "Remains"; } }

		OccupyBed occupyBed = new OccupyBed ();

		void Awake () {

			Inventory = new Inventory (this);
			YearHolder yearHolder = new YearHolder (500, 0);
			yearHolder.HolderEmptied += OnDeliverYears;
			yearHolder.DisplaySettings = new ItemHolderDisplaySettings (true, false);
			Inventory.Add (yearHolder);

			PerformableActions = new PerformableActions (this);
			PerformableActions.Add (occupyBed);
			PerformableActions.Add (new DeliverItem<YearHolder> ());
			PerformableActions.Add (new ConsumeItem<YearHolder> ());
		}

		void Start () {
			Path.Active = false;
		}

		public override void OnPoolCreate () {
			MobileClickable.CanDrag = true;
			MobileClickable.CanSelect = true;
		}

		public override void OnRelease () {
			// PerformableActions.Enable ("DeliverYear"); // TODO: shouldn't have to do this here -> straighten out how enabling/disabling works w/ actions!
			UnitClickable clickable = MobileClickable.Colliding (1 << (int)InputLayer.StaticUnits).GetScript<UnitClickable> ();
			if (clickable != null) {
				OnBindActionable (clickable.StaticUnit as IActionAcceptor);
				if (clickable.StaticUnit is GivingTreeUnit) {
					MobileClickable.CanDrag = false;
					MobileClickable.CanSelect = false;
				}
			} else {
				occupyBed.Remove ();
			}
		}

		void OnDeliverYears () {
			ObjectCreator.Instance.Destroy<Corpse> (transform);
		}
	}
}