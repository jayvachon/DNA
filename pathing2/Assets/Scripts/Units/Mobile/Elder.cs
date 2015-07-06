using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;
using GameInput;

namespace Units {

	public class Elder : MobileUnit, IActionPerformer {

		public override string Name { get { return "Elder"; } }

		HealthManager2 healthManager = new HealthManager2 ();
		public HealthManager2 HealthManager {
			get { return healthManager; }
		}

		public int AverageHappiness { 
			set { Inventory.RemoveItems<HealthHolder> (100 - value); }
		}

		HealthIndicator indicator;
		HealthHolder healthHolder;

		void Awake () {

			Inventory = new Inventory (this);
			Inventory.Add (new YearHolder (500, 65));
			healthHolder = (HealthHolder)Inventory.Add (new HealthHolder (100, 100));
			Inventory.Get<YearHolder> ().DisplaySettings = new ItemHolderDisplaySettings (true, false);
			healthHolder.DisplaySettings = new ItemHolderDisplaySettings (true, true);

			PerformableActions.Add (new ConsumeItem<HealthHolder> ());
			PerformableActions.Add (new CollectHealth ());

			PerformableActions.Add (new GenerateItem<YearHolder> ());
			PerformableActions.Add (new OccupyUnit ());
			PerformableActions.SetActive ("OccupyUnit", false);
		}

		void Start () {
			Path.Active = false;
			Path.Speed = Path.PathSettings.MinSpeed / TimerValues.Instance.Year;
		}
		
		public override void OnPoolCreate () {
			InitInventory ();
			InitIndicator ();
			PerformableActions.Start ("ConsumeHealth");
			NotificationCenter.Instance.ShowNotification ("laborerRetired");
		}

		void InitInventory () {
			Inventory.Get<YearHolder> ().Clear ();
			Inventory.AddItems<YearHolder> (65);
			Inventory.AddItems<HealthHolder> (100);
			healthHolder.HolderUpdated += OnHealthUpdate;
			healthHolder.HolderEmptied += OnDie;
		}

		void InitIndicator () {
			indicator = ObjectCreator.Instance.Create<HealthIndicator> ().GetScript<HealthIndicator> ();
			indicator.Initialize (Transform);
		}

		public override void OnPoolDestroy () {
			healthHolder.HolderEmptied -= OnDie;
			ObjectCreator.Instance.Destroy<HealthIndicator> (indicator.MyTransform);
			indicator = null;
		}

		// TODO: remove this?
		protected override void OnBind () {
			PerformableActions.SetActive ("OccupyUnit", true);
			IActionAcceptor boundAcceptor = BoundAcceptor;
			BoundAcceptor = null;
			//OnBindActionable (boundAcceptor);
		}

		void OnHealthUpdate () {
			if (indicator != null) indicator.Fill = healthHolder.PercentFilled;
		}

		void OnDie () {
			PerformableActions.Stop ("ConsumeHealth");
			ChangeUnit<Elder, Corpse> ();
		}

		protected override void OnChangeUnit<U> (U u) {
			Corpse corpse = u as Corpse;
			corpse.Inventory.Transfer<YearHolder> (Inventory);
		}
	}
}