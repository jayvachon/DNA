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
			Inventory.Add (new YearHolder (500, 0));
			Inventory.Get<YearHolder> ().HolderEmptied += OnDeliverYears;

			PerformableActions = new PerformableActions (this);
			PerformableActions.Add ("OccupyBed", occupyBed);
			PerformableActions.Add ("DeliverYear", new DeliverItem<YearHolder> ());
		}

		void Start () {
			Path.Active = false;
		}

		public override void OnPoolCreate () {
			Inventory.AddItems<YearHolder> (100); // temp
			MobileClickable.CanDrag = true;
			MobileClickable.CanSelect = true;
		}

		public override void OnRelease () {
			PerformableActions.Enable ("DeliverYear"); // TODO: shouldn't have to do this here -> straighten out how enabling/disabling works w/ actions!
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