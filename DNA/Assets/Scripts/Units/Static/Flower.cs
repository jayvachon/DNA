using UnityEngine;
using System.Collections;
using DNA.Tasks;
using InventorySystem;

namespace DNA.Units {

	public class Flower : StaticUnit, ISeedProducer {

		SeedProductionHandler seedProduction;

		protected override void OnEnable () {
			base.OnEnable ();
			StartSeedProduction ();
		}

		protected override void OnDisable () {
			base.OnDisable ();
			seedProduction.Stop ();
		}

		protected override void OnInitInventory (Inventory i) {
			i.Add (new HappinessGroup (200, 200));
			i["Happiness"].onUpdate += OnCollectHappiness;
		}

		protected override void OnInitAcceptableTasks (AcceptableTasks a) {
			a.Add (new AcceptCollectItem<HappinessGroup> ());
		}

		void OnCollectHappiness () {
			Inventory["Happiness"].Fill ();
		}

		public void StartSeedProduction () {
			seedProduction = new SeedProductionHandler (MyTransform, 2.5f);
		}
	}
}