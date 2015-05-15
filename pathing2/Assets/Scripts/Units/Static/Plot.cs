using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;
using GameInput;

namespace Units {
	
	public class Plot : StaticUnit, IActionPerformer {

		readonly string defaultName = "Plot";
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
			PerformableActions.Add (new GenerateUnit<MilkshakePool, MilkshakeHolder> (15, OnUnitGenerated), "Birth Milkshake Derrick (15M)");
			PerformableActions.Add (new GenerateUnit<CoffeePlant, MilkshakeHolder> (5, OnUnitGenerated), "Birth Coffee Plant (5M)");
			PerformableActions.Add (new GenerateUnit<Jacuzzi, MilkshakeHolder> (20, OnUnitGenerated), "Birth Jacuzzi (20M)");
			PerformableActions.Add (new GenerateUnit<Clinic, MilkshakeHolder> (25, OnUnitGenerated), "Birth Clinic (25M)");
			// PerformableActions.Add ("CancelGenerateUnit", new CancelGenerateUnit (), "Cancel");
		}

		public override void OnPoolCreate () {
			if (name != defaultName) {
				name = defaultName;
				Inventory.Get<MilkshakeHolder> ().DisplaySettings = new ItemHolderDisplaySettings (false, true);
				PerformableActions.ActivateAll ();
				unitInfoContent.Refresh ();
			}
		}

		void OnStartAction (string id) {
			PerformableActions.DeactivateAll ();
			AcceptableActions.SetActive ("DeliverMilkshake", true);
			string newUnit = "";
			switch (id) {
				case "GenerateMilkshakePool": 	newUnit = "Milkshake Derrick"; break;
				case "GenerateCoffeePlant": 	newUnit = "Coffee Plant"; break;
				case "GenerateJacuzzi": 		newUnit = "Jacuzzi"; break;
				case "GenerateClinic": 			newUnit = "Clinic"; break;
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
			staticUnit.Position = Position;
			staticUnit.PathPoint = PathPoint;
			PathPoint.StaticUnit = staticUnit;
			if (Selected) {
				SelectionManager.Select (staticUnit.UnitClickable);
			}
			ObjectCreator.Instance.Destroy<Plot> (transform);
		}
	}
}