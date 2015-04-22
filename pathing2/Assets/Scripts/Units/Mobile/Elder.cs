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

		OccupyBed occupyBed;
		bool dead = false;

		void Awake () {

			Inventory = new Inventory (this);
			Inventory.Add (new YearHolder (500, 65));
			Inventory.Add (new HealthHolder (100, 100));
			Inventory.Get<HealthHolder> ().HolderEmptied += OnDie;

			PerformableActions = new PerformableActions (this);
			occupyBed = new OccupyBed ();
			PerformableActions.Add ("OccupyBed", occupyBed);
			PerformableActions.Add ("ConsumeHealth", new ConsumeHealth (healthManager));
			PerformableActions.Add ("GenerateYear", new GenerateItem<YearHolder> (TimerValues.Retirement / 65f));
		}

		void Start () {
			Path.Active = false;
		}
		
		public override void OnPoolCreate () {
			Inventory.Get<YearHolder> ().Clear ();
			Inventory.AddItems<YearHolder> (65);
			Inventory.AddItems<HealthHolder> (100);
			PerformableActions.Start ("ConsumeHealth");
			dead = false;
		}

		public override void OnRelease () {
			UnitClickable clickable = MobileClickable.Colliding (1 << (int)InputLayer.StaticUnits).GetScript<UnitClickable> ();
			if (clickable != null) {
				OnBindActionable (clickable.StaticUnit as IActionAcceptor);
				// TODO: Check if elder was dropped on giving tree
				// if it was, check if it's delivering years and if so set UnitClickable CanDrag = false and CanSelect = false
			} else {
				occupyBed.Remove ();
			}
		}

		void OnDie () {
			if (dead) return; // TODO: this is a problem w/ the HolderEmptied callback
			dead = true;
			PerformableActions.Stop ("ConsumeHealth");
			occupyBed.Remove (); // TODO: Transfer occuppied bed to corpse
			// maybe this ^ is an opportunity to measure respect for elders/spirits: when an elder dies, does 
			// the player immediately deliver its remains to the giving tree, or let them sit in a bed?
			ChangeUnit<Elder, Corpse> ();
		}

		protected override void OnChangeUnit<Corpse> (Corpse corpse) {
			corpse.Inventory.Transfer<YearHolder> (Inventory);
		}
	}
}