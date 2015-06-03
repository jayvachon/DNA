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

		Clinic boundClinic = null;

		void Awake () {

			Inventory = new Inventory (this);
			Inventory.Add (new YearHolder (500, 65));
			Inventory.Add (new HealthHolder (100, 100));
			Inventory.Get<YearHolder> ().DisplaySettings = new ItemHolderDisplaySettings (true, false);

			PerformableActions = new PerformableActions (this);
			PerformableActions.Add (new DeliverUnpairedItem<ElderHolder> ());
			PerformableActions.Add (new ConsumeHealth (healthManager));
			PerformableActions.Add (new GenerateItem<YearHolder> ());
			PerformableActions.Add (new OccupyUnit ());
			PerformableActions.SetActive ("OccupyUnit", false);
		}

		void Start () {
			Path.Active = false;
		}
		
		public override void OnPoolCreate () {
			Inventory.Get<YearHolder> ().Clear ();
			Inventory.AddItems<YearHolder> (65);
			Inventory.AddItems<HealthHolder> (100);
			Inventory.Get<HealthHolder> ().HolderEmptied += OnDie;
			PerformableActions.Start ("ConsumeHealth");
		}

		public override void OnPoolDestroy () {
			Inventory.Get<HealthHolder> ().HolderEmptied -= OnDie;
		}

		protected override void OnBind () {
			UnbindClinic ();
			Clinic clinic = BoundAcceptor as Clinic;
			if (clinic != null) {
				boundClinic = clinic;
				PerformableActions.SetActive ("DeliverElder", false);
					
				// a disgusting hack
				// this starts the "OccupyUnit" action (which causes the elders to circle the clinic)
				// but only if they were added to the clinic's inventory
				if (boundClinic.AcceptableActions.ActionEnabled ("DeliverElder")) {
					PerformableActions.SetActive ("OccupyUnit", true);
					IActionAcceptor boundAcceptor = BoundAcceptor;
					BoundAcceptor = null;
					OnBindActionable (boundAcceptor);
				}
			}
		}

		protected override void OnUnbind () {
			UnbindClinic ();
		}

		void UnbindClinic () {
			if (boundClinic != null) {
				boundClinic.Inventory.RemoveItem<ElderHolder> ();
				PerformableActions.SetActive ("DeliverElder", true);
				boundClinic = null;
			}
		}

		void OnDie () {
			PerformableActions.Stop ("ConsumeHealth");
			ChangeUnit<Elder, Corpse> ();
		}

		protected override void OnChangeUnit<U> (U u) {
			Corpse corpse = u as Corpse;
			corpse.Inventory.Transfer<YearHolder> (Inventory);
			UnbindClinic ();
		}
	}
}