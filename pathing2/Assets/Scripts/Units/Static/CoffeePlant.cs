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
			Inventory.Add (new CoffeeHolder (25, 10));
			Inventory.Add (new YearHolder (10, 10));
			Inventory.Get<CoffeeHolder> ().DisplaySettings = new ItemHolderDisplaySettings (true, false);
			Inventory.Get<YearHolder> ().HolderEmptied += OnDie;

			AcceptableActions = new AcceptableActions (this);
			AcceptableActions.Add (new AcceptCollectItem<CoffeeHolder> ());

			PerformableActions = new PerformableActions (this);
			PerformableActions.Add (new GenerateItem<CoffeeHolder> ());
			PerformableActions.Add (new ConsumeItem<CoffeeHolder> ());
			// left off here -- for some reason consume year can't find this performer's inventory
			PerformableActions.Add (new ConsumeItem<YearHolder> (TimerValues.year));
		}

		void OnDie () {
			StaticUnit plot = ObjectCreator.Instance.Create<Plot> (Vector3.zero).GetScript<Plot> () as StaticUnit;
			plot.Position = Position;
			plot.PathPoint = PathPoint;
			ObjectCreator.Instance.Destroy<CoffeePlant> (transform);
		}
	}
}