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

		void Awake () {

			Inventory = new Inventory (this);
			Inventory.Add (new YearHolder (500, 65));
			Inventory.Add (new HealthHolder (100, 100));
			Inventory.Get<YearHolder> ().DisplaySettings = new ItemHolderDisplaySettings (true, false);
			Inventory.Get<HealthHolder> ().DisplaySettings = new ItemHolderDisplaySettings (true, true);

			PerformableActions = new PerformableActions (this);
			PerformableActions.Add (new ConsumeItem<HealthHolder> ()); // new
			PerformableActions.Add (new CollectHealth ()); // new

			PerformableActions.Add (new GenerateItem<YearHolder> ());
			PerformableActions.Add (new OccupyUnit ());
			PerformableActions.SetActive ("OccupyUnit", false);
		}

		void Start () {
			Path.Active = false;
			Path.Speed = Path.PathSettings.MaxSpeed / TimerValues.Instance.Year;
		}
		
		public override void OnPoolCreate () {
			InitInventory ();
			InitIndicator ();
			PerformableActions.Start ("ConsumeHealth");
		}

		void InitInventory () {
			Inventory.Get<YearHolder> ().Clear ();
			Inventory.AddItems<YearHolder> (65);
			Inventory.AddItems<HealthHolder> (100);
			Inventory.Get<HealthHolder> ().HolderEmptied += OnDie;
		}

		void InitIndicator () {
			indicator = ObjectCreator.Instance.Create<HealthIndicator> ().GetScript<HealthIndicator> ();
			indicator.Initialize (Transform);
		}

		public override void OnPoolDestroy () {
			Inventory.Get<HealthHolder> ().HolderEmptied -= OnDie;
		}

		protected override void OnBind () {
			PerformableActions.SetActive ("OccupyUnit", true);
			IActionAcceptor boundAcceptor = BoundAcceptor;
			BoundAcceptor = null;
			OnBindActionable (boundAcceptor);
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