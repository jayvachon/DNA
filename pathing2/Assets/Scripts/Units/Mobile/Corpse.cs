using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;
using GameInput;

namespace Units {

	public class Corpse : MobileUnit, IActionPerformer {

		public override string Name { get { return "Remains"; } }

		Clinic boundClinic = null;

		void Awake () {

			Inventory = new Inventory (this);
			YearHolder yearHolder = new YearHolder (500, 0);
			yearHolder.HolderEmptied += OnDeliverYears;
			yearHolder.DisplaySettings = new ItemHolderDisplaySettings (true, false);
			Inventory.Add (yearHolder);

			PerformableActions = new PerformableActions (this);
			PerformableActions.Add (new DeliverElder ());
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

		/*public override void OnRelease () {
			// PerformableActions.Enable ("DeliverYear"); // TODO: shouldn't have to do this here -> straighten out how enabling/disabling works w/ actions!
			UnitClickable clickable = MobileClickable.Colliding (1 << (int)InputLayer.StaticUnits);
			if (clickable != null) {
				OnBindActionable (clickable.StaticUnit as IActionAcceptor);
				if (clickable.StaticUnit is GivingTreeUnit) {
					MobileClickable.CanDrag = false;
					MobileClickable.CanSelect = false;
				}
			} else {
				occupyBed.Remove ();
			}
		}*/

		protected override void OnBind () {
			UnbindClinic ();
			Clinic clinic = BoundAcceptor as Clinic;
			if (clinic != null) {
				boundClinic = clinic;
				PerformableActions.SetActive ("DeliverElder", false);
			}
		}

		protected override void OnUnbind () {
			UnbindClinic ();
		}

		void UnbindClinic () {
			if (boundClinic != null) {
				boundClinic.Inventory.RemoveItem<ElderHolder> ();
				PerformableActions.SetActive ("DeliverElder", true);
				boundClinic = null;
			}
		}

		void OnDeliverYears () {
			ObjectCreator.Instance.Destroy<Corpse> (transform);
		}
	}
}