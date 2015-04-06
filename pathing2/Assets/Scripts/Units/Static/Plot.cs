using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

namespace Units {
	
	public class Plot : StaticUnit, IActionAcceptor, IActionPerformer {

		new string name = "Plot";
		public override string Name {
			get { return name; }
		}

		public AcceptableActions AcceptableActions { get; private set; }
		public PerformableActions PerformableActions { get; private set; }

		void Awake () {

			Inventory = new Inventory ();
			Inventory.Add (new MilkHolder (0, 0));

			AcceptableActions = new AcceptableActions (this);
			AcceptableActions.Add ("DeliverMilk", new AcceptDeliverItem<MilkHolder> ());
			AcceptableActions.Disable ("DeliverMilk");
		}

		void Start () {
			PerformableActions = new PerformableActions (this);
			PerformableActions.StartAction += OnStartAction;
			PerformableActions.Add ("GenerateHouse", new GenerateUnit<House, MilkHolder> (5, Position, OnUnitGenerated), "Birth House");
			PerformableActions.Add ("GenerateMilkPool", new GenerateUnit<MilkPool, MilkHolder> (5, Position, OnUnitGenerated), "Birth Milk Pool");
			PerformableActions.Add ("GeneratePasture", new GenerateUnit<Pasture, MilkHolder> (10, Position, OnUnitGenerated), "Birth Pasture");
			PerformableActions.Add ("GenerateMilkshakeMaker", new GenerateUnit<MilkshakeMaker, MilkHolder> (10, Position, OnUnitGenerated), "Birth Milkshake Maker");
			PerformableActions.Add ("GenerateHospital", new GenerateUnit<Hospital, MilkHolder> (10, Position, OnUnitGenerated), "Birth Hospital");
		}

		void OnStartAction (string id) {
			AcceptableActions.Enable ("DeliverMilk");
			PerformableActions.DisableAll ();
			string newUnit = "";
			switch (id) {
				case "GenerateHouse": newUnit = "House"; break;
				case "GenerateMilkPool": newUnit = "Milk Pool"; break;
				case "GeneratePasture": newUnit = "Pasture"; break;
				case "GenerateMilkshakeMaker": newUnit = "Milkshake Maker"; break;
				case "GenerateHospital": newUnit = "Hospital"; break;
			}
			name = string.Format ("{0} to Be", newUnit);
			unitInfoContent.Refresh ();
		}

		void OnUnitGenerated (Unit unit) {
			StaticUnit staticUnit = unit as StaticUnit;
			staticUnit.PathPoint = PathPoint;
			PathPoint.StaticUnit = staticUnit;
			ObjectCreator.Instance.Destroy<Plot> (transform);
		}
	}
}