using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

namespace Units {

	public class GivingTreeUnit : StaticUnit, IActionPerformer {

		public override string Name {
			get { return "Giving Tree"; }
		}

		public PerformableActions PerformableActions { get; private set; }

		void Awake () {

			Inventory = new Inventory (this);
			Inventory.Add (new CoffeeHolder (100, 20));
			Inventory.Add (new YearHolder (500, 0));
			Inventory.Get<YearHolder> ().HolderFilled += OnYearsCollected;

			AcceptableActions = new AcceptableActions (this);
			AcceptableActions.Add ("DeliverCoffee", new AcceptDeliverItem<CoffeeHolder> ());
			AcceptableActions.Add ("DeliverYear", new AcceptDeliverItem<YearHolder> ());

			PerformableActions = new PerformableActions (this);
			Vector3 createPosition = StaticTransform.Position;
			createPosition.x -= 2;
			
			PerformableActions.Add ("GenerateLaborer", new GenerateUnit<Distributor, CoffeeHolder> (5, createPosition), "Birth Laborer");
		}

		void OnYearsCollected () {
			Debug.Log ("on to the next tree!");
		}
	}
}