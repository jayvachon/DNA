using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;
using Pathing;

namespace Units {

	public class Distributor : MobileUnit, IActionPerformer {

		new string name = "Distributor";
		public override string Name {
			get { return name; }
		}

		public PerformableActions PerformableActions { get; private set; }

		AgeManager ageManager = new AgeManager ();

		void Awake () {

			Inventory = new Inventory ();
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
			PerformableActions.Add ("CollectUnitElder", new CollectUnit<Elder> (3));
		}

		void Start () {
			ageManager.BeginAging (OnAge, OnRetirement);
		}

		void OnAge (float progress) {
			float p = Mathf.Clamp01 (Mathf.Abs (progress - 1));
			Path.Speed = Path.PathSettings.maxSpeed * Mathf.Sqrt(-(p - 2) * p);
		}

		void OnRetirement () {
			Path.Speed = 0;
			MobileTransform.StopMovingOnPath ();
			name = "Elder";
			unitInfoContent.Refresh ();
			Path.Enabled = false;
		}

		public override void OnRelease () {
			MobileTransform.StartMovingOnPath ();
			if (!ageManager.Retired) {
				Path.Speed = Path.PathSettings.maxSpeed;
			}
		}

		public override void OnDragRelease (Unit unit) {
			if (ageManager.Retired) {
				House house = unit as House;
				if (house != null) {
					house.Inventory.AddItem<ElderHolder> (new ElderItem ());
					ObjectCreator.Instance.Destroy<Distributor> (transform);
				}
			}
		}

		public override void OnCreate () {
			Path.Enabled = true;
		}
	}
}