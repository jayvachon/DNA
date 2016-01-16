using UnityEngine;
using System.Collections;
using DNA.Tasks;
using InventorySystem;

namespace DNA.Units {

	public class University : StaticUnit, ITaskPerformer {

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
			Inventory = Player.Instance.Inventory;
		}

		protected override void OnInitAcceptableTasks (AcceptableTasks a) {
			a.Add (new AcceptCollectItem<LaborGroup> ());
		}

		protected void OnInitPerformableTasks (PerformableTasks p) {
			p.Add (new ResearchUpgrade<CoffeeCapacity> ());
			p.Add (new ResearchUpgrade<MilkshakeCapacity> ());
			p.Add (new UpgradeLevee ());
		}
	}
}