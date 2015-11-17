using UnityEngine;
using System.Collections;
//using DNA.InventorySystem;
using DNA.InputSystem;
using InventorySystem;

namespace DNA.Units {

	public class Elder : MobileUnit {

		HealthManager2 healthManager = new HealthManager2 ();
		public HealthManager2 HealthManager {
			get { return healthManager; }
		}

		/*public int AverageHappiness { 
			set { Inventory.RemoveItems<HealthHolder> (100 - value); }
		}*/

		void Awake () {

			unitRenderer.SetColors (new Color (0.447f, 0.251f, 0.447f));

			//Inventory.Get<YearHolder> ().DisplaySettings = new ItemHolderDisplaySettings (true, false);
			//healthHolder.DisplaySettings = new ItemHolderDisplaySettings (true, true);

			/*PerformableActions.Add (new ConsumeItem<HealthHolder> ());
			PerformableActions.Add (new CollectHealth ());

			PerformableActions.Add (new GenerateItem<YearHolder> ());
			PerformableActions.Add (new OccupyUnit ());
			PerformableActions.SetActive ("OccupyUnit", false);*/
		}

		protected override void OnInitInventory (Inventory i) {
			i.Add (new YearGroup (65, 500));
			i.Add (new HealthGroup (100, 100));
		}

		protected override void OnEnable () {
			InitInventory ();
			NotificationCenter.Instance.ShowNotification ("laborerRetired");
			base.OnEnable ();
		}

		void InitInventory () {
			Inventory["Years"].Clear ();
			Inventory["Years"].Set (65);
			Inventory["Health"].Set (100);
			Inventory["Health"].onEmpty += OnDie;
		}

		protected override void OnDisable () {
			Inventory["Health"].onEmpty -= OnDie;
			base.OnDisable ();
		}

		void OnDie () {
			ChangeUnit<Elder, Corpse> ();
		}

		protected override void OnChangeUnit<U> (U u) {
			Corpse corpse = u as Corpse;
			//corpse.Inventory.Transfer<YearHolder> (Inventory);
			Inventory.Get<YearGroup> ().Transfer (corpse.Inventory.Get<YearGroup> ());
		}
	}
}