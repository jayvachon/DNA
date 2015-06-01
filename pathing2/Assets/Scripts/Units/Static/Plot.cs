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

		bool pathPointEnabled = false;
		public override bool PathPointEnabled {
			get { return pathPointEnabled; }
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
			PerformableActions.Add (new GenerateUnit<CoffeePlant, MilkshakeHolder> (-1, OnUnitGenerated), "Birth Coffee Plant (5M)");
			PerformableActions.Add (new GenerateUnit<Jacuzzi, MilkshakeHolder> (-1, OnUnitGenerated), "Birth Jacuzzi (20M)");
			PerformableActions.Add (new GenerateUnit<Clinic, MilkshakeHolder> (-1, OnUnitGenerated), "Birth Clinic (25M)");
			PerformableActions.Add (new CancelGenerateUnit (), "Cancel");
			PerformableActions.SetActive ("CancelGenerateUnit", false);
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
			if (id == "CancelGenerateUnit") {
				CancelGenerateUnit ();
			} else {
				GenerateUnit (id);
			}
		}

		void CancelGenerateUnit () {
			pathPointEnabled = false;
			PerformableActions.StopAll ();
			PerformableActions.ActivateAll ();
			PerformableActions.SetActive ("CancelGenerateUnit", false);
			AcceptableActions.SetActive ("DeliverMilkshake", false);
			name = "Plot";
			Inventory.Get<MilkshakeHolder> ().DisplaySettings = new ItemHolderDisplaySettings (false, false);
			ObjectCreator.Instance.Destroy<BuildingIndicator> (indicator.MyTransform);
			unitInfoContent.Refresh ();
		}

		void GenerateUnit (string id) {
			if (!gameObject.activeSelf) return;
			pathPointEnabled = true;
			PerformableActions.DeactivateAll ();
			PerformableActions.SetActive ("CancelGenerateUnit", true);
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