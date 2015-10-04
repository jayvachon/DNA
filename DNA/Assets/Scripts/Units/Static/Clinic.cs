using UnityEngine;
using System.Collections;
using DNA.InventorySystem;

namespace DNA.Units {

	public class Clinic : StaticUnit {

		public override string Name {
			get { return "Clinic"; }
		}

		public override string Description {
			get { return "Elders live longer when they're receiving care at a Clinic."; }
		}

		public override bool PathPointEnabled {
			get { return false; }
		}

		HealthHolder healthHolder = new HealthHolder (300, 300);
		HealthIndicator indicator;

		void Awake () {

			Inventory = new Inventory (this);
			Inventory.Add (healthHolder);

			//AcceptableActions.Add (new AcceptCollectItem<HealthHolder> ());

			/*PerformableActions = new PerformableActions (this);
			PerformableActions.Add (new GenerateItem<HealthHolder> ());*/
		}

		//public override void OnPoolCreate () {
		protected override void OnEnable () {
			healthHolder.HolderUpdated += OnHealthUpdate;
			indicator = ObjectPool.Instantiate<HealthIndicator> ();
			indicator.Initialize (Transform, 1.5f);
		}

		//public override void OnPoolDestroy () {
		protected override void OnDisable () {
			healthHolder.HolderUpdated -= OnHealthUpdate;
			ObjectPool.Destroy<HealthIndicator> (indicator.MyTransform);
		}

		void OnHealthUpdate () {
			//TODO: should set indicator as listener on init (and set position & parent) --- basically move all this out of the unit
			indicator.Fill = healthHolder.PercentFilled;
		}
	}
}