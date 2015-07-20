using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;
using GameInput;

namespace Units {

	public class CoffeePlant : StaticUnit, IActionPerformer {

		public override string Name {
			get { return "Coffee Plant"; }
		}

		public override string Description {
			get { return "Deliver coffee to the Giving Tree to create more Laborers."; }
		}
		
		public PerformableActions PerformableActions { get; private set; }

		static bool shortLife = false;

		void Awake () {
			
			Inventory = new Inventory (this);
			Inventory.Add (new CoffeeHolder (20, 0));
			Inventory.Add (new YearHolder (30, 0));
			Inventory.Get<CoffeeHolder> ().DisplaySettings = new ItemHolderDisplaySettings (true, false);

			AcceptableActions = new AcceptableActions (this);
			AcceptableActions.Add (new AcceptCollectItem<CoffeeHolder> ());

			PerformableActions = new PerformableActions (this);
			PerformableActions.Add (new GenerateItem<CoffeeHolder> ());
			PerformableActions.Add (new ConsumeItem<YearHolder> (TimerValues.Instance.Year));
			PerformableActions.SetActive ("ConsumeYear", false);
		}

		public override void OnPoolCreate () {
			base.OnPoolCreate ();
			Inventory.Get<YearHolder> ().HolderEmptied += OnDie;
			Inventory.Get<YearHolder> ().Initialize ();
			if (!shortLife) {
				Inventory.Get<YearHolder> ().Remove (20);
				shortLife = true;
			}
			PerformableActions.SetActive ("ConsumeYear", true);
		}

		void OnDie () {
			Inventory.Get<YearHolder> ().HolderEmptied -= OnDie;
			PerformableActions.Stop ("ConsumeYear");
			PerformableActions.SetActive ("ConsumeYear", false);
			Destroy<CoffeePlant> ();
		}
	}
}