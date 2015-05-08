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

		OccupyBed occupyBed = new OccupyBed ();

		void Awake () {

			Inventory = new Inventory (this);
			Inventory.Add (new YearHolder (500, 65));
			Inventory.Add (new HealthHolder (100, 100));
			Inventory.Get<YearHolder> ().DisplaySettings = new ItemHolderDisplaySettings (true, false);

			PerformableActions = new PerformableActions (this);
			PerformableActions.Add ("OccupyBed", occupyBed);
			PerformableActions.Add ("ConsumeHealth", new ConsumeHealth (healthManager));
			PerformableActions.Add ("GenerateYear", new GenerateItem<YearHolder> ());
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

		public override void OnRelease () {
			PerformableActions.Enable ("OccupyBed");
			UnitClickable clickable = MobileClickable.Colliding (1 << (int)InputLayer.StaticUnits).GetScript<UnitClickable> ();
			if (clickable != null) {
				OnBindActionable (clickable.StaticUnit as IActionAcceptor);
			} else {
				occupyBed.Remove ();
			}
		}

		void OnDie () {
			PerformableActions.Stop ("ConsumeHealth");
			ChangeUnit<Elder, Corpse> ();
		}

		protected override void OnChangeUnit<U> (U u) {
			Corpse corpse = u as Corpse;
			corpse.Inventory.Transfer<YearHolder> (Inventory);
			if (occupyBed.Occupying) {
				Clinic clinic = occupyBed.Clinic;
				occupyBed.Remove ();
				corpse.OnBindActionable (clinic);
			}
		}
	}
}