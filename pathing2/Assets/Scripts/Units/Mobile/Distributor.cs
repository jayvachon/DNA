using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameInventory;
using GameActions;
using Pathing;

namespace Units {

	public class Distributor : MobileUnit {

		string workerName = "Distributor";
		string retirementName = "Elder";
		new string name = "Distributor";
		public override string Name { 
			get { return name; }
		}

		RetirementTimer retirementTimer = new RetirementTimer ();
		HealthManager healthManager = new HealthManager ();

		void Awake () {

			Inventory = new Inventory ();
			Inventory.Add (new HappinessHolder (100, 100));
			Inventory.Add (new MilkHolder (5, 0));
			Inventory.Add (new IceCreamHolder (3, 0));
			Inventory.Add (new MilkshakeHolder (3, 0));
			Inventory.Add (new ElderHolder (2, 0));

			PerformableActions = new PerformableActions (this);
			PerformableActions.Add ("CollectMilk", new CollectItem<MilkHolder> (0.5f));
			PerformableActions.Add ("DeliverMilk", new DeliverItem<MilkHolder> (0.5f));
			PerformableActions.Add ("CollectIceCream", new CollectItem<IceCreamHolder> (1));
			PerformableActions.Add ("DeliverIceCream", new DeliverItem<IceCreamHolder> (1));
			PerformableActions.Add ("CollectMilkshake", new CollectItem<MilkshakeHolder> (2));
			PerformableActions.Add ("DeliverMilkshake", new DeliverItem<MilkshakeHolder> (2));
			PerformableActions.Add ("CollectElder", new CollectItem<ElderHolder> (2));
			PerformableActions.Add ("DeliverElder", new DeliverItem<ElderHolder> (2));
			PerformableActions.Add ("CollectHappiness", new CollectHappiness (3));
			PerformableActions.Add ("ConsumeHappiness", new ConsumeItem<HappinessHolder> (5, false));
			PerformableActions.DisableAll ();
		}

		void OnAge (float progress) {
			healthManager.OnAge (progress);
			float p = Mathf.Clamp01 (Mathf.Abs (progress - 1));
			Path.Speed = Path.PathSettings.maxSpeed * Mathf.Sqrt(-(p - 2) * p);
		}

		void OnRetirement () {
			name = retirementName;
			Path.Active = false;
			UnitInfoContent.Refresh ();
		}

		public override void OnDragRelease (Unit unit) {
			if (retirementTimer.Retired) {
				House house = unit as House;
				if (house != null) {
					house.Inventory.AddItem<ElderHolder> (new ElderItem ());
					ObjectCreator.Instance.Destroy<Distributor> (transform);
				}
			}
		}

		public override void OnPoolCreate () {
			name = workerName;
			Inventory.Empty ();
			Inventory.Get<HappinessHolder> ().AddInitial (100);
			retirementTimer.BeginAging (OnAge, OnRetirement);
			Path.Active = true;
			UnitInfoContent.Refresh ();
		}
	}
}