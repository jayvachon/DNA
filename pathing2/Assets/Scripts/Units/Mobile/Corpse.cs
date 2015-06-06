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
			PerformableActions.Add (new DeliverUnpairedItem<ElderHolder> ());
			PerformableActions.Add (new DeliverAllYears ());
			PerformableActions.Add (new ConsumeItem<YearHolder> ());
		}

		void Start () {
			Path.Active = false;
		}

		public override void OnPoolCreate () {
			MobileClickable.CanDrag = true;
			MobileClickable.CanSelect = true;
			NotificationCenter.Instance.ShowNotification ("elderDied");
		}

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