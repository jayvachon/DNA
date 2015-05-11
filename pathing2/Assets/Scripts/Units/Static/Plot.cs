using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

namespace Units {
	
	public class Plot : StaticUnit, IActionPerformer {

		new string name = "Plot";
		public override string Name {
			get { return name; }
		}

		public PerformableActions PerformableActions { get; private set; }

		BuildingIndicator indicator;

		void Awake () {

			Inventory = new Inventory (this);
			Inventory.Add (new MilkshakeHolder (0, 0));

			AcceptableActions = new AcceptableActions (this);
			AcceptableActions.Add (new AcceptDeliverItem<MilkshakeHolder> ());
			AcceptableActions.SetActive ("DeliverMilkshake", false);
		}

		void Start () {
			PerformableActions = new PerformableActions (this);
			PerformableActions.StartAction += OnStartAction;
			//PerformableActions.Add ("GenerateHouse", new GenerateUnit<House, MilkHolder> (5, Position, OnUnitGenerated), "Birth House");
			//PerformableActions.Add ("GenerateMilkPool", new GenerateUnit<MilkPool, MilkHolder> (5, Position, OnUnitGenerated), "Birth Milk Pool");
			//PerformableActions.Add ("GeneratePasture", new GenerateUnit<Pasture, MilkHolder> (10, Position, OnUnitGenerated), "Birth Pasture");
			//PerformableActions.Add ("GenerateMilkshakeMaker", new GenerateUnit<MilkshakeMaker, MilkHolder> (10, Position, OnUnitGenerated), "Birth Milkshake Maker");
			//PerformableActions.Add ("GenerateHospital", new GenerateUnit<Hospital, MilkHolder> (10, Position, OnUnitGenerated), "Birth Hospital");
			PerformableActions.Add (new GenerateUnit<MilkshakePool, MilkshakeHolder> (15, Position, OnUnitGenerated), "Birth Milkshake Derrick (15M)");
			PerformableActions.Add (new GenerateUnit<CoffeePlant, MilkshakeHolder> (5, Position, OnUnitGenerated), "Birth Coffee Plant (5M)");
			PerformableActions.Add (new GenerateUnit<Jacuzzi, MilkshakeHolder> (20, Position, OnUnitGenerated), "Birth Jacuzzi (20M)");
			PerformableActions.Add (new GenerateUnit<Clinic, MilkshakeHolder> (25, Position, OnUnitGenerated), "Birth Clinic (25M)");
			// PerformableActions.Add ("CancelGenerateUnit", new CancelGenerateUnit (), "Cancel");
		}

		void OnStartAction (string id) {
			PerformableActions.DeactivateAll ();
			AcceptableActions.SetActive ("DeliverMilkshake", true);
			string newUnit = "";
			switch (id) {
				case "GenerateHouse": 			newUnit = "House"; break;
				case "GenerateClinic": 			newUnit = "Clinic"; break;
				case "GenerateMilkPool": 		newUnit = "Milk Pool"; break;
				case "GeneratePasture": 		newUnit = "Pasture"; break;
				case "GenerateMilkshakeMaker": 	newUnit = "Milkshake Maker"; break;
				case "GenerateHospital": 		newUnit = "Hospital"; break;
				case "GenerateCoffeePlant": 	newUnit = "Coffee Plant"; break;
				case "GenerateMilkshakePool": 	newUnit = "Milkshake Derrick"; break;
				case "GenerateJacuzzi": 		newUnit = "Jacuzzi"; break;
			}
			name = string.Format ("{0} to Be", newUnit);
			Inventory.Get<MilkshakeHolder> ().DisplaySettings = new ItemHolderDisplaySettings (true, true);
			indicator = ObjectCreator.Instance.Create<BuildingIndicator> ().GetScript<BuildingIndicator> ();
			indicator.Initialize (newUnit, Position);
			unitInfoContent.Refresh ();
		}

		void OnUnitGenerated (Unit unit) {
			AcceptableActions.SetActive ("DeliverMilkshake", false);
			ObjectCreator.Instance.Destroy<BuildingIndicator> (indicator.MyTransform);
			StaticUnit staticUnit = unit as StaticUnit;
			staticUnit.PathPoint = PathPoint;
			PathPoint.StaticUnit = staticUnit;
			ObjectCreator.Instance.Destroy<Plot> (transform);
		}
	}
}