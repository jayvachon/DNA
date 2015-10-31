using UnityEngine;
using System.Collections;
using DNA.Tasks;
using InventorySystem;

namespace DNA.Units {

	public class University : StaticUnit, ITaskPerformer {

		public override string Name {
			get { return "University"; }
		}

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

		void Awake () {
			unitRenderer.SetColors (new Color (0.831f, 0.231f, 1f));
			Inventory = Player.Instance.Inventory;
		}

		protected override void OnInitInventory (Inventory i) {
			i.Add (new LaborGroup ());
		}

		protected override void OnInitAcceptableTasks (AcceptableTasks a) {
			a.Add (new AcceptCollectItem<LaborGroup> ());
		}

		protected void OnInitPerformableTasks (PerformableTasks p) {
			p.Add (new ResearchUpgrade<CoffeeCapacity> ());
		}
	}
}