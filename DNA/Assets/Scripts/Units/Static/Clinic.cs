using UnityEngine;
using System.Collections;
using DNA.Tasks;
using InventorySystem;

namespace DNA.Units {

	public class Clinic : StaticUnit, ITaskPerformer {

		public override string Name {
			get { return "Clinic"; }
		}

		public override string Description {
			get { return "Elders live longer when they're receiving care at a Clinic."; }
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

		HealthIndicator indicator;

		void Awake () {
			unitRenderer.SetColors (new Color (1f, 0.898f, 0.231f));
		}

		protected override void OnInitInventory (Inventory i) {
			i.Add (new HealthGroup (200, 200));
		}

		protected override void OnInitAcceptableTasks (AcceptableTasks a) {
			a.Add (new AcceptCollectItem<HealthGroup> ());
		}

		protected void OnInitPerformableTasks (PerformableTasks p) {
			p.Add (new GenerateItem<HealthGroup> ());
		}

		protected override void OnEnable () {
			//healthHolder.HolderUpdated += OnHealthUpdate;
			indicator = ObjectPool.Instantiate<HealthIndicator> ();
			indicator.Initialize (Transform, 1.5f);
		}

		//public override vod OnPoolDestroy () {
		protected override void OnDisable () {
			//healthHolder.HolderUpdated -= OnHealthUpdate;
			ObjectPool.Destroy<HealthIndicator> (indicator.MyTransform);
		}

		void OnHealthUpdate () {
			//TODO: should set indicator as listener on init (and set position & parent) --- basically move all this out of the unit
			//indicator.Fill = healthHolder.PercentFilled;
		}
	}
}