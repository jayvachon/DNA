using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

namespace Units {

	public class Clinic : StaticUnit, IActionPerformer {

		public override string Name {
			get { return "Clinic"; }
		}

		public override bool PathPointEnabled {
			get { return false; }
		}

		public PerformableActions PerformableActions { get; private set; }

		HealthHolder healthHolder = new HealthHolder (300, 300);
		HealthIndicator indicator;

		void Awake () {

			Inventory = new Inventory (this);
			Inventory.Add (healthHolder);

			AcceptableActions = new AcceptableActions (this);
			AcceptableActions.Add (new AcceptCollectItem<HealthHolder> ());

			PerformableActions = new PerformableActions (this);
			PerformableActions.Add (new GenerateItem<HealthHolder> ());
		}

		public override void OnPoolCreate () {
			healthHolder.HolderUpdated += OnHealthUpdate;
			indicator = ObjectCreator.Instance.Create<HealthIndicator> ().GetScript<HealthIndicator> ();
			indicator.Initialize (Transform, 1.5f);
		}

		public override void OnPoolDestroy () {
			healthHolder.HolderUpdated -= OnHealthUpdate;
			ObjectCreator.Instance.Destroy<HealthIndicator> (indicator.MyTransform);
		}

		void OnHealthUpdate () {
			//TODO: should set indicator as listener on init (and set position & parent) --- basically move all this out of the unit
			indicator.Fill = healthHolder.PercentFilled;
		}
	}
}