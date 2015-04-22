using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameInventory;
using GameActions;
using Pathing;
using GameInput;

namespace Units {

	// TODO: Rename to Worker
	public class Distributor : MobileUnit {

		public override string Name { 
			get { return "Worker"; }
		}

		//RetirementTimer retirementTimer = new RetirementTimer ();
		RetirementTimer retirementTimer = null;
		RetirementTimer RetirementTimer {
			get {
				if (retirementTimer == null) {
					retirementTimer = new RetirementTimer ();
				}
				return retirementTimer;
			}
		}

		void Awake () {

			Inventory = new Inventory (this);
			Inventory.Add (new YearHolder (65, 0));
			Inventory.Add (new HappinessHolder (100, 100));
			Inventory.Add (new CoffeeHolder (5, 0));
			// Inventory.Add (new MilkHolder (5, 0));
			// Inventory.Add (new IceCreamHolder (3, 0));
			Inventory.Add (new MilkshakeHolder (3, 0));
			// Inventory.Add (new ElderHolder (2, 0));

			PerformableActions = new PerformableActions (this);
			// PerformableActions.Add ("CollectMilk", new CollectItem<MilkHolder> (TimerValues.ActionTimes["CollectMilk"]));
			// PerformableActions.Add ("DeliverMilk", new DeliverItem<MilkHolder> (TimerValues.ActionTimes["DeliverMilk"]));
			// PerformableActions.Add ("CollectIceCream", new CollectItem<IceCreamHolder> (TimerValues.ActionTimes["DeliverIceCream"]));
			// PerformableActions.Add ("DeliverIceCream", new DeliverItem<IceCreamHolder> (TimerValues.ActionTimes["DeliverIceCream"]));
			PerformableActions.Add ("CollectMilkshake", new CollectItem<MilkshakeHolder> (TimerValues.ActionTimes["CollectMilkshake"]));
			PerformableActions.Add ("DeliverMilkshake", new DeliverItem<MilkshakeHolder> (TimerValues.ActionTimes["DeliverMilkshake"]));
			PerformableActions.Add ("CollectCoffee", new CollectItem<CoffeeHolder> (TimerValues.ActionTimes["CollectCoffee"]));
			PerformableActions.Add ("DeliverCoffee", new DeliverItem<CoffeeHolder> (TimerValues.ActionTimes["DeliverCoffee"]));
			// PerformableActions.Add ("CollectElder", new CollectItem<ElderHolder> (2));
			// PerformableActions.Add ("DeliverElder", new DeliverItem<ElderHolder> (2));
			PerformableActions.Add ("CollectHappiness", new CollectHappiness (TimerValues.ActionTimes["CollectHappiness"]));
			PerformableActions.Add ("ConsumeHappiness", new ConsumeItem<HappinessHolder> (TimerValues.ActionTimes["ConsumeHappiness"], true, true, false));
			PerformableActions.Add ("GenerateYear", new GenerateItem<YearHolder> (RetirementTimer.RetirementAge / 65f));
			PerformableActions.DisableAll ();
		}

		void OnAge (float progress) {
			float p = Mathf.Clamp01 (Mathf.Abs (progress - 1));
			Path.Speed = Path.PathSettings.maxSpeed * Mathf.Sqrt(-(p - 2) * p);
		}

		void OnRetirement () {
			ChangeUnit<Distributor, Elder> ();
		}

		protected override void OnChangeUnit<U> (U u) {
			Path.Active = false;
		}

		public override void OnPoolCreate () {
			Inventory.Empty ();
			Inventory.Get<HappinessHolder> ().AddInitial (100);
			RetirementTimer.BeginAging (OnAge, OnRetirement);
			Path.Active = true;
			UnitInfoContent.Refresh ();
		}
	}
}