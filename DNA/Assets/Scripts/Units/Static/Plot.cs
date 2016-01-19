using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using DNA.InventorySystem;
using DNA.InputSystem;
using DNA.EventSystem;
using DNA.Tasks;
using InventorySystem;

namespace DNA.Units {
	
	public class Plot : StaticUnit, ITaskPerformer {

		public override string Name {
			get { return "Plot"; }
		}

		public override string Description {
			get { return "Construct buildings on plots."; }
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

		void Awake () {
			Inventory = new Inventory (this);
			Inventory.Add (new MilkshakeGroup ());

		}
		void OnGenerateUnit (PerformerTask task) {
			Unit unit = ((GenerateUnit)task).GeneratedUnit;
			StaticUnit staticUnit = unit as StaticUnit;
			staticUnit.Position = Position;
			if (Selected) SelectionManager.Select (staticUnit.UnitClickable);
			DestroyThis ();
		}

		protected virtual void DestroyThis () {
			DestroyThis<Plot> ();
		}
	}
}