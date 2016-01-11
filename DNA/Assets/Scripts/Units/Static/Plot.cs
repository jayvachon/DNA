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

			// unitRenderer.SetColors (new Color (0.47f, 0.043f, 0.24f));
			// unitRenderer.SetColors (Palette.Black);

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