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

		ProgressBar pbar;
		bool upgrading = false;

		void Awake () {
			Inventory = Player.Instance.Inventory;
		}

		protected void OnInitPerformableTasks (PerformableTasks p) {
			// p.Add (new ResearchUpgrade<CoffeeCapacity> ());
			// p.Add (new ResearchUpgrade<MilkshakeCapacity> ());
			p.Add (new ResearchUpgrade<LaborerSpeed> ());
			p.Add (new ResearchUnit<Apartment> ());
			p.Add (new UpgradeLevee ());
			// p.Add (new ResearchUpgrade<Eyesight> ());
			// p.Add (new UpgradeFogOfWar ());

			foreach (var task in PerformableTasks.ActiveTasks) {
				task.Value.onStart += OnStartUpgrade;
				task.Value.onComplete += OnCompleteUpgrade;
			}
		}

		void OnStartUpgrade (PerformerTask p) {
			SetUpgradeInProgress (true);
			pbar = UI.Instance.CreateProgressBar (Position);
			pbar.SetColor (Color.green);
			StartCoroutine (CoUpdateUpgradeProgress (p));
		}

		void OnCompleteUpgrade (PerformerTask p) {
			SetUpgradeInProgress (false);
			UI.Instance.DestroyProgressBar (pbar); 
			upgrading = false;
		}

		void SetUpgradeInProgress (bool inProgress) {
			foreach (var task in PerformableTasks.ActiveTasks)
				((UpgradeTask)task.Value).UpgradeInProgress = inProgress;
			SelectionHandler.Reselect (); // hack: force a redraw in the ui
		}

		IEnumerator CoUpdateUpgradeProgress (PerformerTask task) {

			upgrading = true;

			while (upgrading) {
				pbar.SetProgress (task.Progress);
				yield return null;
			}
		}
	}
}