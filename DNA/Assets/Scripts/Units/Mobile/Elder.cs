using UnityEngine;
using System.Collections;
//using DNA.InventorySystem;
using DNA.InputSystem;
using InventorySystem;

namespace DNA.Units {

	public class Elder : MobileUnit {

		public override string Name { get { return "Elder"; } }

		public override string Description { get { return "Elders need to be cared for."; } }

		HealthManager2 healthManager = new HealthManager2 ();
		public HealthManager2 HealthManager {
			get { return healthManager; }
		}

		/*public int AverageHappiness { 
			set { Inventory.RemoveItems<HealthHolder> (100 - value); }
		}*/

		HealthIndicator indicator;
		//HealthHolder healthHolder;
		//HealthGroup healthGroup;

		void Awake () {

			unitRenderer.SetColors (new Color (0.447f, 0.251f, 0.447f));

			Inventory = new Inventory (this);
			//Inventory.Add (new YearHolder (500, 65));
			Inventory.Add (new YearGroup (65, 500));
			//healthHolder = (HealthHolder)Inventory.Add (new HealthHolder (100, 100));
			Inventory.Add (new HealthGroup (100, 100));
			//Inventory.Get<YearHolder> ().DisplaySettings = new ItemHolderDisplaySettings (true, false);
			//healthHolder.DisplaySettings = new ItemHolderDisplaySettings (true, true);

			/*PerformableActions.Add (new ConsumeItem<HealthHolder> ());
			PerformableActions.Add (new CollectHealth ());

			PerformableActions.Add (new GenerateItem<YearHolder> ());
			PerformableActions.Add (new OccupyUnit ());
			PerformableActions.SetActive ("OccupyUnit", false);*/
		}

		void Start () {
			//Path.Active = false;
			//Path.Speed = Path.PathSettings.MaxSpeed / TimerValues.Instance.Year;
		}
		
		//public override void OnPoolCreate () {
		protected override void OnEnable () {
			InitInventory ();
			InitIndicator ();
			//PerformableActions.Start ("ConsumeHealth");
			NotificationCenter.Instance.ShowNotification ("laborerRetired");
			base.OnEnable ();
		}

		void InitInventory () {
			/*Inventory.Get<YearHolder> ().Clear ();
			Inventory.AddItems<YearHolder> (65);
			Inventory.AddItems<HealthHolder> (100);
			healthHolder.HolderUpdated += OnHealthUpdate;
			healthHolder.HolderEmptied += OnDie;*/
			Inventory["Years"].Clear ();
			Inventory["Years"].Set (65);
			Inventory["Health"].Set (100);
			Inventory["Health"].onUpdate += OnHealthUpdate;
			Inventory["Health"].onEmpty += OnDie;
		}

		void InitIndicator () {
			indicator = ObjectPool.Instantiate<HealthIndicator> ();
			indicator.Initialize (Transform);
		}

		//public override void OnPoolDestroy () {
		protected override void OnDisable () {
			//healthHolder.HolderEmptied -= OnDie;
			Inventory["Health"].onEmpty -= OnDie;
			ObjectPool.Destroy<HealthIndicator> (indicator.MyTransform);
			indicator = null;
			base.OnDisable ();
		}

		// TODO: remove this?
		/*protected override void OnBind () {
			PerformableActions.SetActive ("OccupyUnit", true);
			IActionAcceptor boundAcceptor = BoundAcceptor;
			BoundAcceptor = null;
			//OnBindActionable (boundAcceptor);
		}*/

		void OnHealthUpdate () {
			//if (indicator != null) indicator.Fill = healthHolder.PercentFilled;
		}

		void OnDie () {
			//PerformableActions.Stop ("ConsumeHealth");
			ChangeUnit<Elder, Corpse> ();
		}

		protected override void OnChangeUnit<U> (U u) {
			Corpse corpse = u as Corpse;
			//corpse.Inventory.Transfer<YearHolder> (Inventory);
			Inventory.Get<YearGroup> ().Transfer (corpse.Inventory.Get<YearGroup> ());
		}
	}
}