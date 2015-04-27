using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

namespace Units {

	public class CoffeePlant : StaticUnit, IActionPerformer {

		public override string Name {
			get { return "Coffee Plant"; }
		}
		
		public PerformableActions PerformableActions { get; private set; }

		void Awake () {
			
			Inventory = new Inventory (this);
			Inventory.Add (new CoffeeHolder (5, 0));

			AcceptableActions = new AcceptableActions (this);
			AcceptableActions.Add ("CollectCoffee", new AcceptCollectItem<CoffeeHolder> ());

			PerformableActions = new PerformableActions (this);
			PerformableActions.Add ("GenerateCoffee", new GenerateItem<CoffeeHolder> ());
			PerformableActions.Add ("ConsumeCoffee", new ConsumeItem<CoffeeHolder> ());
		}
	}
}