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
			Inventory.Add (new YearHolder (10, 0));
			Inventory.Get<CoffeeHolder> ().DisplaySettings = new ItemHolderDisplaySettings (true, false);

			AcceptableActions = new AcceptableActions (this);
			AcceptableActions.Add (new AcceptCollectItem<CoffeeHolder> ());

			PerformableActions = new PerformableActions (this);
			PerformableActions.Add (new GenerateItem<CoffeeHolder> ());
			//PerformableActions.Add (new ConsumeItem<CoffeeHolder> ());
			PerformableActions.Add (new ConsumeItem<YearHolder> (TimerValues.Instance.Year));
			PerformableActions.SetActive ("ConsumeYear", false);
		}

		public override void OnPoolCreate () {
			Inventory.Get<YearHolder> ().HolderEmptied += OnDie;
			Inventory.Get<YearHolder> ().Initialize (15);
			PerformableActions.SetActive ("ConsumeYear", true);
		}

		void OnDie () {
			Inventory.Get<YearHolder> ().HolderEmptied -= OnDie;
			PerformableActions.SetActive ("ConsumeYear", false);
			StaticUnit plot = ObjectCreator.Instance.Create<Plot> (Vector3.zero).GetScript<Plot> () as StaticUnit;
			plot.Position = Position;
			plot.PathPoint = PathPoint;
			PathPoint.StaticUnit = plot;
			if (Selected) {
				SelectionManager.Select (plot.UnitClickable);
			}
			ObjectCreator.Instance.Destroy<CoffeePlant> (transform);
		}
	}
}