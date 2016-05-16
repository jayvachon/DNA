using UnityEngine;
using System.Collections;
using DNA.Tasks;
using InventorySystem;

namespace DNA.Units {

	public class Clinic : StaticUnit {

		HealthIndicator indicator;

		protected override void OnInitInventory (Inventory i) {
			i.Add (new HealthGroup (200, 200));
		}

		protected override void OnInitAcceptableTasks (AcceptableTasks a) {
			a.Add (new AcceptCollectItem<HealthGroup> ());
		}

		protected override void OnInitPerformableTasks (PerformableTasks p) {
			p.Add (new GenerateItem<HealthGroup> ());
		}

		protected override void OnEnable () {
			base.OnEnable ();
			//healthHolder.HolderUpdated += OnHealthUpdate;
			indicator = ObjectPool.Instantiate<HealthIndicator> ();
			indicator.Initialize (Transform, 1.5f);
		}

		//public override vod OnPoolDestroy () {
		protected override void OnDisable () {
			base.OnDisable ();
			//healthHolder.HolderUpdated -= OnHealthUpdate;
			ObjectPool.Destroy<HealthIndicator> (indicator.MyTransform);
		}

		void OnHealthUpdate () {
			//TODO: should set indicator as listener on init (and set position & parent) --- basically move all this out of the unit
			//indicator.Fill = healthHolder.PercentFilled;
		}
	}
}