using UnityEngine;
using System.Collections;
using DNA.Tasks;
using InventorySystem;

namespace DNA.Units {

	// TODO: handle damaged houses	

	public class House : StaticUnit, ITaskPerformer {

		PerformableTasks performableTasks;
		public PerformableTasks PerformableTasks {
			get {
				if (performableTasks == null) {
					performableTasks = new PerformableTasks (this);
					OnInitPerformableTasks (performableTasks);
				}
				return performableTasks;
			}
		}

		protected void OnInitPerformableTasks (PerformableTasks p) {
			p.Add (new DemolishUnit (Container));
		}

		protected override void OnEnable () {
			base.OnEnable ();
			Player.Instance.Inventory["Laborer"].Capacity += 3;
		}

		protected override void OnDisable () {
			base.OnDisable ();
			Player.Instance.Inventory["Laborer"].Capacity -= 3;
		}
	}
}