using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.InventorySystem;
using DNA.InputSystem;
using DNA.EventSystem;
using DNA.Tasks;

namespace DNA.Units {
	
	public class Plot : StaticUnit, ITaskPerformer {

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

		public override bool PathPointEnabled {
			get { return true; }
		}

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

			/*AcceptableActions.Add (new GameActions.AcceptDeliverItem<MilkshakeHolder> ());
			AcceptableActions.SetActive ("DeliverMilkshake", false);*/

			//Events.instance.AddListener<UnlockUnitEvent> (OnUnlockUnitEvent);
		}

		protected virtual void Start () {
			//PerformableTasks.Add (new DNA.Tasks.GenerateUnit<MilkshakePool> (Player.Instance.Inventory)).onEnd += OnGenerateUnit;
			//PerformableTasks.Add (new DNA.Tasks.GenerateUnit<CoffeePlant> (Player.Instance.Inventory)).onEnd += OnGenerateUnit;
		}

		//public override void OnPoolCreate () {
		protected override void OnEnable () {
			if (name != DefaultName) {
				name = DefaultName;
				Inventory.Get<MilkshakeHolder> ().DisplaySettings = new ItemHolderDisplaySettings (false, true);
				RefreshInfoContent ();
			}
			base.OnEnable ();
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

		protected virtual void DestroyThis () {
			DestroyThis<Plot> ();
		}
	}
}