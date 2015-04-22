using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;
using GameInput;

namespace Units {

	public class Corpse : MobileUnit, IActionPerformer {

		public override string Name { get { return "Remains"; } }

		void Awake () {

			Inventory = new Inventory (this);
			Inventory.Add (new YearHolder (500, 0));
			Inventory.Get<YearHolder> ().HolderEmptied += OnDeliverYears;

			PerformableActions = new PerformableActions (this);
			PerformableActions.Add ("DeliverYear", new DeliverItem<YearHolder> (0));
		}

		void Start () {
			Path.Active = false;
		}

		public override void OnRelease () {
			UnitClickable clickable = MobileClickable.Colliding (1 << (int)InputLayer.StaticUnits).GetScript<UnitClickable> ();
			if (clickable != null) {
				OnBindActionable (clickable.StaticUnit as IActionAcceptor);
				// TODO: Check if corpse was dropped on giving tree
				// if it was, check if it's delivering years and if so set UnitClickable CanDrag = false and CanSelect = false
			}
		}

		void OnDeliverYears () {
			ObjectCreator.Instance.Destroy<Corpse> (transform);
		}
	}
}