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

		void Awake () {

			Inventory = new Inventory (this);
			Inventory.Add (new HealthHolder (100, 100));

			PerformableActions = new PerformableActions (this);
			occupyBed = new OccupyBed ();
			PerformableActions.Add ("OccupyBed", occupyBed);
			PerformableActions.Add ("ConsumeHealth", new ConsumeHealth (healthManager));
		}

		void Start () {
			Path.Active = false;
		}

		public override void OnRelease () {
			UnitClickable clickable = MobileClickable.Colliding (1 << (int)InputLayer.StaticUnits).GetScript<UnitClickable> ();
			if (clickable != null) {
				OnBindActionable (clickable.StaticUnit as IActionAcceptor);
			} else {
				occupyBed.Remove ();
			}
		}
	}
}