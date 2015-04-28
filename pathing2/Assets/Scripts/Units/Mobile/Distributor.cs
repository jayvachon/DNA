using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameInventory;
using GameActions;
using Pathing;
using GameInput;

namespace Units {

	// TODO: Rename to Laborer
	public class Distributor : MobileUnit {

		public override string Name { 
			get { return "Laborer"; }
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
			Inventory.Add (new MilkshakeHolder (3, 0));
			Inventory.Get<HappinessHolder> ().DisplaySettings = new ItemHolderDisplaySettings (true, true);

			PerformableActions = new PerformableActions (this);
			PerformableActions.Add ("CollectMilkshake", new CollectItem<MilkshakeHolder> ());
			PerformableActions.Add ("DeliverMilkshake", new DeliverItem<MilkshakeHolder> ());
			PerformableActions.Add ("CollectCoffee", new CollectItem<CoffeeHolder> ());
			PerformableActions.Add ("DeliverCoffee", new DeliverItem<CoffeeHolder> ());
			PerformableActions.Add ("CollectHappiness", new CollectHappiness ());
			PerformableActions.Add ("ConsumeHappiness", new ConsumeItem<HappinessHolder> (-1, true, true, false));
			PerformableActions.Add ("GenerateYear", new GenerateItem<YearHolder> ());
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
			Elder elder = u as Elder;
			elder.AverageHappiness = Inventory.Get<HappinessHolder> ().Average;
		}

		public override void OnPoolCreate () {
			Inventory.Empty ();
			Inventory.Get<HappinessHolder> ().Initialize (100);
			RetirementTimer.BeginAging (OnAge, OnRetirement);
			Path.Active = true;
			UnitInfoContent.Refresh ();
		}
	}
}