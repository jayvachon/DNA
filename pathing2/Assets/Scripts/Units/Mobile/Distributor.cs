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
			get { return "Distributor"; }
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
			PerformableActions.Add ("ConsumeHappiness", new ConsumeItem<HappinessHolder> (5, true, true, false));
			PerformableActions.DisableAll ();
		}

		void OnAge (float progress) {
			float p = Mathf.Clamp01 (Mathf.Abs (progress - 1));
			Path.Speed = Path.PathSettings.maxSpeed * Mathf.Sqrt(-(p - 2) * p);
		}

		void OnRetirement () {
			/*name = retirementName;
			Path.Active = false;
			UnitInfoContent.Refresh ();*/
			CreateElder ();
			DestroySelf ();			
		}

		public override void OnDragRelease (Unit unit) {
			/*if (RetirementTimer.Retired) {
				House house = unit as House;
				if (house != null) {
					house.Inventory.AddItem<ElderHolder> (new ElderItem ());
					ObjectCreator.Instance.Destroy<Distributor> (transform);
				}
			}*/
		}

		public override void OnPoolCreate () {
			Inventory.Empty ();
			Inventory.Get<HappinessHolder> ().AddInitial (100);
			RetirementTimer.BeginAging (OnAge, OnRetirement);
			Path.Active = true;
			UnitInfoContent.Refresh ();
		}

		void CreateElder () {
			Elder elder = ObjectCreator.Instance.Create<Elder> ().GetScript<Elder> ();
			elder.Position = MobileTransform.Position;
			if (Selected) {
				SelectionManager.Select (elder.UnitClickable);
			}
		}

		void DestroySelf () {
			Path.Active = false;
			ObjectCreator.Instance.Destroy<Distributor> (transform);
		}
	}
}