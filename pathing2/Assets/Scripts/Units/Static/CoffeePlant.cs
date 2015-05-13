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
		
		public PerformableActions PerformableActions { get; private set; }

		void Awake () {
			
			Inventory = new Inventory (this);
			Inventory.Add (new CoffeeHolder (25, 0));
			Inventory.Add (new YearHolder (5, 5));
			Inventory.Get<CoffeeHolder> ().DisplaySettings = new ItemHolderDisplaySettings (true, false);
			Inventory.Get<YearHolder> ().HolderEmptied += OnDie;

			AcceptableActions = new AcceptableActions (this);
			AcceptableActions.Add ("CollectCoffee", new AcceptCollectItem<CoffeeHolder> ());

			PerformableActions = new PerformableActions (this);
			PerformableActions.Add ("GenerateCoffee", new GenerateItem<CoffeeHolder> ());
			PerformableActions.Add ("ConsumeCoffee", new ConsumeItem<CoffeeHolder> ());
			PerformableActions.Add ("ConsumeYear", new ConsumeItem<YearHolder> (TimerValues.year));
		}

		void OnDie () {
			StaticUnit plot = ObjectCreator.Instance.Create<Plot> (Vector3.zero).GetScript<Plot> () as StaticUnit;
			plot.Position = Position;
			plot.PathPoint = PathPoint;
			if (Selected) {
				SelectionManager.Select (plot.UnitClickable);
			}
			ObjectCreator.Instance.Destroy<CoffeePlant> (transform);
		}
	}
}