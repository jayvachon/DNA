using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameInventory;
using GameActions;
using GameInput;
using GameEvents;
using DNA.Tasks;

namespace Units {
	
	public class Plot : StaticUnit, IActionPerformer, ITaskPerformer {

		protected virtual string DefaultName { 
			get { return "Plot"; }
		}

		new string name = "Plot";
		public override string Name {
			get { return name; }
		}

		public override string Description {
			get { return "Construct buildings on plots."; }
		}

		bool pathPointEnabled = false;
		public override bool PathPointEnabled {
			get { return true; }
		}

		public PerformableActions PerformableActions { get; private set; }

		PerformableTasks performableTasks;
		public PerformableTasks PerformableTasks {
			get {
				if (performableTasks == null) {
					performableTasks = new PerformableTasks (this);
				}
				return performableTasks;
			}
		}

		BuildingIndicator indicator;

		void Awake () {

			Inventory = new Inventory (this);
			Inventory.Add (new MilkshakeHolder (0, 0));

			AcceptableActions = new AcceptableActions (this);
			AcceptableActions.Add (new GameActions.AcceptDeliverItem<MilkshakeHolder> ());
			AcceptableActions.SetActive ("DeliverMilkshake", false);

			//Events.instance.AddListener<UnlockUnitEvent> (OnUnlockUnitEvent);
		}

		protected virtual void Start () {
			
			PerformableTasks.Add (new DNA.Tasks.GenerateUnit<MilkshakePool> (Player.Instance.Inventory)).onEnd += OnGenerateUnit;

			/*PerformableActions = new PerformableActions (this);
			PerformableActions.OnStartAction += OnStartAction;
			//PerformableActions.Add (new GenerateUnit<CoffeePlant, MilkshakeHolder> (-1, OnUnitGenerated), "Birth Coffee Plant (5M)");
			PerformableActions.Add (new GenerateUnit<Jacuzzi, MilkshakeHolder> (-1, OnUnitGenerated), "Birth Jacuzzi (10M)");
			PerformableActions.Add (new GenerateUnit<Clinic, MilkshakeHolder> (-1, OnUnitGenerated), "Birth Clinic (15M)");
			//PerformableActions.Add (new GenerateUnit<University, MilkshakeHolder> (-1, OnUnitGenerated), "Birth University (25M)");
			PerformableActions.Add (new CancelGenerateUnit (), "Cancel");
			SetActiveActions ();*/
		}

		protected void SetActiveActions () {
			//PerformableActions.SetActive ("GenerateCoffeePlant", false);
			//PerformableActions.SetActive ("GenerateJacuzzi", false);
			//PerformableActions.SetActive ("GenerateClinic", false);
			/*PerformableActions.SetActive ("GenerateUniversity", false);
			List<string> unlockedUnits = StaticUnitsManager.UnlockedUnits;
			for (int i = 0; i < unlockedUnits.Count; i ++) {
				PerformableActions.SetActive ("Generate" + unlockedUnits[i], true);
			}*/
			PerformableActions.ActivateAll ();
			PerformableActions.SetActive ("CancelGenerateUnit", false);
		}

		public override void OnPoolCreate () {
			if (name != DefaultName) {
				name = DefaultName;
				Inventory.Get<MilkshakeHolder> ().DisplaySettings = new ItemHolderDisplaySettings (false, true);
				//PerformableActions.ActivateAll ();
				//SetActiveActions ();
				RefreshInfoContent ();
			}
		}

		// deprecate
		void OnStartAction (string id) {
			if (id == "CancelGenerateUnit") {
				CancelGenerateUnit ();
			} else {
				GenerateUnit (id);
			}
		}

		// deprecate
		void CancelGenerateUnit () {
			pathPointEnabled = false;
			PerformableActions.StopAll ();
			PerformableActions.ActivateAll ();
			AcceptableActions.SetActive ("DeliverMilkshake", false);
			SetActiveActions ();
			name = DefaultName;
			Inventory.Get<MilkshakeHolder> ().DisplaySettings = new ItemHolderDisplaySettings (false, false);
			ObjectCreator.Instance.Destroy<BuildingIndicator> (indicator.MyTransform);
			unitInfoContent.Refresh ();
		}

		void OnGenerateUnit (PerformerTask task) {
			Unit unit = ((GenerateUnit)task).GeneratedUnit;
			StaticUnit staticUnit = unit as StaticUnit;
			staticUnit.Position = Position;
			staticUnit.PathPoint = PathPoint;
			PathPoint.StaticUnit = staticUnit;
			if (Selected) SelectionManager.Select (staticUnit.UnitClickable);
			DestroyThis ();
		}

		// deprecate
		protected virtual void GenerateUnit (string id) {
			
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
				case "GenerateUniversity":		newUnit = "University"; break;
			}

			name = string.Format ("{0} to Be", newUnit);
			
			Inventory.Get<MilkshakeHolder> ().DisplaySettings = new ItemHolderDisplaySettings (true, true);
			indicator = ObjectCreator.Instance.Create<BuildingIndicator> ().GetScript<BuildingIndicator> ();
			indicator.Initialize (newUnit, Transform);
			unitInfoContent.Refresh ();
		}

		// deprecate
		protected void OnUnitGenerated (Unit unit) {
			AcceptableActions.SetActive ("DeliverMilkshake", false);
			if (indicator != null)
				ObjectCreator.Instance.Destroy<BuildingIndicator> (indicator.MyTransform);

			StaticUnit staticUnit = unit as StaticUnit;
			staticUnit.Position = Position;
			staticUnit.PathPoint = PathPoint;
			PathPoint.StaticUnit = staticUnit;
			if (Selected) {
				SelectionManager.Select (staticUnit.UnitClickable);
			}
			DestroyThis ();
		}

		// TODO: Move to StaticUnit
		protected virtual void DestroyThis () {
			ObjectCreator.Instance.Destroy<Plot> (transform);
		}

		/*void OnUnlockUnitEvent (UnlockUnitEvent e) {
			PerformableActions.SetActive ("Generate" + e.id, true);
		}*/
	}
}