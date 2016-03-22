using UnityEngine;
using System.Collections;
using DNA.Tasks;
using InventorySystem;
using DNA.InputSystem;

namespace DNA.Units {

	// TODO: show upgrade progress when task is started

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

		protected void OnInitPerformableTasks (PerformableTasks p) {
			// p.Add (new ResearchUpgrade<CoffeeCapacity> ());
			// p.Add (new ResearchUpgrade<MilkshakeCapacity> ());
			p.Add (new ResearchUnit<Apartment> ());
			p.Add (new UpgradeLevee ());
			// p.Add (new UpgradeFogOfWar ());

			foreach (var task in PerformableTasks.ActiveTasks) {
				task.Value.onStart += OnStartUpgrade;
				task.Value.onComplete += OnCompleteUpgrade;
			}
		}

		void OnStartUpgrade (PerformerTask p) {
			SetUpgradeInProgress (true);
		}

		void OnCompleteUpgrade (PerformerTask p) {
			SetUpgradeInProgress (false);
		}

		void SetUpgradeInProgress (bool inProgress) {
			foreach (var task in PerformableTasks.ActiveTasks)
				((UpgradeTask)task.Value).UpgradeInProgress = inProgress;
			SelectionHandler.Reselect (); // hack: force a redraw in the ui
		}
	}
}