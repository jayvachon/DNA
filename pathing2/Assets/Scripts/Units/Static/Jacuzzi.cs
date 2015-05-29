using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

namespace Units {

	public class Jacuzzi : StaticUnit, IActionPerformer {

		public override string Name {
			get { return "Jacuzzi"; }
		}

		public override bool PathPointEnabled {
			get { return false; }
		}
		
		public PerformableActions PerformableActions { get; private set; }

		HappinessHolder happinessHolder = new HappinessHolder (100, 100);
		HappinessIndicator indicator;

		void Awake () {
			
			Inventory = new Inventory (this);
			Inventory.Add (happinessHolder);
			Inventory.Add (new DistributorHolder (3, 0));
			Inventory.Get<DistributorHolder> ().DisplaySettings = new ItemHolderDisplaySettings (true, true);

			AcceptableActions = new AcceptableActions (this);
			AcceptableActions.Add (new AcceptCollectItem<HappinessHolder> ());
			AcceptableActions.Add (new AcceptDeliverUnpairedItem<DistributorHolder> ());

			PerformableActions = new PerformableActions (this);
			PerformableActions.Add (new GenerateItem<HappinessHolder> ());
		}

		public override void OnPoolCreate () {
			happinessHolder.HolderUpdated += OnHappinessUpdate;
			indicator = ObjectCreator.Instance.Create<HappinessIndicator> ().GetScript<HappinessIndicator> ();
			indicator.Parent = Transform;
			indicator.MyTransform.SetLocalPosition (new Vector3 (0f, 1.5f, 0f));
		}

		public override void OnPoolDestroy () {
			happinessHolder.HolderUpdated -= OnHappinessUpdate;
			ObjectCreator.Instance.Destroy<HappinessIndicator> (indicator.MyTransform);
		}

		void OnHappinessUpdate () {
			//TODO: should set indicator as listener on init (and set position & parent) --- basically move all this out of the unit
			indicator.Fill = happinessHolder.PercentFilled;
		}
	}
}