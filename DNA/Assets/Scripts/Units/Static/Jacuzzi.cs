using UnityEngine;
using System.Collections;
using DNA.InventorySystem;

namespace DNA.Units {

	public class Jacuzzi : StaticUnit {

		public override string Name {
			get { return "Jacuzzi"; }
		}

		public override string Description {
			get { return "The Jacuzzi increases Laborer happiness so that they will work faster."; }
		}

		HappinessHolder happinessHolder = new HappinessHolder (50, 50);
		HappinessIndicator indicator;

		void Awake () {
			
			unitRenderer.SetColors (new Color (0f, 0.5f, 1f));

			Inventory = new Inventory (this);
			Inventory.Add (happinessHolder);

			/*AcceptableActions.Add (new AcceptCollectHappiness ());*/

			//PerformableActions.Add (new GenerateItem<HappinessHolder> ());
		}

		//public override void OnPoolCreate () {
		protected override void OnEnable () {
			happinessHolder.HolderUpdated += OnHappinessUpdate;
			indicator = ObjectPool.Instantiate<HappinessIndicator> ();
			indicator.Initialize (Transform, 1.5f);
			base.OnEnable ();
		}

		//public override void OnPoolDestroy () {
		protected override void OnDisable () {
			happinessHolder.HolderUpdated -= OnHappinessUpdate;
			ObjectPool.Destroy<HappinessIndicator> (indicator.MyTransform);
			base.OnDisable ();
		}

		void OnHappinessUpdate () {
			//TODO: should set indicator as listener on init (and set position & parent) --- basically move all this out of the unit
			indicator.Fill = happinessHolder.PercentFilled;
		}
	}
}