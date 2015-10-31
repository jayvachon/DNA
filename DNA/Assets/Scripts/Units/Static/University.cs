using UnityEngine;
using System.Collections;

namespace DNA.Units {

	public class University : StaticUnit {

		public override string Name {
			get { return "University"; }
		}

		//CoffeeHolder coffeeHolder;

		void Awake () {

			unitRenderer.SetColors (new Color (0.831f, 0.231f, 1f));

			//Inventory = new Inventory (this);
			//coffeeHolder = (CoffeeHolder)Inventory.Add (new CoffeeHolder (0, 0));
			//coffeeHolder.DisplaySettings = new ItemHolderDisplaySettings (false, false);

			/*AcceptableActions.Add (new AcceptDeliverItem<CoffeeHolder> ());
			AcceptableActions.SetActive ("DeliverCoffee", false);*/

			//PerformableActions.OnStartAction += OnStartAction;
			//PerformableActions.Add (new ResearchUnit<Jacuzzi, CoffeeHolder> (OnUnitUnlocked), "Research Happiness");
			//PerformableActions.Add (new ResearchUnit<Clinic, CoffeeHolder> (OnUnitUnlocked), "Research Clinic (15C)");
		}

		void OnUnitUnlocked (string id) {
			//AcceptableActions.SetActive ("DeliverCoffee", false);
			//coffeeHolder.Clear ();
			//coffeeHolder.DisplaySettings = new ItemHolderDisplaySettings (false);
		}

		void OnStartAction (string id) {
			//PerformableActions.DeactivateAll ();
			//AcceptableActions.SetActive ("DeliverCoffee", true);
			//coffeeHolder.DisplaySettings = new ItemHolderDisplaySettings (true, true);
			//RefreshInfoContent ();
		}
	}
}