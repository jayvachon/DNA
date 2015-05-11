using UnityEngine;
using System.Collections;
using GameActions;
using Units;

namespace GameInventory {

	// TODO: Delete this item
	public class ElderItem : Item, IActionPerformer {

		class Settings {
			public bool CanBecomeSickWhileMoving = false;
		}
		Settings settings = new Settings ();

		float health = 1f;
		public float Health {
			get { return HealthManager.Health; }
		}

		HealthManager healthManager = new HealthManager ();
		public HealthManager HealthManager {
			get { return healthManager; }
		}

		public PerformableActions PerformableActions { get; private set; }

		public ElderItem () {
			HealthManager.onDie += OnDie;
			PerformableActions = new PerformableActions (this);
			PerformableActions.Add (new SubtractHealth ());
		}

		public override void OnAdd () {
			if (settings.CanBecomeSickWhileMoving) return;
			if (Inventory == null) {
				// This is null if the ItemHolder adds items in its constructor (because Inventory hasn't been assigned to the Item yet)
				// there probably exists a better way of handling this, but it's fine for now i think?
				return;
			}
			if (Inventory.holder is MobileUnit) {
				HealthManager.CanBecomeSick = false;
			} else if (Inventory.holder is StaticUnit) {
				HealthManager.CanBecomeSick = true;
			}
		}

		void OnDie () {
			Debug.Log ("die");
			Holder.Remove (this);
		}

		public override void Print () {
			Debug.Log ("Elder Health: " + health);
		}
	}
}